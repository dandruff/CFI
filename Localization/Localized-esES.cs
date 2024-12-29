namespace CFI.Localization;

// Generated with ChatGPT-o1
public class Localized_esES : ILocalized
{
    string ILocalized.HELP => @"
      Uso: cfi [opciones]
    
      Opciones:
        -p, --pkgmeta <ruta>    Usa ruta como el archivo pkgmeta, de lo contrario buscará un archivo .pkgmeta en el directorio actual
        -l, --links             Usa enlaces simbólicos (puntos de enlace en Windows) en lugar de copiar (ignora archivos 'ignored', ten cuidado)
        -e, --external <dir>    Usa directorio para clonar repositorios externos (por defecto: ./.externals)
        -a, --addon <dir>       Usa directorio para empaquetar el addon (por defecto: ./.addon)
        -w, --wow <dir>         Usa directorio como la carpeta de addons de World of Warcraft, en lugar de la variable de entorno WOW_ADDONS, para instalar el addon
        -v, --version           Muestra la versión
        -q, --quiet             No muestra ninguna salida, excepto errores
        -h, --help              Muestra ayuda
    ";

    string ILocalized.ERROR_COPY_FOLDER_FAILED => "No se pudo copiar la carpeta {external} a {addon}";

    string ILocalized.ERROR_FAILED_REPO_CLONE => "No se pudo clonar el repositorio {repo}";

    string ILocalized.ERROR_LINK_FOLDER_FAILED => "No se pudo enlazar la carpeta {external} con {addon}";

    string ILocalized.ERROR_SKIPPING_REPO => "Omitiendo el repositorio {repo} (no es un repositorio Git ni SVN)";

    string ILocalized.ERROR_WOW_ADDON_DIRECTORY_NOT_FOUND => "No se pudo encontrar el directorio de Addons de World of Warcraft. El addon no se instalará. Asegúrese de configurar la variable de entorno 'WOW_ADDONS' con la ruta correcta o use el argumento '-wow <dir>'.";

    string ILocalized.FATAL_COULDNT_FIND_PKGMETA => "No se pudo encontrar el archivo .pkgmeta en el directorio actual.";

    string ILocalized.INFO_COPIED_FOLDER => "Se copió la carpeta {external} a {addon}";

    string ILocalized.INFO_CREATING_EXTERNAL_REPO_DIRECTORY => "Creando el directorio de repositorio externo en: {directory}";

    string ILocalized.INFO_CREATING_PACKAGED_ADDON_DIRECTORY => "Creando el directorio de addon empaquetado en: {directory}";

    string ILocalized.INFO_LINKED_FOLDER => "Carpeta {external} enlazada con {addon}";

    string ILocalized.INFO_LOADED_PKGMETA => "Se cargó .pkgmeta para: {package}";

    string ILocalized.INFO_RUNNING_COMMAND => "Ejecutando {executable} {command}";

    string ILocalized.INFO_SUCCESSFULLY_CLONED_REPO => "Repositorio {repo} clonado con éxito";

    string ILocalized.WARNING_DEPENDENCIES_NOT_SUPPORTED => "Las dependencias no son compatibles y se ignorarán.";

    string ILocalized.WARNING_FOLDER_ALREADY_EXISTS_SKIPPING => "La carpeta {directory} ya existe, se omite...";

    string ILocalized.WARNING_IGNORNING_UNSUPPORTED_REPO_PROPERTY => "Se ignora la propiedad de repositorio no compatible: {property}";
}