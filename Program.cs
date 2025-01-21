using System.Diagnostics;
using System.Text;
using CFI.Localization;
using CFI.Models;
using CFI.YamlHelpers;

// Create a cancel token and connect it to ctrl+c
CancellationTokenSource cts = new();
Console.CancelKeyPress += (s, e) => {
    e.Cancel = true;
    cts.Cancel();
};


const string VERSION = "0.2.0";

// Check if the user wants to see the help or version
if (args.Contains("-h") || args.Contains("--help"))
{
    Console.WriteLine(Localizations.Current.HELP);
    Environment.Exit(0);
}
if (args.Contains("-v") || args.Contains("--version"))
{
    Console.WriteLine(VERSION);
    Environment.Exit(0);
}

// Parse the arguments to build our config
Config config = Config.FromParams(args);

// Setup the logger (Serilog)
LoggerConfiguration _config = new LoggerConfiguration()
    .WriteTo.Console(
        theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code,
        outputTemplate: "{Level:u4}: {Message:lj}{NewLine}{Exception}");
if (config.Quiet) _config.MinimumLevel.Error();
Log.Logger = _config.CreateLogger();

// Find the pkgmeta file if not provided as an argument
config.PackageMetaPath ??= Directory.GetFiles(Environment.CurrentDirectory, "*.pkgmeta").FirstOrDefault();

// Check if the pkgmeta file exists
if (config.PackageMetaPath is null || string.IsNullOrEmpty(config.PackageMetaPath) || !File.Exists(config.PackageMetaPath))
{
    Log.Fatal(Localizations.Current.FATAL_COULDNT_FIND_PKGMETA);
    Environment.Exit(1);
}

// Check to see if the WoW directory is valid, if not, alert the user we won't install
if (!config.IsWoWDirValid)
    Log.Error(Localizations.Current.ERROR_WOW_ADDON_DIRECTORY_NOT_FOUND);

// Create a deserializer with the static context/code gen (AoT support)
var deserializer = new StaticDeserializerBuilder(YamlStaticContext.Instance)
    .WithTypeConverter(PackageMetadataTypesConverter.Instance)
    .Build();

// Open the pkgmeta file and deserialize it
using TextReader reader = File.OpenText(config.PackageMetaPath!);
PackageMetadata model = deserializer.Deserialize<PackageMetadata>(reader);

// Manually add .files to ignore
if (model.Ignore is null)
    model.Ignore = [ ".*" ];
else
    model.Ignore = [.. model.Ignore, ".*"];

// Print the package name
Log.Information(Localizations.Current.INFO_LOADED_PKGMETA, model.PackageAs);

// clone the repos into the .externals directory
if (!Directory.Exists(config.ExternalDir))
{
    Log.Information(Localizations.Current.INFO_CREATING_EXTERNAL_REPO_DIRECTORY, config.ExternalDir);
    _ = Directory.CreateDirectory(config.ExternalDir);
}


StringBuilder changelog = new();

foreach (var external in model.Externals ?? [])
{
    ExternalRepo repo = external.Value;
    repo.ProjectName = Utilities.AttemptGetPackageName(external.Key);
    string externalPath = Path.GetFullPath($"{config.ExternalDir}/{repo.ProjectName}");

    // Check if we have already cloned this repo
    if (Directory.Exists(externalPath))
    {
        try
        {
            // Use local methods to check if the repo is a git or svn repo
            repo.RepoType = await Commands.CheckLocalUriRepoTypeAsync(externalPath, cts.Token);

            // Save the current head (oldHead)
            string oldHead = await Commands.HeadHashRepoAsync(repo.RepoType, externalPath, cts.Token);

            // Pull the repo
            await Commands.PullRepoAsync(repo.RepoType, externalPath, cts.Token);

            // Save the new head (newHead)
            string newHead = await Commands.HeadHashRepoAsync(repo.RepoType, externalPath, cts.Token);

            // If the repo has changed, add the changes to the changelog
            if (oldHead != newHead)
            {
                changelog.AppendLine($"  - {repo.ProjectName} updated from {oldHead} to {newHead}");
                // Gets all the commits between the two hashes, we need to split it by new line and add to our changelog
                foreach(string line in (await Commands.LogRepoAsync(repo.RepoType, oldHead, newHead, externalPath, cts.Token)).Split(Environment.NewLine))
                    changelog.AppendLine($"    {line}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, Localizations.Current.ERROR_FAILED_REPO_CLONE, repo.ProjectName);
        }
    }
    else
    {
        // Use remote methods to check if the repo is a git or svn repo
        repo.RepoType = await Commands.CheckRemoteUriRepoTypeAsync(repo.Url, cts.Token);

        // Clone the repo
        await Commands.CloneRepoAsync(repo.RepoType, repo.Url, externalPath, cts.Token);
    }
}




// package addon into .addon directory
if (!Directory.Exists(config.AddonDir))
{
    Log.Information(Localizations.Current.INFO_CREATING_PACKAGED_ADDON_DIRECTORY, config.AddonDir);
    Directory.CreateDirectory(config.AddonDir);
}
foreach (var external in model.Externals ?? [])
{
    ExternalRepo repo = external.Value;

    string externalPath = Path.GetFullPath($"{config.ExternalDir}/{repo.ProjectName}");
    string addonPath = Path.GetFullPath($"{config.AddonDir}/{external.Key}");

    if (Directory.Exists(addonPath))
    {
        Log.Warning(Localizations.Current.WARNING_FOLDER_ALREADY_EXISTS_SKIPPING, addonPath);
        continue;
    }
    else
    {
        // Don't create the last directory in the path
        string[] pathParts = addonPath.Split('/');
        string exceptLastDirectory = string.Join('/', pathParts.Take(pathParts.Length - 1));

        Directory.CreateDirectory(exceptLastDirectory);

        if (config.UseLinks)
        {
            ProcessStartInfo psi = new("ln", $"-s \"{externalPath}\" \"{addonPath}\"")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process p = Process.Start(psi)!;
            p.WaitForExit();

            if (p.ExitCode != 0)
                Log.Error(Localizations.Current.ERROR_LINK_FOLDER_FAILED, externalPath, addonPath);
            else
                Log.Information(Localizations.Current.INFO_LINKED_FOLDER, externalPath, addonPath);
        }
        else
        {
            try
            {
                Utilities.CopyDirectoryWithIgnore(Environment.CurrentDirectory, externalPath, addonPath, model.Ignore);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Localizations.Current.ERROR_COPY_FOLDER_FAILED, externalPath, addonPath);
                continue;
            }

            Log.Information(Localizations.Current.INFO_COPIED_FOLDER, externalPath, addonPath);
        }
    }
}

// link the local directory files to the .addon directory (better than copy)
foreach (string path in model.PlainCopy ?? [])
{
    // GetFullPath resolves children directories as parents if they have the same name, thus we need to use the current directory
    string source = Path.GetFullPath($"./{path}");
    string destination = Path.GetFullPath($"{config.AddonDir}/{path}");

    if (config.UseLinks)
    {
        if (Directory.Exists(destination))
        {
            Console.WriteLine($"Folder '{destination}' already exists, skipping...");
            continue;
        }
        else
        {
            ProcessStartInfo psi = new("ln", $"-s {source} {destination}")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process p = Process.Start(psi)!;
            p.WaitForExit();

            if (p.ExitCode != 0)
                Log.Error(Localizations.Current.ERROR_LINK_FOLDER_FAILED, source, destination);
            else
                Log.Information(Localizations.Current.INFO_LINKED_FOLDER, source, destination);
        }
    }
    else
    {
        try
        {
            Utilities.CopyDirectoryWithIgnore(Environment.CurrentDirectory, source, destination, model.Ignore);
        }
        catch (Exception ex)
        {
            Log.Error(ex, Localizations.Current.ERROR_COPY_FOLDER_FAILED, source, destination);
            continue;
        }

        Log.Information(Localizations.Current.INFO_COPIED_FOLDER, source, destination);
    }
}

if (config.IsWoWDirValid)
{
    // TODO: Install the addon...

    Console.WriteLine("This is suppose to install the addon, but my code isn't ready yet. You can manually run the following commands to complete the move:");

    // Tell the user the final commands they need to run
    Console.WriteLine($"\nRun the following commands to complete the move:");
    Console.WriteLine("(Replace WOW_ADDONS with the path to your World of Warcraft addons folder)\n");
    foreach (var move in model.MoveFolders ?? [])
    {
        Console.WriteLine($"    ln -s {Environment.CurrentDirectory}/.addon/{move.Key} YOUR_WOW_ADDONS/{move.Value}");
    }
}
else
{
    // Tell the user the final commands they need to run
    Console.WriteLine($"\nRun the following commands to complete the move:");
    Console.WriteLine("(Replace WOW_ADDONS with the path to your World of Warcraft addons folder)\n");
    foreach (var move in model.MoveFolders ?? [])
    {
        Console.WriteLine($"    ln -s {Environment.CurrentDirectory}/.addon/{move.Key} YOUR_WOW_ADDONS/{move.Value}");
    }
}

Console.WriteLine("\nFinished");
