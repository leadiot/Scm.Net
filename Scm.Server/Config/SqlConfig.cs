namespace Com.Scm.Config
{
    public class SqlConfig
    {
        public const string NAME = "Sql";

        public string Type { get; set; }
        public string Text { get; set; }

        public void Prepare(EnvConfig config)
        {
        }
    }
}
