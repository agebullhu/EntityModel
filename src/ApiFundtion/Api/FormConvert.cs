/*****************************************************
(c)2016-2021 by ZeroTeam
作者: 胡天水
工程: Agebull.EntityModel.CoreAgebull.DataModel
建立: 忘了日期
修改: -
*****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Text;
using ZeroTeam.MessageMVC.Context;

#endregion

namespace ZeroTeam.MessageMVC.ModelApi
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
        ///     参数
        /// </summary>
        readonly Dictionary<string, string> Messages = new Dictionary<string, string>();


        /// <summary>
        /// 设置错误字段
        /// </summary>
        /// <param name="field"></param>
        /// <param name="msg"></param>
        private void AddMessage(string field, string msg)
        {
            if (Messages.TryGetValue(field, out var val))
                Messages[field] = $"{val};{msg}";
            else
                Messages.Add(field, msg);
        }


        /// <summary>
        ///     是否发生解析错误
        /// </summary>
        public string Message
        {
            get
            {
                StringBuilder msg = new StringBuilder();
                foreach (var kv in Messages)
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
        Dictionary<string, string> Arguments => GlobalContext.Current.Message.ExtensionDictionary;


        /// <summary>
        ///     获取参数(文本)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="val">结果值</param>
        /// <returns>文本</returns>
        bool TryGet(string name, out string val)
        {
            if (!Arguments.TryGetValue(name, out var value))
            {
                val = null;
                return false;
            }
            val = string.IsNullOrWhiteSpace(value) ? null : value?.Trim();
            return true;
        }

        /// <summary>
        ///     读参数(泛型),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="convert">转换方法</param>
        /// <param name="value">参数值</param>
        /// <returns>如果参数存在且可转换为对应类型，则返回True</returns>
        public bool TryGet<T>(string name, Func<string, T> convert, out T value)
        {
            if (!TryGet(name, out string str))
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
        public bool TryGetNullable<T>(string name, Func<string, T> convert, out T? value)
            where T : struct
        {
            if (!TryGet(name, out string str))
            {
                value = default;
                return false;
            }
            if(str == null)
            {
                value = null;
                return true;
            }
            try
            {
                value = convert(str);
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
            return TryGet(field, out value);
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
            return TryGetNullable(field, str => Enum.Parse<TEnum>(str, true), out value);
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
            return TryGetNullable(field, byte.Parse, out value);
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
            return TryGetNullable(field, sbyte.Parse, out value);
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
            return TryGetNullable(field, short.Parse, out value);
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
            return TryGetNullable(field, ushort.Parse, out value);
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
            return TryGetNullable(field, str =>
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
            return TryGetNullable(field, int.Parse, out value);
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
            return TryGetNullable(field, uint.Parse, out value);
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
            return TryGetNullable(field, long.Parse, out value);
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
            return TryGetNullable(field, ulong.Parse, out value);
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
            return TryGetNullable(field, float.Parse, out value);
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
            return TryGetNullable(field, double.Parse, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out DateTime value)
        {
            return TryGet(field, vl => vl == null ? default: DateTime.Parse(vl), out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out DateTime? value)
        {
            return TryGetNullable(field, DateTime.Parse, out value);
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
            return TryGetNullable(field, decimal.Parse, out value);
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
            return TryGetNullable(field, Guid.Parse, out value);
        }


        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out List<int> value)
        {
            if (!TryGet(field, out string str))
            {
                value = default;
                return false;
            }
            value = new List<int>();
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
            return value.Count > 0;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out List<long> value)
        {
            if (!TryGet(field, out string str))
            {
                value = default;
                return false;
            }
            value = new List<long>();
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
            return value.Count > 0;
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
