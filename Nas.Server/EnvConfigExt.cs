using Com.Scm.Config;

namespace Com.Scm.Nas
{
    public static class EnvConfigExt
    {
        public static string GetNasPath(this EnvConfig config, string userCodes, string path)
        {
            return config.GetDataPath($"/{NasEnv.DEF_NAS_DIR}/{userCodes}" + path);
        }

        public static string GetNasDownloadPath(this EnvConfig config, string userCodes, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return config.GetDataPath($"/{NasEnv.DEF_NAS_DIR}/{userCodes}");
            }

            return config.GetDataPath($"/{NasEnv.DEF_NAS_DIR}/{userCodes}" + path);
        }
    }
}
