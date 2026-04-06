using Com.Scm.Image.Captcha;
using SkiaSharp;
using System;
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

            //创建位图
            using (var image = new SKBitmap(_Option.Width, _Option.Height))
            {
                //创建画笔
                using (var canvas = new SKCanvas(image))
                {
                    //填充白色背景
                    canvas.Clear(GetBackgroundColor());

                    AddBacknoiseLine(image, canvas);
                    AddBacknoisePoint(image, canvas);

                    //canvas.DrawColor(SKColors.White);
                    //将文字写到画布上

                    var drawFont = new SKFont
                    {
                        Size = _Option.FontSize,
                        Typeface = SKTypeface.FromFamilyName(_Option.FontName, SKFontStyleWeight.SemiBold, SKFontStyleWidth.ExtraCondensed, SKFontStyleSlant.Upright)
                    };
                    var drawStyle = new SKPaint();
                    drawStyle.IsAntialias = true;
                    //drawStyle.TextSize = _Option.FontSize;
                    //drawStyle.Typeface = SKTypeface.FromFamilyName(_Option.FontName, SKFontStyleWeight.SemiBold, SKFontStyleWidth.ExtraCondensed, SKFontStyleSlant.Upright);

                    char[] chars = _Result.Key.ToCharArray();
                    for (int i = 0; i < chars.Length; i++)
                    {
                        canvas.Translate(_Option.Padding, _Option.Padding);

                        float px = (i) * _Option.FontSize;
                        float py = _Option.Height / 2;

                        //转动的度数
                        var angle = _Option.TextRotate > 0 ? _Random.Next(-_Option.TextRotate, _Option.TextRotate) : 0;
                        canvas.RotateDegrees(angle, px, py);

                        drawStyle.Color = GetFontColor();
                        //写字 (i + 1) * 16, 28
                        canvas.DrawText(chars[i].ToString(), px, py, drawFont, drawStyle);

                        // canvas.DrawText(chars[i].ToString(), (i ) * SetFontSize, (SetHeight-1), drawStyle);

                        canvas.RotateDegrees(-angle, px, py);
                        canvas.Translate(-_Option.Padding, -_Option.Padding);
                    }

                    //    using (SKPaint drawStyle = CreatePaint(SKColors.Black, SetHeight))
                    //{
                    //    Console.WriteLine( drawStyle.TextSize );
                    //    drawStyle.TextSize = SetFontSize;

                    //    canvas.DrawText(SetVerifyCodeText, 1, SetHeight-1, drawStyle);
                    //}

                    AddForenoiseLine(image, canvas);
                    AddForenoisePoint(image, canvas);
                }

                result.Image = image.Encode(SKEncodedImageFormat.Png, 100).ToArray();
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
        private SKColor GetFontColor()
        {
            if (!_Option.ForegroundColorRandom)
            {
                return SKColor.Parse(_Option.FontColor);
            }

            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            // 对于C#的随机数，没什么好说的
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            // 为了在白色背景上显示，尽量生成深色
            int int_Red = RandomNum_First.Next(256);
            int int_Green = RandomNum_Sencond.Next(256);
            int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return SKColor.FromHsv(int_Red, int_Green, int_Blue);
        }

        /// <summary>
        /// 获取前景噪音颜色
        /// </summary>
        /// <returns></returns>
        private SKColor GetForenoiseColor()
        {
            var colors = _Option.ForenoiseColor;
            if (colors != null)
            {
                var color = colors[_Random.Next(colors.Length)];
                return SKColor.Parse(color);
            }

            return SKColors.White;
        }

        /// <summary>
        /// 获取背景画布颜色
        /// </summary>
        /// <returns></returns>
        private SKColor GetBackgroundColor()
        {
            if (!_Option.BackgroundColorRandom)
            {
                return SKColor.Parse(_Option.BackgroundColor);
            }
            var colors = _Option.BacknoiseColor;
            if (colors != null)
            {
                var color = colors[_Random.Next(colors.Length)];
                return SKColor.Parse(color);
            }

            return SKColors.White;
        }

        /// <summary>
        /// 获取背景噪音颜色
        /// </summary>
        /// <returns></returns>
        private SKColor GetBacknoiseColor()
        {
            var colors = _Option.BacknoiseColor;
            if (colors != null)
            {
                var color = colors[_Random.Next(colors.Length)];
                return SKColor.Parse(color);
            }

            return SKColors.White;
        }

        /// <summary>
        /// 创建画笔
        /// </summary>
        /// <param name="color"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        private SKPaint CreatePaint(SKColor color, float fontSize)
        {
            var font = SKTypeface.FromFamilyName(null, SKFontStyleWeight.SemiBold, SKFontStyleWidth.ExtraCondensed, SKFontStyleSlant.Upright);
            SKPaint paint = new SKPaint();
            paint.IsAntialias = true;
            paint.Color = color;
            //paint.Typeface = font;
            //paint.TextSize = fontSize;
            return paint;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private SKPaint CreatePaint(SKColor color)
        {
            SKPaint paint = new SKPaint();
            paint.IsAntialias = true;
            paint.Color = color;
            return paint;
        }

        /// <summary>
        /// 绘制前景噪音点
        /// </summary>
        /// <param name="objBitmap"></param>
        private void AddForenoisePoint(SKBitmap objBitmap, SKCanvas objGraphics)
        {
            if (!_Option.HasForenoisePoint)
            {
                return;
            }

            for (int i = 0; i < objBitmap.Width * _Option.ForenoisePointCount; i++)
            {
                //objBitmap.SetPixel(_Random.Next(objBitmap.Width), _Random.Next(objBitmap.Height), GetForenoiseColor());
                using (SKPaint objPen = CreatePaint(GetForenoiseColor()))
                {
                    objGraphics.DrawRect(_Random.Next(objBitmap.Width), _Random.Next(objBitmap.Height), 1, 1, objPen);
                }
            }
        }

        /// <summary>
        /// 绘制前景噪音线
        /// </summary>
        /// <param name="objBitmap"></param>
        /// <param name="objGraphics"></param>
        private void AddForenoiseLine(SKBitmap objBitmap, SKCanvas objGraphics)
        {
            if (!_Option.HasForenoiseLine)
            {
                return;
            }

            //画图片的背景噪音线
            for (var i = 0; i < _Option.ForenoiseLineCount; i++)
            {
                var x1 = _Random.Next(objBitmap.Width);
                var x2 = _Random.Next(objBitmap.Width);
                var y1 = _Random.Next(objBitmap.Height);
                var y2 = _Random.Next(objBitmap.Height);

                var color = GetForenoiseColor();
                var paint = CreatePaint(color);
                paint.StrokeWidth = _Option.ForenoiseLineWidth;
                objGraphics.DrawLine(x1, y1, x2, y2, paint);
            }
        }

        /// <summary>
        /// 绘制背景噪音点
        /// </summary>
        /// <param name="objBitmap"></param>
        /// <param name="objGraphics"></param>
        private void AddBacknoisePoint(SKBitmap objBitmap, SKCanvas objGraphics)
        {
            if (!_Option.HasBacknoisePoint)
            {
                return;
            }

            for (int i = 0; i < objBitmap.Width * 2; i++)
            {
                using (SKPaint objPen = CreatePaint(GetBacknoiseColor()))
                {
                    objGraphics.DrawRect(_Random.Next(objBitmap.Width), _Random.Next(objBitmap.Height), 1, 1, objPen);
                }
            }
        }

        /// <summary>
        /// 绘制背景噪音线
        /// </summary>
        /// <param name="objBitmap"></param>
        /// <param name="objGraphics"></param>
        private void AddBacknoiseLine(SKBitmap objBitmap, SKCanvas objGraphics)
        {
            if (!_Option.HasBacknoiseLine)
            {
                return;
            }

            //画图片的背景噪音线
            for (var i = 0; i < _Option.BacknoiseLineCount; i++)
            {
                var x1 = _Random.Next(objBitmap.Width);
                var x2 = _Random.Next(objBitmap.Width);
                var y1 = _Random.Next(objBitmap.Height);
                var y2 = _Random.Next(objBitmap.Height);

                var color = GetBacknoiseColor();
                var paint = CreatePaint(color);
                paint.StrokeWidth = _Option.BacknoiseLineWidth;
                objGraphics.DrawLine(x1, y1, x2, y2, paint);
            }
        }
        #endregion
    }
}
