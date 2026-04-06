using Com.Scm.Image.Magick.Formats.Bit;
using Com.Scm.Image.Magick.Formats.Gif;
using Com.Scm.Image.Magick.Formats.Ico;
using Com.Scm.Image.WaterMark;
using Com.Scm.Plugin.Image;
using ImageMagick;
using System.Collections.Generic;

namespace Com.Scm.Image.Magick
{
    public abstract class PluginImage : ScmImage
    {
        public List<PluginFrame> Frames { get; private set; }

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
            PluginImage image;

            var ext = System.IO.Path.GetExtension(file)?.ToLower();
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

        public override bool Convert(string format)
        {
            return true;
        }

        protected abstract MagickImage DefaultImage();

        public override int Width
        {
            get
            {
                var image = DefaultImage();
                return image != null ? (int)image.Width : 0;
            }
        }

        public override int Height
        {
            get
            {
                var image = DefaultImage();
                return image != null ? (int)image.Height : 0;
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

        public override List<ImageColor> GetImageColor(int count, int delta = 16)
        {
            return null;
        }

        protected static void Resize(IMagickImage bitmap, int width, int height)
        {
            bitmap.Resize((uint)width, (uint)height);
        }

        protected static void Scale(IMagickImage bitmap, double scaleX, double scaleY)
        {
            //bitmap.Resize(scaleX, scaleY);
        }

        protected static void Rotate(IMagickImage bitmap, int degrees)
        {
            bitmap.Rotate(degrees);
        }

        protected static void Flip(IMagickImage bitmap)
        {
            bitmap.Flip();
        }

        protected static void Flop(IMagickImage bitmap)
        {
            bitmap.Flop();
        }

        public override void WaterMark(WaterMarkOption option)
        {

        }

        #region 生成水印
        protected static void WaterMark(MagickImage image, WaterMarkOption option)
        {
            using (var water = new MagickImage(option.ImagePath))
            {
                water.Evaluate(Channels.Alpha, EvaluateOperator.Divide, option.Opaque);
                //if (option.Rotate != 0)
                //{
                //    water.Rotate(option.Rotate);
                //}
                switch (option.WaterMarkStyle)
                {
                    case WaterMarkStyleEnum.Fill:
                        WaterMarkFill(image, water, option);
                        break;
                    case WaterMarkStyleEnum.Repeat:
                        WaterMarkRepeat(image, water, option);
                        break;
                    case WaterMarkStyleEnum.Fixed:
                        WaterMarkFixed(image, water, option);
                        break;
                }
            }
        }

        protected static void WaterMarkFill(MagickImage image, MagickImage water, WaterMarkOption option)
        {
            var x = (int)option.Margin.Left;
            var y = (int)option.Margin.Top;
            image.Composite(water, x, y, CompositeOperator.Over);
        }

        protected static void WaterMarkRepeat(MagickImage image, MagickImage water, WaterMarkOption option)
        {
            var margin = option.Margin;

            var x = (int)margin.Left;
            var y = (int)margin.Top;
            var right = image.Width - margin.Right;
            var bottom = image.Height - margin.Bottom;
            while (y < bottom)
            {
                while (x < right)
                {
                    image.Composite(water, x, y, CompositeOperator.Over);
                    x += (int)water.Width;
                }
                y += (int)water.Height;
            }
        }

        protected static void WaterMarkFixed(MagickImage image, MagickImage water, WaterMarkOption option)
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

            image.Composite(water, (int)x, (int)y, CompositeOperator.Over);
        }
        #endregion

        public override PluginFrame GetFirstFrame()
        {
            return GetFrame(0);
        }

        public override PluginFrame GetFrame(int index)
        {
            if (Frames != null)
            {
                if (index >= 0 && index < Frames.Count)
                {
                    return Frames[index];
                }
            }
            return null;
        }

        public override PluginFrame GetFrameBySize(int size)
        {
            return GetFrameBySize(size, size);
        }

        public override PluginFrame GetFrameBySize(int width, int height)
        {
            if (Frames != null)
            {
                foreach (var frame in Frames)
                {
                    if (frame.Width == width && frame.Height == height)
                    {
                        return frame;
                    }
                }
            }
            return null;
        }
    }
}