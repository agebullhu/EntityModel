// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

#endregion

namespace Agebull.Common.Base
{
    /// <summary>
    ///   实体的帮助类
    /// </summary>
    public class TypeHelper
    {
        #region 格式化

        /// <summary>
        ///   格式化时间
        /// </summary>
        /// <param name="value"> </param>
        /// <param name="fmt"> </param>
        /// <returns> </returns>
        public static string FormatDateTime(object value, string fmt)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }
            if (!(value is DateTime?))
            {
                try
                {
                    return Convert.ToDateTime(value).ToString(fmt);
                }
                catch
                {
                    return null;
                }
            }
            var nv = value as DateTime?;
            return nv.Value.ToString(fmt);
        }

        /// <summary>
        ///   格式化实数
        /// </summary>
        /// <param name="value"> </param>
        /// <param name="f"> </param>
        /// <returns> </returns>
        public static string FormatDecimal(object value, int f)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            if (value is decimal nv)
            {
                return FormatDecimal(nv, f);
            }

            return decimal.TryParse(value.ToString(), out var v)
                           ? FormatDecimal(v, f)
                           : null;
        }

        /// <summary>
        ///   格式化实数
        /// </summary>
        /// <param name="value"> </param>
        /// <param name="f"> </param>
        /// <returns> </returns>
        public static string FormatDecimal(decimal value, int f)
        {
            if (value == 0M)
            {
                return "0";
            }
            if (f < 0)
            {
                return long.TryParse(value.ToString(CultureInfo.InvariantCulture), out var v)
                               ? f.ToString(CultureInfo.InvariantCulture)
                               : null;
            }
            var s = value.ToString(CultureInfo.InvariantCulture);
            var dot = s.IndexOf('.');
            if (dot < 0)
            {
                return s;
            }
            var i = dot + f;
            if (i > (s.Length - 1))
            {
                i = s.Length - 1;
            }
            for (; i > dot; i--)
            {
                if (s[i] == '0')
                {
                    continue;
                }
                i++;
                break;
            }
            return s.Substring(0, i);
        }

        #endregion

        #region 文本到对象(不可空)

        /// <summary>
        ///   文本到文本
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static string StringToString(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                           ? null
                           : value.Trim();
        }

        /// <summary>
        ///   文本到long
        /// </summary>
        /// <param name="value"> </param>
        /// <param name="def"></param>
        /// <returns> </returns>
        public static long StringToInt64(string value, long def = 0)
        {
            if (string.IsNullOrWhiteSpace(value)) return def;
            if (Int64.TryParse(value, out var d))
            {
                return d;
            }
            return def;
        }

        /// <summary>
        ///   文本到int
        /// </summary>
        /// <param name="value"> </param>
        /// <param name="def"></param>
        /// <returns> </returns>
        public static int StringToInt32(string value, int def = 0)
        {
            if (string.IsNullOrWhiteSpace(value)) return def;
            if (Int32.TryParse(value, out var d))
            {
                return d;
            }
            return def;
        }

        /// <summary>
        ///   文本到short
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static short StringToInt16(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (Int16.TryParse(value, out var d))
                {
                    return d;
                }
            }
            return 0;
        }

        /// <summary>
        ///   文本到byte
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static byte StringToByte(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (byte.TryParse(value, out var d))
                {
                    return d;
                }
            }
            return 0;
        }

        /// <summary>
        ///   文本到decimal
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static decimal StringToDecimal(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (decimal.TryParse(value, out var d))
                {
                    return d;
                }
            }
            return 0M;
        }

        /// <summary>
        ///   文本到double
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static Double StringToDouble(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (Double.TryParse(value, out var d))
                {
                    return d;
                }
            }
            return 0D;
        }

        /// <summary>
        ///   文本到float
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static float StringToFloat(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (float.TryParse(value, out var d))
                {
                    return d;
                }
            }
            return 0F;
        }

        /// <summary>
        ///   文本到float
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static DateTime StringToDateTime(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (DateTime.TryParse(value, out var d))
                {
                    return d;
                }
            }
            return new DateTime();
        }

        /// <summary>
        ///   文本到char
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static Char StringToChar(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value[0];
            }
            return (char)0;
        }

        /// <summary>
        ///   文本到布尔
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static bool StringToBoolean(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (value == "on" || value == "yes")
                {
                    return true;
                }
                if (value == "off" || value == "no")
                {
                    return false;
                }
                if (int.TryParse(value, out var i))
                {
                    return i != 0;
                }
                if (bool.TryParse(value, out var d))
                {
                    return d;
                }
            }
            return false;
        }

        /// <summary>
        ///   文本到
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static Guid StringToGuid(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (Guid.TryParse(value, out var g))
                {
                    return g;
                }
            }
            return Guid.Empty;
        }

        #endregion

        #region 文本到对象(可空)

        /// <summary>
        ///   可空值到文本
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static string NullableString<T>(object value) where T : struct
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }
            if (value is T?)
            {
                var nv = value as T?;
                return nv.Value.ToString();
            }
            return value.ToString();
        }

        /// <summary>
        ///   文本到long
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static Int64? StringToNullableInt64(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (Int64.TryParse(value, out var d))
                {
                    return d;
                }
            }
            return null;
        }

        /// <summary>
        ///   文本到int
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static Int32? StringToNullableInt32(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (Int32.TryParse(value, out var d))
                {
                    return d;
                }
            }
            return null;
        }

        /// <summary>
        ///   文本到short
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static Int16? StringToNullableInt16(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (Int16.TryParse(value, out var d))
                {
                    return d;
                }
            }
            return null;
        }

        /// <summary>
        ///   文本到byte
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static byte? StringToNullableByte(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (byte.TryParse(value, out var d))
                {
                    return d;
                }
            }
            return null;
        }

        /// <summary>
        ///   文本到decimal
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static decimal? StringToNullableDecimal(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (decimal.TryParse(value, out var d))
                {
                    return d;
                }
            }
            return null;
        }

        /// <summary>
        ///   文本到double
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static Double? StringToNullableDouble(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (Double.TryParse(value, out var d))
                {
                    return d;
                }
            }
            return null;
        }

        /// <summary>
        ///   文本到float
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static float? StringToNullableFloat(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (float.TryParse(value, out var d))
                {
                    return d;
                }
            }
            return null;
        }

        /// <summary>
        ///   文本到float
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static DateTime? StringToNullableDateTime(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (DateTime.TryParse(value, out var d))
                {
                    return d;
                }
            }
            return null;
        }

        /// <summary>
        ///   文本到char
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static Char? StringToNullableChar(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value[0];
            }
            return null;
        }

        /// <summary>
        ///   文本到布尔
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static bool? StringToNullableBoolean(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (value == "on" || value == "yes")
                {
                    return true;
                }
                if (value == "off" || value == "no")
                {
                    return false;
                }
                if (int.TryParse(value, out var i))
                {
                    return i != 0;
                }
                if (bool.TryParse(value, out var d))
                {
                    return d;
                }
            }
            return null;
        }

        /// <summary>
        ///   文本到
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static Guid? StringToNullableGuid(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (Guid.TryParse(value, out var g))
                {
                    return g;
                }
            }
            return null;
        }

        #endregion

        #region 值转化帮助

        /// <summary>
        ///   文本到string
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static string ValueToString(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }
            var s = value.ToString().Trim();

            return string.IsNullOrEmpty(s)
                           ? null
                           : s;
        }

        /// <summary>
        ///   文本到日期
        /// </summary>
        /// <param name="value"> 文本 </param>
        /// <returns> 日期 </returns>
        public static float ValueToFloat(object value)
        {
            if (value is float f)
            {
                return f;
            }
            if (value == null || value == DBNull.Value)
            {
                return 0F;
            }

            return float.TryParse(value.ToString(), out var t)
                           ? t
                           : 0F;
        }

        /// <summary>
        ///   文本到日期
        /// </summary>
        /// <param name="value"> 文本 </param>
        /// <returns> 日期 </returns>
        public static float? ValueToNullableFloat(object value)
        {
            if (value is float f)
            {
                return f;
            }
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            return float.TryParse(value.ToString(), out var t)
                           ? (float?)t
                           : null;
        }

        /// <summary>
        ///   文本到日期
        /// </summary>
        /// <param name="value"> 文本 </param>
        /// <returns> 日期 </returns>
        public static double ValueToDouble(object value)
        {
            if (value is double)
            {
                return (double)value;
            }
            if (value == null || value == DBNull.Value)
            {
                return default(double);
            }

            return double.TryParse(value.ToString(), out var t)
                           ? t
                           : default(double);
        }

        /// <summary>
        ///   文本到日期
        /// </summary>
        /// <param name="value"> 文本 </param>
        /// <returns> 日期 </returns>
        public static double? ValueToNullableDouble(object value)
        {
            if (value is double)
            {
                return (double)value;
            }
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            return double.TryParse(value.ToString(), out var t)
                           ? (double?)t
                           : null;
        }

        /// <summary>
        ///   文本到日期
        /// </summary>
        /// <param name="value"> 文本 </param>
        /// <returns> 日期 </returns>
        public static DateTime ValueToDate(object value)
        {
            if (value is DateTime)
            {
                return (DateTime)value;
            }
            if (value == null || value == DBNull.Value)
            {
                return default(DateTime);
            }

            return DateTime.TryParse(value.ToString(), out var t)
                           ? t
                           : default(DateTime);
        }

        /// <summary>
        ///   文本到日期
        /// </summary>
        /// <param name="value"> 文本 </param>
        /// <returns> 日期 </returns>
        public static DateTime? ValueToNullableDate(object value)
        {
            if (value is DateTime)
            {
                return (DateTime)value;
            }
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            return DateTime.TryParse(value.ToString(), out var t)
                           ? (DateTime?)t
                           : null;
        }

        /// <summary>
        ///   文本到数字
        /// </summary>
        /// <param name="value"> 文本 </param>
        /// <returns> 数字 </returns>
        public static decimal ValueToDecimal(object value)
        {
            if (value is decimal)
            {
                return (decimal)value;
            }
            if (value == null || value == DBNull.Value)
            {
                return 0M;
            }

            return decimal.TryParse(value.ToString(), out var t)
                           ? t
                           : 0M;
        }

        /// <summary>
        ///   文本到数字
        /// </summary>
        /// <param name="value"> 文本 </param>
        /// <returns> 数字 </returns>
        public static decimal? ValueToNullableDecimal(object value)
        {
            if (value is decimal)
            {
                return (decimal)value;
            }
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            if (decimal.TryParse(value.ToString(), out var t))
            {
                return t;
            }
            return null;
        }

        /// <summary>
        ///   文本到数字
        /// </summary>
        /// <param name="value"> 文本 </param>
        /// <returns> 数字 </returns>
        public static long ValueToLong(object value)
        {
            if (value is long)
            {
                return (long)value;
            }
            if (value == null || value == DBNull.Value)
            {
                return 0L;
            }

            return long.TryParse(value.ToString(), out var t)
                           ? t
                           : 0L;
        }

        /// <summary>
        ///   文本到数字
        /// </summary>
        /// <param name="value"> 文本 </param>
        /// <returns> 数字 </returns>
        public static long? ValueToNullableLong(object value)
        {
            if (value is long)
            {
                return (long)value;
            }
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            return long.TryParse(value.ToString(), out var t)
                           ? (long?)t
                           : null;
        }

        /// <summary>
        ///   文本到数字
        /// </summary>
        /// <param name="value"> 文本 </param>
        /// <returns> 数字 </returns>
        public static int ValueToInt(object value)
        {
            if (value is int)
            {
                return (int)value;
            }
            if (value == null || value == DBNull.Value)
            {
                return 0;
            }

            return int.TryParse(value.ToString(), out var t)
                           ? t
                           : 0;
        }

        /// <summary>
        ///   文本到数字
        /// </summary>
        /// <param name="value"> 文本 </param>
        /// <returns> 数字 </returns>
        public static int? ValueToNullableInt(object value)
        {
            if (value is int)
            {
                return (int)value;
            }
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            if (int.TryParse(value.ToString(), out var t))
            {
                return t;
            }
            return null;
        }

        /// <summary>
        ///   文本到数字
        /// </summary>
        /// <param name="value"> 文本 </param>
        /// <returns> 数字 </returns>
        public static Guid ValueToGuid(object value)
        {
            if (value is Guid)
            {
                return (Guid)value;
            }
            if (value == null || value == DBNull.Value)
            {
                return Guid.Empty;
            }

            return Guid.TryParse(value.ToString(), out var t)
                           ? t
                           : Guid.Empty;
        }

        /// <summary>
        ///   文本到数字
        /// </summary>
        /// <param name="value"> 文本 </param>
        /// <returns> 数字 </returns>
        public static Guid? ValueToNullableGuid(object value)
        {
            if (value is Guid)
            {
                return (Guid)value;
            }
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            return Guid.TryParse(value.ToString(), out var t)
                           ? (Guid?)t
                           : null;
        }

        /// <summary>
        ///   文本到数字
        /// </summary>
        /// <param name="value"> 文本 </param>
        /// <returns> 数字 </returns>
        public static bool ValueToBool(object value)
        {
            if (value is bool)
            {
                return (bool)value;
            }
            if (value == null || value == DBNull.Value)
            {
                return false;
            }

            if (value is string sv)
            {
                sv = sv.ToLower();
                switch (sv)
                {
                    case "yes":
                    case "on":
                        return true;
                    case "no":
                    case "off":
                        return false;
                }
                if (int.TryParse(sv, out var i))
                {
                    return i != 0;
                }
                return bool.TryParse(sv, out var d) && d;
            }

            return bool.TryParse(value.ToString(), out var t) && t;
        }

        /// <summary>
        ///   文本到数字
        /// </summary>
        /// <param name="value"> 文本 </param>
        /// <returns> 数字 </returns>
        public static bool? ValueToNullableBool(object value)
        {
            if (value is bool b)
            {
                return b;
            }
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            if (value is string sv)
            {
                sv = sv.ToLower();
                if (sv == "on" || sv == "yes")
                {
                    return true;
                }
                if (sv == "off" || sv == "no")
                {
                    return false;
                }
                if (int.TryParse(sv, out var i))
                {
                    return i != 0;
                }
                return bool.TryParse(sv, out var d)
                               ? (bool?)d
                               : null;
            }

            return bool.TryParse(value.ToString(), out var t)
                           ? (bool?)t
                           : null;
        }

        #endregion

        #region 文本数组互换

        /// <summary>
        ///   显示集合的所有文本
        /// </summary>
        /// <param name="d"> </param>
        /// <returns> </returns>
        public static string DictionaryString(IDictionary d)
        {
            var sb = new StringBuilder();
            foreach (DictionaryEntry de in d)
            {
                sb.AppendLine(de.Value.ToString());
            }
            return sb.ToString();
        }

        /// <summary>
        ///   以逗号分开的文本串变为数字列表
        /// </summary>
        /// <param name="s"> </param>
        /// <returns> </returns>
        public static List<long> StringToIds(string s)
        {
            var ids = new List<long>();
            var i = s.Split(new[]
            {
                    ',' , ';' , ' ' , '\n'
            });
            foreach (var si in i)
            {
                if (long.TryParse(si.Trim(), out var l))
                {
                    ids.Add(l);
                }
            }
            return ids;
        }

        /// <summary>
        ///   以逗号分开的文本串变为数字列表
        /// </summary>
        /// <param name="s"> </param>
        /// <returns> </returns>
        public static List<int> StringToIntList(string s)
        {
            var ids = new List<int>();
            var i = s.Split(new[]
            {
                    ',' , ';' , ' ' , '\n'
            });
            foreach (var si in i)
            {
                if (int.TryParse(si.Trim(), out var l))
                {
                    ids.Add(l);
                }
            }
            return ids;
        }

        /// <summary>
        ///   将ID变为一个以逗号分开的文本串
        /// </summary>
        /// <param name="ids"> </param>
        /// <returns> </returns>
        public static string StringIds(List<long> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return null;
            }
            var sb = new StringBuilder();
            sb.Append(ids[0]);
            for (int i = 1,
                    cnt = ids.Count; i < cnt; i++)
            {
                sb.AppendFormat(",{0}", ids[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        ///   将ID变为一个以逗号分开的文本串
        /// </summary>
        /// <param name="ids"> </param>
        /// <returns> </returns>
        public static string StringIds(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return null;
            }
            var sb = new StringBuilder();
            sb.Append(ids[0]);
            for (int i = 1,
                    cnt = ids.Count; i < cnt; i++)
            {
                sb.AppendFormat(",{0}", ids[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        ///   将ID变为一个以逗号分开的文本串
        /// </summary>
        /// <param name="ids"> </param>
        /// <returns> </returns>
        public static string StringList(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return null;
            }
            var sb = new StringBuilder();
            sb.Append(ids[0]);
            for (int i = 1,
                    cnt = ids.Count; i < cnt; i++)
            {
                sb.AppendFormat(",{0}", ids[i]);
            }
            return sb.ToString();
        }

        #endregion

        #region 空值检测与相等检测

        /// <summary>
        ///   数据是不是空
        /// </summary>
        /// <param name="a"> </param>
        /// <returns> </returns>
        public static bool IsNull(object a)
        {
            return (a == null || a == DBNull.Value || string.IsNullOrWhiteSpace(a.ToString().Trim()));
        }

        /// <summary>
        ///   数据是不是空
        /// </summary>
        /// <param name="a"> </param>
        /// <returns> </returns>
        public static bool IsNull2(object a)
        {
            return a == null || a == DBNull.Value;
        }

        /// <summary>
        ///   取得值
        /// </summary>
        /// <param name="value"> </param>
        /// <param name="def"> </param>
        /// <returns> </returns>
        public static string GetString(object value, string def)
        {
            if (value == null || value == DBNull.Value)
            {
                return def;
            }
            var s = value as string;
            return s ?? value.ToString();
        }

        /// <summary>
        ///   取得值
        /// </summary>
        /// <param name="value"> </param>
        /// <param name="def"> </param>
        /// <returns> </returns>
        public static T GetValue<T>(object value, T def)
        {
            if (value == null || value == DBNull.Value)
            {
                return def;
            }
            if (value is T)
            {
                return (T)value;
            }
            return def;
        }

        /// <summary>
        ///   取得值
        /// </summary>
        /// <param name="value"> </param>
        /// <param name="convert"> </param>
        /// <param name="def"> </param>
        /// <returns> </returns>
        public static T GetValue<T>(object value, Func<object, T> convert, T def)
        {
            if (value == null || value == DBNull.Value)
            {
                return def;
            }
            if (value is T)
            {
                return (T)value;
            }
            try
            {
                return convert(value);
            }
            catch
            {
                return def;
            }
        }

        /// <summary>
        ///   数据是不是相等
        /// </summary>
        /// <param name="a"> </param>
        /// <param name="b"> </param>
        /// <param name="aNull"> </param>
        /// <param name="bNull"> </param>
        /// <returns> </returns>
        public static bool IsEquals(object a, object b, out bool aNull, out bool bNull)
        {
            aNull = IsNull(a);
            bNull = IsNull(b);
            if (aNull && bNull)
            {
                return true;
            }
            if (aNull != bNull)
            {
                return false;
            }
            var aT = a.GetType();
            var bT = b.GetType();
            return aT == bT ? b.Equals(a) : b.ToString().Trim().Equals(a.ToString().Trim(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///   数据是不是相等
        /// </summary>
        /// <param name="a"> </param>
        /// <param name="b"> </param>
        /// <returns> </returns>
        public static bool IsEquals(object a, object b)
        {
            return IsEquals(a, b, out var aNull, out var bNull);
        }

        #endregion

        #region 编辑值转化

        /// <summary>
        ///   编辑文本转为日期
        /// </summary>
        /// <param name="str"> </param>
        /// <param name="rv"> </param>
        /// <returns> </returns>
        public static bool EditStringToDate(string str, out object rv)
        {
            var re = true;
            if (str.Length == 1 && str[0] == 't' || str[0] == 'T' || str[0] == 'n' || str[0] == 'N')
            {
                rv = DateTime.Today;
                return true;
            }

            if (DateTime.TryParse(str, out var day))
            {
                rv = day;
                return true;
            }
            var ss = str.Split(new[]
            {
                    '年' , '月' , '日' , ':' , '_' , '-' , ' '
            });
            var v = string.Join("", ss);
            if (int.TryParse(v, out var d))
            {
                try
                {
                    day = DateTime.Today;
                    if (d < 100)
                    {
                        rv = new DateTime(day.Year, day.Month, d);
                    }
                    else if (d < 10000)
                    {
                        rv = new DateTime(day.Year, (d / 100) % 100, d % 100);
                    }
                    else if (d < 1000000)
                    {
                        rv = new DateTime(2000 + (d / 10000) % 100, (d / 100) % 100, d % 100);
                    }
                    else
                    {
                        rv = new DateTime(d / 10000, (d / 100) % 100, d % 100);
                    }
                }
                catch
                {
                    rv = false;
                    return false;
                }
            }
            else
            {
                re = false;
                rv = null;
            }
            return re;
        }

        /// <summary>
        ///   编辑文本转为日期
        /// </summary>
        /// <param name="str"> </param>
        /// <param name="rv"> </param>
        /// <returns> </returns>
        public static bool EditStringToDateTime(string str, out object rv)
        {
            if (str.Length == 1)
            {
                if (str[0] == 't' || str[0] == 'T')
                {
                    rv = DateTime.Today;
                    return true;
                }
                if (str[0] == 'n' || str[0] == 'N')
                {
                    rv = DateTime.Now;
                    return true;
                }
            }

            if (DateTime.TryParse(str, out var n))
            {
                rv = n;
                return true;
            }
            var ss = str.Split(new[]
            {
                    '年' , '月' , '日' , ':' , '_' , '-' , ' '
            });
            var v = string.Join("", ss);
            if (long.TryParse(v, out var d))
            {
                n = DateTime.Today;
                if (d < 100)
                {
                    rv = new DateTime(n.Year, n.Month, n.Day, (int)d, 0, 0);
                }
                else if (d < 10000)
                {
                    rv = new DateTime(n.Year, n.Month, n.Day, (int)(d / 100) % 100, (int)d % 100, 0);
                }
                else if (d < 1000000)
                {
                    rv = new DateTime(n.Year, n.Month, n.Day, (int)(d / 10000) % 100, (int)(d / 100) % 100, (int)d % 100);
                }
                else if (d < 100000000)
                {
                    rv = new DateTime(n.Year, n.Month, (int)(d / 1000000) % 100, (int)(d / 10000) % 100, (int)(d / 100) % 100, (int)d % 100);
                }
                else if (d < 10000000000)
                {
                    rv = new DateTime(n.Year, (int)(d / 100000000) % 100, (int)(d / 1000000) % 100, (int)(d / 10000) % 100, (int)(d / 100) % 100, (int)d % 100);
                }
                else if (d < 1000000000000)
                {
                    rv = new DateTime(2000 + (int)(d / 10000000000) % 100, (int)(d / 100000000) % 100, (int)(d / 1000000) % 100, (int)(d / 10000) % 100, (int)(d / 100) % 100, (int)d % 100);
                }
                else
                {
                    rv = new DateTime((int)(d / 10000000000), (int)(d / 100000000) % 100, (int)(d / 1000000) % 100, (int)(d / 10000) % 100, (int)(d / 100) % 100, (int)d % 100);
                }
                return true;
            }
            rv = null;
            return false;
        }

        /// <summary>
        ///   编辑文本转为日期
        /// </summary>
        /// <param name="str"> </param>
        /// <param name="rv"> </param>
        /// <returns> </returns>
        public static bool EditStringToTime(string str, out object rv)
        {
            if (str.Length == 1)
            {
                if (str[0] == 't' || str[0] == 'T' || str[0] == 'j' || str[0] == 'J')
                {
                    rv = DateTime.Today;
                    return true;
                }
                if (str[0] == 'n' || str[0] == 'N' || str[0] == 'x' || str[0] == 'X')
                {
                    rv = DateTime.Now;
                    return true;
                }
            }

            if (DateTime.TryParse(str, out var n))
            {
                rv = n;
                return true;
            }
            var ss = str.Split(new[]
            {
                    '年' , '月' , '日' , ':' , '_' , '-' , ' '
            });
            var v = string.Join("", ss);
            if (int.TryParse(v, out var d))
            {
                if (d < 100)
                {
                    rv = new DateTime(9999, 1, 1, d, 0, 0);
                }
                else if (d < 10000)
                {
                    rv = new DateTime(9999, 1, 1, (d / 100) % 100, d % 100, 0);
                }
                else
                {
                    rv = new DateTime(9999, 1, 1, (d / 10000) % 100, (d / 100) % 100, d % 100);
                }
                return true;
            }
            rv = null;
            return false;
        }

        #endregion

        #region 位检测
        /// <summary>
        ///   检测一个数是否有多个位
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static bool IsFlag(long value)
        {
            var bitIsSet = false;
            while (value > 0)
            {
                if ((value & 1) == 1)
                {
                    if (bitIsSet)
                    {
                        return true;
                    }
                    bitIsSet = true;
                }
                value >>= 1;
            }
            return false;
        }
        #endregion

        #region 本机代码编译
        /// <summary>
        /// 调用ngen.exe并执行文件的本机代码编译
        /// </summary>
        /// <param name="file"></param>
        [Conditional("NGEN")]
        public static void CallNgen(string file)
        {
            var ngen = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"Microsoft.NET\Framework\v4.0.30319", "ngen.exe");
            var process = Process.Start(ngen, $"uninstall {file} /AppBase:{AppDomain.CurrentDomain.BaseDirectory}");
            // ReSharper disable PossibleNullReferenceException
            process.WaitForExit();
            // ReSharper restore PossibleNullReferenceException
            process = Process.Start(ngen, $"install {file} /AppBase:{AppDomain.CurrentDomain.BaseDirectory}");
            // ReSharper disable PossibleNullReferenceException
            process.WaitForExit();
            // ReSharper restore PossibleNullReferenceException
        }
        /// <summary>
        /// 调用ngen.exe安装文件的本机代码编译版本
        /// </summary>
        /// <param name="file"></param>
        [Conditional("NGEN")]
        public static void Install(string file)
        {
            var ngen = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"Microsoft.NET\Framework\v4.0.30319", "ngen.exe");
            var process = Process.Start(ngen, $"install {file} /AppBase:{AppDomain.CurrentDomain.BaseDirectory}");
            // ReSharper disable PossibleNullReferenceException
            process.WaitForExit();
            // ReSharper restore PossibleNullReferenceException
        }
        /// <summary>
        /// 调用ngen.exe反安装文件的本机代码编译版本
        /// </summary>
        /// <param name="file"></param>
        [Conditional("NGEN")]
        public static void Uninstall(string file)
        {
            var ngen = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"Microsoft.NET\Framework\v4.0.30319", "ngen.exe");
            var process = Process.Start(ngen, $"uninstall {file} /AppBase:{AppDomain.CurrentDomain.BaseDirectory}");
            // ReSharper disable PossibleNullReferenceException
            process.WaitForExit();
            // ReSharper restore PossibleNullReferenceException
        }
        #endregion

        #region 强类型转换器

        /// <summary>
        ///     得到强类型转换器
        /// </summary>
        /// <returns> </returns>
        public static Func<object, T> GetConvert<T>()
        {
            var type = typeof(T);

            if (type == typeof(char))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    try
                    {
                        dynamic re = Convert.ToChar(t);
                        return (T)re;
                    }
                    catch
                    {
                        return default(T);
                    }
                };
            }
            if (type == typeof(Byte))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    try
                    {
                        dynamic re = Convert.ToByte(t);
                        return (T)re;
                    }
                    catch
                    {
                        return default(T);
                    }
                };
            }
            if (type == typeof(SByte))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    try
                    {
                        dynamic re = Convert.ToSByte(t);
                        return (T)re;
                    }
                    catch
                    {
                        return default(T);
                    }
                };
            }
            if (type == typeof(Int16))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    try
                    {
                        dynamic re = Convert.ToInt16(t);
                        return (T)re;
                    }
                    catch
                    {
                        return default(T);
                    }
                };
            }
            if (type == typeof(Int32))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    try
                    {
                        dynamic re = Convert.ToInt32(t);
                        return (T)re;
                    }
                    catch
                    {
                        return default(T);
                    }
                };
            }

            if (type == typeof(Int64))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    try
                    {
                        dynamic re = Convert.ToInt64(t);
                        return (T)re;
                    }
                    catch
                    {
                        return default(T);
                    }
                };
            }
            if (type == typeof(decimal))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    try
                    {
                        dynamic re = Convert.ToDecimal(t);
                        return (T)re;
                    }
                    catch
                    {
                        return default(T);
                    }
                };
            }

            if (type == typeof(UInt16))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    try
                    {
                        dynamic re = Convert.ToUInt16(t);
                        return (T)re;
                    }
                    catch
                    {
                        return default(T);
                    }
                };
            }
            if (type == typeof(UInt32))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    try
                    {
                        dynamic re = Convert.ToUInt32(t);
                        return (T)re;
                    }
                    catch
                    {
                        return default(T);
                    }
                };
            }

            if (type == typeof(UInt64))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    try
                    {
                        dynamic re = Convert.ToUInt64(t);
                        return (T)re;
                    }
                    catch
                    {
                        return default(T);
                    }
                };
            }
            if (type == typeof(decimal))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    try
                    {
                        dynamic re = Convert.ToDecimal(t);
                        return (T)re;
                    }
                    catch
                    {
                        return default(T);
                    }
                };
            }
            if (type == typeof(DateTime))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    try
                    {
                        dynamic re = Convert.ToDateTime(t);
                        return (T)re;
                    }
                    catch
                    {
                        return default(T);
                    }
                };
            }
            if (type == typeof(Double))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    try
                    {
                        dynamic re = Convert.ToDouble(t);
                        return (T)re;
                    }
                    catch
                    {
                        return default(T);
                    }
                };
            }
            if (type == typeof(float))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    try
                    {
                        dynamic re = Convert.ToSingle(t);
                        return (T)re;
                    }
                    catch
                    {
                        return default(T);
                    }
                };
            }
            if (type == typeof(string))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    try
                    {
                        dynamic re = t.ToString();
                        return (T)re;
                    }
                    catch
                    {
                        return default(T);
                    }
                };
            }
            if (type == typeof(Guid))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    try
                    {
                        dynamic re = Guid.TryParse(t.ToString(), out var guid) ? guid : Guid.Empty;
                        return (T)re;
                    }
                    catch
                    {
                        return default(T);
                    }
                };
            }
            return t => (T)t;
        }

        /// <summary>
        ///     得到强类型转换器
        /// </summary>
        /// <returns> </returns>
        public static Func<string, T> GetStringConvert<T>()
        {
            var type = typeof(T);

            if (type == typeof(char))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    dynamic re = char.Parse(t);
                    return (T)re;
                };
            }
            if (type == typeof(Byte))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    dynamic re = Byte.Parse(t);
                    return (T)re;
                };
            }
            if (type == typeof(SByte))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    dynamic re = SByte.Parse(t);
                    return (T)re;
                };
            }
            if (type == typeof(Int16))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    dynamic re = Int16.Parse(t);
                    return (T)re;
                };
            }
            if (type == typeof(Int32))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    dynamic re = Int32.Parse(t);
                    return (T)re;
                };
            }

            if (type == typeof(Int64))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    dynamic re = Int64.Parse(t);
                    return (T)re;
                };
            }
            if (type == typeof(decimal))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    dynamic re = decimal.Parse(t);
                    return (T)re;
                };
            }

            if (type == typeof(UInt16))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    dynamic re = UInt16.Parse(t);
                    return (T)re;
                };
            }
            if (type == typeof(UInt32))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    dynamic re = UInt32.Parse(t);
                    return (T)re;
                };
            }

            if (type == typeof(UInt64))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    dynamic re = UInt64.Parse(t);
                    return (T)re;
                };
            }
            if (type == typeof(decimal))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    dynamic re = decimal.Parse(t);
                    return (T)re;
                };
            }
            if (type == typeof(DateTime))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    dynamic re = DateTime.Parse(t);
                    return (T)re;
                };
            }
            if (type == typeof(Double))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    dynamic re = Double.Parse(t);
                    return (T)re;
                };
            }
            if (type == typeof(float))
            {
                return t =>
                {
                    if (t == null)
                    {
                        return default(T);
                    }
                    dynamic re = float.Parse(t);
                    return (T)re;
                };
            }
            if (type == typeof(string))
            {
                return t =>
                {
                    dynamic re = t;
                    return (T)re;
                };
            }
            return null;
        }

        #endregion
    }
}
