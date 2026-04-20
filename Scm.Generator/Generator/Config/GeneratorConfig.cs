using Com.Scm.Config;

namespace Com.Scm.Generator.Config
{
    public class GeneratorConfig
    {
        public const string NAME = "Generator";

        /// <summary>
        /// 模板目录
        /// </summary>
        public string TemplatesDir { get; set; }
        /// <summary>
        /// 生成目录
        /// </summary>
        public string GeneratorDir { get; set; }

        /// <summary>
        /// 默认查询列
        /// </summary>
        public List<string> SearchEnabledColumns { get; set; }
        /// <summary>
        /// 结果忽略列
        /// </summary>
        public List<string> ResultIgnoredColumns { get; set; }
        /// <summary>
        /// 编辑忽略列
        /// </summary>
        public List<string> UpdateIgnoredColumns { get; set; }

        /// <summary>
        /// 是否生成文件
        /// </summary>
        public bool GenFiles { get; set; }
        /// <summary>
        /// 是否打包下载
        /// </summary>
        public bool Download { get; set; }

        public void Prepare(EnvConfig envConfig)
        {
            if (string.IsNullOrWhiteSpace(TemplatesDir))
            {
                TemplatesDir = "templates";
            }
            if (!Path.IsPathRooted(TemplatesDir))
            {
                TemplatesDir = envConfig.GetDataPath(TemplatesDir);
            }
            //if (!Directory.Exists(TemplatesDir))
            //{
            //    Directory.CreateDirectory(TemplatesDir);
            //}

            if (string.IsNullOrWhiteSpace(GeneratorDir))
            {
                GeneratorDir = "generator";
            }
            if (!Path.IsPathRooted(GeneratorDir))
            {
                GeneratorDir = envConfig.GetDataPath(GeneratorDir);
            }
            //if (!Directory.Exists(GeneratorDir))
            //{
            //    Directory.CreateDirectory(GeneratorDir);
            //}

            if (UpdateIgnoredColumns == null)
            {
                UpdateIgnoredColumns = new List<string>
                {
                    "id","row_status","update_time","update_user","create_time","create_user"
                };
            }
        }

        public bool IsIgnoreColumn(string colName)
        {
            return UpdateIgnoredColumns != null && UpdateIgnoredColumns.Contains(colName);
        }

        public string GetTemplatesFile(params string[] files)
        {
            if (files == null || files.Length == 0)
            {
                return TemplatesDir;
            }

            var path = Path.Combine(files);
            return Path.Combine(TemplatesDir, path);
        }

        public string GetGeneratorFile(params string[] files)
        {
            if (files == null || files.Length == 0)
            {
                return GeneratorDir;
            }

            var path = Path.Combine(files);
            return Path.Combine(GeneratorDir, path);
        }
    }
}
