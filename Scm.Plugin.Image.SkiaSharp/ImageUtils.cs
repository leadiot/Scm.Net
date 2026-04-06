using Com.Scm.Plugin.Image;
using SkiaSharp;
using System.IO;

namespace Com.Scm.Image.SkiaSharp
{
    public class ImageUtils
    {
        /// <summary>
        /// 保存图像
        /// </summary>
        /// <param name="image"></param>
        /// <param name="path"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static bool Save(SKBitmap image, string path, ScmImageFormat format)
        {
            SKEncodedImageFormat fmt;
            switch (format)
            {
                case ScmImageFormat.Png:
                    fmt = SKEncodedImageFormat.Png;
                    break;

                case ScmImageFormat.Jpg:
                    fmt = SKEncodedImageFormat.Jpeg;
                    break;

                case ScmImageFormat.Gif:
                    fmt = SKEncodedImageFormat.Gif;
                    break;

                case ScmImageFormat.Bmp:
                    fmt = SKEncodedImageFormat.Bmp;
                    break;

                default:
                    return false;
            }

            using (var stream = File.OpenWrite(path))
            {
                image.Encode(stream, fmt, 100);
                return true;
            }
        }

        /// <summary>
        /// 保存图像
        /// </summary>
        /// <param name="image"></param>
        /// <param name="stream"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static bool Save(SKBitmap image, Stream stream, ScmImageFormat format)
        {
            SKEncodedImageFormat fmt;
            switch (format)
            {
                case ScmImageFormat.Png:
                    fmt = SKEncodedImageFormat.Png;
                    break;

                case ScmImageFormat.Jpg:
                    fmt = SKEncodedImageFormat.Jpeg;
                    break;

                case ScmImageFormat.Gif:
                    fmt = SKEncodedImageFormat.Gif;
                    break;

                case ScmImageFormat.Bmp:
                    fmt = SKEncodedImageFormat.Bmp;
                    break;

                default:
                    return false;
            }

            image.Encode(stream, fmt, 100);
            return true;
        }
    }
}
