namespace Com.Scm.Config
{
    public class SwaggerConfig
    {
        public const string NAME = "Swagger";

        /// <summary>
        /// 
        /// </summary>
        public List<string> DllXmls { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ApiInfo> ApiDocs { get; set; }

        public void LoadDef()
        {
            DllXmls = new List<string>();
            ApiDocs = new List<ApiInfo>();
        }
    }

    public class ApiInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
