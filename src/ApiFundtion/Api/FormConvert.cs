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
        /// <param name="controler"></param>
        /// <param name="data"></param>
        public FormConvert(ModelApiController controler, EditDataObject data)
        {
            Controler = controler;
            Data = data;
        }
        /// <summary>
        ///     读取过程的错误消息记录
        /// </summary>
        public ModelApiController Controler { get; }

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
        ///     是否发生解析错误
        /// </summary>
        public string Message
        {
            get
            {
                StringBuilder msg = new StringBuilder();
                foreach (var kv in _messages)
                {
                    var field = Data.__Struct.Properties.Values.FirstOrDefault(p => p.Name == kv.Key)?.Caption ?? kv.Key;
                    msg.AppendLine($"{field} : {kv.Value}<br/>");
                }
                return msg.ToString();
            }
        }

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
        #endregion

        #region 新方法 

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out string value)
        {
            return Controler.TryGetValue(field, out value);
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out byte value)
        {
            if (!Controler.TryGetValue(field, out var str))
            {
                value = 0;
                return false;
            }
            if (byte.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = 0;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = null;
                return false;
            }
            if (str == "null")
            {
                value = null;
                return true;
            }

            if (byte.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = null;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = 0;
                return false;
            }
            if (sbyte.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = 0;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = null;
                return false;
            }
            if (str == "null")
            {
                value = null;
                return true;
            }

            if (sbyte.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = null;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = 0;
                return false;
            }
            if (short.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = 0;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = null;
                return false;
            }
            if (str == "null")
            {
                value = null;
                return true;
            }

            if (short.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = null;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = 0;
                return false;
            }
            if (ushort.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = 0;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = null;
                return false;
            }
            if (str == "null")
            {
                value = null;
                return true;
            }

            if (ushort.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = null;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = false;
                return false;
            }

            switch (str.ToUpper())
            {
                case "1":
                case "yes":
                    value = true;
                    return true;
                case "0":
                case "no":
                    value = false;
                    return true;
            }
            if (!bool.TryParse(str, out var vl))
            {
                AddMessage(field, "参数值转换出错");
                Failed = true;
                value = false;
                return false;
            }
            value = vl;
            return true;
        }

        /// <summary>
        /// 字段值转换
        /// </summary>
        /// <param name="field">名称</param>
        /// <param name="value">字段名称</param>
        /// <returns>是否接收值</returns>
        public bool TryGetValue(string field, out bool? value)
        {
            if (!Controler.TryGetValue(field, out var str))
            {
                value = null;
                return false;
            }
            if (str == "null")
            {
                value = null;
                return true;
            }
            switch (str.ToUpper())
            {
                case "1":
                case "yes":
                    value = true;
                    return true;
                case "0":
                case "no":
                    value = false;
                    return true;
            }
            if (bool.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            value = null;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = 0;
                return false;
            }
            if (int.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            value = 0;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = null;
                return false;
            }
            if (str == "null")
            {
                value = null;
                return true;
            }

            if (int.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            value = null;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = 0;
                return false;
            }
            if (uint.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            value = 0;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = null;
                return false;
            }
            if (str == "null")
            {
                value = null;
                return true;
            }

            if (uint.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            value = null;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = 0;
                return false;
            }
            if (long.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            value = 0;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = null;
                return false;
            }
            if (str == "null")
            {
                value = null;
                return true;
            }

            if (long.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = null;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = 0;
                return false;
            }
            if (ulong.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = 0;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = null;
                return false;
            }
            if (str == "null")
            {
                value = null;
                return true;
            }

            if (ulong.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = null;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = 0;
                return false;
            }
            if (float.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = 0;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = null;
                return false;
            }
            if (str == "null")
            {
                value = null;
                return true;
            }

            if (float.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = null;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = 0;
                return false;
            }
            if (double.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = 0;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = null;
                return false;
            }
            if (str == "null")
            {
                value = null;
                return true;
            }

            if (double.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = null;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = DateTime.MinValue;
                return false;
            }
            if (DateTime.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = DateTime.MinValue;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = null;
                return false;
            }
            if (str == "null")
            {
                value = null;
                return true;
            }

            if (DateTime.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = null;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = 0M;
                return false;
            }
            if (decimal.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = 0M;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = null;
                return false;
            }
            if (str == "null")
            {
                value = null;
                return true;
            }

            if (decimal.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = null;
            Failed = true;
            AddMessage(field, "参数值转换出错");
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = Guid.Empty;
                return false;
            }
            if (Guid.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            value = Guid.Empty;
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = null;
                return false;
            }
            if (str == "null")
            {
                value = null;
                return true;
            }
            if (Guid.TryParse(str, out var vl))
            {
                value = vl;
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            value = null;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = null;
                return false;
            }
            if (Controler.TryGet(field, out List<int> ids))
            {
                value = ids;
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            value = null;
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
            if (!Controler.TryGetValue(field, out var str))
            {
                value = null;
                return false;
            }
            if (Controler.TryGetIDs(field, out var ids))
            {
                value = ids;
                return true;
            }
            AddMessage(field, "参数值转换出错");
            Failed = true;
            value = null;
            return false;
        }

        #endregion
    }
}
