using System.IO;

namespace Com.Scm.Plugin.Image
{
    public abstract class ScmFrame : IFrame
    {
        public abstract double Width { get; }

        public abstract double Height { get; }

        public abstract int PixelWidth { get; }

        public abstract int PixelHeight { get; }

        public abstract byte[] ToBytes(ScmImageFormat format);

        public abstract bool Save(string file, ScmImageFormat format);

        public abstract bool Save(Stream stream, ScmImageFormat format);
    }
}
