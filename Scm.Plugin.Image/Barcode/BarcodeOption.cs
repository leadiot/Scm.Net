namespace Com.Scm.Image.Barcode
{
    public class BarcodeInfo
    {
        public int id { get; set; }
        public string codec { get; set; }
        public string namec { get; set; }
        public int types { get; set; }
        public int length { get; set; }
        public bool checksum { get; set; }
        public string format { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }


    public class BarcodeConfig
    {
        /// <summary>
        /// 条码文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 条码图片
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 字体大小
        /// </summary>
        public int FontSize { get; set; }
        /// <summary>
        /// 字体名称
        /// </summary>
        public string FontName { get; set; }
        /// <summary>
        /// 图片宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 图片高度
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 文本位置
        /// </summary>
        public PositionEnum Position { get; set; }
        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeTypeEnum BarcodeType { get; set; }
    }

    public enum BarcodeTypeEnum
    {
        None = 0,
        Image = 1,
        ImageWithText = 2
    }

    public enum PositionEnum
    {
        None = 0,

        TopLeft = 1,
        TopCenter = 2,
        TopRight = 3,

        MiddleLeft = 4,
        MiddleCenter = 5,
        MiddleRight = 6,

        BottomLeft = 7,
        BottomCenter = 8,
        BottomRight = 9,

        Hidden = 10,
        Custom = 11,
    }
}
