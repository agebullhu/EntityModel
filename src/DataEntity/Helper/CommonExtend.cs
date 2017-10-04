// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     公共方法的一些扩展
    /// </summary>
    public static class CommonExtend
    {
        /// <summary>
        ///     正确安全的转为小数
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="def">无法转换时的缺省值</param>
        /// <returns>小数</returns>
        public static decimal ToDecimal(this object obj, decimal def = 0)
        {
            if (obj == null)
            {
                return def;
            }
            if (obj is decimal)
            {
                return (decimal) obj;
            }
            decimal re;
            return decimal.TryParse(obj.ToString().Trim(), out re) ? re : def;
        }

        /// <summary>
        ///     正确安全的转为小数
        /// </summary>
        /// <param name="str">文本对象</param>
        /// <param name="def">无法转换时的缺省值</param>
        /// <returns>小数</returns>
        public static decimal ToDecimal(this string str, decimal def = 0)
        {
            if (str == null)
            {
                return def;
            }
            decimal re;
            return decimal.TryParse(str.Trim(), out re) ? re : def;
        }

        /// <summary>
        ///     正确安全的转为整数
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="def">无法转换时的缺省值</param>
        /// <returns>整数</returns>
        public static int ToInteger(this object obj, int def = 0)
        {
            if (obj == null)
            {
                return def;
            }
            if (obj is int)
            {
                return (int) obj;
            }
            int re;
            return int.TryParse(obj.ToString().Trim(), out re) ? re : def;
        }

        /// <summary>
        ///     正确安全的转为整数
        /// </summary>
        /// <param name="str">文本对象</param>
        /// <param name="def">无法转换时的缺省值</param>
        /// <returns>整数</returns>
        public static int ToInteger(this string str, int def = 0)
        {
            if (str == null)
            {
                return def;
            }
            int re;
            return int.TryParse(str.Trim(), out re) ? re : def;
        }

        /// <summary>
        ///     正确安全的转为整数数组
        /// </summary>
        /// <param name="str">文本对象(数字以,分开)</param>
        /// <returns>整数数组</returns>
        public static int[] ToIntegers(this string str)
        {
            return str == null
                ? new int[0]
                : str.Trim().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }

        /// <summary>
        ///     正确安全的转为文本
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="def">无法转换时的缺省值</param>
        /// <returns>文本</returns>
        public static string ToSafeString(this object obj, string def = "")
        {
            return obj == null ? def : obj.ToString();
        }

        /// <summary>
        /// 获取枚举类型的值和描述集合
        /// </summary>
        /// <param name="enumObj">目标枚举对象</param>
        /// <returns>值和描述的集合</returns>
        public static Dictionary<int, string> GetEnumValAndDescList(this Enum enumObj)
        {
            Dictionary<int, string> enumDic = new Dictionary<int, string>();
            Type enumType = enumObj.GetType();
            List<int> enumValues = enumType.GetEnumValues().Cast<int>().ToList();

            enumValues.ForEach(item =>
            {
                int key = (int)item;
                string text = enumType.GetEnumName(key);
                string descText = enumType.GetField(text).GetCustomAttributes(typeof(DescriptionAttribute),
                    false).Cast<DescriptionAttribute>().FirstOrDefault()?.Description;

                text = string.IsNullOrWhiteSpace(descText) ? text : descText;

                enumDic.Add(key, text);
            });

            return enumDic;
        }

        /// <summary>
        /// 获取特定枚举值的描述
        /// </summary>
        /// <param name="enumObj">目标枚举对象</param>
        /// <param name="val">枚举值</param>
        /// <returns>枚举值的描述</returns>
        public static string GetEnumValDesc(this Enum enumObj, object val)
        {
            Type enumType = enumObj.GetType();
            string text = enumType.GetEnumName(val);
            string descText = enumType.GetField(text).GetCustomAttributes(typeof(DescriptionAttribute),
                    false).Cast<DescriptionAttribute>().FirstOrDefault()?.Description;
            text = string.IsNullOrWhiteSpace(descText) ? text : descText;

            return text;
        }
    }
}