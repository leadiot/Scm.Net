namespace Com.Scm.Config
{
    public class CorsConfig
    {
        public const string NAME = "Cors";

        public bool GlobalCors { get; set; }

        public bool AllowAnyOrigin { get; set; }
        public string[] AllowedOrigins { get; set; }

        public bool AllowAnyMethod { get; set; }
        public string[] AllowedMethods { get; set; }

        public bool AllowAnyHeader { get; set; }
        public string[] AllowedHeaders { get; set; }

        public bool AllowCredentials { get; set; }

        public string[] ExposedHeaders { get; set; }

        public int PreflightMaxAge { get; set; } = 1;

        public void Prepare(EnvConfig config)
        {
            if (AllowedOrigins == null)
            {
                AllowedOrigins = Array.Empty<string>();
            }
            if (AllowedMethods == null)
            {
                AllowedMethods = Array.Empty<string>();
            }
            if (AllowedHeaders == null)
            {
                AllowedHeaders = Array.Empty<string>();
            }
            if (ExposedHeaders == null)
            {
                ExposedHeaders = Array.Empty<string>();
            }
            if (PreflightMaxAge < 1)
            {
                PreflightMaxAge = 1;
            }
        }
    }
}
