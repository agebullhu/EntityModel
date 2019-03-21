// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Text;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 随机字符串生成器
    /// </summary>
    public class RandomOperate
    {
        /// <summary>
        /// 基准数字
        /// </summary>
        private static readonly long BaseTicks;
        /// <summary>
        /// 内部构造
        /// </summary>
        static RandomOperate()
        {
            BaseTicks = new DateTime(2015, 1, 1).Ticks;
        }
        /// <summary>
        /// 内部构架
        /// </summary>
        private RandomOperate()
        {
        }
        /// <summary>
        /// 字符
        /// </summary>
        private static readonly char[] keys = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'i', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };//'O',
        /// <summary>
        /// 随机生成字符串（数字和字母混和）
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string Generate(int codeCount)
        {
            return new RandomOperate().GenerateCode(codeCount);
        }

        //随机生成字符串（数字和字母混和）
        string GenerateCode(int codeCount)
        {
            var str = new StringBuilder();
            var random1 = new Random((int)(DateTime.Now.Ticks - BaseTicks));
            var random2 = new Random(GetHashCode());
            for (var i = 0; i < codeCount; i += 2)
            {
                str.Append(keys[random1.Next(keys.Length)]);
                str.Append(keys[random2.Next(keys.Length)]);
            }
            return str.ToString();
        }
    }
}
