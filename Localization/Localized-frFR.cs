namespace CFI.Localization;

// CONTRIBUTORS: Makre sure to add the language code to the Localizations.cs file

// Generated with ChatGPT-o1
public class Localized_frFR : ILocalized
{
    string ILocalized.HELP => @"
      Utilisation: cfi [options]
    
      Options:
        -p, --pkgmeta <chemin>  Utilisez chemin comme fichier pkgmeta, sinon il recherchera un fichier .pkgmeta dans le répertoire actuel
        -l, --links             Utilisez des liens symboliques (jonctions sous Windows) au lieu de copier (ignore les fichiers 'ignored', soyez prudent)
        -e, --external <dir>    Utilisez le répertoire pour cloner les dépôts externes (par défaut : ./.externals)
        -a, --addon <dir>       Utilisez le répertoire pour empaqueter l'addon (par défaut : ./.addon)
        -w, --wow <dir>         Utilisez le répertoire comme dossier d'addons de World of Warcraft, au lieu de la variable d'environnement WOW_ADDONS, pour installer l'addon
        -v, --version           Afficher la version
        -q, --quiet             Ne pas afficher de sortie, sauf les erreurs
        -h, --help              Afficher l'aide
    ";

    string ILocalized.ERROR_COPY_FOLDER_FAILED => "Échec de la copie du dossier {external} vers {addon}";

    string ILocalized.ERROR_FAILED_REPO_CLONE => "Échec du clonage du dépôt {repo}";

    string ILocalized.ERROR_LINK_FOLDER_FAILED => "Échec de la liaison du dossier {external} à {addon}";

    string ILocalized.ERROR_SKIPPING_REPO => "Passage du dépôt {repo} (ce n'est ni un dépôt Git ni SVN)";

    string ILocalized.ERROR_WOW_ADDON_DIRECTORY_NOT_FOUND => "Le répertoire des addons de World of Warcraft est introuvable. L'addon ne sera pas installé. Veuillez vérifier que la variable d'environnement 'WOW_ADDONS' est définie correctement ou utilisez l'argument '-wow <dir>'.";

    string ILocalized.FATAL_COULDNT_FIND_PKGMETA => "Impossible de trouver le fichier .pkgmeta dans le répertoire actuel.";

    string ILocalized.INFO_COPIED_FOLDER => "Dossier {external} copié vers {addon}";

    string ILocalized.INFO_CREATING_EXTERNAL_REPO_DIRECTORY => "Création du répertoire de dépôt externe à : {directory}";

    string ILocalized.INFO_CREATING_PACKAGED_ADDON_DIRECTORY => "Création du répertoire d’addon empaqueté à : {directory}";

    string ILocalized.INFO_LINKED_FOLDER => "Dossier {external} lié à {addon}";

    string ILocalized.INFO_LOADED_PKGMETA => "Fichier .pkgmeta chargé pour : {package}";

    string ILocalized.INFO_RUNNING_COMMAND => "Exécution de {executable} {command}";

    string ILocalized.INFO_SUCCESSFULLY_CLONED_REPO => "Dépôt {repo} cloné avec succès";

    string ILocalized.WARNING_DEPENDENCIES_NOT_SUPPORTED => "Les dépendances ne sont pas prises en charge et seront ignorées.";

    string ILocalized.WARNING_FOLDER_ALREADY_EXISTS_SKIPPING => "Le dossier {directory} existe déjà, on ignore...";

    string ILocalized.WARNING_IGNORNING_UNSUPPORTED_REPO_PROPERTY => "Propriété de dépôt non prise en charge ignorée : {property}";
}