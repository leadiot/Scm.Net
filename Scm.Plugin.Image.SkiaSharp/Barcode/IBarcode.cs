using Com.Scm.Image.Barcode;
using SkiaSharp;
using System.Collections.Generic;

namespace Com.Scm.Barcode
{
    public interface IBarcode
    {
        List<BarcodeInfo> Options { get; }

        /// <summary>
        /// 生成一维码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="format"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        SKBitmap Gen1D(string text, int format, int width, int height);

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="format"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        SKBitmap Gen2D(string text, int format, int width, int height);

        /// <summary>
        /// 生成带文字的图像
        /// </summary>
        /// <param name="image"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="font"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        SKBitmap GenBar(SKBitmap image, string text, PositionEnum position, string font, int size);

        /// <summary>
        /// 生成带图像的图像
        /// </summary>
        /// <param name="text"></param>
        /// <param name="format"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        SKBitmap GenBar(SKBitmap image, SKBitmap icon, PositionEnum position, int width, int height);

        /// <summary>
        /// 生成无文字条码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="format"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        SKBitmap GenBar(string text, int format, int width, int height);
    }
}