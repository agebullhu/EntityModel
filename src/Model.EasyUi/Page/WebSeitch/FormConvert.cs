// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#endregion

namespace Gboxt.Common.WebUI
{
    /// <summary>
    /// FORM对象转化辅助类
    /// </summary>
    public class FormConvert
    {
        /// <summary>
        /// 仅接受Form即POST参数吗?
        /// </summary>
        public bool OnlyForm { get; set; }
        /// <summary>
        /// 读取过程的错误消息记录
        /// </summary>
        public readonly Dictionary<string, string> Messages = new Dictionary<string, string>();
        /// <summary>
        /// 是否发生解析错误
        /// </summary>
        public bool Failed { get; set; }
        /// <summary>
        /// HttpRequest对象
        /// </summary>
        public HttpRequest Request { get; set; }

        private string GetValue(string name)
        {
            var val = !OnlyForm ? Request[name] : Request.Form[name];

            return val?.Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="canNull"></param>
        /// <returns></returns>
        public string ToString(string name, bool canNull)
        {
            if (!string.IsNullOrWhiteSpace(GetValue(name)))
            {
                return GetValue(name).Trim();
            }
            //if (!canNull)
            //{
            //    AddMessage(name, "值不能为空");
            //    this.Failed = true;
            //}
            return null;
        }

        void AddMessage(string name, string msg)
        {
            if (Messages.ContainsKey(name))
                Messages[name] = msg;
            else
                Messages.Add(name, msg);
        }
        public string ToString(string name, string def = null)
        {
            return string.IsNullOrWhiteSpace(GetValue(name)) ? def : GetValue(name).Trim();
        }

        public byte ToByte(string name)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                AddMessage(name, "值不能为空");
                Failed = true;
                return 0;
            }
            byte vl;
            if (!byte.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为数字");
                Failed = true;
                return 0;
            }
            return vl;
        }
        public byte? ToNullByte(string name)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                AddMessage(name, "值不能为空");
                Failed = true;
                return null;
            }
            byte vl;
            if (!byte.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为数字");
                Failed = true;
                return null;
            }
            return vl;
        }

        public sbyte ToSByte(string name)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                AddMessage(name, "值不能为空");
                Failed = true;
                return 0;
            }
            sbyte vl;
            if (!sbyte.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为数字");
                Failed = true;
                return 0;
            }
            return vl;
        }
        public sbyte? ToNullSByte(string name)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                AddMessage(name, "值不能为空");
                Failed = true;
                return null;
            }
            sbyte vl;
            if (!sbyte.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为数字");
                Failed = true;
                return null;
            }
            return vl;
        }
        public long ToLong(string name)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                AddMessage(name, "值不能为空");
                Failed = true;
                return 0;
            }
            long vl;
            if (!long.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为数字");
                Failed = true;
                return 0;
            }
            return vl;
        }

        public long ToLong(string name, long def)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                return def;
            }
            long vl;
            if (!long.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为数字");
                Failed = true;
                return 0;
            }
            return vl;
        }

        public long? ToNullLong(string name)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                return null;
            }
            long vl;
            if (!long.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为数字");
                Failed = true;
                return null;
            }
            return vl;
        }
        public uint ToUInteger(string name, bool canNull = true)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                if (!canNull)
                {
                    AddMessage(name, "值不能为空");
                    Failed = true;
                }
                return 0;
            }
            uint vl;
            if (!uint.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为数字");
                Failed = true;
                return 0;
            }
            return vl;
        }

        public int ToInteger(string name, bool canNull = true)
        {
            var val = Request.Form[name];
            if (string.IsNullOrWhiteSpace(val))
            {
                if (!canNull)
                {
                    AddMessage(name, "值不能为空");
                    Failed = true;
                }
                return 0;
            }
            int vl;
            if (!int.TryParse(val, out vl))
            {
                AddMessage(name, "值无法转为数字");
                Failed = true;
                return 0;
            }
            return vl;
        }

        public int ToInteger(string name,Func<string,string> procFunc)
        {
            var str = GetValue(name);
            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }
            str = procFunc(str);
            int vl;
            if (!int.TryParse(str, out vl))
            {
                AddMessage(name, "值无法转为数字");
                Failed = true;
                return 0;
            }
            return vl;
        }

        public int ToInteger(string name, int def)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                return def;
            }
            int vl;
            if (!int.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为数字");
                Failed = true;
                return 0;
            }
            return vl;
        }

        public int? ToNullInteger(string name)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                return null;
            }
            int vl;
            if (!int.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为数字");
                Failed = true;
                return null;
            }
            return vl;
        }

        public bool ToBoolean(string name)
        {
            var str = GetValue(name);
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }
            switch (str.ToLower())
            {
                case "0":
                case "off":
                case "no":
                case "false":
                    return false;
                case "1":
                case "on":
                case "yes":
                case "true":
                    return true;
            }
            Failed = true;
            return false;
        }

        public bool ToBoolean(string name, bool def)
        {
            var str = GetValue(name);
            if (string.IsNullOrWhiteSpace(str))
            {
                return def;
            }
            switch (str.ToLower())
            {
                case "0":
                case "off":
                case "no":
                case "false":
                    return false;
                case "1":
                case "on":
                case "yes":
                case "true":
                    return true;
            }
            Failed = true;
            return false;
        }

        public bool? ToNullBoolean(string name)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                return null;
            }
            return ToBoolean(name);
        }

        public decimal ToDecimal(string name, bool canNull = true)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                if (!canNull)
                {
                    AddMessage(name, "值不能为空");
                    Failed = true;
                }
                return 0;
            }
            decimal vl;
            if (!decimal.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为小数");
                Failed = true;
                return 0;
            }
            return vl;
        }

        public decimal ToDecimal(string name, decimal def)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                return def;
            }
            decimal vl;
            if (!decimal.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为小数");
                Failed = true;
                return 0;
            }
            return vl;
        }

        public decimal? ToNullDecimal(string name)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                return null;
            }
            decimal vl;
            if (!decimal.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为小数");
                Failed = true;
                return null;
            }
            return vl;
        }

        public Guid ToGuid(string name)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                AddMessage(name, "值不能为空");
                Failed = true;
                return Guid.Empty;
            }
            Guid vl;
            if (!Guid.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为GUID");
                Failed = true;
                return Guid.Empty;
            }
            return vl;
        }

        public Guid ToGuid(string name, Guid def)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                return def;
            }
            Guid vl;
            if (!Guid.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为GUID");
                Failed = true;
                return Guid.Empty;
            }
            return vl;
        }

        public Guid? ToNullGuid(string name)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                return null;
            }
            Guid vl;
            if (!Guid.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为GUID");
                Failed = true;
                return null;
            }
            return vl;
        }

        public double ToDouble(string name)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                AddMessage(name, "值不能为空");
                Failed = true;
                return 0;
            }
            double vl;
            if (!double.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为小数");
                Failed = true;
                return 0;
            }
            return vl;
        }

        public double ToDouble(string name, double def)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                return def;
            }
            double vl;
            if (!double.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为小数");
                Failed = true;
                return 0;
            }
            return vl;
        }

        public double? ToNullDouble(string name)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                return null;
            }
            double vl;
            if (!double.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为小数");
                Failed = true;
                return null;
            }
            return vl;
        }

        public float ToSingle(string name)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                AddMessage(name, "值不能为空");
                Failed = true;
                return 0;
            }
            float vl;
            if (!float.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为小数");
                Failed = true;
                return 0;
            }
            return vl;
        }

        public float ToSingle(string name, float def)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                return def;
            }
            float vl;
            if (!float.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为小数");
                Failed = true;
                return 0;
            }
            return vl;
        }

        public float? ToNullSingle(string name)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                return null;
            }
            float vl;
            if (!float.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为小数");
                Failed = true;
                return null;
            }
            return vl;
        }

        public DateTime ToDateTime(string name)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                AddMessage(name, "值不能为空");
                Failed = true;
                return DateTime.MinValue;
            }
            DateTime vl;
            if (!DateTime.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为日期");
                Failed = true;
                return DateTime.MinValue;
            }
            return vl;
        }

        public DateTime ToDateTime(string name, DateTime def)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                return def;
            }
            DateTime vl;
            if (!DateTime.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为日期");
                Failed = true;
                return DateTime.MinValue;
            }
            return vl;
        }

        public DateTime? ToNullDateTime(string name)
        {
            if (string.IsNullOrWhiteSpace(GetValue(name)))
            {
                return null;
            }
            DateTime vl;
            if (!DateTime.TryParse(GetValue(name), out vl))
            {
                AddMessage(name, "值无法转为日期");
                Failed = true;
                return null;
            }
            return vl;
        }
        public List<int> ToArray(string name)
        {
            var cs = GetValue(name);
            if (string.IsNullOrWhiteSpace(cs))
            {
                return null;
            }
            var css = cs.Trim('[', ']').Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return css.Length > 0 ? css.Select(int.Parse).ToList() : null;
        }

        public List<T>  ToArray<T>(string name,Func<string,T> parse)
        {
            var cs = GetValue(name);
            if (string.IsNullOrWhiteSpace(cs))
            {
                return null;
            }
            var css = cs.Trim('[', ']').Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return css.Length > 0 ? css.Select(parse).ToList() : null;
        }
    }
}