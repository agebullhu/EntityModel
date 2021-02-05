#region 引用

using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using ZeroTeam.MessageMVC.Context;

#endregion

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    ///     参数解析器
    /// </summary>
    public static class RequestArgumentConvert
    {
        #region 消息对象

        /// <summary>
        ///     参数
        /// </summary>
        static Dictionary<string, string> Arguments => GlobalContext.Current.Message.Dictionary;

        #endregion

        #region 参数设置

        /// <summary>
        ///     当前请求是否包含这个参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>是否包含这个参数</returns>
        public static bool ContainsArgument(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && Arguments.ContainsKey(name);
        }

        /// <summary>
        ///     设置替代参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public static void SetArgument(string name, string value)
        {
            Arguments[name] = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        /// <summary>
        ///     设置替代参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public static void SetArgument<T>(string name, T value)
        {
            Arguments[name] = value.ToString();
        }

        /// <summary>
        ///     设置替代参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public static void SetArgument(string name, object value)
        {
            Arguments[name] = value?.ToString();
        }

        #endregion

        #region Get

        /// <summary>
        ///     获取参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        [Obsolete("请改为GetString方法")]
        public static string GetArgValue(string name)
        {
            return GetString(name);
        }

        /// <summary>
        ///     获取参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        [Obsolete("请改为GetString方法")]
        public static string Get(string name)
        {
            return GetString(name);
        }

        /// <summary>
        ///     获取参数(文本)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>文本</returns>
        public static string GetString(string name)
        {
            Arguments.TryGetValue(name, out var value);
            return value;
        }

        /// <summary>
        ///     读参数(泛型),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="convert">转换方法</param>
        /// <param name="def">默认值</param>
        /// <returns>值</returns>
        public static T? GetNullable<T>(string name, Func<string, T> convert, T? def = null)
            where T : struct
        {
            if (!Arguments.TryGetValue(name, out var value))
                return def;
            if (string.IsNullOrWhiteSpace(value))
                return def;
            return convert(value);
        }

        /// <summary>
        ///     读参数(泛型),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="convert">转换方法</param>
        /// <param name="def">默认值</param>
        /// <returns>值</returns>
        public static T GetArg<T>(string name, Func<string, T> convert, T def)
        {
            if (!Arguments.TryGetValue(name, out var value) || string.IsNullOrEmpty(value))
                return def;
            return convert(value.Trim());
        }

        /// <summary>
        ///     读参数(泛型)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>参数为空或不存在,返回不成功,其它情况视convert返回值自行控制</returns>
        public static bool GetBool(string name)
        {
            return GetArg(name, bool.Parse, false);
        }

        /// <summary>
        ///     获取参数int类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为0,如果存在且不能转为int类型将出现异常</returns>
        public static int GetInt(string name)
        {
            return GetArg(name, int.Parse, 0);
        }

        /// <summary>
        ///     获取参数int类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为0,如果存在且不能转为int类型将出现异常</returns>
        public static double GetDouble(string name)
        {
            return GetArg(name, double.Parse, 0.0);
        }

        /// <summary>
        ///     获取参数int类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为0,如果存在且不能转为int类型将出现异常</returns>
        public static float GetSingle(string name)
        {
            return GetArg(name, float.Parse, 0.0F);
        }

        /// <summary>
        ///     获取参数int类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为0,如果存在且不能转为int类型将出现异常</returns>
        public static Guid GetGuid(string name)
        {
            return GetArg(name, Guid.Parse, Guid.Empty);
        }
        /// <summary>
        ///     获取参数int类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为0,如果存在且不能转为int类型将出现异常</returns>
        public static byte GetByte(string name)
        {
            return GetArg(name, byte.Parse, (byte)0);
        }

        /// <summary>
        ///     获取参数int类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为0,如果存在且不能转为int类型将出现异常</returns>
        public static int[] GetIntArray(string name)
        {
            if (!TryGetValue(name, out var value))
                return new int[0];

            return string.IsNullOrEmpty(value)
                ? (new int[0])
                : value.Split(new[] { ',', '\'', '\"' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }

        /// <summary>
        ///     获取参数(int类型),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="def">默认值</param>
        /// <returns>int类型,如果存在且不能转为int类型将出现异常</returns>
        public static int GetInt(string name, int def)
        {
            return GetArg(name, int.Parse, def);
        }

        /// <summary>
        ///     获取参数(数字),模糊名称读取
        /// </summary>
        /// <param name="names">多个名称</param>
        /// <returns>名称解析到的第一个不为0的数字,如果有名称存在且不能转为int类型将出现异常</returns>
        public static int GetIntAny(params string[] names)
        {
            foreach (var name in names)
            {
                if (TryGet(name, out int val) && val != 0)
                    return val;
            }
            throw new ArgumentException("无法解析参数");
        }

        /// <summary>
        ///     获取参数(日期类型)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>日期类型,为空则为空,如果存在且不能转为日期类型将出现异常</returns>
        public static DateTime? GetNullableDate(string name)
        {
            return GetNullable(name, DateTime.Parse);
        }

        /// <summary>
        ///     获取参数(日期类型)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>日期类型,为空则为DateTime.MinValue,如果存在且不能转为日期类型将出现异常</returns>
        public static DateTime GetDate(string name)
        {
            return GetArg(name, DateTime.Parse, DateTime.MinValue);
        }

        /// <summary>
        ///     获取参数(日期类型)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="def"></param>
        /// <returns>日期类型,为空则为空,如果存在且不能转为日期类型将出现异常</returns>
        public static DateTime GetDate(string name, DateTime def)
        {
            return GetArg(name, DateTime.Parse, def);
        }


        /// <summary>
        ///     获取参数bool类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="def">默认值</param>
        /// <returns>int类型,为空则为0,如果存在且不能转为int类型将出现异常</returns>
        public static bool GetBool(string name, bool def)
        {
            return GetArg(name, bool.Parse, def);
        }


        /// <summary>
        ///     获取参数(decimal型数据)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>decimal型数据,如果未读取值则为-1,如果存在且不能转为decimal类型将出现异常</returns>
        public static decimal GetDecimal(string name)
        {
            return GetArg(name, decimal.Parse, 0M);
        }

        /// <summary>
        ///     获取参数(decimal型数据),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="def">默认值</param>
        /// <returns>decimal型数据,如果存在且不能转为decimal类型将出现异常</returns>
        public static decimal GetDecimal(string name, decimal def)
        {
            return GetArg(name, decimal.Parse, def);
        }

        /// <summary>
        ///     获取参数(long型数据),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="def">默认值</param>
        /// <returns>long型数据,如果存在且不能转为long类型将出现异常</returns>
        public static long GetLong(string name, long def = 0)
        {
            return GetArg(name, long.Parse, def);
        }

        /// <summary>
        ///     获取参数int类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为0,如果存在且不能转为int类型将出现异常</returns>
        public static long[] GetLongArray(string name)
        {
            if (!TryGetValue(name, out var value))
                return null;

            return string.IsNullOrEmpty(value)
                ? (new long[0])
                : value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        }

        /// <summary>
        ///     获取参数(数字),模糊名称读取
        /// </summary>
        /// <param name="names">多个名称</param>
        /// <returns>名称解析到的第一个不为0的数字,如果有名称存在且不能转为int类型将出现异常</returns>
        public static long GetLongAny(params string[] names)
        {
            foreach (var name in names)
            {
                if (TryGet(name, out long val) && val != 0)
                    return val;
            }
            throw new ArgumentException("无法解析参数");
        }
        #endregion

        #region TryGet

        /// <summary>
        ///     获取参数(文本)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="val"></param>
        /// <returns>文本</returns>
        static bool TryGetValue(string name, out string val)
        {
            if (!Arguments.TryGetValue(name, out var value) || string.IsNullOrWhiteSpace(value))
            {
                val = null;
                return false;
            }
            val = value.Trim();
            return true;
        }


        /// <summary>
        ///     获取参数(文本)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="val">结果值</param>
        /// <param name="trim">是否清除首尾空白</param>
        /// <returns>文本</returns>
        public static bool TryGet(string name, out string val, bool trim = true)
        {
            if (!Arguments.TryGetValue(name, out var value))
            {
                val = null;
                return false;
            }
            if (trim && value != null)
                val = value.Trim();
            else
                val = value;
            return true;
        }


        /// <summary>
        ///     读参数(泛型),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="convert">转换方法</param>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGet<T>(string name, Func<string, T> convert, out T? value)
            where T : struct
        {
            if (!TryGet(name, out string str))
            {
                value = null;
                return false;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    value = null;
                    return true;
                }
                value = convert(str);
                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }

        /// <summary>
        ///     读参数(泛型),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="convert">转换方法</param>
        /// <param name="value">参数值</param>
        /// <param name="hase">参数是否存在</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        internal static bool TryGet<T>(string name, Func<string, T> convert, out T value, out bool hase)
        {
            hase = Arguments.TryGetValue(name, out var str);
            if (!hase)
            {
                value = default;
                return false;
            }
            if (string.IsNullOrWhiteSpace(str))
            {
                value = default;
                return false;
            }
            try
            {
                value = convert(str.Trim());
                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }

        /// <summary>
        ///     读参数(泛型),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="convert">转换方法</param>
        /// <param name="value">参数值</param>
        /// <param name="hase">参数是否存在</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        internal static bool TryGet<T>(string name, Func<string, (bool state, T value)> convert, out T value, out bool hase)
        {
            hase = Arguments.TryGetValue(name, out var str);
            if (!hase)
            {
                value = default;
                return false;
            }
            if (string.IsNullOrWhiteSpace(str))
            {
                value = default;
                return false;
            }
            try
            {
                var re = convert(str.Trim());
                value = re.value;
                return re.state;
            }
            catch
            {
                value = default;
                return false;
            }
        }

        /// <summary>
        ///     读参数(泛型),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="convert">转换方法</param>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGet<T>(string name, Func<string, T> convert, out T value)
        {
            if (!TryGetValue(name, out var str))
            {
                value = default;
                return false;
            }

            try
            {
                value = convert(str);
                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }

        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGet(string name, out bool? value)
        {
            if (!TryGet(name, out string str))
            {
                value = false;
                return false;
            }
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    value = null;
                    return true;
                }
                switch (str.ToLower())
                {
                    case "1":
                    case "yes":
                        value = true;
                        return true;
                    case "0":
                    case "no":
                        value = false;
                        return true;
                    default:
                        if (bool.TryParse(str, out bool bl))
                        {
                            value = bl;
                            return true;
                        }
                        value = false;
                        return false;
                }
            }
            catch
            {
                value = false;
                return false;
            }
        }


        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGet(string name, out bool value)
        {
            if (!TryGetValue(name, out var str))
            {
                value = false;
                return false;
            }
            try
            {
                switch (str.ToLower())
                {
                    case "1":
                    case "yes":
                        value = true;
                        return true;
                    case "0":
                    case "no":
                        value = false;
                        return true;
                    default:
                        return bool.TryParse(str, out value);
                }
            }
            catch
            {
                value = false;
                return false;
            }
        }

        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGet(string name, out Guid value)
        {
            if (!TryGetValue(name, out var str))
            {
                value = Guid.Empty;
                return false;
            }
            try
            {
                return Guid.TryParse(str, out value);
            }
            catch
            {
                value = Guid.Empty;
                return false;
            }
        }

        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGet(string name, out DateTime value)
        {
            if (!TryGetValue(name, out var str))
            {
                value = DateTime.MinValue;
                return false;
            }
            try
            {
                return DateTime.TryParse(str, out value);
            }
            catch
            {
                value = DateTime.MinValue;
                return false;
            }
        }

        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGetEnum<TEnum>(string name, out TEnum value)
            where TEnum : struct
        {
            if (!TryGetValue(name, out var str))
            {
                value = default;
                return false;
            }
            try
            {
                return Enum.TryParse(str, true, out value);
            }
            catch
            {
                value = default;
                return false;
            }
        }

        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGet(string name, out int value)
        {
            if (!TryGetValue(name, out var str))
            {
                value = 0;
                return false;
            }
            try
            {
                return int.TryParse(str, out value);
            }
            catch
            {
                value = 0;
                return false;
            }
        }

        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGet(string name, out decimal value)
        {
            if (!TryGetValue(name, out var str))
            {
                value = 0;
                return false;
            }
            try
            {
                return decimal.TryParse(str, out value);
            }
            catch
            {
                value = 0;
                return false;
            }
        }

        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGet(string name, out float value)
        {
            if (!TryGetValue(name, out var str))
            {
                value = float.NaN;
                return false;
            }
            try
            {
                return float.TryParse(str, out value) && !float.IsNaN(value);
            }
            catch
            {
                value = float.NaN;
                return false;
            }
        }

        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGet(string name, out double value)
        {
            if (!TryGetValue(name, out var str))
            {
                value = double.NaN;
                return false;
            }
            try
            {
                return double.TryParse(str, out value) && !double.IsNaN(value);
            }
            catch
            {
                value = double.NaN;
                return false;
            }
        }
        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGet(string name, out short value)
        {
            if (!TryGetValue(name, out var str))
            {
                value = 0;
                return false;
            }
            try
            {
                return short.TryParse(str, out value);
            }
            catch
            {
                value = 0;
                return false;
            }
        }
        /// <summary>
        ///     读主键参数
        /// </summary>
        /// <param name="jsonName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGetId(string jsonName, out long value)
        {
            return TryGet("id", out value) || TryGet(jsonName, out value);
        }

        /// <summary>
        ///     读主键参数
        /// </summary>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGetId(out long value)
        {
            return TryGet("id", out value);
        }

        /// <summary>
        ///     读主键参数
        /// </summary>
        /// <param name="value">参数值</param>
        /// <param name="convert">转换器</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGetId<TData, TPrimaryKey>(Func<string, (bool state, TPrimaryKey key)> convert, out TPrimaryKey value)
           where TData : class, new()
        {
            if (TryGetValue("id", out var str))
            {
                var (state, key) = convert(str);
                value = key;
                return state;
            }
            value = default;
            return false;
        }

        /// <summary>
        ///     读主键参数
        /// </summary>
        /// <param name="jsonName">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="convert">转换器</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGetId<TData, TPrimaryKey>(string jsonName, Func<string, (bool state, TPrimaryKey key)> convert, out TPrimaryKey value)
           where TData : class, new()
        {
            if (TryGetValue("id", out var str))
            {
                var (state, key) = convert(str);
                value = key;
                return state;
            }
            if (TryGetValue(jsonName, out str))
            {
                var (state, key) = convert(str);
                value = key;
                return state;
            }
            value = default;
            return false;
        }


        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGet(string name, out long value)
        {
            if (!TryGetValue(name, out var str))
            {
                value = 0;
                return false;
            }
            try
            {
                return long.TryParse(str, out value);
            }
            catch
            {
                value = 0;
                return false;
            }
        }

        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGet(string name, out string[] value)
        {
            if (!TryGetValue(name, out var str))
            {
                value = new string[0];
                return false;
            }
            try
            {
                value = str.Split(new[] { ',' });
                return value.Length > 0;
            }
            catch
            {
                value = new string[0];
                return false;
            }
        }

        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="values">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGet(string name, out int[] values)
        {
            if (!TryGetValue(name, out var str))
            {
                values = null;
                return false;
            }
            try
            {
                var array = str.Split(new[] { ',', '[', ']', '\"', '\'' }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length == 0)
                {
                    values = null;
                    return false;
                }
                values = array.Select(int.Parse).ToArray();
                return values.Length > 0;
            }
            catch
            {
                values = null;
                return false;
            }
        }
        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="values">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGet(string name, out List<int> values)
        {
            if (!TryGetValue(name, out var str))
            {
                values = null;
                return false;
            }
            try
            {
                var array = str.Split(new[] { ',', '[', ']', '\"', '\'' }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length == 0)
                {
                    values = null;
                    return false;
                }
                values = array.Select(int.Parse).ToList();
                return values.Count > 0;
            }
            catch
            {
                values = null;
                return false;
            }
        }

        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="values">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGet(string name, out long[] values)
        {
            if (!TryGetValue(name, out var str))
            {
                values = null;
                return false;
            }
            try
            {
                var array = str.Split(new[] { ',', '[', ']', '\"', '\'' }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length == 0)
                {
                    values = null;
                    return false;
                }
                values = array.Select(long.Parse).ToArray();
                return values.Length > 0;
            }
            catch
            {
                values = null;
                return false;
            }
        }
        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="values">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGet(string name, out List<long> values)
        {
            if (!TryGetValue(name, out var str))
            {
                values = null;
                return false;
            }
            try
            {
                var array = str.Split(new[] { ',', '[', ']', '\"', '\'' }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length == 0)
                {
                    values = null;
                    return false;
                }
                values = array.Select(long.Parse).ToList();
                return values.Count > 0;
            }
            catch
            {
                values = null;
                return false;
            }
        }
        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="values">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGetIDs(string name, out List<long> values)
        {
            if (!TryGetValue(name, out var str))
            {
                values = null;
                return false;
            }
            try
            {
                var array = str.Split(new[] { ',', '[', ']', '\"', '\'' }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length == 0)
                {
                    values = null;
                    return false;
                }
                values = array.Select(long.Parse).ToList();
                return values.Count > 0;
            }
            catch
            {
                values = null;
                return false;
            }
        }
        /// <summary>
        ///     试图读参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="convert"></param>
        /// <param name="values">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public static bool TryGetIDs<TPrimaryKey>(string name, Func<string, (bool state, TPrimaryKey key)> convert, out List<TPrimaryKey> values)
        {
            values = new List<TPrimaryKey>();
            if (!TryGetValue(name, out var str))
            {
                return false;
            }
            try
            {
                var array = str.Split(new[] { ',', '[', ']', '\"', '\'' }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length == 0)
                {
                    return false;
                }
                foreach (var key in array)
                {
                    var re = convert(key);
                    if (re.state)
                        values.Add(re.key);
                }
                return values.Count > 0;
            }
            catch
            {
                values = null;
                return false;
            }
        }
        #endregion
    }
}
