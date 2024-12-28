using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using YamlDotNet.Serialization;

namespace CFI.Models;

[YamlSerializable(typeof(ExternalRepo))]
public class ExternalRepo
{
    // Automatically determine the repository type when ExternalRepo created during deserialization
    // This is done by running `git ls-remote` and `svn info` on the repository URL
    // You need to await this property to get the result
    [YamlIgnore]
    public Task<RepoTypes> RepoType => RepoManager.GetRepoTypeAsync(Url);

    [YamlIgnore]
    public string? ProjectName { get; set; }

    [Required, YamlMember(Alias = "url")]
    public string Url { get; set; } = null!;

    [YamlMember(Alias = "tag")]
    public string? Tag { get; set; }

    [YamlMember(Alias = "commit")]
    public string? Commit { get; set; }
}
