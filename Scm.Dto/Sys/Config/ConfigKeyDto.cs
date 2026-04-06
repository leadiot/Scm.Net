using Com.Scm.Dto;
using Com.Scm.Enums;

namespace Com.Scm.Sys.Config
{
    public class ConfigKeyDto : ScmDataDto
    {
        /// <summary>
        /// 分类
        /// </summary>
        public long cat_id { get; set; }
        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }
        /// <summary>
        /// 键
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 终端类型
        /// </summary>
        public ScmClientTypeEnum client { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public ScmDataTypeEnum date { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
}
