using Com.Scm.Image.Avatar;
using Com.Scm.Image.Barcode;
using Com.Scm.Image.Captcha;
using Com.Scm.Image.Enums;
using Com.Scm.Image.WaterMark;
using System;
using System.Collections.Generic;
using System.IO;

namespace Com.Scm.Plugin.Image
{
    public abstract class ScmImage : IImage
    {
        protected bool _LoadOk;

        public PluginType Type { get { return PluginType.Image; } }

        #region 属性
        /// <summary>
        /// 像素宽度
        /// </summary>
        public abstract int Width { get; }

        /// <summary>
        /// 像素高度
        /// </summary>
        public abstract int Height { get; }

        /// <summary>
        /// 屏幕宽度
        /// </summary>
        public abstract double VisualWidth { get; }

        /// <summary>
        /// 屏幕高度
        /// </summary>
        public abstract double VisualHeight { get; }

        public abstract int Count { get; }

        public string Path { get; set; }

        public string Name { get; set; }

        public string Format { get; set; }
        #endregion

        #region 存取
        public abstract bool Read(string file);

        public abstract bool Read(Stream stream);

        public abstract bool Read(Uri file);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public abstract bool Save(string file);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public abstract bool Save(Stream stream);
        #endregion

        #region 操作
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public abstract bool Convert(string format);
        public abstract bool Convert(ScmImageFormat format);

        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public abstract void Resize(int width, int height);

        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="scaleX"></param>
        /// <param name="scaleY"></param>
        public abstract void Scale(double scaleX, double scaleY);

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="degrees"></param>
        public abstract void Rotate(int degrees);

        /// <summary>
        /// 垂直翻转
        /// </summary>
        public abstract void Flip(FlipOption option);

        /// <summary>
        /// 水平翻转
        /// </summary>
        public abstract void Flop(FlopOption option);

        /// <summary>
        /// 裁剪
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public abstract void Crop(int x, int y, int width, int height);

        /// <summary>
        /// 获取图像颜色
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="count"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public abstract List<ImageColor> GetImageColor(int count, int delta = 16);
        #endregion

        #region 展示
        /// <summary>
        /// 显示图片
        /// </summary>
        public virtual void Show()
        {
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public virtual void Suspend()
        {

        }

        /// <summary>
        /// 重新开始
        /// </summary>
        public virtual void Restart()
        {

        }

        /// <summary>
        /// 停止
        /// </summary>
        public virtual void Stop()
        {

        }
        #endregion

        #region 帧
        public abstract void AddFrame(string file);

        public abstract void AddFrameBySize(string file, int size);

        public abstract void AddFrameBySize(string file, int width, int height);

        public abstract void AddFrame(Stream stream);

        public abstract void AddFrameBySize(Stream stream, int size);

        public abstract void AddFrameBySize(Stream stream, int width, int height);

        public abstract void SetFrame(int index, string file);

        public abstract void SetFrame(int index, Stream stream);

        public abstract void RemoveFrame(int index);

        public abstract IFrame GetFrame(int index);

        public abstract IFrame GetFirstFrame();

        public abstract IFrame GetDefaultFrame();

        public abstract IFrame GetFrameBySize(int size);

        public abstract IFrame GetFrameBySize(int width, int height);
        #endregion

        #region 应用
        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="format">条码格式</param>
        /// <param name="width">条码宽度(像素)</param>
        /// <param name="height">条码高度(像素)</param>
        /// <returns></returns>
        public abstract IFrame GenBarcode(string text, int format, int width, int height);

        /// <summary>
        /// 生成带文字的图像
        /// </summary>
        /// <param name="image"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="font"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public abstract IFrame GenBarcode(IFrame image, string text, PositionEnum position, string font, int size);

        /// <summary>
        /// 生成识别码
        /// </summary>
        /// <returns></returns>
        public abstract CaptchaResult GenCaptcha(CaptchaOption option = null);

        /// <summary>
        /// 默认头像
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public abstract AvatarResult GenAvatar(AvatarOption option = null);

        /// <summary>
        /// 生成水印
        /// </summary>
        /// <param name="option"></param>
        public abstract void WaterMark(WaterMarkOption option);
        #endregion
    }
}
