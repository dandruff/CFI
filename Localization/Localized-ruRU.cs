namespace CFI.Localization;

// CONTRIBUTORS: Makre sure to add the language code to the Localizations.cs file

// Generated with ChatGPT-o1
public class Localized_ruRU : ILocalized
{
    string ILocalized.HELP => @"
      Использование: cfi [опции]
    
      Опции:
        -p, --pkgmeta <путь>    Используйте путь как файл pkgmeta, в противном случае будет искать файл .pkgmeta в текущей директории
        -l, --links             Используйте символические ссылки (соединения в Windows) вместо копирования (игнорирует файлы 'ignored', будьте осторожны)
        -e, --external <дир>    Используйте директорию для клонирования внешнего репозитория (по умолчанию: ./.externals)
        -a, --addon <дир>       Используйте директорию для упаковки аддона (по умолчанию: ./.addon)
        -w, --wow <дир>         Используйте директорию в качестве папки аддонов World of Warcraft, вместо переменной окружения WOW_ADDONS, для установки аддона
        -v, --version           Показать версию
        -q, --quiet             Не показывать никаких выводов, кроме ошибок
        -h, --help              Показать помощь
    ";

    string ILocalized.ERROR_COPY_FOLDER_FAILED => "Не удалось скопировать папку {external} в {addon}";

    string ILocalized.ERROR_FAILED_REPO_CLONE => "Не удалось клонировать репозиторий {repo}";

    string ILocalized.ERROR_LINK_FOLDER_FAILED => "Не удалось связать папку {external} с {addon}";

    string ILocalized.ERROR_SKIPPING_REPO => "Пропуск репозитория {repo} (не Git и не SVN репозиторий)";

    string ILocalized.ERROR_WOW_ADDON_DIRECTORY_NOT_FOUND => "Каталог модификаций World of Warcraft не найден. Модификация не будет установлена. Убедитесь, что переменная среды 'WOW_ADDONS' задана верно, или используйте аргумент '-wow <dir>'.";

    string ILocalized.FATAL_COULDNT_FIND_PKGMETA => "Не удалось найти файл .pkgmeta в текущем каталоге.";

    string ILocalized.INFO_COPIED_FOLDER => "Скопирована папка {external} в {addon}";

    string ILocalized.INFO_CREATING_EXTERNAL_REPO_DIRECTORY => "Создается каталог внешнего репозитория: {directory}";

    string ILocalized.INFO_CREATING_PACKAGED_ADDON_DIRECTORY => "Creating packaged addon directory at: {directory}";

    string ILocalized.INFO_LINKED_FOLDER => "Папка {external} была связана с {addon}";

    string ILocalized.INFO_LOADED_PKGMETA => "Загружен .pkgmeta для: {package}";

    string ILocalized.INFO_RUNNING_COMMAND => "Выполняется {executable} {command}";

    string ILocalized.INFO_SUCCESSFULLY_CLONED_REPO => "Успешно клонирован репозиторий {repo}";

    string ILocalized.WARNING_DEPENDENCIES_NOT_SUPPORTED => "Зависимости не поддерживаются и будут проигнорированы.";

    string ILocalized.WARNING_FOLDER_ALREADY_EXISTS_SKIPPING => "Папка {directory} уже существует, пропуск...";

    string ILocalized.WARNING_IGNORNING_UNSUPPORTED_REPO_PROPERTY => "Игнорируется неподдерживаемое свойство репозитория: {property}";
}