// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeroTeam.MessageMVC.Context;

#endregion

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    ///     FORM对象转化辅助类
    /// </summary>
    public class FormConvert
    {
        #region 基本属性

        /// <summary>
        ///     是否更新状态
        /// </summary>
        public bool IsUpdata { get; set; }

        /// <summary>
        ///     是否发生解析错误
        /// </summary>
        public bool Failed { get; set; }

        /// <summary>
        ///     字段
        /// </summary>
        private readonly Dictionary<string, string> _messages = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 设置错误字段
        /// </summary>
        /// <param name="field"></param>
        /// <param name="msg"></param>
        private void AddMessage(string field, string msg)
        {
            if (_messages.TryGetValue(field, out var val))
                _messages[field] = $"{val};{msg}";
            else
                _messages.Add(field, msg);
        }


        /// <summary>
        ///     是否发生解析错误
        /// </summary>
        public string Message
        {
            get
            {
                StringBuilder msg = new StringBuilder();
                foreach (var kv in _messages)
                {
                    msg.AppendLine($"{kv.Key} : {kv.Value}");
                }
                return msg.ToString();
            }
        }

        #endregion

        #region 基本操作


        /// <summary>
        ///     参数
        /// </summary>
        internal readonly Dictionary<string, string> Arguments = GlobalContext.Current.Message.Dictionary;

        /// <summary>
        ///     读参数(泛型),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="convert">转换方法</param>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        bool TryGet<T>(string name, Func<string, T> convert, out T value)
        {
            var hase = Arguments.TryGetValue(name, out var str);
            if (!hase)
            {
                value = default;
                return false;
            }
            if (string.IsNullOrWhiteSpace(str))
            {
                value = default;
                return true;
            }
            try
            {
                value = convert(str.Trim());
                return true;
            }
            catch
            {
                AddMessage(name, $"转换为{typeof(T).Name}出错");
                Failed = true;
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
        bool TryGet<T>(string name, Func<string, T> convert, out T? value)
            where T : struct
        {
            var hase = Arguments.TryGetValue(name, out var str);
            if (!hase)
            {
                value = null;
                return false;
            }
            if (string.IsNullOrWhiteSpace(str))
            {
                value = null;
                return true;
            }
            try
            {
                value = convert(str.Trim());
                return true;
            }
            catch
            {
                AddMessage(name, $"转换为{typeof(T).Name}出错");
                Failed = true;
                value = default;
                return false;
            }
        }

        #endregion

        #region 字段值转换 

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out string value)
        {
            if (!Arguments.TryGetValue(field, out var str))
            {
                value = default;
                return false;
            }
            value = str;
            return true;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetEnum<TEnum>(string field, out TEnum value)
            where TEnum : struct
        {
            return TryGet(field, str => Enum.Parse<TEnum>(str, true), out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetEnum<TEnum>(string field, out TEnum? value)
            where TEnum : struct
        {
            return TryGet(field, str => Enum.Parse<TEnum>(str, true), out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out byte value)
        {
            return TryGet(field, byte.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out byte? value)
        {
            return TryGet(field, byte.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out sbyte value)
        {
            return TryGet(field, sbyte.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out sbyte? value)
        {
            return TryGet(field, sbyte.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out short value)
        {
            return TryGet(field, short.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out short? value)
        {
            return TryGet(field, short.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out ushort value)
        {
            return TryGet(field, ushort.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out ushort? value)
        {
            return TryGet(field, ushort.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out bool value)
        {
            return TryGet(field, str =>
            {
                switch (str.ToLower())
                {
                    default:
                        return bool.Parse(str);
                    case "off":
                    case "no":
                    case "0":
                        return false;
                    case "on":
                    case "yes":
                    case "1":
                        return true;
                }
            }, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out bool? value)
        {
            return TryGet(field, str =>
            {
                switch (str.ToLower())
                {
                    default:
                        return bool.Parse(str);
                    case "off":
                    case "no":
                    case "0":
                        return false;
                    case "on":
                    case "yes":
                    case "1":
                        return true;
                }
            }, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out int value)
        {
            return TryGet(field, int.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out int? value)
        {
            return TryGet(field, int.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out uint value)
        {
            return TryGet(field, uint.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out uint? value)
        {
            return TryGet(field, uint.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out long value)
        {
            return TryGet(field, long.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out long? value)
        {
            return TryGet(field, long.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out ulong value)
        {
            return TryGet(field, ulong.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out ulong? value)
        {
            return TryGet(field, ulong.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out float value)
        {
            return TryGet(field, float.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out float? value)
        {
            return TryGet(field, float.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out double value)
        {
            return TryGet(field, double.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out double? value)
        {
            return TryGet(field, double.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out DateTime value)
        {
            return TryGet(field, DateTime.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out DateTime? value)
        {
            return TryGet(field, DateTime.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out decimal value)
        {
            return TryGet(field, decimal.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out decimal? value)
        {
            return TryGet(field, decimal.Parse, out value);
        }
        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out Guid value)
        {
            return TryGet(field, Guid.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out Guid? value)
        {
            return TryGet(field, Guid.Parse, out value);
        }


        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out List<int> value)
        {
            if (!Arguments.TryGetValue(field, out var str))
            {
                value = default;
                return false;
            }
            value = new List<int>();
            if (string.IsNullOrWhiteSpace(str))
            {
                return true;
            }
            var words = str.Split(new char[] { '[', ']', '\'', '\"', ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                if (int.TryParse(word, out var num))
                {
                    value.Add(num);
                    continue;
                }
                AddMessage(field, "参数值转换出错");
                Failed = true;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out List<long> value)
        {
            if (!Arguments.TryGetValue(field, out var str))
            {
                value = default;
                return false;
            }
            value = new List<long>();
            if (string.IsNullOrWhiteSpace(str))
            {
                return true;
            }
            var words = str.Split(new char[] { '[', ']', '\'', '\"', ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                if (long.TryParse(word, out var num))
                {
                    value.Add(num);
                    continue;
                }
                AddMessage(field, "参数值转换出错");
                Failed = true;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetIDs(string field, out List<long> value) => TryGetValue(field, out value);

        #endregion
    }
}
