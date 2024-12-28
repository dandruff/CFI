using System.ComponentModel.DataAnnotations;

namespace CFI.Models;

[YamlSerializable(typeof(PackageMetadata))]
public class PackageMetadata
{
    [Required, YamlMember(Alias = "package-as")]
    public string PackageAs { get; set; } = null!;

    [YamlMember(Alias = "externals")]
    public Dictionary<string, ExternalRepo>? Externals { get; set; }

    [YamlMember(Alias = "manual-changelog")]
    public ManualChangelog? ManualChangelog { get; set; }

    [YamlMember(Alias = "ignore")]
    public string[]? Ignore { get; set; }

    [YamlMember(Alias = "plain-copy")]
    public string[]? PlainCopy { get; set; }

    [YamlMember(Alias = "move-folders")]
    public Dictionary<string, string>? MoveFolders { get; set; }
}
