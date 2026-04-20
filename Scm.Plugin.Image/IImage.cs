namespace Com.Scm.Plugin.Image
{
    public interface IImage
    {
        #region 帧
        /// <summary>
        /// 帧数
        /// </summary>
        int Count { get; }

        IFrame GetFrame(int index);

        IFrame GetDefaultFrame();
        #endregion

        /// <summary>
        /// 宽度
        /// </summary>
        int Width { get; }

        /// <summary>
        /// 高度
        /// </summary>
        int Height { get; }

        /// <summary>
        /// 图片路径
        /// </summary>
        string Path { get; }

        /// <summary>
        /// 图片名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 图片格式
        /// </summary>
        string Format { get; }
    }
}
