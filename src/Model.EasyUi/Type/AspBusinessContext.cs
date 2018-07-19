// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-24
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Web;
using Agebull.Common.Ioc;
using Gboxt.Common.SystemModel;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     为业务处理上下文对象
    /// </summary>
    public class AspBusinessContext : BusinessContext
    {
        /// <summary>
        /// 请求对象
        /// </summary>
        public HttpRequest Request { get; set; }

        /// <summary>
        ///     当前页面节点配置
        /// </summary>
        public IPageItem PageItem { get; set; }

        /// <summary>
        ///     权限校验对象
        /// </summary>
        private IPowerChecker _powerChecker;


        /// <summary>
        ///     权限校验对象
        /// </summary>
        public IPowerChecker PowerChecker => _powerChecker ?? (_powerChecker = IocHelper.Create<IPowerChecker>());

        /// <summary>
        ///     用户的角色权限
        /// </summary>
        private List<IRolePower> _powers;

        /// <summary>
        ///     用户的角色权限
        /// </summary>
        public List<IRolePower> Powers => _powers ?? (_powers = PowerChecker.LoadUserPowers(LoginUser));

        /// <summary>
        /// 当前页面权限设置
        /// </summary>
        public IRolePower CurrentPagePower
        {
            get;
            set;
        }
        /// <summary>
        /// 在当前页面检查是否可以执行操作
        /// </summary>
        /// <param name="action">操作</param>
        /// <returns></returns>
        public bool CanDoCurrentPageAction(string action)
        {
            return PowerChecker == null || PowerChecker.CanDoAction(LoginUser, PageItem, action);
        }

        /// <summary>
        /// 用户令牌
        /// </summary>
        public Guid Token { get; set; }

        /// <summary>
        /// 用户令牌是否保存在COOKIE中;
        /// </summary>
        public bool WorkByCookie { get; set; }
    }
}