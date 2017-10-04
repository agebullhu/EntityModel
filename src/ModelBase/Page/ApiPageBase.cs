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
using Agebull.Common;
using Agebull.Common.Logging;
using Gboxt.Common.DataModel;
using Newtonsoft.Json;

#endregion

namespace Gboxt.Common.WebUI
{
    /// <summary>
    ///     AJAX访问的API页面的基类
    /// </summary>
    public abstract class ApiPageBase : MyPageBase
    {
        #region 权限检查


        /// <summary>
        ///     检查登录情况
        /// </summary>
        protected override bool CheckLogined()
        {
            if (UserIsLogin)
                return true;
            IsFailed = true;
            Message = "登录超时";
            State = 1;
            return false;
        }

        private readonly HashSet<string> _commonActions = new HashSet<string> { "eid", "list", "details", "tree" };

        /// <summary>
        /// 注册公共动作(不需要进一步检查权限的动作)
        /// </summary>
        /// <param name="action"></param>
        protected void RegisteCommonAction(params string[] action)
        {
            action.Foreach(p => _commonActions.Add(p));
        }

        private readonly HashSet<string> _denyActions = new HashSet<string>();

        /// <summary>
        /// 注册禁止动作(不需要进一步检查权限的动作)
        /// </summary>
        /// <param name="action"></param>
        protected void RegisteDenyAction(params string[] action)
        {
            action.Foreach(p => _denyActions.Add(p));
        }
        /// <summary>
        ///     当前页面的所有按钮
        /// </summary>
        protected bool CanDoAction(string action)
        {
#if NoAuthority
            return true;
#else
            if (AllAction)
                return !_denyActions.Contains(action);
            return _commonActions.Contains(action)
                   || BusinessContext.Current.PowerChecker.CanDoAction(LoginUser, PageItem, action)
                   || !_denyActions.Contains(action);
#endif 
        }
        /// <summary>
        ///     检查动作是否允许
        /// </summary>
        protected override bool CheckCanDo()
        {
            if (CanDoAction(_action))
            {
                LogRecorder.RecordLoginLog("用户{0}({3})访问{1}的的动作{2}", LoginUser.RealName, Request.Url, _action, LoginUser.Id);
                return true;
            }
            LogRecorder.RecordLoginLog("用户{0}({3})访问{1}的动作{2}时没有权限", LoginUser.RealName, Request.Url, _action, LoginUser.Id);
            IsFailed = true;
            Message = "非法访问";
            State = 2;
            return false;
        }

        #endregion
        #region 准备与校验


        /// <summary>
        ///     页面处理前准备
        /// </summary>
        protected override void OnPrepare()
        {
            _action = Request["action"];
            if (string.IsNullOrEmpty(_action))
            {
                return;
            }
            _action = _action.ToLower();
            if (!IsPublicPage)
            {
                BusinessContext.Current.CurrentPagePower = PagePower;
                BusinessContext.Current.PowerChecker.SavePageAction(PageItem.Id, _action);
            }
            //LogRecorder.MonitorTrace("Headers:" + this.Request.Headers);
            //LogRecorder.MonitorTrace("Form:" + this.Request.Form);
            //LogRecorder.MonitorTrace("QueryString:" + this.Request.QueryString);
            //LogRecorder.MonitorTrace("Cookies:" + this.Request.Cookies);
            LogRecorder.MonitorTrace("Action:" + _action);
        }
        #endregion

        #region API操作

        /// <summary>
        /// 当前API的动作请求
        /// </summary>
        private string _action;

        /// <summary>
        ///     页面操作处理入口
        /// </summary>
        protected override void OnPageLoaded()
        {
            IsFailed = false;
            Message = null;
            State = 0;
            try
            {
                DoActin(_action);
            }
            catch (AgebullBusinessException exception)
            {
                IsFailed = true;
                Message = exception.Message;
            }
            catch (Exception exception)
            {
                IsFailed = true;
                State = 3;
                Message = "*--系统内部错误--*";
                Debug.WriteLine(exception);
                LogRecorder.Error(exception.StackTrace);
                LogRecorder.Error(exception.ToString());
                LogRecorder.MonitorTrace("Exception:"+ exception.Message);
            }
        }
        /// <summary>
        ///     执行操作
        /// </summary>
        /// <param name="action">传入的动作参数,如为单个操作可忽略</param>
        protected abstract void DoActin(string action);
        #endregion


        #region 标准返回值
        /// <summary>
        ///     页面执行出错的处理
        /// </summary>
        protected override void OnFailed(Exception exception)
        {
            SetFailed("*-*系统内部错误*-*");
        }

        /// <summary>
        ///     页面处理结束
        /// </summary>
        protected override void OnResult()
        {
            Response.Clear();
            try
            {
                if (string.IsNullOrWhiteSpace(CustomJson))
                {
#if NewJson
                    var msg = string.IsNullOrEmpty(Message)
                        ? (IsFailed ? BusinessContext.Current.LastMessage ?? "操作失败" : "操作成功")
                        : Message.Replace('\"', '\'');
                    if (AjaxResult == null)
                    {
                        AjaxResult = new AjaxResult
                        {
                            State = State,
                            Succeed = !IsFailed,
                            Message = msg,
                            Message2 = Message2
                        };
                    }
                    else
                    {
                        AjaxResult.Message = msg;
                        AjaxResult.Message2 = Message2;
                        AjaxResult.Succeed = !IsFailed;
                        AjaxResult.State = State;
                    }
                    CustomJson = JsonConvert.SerializeObject(AjaxResult);
#else
                var json = new StringBuilder();
                json.AppendFormat(@"{{""succeed"":{0}", this.IsFailed ? "false" : "true");
                json.AppendFormat(@",""message"":""{0}""",
                    string.IsNullOrEmpty(this.Message)
                        ? (this.IsFailed ? "操作失败" : "操作成功")
                        : this.Message.Replace('\"', '\''));
                if (!string.IsNullOrWhiteSpace(this.ResultData))
                {
                    json.AppendFormat(@",{0}", this.ResultData);
                }
                json.Append('}');
                this.CustomJson = json.ToString();
#endif
                }
                Response.Write(CustomJson);
            }
            catch (Exception exception)
            {
                LogRecorder.Error(exception.ToString());
                Debug.WriteLine(exception);
                Response.Write(@"{""succeed"":false,""message"":""***系统内部错误**""}");
            }
            LogRecorder.MonitorTrace(CustomJson);
        }
#if NewJson

        /// <summary>
        ///     使用类定义的AJAX返回值
        /// </summary>
        protected AjaxResult AjaxResult;
#else
        /// <summary>
        ///     返回数据的JSON表示
        /// </summary>
        protected string ResultData { get; set; }
#endif

        /// <summary>
        ///     结果状态
        /// </summary>
        public int State { get; set; } = 2;

        /// <summary>
        ///     是否操作失败
        /// </summary>
        protected bool IsFailed { get; set; } = true;

        /// <summary>
        ///     操作消息
        /// </summary>
        protected string Message { get; set; }

        /// <summary>
        ///     操作消息
        /// </summary>
        protected string Message2 { get; set; }


        /// <summary>
        ///     设置当前操作失败
        /// </summary>
        /// <param name="message"></param>
        protected void SetFailed(string message)
        {
            IsFailed = true;
            Message = message;
        }

        /// <summary>
        ///     设置返回值为DataGrid需要的数据
        /// </summary>
        /// <typeparam name="T">数据类</typeparam>
        /// <param name="dataList">数据</param>
        protected void SetDataGirdResult<T>(IList<T> dataList) where T : class
        {
#if NewJson
            AjaxResult = new EasyUiGridData<T>
            {
                Total = dataList?.Count ?? 0,
                Data = dataList
            };
#else
            var data = new EasyUiGridData<T>
            {
                Total = dataList == null ? 0 : dataList.Count,
                Data = dataList
            };
            var json = JsonConvert.SerializeObject(data);
            this.ResultData = json.Substring(1, json.Length - 2);
#endif
        }

        /// <summary>
        ///     设置返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        protected void SetResultData<T>(T data)
        {
            if (Equals(data, default(T)))
            {
                return;
            }
#if NewJson
            AjaxResult = new AjaxResult<T>
            {
                Value = data
            };
#else

            if (typeof (T) == typeof (string))
            {
                this.ResultData = string.Format("\"value\":\"{0}\"", data.ToString().Replace('\"', '\''));
            }
            else if (data is Array || data is IEnumerable)
            {
                this.ResultData = string.Format("\"value\":{0}", JsonConvert.SerializeObject(data));
            }
            else
            {
                var json = JsonConvert.SerializeObject(data);
                this.ResultData = json.Substring(1, json.Length - 2);
            }
#endif
        }

        #endregion

        #region 自定义返回值

        /// <summary>
        ///     设置自定义返回的JSON值(带状态的,内容是value字段,这与标准返回是一样的)
        /// </summary>
        /// <param name="value"></param>
        protected void SetCustomJsonValue(string value)
        {
            CustomJson = $@"{{""state"":0,""succeed"":true,""message"":null,""value"":{value}}}"; ;
        }


        /// <summary>
        ///     自定义返回的JSON值,如果不为空,直接返回它,无状态
        /// </summary>
        protected string CustomJson { get; set; }

        /// <summary>
        ///     设置自定义返回的JSON值,无状态
        /// </summary>
        /// <typeparam name="TIn">传入数据</typeparam>
        /// <typeparam name="TOut">输出数据</typeparam>
        /// <param name="datas"></param>
        /// <param name="convert"></param>
        /// <param name="nullValue"></param>
        protected void SetCustomJsonResult<TIn, TOut>(IEnumerable<TIn> datas, Func<TIn, TOut> convert,
            string nullValue = "[]")
            where TIn : class
            where TOut : class
        {
            if (datas == null)
            {
                CustomJson = nullValue;
                return;
            }
            var outs = datas.Select(convert).ToList();
            CustomJson = outs.Count == 0 ? nullValue : JsonConvert.SerializeObject(outs);
        }

        /// <summary>
        ///     设置自定义返回的JSON值,无状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="nullValue"></param>
        protected void SetCustomJsonResult<T>(T data, string nullValue = "[]") where T : class
        {
            CustomJson = data == null ? nullValue : JsonConvert.SerializeObject(data);
        }

        /// <summary>
        ///     设置自定义返回的JSON值,无状态
        /// </summary>
        /// <param name="value"></param>
        protected void SetCustomJsonResult(string value)
        {
            CustomJson = value;
        }
        /// <summary>
        ///     设置空的自定义返回的JSON值
        /// </summary>
        /// <param name="isArray"></param>
        protected void SetEmptyCustomJsonResult(bool isArray = true)
        {
            CustomJson = isArray ? "[]" : "{}";
        }

        #endregion
    }
}