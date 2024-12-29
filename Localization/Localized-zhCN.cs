namespace CFI.Localization;

// Generated with ChatGPT-o1mini
public class Localized_zhCN : ILocalized
{
    string ILocalized.HELP => @"
      使用方法: cfi [选项]

      选项:
        -p, --pkgmeta <路径>     使用路径作为 pkgmeta 文件，否则将在当前目录查找 .pkgmeta 文件
        -l, --links             使用符号链接（Windows 上为连接点）代替复制（忽略 'ignored' 文件，请小心）
        -e, --external <目录>    用于克隆外部仓库的目录（默认: ./.externals）
        -a, --addon <目录>       用于打包插件的目录（默认: ./.addon）
        -w, --wow <目录>         使用指定目录作为《魔兽世界》插件文件夹，代替 WOW_ADDONS 环境变量来安装插件
        -v, --version           显示版本
        -q, --quiet             只显示错误，不显示其他输出
        -h, --help              显示帮助
    ";

    string ILocalized.ERROR_COPY_FOLDER_FAILED => "复制文件夹 {external} 到 {addon} 失败。";

    string ILocalized.ERROR_FAILED_REPO_CLONE => "克隆仓库 {repo} 失败。";

    string ILocalized.ERROR_LINK_FOLDER_FAILED => "链接文件夹 {external} 到 {addon} 失败。";

    string ILocalized.ERROR_SKIPPING_REPO => "跳过仓库 {repo}（不是 Git 或 SVN 仓库）。";

    string ILocalized.ERROR_WOW_ADDON_DIRECTORY_NOT_FOUND => "未找到《魔兽世界》插件目录。插件将不会被安装。请确保将 'WOW_ADDONS' 环境变量设置为正确的目录，或使用 '-wow <dir>' 参数。";

    string ILocalized.FATAL_COULDNT_FIND_PKGMETA => "当前目录中找不到 .pkgmeta 文件。";

    string ILocalized.INFO_COPIED_FOLDER => "已复制文件夹 {external} 到 {addon}。";

    string ILocalized.INFO_CREATING_EXTERNAL_REPO_DIRECTORY => "正在创建外部仓库目录：{directory}。";

    string ILocalized.INFO_CREATING_PACKAGED_ADDON_DIRECTORY => "正在创建打包插件目录：{directory}。";

    string ILocalized.INFO_LINKED_FOLDER => "已链接文件夹 {external} 到 {addon}。";

    string ILocalized.INFO_LOADED_PKGMETA => "已加载 .pkgmeta 文件：{package}。";

    string ILocalized.INFO_RUNNING_COMMAND => "正在运行 {executable} {command}。";

    string ILocalized.INFO_SUCCESSFULLY_CLONED_REPO => "已成功克隆仓库 {repo}。";

    string ILocalized.WARNING_DEPENDENCIES_NOT_SUPPORTED => "不支持依赖项，将被忽略。";

    string ILocalized.WARNING_FOLDER_ALREADY_EXISTS_SKIPPING => "文件夹 {directory} 已存在，跳过...";

    string ILocalized.WARNING_IGNORNING_UNSUPPORTED_REPO_PROPERTY => "忽略不支持的仓库属性：{property}。";
}