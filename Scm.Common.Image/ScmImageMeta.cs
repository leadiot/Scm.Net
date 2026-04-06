using System;

namespace Com.Scm.Image
{
    public class ScmImageMeta
    {
        /// <summary>
        /// 图像格式
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 图像宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 图像高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }
    }
}
