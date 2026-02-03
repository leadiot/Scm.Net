using Com.Scm.Enums;
using System;

namespace Com.Scm.Utils
{
    /// <summary>
    /// 文件内容
    /// </summary>
    public class ScmFileInfo
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public ScmDocTypeEnum Type { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 文件物理路径
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// 原始大小（字节）
        /// </summary>
        public long Length { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }
        /// <summary>
        /// 文件虚拟路径
        /// </summary>
        public string Uri { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileStyle { get; set; }
        /// <summary>
        /// 文件图标
        /// </summary>
        public string FileIcon { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastWriteTime { get; set; }
        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastAccessTime { get; set; }
    }
}
