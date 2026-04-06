using System;

namespace Com.Scm.Audio
{
    public class ScmAudioMeta
    {
        /// <summary>
        /// 音频格式
        /// </summary>
        public string Format { get; set; }

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
