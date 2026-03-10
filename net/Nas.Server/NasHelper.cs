using Com.Scm.Enums;

namespace Com.Scm.Nas
{
    public class NasHelper
    {
        public static Dictionary<ScmFileKindEnum, List<string>> _ExtList = [];

        public static void setup()
        {
            if (_ExtList.Count > 0)
            {
                return;
            }

            _ExtList[ScmFileKindEnum.Byte] = new List<string>() { "bin", "dat", "apk", "exe" };
            _ExtList[ScmFileKindEnum.Text] = new List<string>() { "txt", "log", "ini", "json" };
            _ExtList[ScmFileKindEnum.Image] = new List<string>() { "png", "jpg", "jpeg", "jpe", "jp2", "webp", "svg", "bmp", "gif",
                    "tiff", "tif", "heic", "heif", "ai", "eps", "cdr", "ico", "psd" };
            _ExtList[ScmFileKindEnum.Audio] = new List<string>() { "wav", "flac", "ape", "m4a", "aiff", "aif", "mp3", "aac", "m4a",
                    "ogg", "wma", "amr", "mid", "midi", "dsf", "dff", "aac"};
            _ExtList[ScmFileKindEnum.Video] = new List<string>() { "mp4", "mkv", "avi", "mov", "flv", "webm", "m3u8", "wmv", "ts",
                    "mov", "3gp", "vob", "rmvb", "rm", "mpeg", "mpg"};
            _ExtList[ScmFileKindEnum.Office] = new List<string>() { "doc", "docx", "wps", "wpt", "pdf", "txt", "odt", "xls", "xlsx",
                    "csv", "et", "ett", "ods", "ppt", "pptx", "dps", "dot", "odp", "vsx", "vsdx", "draw", "xmind"};
            _ExtList[ScmFileKindEnum.Archive] = new List<string>() { "zip", "rar", "7z", "gz", "tgz", "tar", "bz", "bz2", "dmg", "cab",
                    "iso", "lzma", "zipx", "arc", "jar", "war", "gzip", "bzip", "bzip2"};
            _ExtList[ScmFileKindEnum.Code] = new List<string>() { "py", "js", "mjs", "cjs", "php", "php5", "phtml", "sh", "bash",
                    "zsh", "c", "h", "cpp", "cc", "cxx", "hpp", "h", "cs", "java", "go", "html", "htm", "css", "scss", "sass",
                    "less", "vue", "jsx", "tsx", "ts", "sql", "json", "yml", "xml", "yaml", "rb", "swfit", "kt", "kts", "rs"};
        }

        public static bool IsValid(ScmFileKindEnum doc, string ext)
        {
            setup();

            if (!_ExtList.ContainsKey(doc))
            {
                return false;
            }

            var extList = _ExtList[doc];
            if (extList == null)
            {
                return false;
            }

            ext = ext.ToLower().TrimStart('.');
            return extList.Contains(ext);
        }

        public static bool IsImage(string ext)
        {
            return IsValid(ScmFileKindEnum.Image, ext);
        }

        public static bool IsAudio(string ext)
        {
            return IsValid(ScmFileKindEnum.Audio, ext);
        }

        public static bool IsVideo(string ext)
        {
            return IsValid(ScmFileKindEnum.Video, ext);
        }

        public static bool IsOffice(string ext)
        {
            return IsValid(ScmFileKindEnum.Office, ext);
        }

        public static bool IsScript(string ext)
        {
            return IsValid(ScmFileKindEnum.Code, ext);
        }

        public static bool IsArchive(string ext)
        {
            return IsValid(ScmFileKindEnum.Archive, ext);
        }

        public static bool IsText(string ext)
        {
            return IsValid(ScmFileKindEnum.Text, ext);
        }

        public static ScmFileKindEnum getKind(string ext)
        {
            setup();

            if (ext != null)
            {
                foreach (var dic in _ExtList.Keys)
                {
                    var list = _ExtList[dic];
                    if (list.Contains(ext))
                    {
                        return dic;
                    }
                }
            }

            return ScmFileKindEnum.None;
        }
    }
}
