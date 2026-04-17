using Com.Scm.Image.Avatar;
using Com.Scm.Image.Barcode;
using Com.Scm.Image.Captcha;
using Com.Scm.Image.Enums;
using Com.Scm.Image.WaterMark;
using Com.Scm.Plugin.Image;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Com.Scm.Image.ImageSharp
{
    public class PluginImage : ScmImage
    {
        public SixLabors.ImageSharp.Image _Image { get; private set; }

        public PluginImage()
        {
        }

        public static PluginImage CreateInstance(ScmImageFormat format)
        {
            return new PluginImage();
        }

        public static PluginImage Load(string file)
        {
            PluginImage image = new PluginImage();
            image.Read(file);
            return image;
        }

        public override bool Convert(string format)
        {
            return true;
        }

        protected SixLabors.ImageSharp.Image DefaultImage()
        {
            return _Image;
        }

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

        public override int Count => throw new NotImplementedException();

        public override List<ImageColor> GetImageColor(int count, int delta = 16)
        {
            return null;
        }

        #region 生成水印
        protected static void WaterMarkFill(SixLabors.ImageSharp.Image image, SixLabors.ImageSharp.Image water, WaterMarkOption option)
        {
            var x = (int)option.Margin.Left;
            var y = (int)option.Margin.Top;
            //image.Composite(water, x, y, CompositeOperator.Over);
        }

        protected static void WaterMarkRepeat(SixLabors.ImageSharp.Image image, SixLabors.ImageSharp.Image water, WaterMarkOption option)
        {
            //var margin = option.Margin;

            //var x = (int)margin.Left;
            //var y = (int)margin.Top;
            //var right = image.Width - margin.Right;
            //var bottom = image.Height - margin.Bottom;
            //while (y < bottom)
            //{
            //    while (x < right)
            //    {
            //        image.Composite(water, x, y, CompositeOperator.Over);
            //        x += (int)water.Width;
            //    }
            //    y += (int)water.Height;
            //}
        }

        protected static void WaterMarkFixed(SixLabors.ImageSharp.Image image, SixLabors.ImageSharp.Image water, WaterMarkOption option)
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

            //image.Composite(water, (int)x, (int)y, CompositeOperator.Over);
        }
        #endregion

        public override PluginFrame GetFirstFrame()
        {
            return GetFrame(0);
        }

        public override PluginFrame GetFrame(int index)
        {
            if (_Image != null)
            {
                if (index >= 0 && index < _Image.Frames.Count)
                {
                    return new PluginFrame(_Image.Frames[index]);
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
            if (_Image != null)
            {
                foreach (var frame in _Image.Frames)
                {
                    if (frame.Width == width && frame.Height == height)
                    {
                        return new PluginFrame(frame);
                    }
                }
            }
            return null;
        }

        public override bool Read(string file)
        {
            if (!File.Exists(file))
            {
                return false;
            }

            try
            {
                SixLabors.ImageSharp.Image.Load(file);

                _LoadOk = true;
            }
            catch (Exception)
            {
                _LoadOk = false;
            }

            return _LoadOk;
        }

        public override bool Read(Stream stream)
        {
            throw new NotImplementedException();
        }

        public override bool Read(Uri file)
        {
            throw new NotImplementedException();
        }

        public override bool Save(string file)
        {
            _Image.Save(file);
            return true;
        }

        public override bool Save(Stream stream)
        {
            _Image.Save(stream, null);
            return true;
        }

        public override bool Convert(ScmImageFormat format)
        {
            throw new NotImplementedException();
        }

        public override void Resize(int width, int height)
        {
            _Image.Mutate(x => x.Resize(width, height));
        }

        public override void Scale(double scaleX, double scaleY)
        {
            throw new NotImplementedException();
        }

        public override void Rotate(int degrees)
        {
            _Image.Mutate(x => x.Rotate(degrees));
        }

        public override void Flip(FlipOption option)
        {
            _Image.Mutate(x => x.Flip(FlipMode.Vertical));
        }

        public override void Flop(FlopOption option)
        {
            _Image.Mutate(x => x.Flip(FlipMode.Horizontal));
        }

        public override void Crop(int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        public override void AddFrame(string file)
        {
            throw new NotImplementedException();
        }

        public override void AddFrameBySize(string file, int size)
        {
            throw new NotImplementedException();
        }

        public override void AddFrameBySize(string file, int width, int height)
        {
            throw new NotImplementedException();
        }

        public override void AddFrame(Stream stream)
        {
            throw new NotImplementedException();
        }

        public override void AddFrameBySize(Stream stream, int size)
        {
            throw new NotImplementedException();
        }

        public override void AddFrameBySize(Stream stream, int width, int height)
        {
            throw new NotImplementedException();
        }

        public override void SetFrame(int index, string file)
        {
            throw new NotImplementedException();
        }

        public override void SetFrame(int index, Stream stream)
        {
            throw new NotImplementedException();
        }

        public override void RemoveFrame(int index)
        {
            throw new NotImplementedException();
        }

        public override IFrame GetDefaultFrame()
        {
            throw new NotImplementedException();
        }

        public override IFrame GenBarcode(string text, int format, int width, int height)
        {
            throw new NotImplementedException();
        }

        public override IFrame GenBarcode(IFrame image, string text, PositionEnum position, string font, int size)
        {
            throw new NotImplementedException();
        }

        public override CaptchaResult GenCaptcha(CaptchaOption option = null)
        {
            throw new NotImplementedException();
        }

        public override AvatarResult GenAvatar(AvatarOption option = null)
        {
            throw new NotImplementedException();
        }

        public override void WaterMark(WaterMarkOption option)
        {
            SixLabors.ImageSharp.Image water = null;
            switch (option.WaterMarkStyle)
            {
                case WaterMarkStyleEnum.Fill:
                    WaterMarkFill(_Image, water, option);
                    break;
                case WaterMarkStyleEnum.Repeat:
                    WaterMarkRepeat(_Image, water, option);
                    break;
                case WaterMarkStyleEnum.Fixed:
                    WaterMarkFixed(_Image, water, option);
                    break;
            }
        }
    }
}
