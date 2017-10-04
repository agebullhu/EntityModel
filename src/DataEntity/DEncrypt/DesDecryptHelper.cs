// /***********************************************************************************************************************
// 工程：GameApiFoundation
// 项目：WebServer
// 文件：DesDecryptHelper.cs
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
using System.IO;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace HY.Web.Apis
{
    public static class DesDecryptHelper
    {
        private static readonly byte[] Kv = Encoding.UTF8.GetBytes("liuyou!j");

        private static readonly byte[] Iv = {0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF};

        public static string DesDecrypt(string val)
        {
            return DesDecryptInner(DesDecryptInner(val)).Trim('[', ']');
        }

        public static string DesDecryptInner(string val)
        {
            var des = new DESCryptoServiceProvider();
            var inputByteArray = Convert.FromBase64String(val);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(Kv, Iv), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            Encoding encoding = new UTF8Encoding();
            return encoding.GetString(ms.ToArray());
        }
        public static string Encryptor(string val)
        {
            return EncryptorInner(EncryptorInner(val));
        }
        public static string EncryptorInner(string val)
        {
            var des = new DESCryptoServiceProvider();
            var inputByteArray = Encoding.UTF8.GetBytes(val);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(Kv, Iv), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}