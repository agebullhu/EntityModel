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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.UI;
using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Gboxt.Common.DataModel;
using Gboxt.Common.SystemModel;

#endregion

namespace Gboxt.Common.WebUI
{
    /// <summary>
    ///     页面的基类
    /// </summary>
    public abstract class MyPageBase : Page
    {
        #region 权限相关

        AspBusinessContext _context;

        /// <summary>
        /// 上下文对象
        /// </summary>
        public AspBusinessContext ModelContext =>
            _context ?? (_context = IocHelper.Create<IBusinessContext>() as AspBusinessContext);


        /// <summary>
        ///     是否页面内动作不检查
        /// </summary>
        protected bool AllAction { get; set; }

        /// <summary>
        ///     是否公开页面
        /// </summary>
        protected bool IsPublicPage { get; set; }

        /// <summary>
        ///     当前页面节点配置
        /// </summary>
        public IPageItem PageItem { get; private set; }

        /// <summary>
        ///     当前页面权限配置
        /// </summary>
        public IRolePower PagePower { get; private set; }

        /// <summary>
        ///     当前用户是否已登录成功
        /// </summary>
        protected bool UserIsLogin => LoginUser != null && ModelContext.LoginUserId > 0;

        /// <summary>
        ///     当前登录用户
        /// </summary>
        protected ILoginUser LoginUser => ModelContext.LoginUser;

        /// <summary>
        ///     当前主页面(对应配置名称)
        /// </summary>
        protected string CurrentPageName { get; private set; }

        /// <summary>
        /// 载入用户账号信息
        /// </summary>
        protected bool LoadAuthority()
        {
            ModelContext.PowerChecker.ReloadLoginUserInfo();
            if (!CheckLogined())
            {
                LogRecorder.MonitorTrace($"非法用户:{Request.RawUrl}");
                return false;
            }
            LogRecorder.MonitorTrace($"当前用户:{LoginUser.RealName}({LoginUser.Id})**{LoginUser.RealName}");
            if (IsPublicPage)
            {
                LogRecorder.MonitorTrace("公共页面");
                PagePower = new SimpleRolePower
                {
                    Id = -1,
                    PageItemId = -1,
                    Power = 1,
                    RoleId = LoginUser.RoleId
                };
            }
            else
            {
                PageItem = ModelContext.PowerChecker.LoadPageConfig(CurrentPageName);
                if (PageItem == null)
                {
                    LogRecorder.MonitorTrace("非法页面");
                    return false;
                }
                if (PageItem.IsHide)
                {
                    LogRecorder.MonitorTrace("隐藏页面");
                    PagePower = new SimpleRolePower
                    {
                        Id = -1,
                        PageItemId = -1,
                        Power = 1,
                        RoleId = LoginUser.RoleId
                    };
                }
                else
                {
                    PagePower = ModelContext.PowerChecker.LoadPagePower(ModelContext.LoginUser, PageItem);
                    if (PagePower == null)
                    {
                        LogRecorder.MonitorTrace("非法访问");
                        return false;
                    }
                    LogRecorder.MonitorTrace("授权访问");
                }
            }
            ModelContext.PageItem = PageItem;
            ModelContext.CurrentPagePower = PagePower;
            return true;
        }

        /// <summary>
        ///     检查登录情况
        /// </summary>
        protected virtual bool CheckLogined()
        {
            return UserIsLogin;
        }

        /// <summary>
        ///     检查动作是否允许
        /// </summary>
        protected virtual bool CheckCanDo()
        {
            return true;
        }

        /// <summary>
        /// 分析当前访问的主页面
        /// </summary>
        /// <returns></returns>
        private string GetFriendPageUrl()
        {
            var paths = Request.RawUrl.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var friendPage = "/";
            var index = 0;
            for (; index < paths.Length - 1; index++)
            {
                friendPage += paths[index] + "/";
            }
            friendPage += "Index.aspx";
            return friendPage;
        }

        #endregion

        #region 页面载入处理

        /// <summary>
        ///     页面操作处理入口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentPageName = GetFriendPageUrl();
            LogRecorder.BeginMonitor(CurrentPageName);
            LogRecorder.MonitorTrace(Request.Url.AbsolutePath);

            LogRequestInfo();
            try
            {
                ProcessRequest();
            }
            catch (Exception exception)
            {
                LogRecorder.EndStepMonitor();
                LogRecorder.BeginStepMonitor("Exception");
                LogRecorder.MonitorTrace(exception.Message);
                LogRecorder.Exception(exception);
                Debug.WriteLine(exception);
                OnFailed(exception);
                LogRecorder.EndStepMonitor();
            }
            LogRecorder.BeginStepMonitor("Result");
            OnResult();
            LogRecorder.EndStepMonitor();
            LogRecorder.EndMonitor();
        }
        private void LogRequestInfo()
        {
            if (!LogRecorder.LogMonitor)
                return;
            var args = new StringBuilder();
            args.Append("Headers:");
            foreach (var head in Request.Headers.AllKeys)
            {
                args.Append($"[{head}]{Request[head]}");
            }
            LogRecorder.MonitorTrace(args.ToString());
            args.Clear();
            args.Append("QueryString:");
            foreach (var head in Request.QueryString.AllKeys)
            {
                args.Append($"[{head}]{Request[head]}");
            }
            LogRecorder.MonitorTrace(args.ToString());
            args.Clear();
            args.Append("Form:");
            foreach (var head in Request.Form.AllKeys)
            {
                args.Append($"[{head}]{Request[head]}");
            }
            LogRecorder.MonitorTrace(args.ToString());
            args.Clear();
            args.Append("Cookies:");
            foreach (var head in Request.Cookies.AllKeys)
            {
                args.Append($"[{head}]{Request[head]}");
            }
            LogRecorder.MonitorTrace(args.ToString());
        }

        private void ProcessRequest()
        {
            LogRecorder.BeginStepMonitor("LoadAuthority");
            var canDo = LoadAuthority();
            LogRecorder.MonitorTrace(canDo.ToString());
            LogRecorder.EndStepMonitor();
            if (!canDo)
            {
                ModelContext.LastMessage = "非法访问";
                return;
            }
            LogRecorder.BeginStepMonitor("Prepare");
            OnPrepare();
            LogRecorder.EndStepMonitor();
            LogRecorder.BeginStepMonitor("CheckCanDo");
            canDo = IsPublicPage || CheckCanDo();
            LogRecorder.MonitorTrace(canDo.ToString());
            LogRecorder.EndStepMonitor();
            if (!canDo)
                return;
            LogRecorder.BeginStepMonitor("OnPageLoaded");
            OnPageLoaded();
            LogRecorder.EndStepMonitor();
        }

        /// <summary>
        ///     页面处理前准备
        /// </summary>
        protected abstract void OnPrepare();

        /// <summary>
        ///     页面处理结束
        /// </summary>
        protected abstract void OnResult();

        /// <summary>
        ///     页面载入后的处理
        /// </summary>
        protected abstract void OnPageLoaded();

        /// <summary>
        ///     页面执行出错的处理
        /// </summary>
        protected virtual void OnFailed(Exception exception)
        {
        }

        #endregion

        #region 参数解析


        /// <summary>
        ///     转换页面参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="action">转换方法</param>
        protected void ConvertQueryString(string name, Action<string> action)
        {
            var val = Request[name];
            if (!string.IsNullOrEmpty(val))
            {
                action(val);
            }
        }

        /// <summary>
        ///     当前请求是否包含这个参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>是否包含这个参数</returns>
        protected bool ContainsArgument(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            return Request[name] != null;
        }
        /// <summary>
        /// 替代参数
        /// </summary>
        private readonly Dictionary<string, string> _args = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 设置替代参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetArg(string name, string value)
        {
            if (_args.ContainsKey(name))
                _args[name] = value;
            else _args.Add(name, value);
        }
        /// <summary>
        /// 设置替代参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetArg(string name, int value)
        {
            if (_args.ContainsKey(name))
                _args[name] = value.ToString();
            else _args.Add(name, value.ToString());
        }
        /// <summary>
        /// 设置替代参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetArg(string name, object value)
        {
            if (_args.ContainsKey(name))
                _args[name] = value?.ToString();
            else _args.Add(name, value?.ToString());
        }

        /// <summary>
        /// 设置替代参数
        /// </summary>
        /// <param name="name"></param>
        protected string GetArgValue(string name)
        {
            if (_args.ContainsKey(name))
                return _args[name];
            return Request[name];
        }

        /// <summary>
        ///     读取页面参数(文本)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>文本</returns>
        protected string GetArg(string name)
        {
            var value = GetArgValue(name);
            if (value == null)
            {
                return null;
            }
            var vl = value.Trim();
            if (vl == "null")
            {
                return null;
            }
            if (value == "undefined")
            {
                return null;
            }
            return string.IsNullOrWhiteSpace(vl) ? null : vl;
        }

        /// <summary>
        ///     读参数(文本),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="convert">转换方法</param>
        /// <param name="def">默认值</param>
        /// <returns>文本</returns>
        protected T GetArg<T>(string name, Func<string, T> convert, T def)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return def;
            }
            return convert(value);
        }


        /// <summary>
        ///     读参数(泛型)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="convert">转换方法</param>
        /// <returns>参数为空或不存在,返回不成功,其它情况视convert返回值自行控制</returns>
        protected bool GetArg(string name, Func<string, bool> convert)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return false;
            }
            return convert(value);
        }

        /// <summary>
        ///     读参数(文本),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="def">默认值</param>
        /// <returns>文本</returns>
        protected string GetArg(string name, string def)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return def;
            }
            return value;
        }

        /// <summary>
        ///     读取页面参数(日期类型)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>日期类型,为空则为空,如果存在且不能转为日期类型将出现异常</returns>
        protected DateTime? GetDateArg(string name)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return null;
            }
            return DateTime.Parse(value);
        }

        /// <summary>
        ///     读取页面参数(日期类型)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>日期类型,为空则为DateTime.MinValue,如果存在且不能转为日期类型将出现异常</returns>
        protected DateTime GetDateArg2(string name)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return DateTime.MinValue;
            }
            return DateTime.Parse(value);
        }

        /// <summary>
        ///     读取页面参数(日期类型)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="def"></param>
        /// <returns>日期类型,为空则为空,如果存在且不能转为日期类型将出现异常</returns>
        protected DateTime GetDateArg(string name, DateTime def)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return def;
            }
            return DateTime.Parse(value);
        }

        /// <summary>
        ///     读取页面参数int类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为-1,如果存在且不能转为int类型将出现异常</returns>
        protected int GetIntArg(string name)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return -1;
            }
            return int.Parse(value);
        }

        /// <summary>
        ///     读取页面参数int类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为-1,如果存在且不能转为int类型将出现异常</returns>
        protected int[] GetIntArrayArg(string name)
        {
            var value = GetArgValue(name);

            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return new int[0];
            }
            return value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }

        /// <summary>
        ///     读取页面参数bool类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>int类型,为空则为-1,如果存在且不能转为int类型将出现异常</returns>
        protected bool GetBoolArg(string name)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return false;
            }
            return value != "0" && (value == "1" || value == "yes" || bool.Parse(value));
        }

        /// <summary>
        ///     读取页面参数(int类型),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="def">默认值</param>
        /// <returns>int类型,如果存在且不能转为int类型将出现异常</returns>
        protected int GetIntArg(string name, int def)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "NaN" || value == "undefined" || value == "null")
            {
                return def;
            }
            return int.Parse(value);
        }

        /// <summary>
        ///     读取页面参数(数字),模糊名称读取
        /// </summary>
        /// <param name="names">多个名称</param>
        /// <returns>名称解析到的第一个不为0的数字,如果有名称存在且不能转为int类型将出现异常</returns>
        protected int GetIntAnyArg(params string[] names)
        {
            return names.Select(p => GetIntArg(p, 0)).FirstOrDefault(re => re != 0);
        }

        /// <summary>
        ///     读取页面参数(decimal型数据)
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>decimal型数据,如果未读取值则为-1,如果存在且不能转为decimal类型将出现异常</returns>
        protected decimal GetDecimalArg(string name)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return -1;
            }
            return decimal.Parse(value);
        }

        /// <summary>
        ///     读取页面参数(decimal型数据),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="def">默认值</param>
        /// <returns>decimal型数据,如果存在且不能转为decimal类型将出现异常</returns>
        protected decimal GetDecimalArg(string name, decimal def)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return def;
            }
            return decimal.Parse(value);
        }

        /// <summary>
        ///     读取页面参数(long型数据),如果参数为空或不存在,用默认值填充
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="def">默认值</param>
        /// <returns>long型数据,如果存在且不能转为long类型将出现异常</returns>
        protected long GetLongArg(string name, long def = -1)
        {
            var value = GetArgValue(name);
            if (string.IsNullOrEmpty(value) || value == "undefined" || value == "null")
            {
                return def;
            }
            return long.Parse(value);
        }

        #endregion

        #region 功能扩展

        /// <summary>
        /// 到HTML段落
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ToHtmlParagraph(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? "<p class='memo_html'></p>"
                : $"<p class='memo_html'>{value.Replace("\n", "</p><p class='memo_html'>")}</p>";
        }

        /// <summary>
        /// 到HTML段落
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ToHtml(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "" : value.Replace("\n", "<br\\>");
        }


        /// <summary>
        /// 到HTML段落
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ToHtmlDate(DateTime value)
        {
            return value == DateTime.MinValue ? "" : value.ToString("yyyy年M月d日");
        }
        /// <summary>
        /// 到HTML段落
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ToMoney(decimal value)
        {
            return value == 0M ? "" : value.ToChineseMoney();
        }

        #endregion
    }
}