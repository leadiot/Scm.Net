using Com.Scm.Image.Avatar;
using Com.Scm.Image.Barcode;
using Com.Scm.Image.Captcha;
using Com.Scm.Image.Enums;
using Com.Scm.Image.WaterMark;
using Com.Scm.Plugin.Image;
using SkiaSharp;
using System;
using System.IO;

namespace Com.Scm.Image.SkiaSharp.Formats.Bit
{
    public class BitImage : PluginImage
    {
        private ScmImageFormat _Format;

        public override bool Read(string file)
        {
            if (!File.Exists(file))
            {
                return false;
            }

            using (var stream = File.OpenRead(file))
            {
                return Read(stream);
            }
        }

        public override bool Read(Stream stream)
        {
            var image = SKBitmap.Decode(stream);
            _Frame = new PluginFrame(image);
            Frames.Add(_Frame);
            _LoadOk = true;

            return _LoadOk;
        }

        public override bool Read(Uri file)
        {
            throw new NotImplementedException();
        }

        public override bool Convert(ScmImageFormat format)
        {
            _Format = format;
            //var imgFormat = (MagickFormat)Enum.Parse(typeof(MagickFormat), format);
            //_Image.Format = imgFormat;
            return true;
        }

        public override void Stop()
        {
        }

        public override void Resize(int width, int height)
        {
            foreach (PluginFrame frame in Frames)
            {
                frame.Image = Resize(frame.Image, width, height);
            }
            Show();
        }

        public override void Scale(double scaleX, double scaleY)
        {
            foreach (PluginFrame frame in Frames)
            {
                frame.Image = Scale(frame.Image, scaleX, scaleY);
            }
            Show();
        }

        public override bool Save(string file)
        {
            return true;
        }

        public override bool Save(Stream stream)
        {
            //if (_Image.Format == MagickFormat.Ico)
            //{
            //    if (_Image.Width > 256 || _Image.Height > 256)
            //    {
            //        _Image.Resize(256, 256);
            //    }
            //}
            //ImageUtils.Save(_Image, file, Scm.Image.Enums.ImageFormat.Gif);
            return true;
        }

        public override void Rotate(int degrees)
        {
            foreach (PluginFrame frame in Frames)
            {
                frame.Image = Rotate(frame.Image, degrees);
            }
            Show();
        }

        public override void Flip(FlipOption option)
        {
            foreach (PluginFrame frame in Frames)
            {
                frame.Image = Flip(frame.Image);
            }
            Show();
        }

        public override void Flop(FlopOption option)
        {
            foreach (PluginFrame frame in Frames)
            {
                frame.Image = Flop(frame.Image);
            }
            Show();
        }

        public override void Crop(int x, int y, int width, int height)
        {
        }

        protected override SKBitmap DefaultImage()
        {
            return _Frame.Image;
        }

        public override void WaterMark(WaterMarkOption option)
        {
            foreach (PluginFrame frame in Frames)
            {
                frame.Image = WaterMark(frame.Image, option);
            }
        }

        public override void AddFrame(string file)
        {
            if (_Frame == null)
            {
                _Frame = new PluginFrame();
                Frames.Add(_Frame);
            }
            _Frame.Image = SKBitmap.Decode(file);
        }

        public override void AddFrameBySize(string file, int size)
        {
            AddFrameBySize(file, size, size);
        }

        public override void AddFrameBySize(string file, int width, int height)
        {
            using (var stream = File.OpenRead(file))
            {
                AddFrameBySize(stream, width, height);
            }
        }

        public override void AddFrame(Stream stream)
        {
            _Frame.Image = SKBitmap.Decode(stream);
        }

        public override void AddFrameBySize(Stream stream, int size)
        {
            AddFrameBySize(stream, size, size);
        }

        public override void AddFrameBySize(Stream stream, int width, int height)
        {
            _Frame.Image = SKBitmap.Decode(stream);
            _Frame.Image = Resize(_Frame.Image, width, height);
        }

        public override void SetFrame(int index, string file)
        {
        }

        public override void SetFrame(int index, Stream stream)
        {
        }

        public override void RemoveFrame(int index)
        {
        }

        public override IFrame GetFrame(int index)
        {
            if (index >= 0 && index < Frames.Count)
            {
                return Frames[index];
            }

            return null;
        }

        public override IFrame GetFirstFrame()
        {
            if (Frames.Count > 0)
            {
                return Frames[0];
            }

            return null;
        }

        public override IFrame GetDefaultFrame()
        {
            return null;
        }

        public override IFrame GetFrameBySize(int size)
        {
            return null;
        }

        public override IFrame GetFrameBySize(int width, int height)
        {
            return null;
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
    }
}
