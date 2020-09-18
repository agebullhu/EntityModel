// /***********************************************************************************************************************
// 工程：GameApiFoundation
// 项目：WebServer
// 文件：DEncrypt.cs
// 作者：Administrator/
// 建立：2015－04－23 20:15
// ****************************************************文件说明**********************************************************
// 对应文档：
// 说明摘要：
// 作者备注：
// ****************************************************修改记录**********************************************************
// 日期：
// 人员：
// 说明：
// ************************************************************************************************************************
// 日期：
// 人员：
// 说明：
// ************************************************************************************************************************
// 日期：
// 人员：
// 说明：
// ***********************************************************************************************************************/

#region 命名空间引用

using System;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace Maticsoft.Common.DEncrypt
{
    /// <summary>
    ///     Encrypt 的摘要说明。
    ///     Copyright (C) Maticsoft
    /// </summary>
    public class DEncrypt
    {
        #region 使用 缺省密钥字符串 加密/解密string

        /// <summary>
        ///     使用缺省密钥字符串加密string
        /// </summary>
        /// <param name="original">明文</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original)
        {
            return Encrypt(original, "MATICSOFT");
        }

        /// <summary>
        ///     使用缺省密钥字符串解密string
        /// </summary>
        /// <param name="original">密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string original)
        {
            return Decrypt(original, "MATICSOFT", Encoding.Default);
        }

        #endregion

        #region 使用 给定密钥字符串 加密/解密string

        /// <summary>
        ///     使用给定密钥字符串加密string
        /// </summary>
        /// <param name="original">原始文字</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original, string key)
        {
            byte[] buff = Encoding.Default.GetBytes(original);
            byte[] kb = Encoding.Default.GetBytes(key);
            return Convert.ToBase64String(Encrypt(buff, kb));
        }

        /// <summary>
        ///     使用给定密钥字符串解密string
        /// </summary>
        /// <param name="original">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string Decrypt(string original, string key)
        {
            return Decrypt(original, key, Encoding.Default);
        }

        /// <summary>
        ///     使用给定密钥字符串解密string,返回指定编码方式明文
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>明文</returns>
        public static string Decrypt(string encrypted, string key, Encoding encoding)
        {
            byte[] buff = Convert.FromBase64String(encrypted);
            byte[] kb = Encoding.Default.GetBytes(key);
            return encoding.GetString(Decrypt(buff, kb));
        }

        #endregion

        #region 使用 缺省密钥字符串 加密/解密/byte[]

        /// <summary>
        ///     使用缺省密钥字符串解密byte[]
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[] Decrypt(byte[] encrypted)
        {
            byte[] key = Encoding.Default.GetBytes("MATICSOFT");
            return Decrypt(encrypted, key);
        }

        /// <summary>
        ///     使用缺省密钥字符串加密
        /// </summary>
        /// <param name="original">原始数据</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(byte[] original)
        {
            byte[] key = Encoding.Default.GetBytes("MATICSOFT");
            return Encrypt(original, key);
        }

        #endregion

        #region  使用 给定密钥 加密/解密/byte[]

        /// <summary>
        ///     生成MD5摘要
        /// </summary>
        /// <param name="original">数据源</param>
        /// <returns>摘要</returns>
        public static byte[] MakeMD5(byte[] original)
        {
            var hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyhash = hashmd5.ComputeHash(original);
            hashmd5 = null;
            return keyhash;
        }


        /// <summary>
        ///     使用给定密钥加密
        /// </summary>
        /// <param name="original">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(byte[] original, byte[] key)
        {
            var des = new TripleDESCryptoServiceProvider();
            des.Key = MakeMD5(key);
            des.Mode = CipherMode.ECB;

            return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
        }

        /// <summary>
        ///     使用给定密钥解密数据
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            var des = new TripleDESCryptoServiceProvider();
            des.Key = MakeMD5(key);
            des.Mode = CipherMode.ECB;

            return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
        }

        #endregion
    }
}