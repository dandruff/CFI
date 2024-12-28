namespace CFI.Localization;

// CONTRIBUTORS: Makre sure to add the language code to the Localizations.cs file

public class Localized_enUS : ILocalized
{
    // CONTRIBUTORS: This constructor is only used for fallbacks, other languages don't need any constructor
    public Localized_enUS(string? fallback = null)
    {
        if (fallback is not null) Console.WriteLine($"Warning: Localization for {fallback} not found. Falling back to en-US.");
    }

    string ILocalized.ERROR_COPY_FOLDER_FAILED => "Failed to copy folder {external} to {addon}";

    string ILocalized.ERROR_FAILED_REPO_CLONE => "Failed to clone repository {repo}";

    string ILocalized.ERROR_LINK_FOLDER_FAILED => "Failed to link folder {external} to {addon}";

    string ILocalized.ERROR_SKIPPING_REPO => "Skipping repository {repo} (not a Git nor a SVN repository)";

    string ILocalized.ERROR_WOW_ADDON_DIRECTORY_NOT_FOUND => "The World of Warcraft Addon directory was not found. Addon will not be installed. Please make sure you set 'WOW_ADDONS' environment variable to the correct directory or use '-wow <dir>' argument.";

    string ILocalized.FATAL_COULDNT_FIND_PKGMETA => "Could not find the .pkgmeta in the current directory.";

    string ILocalized.INFO_COPIED_FOLDER => "Copied folder {external} to {addon}";

    string ILocalized.INFO_CREATING_EXTERNAL_REPO_DIRECTORY => "Creating external repo directory at: {directory}";

    string ILocalized.INFO_CREATING_PACKAGED_ADDON_DIRECTORY => "Creating packaged addon directory at: {directory}";

    string ILocalized.INFO_LINKED_FOLDER => "Linked folder {external} to {addon}";

    string ILocalized.INFO_LOADED_PKGMETA => "Loaded .pkgmeta for: {package}";

    string ILocalized.INFO_RUNNING_COMMAND => "Running {executable} {command}";

    string ILocalized.INFO_SUCCESSFULLY_CLONED_REPO => "Successfully cloned {repo}";

    string ILocalized.WARNING_DEPENDENCIES_NOT_SUPPORTED => "Dependencies are not supported and will be ignored.";

    string ILocalized.WARNING_FOLDER_ALREADY_EXISTS_SKIPPING => "Folder {directory} already exists, skipping...";

    string ILocalized.WARNING_IGNORNING_UNSUPPORTED_REPO_PROPERTY => "Ignoring unsupported repo property: {property}";
}