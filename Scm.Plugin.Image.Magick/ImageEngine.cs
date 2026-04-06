using Com.Scm.Image.Avatar;
using Com.Scm.Image.Barcode;
using Com.Scm.Image.Captcha;
using Com.Scm.Image.Enums;
using Com.Scm.Image.WaterMark;
using Com.Scm.Plugin;
using Com.Scm.Plugin.Image;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;

namespace Com.Scm.Image.Magick
{
    public class ImageEngine : IPluginImage
    {
        public PluginType Type { get { return PluginType.Image; } }

        public string Name { get { return "Magick"; } }

        private static Dictionary<string, bool> _ImgExts;

        public ImageEngine()
        {
        }

        public bool IsImageFile(string ext)
        {
            return true;
        }

        public bool IsReadableFile(string ext)
        {
            return true;
        }

        public List<FileExt> GetReadableExts()
        {
            return null;
        }

        /// <summary>
        /// 是否所有支持的图片文件
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public bool IsWritableFile(string ext)
        {
            if (_ImgExts == null)
            {
                _ImgExts = new Dictionary<string, bool>();
                string[] nameList = System.Enum.GetNames(typeof(MagickFormat));
                foreach (var name in nameList)
                {
                    _ImgExts["." + name.ToLower()] = true;
                }
                _ImgExts[".jfif"] = true;
                //if (_ImgExts.ContainsKey(".pdf"))
                {
                    _ImgExts.Remove(".pdf");
                    _ImgExts.Remove(".txt");
                }
            }

            if (string.IsNullOrWhiteSpace(ext))
            {
                return false;
            }

            if (ext[0] != '.')
            {
                ext = '.' + ext;
            }
            return _ImgExts.ContainsKey(ext.ToLower());
        }

        public List<FileExt> GetWritableExts()
        {
            return null;
        }

        #region 操作Stream
        public ScmImageMeta GetImageInfo(Stream stream)
        {
            stream.Position = 0;

            var info = new MagickImageInfo();
            info.Read(stream);

            return new ScmImageMeta { Width = (int)info.Width, Height = (int)info.Height, Format = info.Format.ToString() };
        }

        public List<ImageColor> GetImageColor(Stream stream, int count, int delta = 16)
        {
            stream.Position = 0;

            var image = new MagickImage();
            image.Read(stream);

            //return Analyser.Analysis(image.ToBitmap(), count, delta);
            return null;
        }
        #endregion

        #region 操作Image对象
        private PluginImage _Image;
        public bool ReadImage(string file)
        {
            if (_Image != null)
            {
                _Image.Stop();
            }

            _Image = PluginImage.Load(file);
            return true;
        }

        public bool ReadImage(Stream stream)
        {
            stream.Position = 0;
            //return new MagickImage(stream);
            return true;
        }

        /// <summary>
        /// 格式转换
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool Convert(ScmImageFormat format)
        {
            if (_Image != null)
            {
                return _Image.Convert(format);
            }
            return false;
        }

        public bool Convert(string format)
        {
            return true;
        }

        public void Resize(int width, int height)
        {
            if (_Image != null)
            {
                _Image.Resize(width, height);
            }
        }

        public bool Save(string file, ScmImageFormat format)
        {
            if (_Image != null)
            {
                _Image.Save(file);
            }

            return true;
        }

        public bool Save(Stream stream, ScmImageFormat format)
        {
            throw new NotImplementedException();
        }

        public void Rotate(int degrees)
        {
            if (_Image != null)
            {
                _Image.Rotate(degrees);
            }
        }

        public void Flip(FlipOption option)
        {
            if (_Image != null)
            {
                _Image.Flip(option);
            }
        }

        public void Flop(FlopOption option)
        {
            if (_Image != null)
            {
                _Image.Flop(option);
            }
        }

        public int ImageWidth { get { return _Image.Width; } }

        public int ImageHeight { get { return _Image.Height; } }

        public double PixelWidth { get { return _Image.VisualWidth; } }

        public double PixelHeight { get { return _Image.VisualHeight; } }
        #endregion

        #region 缩略图
        //public BitmapSource GetBitmapSourceThumb(FileInfo fileInfo, byte quality = 100, int size = 500, bool checkSize = false)
        //{
        //    if (!fileInfo.Exists)
        //    {
        //        return null;
        //    }

        //    switch (fileInfo.Extension)
        //    {
        //        case ".bmp":
        //        case ".gif":
        //        case ".ico":
        //        case ".jfif":
        //        case ".jpe":
        //        case ".jpg":
        //        case ".jpeg":
        //        case ".png":
        //        case ".tif":
        //        case ".tiff":
        //        case ".webp":
        //        case ".wmp":
        //            return GetWindowsThumbnail(fileInfo.FullName);
        //    }

        //    if (checkSize)
        //    {
        //        return fileInfo.Length > 5e+6 ? null : GetMagickImageThumb(fileInfo, quality, size);
        //    }

        //    return GetMagickImageThumb(fileInfo, quality, size);
        //}

        //private static BitmapSource GetMagickImageThumb(FileInfo fileInfo, byte quality = 100, int size = 500)
        //{
        //    var magickImage = new MagickImage()
        //    {
        //        Quality = quality,
        //        ColorSpace = ColorSpace.Transparent
        //    };
        //    try
        //    {
        //        magickImage.Read(fileInfo);
        //        var exifData = magickImage.GetExifProfile();
        //        if (exifData != null)
        //        {
        //            // Create thumbnail from exif information
        //            var thumbnail = exifData.CreateThumbnail();
        //            // Check if exif profile contains thumbnail and save it
        //            var bitmapSource = thumbnail.ToBitmapSource();
        //            bitmapSource.Freeze();
        //            return bitmapSource;
        //        }

        //        magickImage.AdaptiveResize(size, size);

        //        var pic = magickImage.ToBitmapSource();
        //        pic.Freeze();
        //        return pic;
        //    }
        //    catch (Exception e)
        //    {
        //        LogFactory.GetLogger("WinForm").Error(e);
        //        return null;
        //    }
        //}

        //private static BitmapSource GetWindowsThumbnail(string path)
        //{
        //    BitmapSource pic = ShellFile.FromFilePath(path).Thumbnail.BitmapSource;
        //    pic.Freeze();
        //    return pic;
        //}
        #endregion

        public void WaterMark(WaterMarkOption option)
        {
            //_Image.WaterMark(option);
        }

        public IImage Load(string file)
        {
            throw new NotImplementedException();
        }

        public IImage Load(Stream stream)
        {
            throw new NotImplementedException();
        }

        public IImage Load(Stream stream, ScmImageFormat format)
        {
            throw new NotImplementedException();
        }

        public IImage Thumbnail(string file)
        {
            throw new NotImplementedException();
        }

        public IImage GenBarcode(string text, int format, int width, int height)
        {
            throw new NotImplementedException();
        }

        public IImage GenCaptcha()
        {
            throw new NotImplementedException();
        }

        public IImage GenBarcode(IImage image, string text, PositionEnum position, string font, int size)
        {
            throw new NotImplementedException();
        }

        public CaptchaResult GenCaptcha(CaptchaOption option = null)
        {
            throw new NotImplementedException();
        }

        public AvatarResult GenAvatar(AvatarOption option = null)
        {
            throw new NotImplementedException();
        }
    }
}