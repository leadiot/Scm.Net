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
    }
}
