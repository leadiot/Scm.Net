using Com.Scm.Image.Avatar;
using Com.Scm.Image.Barcode;
using Com.Scm.Image.Captcha;
using Com.Scm.Image.Enums;
using Com.Scm.Image.WaterMark;
using Com.Scm.Plugin.Image;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace Com.Scm.Image.SkiaSharp.Formats.Ico
{
    public class IcoImage : PluginImage
    {
        /// <summary>
        /// 对应idCount的结构体，多少张图就有多少个该结构
        /// </summary>
        private List<IcoDirEntry> _Entries = new List<IcoDirEntry>();

        public override bool Read(string file)
        {
            if (!File.Exists(file))
            {
                return false;
            }

            return Read(File.OpenRead(file));
        }

        public override bool Read(Stream stream)
        {
            var header = new IcoHeader();
            if (!header.FromStream(stream))
            {
                return false;
            }

            for (var i = 0; i < header.idCount; i += 1)
            {
                var entry = new IcoDirEntry();
                if (!entry.FromStream(stream))
                {
                    return false;
                }
                _Entries.Add(entry);
            }

            foreach (var entry in _Entries)
            {
                if (entry.DwImageOffset > 0)
                {
                    stream.Seek(entry.DwImageOffset, SeekOrigin.Begin);
                }

                var length = (int)entry.DwBytesInRes;
                var bytes = new byte[length];
                var readed = stream.Read(bytes, 0, length);
                if (readed != length)
                {
                    return false;
                }

                var bmpInfo = new BitmapInfo();
                if (!bmpInfo.Decode(bytes))
                {
                    return false;
                }

                Frames.Add(new PluginFrame(bmpInfo.IconBmp));
            }

            return true;
        }

        public override bool Read(Uri file)
        {
            throw new NotImplementedException();
        }

        //public void AddBmp(System.Drawing.Image img, int size)
        //{
        //    byte[] png;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        Bitmap bmp = new Bitmap(size, size, PixelFormat.Format32bppArgb);
        //        Graphics graphics = Graphics.FromImage(bmp);
        //        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        //        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //        graphics.DrawImage(img, new Rectangle(0, 0, size, size));
        //        graphics.Dispose();

        //        //Bitmap bmp = new Bitmap(img, new Size(size, size));
        //        new IconBitmap(bmp).ToStream(ms);
        //        png = ms.ToArray();
        //    }
        //    fileData.Write(png, 0, png.Length);

        //    IconDirEntry entry = new IconDirEntry();
        //    entry.bWidth = Convert.ToByte((size >= 256) ? 0 : size);
        //    entry.bHeight = entry.bWidth;
        //    entry.dwBytesInRes = Convert.ToUInt32(png.Length);
        //    entry.dwImageOffset = 0;
        //    idEntries.Add(entry);
        //}

        //public void AddPng(System.Drawing.Image img, int size)
        //{
        //    byte[] png;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        Bitmap bmp = new Bitmap(img, new Size(size, size));
        //        bmp.Save(ms, ImageFormat.Png);
        //        png = ms.ToArray();
        //    }
        //    fileData.Write(png, 0, png.Length);

        //    IconDirEntry entry = new IconDirEntry();
        //    entry.bWidth = Convert.ToByte((size >= 256) ? 0 : size);
        //    entry.bHeight = entry.bWidth;
        //    entry.dwBytesInRes = Convert.ToUInt32(png.Length);
        //    entry.dwImageOffset = 0;
        //    idEntries.Add(entry);
        //}

        public override bool Save(string file)
        {
            using (var stream = new FileStream(file, FileMode.Create))
            {
                return Save(stream);
            }
        }

        public override bool Save(Stream stream)
        {
            // 目录头
            IcoHeader header = new IcoHeader();
            header.idCount = System.Convert.ToUInt16(_Entries.Count);
            header.ToStream(stream);

            // 索引段
            uint startOffset = System.Convert.ToUInt32(6 + 16 * _Entries.Count);
            foreach (var entry in _Entries)
            {
                entry.DwImageOffset = startOffset;
                entry.ToStream(stream);
                startOffset += entry.DwBytesInRes;
            }

            // 图像数据段
            foreach (var entry in _Entries)
            {
                stream.Write(entry.Data, 0, entry.Data.Length);
            }

            return true;
        }

        public override void AddFrame(string file)
        {
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
        }

        public override void AddFrameBySize(Stream stream, int size)
        {
        }

        public override void AddFrameBySize(Stream stream, int width, int height)
        {
            var bitmap = SKBitmap.Decode(stream);
            bitmap = Resize(bitmap, width, height);
            var bytes = bitmap.Encode(SKEncodedImageFormat.Png, 100).ToArray();

            IcoDirEntry entry = new IcoDirEntry();
            entry.Width = System.Convert.ToByte((width >= 256) ? 0 : width);
            entry.Height = System.Convert.ToByte(entry.Width * 2);
            entry.DwBytesInRes = System.Convert.ToUInt32(bytes.Length);
            entry.DwImageOffset = (uint)(6 + _Entries.Count * 16);
            entry.Data = bytes;
            _Entries.Add(entry);
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
            if (Frames.Count > 0)
            {
                return Frames[0];
            }

            return null;
        }

        public override IFrame GetFrameBySize(int size)
        {
            foreach (var frame in Frames)
            {
                if (frame.Width == size && frame.Height == size)
                {
                    return frame;
                }
            }

            return null;
        }

        public override IFrame GetFrameBySize(int width, int height)
        {
            foreach (var frame in Frames)
            {
                if (frame.Width == width && frame.Height == height)
                {
                    return frame;
                }
            }

            return null;
        }

        public override void Stop()
        {
        }

        public override bool Convert(ScmImageFormat format)
        {
            return false;
        }

        public override void Resize(int width, int height)
        {
            foreach (var frame in Frames)
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
            foreach (var frame in Frames)
            {
                frame.Image = Rotate(frame.Image, degrees);
            }
            Show();
        }

        public override void Flip(FlipOption option)
        {
            foreach (var frame in Frames)
            {
                frame.Image = Flip(frame.Image);
            }
            Show();
        }

        public override void Flop(FlopOption option)
        {
            foreach (var frame in Frames)
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
            //if (_Images.Count > 0)
            //{
            //    return (MagickImage)_Images[0];
            //}
            return null;
        }

        public override void WaterMark(WaterMarkOption option)
        {
            foreach (var frame in Frames)
            {
                frame.Image = WaterMark(frame.Image, option);
            }
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
