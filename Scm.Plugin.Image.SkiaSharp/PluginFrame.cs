using Com.Scm.Plugin.Image;
using SkiaSharp;
using System.IO;

namespace Com.Scm.Image.SkiaSharp
{
    public class PluginFrame : ScmFrame
    {
        public SKBitmap Image { get; set; }

        public PluginFrame()
        {
        }

        public PluginFrame(SKBitmap image)
        {
            Image = image;
            Delay = 0;
        }

        public PluginFrame(SKBitmap image, int delay)
        {
            Image = image;
            Delay = delay;
        }

        public override double Width { get; }
        public override double Height { get; }

        public override int PixelWidth => throw new System.NotImplementedException();
        public override int PixelHeight => throw new System.NotImplementedException();

        private int _Delay;
        public int Delay
        {
            get { if (_Delay < 10) { _Delay = 100; } return _Delay; }
            set { _Delay = value; }
        }

        public override byte[] ToBytes(ScmImageFormat format)
        {
            using (var stream = new MemoryStream())
            {
                ImageUtils.Save(Image, stream, format);
                return stream.ToArray();
            }
        }

        public override bool Save(string file, ScmImageFormat format)
        {
            ImageUtils.Save(Image, file, format);
            return true;
        }

        public override bool Save(Stream stream, ScmImageFormat format)
        {
            ImageUtils.Save(Image, stream, format);
            return true;
        }
    }
}
