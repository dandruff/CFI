namespace CFI.Localization;

// Generated with ChatGPT-o1mini
public class Localized_deDE : ILocalized
{
    string ILocalized.HELP => @"
      Verwendung: cfi [Optionen]
    
      Optionen:
        -p, --pkgmeta <pfad>    Verwende Pfad als pkgmeta Datei, andernfalls wird nach einer .pkgmeta Datei im aktuellen Verzeichnis gesucht
        -l, --links             Verwende symbolische Links (Verknüpfungen unter Windows) anstelle von Kopieren (ignoriert 'ignored' Dateien, sei vorsichtig)
        -e, --external <ver>    Verzeichnis zum Klonen externer Repositories (Standard: ./.externals)
        -a, --addon <ver>       Verzeichnis zum Verpacken des Addons (Standard: ./.addon)
        -w, --wow <ver>         Verwende Verzeichnis als World of Warcraft Addons Ordner, anstelle der WOW_ADDONS Umgebungsvariable, um das Addon zu installieren
        -v, --version           Version anzeigen
        -q, --quiet             Nur Fehler anzeigen, keine weiteren Ausgaben
        -h, --help              Hilfe anzeigen
    ";

    string ILocalized.ERROR_COPY_FOLDER_FAILED => "Fehler beim Kopieren des Ordners {external} nach {addon}";

    string ILocalized.ERROR_FAILED_REPO_CLONE => "Fehler beim Klonen des Repositorys {repo}";

    string ILocalized.ERROR_LINK_FOLDER_FAILED => "Fehler beim Verknüpfen des Ordners {external} mit {addon}";

    string ILocalized.ERROR_SKIPPING_REPO => "Repository {repo} wird übersprungen (kein Git- oder SVN-Repository)";

    string ILocalized.ERROR_WOW_ADDON_DIRECTORY_NOT_FOUND => "Das World of Warcraft Addon-Verzeichnis wurde nicht gefunden. Addon wird nicht installiert. Bitte stellen Sie sicher, dass Sie die Umgebungsvariable 'WOW_ADDONS' auf das richtige Verzeichnis setzen oder das Argument '-wow <dir>' verwenden.";

    string ILocalized.FATAL_COULDNT_FIND_PKGMETA => "Die .pkgmeta konnte im aktuellen Verzeichnis nicht gefunden werden.";

    string ILocalized.INFO_COPIED_FOLDER => "Ordner {external} nach {addon} kopiert";

    string ILocalized.INFO_CREATING_EXTERNAL_REPO_DIRECTORY => "Externes Repo-Verzeichnis wird erstellt bei: {directory}";

    string ILocalized.INFO_CREATING_PACKAGED_ADDON_DIRECTORY => "Verpacktes Addon-Verzeichnis wird erstellt bei: {directory}";

    string ILocalized.INFO_LINKED_FOLDER => "Ordner {external} mit {addon} verknüpft";

    string ILocalized.INFO_LOADED_PKGMETA => ".pkgmeta für: {package} geladen";

    string ILocalized.INFO_RUNNING_COMMAND => "Führe {executable} {command} aus";

    string ILocalized.INFO_SUCCESSFULLY_CLONED_REPO => "{repo} erfolgreich geklont";

    string ILocalized.WARNING_DEPENDENCIES_NOT_SUPPORTED => "Abhängigkeiten werden nicht unterstützt und werden ignoriert.";

    string ILocalized.WARNING_FOLDER_ALREADY_EXISTS_SKIPPING => "Ordner {directory} existiert bereits, überspringe...";

    string ILocalized.WARNING_IGNORNING_UNSUPPORTED_REPO_PROPERTY => "Ignoriere nicht unterstützte Repo-Eigenschaft: {property}";
}