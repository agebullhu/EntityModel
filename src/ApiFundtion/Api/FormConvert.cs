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
        /// 构造
        /// </summary>
        /// <param name="data"></param>
        public FormConvert(EditDataObject data)
        {
            Data = data;
        }

        /// <summary>
        ///     读取过程的错误消息记录
        /// </summary>
        public EditDataObject Data { get; }

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
        private readonly Dictionary<string, string> _messages = new Dictionary<string, string>();

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
                    var field = Data.__Struct.Properties.Values.FirstOrDefault(p => p.JsonName == kv.Key)?.Caption ?? kv.Key;
                    msg.AppendLine($"{field} : {kv.Value}");
                }
                return msg.ToString();
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
            return RequestArgumentConvert.TryGet(field, out value);
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
            return RequestArgumentConvert.TryGetEnum(field, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out byte value)
        {
            if (RequestArgumentConvert.TryGet(field, byte.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out byte? value)
        {
            if (RequestArgumentConvert.TryGet(field, byte.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out sbyte value)
        {
            if (RequestArgumentConvert.TryGet(field, sbyte.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out sbyte? value)
        {
            if (RequestArgumentConvert.TryGet(field, sbyte.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out short value)
        {
            if (RequestArgumentConvert.TryGet(field, short.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out short? value)
        {
            if (RequestArgumentConvert.TryGet(field, short.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out ushort value)
        {
            if (RequestArgumentConvert.TryGet(field, ushort.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out ushort? value)
        {
            if (RequestArgumentConvert.TryGet(field, ushort.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }
        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out bool value)
        {
            if (RequestArgumentConvert.TryGet(field, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out bool? value)
        {
            if (RequestArgumentConvert.TryGet(field, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out int value)
        {
            if (RequestArgumentConvert.TryGet(field, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out int? value)
        {
            if (RequestArgumentConvert.TryGet(field, int.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }
        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out uint value)
        {
            if (RequestArgumentConvert.TryGet(field, uint.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out uint? value)
        {
            if (RequestArgumentConvert.TryGet(field, uint.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out long value)
        {
            if (RequestArgumentConvert.TryGet(field, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out long? value)
        {
            if (RequestArgumentConvert.TryGet(field, long.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out ulong value)
        {
            if (RequestArgumentConvert.TryGet(field, ulong.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out ulong? value)
        {
            if (RequestArgumentConvert.TryGet(field, ulong.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out float value)
        {
            if (RequestArgumentConvert.TryGet(field, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out float? value)
        {
            if (RequestArgumentConvert.TryGet(field, float.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out double value)
        {
            if (RequestArgumentConvert.TryGet(field, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out double? value)
        {
            if (RequestArgumentConvert.TryGet(field, double.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out DateTime value)
        {
            if (RequestArgumentConvert.TryGet(field, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out DateTime? value)
        {
            if (RequestArgumentConvert.TryGet(field, DateTime.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out decimal value)
        {
            if (RequestArgumentConvert.TryGet(field, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out decimal? value)
        {
            if (RequestArgumentConvert.TryGet(field, decimal.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }
        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out Guid value)
        {
            if (RequestArgumentConvert.TryGet(field, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out Guid? value)
        {
            if (RequestArgumentConvert.TryGet(field, Guid.Parse, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }


        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out List<int> value)
        {
            if (RequestArgumentConvert.TryGet(field, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }
        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out List<long> value)
        {
            if (RequestArgumentConvert.TryGet(field, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetIDs(string field, out List<long> value)
        {
            if (RequestArgumentConvert.TryGet(field, out value))
            {
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            return false;
        }
        #endregion
    }
}
