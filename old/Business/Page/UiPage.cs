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
using Gboxt.Common.DataModel;

#endregion

namespace Gboxt.Common.WebUI
{
    /// <summary>
    ///     UI呈现页面的基类
    /// </summary>
    public class UiPage : MyPageBase
    {
        /// <summary>
        ///     用在界面上的当前用户可以访问的按钮集合
        /// </summary>
        private string _uibuttons;

        /// <summary>
        ///     用在界面上的当前用户可以访问的按钮集合
        /// </summary>
        protected string UiButtons
        {
            get
            {
                if (_uibuttons != null)
                {
                    return _uibuttons;
                }
                var buttons = BusinessContext.Current.PowerChecker.LoadPageButtons(BusinessContext.Current.LoginUser, PageItem);
                return _uibuttons = buttons == null
                    ? ""
                    : "'" + string.Join("','", buttons) + "'";
            }
        }

        /// <summary>
        ///     本地路径
        /// </summary>
        protected string Path
        {
            get
            {
                var path = Request.Url.AbsolutePath;
                var idx = path.LastIndexOf('/');
                return idx > 0 ? path.Substring(0, idx) : "";
            }
        }

        /// <summary>
        ///     上次用户的查询历史的JSON文本
        /// </summary>
        protected string PreQueryArgs
        {
            get
            {
                var preargs = BusinessContext.Current.PowerChecker.LoadQueryHistory(LoginUser, PageItem);
                return string.IsNullOrWhiteSpace(preargs) ? "{}" : preargs;
            }
        }

        /// <summary>
        ///     页面处理前准备
        /// </summary>
        protected override void OnPrepare()
        {

        }

        /// <summary>
        ///     页面载入后的处理
        /// </summary>
        protected override void OnPageLoaded()
        {
        }

        /// <summary>
        ///     页面处理结束
        /// </summary>
        protected override void OnResult()
        {
            //if (!UserIsLogin)
            //    Response.Redirect("/login.htm");
            //else if (PagePower == null)
            //    Response.Redirect("/nosupper.htm");
        }
    }

    /// <summary>
    ///     公共页面
    /// </summary>
    public class PublishPage : MyPageBase
    {
        public PublishPage()
        {
            IsPublicPage = true;
        }
        /// <summary>
        ///     本地路径
        /// </summary>
        protected string Path
        {
            get
            {
                var path = Request.Url.AbsolutePath;
                var idx = path.LastIndexOf('/');
                return idx > 0 ? path.Substring(0, idx) : "";
            }
        }

        /// <summary>
        ///     上次用户的查询历史的JSON文本
        /// </summary>
        protected string PreQueryArgs
        {
            get
            {
                var preargs = BusinessContext.Current.PowerChecker.LoadQueryHistory(LoginUser, PageItem);
                return string.IsNullOrWhiteSpace(preargs) ? "{}" : preargs;
            }
        }

        /// <summary>
        ///     页面处理前准备
        /// </summary>
        protected override void OnPrepare()
        {

        }

        /// <summary>
        ///     页面载入后的处理
        /// </summary>
        protected override void OnPageLoaded()
        {
        }

        /// <summary>
        ///     页面处理结束
        /// </summary>
        protected override void OnResult()
        {
            //if (!UserIsLogin)
            //    Response.Redirect("/login.htm");
        }

        public static string FormatDateTime(DateTime date, string fmt)
        {
            return date == DateTime.MinValue ? "" : date.ToString(fmt);
        }
    }
}