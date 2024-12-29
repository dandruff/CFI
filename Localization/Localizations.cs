using System.Globalization;

namespace CFI.Localization;

public static class Localizations
{
    public readonly static ILocalized Current =

        // If you need to change this (using only the first two characters of the culture name),
        // make sure you mention that in the Pull Request!!!
        CultureInfo.CurrentCulture.Name[..2] switch {
            "de" => new Localized_deDE(),
            "en" => new Localized_enUS(),
            "es" => new Localized_esES(),
            "fr" => new Localized_frFR(),
            "ko" => new Localized_koKR(),
            "pt" => new Localized_ptBR(),
            "ru" => new Localized_ruRU(),
            "zh" => new Localized_zhCN(),

            // CONTRIBUTORS: Add your localizations here

            // fallback to enUS (displays a message in the console)
            _ => new Localized_enUS(CultureInfo.CurrentCulture.Name)
        };
}
