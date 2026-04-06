using System.IO;
using System.Text.RegularExpressions;

namespace Com.Scm.Utils
{
    /// <summary>
    /// PDF文档公共方法
    /// </summary>
    public static class PdfUtils
    {
        /// <summary>
        /// 获取pdf文档的页数
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>-1表示文件不存在</returns>
        public static int GetPdfOfPageCount(string filePath)
        {
            int count = -1; //-1表示文件不存在
            if (!File.Exists(filePath)) return count;
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fs);
            //从流的当前位置到末尾读取流
            string pdfText = reader.ReadToEnd();
            Regex rgx = new Regex(@"/Type\s*/Page[^s]");
            MatchCollection matches = rgx.Matches(pdfText);
            count = matches.Count;
            return count;
        }
    }
}