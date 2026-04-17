using Com.Scm.Plugin.Image;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace Com.Scm.Image.ImageSharp
{
    public class PluginFrame : ScmFrame
    {
        public ImageFrame Frame { get; set; }

        public PluginFrame()
        {
        }

        public PluginFrame(ImageFrame image)
        {
            Frame = image;
            Delay = 0;
        }

        public PluginFrame(ImageFrame image, int delay)
        {
            Frame = image;
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
            IImageFormat fmt;
            switch (format)
            {
                case ScmImageFormat.Png:
                    fmt = SixLabors.ImageSharp.Formats.Png.PngFormat.Instance;
                    break;
                case ScmImageFormat.Jpg:
                    fmt = SixLabors.ImageSharp.Formats.Jpeg.JpegFormat.Instance;
                    break;
                case ScmImageFormat.Bmp:
                    fmt = SixLabors.ImageSharp.Formats.Bmp.BmpFormat.Instance;
                    break;
                case ScmImageFormat.Gif:
                    fmt = SixLabors.ImageSharp.Formats.Gif.GifFormat.Instance;
                    break;
                case ScmImageFormat.Ico:
                    fmt = SixLabors.ImageSharp.Formats.Bmp.BmpFormat.Instance;
                    break;
                default:
                    return false;
            }

            return true;
        }

        public override byte[] ToBytes(ScmImageFormat format)
        {
            //return Frame.ToByteArray();
            return null;
        }
    }
}
