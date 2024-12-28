namespace CFI.Localization;

public interface ILocalized
{
    /// <summary>
    /// Failed to copy folder {external} to {addon}
    /// </summary>
    string ERROR_COPY_FOLDER_FAILED { get; }

    /// <summary>
    /// Failed to clone repository {repo}
    /// </summary>
    string ERROR_FAILED_REPO_CLONE { get; }

    /// <summary>
    /// Failed to link folder {external} to {addon}
    /// </summary>
    string ERROR_LINK_FOLDER_FAILED { get; }

    /// <summary>
    /// Skipping repository {repo} (not a Git nor a SVN repository)
    /// </summary>
    string ERROR_SKIPPING_REPO { get; }
    
    /// <summary>
    /// The World of Warcraft Addon directory was not found. Addon will not be installed. Please make sure you set 'WOW_ADDONS' environment variable to the correct directory or use '-wow <dir>' argument.
    /// </summary>
    string ERROR_WOW_ADDON_DIRECTORY_NOT_FOUND { get; }

    /// <summary>
    /// Could not find the .pkgmeta in the current directory.
    /// </summary>
    string FATAL_COULDNT_FIND_PKGMETA { get; }

    /// <summary>
    /// Copied folder {external} to {addon}
    /// </summary>
    string INFO_COPIED_FOLDER { get; }

    /// <summary>
    /// Creating external repo directory at: {directory}
    /// </summary>
    string INFO_CREATING_EXTERNAL_REPO_DIRECTORY { get; }

    /// <summary>
    /// Creating packaged addon directory at: {directory}
    /// </summary>
    string INFO_CREATING_PACKAGED_ADDON_DIRECTORY { get; }

    /// <summary>
    /// Linked folder {external} to {addon}
    /// </summary>
    string INFO_LINKED_FOLDER { get; }

    /// <summary>
    /// Loaded .pkgmeta for: {package}
    /// </summary>
    string INFO_LOADED_PKGMETA { get; }

    /// <summary>
    /// Running {executable} {command}
    /// </summary>
    string INFO_RUNNING_COMMAND { get; }

    /// <summary>
    /// Successfully cloned {repo}
    /// </summary>
    string INFO_SUCCESSFULLY_CLONED_REPO { get; }

    /// <summary>
    /// Dependencies are not supported and will be ignored.
    /// </summary>
    string WARNING_DEPENDENCIES_NOT_SUPPORTED { get; }

    /// <summary>
    /// Folder {directory} already exists, skipping...
    /// </summary>
    string WARNING_FOLDER_ALREADY_EXISTS_SKIPPING { get; }

    /// <summary>
    /// Ignoring unsupported repo property: {property}
    /// </summary>
    string WARNING_IGNORNING_UNSUPPORTED_REPO_PROPERTY { get; }
}