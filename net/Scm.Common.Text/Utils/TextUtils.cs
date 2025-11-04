using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Com.Scm.Utils
{
    public partial class TextUtils
    {
        #region 对象转换
        #region 大小写转换
        /// <summary>
        /// 首字母转小写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstCharToLower(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return char.ToLower(s[0]) + s.Substring(1);
        }

        /// <summary>
        /// 首字母转大写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return char.ToUpper(s[0]) + s.Substring(1);
        }
        #endregion

        #region 编码方案转换
        #region 任意进制字符转换
        /// <summary>
        /// 将字节数组转换为指定进制的字符串
        /// </summary>
        /// <param name="bytes">要转换的数据</param>
        /// <param name="bits">转换的进制，在1-8之间</param>
        /// <param name="masks">转换的掩码</param>
        /// <param name="padChar">位数不足时的填充字符</param>
        /// <returns>转换后的字符串</returns>
        public static string Encode(byte[] bytes, int bits, string masks, char padChar = '=')
        {
            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentException("目标数据不能为空！");
            }

            if (bits < 1 || bits > 8)
            {
                throw new ArgumentException("无效的进制：" + bits);
            }

            var qty = 1 << bits;
            if (masks == null || masks.Length < qty)
            {
                throw new ArgumentException("掩码空间不足！");
            }

            qty = (bytes.Length * 8 + 1) / bits;
            var mask = 0xFF >> (8 - bits);

            int buffer = 0;
            int bitsLeft = 0;

            var builder = new StringBuilder();
            foreach (byte tmp in bytes)
            {
                buffer = (buffer << 8) | tmp;
                bitsLeft += 8;

                while (bitsLeft >= bits)
                {
                    int index = (buffer >> (bitsLeft - bits)) & mask;
                    builder.Append(masks[index]);
                    bitsLeft -= bits;
                }
            }

            // 处理剩余的位
            if (bitsLeft > 0)
            {
                int index = (buffer << (bits - bitsLeft)) & mask;
                builder.Append(masks[index]);
            }

            var length = builder.Length;
            while (length < qty)
            {
                builder.Append(padChar);
                length += 1;
            }
            return builder.ToString();
        }

        /// <summary>
        /// 将字符串转换为指定进制的字节数组
        /// </summary>
        /// <param name="base32">Base32编码字符串</param>
        /// <returns>解码后的数据</returns>
        public static byte[] Decode(string chars, int bits, string masks, char padChar = '=')
        {
            if (chars == null || chars.Length < 1)
            {
                throw new ArgumentException("目标数据不能为空！");
            }

            if (bits < 1 || bits > 8)
            {
                throw new ArgumentException("无效的进制：" + bits);
            }

            var qty = 1 << bits;
            if (masks == null || masks.Length < qty)
            {
                throw new ArgumentException("掩码空间不足！");
            }

            chars = chars.TrimEnd(padChar);

            int buffer = 0;
            int bitsLeft = 0;

            qty = (chars.Length * bits + 1) / 8;
            var result = new byte[qty];
            int index = 0;
            foreach (char tmp in chars)
            {
                int charIndex = masks.IndexOf(tmp);
                if (charIndex == -1)
                {
                    throw new ArgumentException("无效的字符：" + tmp);
                }

                buffer = (buffer << bits) | charIndex;
                bitsLeft += bits;

                if (bitsLeft >= 8)
                {
                    result[index++] = (byte)(buffer >> (bitsLeft - 8));
                    bitsLeft -= 8;
                }
            }

            return result;
        }
        #endregion

        #region 16进制转换
        public const string Base16Chars = "0123456789ABCDEF";
        /// <summary>
        /// 将字节数组转换为16进制的字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Base16Encode(byte[] bytes)
        {
            return Encode(bytes, 4, Base16Chars);
        }

        /// <summary>
        /// 将字符串转换为16进制的字节数组
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static byte[] Base16Decode(string chars)
        {
            return Decode(chars, 4, Base16Chars);
        }
        #endregion

        #region 32进制转换
        public const string Base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        /// <summary>
        /// 将字节数组转换为32进制的字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Base32Encode(byte[] bytes)
        {
            return Encode(bytes, 5, Base32Chars);
        }

        /// <summary>
        /// 将字符串转换为32进制的字节数组
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static byte[] Base32Decode(string chars)
        {
            return Decode(chars, 5, Base32Chars);
        }
        #endregion

        #region 64进制转换
        public const string Base64Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456790+/";

        /// <summary>
        /// 将字节数组转换为64进制的字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Base64Encode(byte[] bytes)
        {
            return Encode(bytes, 6, Base64Chars);
        }

        /// <summary>
        /// 将字符串转换为64进制的字节数组
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static byte[] Base64Decode(string chars)
        {
            return Decode(chars, 6, Base64Chars);
        }
        #endregion

        /// <summary>
        /// 转换为2进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToBinString(byte[] bytes)
        {
            return Encode(bytes, 1, Base16Chars);
        }

        /// <summary>
        /// 转换为8进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="upperCase"></param>
        /// <returns></returns>
        public static string ToOctString(byte[] bytes)
        {
            return Encode(bytes, 3, Base16Chars);
        }

        /// <summary>
        /// 转换为16进制字符串
        /// </summary>
        /// <param name="bytes">待转换数组</param>
        /// <param name="upperCase">是否为大写</param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes, bool upperCase = false)
        {
            var format = upperCase ? "X2" : "x2";
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString(format));
            }
            return builder.ToString();
        }

        public string FromUrlBase64String(string str)
        {
            byte[] decbuff = Convert.FromBase64String(str.Replace(",", "=").Replace("-", "+").Replace("/", "_"));
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }

        public string ToUrlBase64String(string input)
        {
            byte[] encbuff = Encoding.UTF8.GetBytes(input ?? "");
            return Convert.ToBase64String(encbuff).Replace("=", ",").Replace("+", "-").Replace("_", "/");
        }
        #endregion

        #region 字符与对象转换
        private static JsonSerializerSettings _Settings = new JsonSerializerSettings();

        public static string ToJsonString(object obj, bool indented = false, bool ignore = false)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj is string)
            {
                return obj as string;
            }
            _Settings.Formatting = indented ? Formatting.Indented : Formatting.None;
            _Settings.DefaultValueHandling = ignore ? DefaultValueHandling.IgnoreAndPopulate : DefaultValueHandling.Include;
            return JsonConvert.SerializeObject(obj, _Settings);
        }

        public static T AsJsonObject<T>(string text) where T : new()
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(text);
        }

        public static string ToXmlString(object data)
        {
            using (var writer = new StringWriter())
            {
                XmlSerializer xz = new XmlSerializer(data.GetType());
                xz.Serialize(writer, data);
                return writer.ToString();
            }
        }

        public static T AsXmlObject<T>(string text) where T : new()
        {
            using (var reader = new StringReader(text))
            {
                XmlSerializer xz = new XmlSerializer(typeof(T));
                T obj = (T)xz.Deserialize(reader);
                return obj;
            }
        }

        public static T ToModel<T>(Dictionary<string, object> dict)
        {
            T obj = Activator.CreateInstance<T>();
            var modelType = typeof(T);
            foreach (var param in dict.Keys)
            {
                if (!(param is string))
                {
                    continue;
                }

                var key = param as string;
                PropertyInfo pi = modelType.GetProperty(key, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (pi != null)
                {
                    pi.SetValue(obj, CheckType(dict[key], pi.PropertyType), null);
                }
            }
            return obj;
        }
        #endregion

        #region 对象与对象转换
        public static T ConvertTo<T>(object src, bool props = true, bool fields = true)
        {
            var srcType = src.GetType();
            var dstType = typeof(T);
            var dst = (T)Activator.CreateInstance(dstType);
            if (props)
            {
                ConvertProperty(src, dst, srcType, dstType);
            }
            if (fields)
            {
                ConvertField(src, dst, srcType, dstType);
            }
            return dst;
        }

        private static void ConvertProperty(object src, object dst, Type srcType, Type dstType)
        {
            var dstProps = dstType.GetProperties();
            foreach (var dstProp in dstProps)
            {
                var name = dstProp.Name;
                var prop = srcType.GetProperty(name);
                if (prop == null)
                {
                    continue;
                }
                var vall = prop.GetValue(src, null);

                dstProp.SetValue(dst, CheckType(vall, dstProp.PropertyType), null);
            }
        }

        private static void ConvertField(object src, object dst, Type srcType, Type dstType)
        {
            var dstProps = dstType.GetFields();
            foreach (var dstProp in dstProps)
            {
                var name = dstProp.Name;
                var prop = srcType.GetField(name);
                if (prop == null)
                {
                    continue;
                }
                var vall = prop.GetValue(src);

                dstProp.SetValue(dst, CheckType(vall, dstProp.FieldType));
            }
        }

        private static object CheckType(object value, Type conversionType)
        {
            if (conversionType.IsGenericParameter && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }
        #endregion
        #endregion

        #region 数值转换
        /// <summary>
        ///  重量格式化（由克转换为千克），格式：0.000；
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static string FormatWeight(int weight)
        {
            return (weight / 1000f).ToString("f3");
        }

        /// <summary>
        /// 重量转换，由千克转换为克
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static int ParseWeight(string weight)
        {
            if (IsDecimal(weight, 3))
            {
                return (int)(double.Parse(weight) * 1000);
            }
            return 0;
        }

        /// <summary>
        /// 体积格式化（由立方厘米转为立方米），格式：0.000；
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static string FormatVolume(int volume)
        {
            return (volume / 1000000f).ToString("f3");
        }

        /// <summary>
        /// 体积转换，由立方米转为立方厘米
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static int ParseVolume(string volume)
        {
            if (IsDecimal(volume, 3))
            {
                return (int)(double.Parse(volume) * 1000000);
            }
            return 0;
        }

        /// <summary>
        /// 金额格式化（由分转换为元），格式：0.00；
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static string FormatPrice(int price)
        {
            return (price / 100f).ToString("f2");
        }

        /// <summary>
        /// 金额转换（由元转换为分）
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static int ParsePrice(string price)
        {
            if (IsDecimal(price, 3))
            {
                return (int)(double.Parse(price) * 100);
            }
            return 0;
        }
        #endregion

        #region 字符截取
        /// <summary>
        /// 取左侧不超指定长度的字符
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length">最大长度</param>
        /// <param name="append">超出时，追加提示字符串</param>
        /// <returns></returns>
        public static string Left(string text, int length, string append = "")
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            if (text.Length <= length)
            {
                return text;
            }

            if (append == null)
            {
                append = "";
            }
            if (length < append.Length)
            {
                return append;
            }
            return text.Substring(0, length - append.Length) + append;
        }

        /// <summary>
        /// 取左侧指定长度的字符，不足时以指定字符填充，超过时附加指定字符
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length">长度</param>
        /// <param name="pad">不足时，固定填充字符</param>
        /// <param name="append">超出时，追加提示字符串</param>
        /// <returns></returns>
        public static string Left(string text, int length, char pad, string append = "")
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            if (length < 1)
            {
                return text;
            }

            if (text.Length <= length)
            {
                return text.PadRight(length, pad);
            }

            if (append == null)
            {
                append = "";
            }
            if (length < append.Length)
            {
                return append;
            }

            return text.Substring(0, length - append.Length) + append;
        }

        /// <summary>
        /// 取右侧不超指定长度的字符
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length">最大长度</param>
        /// <param name="prepend">超出时，追加提示字符串</param>
        /// <returns></returns>
        public static string Right(string text, int length, string prepend = "")
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            if (text.Length <= length)
            {
                return text;
            }

            if (prepend == null)
            {
                prepend = "";
            }
            if (length < prepend.Length)
            {
                return prepend;
            }

            var len = length - prepend.Length;
            return prepend + text.Substring(text.Length - len - 1, len);
        }

        /// <summary>
        /// 取左侧指定长度的字符，不足时以指定字符填充，超过时附加指定字符
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length">长度</param>
        /// <param name="pad">不足时，固定填充字符</param>
        /// <param name="prepend">超出时，追加提示字符串</param>
        /// <returns></returns>
        public static string Right(string text, int length, char pad, string prepend = "")
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            if (length < 1)
            {
                return text;
            }

            if (text.Length <= length)
            {
                return text.PadLeft(length, pad);
            }

            if (prepend == null)
            {
                prepend = "";
            }
            if (length < prepend.Length)
            {
                return prepend;
            }

            var len = length - prepend.Length;
            return prepend + text.Substring(text.Length - len - 1, len);
        }
        #endregion

        #region 掩码处理
        /// <summary>
        /// 字符串掩码，含手机号、用户名等
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="mask">掩码符</param>
        /// <returns></returns>
        public static string Mask(string s, char mask = '*')
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return s;
            }

            s = s.Trim();
            string masks = mask.ToString().PadLeft(4, mask);

            var len = s.Length;
            if (len >= 11)
            {
                return Regex.Replace(s, "(.{3}).*(.{4})", $"$1{masks}$2");
            }
            if (len == 10)
            {
                return Regex.Replace(s, "(.{3}).*(.{3})", $"$1{masks}$2");
            }
            if (len == 9)
            {
                return Regex.Replace(s, "(.{2}).*(.{3})", $"$1{masks}$2");
            }
            if (len == 8)
            {
                return Regex.Replace(s, "(.{2}).*(.{2})", $"$1{masks}$2");
            }
            if (len == 7)
            {
                return Regex.Replace(s, "(.{1}).*(.{2})", $"$1{masks}$2");
            }
            if (len == 6)
            {
                return Regex.Replace(s, "(.{1}).*(.{1})", $"$1{masks}$2");
            }
            return Regex.Replace(s, "(.{1}).*", $"$1{masks}");
        }

        /// <summary>
        /// 手机号码掩码处理
        /// </summary>
        /// <param name="s"></param>
        /// <param name="masks"></param>
        /// <returns></returns>
        public static string MaskCellphone(string s, char masks = '*')
        {
            if (IsCellphone(s))
            {
                return Regex.Replace(s, "^(.{3}).*(.{4})$", $"$1{masks}$2");
            }
            return s;
        }

        /// <summary>
        /// 电子邮件掩码处理
        /// </summary>
        /// <param name="s"></param>
        /// <param name="masks"></param>
        /// <returns></returns>
        public static string MaskEmail(string s, char masks = '*')
        {
            if (IsEmail(s))
            {
                return Regex.Replace(s, "^(.{2})[^@]+(.+)", $"$1{masks}$2");
            }
            return s;
        }
        #endregion

        #region 格式校验
        /// <summary>
        /// 判断是否为合法的电子邮件，支持格式如下：
        /// someone@host.com
        /// some.one@host.com.cn
        /// some_one@host.com.cn
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmail(string s)
        {
            return Regex.IsMatch(s, @"^\w+[-.\w]*@\w+[-\w]*(\.\w+[-\w])+$");
        }

        /// <summary>
        /// 是否为中国的手机号码，格式如下：
        /// 130XXXXXXXX
        /// 199XXXXXXXX
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsCellphone(string s)
        {
            return Regex.IsMatch(s, "^1[3-9]\\d{9}$");
        }

        /// <summary>
        /// 是否为中国的固话号码，格式如下：
        /// XXXX-XXXXXXXX
        /// XXX-XXXXXXXX
        /// XXXX-XXXXXXX
        /// (XXXX)XXXXXXXX
        /// (XXXX)XXXXXXX
        /// (XXX)XXXXXXXX
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsTelephone(string s)
        {
            return Regex.IsMatch(s, @"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$");
        }

        /// <summary>
        /// 是否为中的电话号码（含手机和固话）
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsPhone(string s)
        {
            return IsTelephone(s) || IsCellphone(s);
        }

        /// <summary>
        /// 是否为浮点数，支持+-符号，格式如下：
        /// 123
        /// 123.456
        /// -123.456
        /// </summary>
        /// <param name="t"></param>
        /// <param name="t">是否包含符号</param>
        /// <returns></returns>
        public static bool IsDecimal(string t, bool s = false)
        {
            var tmp = (s ? "[-+]?" : "");
            return t != null && Regex.IsMatch(t, "^(" + tmp + @"\d+)(.\d+)?$");
        }

        /// <summary>
        /// 不超过n位小数的浮点数，支持+-符号
        /// </summary>
        /// <param name="t"></param>
        /// <param name="l">小数位数</param>
        /// <param name="s">是否包含符号</param>
        /// <returns></returns>
        public static bool IsDecimal(string t, int l, bool s = false)
        {
            var tmp = (s ? "[-+]?" : "");
            return t != null && Regex.IsMatch(t, "^(" + tmp + @"\d+)(.\d{1," + l + "})?$");
        }

        /// <summary>
        /// 是否为纯数字（整数），不支持+-符号
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s">是否包含符号</param>
        /// <returns></returns>
        public static bool IsNumberic(string t, bool s = false)
        {
            var tmp = (s ? "[-+]?" : "");
            return t != null && Regex.IsMatch(t, "^" + tmp + @"\d+$");
        }

        /// <summary>
        /// 是否为指定位数的整数
        /// </summary>
        /// <param name="t">待判断字符串</param>
        /// <param name="l">字符串长度</param>
        /// <param name="s">是否包含符号</param>
        /// <returns></returns>
        public static bool IsNumberic(string t, int l, bool s = false)
        {
            var tmp = (s ? "[-+]?" : "");
            return t != null && Regex.IsMatch(t, "^" + tmp + @"\d{" + l + "}$");
        }

        /// <summary>
        /// 是否为指定位数的整数
        /// </summary>
        /// <param name="t">待判断字符串</param>
        /// <param name="m">最小长度</param>
        /// <param name="n">最大长度</param>
        /// <param name="s">是否包含符号</param>
        /// <returns></returns>
        public static bool IsNumberic(string t, int m, int n, bool s = false)
        {
            var tmp = (s ? "[-+]?" : "");
            return t != null && Regex.IsMatch(t, "^" + tmp + @"\d{" + m + "," + n + "}$");
        }

        /// <summary>
        /// -2,147,483,648 到 2,147,483,647
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s">是否包含符号</param>
        /// <returns></returns>
        public static bool IsInteger(string t, bool s = false)
        {
            var tmp = (s ? "[-+]?" : "");
            return t != null && Regex.IsMatch(t, "^(" + tmp + @"[12]\d{9}|\d{1,9})$");
        }

        /// <summary>
        /// -9,223,372,036,854,775,808 到 9,223,372,036,854,775,807
        /// </summary>
        /// <param name="s"></param>
        /// <param name="n">是否包含符号</param>
        /// <returns></returns>
        public static bool IsLong(string s, bool n = false)
        {
            var tmp = (n ? "[-+]?" : "");
            return s != null && Regex.IsMatch(s, "^" + tmp + @"\d{1,18}$");
        }

        /// <summary>
        /// -128 到 127
        /// </summary>
        /// <param name="s"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool IsSByte(string s, bool n = false)
        {
            var tmp = (n ? "[-+]?" : "");
            return s != null && Regex.IsMatch(s, "^(" + tmp + @"[1]\d{2}|\d{1,2})$");
        }

        /// <summary>
        /// 0 到 255
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsUByte(string s)
        {
            return s != null && Regex.IsMatch(s, @"^([12]\d{2}|\d{1,2})$");
        }

        /// <summary>
        /// 是否为金额，支持最大9位整数和2位小数，格式如下：
        /// 123
        /// 123.01
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsPrice(string s)
        {
            return Regex.IsMatch(s, @"^\d{1,9}(.\d{1,2})?$");
        }

        /// <summary>
        /// 是否为重量，支持最大9位整数和3位小数，格式如下：
        /// 123
        /// 123.01
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsWeight(string s)
        {
            return Regex.IsMatch(s, @"^\d{1,9}(.\d{1,3})?$");
        }

        /// <summary>
        /// 是否为体积，支持最大9位整数和3位小数，格式如下：
        /// 123
        /// 123.01
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsVolume(string s)
        {
            return Regex.IsMatch(s, @"^\d{1,9}(.\d{1,3})?$");
        }

        /// <summary>
        /// 是否为URL
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsUrl(string s)
        {
            return Regex.IsMatch(s, @"^[a-zA-Z]+://\S+$");
        }

        /// <summary>
        /// 是否为中文字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsChineseCharacter(string s)
        {
            return Regex.IsMatch(s, @"^[\u4e00-\u9fa5]+$");
        }

        /// <summary>
        /// 是否为中文或数值
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsChineseOrNumberic(string s)
        {
            return Regex.IsMatch(s, @"^[0-9\u4e00-\u9fa5]+$");
        }

        /// <summary>
        /// 是否为英文或数值
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEnglishOrNumberic(string s)
        {
            return Regex.IsMatch(s, @"^[A-Za-z0-9]+$");
        }

        /// <summary>
        /// 是否为日期，支持格式：
        /// yyyy-MM-dd
        /// yyyy/MM/dd
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsDate(string s)
        {
            return Regex.IsMatch(s, @"^\d{4}[-\/]\d{1,2}[-\/]\d{1,2}$");
        }

        /// <summary>
        /// 是否为时间，支持格式
        /// HH:mm:ss
        /// HH-mm-ss
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsTime(string s)
        {
            return Regex.IsMatch(s, @"^\d{2}[-:]\d{1,2}[-:]\d{1,2}$");
        }

        /// <summary>
        /// 是否为日期时间
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsDateTime(string s)
        {
            return Regex.IsMatch(s, @"^\d{4}[-\/]\d{1,2}[-\/]\d{1,2}\s\d{2}[-:]\d{1,2}[-:]\d{1,2}$");
        }

        #region 权威校验身份证号码
        /// <summary>
        /// 是否为中国身份证号码
        /// 根据GB11643-1999标准权威校验中国身份证号码的合法性
        /// </summary>
        /// <param name="text">源字符串</param>
        /// <param name="formatOnly">是否仅校验格式</param>
        /// <returns>是否匹配成功</returns>
        public static bool IsChinaID(string text, bool formatOnly = true)
        {
            if (formatOnly)
            {
                return Regex.IsMatch(text, @"^[1-9]\d{14}(\d{2}[0-9Xx])?$");
            }

            if (text.Length == 18)
            {
                return CheckChinaID18(text);
            }
            if (text.Length == 15)
            {
                return CheckChinaID15(text);
            }
            return false;
        }

        private static readonly string[] ChinaIDProvinceCodes = {
             "11", "12", "13", "14", "15",
             "21", "22", "23",
             "31", "32", "33", "34", "35", "36", "37",
             "41", "42", "43", "44", "45", "46",
             "50", "51", "52", "53", "54",
             "61", "62", "63", "64", "65",
             "71",
             "81", "82",
             "91"
        };

        private static bool CheckChinaID18(string ID)
        {
            ID = ID.ToUpper();
            Match m = Regex.Match(ID, @"\d{17}[\dX]", RegexOptions.IgnoreCase);
            if (!m.Success)
            {
                return false;
            }
            if (!ChinaIDProvinceCodes.Contains(ID.Substring(0, 2)))
            {
                return false;
            }
            CultureInfo zhCN = new CultureInfo("zh-CN", true);
            if (!DateTime.TryParseExact(ID.Substring(6, 8), "yyyyMMdd", zhCN, DateTimeStyles.None, out DateTime birthday))
            {
                return false;
            }
            if (!IsBetween(birthday, new DateTime(1800, 1, 1), DateTime.Today))
            {
                return false;
            }
            int[] factors = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += (ID[i] - '0') * factors[i];
            }
            int n = (12 - sum % 11) % 11;
            return n < 10 ? ID[17] - '0' == n : ID[17].Equals('X');
        }

        private static bool CheckChinaID15(string ID)
        {
            Match m = Regex.Match(ID, @"\d{15}", RegexOptions.IgnoreCase);
            if (!m.Success)
            {
                return false;
            }
            if (!ChinaIDProvinceCodes.Contains(ID.Substring(0, 2)))
            {
                return false;
            }
            CultureInfo zhCN = new CultureInfo("zh-CN", true);
            if (!DateTime.TryParseExact("19" + ID.Substring(6, 6), "yyyyMMdd", zhCN, DateTimeStyles.None, out DateTime birthday))
            {
                return false;
            }
            return IsBetween(birthday, new DateTime(1800, 1, 1), new DateTime(2000, 1, 1));
        }

        private static bool IsBetween(DateTime time, DateTime start, DateTime end)
        {
            return time >= start && time <= end;
        }
        #endregion 权威校验身份证号码

        #endregion

        public static string Random(string marks, int length, bool repeatable = true)
        {
            if (marks.Length < 1)
            {
                throw new Exception("掩码字符不能为空！");
            }

            if (!repeatable && marks.Length < length)
            {
                throw new Exception("掩码字符空间不足！");
            }

            var random = new Random();

            var builder = new StringBuilder();
            var index = 0;
            if (repeatable)
            {
                for (var i = 0; i < length; i++)
                {
                    index = random.Next(marks.Length);
                    builder.Append(marks[index]);
                }
            }
            else
            {
                var chars = marks.ToList();
                for (var i = 0; i < length; i++)
                {
                    index = random.Next(chars.Count);
                    builder.Append(marks[index]);
                    chars.RemoveAt(index);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// 随机数字串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomNumber(int length = 6)
        {
            string chars = "0123456789";
            return Random(chars, length, true);
        }

        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public static string RandomString(int length = 8, bool upper = true)
        {
            string chars = upper ? "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ" : "0123456789abcdefghijklmnopqrstuvwxyz";
            return Random(chars, length, true);
        }

        public static bool IsHexGuid(string text)
        {
            return Regex.IsMatch(text, @"^[-_\da-fA-F]{32}$");
        }

        /// <summary>
        /// 基于GUID的字符串
        /// </summary>
        /// <param name="upperCase">是否大写</param>
        /// <returns></returns>
        public static string GuidString(bool upperCase = false)
        {
            return ToHexString(Guid.NewGuid().ToByteArray(), upperCase);
        }

        public static bool IsHexTime(string text)
        {
            return Regex.IsMatch(text, @"^[-_\da-fA-F]{16}$");
        }

        /// <summary>
        /// 基于时间的字符串
        /// </summary>
        /// <param name="upperCase">是否大写</param>
        /// <returns></returns>
        public static string TimeString(bool upperCase = false)
        {
            var format = upperCase ? "X" : "x";
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(format + "16");
        }

        #region 过滤emoji字符
        public static string FilterEmoji(string str)
        {

            var origin = str;
            try
            {
                foreach (var a in str)
                {
                    byte[] bts = Encoding.UTF32.GetBytes(a.ToString());
                    if (bts[0] == 253 && bts[1] == 255)
                    {
                        str = str.Replace(a.ToString(), "");
                    }
                }
            }
            catch (Exception)
            {
                str = origin;
            }
            return str;
        }
        #endregion

        #region HTML标记
        /// <summary>
        /// 转换成 HTML code
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string EncodeHtml(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }

        /// <summary>
        ///解析html成 普通文本
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string DecodeHtml(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }

        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="htmlstring"></param>
        /// <returns></returns>
        public static string DropHtml(string htmlstring)
        {
            if (string.IsNullOrEmpty(htmlstring)) return "";
            //删除脚本  
            htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML  
            htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

            htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            htmlstring = htmlstring.Replace("<", "");
            htmlstring = htmlstring.Replace(">", "");
            htmlstring = htmlstring.Replace("\r\n", "");
            //htmlstring = HttpContext.Current.Server.HtmlEncode(htmlstring).Trim(); 
            return htmlstring;
        }
        #endregion

        #region 密码掩码处理
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static string EncodePass(string pass)
        {
            var len1 = 4;
            var len2 = 4;
            var len3 = 4;
            var len = pass.Length >> 1;

            var random = new Random();
            var idx1 = random.Next(len);
            var idx2 = random.Next(len) + len1 + idx1;

            var tmp1 = RandomString(len1, false);
            var tmp2 = RandomString(len2, false);
            var tmp3 = RandomString(len3, false);

            var buf = new StringBuilder(pass);
            buf.Insert(idx1, tmp1).Insert(idx2, tmp2).Append(tmp3).Insert(0, idx1.ToString("x2") + idx2.ToString("x2"));
            return buf.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static string DecodePass(string pass)
        {
            if (pass == null || pass.Length != 80)
            {
                return pass;
            }

            var len1 = 4;
            var len2 = 4;
            var len3 = 4;

            var tmp1 = pass.Substring(0, 2);
            var tmp2 = pass.Substring(2, 2);
            var idx1 = int.Parse(tmp1, NumberStyles.HexNumber);
            var idx2 = int.Parse(tmp2, NumberStyles.HexNumber);

            var buf = new StringBuilder(pass);
            buf.Remove(0, 4).Remove(buf.Length - len3, len3).Remove(idx2, len2).Remove(idx1, len1);
            return buf.ToString();
        }
        #endregion

        /// <summary>
        /// 字符摘要（MD5）
        /// </summary>
        /// <param name="text"></param>
        /// <param name="upperCase"></param>
        /// <returns></returns>
        public static string Md5(string text, bool upperCase = false)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return "";
            }

            var alg = MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(text);
            var result = alg.ComputeHash(bytes);

            return ToHexString(result, upperCase);
        }

        /// <summary>
        /// 字符摘要（SHA）
        /// </summary>
        /// <param name="text"></param>
        /// <param name="upperCase"></param>
        /// <returns></returns>
        public static string Sha(string text, bool upperCase = false)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return "";
            }

            var alg = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(text);
            var result = alg.ComputeHash(bytes);

            return ToHexString(result, upperCase);
        }

        /// <summary>
        /// 字符摘要
        /// </summary>
        /// <param name="text"></param>
        /// <param name="cipher"></param>
        /// <param name="upperCase"></param>
        /// <returns></returns>
        public static string Digest(string text, string cipher, bool upperCase = false)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return "";
            }

            var alg = HashAlgorithm.Create(cipher);
            var bytes = Encoding.UTF8.GetBytes(text);
            var result = alg.ComputeHash(bytes);

            return ToHexString(result, upperCase);
        }
    }
}