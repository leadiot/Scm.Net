using Com.Scm.Image.Barcode;
using Com.Scm.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace Com.Scm.Barcode.Zxing
{
    internal class ZxingBarcode : IBarcode
    {
        public int Format { get; set; }
        public int TextAlign { get; set; }
        public string TextColor { get; set; }
        public string FontName { get; set; }
        public int FontSize { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public SixLabors.ImageSharp.Image Logo { get; set; }

        private static List<BarcodeInfo> _Options;

        public List<BarcodeInfo> Options
        {
            get
            {
                if (_Options == null)
                {
                    _Options = new List<BarcodeInfo>();
                    _Options.Add(new BarcodeInfo { id = 16, codec = "CODE_128", namec = "通用一维码", types = 1 });
                    _Options.Add(new BarcodeInfo { id = 4, codec = "CODE_39", namec = "通用一维码（39码）", types = 1 });
                    _Options.Add(new BarcodeInfo { id = 8, codec = "CODE_93", namec = "通用一维码（93码）", types = 1 });
                    _Options.Add(new BarcodeInfo { id = 128, codec = "EAN_13", namec = "通用商品条形码（标准版）", types = 1, length = 13, checksum = true });
                    _Options.Add(new BarcodeInfo { id = 64, codec = "EAN_8", namec = "通用商品条形码（简缩版）", types = 1, length = 8, checksum = true });
                    _Options.Add(new BarcodeInfo { id = 16384, codec = "UPC_A", namec = "美国商品条码（标准版）", types = 1, length = 13, checksum = true });
                    _Options.Add(new BarcodeInfo { id = 32768, codec = "UPC_E", namec = "美国商品条码（简缩版）", types = 1, length = 8, checksum = true });
                    _Options.Add(new BarcodeInfo { id = 2, codec = "CODABAR", namec = "库德巴码", types = 1 });
                    _Options.Add(new BarcodeInfo { id = 256, codec = "ITF", namec = "交叉二五条码", types = 1 });
                    _Options.Add(new BarcodeInfo { id = 262144, codec = "PLESSEY", namec = "MSI Plessey条码", types = 1 });
                    //Options.Add(new BarcodeOption { id = 4096, name = "RSS_14", types = 1 });
                    //Options.Add(new BarcodeOption { id = 8192, name = "RSS_EXPANDED", types = 1 });
                    //Options.Add(new BarcodeOption { id = 61918, name = "All_1D", types = 1 });

                    _Options.Add(new BarcodeInfo { id = 2048, codec = "QR_CODE", namec = "通用二维码", types = 2, width = 150, height = 150 });
                    _Options.Add(new BarcodeInfo { id = 32, codec = "DATA_MATRIX", namec = "矩阵二维码", types = 2, width = 150, height = 150 });
                    _Options.Add(new BarcodeInfo { id = 1024, codec = "PDF_417", namec = "堆叠二维码", types = 2, width = 150, height = 150 });
                    _Options.Add(new BarcodeInfo { id = 1, codec = "AZTEC", namec = "阿兹特克码", types = 2, width = 150, height = 150 });
                    //Options.Add(new BarcodeOption { id = 512, name = "MAXICODE", types = 2 });
                    //Options.Add(new BarcodeOption { id = 65536, name = "UPC_EAN_EXTENSION", types = 0 });
                    //Options.Add(new BarcodeOption { id = 131072, name = "MSI", types = 0 });
                    //Options.Add(new BarcodeOption { id = 524288, name = "IMB", types = 0 });
                    //Options.Add(new BarcodeOption { id = 1048576, name = "PHARMA_CODE", types = 0 });
                }

                return _Options;
            }
        }

        public BarcodeInfo GetOptionById(int id)
        {
            foreach (var option in Options)
            {
                if (option.id == id)
                {
                    return option;
                }
            }

            return null;
        }

        /// <summary>
        /// 生成一维条形码
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        public SixLabors.ImageSharp.Image Gen1D(string text, int format, int width, int height)
        {
            var writer = new ZXing.ImageSharp.BarcodeWriter<Rgba32>
            {
                Format = (BarcodeFormat)format,
                Options = new EncodingOptions
                {
                    GS1Format = false,
                    Width = width,
                    Height = height,
                    PureBarcode = true,
                    Margin = 1
                }
            };
            return writer.Write(text);
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        public SixLabors.ImageSharp.Image Gen2D(string text, int format, int width, int height)
        {
            var writer = new ZXing.ImageSharp.BarcodeWriter<Rgba32>
            {
                Format = (BarcodeFormat)format,
                Options = new QrCodeEncodingOptions
                {
                    DisableECI = true,//设置内容编码
                    CharacterSet = "UTF-8",  //设置二维码的宽度和高度
                    Width = width,
                    Height = height,
                    Margin = 1//设置二维码的边距,单位不是固定像素
                },
                //Renderer = new BarcodeRenderer<Rgba32>(Color.Black, Color.White)
            };

            return writer.Write(text);
        }

        public SixLabors.ImageSharp.Image GenBar(string text, int format, int width, int height)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            var writer = new ZXing.ImageSharp.BarcodeWriter<Rgba32>();
            writer.Format = (BarcodeFormat)format;
            var option = new EncodingOptions()
            {
                Margin = 3,
                PureBarcode = true,
            };
            if (width > 0)
            {
                option.Width = width;
            }
            if (height > 0)
            {
                option.Height = height;
            }
            writer.Options = option;

            return writer.Write(text);
        }

        public SixLabors.ImageSharp.Image GenBar(SixLabors.ImageSharp.Image image, string text, PositionEnum position, string fontName, int fontSize = 14)
        {
            //FontUtils.InitFonts(24);

            var tmp = new Image<Rgba32>(image.Width, image.Height);
            tmp.Mutate(x => x.DrawImage(image, new SixLabors.ImageSharp.Point(0, 0), 1.0f));

            var font = SystemFonts.CreateFont(FontUtils.GetValidFontName(fontName), fontSize, FontStyle.Bold);
            var option = new TextOptions(font);
            FontRectangle rect = TextMeasurer.MeasureSize(text, option);
            var width = (int)rect.Width;
            var height = (int)rect.Height;
            var rectHeight = height * 3 / 2;
            var point = GetPosition(image, width, rectHeight, position);
            tmp.Mutate(x => x.Fill(Color.White, new Rectangle(point.X, point.Y, width, rectHeight)));

            var textHeight = height;
            point = GetPosition(image, width, textHeight, position);
            tmp.Mutate(x => x.DrawText(text, font, SixLabors.ImageSharp.Color.Black, point));

            return tmp;
        }

        /// <summary>
        /// 生成带Logo的二维码
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public SixLabors.ImageSharp.Image GenBar(SixLabors.ImageSharp.Image image, SixLabors.ImageSharp.Image icon, PositionEnum position, int width, int height)
        {
            var tmp = new Image<Rgba32>(image.Width, image.Height);
            tmp.Mutate(x => x.DrawImage(image, new SixLabors.ImageSharp.Point(0, 0), 1.0f));

            var point = GetPosition(image, width, height, position);
            tmp.Mutate(x => x.DrawImage(image, point, 1.0f));
            return tmp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image">原始图像</param>
        /// <param name="width">目标宽度</param>
        /// <param name="height">目标高度</param>
        /// <param name="position">位置枚举</param>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns></returns>
        private static SixLabors.ImageSharp.Point GetPosition(SixLabors.ImageSharp.Image image, int width, int height, PositionEnum position, int x = 0, int y = 0, int gapx = 0, int gapy = 0)
        {
            var pointX = (image.Width - width) / 2;
            var pointY = (image.Height - height) / 2;
            switch (position)
            {
                case PositionEnum.TopLeft:
                    pointX = gapx;
                    pointY = gapy;
                    break;
                case PositionEnum.TopRight:
                    pointX = image.Width - width - gapx;
                    pointY = gapy;
                    break;
                case PositionEnum.TopCenter:
                    pointX = (image.Width - width) / 2;
                    pointY = gapy;
                    break;
                case PositionEnum.MiddleLeft:
                    pointX = gapx;
                    pointY = (image.Height - height) / 2;
                    break;
                case PositionEnum.MiddleRight:
                    pointX = image.Width - width - 2;
                    pointY = (image.Height - height) / 2;
                    break;
                case PositionEnum.MiddleCenter:
                    pointX = (image.Width - width) / 2;
                    pointY = (image.Height - height) / 2;
                    break;
                case PositionEnum.BottomLeft:
                    pointX = gapx;
                    pointY = image.Height - height - gapy;
                    break;
                case PositionEnum.BottomRight:
                    pointX = image.Width - width - gapx;
                    pointY = image.Height - height - gapy;
                    break;
                case PositionEnum.BottomCenter:
                    pointX = (image.Width - width) / 2;
                    pointY = image.Height - height - gapy;
                    break;
                case PositionEnum.Custom:
                    pointX = x;
                    pointY = y;
                    break;
            }

            return new SixLabors.ImageSharp.Point(pointX, pointY);
        }
    }
}
