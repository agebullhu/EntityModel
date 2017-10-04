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
using Agebull.Common.Base;

#endregion

namespace Gboxt.Common.DataModel
{
    public class RandomOperate
    {
        private static readonly long BaseTicks;
        static RandomOperate()
        {
            BaseTicks = new DateTime(2015, 1, 1).Ticks;
        }
        private RandomOperate()
        {
        }
        private static readonly char[] keys = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'i', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        //随机生成字符串（数字和字母混和）
        public static string Generate(int codeCount)
        {
            return new RandomOperate().GenerateCode(codeCount);
        }
        
        //随机生成字符串（数字和字母混和）
        string GenerateCode(int codeCount)
        {
            StringBuilder str = new StringBuilder();
            Random random1 = new Random((int)(DateTime.Now.Ticks - BaseTicks));
            Random random2 = new Random(GetHashCode());
            for (int i = 0; i < codeCount; i += 2)
            {
                str.Append(keys[random1.Next(36)]);
                str.Append(keys[random2.Next(36)]);
            }
            return str.ToString();
        }
    }
}
