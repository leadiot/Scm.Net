namespace Com.Scm.Image.WaterMark
{
    public class WaterMarkOption
    {
        #region 基础类型设置
        private WaterMarkTypeEnum _waterMarkType = WaterMarkTypeEnum.None;
        /// <summary>
        /// 水印类型
        /// </summary>
        public WaterMarkTypeEnum WaterMarkType
        {
            get { return _waterMarkType; }
            set { _waterMarkType = value; }
        }
        #endregion

        private WaterMarkStyleEnum _WaterMarkStyle = WaterMarkStyleEnum.None;
        /// <summary>
        /// 水印类型
        /// </summary>
        public WaterMarkStyleEnum WaterMarkStyle
        {
            get { return _WaterMarkStyle; }
            set { _WaterMarkStyle = value; }
        }

        #region 水印位置设置
        private WaterMarkLocationEnum _waterMarkLocation = WaterMarkLocationEnum.BottomRight;
        /// <summary>
        /// 水印位置
        /// </summary>
        public WaterMarkLocationEnum WaterMarkLocation
        {
            get { return _waterMarkLocation; }
            set { _waterMarkLocation = value; }
        }

        private float _transparency = 0.7f;
        /// <summary>
        /// 水印透明度
        /// </summary>
        public float Transparency
        {
            get { return _transparency; }
            set { _transparency = value; }
        }

        public ScmSize Size { get; set; }
        #endregion

        #region 图片水印设置
        private string _ImagePath;
        /// <summary>
        /// 图片水印路径
        /// </summary>
        public string ImagePath
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }
        #endregion

        #region 文字水印设置
        private string _Text = "文字水印";
        /// <summary>
        /// 文字水印内容
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }

        private string _FontFamily = "微软雅黑";
        /// <summary>
        /// 文字字体
        /// </summary>
        public string FontFamily
        {
            get { return _FontFamily; }
            set { _FontFamily = value; }
        }

        /// <summary>
        /// 字体样式
        /// </summary>
        public bool Bold { get; set; }

        /// <summary>
        /// 字体样式
        /// </summary>
        public bool Italic { get; set; }

        private ScmColor _TextColor;
        /// <summary>
        /// 文字颜色
        /// </summary>
        public ScmColor TextColor
        {
            get { return _TextColor; }
            set { _TextColor = value; }
        }

        private int _TextSize = 12;
        /// <summary>
        /// 字体大小
        /// </summary>
        public int TextSize
        {
            get { return _TextSize; }
            set { _TextSize = value; }
        }
        #endregion

        public ScmThickness Margin { get; set; }

        private double _Opaque = 80;
        public double Opaque
        {
            get { return _Opaque; }
            set { _Opaque = value; }
        }

        private double _Rotate = 0;
        public double Rotate { get { return _Rotate; } set { _Rotate = value; } }

        private double _Dpi = 96.0;
        public double Dpi { get { return _Dpi; } set { _Dpi = value; } }
    }

    public enum WaterMarkStyleEnum
    {
        None,
        Fill,
        Repeat,
        Fixed
    }

    /// <summary>
    /// 水印位置
    /// </summary>
    public enum WaterMarkLocationEnum
    {
        None = 0,
        /// <summary>
        /// 顶部居左
        /// </summary>
        TopLeft = 1,
        /// <summary>
        /// 顶部居中
        /// </summary>
        TopCenter = 2,
        /// <summary>
        /// 顶部居右
        /// </summary>
        TopRight = 3,
        /// <summary>
        /// 中部居左
        /// </summary>
        CenterLeft = 4,
        /// <summary>
        /// 中部居中
        /// </summary>
        CenterCenter = 5,
        /// <summary>
        /// 中部居右
        /// </summary>
        CenterRight = 6,
        /// <summary>
        /// 底部居左
        /// </summary>
        BottomLeft = 7,
        /// <summary>
        /// 底部居左
        /// </summary>
        BottomCenter = 8,
        /// <summary>
        /// 底部居左
        /// </summary>
        BottomRight = 9,
    }

    /// <summary>
    /// 水印类型
    /// </summary>
    public enum WaterMarkTypeEnum
    {
        /// <summary>
        /// 无水印
        /// </summary>
        None = 0,
        /// <summary>
        /// 文字水印
        /// </summary>
        Text = 1,
        /// <summary>
        /// 图片水印
        /// </summary>
        Image = 2
    }

    public struct ScmThickness
    {
        private double _Left;

        private double _Top;

        private double _Right;

        private double _Bottom;

        public double Left
        {
            get
            {
                return _Left;
            }
            set
            {
                _Left = value;
            }
        }

        public double Top
        {
            get
            {
                return _Top;
            }
            set
            {
                _Top = value;
            }
        }

        public double Right
        {
            get
            {
                return _Right;
            }
            set
            {
                _Right = value;
            }
        }

        public double Bottom
        {
            get
            {
                return _Bottom;
            }
            set
            {
                _Bottom = value;
            }
        }

        public ScmThickness(double uniformLength)
        {
            _Left = _Top = _Right = _Bottom = uniformLength;
        }

        public ScmThickness(double left, double top, double right, double bottom)
        {
            _Left = left;
            _Top = top;
            _Right = right;
            _Bottom = bottom;
        }
    }

    public class ScmColor
    {
        public ScmColor()
        {
        }

        public ScmColor(int value)
        {
            this.Value = value;
        }

        public ScmColor(byte a, byte r, byte g, byte b)
        {
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public int Value { get; set; }

        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
    }

    public class ScmSize
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
