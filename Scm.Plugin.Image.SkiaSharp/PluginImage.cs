using Com.Scm.Image.SkiaSharp.Formats.Bit;
using Com.Scm.Image.SkiaSharp.Formats.Gif;
using Com.Scm.Image.SkiaSharp.Formats.Ico;
using Com.Scm.Image.WaterMark;
using Com.Scm.Plugin.Image;
using Com.Scm.Utils;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace Com.Scm.Image.SkiaSharp
{
    public abstract class PluginImage : ScmImage
    {
        public List<PluginFrame> Frames { get; private set; }
        /// <summary>
        /// 当前帧
        /// </summary>
        protected PluginFrame _Frame;

        public PluginImage()
        {
            Frames = new List<PluginFrame>();
        }

        public static PluginImage CreateInstance(ScmImageFormat format)
        {
            if (format == ScmImageFormat.Ico)
            {
                return new IcoImage();
            }

            if (format == ScmImageFormat.Gif)
            {
                return new GifImage();
            }

            return new BitImage();
        }

        public static PluginImage Load(string file)
        {
            var ext = System.IO.Path.GetExtension(file)?.ToLower();

            PluginImage image;
            if (ext == ".ico")
            {
                image = new IcoImage();
            }
            else if (ext == ".gif")
            {
                image = new GifImage();
            }
            else
            {
                image = new BitImage();
            }

            image.Read(file);
            return image;
        }

        public static PluginImage Load(Stream stream)
        {
            var bytes = new byte[32];
            stream.Read(bytes, 0, bytes.Length);
            var sign = TextUtils.ToHexString(bytes);
            stream.Position = 0;

            PluginImage image;
            if (sign.StartsWith("0000010000"))
            {
                image = new IcoImage();
            }
            else if (sign.StartsWith("47494638"))
            {
                image = new GifImage();
            }
            else
            {
                image = new BitImage();
            }

            image.Read(stream);
            return image;
        }

        public static PluginImage Load(Stream stream, ScmImageFormat format)
        {
            PluginImage image;
            if (format == ScmImageFormat.Ico)
            {
                image = new IcoImage();
            }
            else if (format == ScmImageFormat.Gif)
            {
                image = new GifImage();
            }
            else
            {
                image = new BitImage();
            }

            image.Read(stream);
            return image;
        }

        public override bool Convert(string format)
        {
            switch (format.ToLower())
            {
                case ".jpg":
                    return Convert(ScmImageFormat.Jpg);
                case ".png":
                    return Convert(ScmImageFormat.Png);
                case ".bmp":
                    return Convert(ScmImageFormat.Bmp);
                case ".gif":
                    return Convert(ScmImageFormat.Gif);
                case ".ico":
                    return Convert(ScmImageFormat.Ico);
                default:
                    return false;
            }
        }

        protected abstract SKBitmap DefaultImage();

        public override int Width
        {
            get
            {
                var image = DefaultImage();
                return image != null ? image.Width : 0;
            }
        }

        public override int Height
        {
            get
            {
                var image = DefaultImage();
                return image != null ? image.Height : 0;
            }
        }

        public override double VisualWidth
        {
            get
            {
                var image = DefaultImage();
                return image != null ? image.Width : 0;
            }
        }

        public override double VisualHeight
        {
            get
            {
                var image = DefaultImage();
                return image != null ? image.Height : 0;
            }
        }

        /// <summary>
        /// 垂直旋转
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        protected SKBitmap Flip(SKBitmap bitmap)
        {
            //var tmp = new SKBitmap(bitmap.Height, bitmap.Width);

            //var degrees = option == FlipOption.Clockwise ? -90 : 90;
            //using (var surface = new SKCanvas(tmp))
            //{
            //    surface.Translate(tmp.Width, 0);
            //    surface.RotateDegrees(degrees);
            //    surface.DrawBitmap(bitmap, 0, 0);
            //}

            //return tmp;
            var tmp = new SKBitmap(bitmap.Width, bitmap.Height);

            using (var surface = new SKCanvas(tmp))
            {
                surface.Translate(0, bitmap.Height);
                surface.Scale(1, -1);

                surface.DrawBitmap(bitmap, 0, 0);
            }

            return tmp;
        }

        /// <summary>
        /// 水平翻转
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        protected SKBitmap Flop(SKBitmap bitmap)
        {
            var tmp = new SKBitmap(bitmap.Width, bitmap.Height);

            using (var surface = new SKCanvas(tmp))
            {
                surface.Translate(bitmap.Width, 0);
                surface.Scale(-1, 1);

                surface.DrawBitmap(bitmap, 0, 0);
            }

            return tmp;
        }

        /// <summary>
        /// 任意角度旋转
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        protected SKBitmap Rotate(SKBitmap bitmap, double angle)
        {
            double radians = Math.PI * angle / 180;
            float sine = (float)Math.Abs(Math.Sin(radians));
            float cosine = (float)Math.Abs(Math.Cos(radians));
            int originalWidth = bitmap.Width;
            int originalHeight = bitmap.Height;
            int rotatedWidth = (int)(cosine * originalWidth + sine * originalHeight);
            int rotatedHeight = (int)(cosine * originalHeight + sine * originalWidth);

            var tmp = new SKBitmap(rotatedWidth, rotatedHeight);

            using (var surface = new SKCanvas(tmp))
            {
                surface.Translate(rotatedWidth / 2, rotatedHeight / 2);
                surface.RotateDegrees((float)angle);
                surface.Translate(-originalWidth / 2, -originalHeight / 2);
                surface.DrawBitmap(bitmap, new SKPoint());
            }

            return tmp;
        }

        protected SKBitmap Resize(SKBitmap bitmap, int width, int height)
        {
            var tmp = new SKBitmap(width, height);

            using (var surface = new SKCanvas(tmp))
            {
                var rect = new SKRect(0, 0, width, height);
                surface.DrawBitmap(bitmap, rect);
            }

            return tmp;
        }

        protected SKBitmap Scale(SKBitmap bitmap, double scaleX, double scaleY)
        {
            //var tmp = new SKBitmap(width, height);

            //using (var surface = new SKCanvas(tmp))
            //{
            //    var rect = new SKRect(0, 0, width, height);
            //    surface.DrawBitmap(bitmap, rect);
            //}

            return bitmap;
        }

        protected bool Save(SKBitmap bitmap, Stream stream, ScmImageFormat format)
        {
            SKEncodedImageFormat fmt;
            switch (format)
            {
                case ScmImageFormat.Bmp:
                    fmt = SKEncodedImageFormat.Bmp;
                    break;
                case ScmImageFormat.Jpg:
                    fmt = SKEncodedImageFormat.Jpeg;
                    break;
                case ScmImageFormat.Gif:
                    fmt = SKEncodedImageFormat.Gif;
                    break;
                case ScmImageFormat.Png:
                    fmt = SKEncodedImageFormat.Png;
                    break;
                case ScmImageFormat.Ico:
                    fmt = SKEncodedImageFormat.Ico;
                    break;
                default:
                    return false;
            }

            bitmap.Encode(stream, fmt, 100);
            return true;
        }

        public override List<ImageColor> GetImageColor(int count, int delta = 16)
        {
            return null;
        }

        public override int Count { get { return Frames.Count; } }

        public void AddFrame(SKBitmap bitmap)
        {
            Frames.Add(new PluginFrame(bitmap));
        }

        public override void WaterMark(WaterMarkOption option)
        {
        }

        #region 生成水印
        protected static SKBitmap WaterMark(SKBitmap image, WaterMarkOption option)
        {
            var water = SKBitmap.Decode(option.ImagePath);
            using (var surface = new SKCanvas(water))
            {
                var paint = new SKPaint();
                paint.Color = SKColors.Black.WithAlpha((byte)(option.Opaque * 255));
                surface.DrawBitmap(water, 0, 0, paint);
            }

            switch (option.WaterMarkStyle)
            {
                case WaterMarkStyleEnum.Fill:
                    image = WaterMarkFill(image, water, option);
                    break;
                case WaterMarkStyleEnum.Repeat:
                    image = WaterMarkRepeat(image, water, option);
                    break;
                case WaterMarkStyleEnum.Fixed:
                    image = WaterMarkFixed(image, water, option);
                    break;
            }

            return image;
        }

        protected static SKBitmap WaterMarkFill(SKBitmap image, SKBitmap water, WaterMarkOption option)
        {
            var x = (int)option.Margin.Left;
            var y = (int)option.Margin.Top;

            var tmp = new SKBitmap(image.Width, image.Height);
            using (var surface = new SKCanvas(tmp))
            {
                surface.DrawBitmap(water, x, y);
            }
            return tmp;
        }

        protected static SKBitmap WaterMarkRepeat(SKBitmap image, SKBitmap water, WaterMarkOption option)
        {
            var margin = option.Margin;

            var x = (int)margin.Left;
            var y = (int)margin.Top;
            var right = image.Width - margin.Right;
            var bottom = image.Height - margin.Bottom;

            var tmp = new SKBitmap(image.Width, image.Height);
            using (var surface = new SKCanvas(tmp))
            {
                while (y < bottom)
                {
                    while (x < right)
                    {
                        surface.DrawBitmap(water, x, y);
                        x += water.Width;
                    }
                    y += water.Height;
                }
            }
            return tmp;
        }

        protected static SKBitmap WaterMarkFixed(SKBitmap image, SKBitmap water, WaterMarkOption option)
        {
            var margin = option.Margin;

            double x = 0;
            double y = 0;
            switch (option.WaterMarkLocation)
            {
                case WaterMarkLocationEnum.TopLeft:
                    x = margin.Left;
                    y = margin.Top;
                    break;
                case WaterMarkLocationEnum.TopCenter:
                    x = (margin.Left + image.Width - margin.Right) / 2;
                    y = margin.Top;
                    break;
                case WaterMarkLocationEnum.TopRight:
                    x = image.Width - margin.Right - water.Width;
                    y = margin.Top;
                    break;
                case WaterMarkLocationEnum.CenterLeft:
                    x = margin.Left;
                    y = (margin.Top + image.Height - margin.Bottom) / 2;
                    break;
                case WaterMarkLocationEnum.CenterCenter:
                    x = (margin.Left + image.Width - margin.Right) / 2;
                    y = (margin.Top + image.Height - margin.Bottom) / 2;
                    break;
                case WaterMarkLocationEnum.CenterRight:
                    x = image.Width - margin.Right - water.Width;
                    y = (margin.Top + image.Height - margin.Bottom) / 2;
                    break;
                case WaterMarkLocationEnum.BottomLeft:
                    x = margin.Left;
                    y = image.Height - margin.Bottom;
                    break;
                case WaterMarkLocationEnum.BottomCenter:
                    x = (margin.Left + image.Width - margin.Right) / 2;
                    y = image.Height - margin.Bottom;
                    break;
                case WaterMarkLocationEnum.BottomRight:
                    x = image.Width - margin.Right - water.Width;
                    y = image.Height - margin.Bottom;
                    break;
            }

            var tmp = new SKBitmap(image.Width, image.Height);
            using (var surface = new SKCanvas(tmp))
            {
                surface.DrawBitmap(water, (float)x, (float)y);
            }
            return tmp;
        }
        #endregion

        #region 色彩分析
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="length">主色数量</param>
        /// <param name="delta">色块大小</param>
        /// <param name="scanRow">采样间隔</param>
        /// <returns></returns>
        public static List<ImageColor> GetMajorColor(SKBitmap image, int length, int delta = 16, int scanRow = 1)
        {
            var width = image.Width;
            var height = image.Height;

            var bytes = image.Pixels;

            return GetMajorColor(bytes, width, height, length, delta, scanRow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="width">图像宽度</param>
        /// <param name="height">图像高度</param>
        /// <param name="length">主色数量</param>
        /// <param name="delta">色块大小</param>
        /// <param name="scanRow">采样间隔</param>
        /// <returns></returns>
        public static List<ImageColor> GetMajorColor(SKColor[] bytes, int width, int height, int length, int delta = 16, int scanRow = 1)
        {
            // 颜色映射
            int[] colorMap = new int[256];
            if ((delta & 1) == 1)
            {
                delta += 1;
            }
            int half = delta >> 1;
            //delta -= 1;

            for (var i = 0; i < 256; i++)
            {
                var t = ((i + half) / delta) * delta;
                if (t > 255)
                {
                    t = 255;
                }
                colorMap[i] = t;
            }

            if (scanRow < 1)
            {
                scanRow = 1;
            }

            // 颜色总数量
            var amount = 0;
            var colorQty = new int[256 * 256 * 256];
            var colorIdx = new int[width * height];
            SKColor pixel;

            var idxrow = 0;
            for (var y = 0; y < height; y += scanRow)
            {
                for (var x = 0; x < width; x++)
                {
                    pixel = bytes[idxrow + x];
                    //var rgba = (Map[pixel.A] << 24) + (Map[pixel.R] << 16) + (Map[pixel.G] << 8) + Map[pixel.B];
                    int offset = (colorMap[pixel.Red] << 16) + (colorMap[pixel.Green] << 8) + colorMap[pixel.Blue];

                    if (colorQty[offset] == 0)
                    {
                        colorIdx[amount++] = offset;
                    }
                    colorQty[offset]++;
                }
                idxrow += width * scanRow;
            }

            var array = new ImageColor[amount];
            for (var i = 0; i < amount; i++)
            {
                var idx = colorIdx[i];
                var tmp = new ImageColor(idx, colorQty[idx]);
                array[i] = tmp;
            }
            Array.Sort(array, (a, b) => { return b.Amount.CompareTo(a.Amount); });

            if (length > amount)
            {
                length = amount;
            }
            var result = new List<ImageColor>(length);
            for (var i = 0; i < length; i++)
            {
                result.Add(array[i]);
            }

            GC.Collect();
            return result;
        }
        #endregion
    }
}
