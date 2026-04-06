using Com.Scm.Barcode;
using Com.Scm.Barcode.Zxing;
using Com.Scm.Captcha;
using Com.Scm.Image.Avatar;
using Com.Scm.Image.Barcode;
using Com.Scm.Image.Captcha;
using Com.Scm.Image.SkiaSharp.Formats.Bit;
using Com.Scm.Image.WaterMark;
using Com.Scm.Plugin;
using Com.Scm.Plugin.Image;
using SkiaSharp;
using System.Collections.Generic;
using System.IO;

namespace Com.Scm.Image.SkiaSharp
{
    public class ImageEngine : IPluginImage
    {
        public PluginType Type { get { return PluginType.Image; } }

        public string Name { get { return "SkiaSharp"; } }

        private static Dictionary<string, bool> _WritableExts;

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
            if (string.IsNullOrWhiteSpace(ext))
            {
                return false;
            }

            if (_WritableExts == null)
            {
                _WritableExts = new Dictionary<string, bool>();
                string[] nameList = System.Enum.GetNames(typeof(SKEncodedImageFormat));
                foreach (var name in nameList)
                {
                    _WritableExts["." + name.ToLower()] = true;
                }
                _WritableExts[".jfif"] = true;
                if (_WritableExts.ContainsKey(".pdf"))
                {
                    _WritableExts.Remove(".pdf");
                    _WritableExts.Remove(".txt");
                }
            }

            if (ext[0] != '.')
            {
                ext = '.' + ext;
            }
            return _WritableExts.ContainsKey(ext.ToLower());
        }

        public List<FileExt> GetWritableExts()
        {
            return null;
        }

        public ScmImageMeta GetImageInfo(Stream stream)
        {
            stream.Position = 0;

            return new ScmImageMeta();
        }

        public IImage Load(string file)
        {
            return PluginImage.Load(file);
        }

        public IImage Load(Stream stream)
        {
            return PluginImage.Load(stream);
        }

        public IImage Load(Stream stream, ScmImageFormat format)
        {
            return PluginImage.Load(stream, format);
        }

        public IImage Thumbnail(string file)
        {
            return null;
        }

        public IImage GenBarcode(string text, int format, int width, int height)
        {
            var barcode = GetBarcodeInstance();
            var bitmap = barcode.GenBar(text, format, width, height);
            var img = new BitImage();
            img.AddFrame(bitmap);

            return img;
        }

        public IImage GenBarcode(IImage image, string text, PositionEnum position, string font, int size)
        {
            var barcode = GetBarcodeInstance();
            for (var i = 0; i < image.Count; i += 1)
            {
                var frame = (PluginFrame)image.GetFrame(i);
                frame.Image = barcode.GenBar(frame.Image, text, position, font, size);
            }
            return image;
        }

        //private static IBarcode _Barcode;
        public static IBarcode GetBarcodeInstance()
        {
            //if (_Barcode == null)
            //{
            //    _Barcode = new ZxingBarcode();
            //}
            //return _Barcode;

            return new ZxingBarcode();
        }

        public CaptchaResult GenCaptcha(CaptchaOption option = null)
        {
            return GetCaptchaInstance().GenCaptcha(option);
        }

        public static ICaptcha GetCaptchaInstance()
        {
            return new ScmCaptcha();
        }

        private static Dictionary<int, AvatarResult> _Avatars = new Dictionary<int, AvatarResult>();
        public AvatarResult GenAvatar(AvatarOption option = null)
        {
            if (option == null)
            {
                option = new AvatarOption();
            }

            var size = option.Size;
            if (_Avatars.ContainsKey(size))
            {
                return _Avatars[size];
            }

            var result = new AvatarResult();

            var image = new SKBitmap(size, size);
            using (var canvas = new SKCanvas(image))
            {
                canvas.DrawColor(SKColors.LightGray);
            }

            using (var stream = new MemoryStream())
            {
                image.Encode(stream, SKEncodedImageFormat.Png, 100);
                result.Image = stream.ToArray();
            }

            return result;
        }

        public bool Convert(ScmImageFormat format)
        {
            return true;
        }

        public bool Convert(string format)
        {
            return true;
        }

        public bool Save(string file, ScmImageFormat format)
        {
            return true;
        }

        public bool Save(Stream stream, ScmImageFormat format)
        {
            return true;
        }

        public void WaterMark(WaterMarkOption option)
        {
        }
    }
}
