using Com.Scm.Image.Captcha;
using Com.Scm.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text;

namespace Com.Scm.Captcha
{
    /// <summary>
    /// 验证码生成
    /// </summary>
    public class ScmCaptcha : ICaptcha
    {
        #region 参数设置
        /// <summary>
        /// 验证码
        /// </summary>
        private CaptchaResult _Result;
        /// <summary>
        /// 参数
        /// </summary>
        private CaptchaOption _Option;
        /// <summary>
        /// 随机数
        /// </summary>
        private Random _Random = new Random();
        #endregion

        public ScmCaptcha()
        {
        }

        #region Public Method
        public CaptchaResult GenText(CaptchaOption option = null)
        {
            if (option == null)
            {
                option = new CaptchaOption();
            }
            _Option = option;

            if (_Option.CaptchaType == CaptchaTypeEnums.Arithmetic)
            {
                _Result = GetArithmetic();
                _Option.TextRotate = 0;
            }
            else
            {
                _Result = GetVerifyCode();
            }

            if (_Option.AutoSize)
            {
                _Option.Width = _Result.Key.Length * _Option.FontSize;
                _Option.Height = Convert.ToInt32(1.2 * _Option.FontSize) + 24;
            }

            return _Result;
        }

        public void GenImage(CaptchaResult result)
        {
            _Result = result;
            // 创建图片（透明背景）
            using var image = new Image<Rgba32>(_Option.Width, _Option.Height);

            // 字体（ImageSharp 自带，不依赖系统字体）
            //var font = SystemFonts.CreateFont(FontUtils.GetValidFontName(_Option.FontName), _Option.FontSize, FontStyle.Bold);
            var font = FontUtils.GetFont(_Option.FontName, _Option.FontSize, FontStyle.Bold);

            // 清空背景（白色/透明）
            image.Mutate(x => x.BackgroundColor(GetBackgroundColor()));

            // ----------------------
            // 画干扰线
            // ----------------------
            AddBacknoiseLine(image);

            // ----------------------
            // 画噪点
            // ----------------------
            AddBacknoisePoint(image);

            // ----------------------
            // 画文字（带偏移）
            // ----------------------
            float xPos = _Option.Padding;
            foreach (char c in result.Value)
            {
                // 随机颜色
                var color = GetFontColor();

                // 随机偏移
                float yPos = _Option.Padding + _Random.Next(5);

                // 画单个字符
                image.Mutate(x => x.DrawText(
                    c.ToString(),
                    font,
                    color,
                    new PointF(xPos, yPos)));

                xPos += 24; // 字符间距
            }

            // ----------------------
            // 画噪点
            // ----------------------
            AddForenoisePoint(image);

            // ----------------------
            // 画干扰线
            // ----------------------
            AddForenoiseLine(image);

            // 输出为 PNG
            using (var ms = new MemoryStream())
            {
                image.SaveAsPng(ms);
                result.Image = ms.ToArray();
            }
        }

        public CaptchaResult GenCaptcha(CaptchaOption option = null)
        {
            if (option == null)
            {
                option = new CaptchaOption();
            }
            _Option = option;

            if (_Option.CaptchaType == CaptchaTypeEnums.Arithmetic)
            {
                _Result = GetArithmetic();
                _Option.TextRotate = 0;
            }
            else
            {
                _Result = GetVerifyCode();
            }

            if (_Option.AutoSize)
            {
                _Option.Width = _Result.Key.Length * _Option.FontSize;
                _Option.Height = Convert.ToInt32(1.2 * _Option.FontSize) + 24;
            }

            GenImage(_Result);
            return _Result;
        }

        /// <summary>
        /// 获取随机字符问题
        /// </summary>
        private CaptchaResult GetVerifyCode()
        {
            var result = new CaptchaResult();

            var objStringBuilder = new StringBuilder();

            //加入数字1-9
            objStringBuilder.Append("123456789");

            //加入大写字母A-Z，不包括O
            if (_Option.HasUpperLetter)
            {
                objStringBuilder.Append("ABCDEFGHKMNPQRSTUVWXYZ");
            }

            //加入小写字母a-z，不包括o
            if (_Option.HasLowerLetter)
            {
                objStringBuilder.Append("abcdefghkmnpqrstuvwxyz");
            }

            //生成验证码字符串
            var key = "";
            for (int i = 0; i < _Option.TextLength; i++)
            {
                int index = _Random.Next(0, objStringBuilder.Length);
                key += objStringBuilder[index];

                objStringBuilder.Remove(index, 1);
            }

            result.Key = key;
            result.Value = result.Key;
            return result;
        }

        /// <summary>
        /// 获取四则运算问题
        /// </summary>
        /// <param name="questionList">默认数字加减验证</param>
        /// <returns></returns>
        private CaptchaResult GetArithmetic()
        {
            var result = new CaptchaResult();

            var left = _Random.Next(0, 10);
            var right = _Random.Next(0, 10);

            var optArray = new string[] { "+", "*", "-", "/" };
            var opt = optArray[_Random.Next(0, optArray.Length)];
            switch (opt)
            {
                case "+":
                    result.Key = $"{left}+{right}=?";
                    result.Value = (left + right).ToString();
                    break;
                case "*":
                    result.Key = $"{left}×{right}=?";
                    result.Value = (left * right).ToString();
                    break;
                case "-":
                    if (left < right)
                    {
                        var tmp = left;
                        left = right;
                        right = tmp;
                    }
                    result.Key = $"{left}-{right}=?";
                    result.Value = (left - right).ToString();
                    break;
                case "/":
                    right = _Random.Next(1, 10);
                    left = right * _Random.Next(1, 10);
                    result.Key = $"{left}÷{right}=?";
                    result.Value = (left / right).ToString();
                    break;
            }

            return result;
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 获取前景画布颜色
        /// </summary>
        /// <returns></returns>
        private Color GetFontColor()
        {
            if (!_Option.ForegroundColorRandom)
            {
                return Color.Parse(_Option.FontColor);
            }

            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            // 对于C#的随机数，没什么好说的
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            // 为了在白色背景上显示，尽量生成深色
            int red = RandomNum_First.Next(256);
            int green = RandomNum_Sencond.Next(256);
            int blue = (red + green > 400) ? 0 : 400 - red - green;
            blue = (blue > 255) ? 255 : blue;
            return Color.FromRgb((byte)red, (byte)green, (byte)blue);
        }

        /// <summary>
        /// 获取前景噪音颜色
        /// </summary>
        /// <returns></returns>
        private Color GetForenoiseColor()
        {
            var colors = _Option.ForenoiseColor;
            if (colors != null)
            {
                var color = colors[_Random.Next(colors.Length)];
                return Color.Parse(color);
            }

            return Color.White;
        }

        /// <summary>
        /// 获取背景画布颜色
        /// </summary>
        /// <returns></returns>
        private Color GetBackgroundColor()
        {
            if (!_Option.BackgroundColorRandom)
            {
                return Color.Parse(_Option.BackgroundColor);
            }
            var colors = _Option.BacknoiseColor;
            if (colors != null)
            {
                var color = colors[_Random.Next(colors.Length)];
                return Color.Parse(color);
            }

            return Color.White;
        }

        /// <summary>
        /// 获取背景噪音颜色
        /// </summary>
        /// <returns></returns>
        private Color GetBacknoiseColor()
        {
            var colors = _Option.BacknoiseColor;
            if (colors != null)
            {
                var color = colors[_Random.Next(colors.Length)];
                return Color.Parse(color);
            }

            return Color.White;
        }

        /// <summary>
        /// 绘制前景噪音点
        /// </summary>
        /// <param name="image"></param>
        private void AddForenoisePoint(Image<Rgba32> image)
        {
            if (!_Option.HasForenoisePoint)
            {
                return;
            }

            for (int i = 0; i < image.Width * _Option.ForenoisePointCount; i++)
            {
                int x = _Random.Next(image.Width);
                int y = _Random.Next(image.Height);
                image[x, y] = GetForenoiseColor();
            }
        }

        /// <summary>
        /// 绘制前景噪音线
        /// </summary>
        /// <param name="image"></param>
        /// <param name="objGraphics"></param>
        private void AddForenoiseLine(Image<Rgba32> image)
        {
            if (!_Option.HasForenoiseLine)
            {
                return;
            }

            //画图片的背景噪音线
            for (var i = 0; i < _Option.ForenoiseLineCount; i++)
            {
                var x1 = _Random.Next(image.Width);
                var x2 = _Random.Next(image.Width);
                var y1 = _Random.Next(image.Height);
                var y2 = _Random.Next(image.Height);

                var color = GetForenoiseColor();
                var pen = Pens.Solid(color, _Option.ForenoiseLineWidth);
                var p1 = new PointF(x1, y1);
                var p2 = new PointF(x2, y2);
                image.Mutate(x => x.DrawLine(pen, p1, p2));
            }
        }

        /// <summary>
        /// 绘制背景噪音点
        /// </summary>
        /// <param name="image"></param>
        /// <param name="objGraphics"></param>
        private void AddBacknoisePoint(Image<Rgba32> image)
        {
            if (!_Option.HasBacknoisePoint)
            {
                return;
            }

            for (int i = 0; i < image.Width * 2; i++)
            {
                int x = _Random.Next(image.Width);
                int y = _Random.Next(image.Height);
                image[x, y] = GetBacknoiseColor();
            }
        }

        /// <summary>
        /// 绘制背景噪音线
        /// </summary>
        /// <param name="objBitmap"></param>
        /// <param name="objGraphics"></param>
        private void AddBacknoiseLine(Image<Rgba32> image)
        {
            if (!_Option.HasBacknoiseLine)
            {
                return;
            }

            //画图片的背景噪音线
            for (var i = 0; i < _Option.BacknoiseLineCount; i++)
            {
                var x1 = _Random.Next(image.Width);
                var x2 = _Random.Next(image.Width);
                var y1 = _Random.Next(image.Height);
                var y2 = _Random.Next(image.Height);

                var color = GetBacknoiseColor();
                var pen = Pens.Solid(color, _Option.BacknoiseLineWidth);
                var p1 = new PointF(x1, y1);
                var p2 = new PointF(x2, y2);
                image.Mutate(x => x.DrawLine(pen, p1, p2));
            }
        }
        #endregion
    }
}
