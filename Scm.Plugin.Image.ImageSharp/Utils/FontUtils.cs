using SixLabors.Fonts;

namespace Com.Scm.Utils
{
    internal static class FontUtils
    {
        // 全局可用的字体集合
        public static readonly FontCollection Collection = new();

        public static string DefaultFontName { get; private set; }

        /// <summary>
        /// 从指定目录加载所有字体文件
        /// </summary>
        public static void LoadFontsFromDirectory(string fontDirectory)
        {
            if (!Directory.Exists(fontDirectory))
            {
                return;
            }

            // 加载 ttf/ttc/otf
            var fontFiles = Directory.EnumerateFiles(fontDirectory, "*.*", SearchOption.TopDirectoryOnly)
                .Where(f => f.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase)
                         || f.EndsWith(".ttc", StringComparison.OrdinalIgnoreCase)
                         || f.EndsWith(".otf", StringComparison.OrdinalIgnoreCase));

            foreach (var file in fontFiles)
            {
                try
                {
                    Collection.Add(file);
                    Console.WriteLine($"已加载字体：{Path.GetFileName(file)}");
                }
                catch
                {
                    Console.WriteLine($"无效字体文件：{file}");
                }
            }
        }

        public static void AddFont(Stream stream)
        {
            try
            {
                Collection.Add(stream);
                Console.WriteLine("已加载字体流");
            }
            catch
            {
                Console.WriteLine("无效字体流");
            }
        }

        public static void SetDefaultFontName(string fontName)
        {
            DefaultFontName = fontName;
        }

        /// <summary>
        /// 获取字体（安全降级）
        /// </summary>
        public static Font GetFont(string fontFamily, float size, FontStyle style = FontStyle.Regular)
        {
            if (Collection.TryGet(GetValidFontName(fontFamily), out var family))
            {
                return family.CreateFont(size, style);
            }

            // 找不到就用第一个加载的字体兜底
            return Collection.Families.First().CreateFont(size, style);
        }

        public static string GetValidFontName(string name)
        {
            return name ?? DefaultFontName;
        }
    }

}
