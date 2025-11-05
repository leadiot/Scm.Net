using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Com.Scm.Utils
{
    public class SecUtils
    {
        private static readonly string Default_AES_Key = "@Scm.Net";

        private static byte[] Keys = new byte[16]
        {
            64, 83, 99, 109, 46, 78, 101, 116,
            64, 83, 99, 109, 46, 78, 101, 116,
        };

        #region AES算法
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static byte[] AesEncrypt(byte[] encryptData)
        {
            return AesEncrypt(encryptData, Default_AES_Key);
        }

        public static byte[] AesEncrypt(byte[] encryptData, string encryptKey, CipherMode mode = CipherMode.CBC)
        {
            if (encryptData == null || encryptData.Length < 1)
            {
                return null;
            }

            if (string.IsNullOrEmpty(encryptKey))
            {
                encryptKey = Default_AES_Key;
            }
            encryptKey = encryptKey.PadRight(32, ' ');

            var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32));
            aes.IV = Keys;
            aes.Mode = mode;

            var cryptoTransform = aes.CreateEncryptor();

            return cryptoTransform.TransformFinalBlock(encryptData, 0, encryptData.Length);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="decryptData"></param>
        /// <returns></returns>
        public static byte[] AesDecrypt(byte[] decryptData)
        {
            return AesDecrypt(decryptData, Default_AES_Key);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="decryptData"></param>
        /// <param name="decryptKey"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static byte[] AesDecrypt(byte[] decryptData, string decryptKey, CipherMode mode = CipherMode.CBC)
        {
            if (decryptData == null || decryptData.Length < 1)
            {
                return null;
            }

            if (string.IsNullOrEmpty(decryptKey))
            {
                decryptKey = Default_AES_Key;
            }
            decryptKey = decryptKey.PadRight(32, ' ');

            var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 32));
            aes.IV = Keys;
            aes.Mode = mode;

            var cryptoTransform = aes.CreateDecryptor();

            return cryptoTransform.TransformFinalBlock(decryptData, 0, decryptData.Length);
        }
        #endregion

        #region 消息摘要
        /// <summary>
        /// MD5摘要
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5(string input)
        {
            if (input == null)
            {
                return null;
            }
            return Md5(Encoding.UTF8.GetBytes(input));
        }

        public static string Md5(Stream stream)
        {
            var alg = MD5.Create();
            byte[] bytes = alg.ComputeHash(stream);
            return TextUtils.ToHexString(bytes);
        }

        public static string Md5(byte[] bytes)
        {
            var alg = MD5.Create();
            bytes = alg.ComputeHash(bytes);
            return TextUtils.ToHexString(bytes);
        }

        public static string Sha256(string input)
        {
            var alg = SHA256.Create();
            byte[] bytes = alg.ComputeHash(Encoding.UTF8.GetBytes(input));
            return TextUtils.ToHexString(bytes);
        }

        public static string Sha256(Stream stream)
        {
            var alg = SHA256.Create();
            byte[] bytes = alg.ComputeHash(stream);
            return TextUtils.ToHexString(bytes);
        }

        public static string Sha256(byte[] bytes)
        {
            var alg = SHA256.Create();
            bytes = alg.ComputeHash(bytes);
            return TextUtils.ToHexString(bytes);
        }
        #endregion

        #region 掩码处理
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

            var tmp1 = TextUtils.RandomString(len1, false);
            var tmp2 = TextUtils.RandomString(len2, false);
            var tmp3 = TextUtils.RandomString(len3, false);

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
    }
}
