// /***********************************************************************************************************************
// 工程：GameApiFoundation
// 项目：WebServer
// 文件：HashEncode.cs
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
    ///     得到随机安全码（哈希加密）。
    /// </summary>
    public class HashEncode
    {
        /// <summary>
        ///     得到随机哈希加密字符串
        /// </summary>
        /// <returns></returns>
        public static string GetSecurity()
        {
            string Security = HashEncoding(GetRandomValue());
            return Security;
        }

        /// <summary>
        ///     得到一个随机数值
        /// </summary>
        /// <returns></returns>
        public static string GetRandomValue()
        {
            var Seed = new Random();
            string RandomVaule = Seed.Next(1, int.MaxValue).ToString();
            return RandomVaule;
        }

        /// <summary>
        ///     哈希加密一个字符串
        /// </summary>
        /// <param name="Security"></param>
        /// <returns></returns>
        public static string HashEncoding(string Security)
        {
            byte[] Value;
            var Code = new UnicodeEncoding();
            byte[] Message = Code.GetBytes(Security);
            var Arithmetic = new SHA512Managed();
            Value = Arithmetic.ComputeHash(Message);
            Security = "";
            foreach (byte o in Value)
            {
                Security += (int) o + "O";
            }
            return Security;
        }
    }
}