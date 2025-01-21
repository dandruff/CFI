using System.Diagnostics;
using System.Text.RegularExpressions;
using CFI.Localization;
using CFI.Models;

namespace CFI;

public static class Utilities
{
    // A pretty basic method that tries to get the name from the URL
    public static string AttemptGetPackageName(string url)
    {
        string[] parts = url.Replace("/trunk", "").Split('/');
        string name = parts[^1].Replace(".git", "").Replace(".svn", "");
        return name;
    }

    public static Task<RepoTypes> GetRepoTypeAsync(string url) =>
        Task.Run(() =>
            {
                ProcessStartInfo gitInfo = new("git", $"ls-remote {url}")
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                };

                Process gitProcess = Process.Start(gitInfo)!;
                string gitOutput = gitProcess.StandardOutput.ReadToEnd();
                gitProcess.WaitForExit();

                // Assume git worked, will be set to a different value if it fails
                RepoTypes repoType = RepoTypes.Git;

                // If Git fails, try to use SVN to check the repository
                if (gitProcess.ExitCode != 0)
                {
                    ProcessStartInfo svnInfo = new("svn", $"info {url}")
                    {
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false
                    };

                    Process svnProcess = Process.Start(svnInfo)!;
                    string svnOutput = svnProcess.StandardOutput.ReadToEnd();
                    svnProcess.WaitForExit();

                    repoType = svnProcess.ExitCode == 0 ? RepoTypes.Svn : RepoTypes.Unknown;
                }

                return repoType;
            });

    private static Task<RepoTypes> CheckLocalRepoType(string directory)
    {
        ProcessStartInfo gitInfo = new("git", "rev-parse --is-inside-work-tree")
        {
            WorkingDirectory = directory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        Process gitProcess = Process.Start(gitInfo)!;
        string gitOutput = gitProcess.StandardOutput.ReadToEnd();
        gitProcess.WaitForExit();

        if (gitProcess.ExitCode == 0)
        {
            return Task.FromResult(RepoTypes.Git);
        }

        ProcessStartInfo svnInfo = new("svn", "info")
        {
            WorkingDirectory = directory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        Process svnProcess = Process.Start(svnInfo)!;
        string svnOutput = svnProcess.StandardOutput.ReadToEnd();
        svnProcess.WaitForExit();

        if (svnProcess.ExitCode == 0)
        {
            return Task.FromResult(RepoTypes.Svn);
        }

        return Task.FromResult(RepoTypes.Unknown);
    }

    public static async Task UpdateOrCloneAsync(ExternalRepo repo, string directory)
    {
        if (Directory.Exists(directory))
        {
            // Check if the directory is a Git repository with git rev-parse --is-inside-work-tree
            ProcessStartInfo gitInfo = new("git", "rev-parse --is-inside-work-tree")
            {
                WorkingDirectory = directory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            Process gitProcess = Process.Start(gitInfo)!;
            string gitOutput = gitProcess.StandardOutput.ReadToEnd();
            gitProcess.WaitForExit();

            if (gitProcess.ExitCode == 0)
            {
                // The directory is a Git repository, so we can use git pull
                ProcessStartInfo gitPullInfo = new("git", "pull")
                {
                    WorkingDirectory = directory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                };

                Process gitPullProcess = Process.Start(gitPullInfo)!;
                string gitPullOutput = gitPullProcess.StandardOutput.ReadToEnd();
                gitPullProcess.WaitForExit();

                if (gitPullProcess.ExitCode != 0)
                {
                    Log.Error(Localizations.Current.ERROR_FAILED_REPO_CLONE, repo.ProjectName);
                    Console.WriteLine($"\n{gitPullProcess.StandardError.ReadToEnd()}\n");
                }
                else
                {
                    Log.Information(Localizations.Current.INFO_SUCCESSFULLY_CLONED_REPO, repo.ProjectName);
                }
            }
            else
            {
                // The directory is not a Git repository, so we can't update it
                Log.Warning(Localizations.Current.WARNING_FOLDER_NOT_GIT_REPO_SKIPPING, repo.ProjectName);
            }
        }
        else
        {
            await CloneRepoAsync(repo, directory);
        }
    }

    public static async Task CloneRepoAsync(ExternalRepo repo, string directory)
    {
        repo.ProjectName = AttemptGetPackageName(repo.Url);
        string? commit = repo.Commit;
        string? tag = repo.Tag;

        string command;
        string executable;

        try
        {
            // repo.RepoType is a task that checks if the repo is a Git
            // or SVN repository using git ls-remote and svn info
            RepoTypes repoType = await repo.RepoType;

            if (repoType == RepoTypes.Git)
            {
                command = $"clone {repo.Url} {directory}/{repo.ProjectName} --depth 1";
                executable = "git";

                if (!string.IsNullOrEmpty(commit))
                    Log.Warning(Localizations.Current.WARNING_IGNORNING_UNSUPPORTED_REPO_PROPERTY, "externals->repo->commit");

                if (!string.IsNullOrEmpty(tag))
                    command += $" --branch {tag}";
            }
            else if (repoType == RepoTypes.Svn)
            {
                command = $"checkout {repo.Url} {directory}/{repo.ProjectName} --depth=empty";
                executable = "svn";

                if (!string.IsNullOrEmpty(commit))
                    Log.Warning(Localizations.Current.WARNING_IGNORNING_UNSUPPORTED_REPO_PROPERTY, "externals->repo->commit");

                if (!string.IsNullOrEmpty(tag))
                    Log.Warning(Localizations.Current.WARNING_IGNORNING_UNSUPPORTED_REPO_PROPERTY, "(for SVN) externals->repo->tag");
            }
            else
            {
                Log.Error(Localizations.Current.ERROR_SKIPPING_REPO, repo.Url);
                return;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, Localizations.Current.ERROR_FAILED_REPO_CLONE, repo.Url);
            return;
        }

        Log.Information(Localizations.Current.INFO_RUNNING_COMMAND, executable, command);
        
        ProcessStartInfo info = new(executable, command)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };
        Process process = Process.Start(info)!;
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            Log.Error(Localizations.Current.ERROR_FAILED_REPO_CLONE, repo.Url);
            Console.WriteLine($"\n{process.StandardError.ReadToEnd()}\n");
        }
        else
        {
            Log.Information(Localizations.Current.INFO_SUCCESSFULLY_CLONED_REPO, repo.ProjectName);
        }
    }


    /// <summary>
    /// Converts a glob-style wildcard pattern (e.g. "*.md") to an equivalent regex pattern.
    /// For example: 
    ///   "*.md"      -> "^.*\\.md$"
    ///   "Makefile"  -> "^Makefile$"
    /// </summary>
    private static string WildcardToRegex(string pattern)
    {
        // Escape all special regex chars, then replace \* with .*, etc.
        // Regex.Escape("*.md") -> "\\*\\.md", then we fix the backslashes.
        string escaped = Regex.Escape(pattern)
                              .Replace("\\*", ".*")
                              .Replace("\\?", ".");
        return "^" + escaped + "$";
    }

    /// <summary>
    /// Checks whether the given path matches any of the provided wildcard patterns.
    /// </summary>
    private static bool ShouldIgnore(string path, string[]? ignorePatterns)
    {
        // If there's no ignore pattern, never ignore
        if (ignorePatterns == null || ignorePatterns.Length == 0)
            return false;

        // We will match against the full path (relative or absolute).
        // If you only want to match the file name or folder name, 
        // you could do Path.GetFileName(path) or similar.
        // Also note: to handle cross-platform directory separators,
        // it can help to unify them:
        string normalizedPath = path.Replace('\\', '/');

        foreach (var pattern in ignorePatterns)
        {
            // Convert the wildcard to a regex
            string regexPattern = WildcardToRegex(pattern);

            // If you prefer ignoring case, add RegexOptions.IgnoreCase
            if (Regex.IsMatch(normalizedPath, regexPattern, RegexOptions.IgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public static void CopyDirectoryWithIgnore(
        string safe,
        string source, 
        string destination, 
        string[]? ignorePatterns)
    {
        if (!Directory.Exists(source))
        {
            throw new ArgumentException($"Source directory does not exist: {source}", nameof(source));
        }

        // 1. Create all non-ignored subdirectories
        //    (Uses SearchOption.AllDirectories to find *every* subdir under source).
        foreach (string dirPath in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
        {
            // Check if the directory should be ignored.
            // If so, skip creating it in the destination.
            if (ShouldIgnore(dirPath.Replace(safe, ""), ignorePatterns))
                continue;

            // Replace source path portion with destination path portion.
            // This builds a parallel folder structure in 'destination'.
            string destDirPath = dirPath.Replace(source, destination);

            // Make sure the directory exists (Directory.CreateDirectory is idempotent).
            Directory.CreateDirectory(destDirPath);
        }

        // 2. Copy all non-ignored files
        //    (Again enumerates all files in the entire directory tree).
        foreach (string filePath in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
        {
            if (ShouldIgnore(filePath.Replace(safe, ""), ignorePatterns))
                continue;

            // Build the destination file path by replacing the source folder portion.
            string destFilePath = filePath.Replace(source, destination);

            // Copy the file (overwrite if it already exists).
            Directory.CreateDirectory(Path.GetDirectoryName(destFilePath)!);
            File.Copy(filePath, destFilePath, overwrite: true);
        }
    }
}
