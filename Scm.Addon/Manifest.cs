using System.Reflection;

namespace Com.Scm.Addon
{
    public class Manifest
    {
        /// <summary>
        /// 插件类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// DLL文件
        /// </summary>
        public string dll { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 插件名称
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 插件说明
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 类路径
        /// </summary>
        public string uri { get; set; }

        /// <summary>
        /// 入口，预留
        /// </summary>
        public string entry { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string args { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string ver { get; set; }

        /// <summary>
        /// 是否单例模式
        /// </summary>
        public bool singleton { get; set; }

        /// <summary>
        /// 程序集
        /// </summary>
        [System.NonSerialized]
        public Assembly assembly;
        /// <summary>
        /// 实例对象
        /// </summary>
        [System.NonSerialized]
        public object instance;
        /// <summary>
        /// 工作目录
        /// </summary>
        [System.NonSerialized]
        public string dir;

        [System.NonSerialized]
        public bool sys;

        public void Parse()
        {
            sys = "system".Equals(dll, System.StringComparison.OrdinalIgnoreCase);
            if (sys)
            {
                assembly = Assembly.GetEntryAssembly();
            }
        }
    }
}
