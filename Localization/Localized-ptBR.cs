namespace CFI.Localization;

// CONTRIBUTORS: Makre sure to add the language code to the Localizations.cs file

// Generated with ChatGPT-o1
public class Localized_ptBR : ILocalized
{
    string ILocalized.HELP => @"
      Uso: cfi [opções]
    
      Opções:
        -p, --pkgmeta <caminho>    Use o caminho como o arquivo pkgmeta, caso contrário, procurará um arquivo .pkgmeta no diretório atual
        -l, --links                Use links simbólicos (junções no Windows) em vez de copiar (ignora arquivos 'ignored', tenha cuidado)
        -e, --external <dir>       Use o diretório para clonar o repositório externo (padrão: ./.externals)
        -a, --addon <dir>          Use o diretório para empacotar o addon (padrão: ./.addon)
        -w, --wow <dir>            Use o diretório como a pasta de addons do World of Warcraft, em vez da variável de ambiente WOW_ADDONS, para instalar o addon
        -v, --version              Mostrar a versão
        -q, --quiet                Não mostre nenhuma saída, exceto erros
        -h, --help                 Mostrar ajuda
    ";

    string ILocalized.ERROR_COPY_FOLDER_FAILED => "Falha ao copiar a pasta {external} para {addon}";

    string ILocalized.ERROR_FAILED_REPO_CLONE => "Falha ao clonar o repositório {repo}";

    string ILocalized.ERROR_LINK_FOLDER_FAILED => "Falha ao vincular a pasta {external} a {addon}";

    string ILocalized.ERROR_SKIPPING_REPO => "Ignorando o repositório {repo} (não é um repositório Git ou SVN)";

    string ILocalized.ERROR_WOW_ADDON_DIRECTORY_NOT_FOUND => "O diretório de Addons do World of Warcraft não foi encontrado. O addon não será instalado. Certifique-se de que a variável de ambiente 'WOW_ADDONS' foi configurada corretamente ou use o argumento '-wow <dir>'.";

    string ILocalized.FATAL_COULDNT_FIND_PKGMETA => "Não foi possível encontrar o arquivo .pkgmeta no diretório atual.";

    string ILocalized.INFO_COPIED_FOLDER => "Pasta {external} copiada para {addon}";

    string ILocalized.INFO_CREATING_EXTERNAL_REPO_DIRECTORY => "Criando diretório de repositório externo em: {directory}";

    string ILocalized.INFO_CREATING_PACKAGED_ADDON_DIRECTORY => "Criando diretório de addon empacotado em: {directory}";

    string ILocalized.INFO_LINKED_FOLDER => "Pasta {external} vinculada a {addon}";

    string ILocalized.INFO_LOADED_PKGMETA => "Carregado .pkgmeta para: {package}";

    string ILocalized.INFO_RUNNING_COMMAND => "Executando {executable} {command}";

    string ILocalized.INFO_SUCCESSFULLY_CLONED_REPO => "Repositório {repo} clonado com sucesso";

    string ILocalized.WARNING_DEPENDENCIES_NOT_SUPPORTED => "Dependências não são suportadas e serão ignoradas.";

    string ILocalized.WARNING_FOLDER_ALREADY_EXISTS_SKIPPING => "A pasta {directory} já existe, ignorando...";

    string ILocalized.WARNING_IGNORNING_UNSUPPORTED_REPO_PROPERTY => "Ignorando a propriedade de repositório não suportada: {property}";
}