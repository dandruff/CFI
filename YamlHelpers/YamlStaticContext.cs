using CFI.Models;

namespace CFI.YamlHelpers;

[YamlStaticContext]
[YamlSerializable(typeof(PackageMetadata))]
public partial class YamlStaticContext : StaticContext
{
    public static readonly StaticContext Instance = new YamlStaticContext();
}
