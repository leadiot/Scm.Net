namespace Com.Scm.Config
{
    public class AimlConfig
    {
        public const string NAME = "Aiml";

        /// <summary>
        /// 
        /// </summary>
        public string Folder { get; set; }

        public void Prepare(EnvConfig envConfig)
        {
            Folder = "Robot";
        }
    }
}
