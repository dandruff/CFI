using System.Globalization;

namespace CFI.Localization;

public static class Localizations
{
    public readonly static ILocalized Current =
        CultureInfo.CurrentCulture.Name switch {
            "en-US" => new Localized_enUS(),

            // CONTRIBUTORS: Add your localizations here

            // fallback to enUS (displays a message in the console)
            _ => new Localized_enUS(CultureInfo.CurrentCulture.Name)
        };
}
