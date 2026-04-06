using Com.Scm.Image.Avatar;
using Com.Scm.Image.Barcode;
using Com.Scm.Image.Captcha;
using Com.Scm.Image.Enums;
using Com.Scm.Image.WaterMark;
using Com.Scm.Plugin.Image;
using ImageMagick;
using System;
using System.IO;

namespace Com.Scm.Image.Magick.Formats.Gif
{
    internal class GifImage : PluginImage
    {
        //private bool _Running;

        public override int Count { get { return Frames.Count; } }

        public override void Stop()
        {
            //_Running = false;
        }

        public override bool Read(string file)
        {
            var images = new MagickImageCollection(file);
            images.Coalesce();
            foreach (MagickImage image in images)
            {
                //image.AdaptiveResize(100, 100);
                Frames.Add(new PluginFrame(image, (int)image.AnimationDelay * 10));
            }

            return true;
        }

        public override bool Read(Stream stream)
        {
            throw new NotImplementedException();
        }

        public override bool Read(Uri file)
        {
            throw new NotImplementedException();
        }

        public override bool Convert(ScmImageFormat format)
        {
            return false;
        }

        public override void Resize(int width, int height)
        {
            foreach (var frame in Frames)
            {
                Resize(frame.Image, width, height);
            }
        }

        public override void Scale(double scaleX, double scaleY)
        {
            foreach (var frame in Frames)
            {
                Scale(frame.Image, scaleX, scaleY);
            }
        }

        public override void Rotate(int degrees)
        {
            foreach (var frame in Frames)
            {
                Rotate(frame.Image, degrees);
            }
        }

        public override void Flip(FlipOption option)
        {
            foreach (var frame in Frames)
            {
                Flip(frame.Image);
            }
        }

        public override void Flop(FlopOption option)
        {
            foreach (var frame in Frames)
            {
                Flop(frame.Image);
            }
        }

        public override void Crop(int x, int y, int width, int height)
        {
        }

        protected override MagickImage DefaultImage()
        {
            if (Frames.Count > 0)
            {
                return Frames[0].Image;
            }
            return null;
        }

        public override void WaterMark(WaterMarkOption option)
        {
            foreach (var frame in Frames)
            {
                WaterMark(frame.Image, option);
            }
        }

        public override bool Save(string file)
        {
            using (var stream = File.OpenWrite(file))
            {
                return Save(stream);
            }
        }

        public override bool Save(Stream stream)
        {
            var images = new MagickImageCollection();
            foreach (var frame in Frames)
            {
                images.Add(frame.Image);
            }
            images.Write(stream);
            return true;
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
