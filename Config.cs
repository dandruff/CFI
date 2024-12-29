namespace CFI;

public class Config(string? pkgmetaPath, bool useLinks, string externalDir, string addonDir, string? wowDir, bool quiet)
{   
    public string? PackageMetaPath { get; set; } = pkgmetaPath;
    
    public bool UseLinks { get; init; } = useLinks;
    
    public string ExternalDir { get; init; } = externalDir;
    
    public string AddonDir { get; init; } = addonDir;

    public string? WowDir { get; init; } = wowDir;
    
    public bool IsWoWDirValid => !string.IsNullOrWhiteSpace(WowDir) && Directory.Exists(WowDir);

    public bool Quiet { get; init; } = quiet;

    public static Config FromParams(string[] args)
    {
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
            }
        }

        return new Config(pkgmetaPath, useLinks, externalDir, addonDir, wowDir, quiet);
    }
}