using Com.Scm.Image.Avatar;
using Com.Scm.Image.Barcode;
using Com.Scm.Image.Captcha;
using Com.Scm.Image.Enums;
using Com.Scm.Image.WaterMark;
using Com.Scm.Plugin.Image;
using ImageMagick;
using System;
using System.IO;

namespace Com.Scm.Image.Magick.Formats.Bit
{
    internal class BitImage : PluginImage
    {
        private MagickImage _Image;

        public override int Count { get { return Frames.Count; } }

        public override bool Read(string file)
        {
            if (!File.Exists(file))
            {
                return false;
            }

            if (_Image == null)
            {
                _Image = new MagickImage();
            }

            try
            {
                using (var stream = File.OpenRead(file))
                {
                    _Image.Read(stream);
                }

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
            return false;
        }

        public override bool Read(Uri file)
        {
            throw new NotImplementedException();
        }

        public override bool Convert(ScmImageFormat format)
        {
            //if (string.IsNullOrEmpty(format))
            //{
            //    return false;
            //}

            //if (format[0] == '.')
            //{
            //    format = format.Substring(1);
            //}

            //var imgFormat = (MagickFormat)Enum.Parse(typeof(MagickFormat), format);
            //_Image.Format = imgFormat;
            return true;
        }

        public override void Stop()
        {
        }

        public override void Resize(int width, int height)
        {
            _Image.Resize((uint)width, (uint)height);
        }

        public override void Scale(double scaleX, double scaleY)
        {
            //_Image.Scale(scaleX, scaleY);
        }

        public override bool Save(string file)
        {
            if (_Image.Format == MagickFormat.Ico)
            {
                if (_Image.Width > 256 || _Image.Height > 256)
                {
                    _Image.Resize(256, 256);
                }
            }
            _Image.Write(file);
            return true;
        }

        public override void Rotate(int degrees)
        {
            _Image.Rotate(degrees);
        }

        public override void Flip(FlipOption option)
        {
            _Image.Flip();
        }

        public override void Flop(FlopOption option)
        {
            _Image.Flop();
        }

        public override void Crop(int x, int y, int width, int height)
        {
        }

        protected override MagickImage DefaultImage()
        {
            return _Image;
        }

        public override void WaterMark(WaterMarkOption option)
        {
            WaterMark(_Image, option);
        }

        public override bool Save(Stream stream)
        {
            throw new NotImplementedException();
        }

        public override PluginFrame GetDefaultFrame()
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

        public override PluginFrame GenBarcode(string text, int format, int width, int height)
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
