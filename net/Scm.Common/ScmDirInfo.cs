using System.Collections.Generic;

namespace Com.Scm.Utils
{
    /// <summary>
    /// 目录信息
    /// </summary>
    public class ScmDirInfo
    {
        public string Name { get; set; }

        public string Uri { get; set; }

        public List<ScmDirInfo> Children { get; set; }
    }
}
