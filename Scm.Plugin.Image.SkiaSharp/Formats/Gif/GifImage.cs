using Com.Scm.Image.Avatar;
using Com.Scm.Image.Barcode;
using Com.Scm.Image.Captcha;
using Com.Scm.Image.Enums;
using Com.Scm.Image.WaterMark;
using Com.Scm.Plugin.Image;
using SkiaSharp;
using System;
using System.IO;

namespace Com.Scm.Image.SkiaSharp.Formats.Gif
{
    public class GifImage : PluginImage
    {
        //private bool _Running;
        /// <summary>
        /// 帧索引
        /// </summary>
        //private int _Index;
        /// <summary>
        /// 当前帧
        /// </summary>
        //private new SKBitmap _Frame;
        /// <summary>
        /// 总时长
        /// </summary>
        private int _TotalDuration;
        /// <summary>
        /// 重复次数
        /// </summary>
        private int _RepetitionCount;

        public override void Stop()
        {
            //_Running = false;
        }

        public override bool Read(string file)
        {
            using (var stream = File.OpenRead(file))
            {
                Read(stream);
            }

            return true;
        }

        public override bool Read(Stream stream)
        {
            using (var skStream = new SKManagedStream(stream))
            {
                using (var codec = SKCodec.Create(skStream))
                {
                    // Get frame count and allocate bitmaps
                    int frameCount = codec.FrameCount;

                    // Note: There's also a RepetitionCount property of SKCodec not used here
                    _RepetitionCount = codec.RepetitionCount;

                    // Loop through the frames
                    for (int frame = 0; frame < frameCount; frame++)
                    {
                        // From the FrameInfo collection, get the duration of each frame
                        var duration = codec.FrameInfo[frame].Duration;

                        // Create a full-color bitmap for each frame
                        var imageInfo = new SKImageInfo(codec.Info.Width, codec.Info.Height);
                        var bitmap = new SKBitmap(imageInfo);

                        // Get the address of the pixels in that bitmap
                        IntPtr pointer = bitmap.GetPixels();

                        // Create an SKCodecOptions value to specify the frame
                        SKCodecOptions codecOptions = new SKCodecOptions(frame);

                        // Copy pixels from the frame into the bitmap
                        codec.GetPixels(imageInfo, pointer, codecOptions);

                        Frames.Add(new PluginFrame(bitmap, duration));

                        _TotalDuration += duration;
                    }
                }
            }
            return true;
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
            foreach (PluginFrame frame in Frames)
            {
                frame.Image = Resize(frame.Image, width, height);
            }
            Show();
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

        public override bool Save(string file)
        {
            return true;
        }

        public override bool Save(Stream stream)
        {
            //_Images.Write(file);
            return true;
        }

        protected override SKBitmap DefaultImage()
        {
            //if (_Images.Count > 0)
            //{
            //    return (MagickImage)_Images[0];
            //}
            return null;
        }

        public override void WaterMark(WaterMarkOption option)
        {
            foreach (PluginFrame frame in Frames)
            {
                WaterMark(frame.Image, option);
            }
        }

        public override void AddFrame(string file)
        {
            var bitmap = SKBitmap.Decode(file);
            Frames.Add(new PluginFrame(bitmap, 0));
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
            var bitmap = SKBitmap.Decode(stream);
            Frames.Add(new PluginFrame(bitmap, 0));
        }

        public override void AddFrameBySize(Stream stream, int size)
        {
            AddFrameBySize(stream, size, size);
        }

        public override void AddFrameBySize(Stream stream, int width, int height)
        {
            var bitmap = SKBitmap.Decode(stream);
            bitmap = Resize(bitmap, width, height);

            Frames.Add(new PluginFrame(bitmap, 0));
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
            return null;
        }

        public override IFrame GetFirstFrame()
        {
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
