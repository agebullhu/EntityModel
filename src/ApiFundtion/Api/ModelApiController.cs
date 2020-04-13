using System;
using System.Collections.Generic;
using System.Linq;
using ZeroTeam.MessageMVC.Context;
using ZeroTeam.MessageMVC.ZeroApis;
using ZeroTeam.MessageMVC.Messages;

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    /// ZeroApi控制器基类
    /// </summary>
    public class ModelApiController : IApiControler
    {
        #region 基本属性
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public IUser UserInfo { get; set; }

        IInlineMessage _message;

        /// <summary>
        /// 原始调用帧消息
        /// </summary>
        public IInlineMessage Message => _message ??= GlobalContext.Current.Message;

        #endregion

        #region 状态

        /// <summary>
        ///     是否操作失败
        /// </summary>
        protected internal bool IsFailed => GlobalContext.Current.Status.LastState != DefaultErrorCode.Success;

        /// <summary>
        ///     设置当前操作失败
        /// </summary>
        /// <param name="message"></param>
        protected internal void SetFailed(string message)
        {
            GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
            GlobalContext.Current.Status.LastMessage = message;
        }

        #endregion

        #region 权限相关

        /*// <summary>
        ///     是否公开页面
        /// </summary>
        internal protected bool IsPublicPage => BusinessContext.Context.PageItem.IsPublic;

        /// <summary>
        ///     当前页面节点配置
        /// </summary>
        public IPageItem PageItem => BusinessContext.Context.PageItem;

        /// <summary>
        ///     当前页面权限配置
        /// </summary>
        public IRolePower PagePower => BusinessContext.Context.CurrentPagePower;*/

        /// <summary>
        ///     当前用户是否已登录成功
        /// </summary>
        protected internal bool UserIsLogin => UserInfo.UserId > 0;

        #endregion

        #region 参数解析

        /// <summary>
        ///     参数
        /// </summary>
        protected internal Dictionary<string, string> Arguments => Message.Dictionary;


        /// <summary>
        /// 获取或新增(修改)参数
        /// </summary>
        /// <param name="arg">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数值</returns>
        protected internal string this[string arg]
        {
            get
            {
                if (arg == null || !Message.Dictionary.TryGetValue(arg, out var val))
                    return null;
                return val?.ToString();
            }
            set
            {
                if (Arguments.ContainsKey(arg))
                    Arguments[arg] = value;
                else
                    Arguments.Add(arg, value);
            }
        }

        /// <summary>
        ///     当前请求是否包含这个参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>是否包含这个参数</returns>
        protected internal bool ContainsArgument(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && Arguments.ContainsKey(name);
        }

        /// <summary>
        ///     设置替代参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        protected internal void SetArg(string name, string value)
        {
            this[name] = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        /// <summary>
        ///     设置替代参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        protected internal void SetArg(string name, int value)
        {
            this[name] = value.ToString();
        }

        /// <summary>
        ///     设置替代参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        protected internal void SetArg(string name, object value)
        {
            this[name] = value?.ToString();
        }

        /// <summary>
        ///     获取参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        [Obsolete("请改为GetString方法")]
        protected internal string GetArgValue(string name)
        {
            return this[name];
        }

        /// <summary>
        ///     获取参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        [Obsolete("请改为GetString方法")]
        protected internal string GetArg(string name)
        {
            return GetString(name);
        }

        /// <summary>
        ///     获取参数(文本)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="val"></param>
        /// <returns>文本</returns>
        protected internal bool TryGetValue(string name, out string val)
        {
            if (!Arguments.TryGetValue(name, out var value) || value == null)
            {
                val = null;
                return false;
            }
            if (!(value is string vl) || string.IsNullOrEmpty(vl))
            {
                val = null;
                return false;
            }
            val = vl;
            return true;
        }

        /// <summary>
        ///     获取参数(文本)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>文本</returns>
        protected internal string GetString(string name)
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
        protected internal T? GetNullArg<T>(string name, Func<string, T> convert, T? def = null)
            where T : struct
        {
            if (!Arguments.TryGetValue(name, out var value) || value == null)
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
        protected internal T GetArg<T>(string name, Func<string, T> convert, T def)
        {
            if (!Arguments.TryGetValue(name, out var value) || string.IsNullOrEmpty(value))
                return def;
            return convert(value.Trim());
        }


        /// <summary>
        ///     读参数(泛型)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="convert">转换方法</param>
        /// <returns>参数为空或不存在,返回不成功,其它情况视convert返回值自行控制</returns>
        protected internal bool GetArg(string name, Func<string, bool> convert)
        {
            return GetArg(name, bool.Parse, false);
        }

        /// <summary>
        ///     获取参数int类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为0,如果存在且不能转为int类型将出现异常</returns>
        protected internal int GetIntArg(string name)
        {
            return GetArg(name, int.Parse, 0);
        }

        /// <summary>
        ///     获取参数int类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为0,如果存在且不能转为int类型将出现异常</returns>
        protected internal double GetDoubleArg(string name)
        {
            return GetArg(name, double.Parse, 0.0);
        }

        /// <summary>
        ///     获取参数int类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为0,如果存在且不能转为int类型将出现异常</returns>
        protected internal float GetSingleArg(string name)
        {
            return GetArg(name, float.Parse, 0.0F);
        }

        /// <summary>
        ///     获取参数int类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为0,如果存在且不能转为int类型将出现异常</returns>
        protected internal Guid GetGuidArg(string name)
        {
            return GetArg(name, Guid.Parse, Guid.Empty);
        }
        /// <summary>
        ///     获取参数int类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为0,如果存在且不能转为int类型将出现异常</returns>
        protected internal byte GetByteArg(string name)
        {
            return GetArg(name, byte.Parse, (byte)0);
        }

        /// <summary>
        ///     获取参数int类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为0,如果存在且不能转为int类型将出现异常</returns>
        protected internal int[] GetIntArrayArg(string name)
        {
            if (!TryGetValue(name, out var value))
                return new int[0];

            return string.IsNullOrEmpty(value)
                ? (new int[0])
                : value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }

        /// <summary>
        ///     获取参数(int类型),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="def">默认值</param>
        /// <returns>int类型,如果存在且不能转为int类型将出现异常</returns>
        protected internal int GetIntArg(string name, int def)
        {
            return GetArg(name, int.Parse, def);
        }

        /// <summary>
        ///     获取参数(数字),模糊名称读取
        /// </summary>
        /// <param name="names">多个名称</param>
        /// <returns>名称解析到的第一个不为0的数字,如果有名称存在且不能转为int类型将出现异常</returns>
        protected internal int GetIntAnyArg(params string[] names)
        {
            return names.Select(p => GetIntArg(p, 0)).FirstOrDefault(re => re != 0);
        }

        /// <summary>
        ///     获取参数(日期类型)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>日期类型,为空则为空,如果存在且不能转为日期类型将出现异常</returns>
        protected internal DateTime? GetDateArg(string name)
        {
            return GetNullArg(name, DateTime.Parse);
        }

        /// <summary>
        ///     获取参数(日期类型)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>日期类型,为空则为DateTime.MinValue,如果存在且不能转为日期类型将出现异常</returns>
        protected internal DateTime GetDateArg2(string name)
        {
            return GetArg(name, DateTime.Parse, DateTime.MinValue);
        }

        /// <summary>
        ///     获取参数(日期类型)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="def"></param>
        /// <returns>日期类型,为空则为空,如果存在且不能转为日期类型将出现异常</returns>
        protected internal DateTime GetDateArg(string name, DateTime def)
        {
            return GetArg(name, DateTime.Parse, def);
        }


        /// <summary>
        ///     获取参数bool类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为0,如果存在且不能转为int类型将出现异常</returns>
        protected internal bool GetBoolArg(string name)
        {
            return GetArg(name, bool.Parse, false);
        }


        /// <summary>
        ///     获取参数(decimal型数据)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>decimal型数据,如果未读取值则为-1,如果存在且不能转为decimal类型将出现异常</returns>
        protected internal decimal GetDecimalArg(string name)
        {
            return GetArg(name, decimal.Parse, 0M);
        }

        /// <summary>
        ///     获取参数(decimal型数据),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="def">默认值</param>
        /// <returns>decimal型数据,如果存在且不能转为decimal类型将出现异常</returns>
        protected internal decimal GetDecimalArg(string name, decimal def)
        {
            return GetArg(name, decimal.Parse, def);
        }

        /// <summary>
        ///     获取参数(long型数据),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="def">默认值</param>
        /// <returns>long型数据,如果存在且不能转为long类型将出现异常</returns>
        protected internal long GetLongArg(string name, long def = -1)
        {
            return GetArg(name, long.Parse, def);
        }

        /// <summary>
        ///     获取参数int类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为0,如果存在且不能转为int类型将出现异常</returns>
        protected internal long[] GetLongArrayArg(string name)
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
        protected internal long GetLongAnyArg(params string[] names)
        {
            return names.Select(p => GetLongArg(p, 0)).FirstOrDefault(re => re != 0);
        }

        #region TryGet

        /// <summary>
        ///     读参数(泛型),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="convert">转换方法</param>
        /// <param name="value">参数值</param>
        /// <returns>值</returns>
        protected internal bool TryGe<T>(string name, Func<string, T> convert, out T value)
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
        ///     尝试获取参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>值是否存在</returns>
        protected internal bool TryGet(string name, out string value)
        {
            return TryGetValue(name, out value);
        }


        /// <summary>
        ///     读参数,如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>值</returns>
        protected internal bool TryGet(string name, out bool value)
        {
            if (!TryGetValue(name, out var str))
            {
                value = false;
                return false;
            }
            try
            {
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
        ///     读参数,如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>值</returns>
        protected internal bool TryGet(string name, out DateTime value)
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
        ///     读参数,如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>值</returns>
        protected internal bool TryGet(string name, out int value)
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
        ///     读参数,如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>值</returns>
        protected internal bool TryGet(string name, out decimal value)
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
        ///     读参数,如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>值</returns>
        protected internal bool TryGet(string name, out float value)
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
        ///     读参数,如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>值</returns>
        protected internal bool TryGet(string name, out double value)
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
        ///     读参数,如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>值</returns>
        protected internal bool TryGet(string name, out short value)
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
        /// <param name="value">参数值</param>
        /// <returns>值</returns>
        protected internal bool TryGetId<TData>(out long value)
           where TData : EntityModel.Common.EditDataObject, new()
        {
            if (TryGet("id", out value))
                return true;
            var data = new TData();
            var pri = data.__Struct.Properties.Values.First(p => p.Name == data.__Struct.PrimaryKey);
            if (TryGet(pri.JsonName, out value))
                return true;
            return TryGet(pri.Name, out value);
        }

        /// <summary>
        ///     读参数,如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>值</returns>
        protected internal bool TryGet(string name, out long value)
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
        ///     读参数,如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>值</returns>
        protected internal bool TryGet(string name, out string[] value)
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
        ///     读参数,如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>值</returns>
        protected internal bool TryGet(string name, out int[] value)
        {
            if (!TryGetValue(name, out var str))
            {
                value = new int[0];
                return false;
            }
            try
            {
                value = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                return value.Length > 0;
            }
            catch
            {
                value = new int[0];
                return false;
            }
        }

        /// <summary>
        ///     读参数,如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>值</returns>
        protected internal bool TryGet(string name, out List<int> value)
        {
            if (!TryGetValue(name, out var str))
            {
                value = new List<int>();
                return false;
            }
            try
            {
                value = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                return value.Count > 0;
            }
            catch
            {
                value = new List<int>();
                return false;
            }
        }
        /// <summary>
        ///     读参数,如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>值</returns>
        protected internal bool TryGetIDs(string name, out List<long> value)
        {
            if (!TryGetValue(name, out var str))
            {
                value = new List<long>();
                return false;
            }
            try
            {
                value = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
                return value.Count > 0;
            }
            catch
            {
                value = new List<long>();
                return false;
            }
        }

        /// <summary>
        ///     读参数,如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>值</returns>
        protected internal bool TryGet(string name, out long[] value)
        {
            if (!TryGetValue(name, out var str))
            {
                value = new long[0];
                return false;
            }
            try
            {
                value = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
                return value.Length > 0;
            }
            catch
            {
                value = new long[0];
                return false;
            }
        }
        #endregion
        #endregion
    }
}