using Com.Scm.Request;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace Com.Scm
{
    public class ScmUploadRequest : ScmRequest
    {
        /// <summary>
        /// 上传方式
        /// 必选
        /// </summary>
        public UploadTypeEnum type { get; set; }

        /// <summary>
        /// 上传文件
        /// 必选
        /// </summary>
        public IFormFile file { get; set; }

        /// <summary>
        /// 上传路径，适用于上传到指定路径的场景
        /// 可选，默认为根路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 版本摘要，适用于摘要上传的场景
        /// </summary>
        public string hash { get; set; }

        #region 其它参数
        /// <summary>
        /// 文件名称
        /// </summary>
        public string file_name { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long file_size { get; set; }
        /// <summary>
        /// 片段总数
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 片段名称
        /// </summary>
        public string part_name { get; set; }
        /// <summary>
        /// 片段大小
        /// </summary>
        public long part_size { get; set; }
        /// <summary>
        /// 自然索引
        /// </summary>
        public int index { get; set; }
        #endregion
    }

    /// <summary>
    /// 上传方式
    /// </summary>
    public enum UploadTypeEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// 文件上传
        /// </summary>
        [Description("文件上传")]
        ByFile = 1,
        /// <summary>
        /// 分段上传
        /// </summary>
        [Description("分段上传")]
        ByPart = 2,
        /// <summary>
        /// 摘要上传
        /// </summary>
        [Description("摘要上传")]
        ByHash = 3,
    }
}
