using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmDevDbDto : ScmDataDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(128)]
        public string namec { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        [StringLength(256)]
        public string host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int port { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        [StringLength(128)]
        public string schame { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [StringLength(32)]
        public string user { get; set; }

        /// <summary>
        /// 字符集
        /// </summary>
        [StringLength(32)]
        public string charset { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }
    }
}