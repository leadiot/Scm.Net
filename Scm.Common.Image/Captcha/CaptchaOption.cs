namespace Com.Scm.Image.Captcha
{
    public class CaptchaOption
    {
        #region 内容设置
        /// <summary>
        /// 验证方式
        /// </summary>
        public CaptchaTypeEnums CaptchaType { get; set; }
        #region 随机字符
        /// <summary>
        /// 是否加入小写字母，默认是
        /// </summary>
        public bool HasLowerLetter { get; set; } = true;
        /// <summary>
        /// 是否加入大写字母，默认是
        /// </summary>
        public bool HasUpperLetter { get; set; } = true;
        /// <summary>
        /// 验证码长度，默认4个字符
        /// </summary>
        public int TextLength { get; set; } = 4;
        #endregion
        #region 四则运算
        /// <summary>
        /// 四则运算
        /// </summary>
        public string[] Operators = new string[] { "+", "-", "*", "/" };
        #endregion
        #endregion

        #region 字体设置
        /// <summary>
        /// 字体大小
        /// </summary>
        public int FontSize = 20;
        /// <summary>
        /// 字体名称
        /// </summary>
        public string FontName = "Verdana";
        /// <summary>
        /// 随机码的旋转角度
        /// </summary>
        public int TextRotate { get; set; } = 40;
        #endregion

        #region 图片设置
        /// <summary>
        /// 图片宽度(像素)
        /// </summary>
        public int Width { get; set; } = 100;
        /// <summary>
        /// 图片高度(像素)
        /// </summary>
        public int Height { get; set; } = 30;
        /// <summary>
        /// 图片边衬
        /// </summary>
        public int Padding { get; set; } = 5;
        /// <summary>
        /// 图片自适应大小
        /// </summary>
        public bool AutoSize { get; set; }
        #endregion

        #region 前景设置
        /// <summary>
        /// 是否加入前景噪音点
        /// </summary>
        public bool HasForenoisePoint { get; set; }
        /// <summary>
        /// 前景噪音点数量
        /// </summary>
        public int ForenoisePointCount { get; set; } = 2;
        /// <summary>
        /// 前景噪音点大小
        /// </summary>
        public int ForenoisePointSize { get; set; } = 1;
        /// <summary>
        /// 是否加入前景噪音线
        /// </summary>
        public bool HasForenoiseLine { get; set; }
        /// <summary>
        /// 前景噪音点数量
        /// </summary>
        public int ForenoiseLineCount { get; set; } = 12;
        /// <summary>
        /// 前景噪音点精细
        /// </summary>
        public int ForenoiseLineWidth { get; set; } = 1;
        /// <summary>
        /// 是否随机字体颜色
        /// </summary>
        public bool ForegroundColorRandom { get; set; } = true;
        /// <summary>
        /// 前景字体颜色
        /// </summary>
        public string FontColor { get; set; } = "#000000";
        /// <summary>
        /// 前景噪音颜色
        /// </summary>
        //public string[] ForenoiseColor = new string[] { "#f6f4fc", "#f6f4fc", "#f6f4fc", "#f6f4fc", "#f6f4fc", "#f6f4fc", "#f6f4fc", "#f6f4fc" };
        public string[] ForenoiseColor = new string[] { "#00E5EE", "#000000", "#2F4F4F", "#000000", "#43CD80", "#191970", "#006400", "#458B00", "#8B7765", "#CD5B45" };
        #endregion

        #region 背景设置
        /// <summary>
        /// 是否加入背景噪音点
        /// </summary>
        public bool HasBacknoisePoint { get; set; } = true;
        /// <summary>
        /// 背景噪音点精细
        /// </summary>
        public int BacknoisePointCount { get; set; } = 2;
        /// <summary>
        /// 背景噪音点大小
        /// </summary>
        public int BacknoisePointSize { get; set; } = 1;
        /// <summary>
        /// 是否加入背景噪音线
        /// </summary>
        public bool HasBacknoiseLine { get; set; } = true;
        /// <summary>
        /// 背景噪音线数量
        /// </summary>
        public int BacknoiseLineCount { get; set; } = 12;
        /// <summary>
        /// 背景噪音线精细
        /// </summary>
        public int BacknoiseLineWidth { get; set; } = 1;
        /// <summary>
        /// 是否随机字体颜色
        /// </summary>
        public bool BackgroundColorRandom { get; set; }
        /// <summary>
        /// 背景画布颜色
        /// </summary>
        public string BackgroundColor { get; set; } = "#ffffff";
        /// <summary>
        /// 背景噪音颜色
        /// </summary>
        //public string[] BacknoiseColor = new string[] { "#00E5EE", "#000000", "#2F4F4F", "#000000", "#43CD80", "#191970", "#006400", "#458B00", "#8B7765", "#CD5B45" };
        public string[] BacknoiseColor = new string[] { "#f6f4fc", "#f6f4fc", "#f6f4fc", "#f6f4fc", "#f6f4fc", "#f6f4fc", "#f6f4fc", "#f6f4fc" };
        #endregion
    }

    public enum CaptchaTypeEnums
    {
        None = 0,
        /// <summary>
        /// 随机字符
        /// </summary>
        VerifyCode = 1,
        /// <summary>
        /// 四则运算
        /// </summary>
        Arithmetic = 2,
    }
}
