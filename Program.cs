using System.Diagnostics;
using CFI.Localization;
using CFI.Models;
using CFI.YamlHelpers;

const string VERSION = "0.1.0";
const string HELP = @"
    Usage: cfi [options]

    Options:
        -p, --pkgmeta <path>    Use path as the pkgmeta file, otherwise it will look for a .pkgmeta file in the current directory
        -l, --links             Use symbolic links (junctions on Windows) instead of copying (ignores 'ignored' files, be careful)
        -e, --external <dir>    Use directory for cloning external repo (default: ./.externals)
        -a, --addon <dir>       Use directory for packaging the addon (default: ./.addon)
        -w, --wow <dir>         Use directory as the World of Warcraft addons folder, instead of WOW_ADDONS env var, to install the addon
        -v, --version           Show version
        -q, --quiet             Don't show any output, except errors
        -h, --help              Show help";

string cd = Environment.CurrentDirectory;
string? pkgmetaPath = null;
bool useLinks = false;
string externalDir = "./.externals";
string addonDir = "./.addon";
string? wowDir = Environment.GetEnvironmentVariable("WOW_ADDONS");
bool quiet = false;


for (int i = 0; i < args.Length; i += 1)
{
    string arg = args[i];
    string? nextArg = i + 1 < args.Length ? args[i + 1] : null;

    switch (arg)
    {
        case "-p":
        case "--pkgmeta":
            pkgmetaPath = nextArg;
            break;
        case "-l":
        case "--links":
            useLinks = true;
            break;
        case "-e":
        case "--external":
            externalDir = nextArg ?? externalDir;
            break;
        case "-a":
        case "--addon":
            addonDir = nextArg ?? addonDir;
            break;
        case "-w":
        case "--wow":
            wowDir = nextArg;
            break;
        case "-q":
        case "--quiet":
            quiet = true;
            break;
        case "-h":
        case "--help":
            Console.WriteLine(HELP);
            Environment.Exit(0);
            break;
        case "-v":
        case "--version":
            Console.WriteLine(VERSION);
            Environment.Exit(0);
            break;
    }
}

// Setup the logger (Serilog)
LoggerConfiguration _config = new LoggerConfiguration()
    .WriteTo.Console(
        theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code,
        outputTemplate: "{Level:u4}: {Message:lj}{NewLine}{Exception}");
if (quiet) _config.MinimumLevel.Error();
Log.Logger = _config.CreateLogger();

// Find the pkgmeta file if not provided as an argument
pkgmetaPath ??= Directory.GetFiles(cd, "*.pkgmeta").FirstOrDefault();

// Check if the pkgmeta file exists
if (pkgmetaPath is null || string.IsNullOrEmpty(pkgmetaPath) || !File.Exists(pkgmetaPath))
{
    Log.Fatal(Localizations.Current.FATAL_COULDNT_FIND_PKGMETA);
    Environment.Exit(1);
}

if (wowDir is null || string.IsNullOrEmpty(wowDir))
{
    Log.Error(Localizations.Current.ERROR_WOW_ADDON_DIRECTORY_NOT_FOUND);
}

// Create a deserializer with the static context/code gen (AoT support)
var deserializer = new StaticDeserializerBuilder(YamlStaticContext.Instance)
    .WithTypeConverter(PackageMetadataTypesConverter.Instance)
    .Build();

// Open the pkgmeta file and deserialize it
using TextReader reader = File.OpenText(pkgmetaPath!);
PackageMetadata model = deserializer.Deserialize<PackageMetadata>(reader);

// Manually add .files to ignore
if (model.Ignore is null)
    model.Ignore = [ ".*" ];
else
    model.Ignore = [.. model.Ignore, ".*"];


Log.Information(Localizations.Current.INFO_LOADED_PKGMETA, model.PackageAs);

// clone the repos into the .externals directory
if (!Directory.Exists(externalDir))
{
    Log.Information(Localizations.Current.INFO_CREATING_EXTERNAL_REPO_DIRECTORY, externalDir);
    _ = Directory.CreateDirectory(externalDir);
}
foreach (var external in model.Externals ?? [])
    await RepoManager.Clone(external.Value, externalDir);

// package addon into .addon directory
if (!Directory.Exists(addonDir))
{
    Log.Information(Localizations.Current.INFO_CREATING_PACKAGED_ADDON_DIRECTORY, addonDir);
    Directory.CreateDirectory(addonDir);
}
foreach (var external in model.Externals ?? [])
{
    ExternalRepo repo = external.Value;

    string externalPath = Path.GetFullPath($"{externalDir}/{repo.ProjectName}");
    string addonPath = Path.GetFullPath($"{addonDir}/{external.Key}");

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

        if (useLinks)
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
                RepoManager.CopyDirectoryWithIgnore(cd, externalPath, addonPath, model.Ignore);
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
    string destination = Path.GetFullPath($"{addonDir}/{path}");

    if (useLinks)
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
            RepoManager.CopyDirectoryWithIgnore(cd, source, destination, model.Ignore);
        }
        catch (Exception ex)
        {
            Log.Error(ex, Localizations.Current.ERROR_COPY_FOLDER_FAILED, source, destination);
            continue;
        }

        Log.Information(Localizations.Current.INFO_COPIED_FOLDER, source, destination);
    }
}

// Tell the user the final commands they need to run
Console.WriteLine($"\nRun the following commands to complete the move:");
Console.WriteLine("(Replace WOW_ADDONS with the path to your World of Warcraft addons folder)\n");
foreach (var move in model.MoveFolders ?? [])
{
    Console.WriteLine($"    ln -s {cd}/.addon/{move.Key} WOW_ADDONS/{move.Value}");
}

Console.WriteLine("\nFinished");
