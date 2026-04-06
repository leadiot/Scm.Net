using Com.Scm.Plugin.Image;
using ImageMagick;
using System.IO;

namespace Com.Scm.Image.Magick
{
    public class PluginFrame : ScmFrame
    {
        public MagickImage Image { get; set; }

        public PluginFrame()
        {

        }

        public PluginFrame(MagickImage image)
        {
            Image = image;
            Delay = 0;
        }

        public PluginFrame(MagickImage image, int delay)
        {
            Image = image;
            Delay = delay;
        }

        private int _Delay;
        public int Delay
        {
            get { if (_Delay < 10) { _Delay = 100; } return _Delay; }
            set { _Delay = value; }
        }

        public override double Width { get; }
        public override double Height { get; }

        public override int PixelWidth => throw new System.NotImplementedException();
        public override int PixelHeight => throw new System.NotImplementedException();

        public override bool Save(string file, ScmImageFormat format)
        {
            using (Stream stream = File.OpenWrite(file))
            {
                return Save(stream, format);
            }
        }

        public override bool Save(Stream stream, ScmImageFormat format)
        {
            MagickFormat fmt;
            switch (format)
            {
                case ScmImageFormat.Png:
                    fmt = MagickFormat.Png;
                    break;
                case ScmImageFormat.Jpg:
                    fmt = MagickFormat.Jpg;
                    break;
                case ScmImageFormat.Bmp:
                    fmt = MagickFormat.Bmp;
                    break;
                case ScmImageFormat.Gif:
                    fmt = MagickFormat.Gif;
                    break;
                case ScmImageFormat.Ico:
                    fmt = MagickFormat.Ico;
                    break;
                default:
                    return false;
            }

            Image.Write(stream, fmt);
            return true;
        }

        public override byte[] ToBytes(ScmImageFormat format)
        {
            return Image.ToByteArray();
        }
    }
}
