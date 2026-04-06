namespace Com.Scm.Nas
{
    public class NasUtils
    {
        /// <summary>
        /// 获取上级目录
        /// </summary>
        /// <param name="file">虚拟绝对路径</param>
        /// <returns></returns>
        public static string GetParentPath(string file)
        {
            file = file.TrimEnd(NasEnv.WebSeparator);
            var idx = file.LastIndexOf(NasEnv.WebSeparator);
            if (idx > 0)
            {
                return file.Substring(0, idx);
            }

            return "";
        }

        public static string CombinePath(string path, params string[] names)
        {
            if (names == null || names.Length == 0)
            {
                return path;
            }

            if (string.IsNullOrEmpty(path))
            {
                path = "";
            }

            foreach (var name in names)
            {
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                path += NasEnv.WebSeparator + name;
            }

            return path;
        }
    }
}
