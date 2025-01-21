using System.Diagnostics;
using System.Runtime.InteropServices;
using CFI.Models;

namespace CFI;

public static class Commands
{
    private static Task<Process> Run(string executable, string command, string? WorkingDirectory = null, CancellationToken cancellationToken = default)
        => Task.Run(async () => {
            Process? gitProcess = Process.Start(
                new ProcessStartInfo(executable, command)
                {
                    WorkingDirectory = WorkingDirectory is not null ? WorkingDirectory : Environment.CurrentDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            ) ?? throw new InvalidOperationException("Failed to start process");
            
            await gitProcess.WaitForExitAsync(cancellationToken);

            return gitProcess;
        });

    private static Task<Process> Git(string command, string? WorkingDirectory = null, CancellationToken cancellationToken = default)
        => Run("git", command, WorkingDirectory, cancellationToken);

    private static Task<Process> Svn(string command, string? WorkingDirectory = null, CancellationToken cancellationToken = default)
        => Run("svn", command, WorkingDirectory, cancellationToken);

    public static Task<RepoTypes> CheckRemoteUriRepoTypeAsync(string uri, CancellationToken cancellationToken = default)
        => Task.Run(async () => {
            Task<Process> git = Git($"ls-remote {uri}", cancellationToken: cancellationToken);
            Task<Process> svn = Svn($"info {uri}", cancellationToken: cancellationToken);

            await foreach(Task<Process> t in Task.WhenEach(git, svn))
                if (t.Result.ExitCode == 0)
                    return t == git ? RepoTypes.Git : RepoTypes.Svn;

            return RepoTypes.Unknown;
        });

    public static Task<RepoTypes> CheckLocalUriRepoTypeAsync(string uri, CancellationToken cancellationToken = default)
        => Task.Run(async () => {
            Task<Process> git = Git($"rev-parse --is-inside-work-tree", uri, cancellationToken: cancellationToken);
            Task<Process> svn = Svn($"info", uri, cancellationToken: cancellationToken);

            await foreach(Task<Process> t in Task.WhenEach(git, svn))
                if (t.Result.ExitCode == 0)
                    return t == git ? RepoTypes.Git : RepoTypes.Svn;

            return RepoTypes.Unknown;
        });

    private static Task<Process> CloneGitRepoAsync(string uri, string destination, CancellationToken cancellationToken = default)
        => Git($"clone {uri} {destination} --depth 1", cancellationToken: cancellationToken);

    private static Task<Process> CloneSvnRepoAsync(string uri, string destination, CancellationToken cancellationToken = default)
        => Svn($"checkout {uri} {destination} --depth=empty", cancellationToken: cancellationToken);

    public static Task CloneRepoAsync(RepoTypes repoType, string uri, string destination, CancellationToken cancellationToken = default)
        => repoType switch
        {
            RepoTypes.Git => CloneGitRepoAsync(uri, destination, cancellationToken),
            RepoTypes.Svn => CloneSvnRepoAsync(uri, destination, cancellationToken),
            _ => throw new InvalidOperationException("Unknown repo type")
        };

    private static Task<Process> PullGitRepoAsync(string WorkingDirectory, CancellationToken cancellationToken = default)
        => Git($"pull", WorkingDirectory: WorkingDirectory, cancellationToken: cancellationToken);

    private static Task<Process> PullSvnRepoAsync(string WorkingDirectory, CancellationToken cancellationToken = default)
        => Svn($"update", WorkingDirectory: WorkingDirectory, cancellationToken: cancellationToken);

    public static Task PullRepoAsync(RepoTypes repoType, string WorkingDirectory, CancellationToken cancellationToken = default)
        => repoType switch
        {
            RepoTypes.Git => PullGitRepoAsync(WorkingDirectory, cancellationToken),
            RepoTypes.Svn => PullSvnRepoAsync(WorkingDirectory, cancellationToken),
            _ => throw new InvalidOperationException("Unknown repo type")
        };

    private static Task<string> HeadHashGitRepoAsync(string WorkingDirectory, CancellationToken cancellationToken = default)
        => Git($"rev-parse HEAD", WorkingDirectory: WorkingDirectory, cancellationToken: cancellationToken).ContinueWith(t => t.Result.StandardOutput.ReadToEnd(), cancellationToken);

    private static Task<string> HeadHashSvnRepoAsync(string WorkingDirectory, CancellationToken cancellationToken = default)
        => Svn($"info --show-item revision", WorkingDirectory: WorkingDirectory, cancellationToken: cancellationToken).ContinueWith(t => t.Result.StandardOutput.ReadToEnd(), cancellationToken);

    public static Task<string> HeadHashRepoAsync(RepoTypes repoType, string WorkingDirectory, CancellationToken cancellationToken = default)
        => repoType switch
        {
            RepoTypes.Git => HeadHashGitRepoAsync(WorkingDirectory, cancellationToken),
            RepoTypes.Svn => HeadHashSvnRepoAsync(WorkingDirectory, cancellationToken),
            _ => throw new InvalidOperationException("Unknown repo type")
        };

    private static Task<string> LogGitRepoAsync(string oldHead, string currentHead, string WorkingDirectory, CancellationToken cancellationToken = default)
        => Git($"log --pretty=format:\"%H - %s\" {oldHead}..{currentHead}", WorkingDirectory: WorkingDirectory, cancellationToken: cancellationToken).ContinueWith(t => t.Result.StandardOutput.ReadToEnd(), cancellationToken);

    private static Task<string> LogSvnRepoAsync(string oldHead, string currentHead, string WorkingDirectory, CancellationToken cancellationToken = default)
        => Svn($"svn log -r {oldHead}:{currentHead}", WorkingDirectory: WorkingDirectory, cancellationToken: cancellationToken).ContinueWith(t => t.Result.StandardOutput.ReadToEnd(), cancellationToken);

    public static Task<string> LogRepoAsync(RepoTypes repoType, string oldHead, string currentHead, string WorkingDirectory, CancellationToken cancellationToken = default)
        => repoType switch
        {
            RepoTypes.Git => LogGitRepoAsync(oldHead, currentHead, WorkingDirectory, cancellationToken),
            RepoTypes.Svn => LogSvnRepoAsync(oldHead, currentHead, WorkingDirectory, cancellationToken),
            _ => throw new InvalidOperationException("Unknown repo type")
        };

    public static Task<string?> LinkFolderAsync(string source, string destination, CancellationToken cancellationToken = default)
        => Task.Run(() => {
            try
            {
                Process p;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    p = Process.Start(new ProcessStartInfo("cmd.exe", $"/c mklink /J \"{destination}\" \"{source}\"") {
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false
                    }) ?? throw new InvalidOperationException("Failed to start process");
                else
                    p = Process.Start(new ProcessStartInfo("ln", $"-s \"{source}\" \"{destination}\"") {
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false
                    }) ?? throw new InvalidOperationException("Failed to start process");

                p.WaitForExit();
                if (p.ExitCode != 0) return p.StandardError.ReadToEnd();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return null;
        }, cancellationToken);
}