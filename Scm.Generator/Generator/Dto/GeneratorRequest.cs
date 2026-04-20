namespace Com.Scm.Generator.Dto
{
    /// <summary>
    /// 连接对象
    /// </summary>
    public class GeneratorRequest
    {
        public GeneratorRequest(string ip, string port, string name, string passWord, string dbName)
        {
            Ip = ip;
            Port = port;
            Name = name;
            PassWord = passWord;
            DbName = dbName;
        }

        public string Ip { get; set; }

        public string Port { get; set; }

        public string Name { get; set; }

        public string PassWord { get; set; }

        public string DbName { get; set; }
    }
}
