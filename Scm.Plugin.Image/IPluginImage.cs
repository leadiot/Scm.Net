using Com.Scm.Image;
using System;
using System.Collections.Generic;
using System.IO;

namespace Com.Scm.Plugin.Image
{
    public interface IPluginImage : IPlugin
    {
        /// <summary>
        /// 是否为图片文件
        /// </summary>
        /// <param name="exts"></param>
        /// <returns></returns>
        bool IsImageFile(string exts);

        /// <summary>
        /// 是否为可读图片文件
        /// </summary>
        /// <param name="exts"></param>
        /// <returns></returns>
        bool IsReadableFile(string exts);

        List<FileExt> GetReadableExts();

        /// <summary>
        /// 是否为可写图片文件
        /// </summary>
        /// <param name="exts"></param>
        /// <returns></returns>
        bool IsWritableFile(string exts);

        List<FileExt> GetWritableExts();

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        IImage Load(string file);

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [Obsolete("此方法已作废")]
        IImage Load(Stream stream);
        IImage Load(Stream stream, ScmImageFormat format);

        /// <summary>
        /// 格式转换
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        bool Convert(string format);

        /// <summary>
        /// 缩略图
        /// </summary>
        /// <returns></returns>
        IImage Thumbnail(string file);

        /// <summary>
        /// 获取图像信息
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        ScmImageMeta GetImageInfo(Stream stream);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="file"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        bool Save(string file, ScmImageFormat format);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        bool Save(Stream stream, ScmImageFormat format);
    }
}
