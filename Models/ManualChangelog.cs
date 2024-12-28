using System.ComponentModel.DataAnnotations;

namespace CFI.Models;

[YamlSerializable(typeof(ManualChangelog))]
public class ManualChangelog
{
    [Required, YamlMember(Alias = "filename")]
    public string Filename { get; set; } = null!;

    [YamlMember(Alias = "markup-type")]
    public string? MarkupType { get; set; }
}
