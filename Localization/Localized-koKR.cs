namespace CFI.Localization;

// CONTRIBUTORS: Makre sure to add the language code to the Localizations.cs file

// Generated with ChatGPT-o1
public class Localized_koKR : ILocalized
{
    string ILocalized.HELP => @"
      사용법: cfi [옵션]
    
      옵션:
        -p, --pkgmeta <경로>      경로를 pkgmeta 파일로 사용합니다. 그렇지 않으면 현재 디렉토리에서 .pkgmeta 파일을 찾습니다.
        -l, --links              복사 대신 심볼릭 링크(Windows에서는 접합)를 사용합니다. ('ignored' 파일은 무시하므로 주의하세요)
        -e, --external <디렉토리>  외부 저장소를 복제하기 위한 디렉토리를 사용합니다. (기본값: ./.externals)
        -a, --addon <디렉토리>     애드온을 패키징하기 위한 디렉토리를 사용합니다. (기본값: ./.addon)
        -w, --wow <디렉토리>       World of Warcraft 애드온 폴더로 디렉토리를 사용하며, 애드온을 설치하기 위해 WOW_ADDONS 환경 변수를 대신하여 사용합니다.
        -v, --version            버전 표시
        -q, --quiet              오류 제외 모든 출력을 표시하지 않습니다.
        -h, --help               도움말 표시
    ";

    string ILocalized.ERROR_COPY_FOLDER_FAILED => "폴더 {external}을(를) {addon}(으)로 복사하지 못했습니다.";

    string ILocalized.ERROR_FAILED_REPO_CLONE => "저장소 {repo}를 복제하지 못했습니다.";

    string ILocalized.ERROR_LINK_FOLDER_FAILED => "폴더 {external}을(를) {addon}(으)로 연결하지 못했습니다.";

    string ILocalized.ERROR_SKIPPING_REPO => "저장소 {repo}를 건너뜁니다. (Git 또는 SVN 저장소가 아닙니다)";

    string ILocalized.ERROR_WOW_ADDON_DIRECTORY_NOT_FOUND => "월드 오브 워크래프트 애드온 디렉터리를 찾을 수 없습니다. 애드온이 설치되지 않습니다. 'WOW_ADDONS' 환경 변수를 올바른 디렉터리로 설정하거나 '-wow <dir>' 인수를 사용하십시오.";

    string ILocalized.FATAL_COULDNT_FIND_PKGMETA => ".pkgmeta 파일을 현재 디렉터리에서 찾을 수 없습니다.";

    string ILocalized.INFO_COPIED_FOLDER => "폴더 {external}을(를) {addon}(으)로 복사했습니다.";

    string ILocalized.INFO_CREATING_EXTERNAL_REPO_DIRECTORY => "외부 저장소 디렉터리를 생성 중입니다: {directory}";

    string ILocalized.INFO_CREATING_PACKAGED_ADDON_DIRECTORY => "패키지된 애드온 디렉터리를 생성 중입니다: {directory}";

    string ILocalized.INFO_LINKED_FOLDER => "폴더 {external}을(를) {addon}(으)로 연결했습니다.";

    string ILocalized.INFO_LOADED_PKGMETA => "{package}에 대한 .pkgmeta를 로드했습니다.";

    string ILocalized.INFO_RUNNING_COMMAND => "{executable} {command} 명령을 실행 중입니다.";

    string ILocalized.INFO_SUCCESSFULLY_CLONED_REPO => "{repo}를 성공적으로 복제했습니다.";

    string ILocalized.WARNING_DEPENDENCIES_NOT_SUPPORTED => "종속성은 지원되지 않으며 무시됩니다.";

    string ILocalized.WARNING_FOLDER_ALREADY_EXISTS_SKIPPING => "폴더 {directory}가 이미 존재합니다. 건너뜁니다...";

    string ILocalized.WARNING_IGNORNING_UNSUPPORTED_REPO_PROPERTY => "지원되지 않는 저장소 속성을 무시합니다: {property}";
}