using System.ComponentModel.DataAnnotations;

namespace CFI.Models;

[YamlSerializable(typeof(ExternalRepo))]
public class ExternalRepo
{
    [YamlIgnore]
    public RepoTypes RepoType { get; set; }

    [YamlIgnore]
    public RepoTypes FullPath { get; set; }

    [YamlIgnore]
    public string? ProjectName { get; set; }

    [Required, YamlMember(Alias = "url")]
    public string Url { get; set; } = null!;

    [YamlMember(Alias = "tag")]
    public string? Tag { get; set; }

    [YamlMember(Alias = "commit")]
    public string? Commit { get; set; }
}
