// // /*****************************************************
// // (c)2016-2017 Copy right Agebull
// // 作者:
// // 工程:Agebull.SystemAuthority.Organizations
// // 建立:2016-06-12
// // 修改:2016-06-17
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using Agebull.Common.DataModel.Redis;
using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Agebull.ProjectDeveloper.WebDomain.Models;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.SystemModel;
using Gboxt.Common.SystemModel.DataAccess;

#endregion

namespace Agebull.Common.DataModel.SystemModel
{
    /// <summary>
    ///     权限检查对象
    /// </summary>
    public class AuthorityChecker : IPowerChecker
    {
        #region 用户信息

        /// <summary>
        ///     取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        public static Guid GetLoginUserInfo()
        {
            var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (string.IsNullOrEmpty(cookie?.Value))
            {
                return Guid.Empty;
            }
            try
            {
                var formsAuthentication = FormsAuthentication.Decrypt(cookie.Value);
                Guid token;
                return Guid.TryParse(formsAuthentication?.UserData, out token) ? token : Guid.Empty;
            }
            catch (Exception ex)
            {
                LogRecorder.Record(ex.ToString());
                return Guid.Empty;
            }
        }

        /// <summary>
        ///     取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        public static Guid GetMvcLoginUserInfo()
        {
            var paths = HttpContext.Current.Request.Url.AbsoluteUri.Split('\\');
            foreach (var p in paths)
            {
                Guid tooken;
                if (Guid.TryParse(p, out tooken))
                {
                    return tooken;
                }
            }
            return Guid.Empty;
        }

        /// <summary>
        /// 保存页面的动作
        /// </summary>
        void IPowerChecker.SavePageAction(long pageid, string name, string title, string action, string type)
        {
            PageItemLogical.SavePageAction(pageid, name, title, action, type);
        }

        /// <summary>
        ///     重新载入用户信息
        /// </summary>
        void IPowerChecker.ReloadLoginUserInfo()
        {
            ReloadLoginUserInfo(GetLoginUserInfo());
        }

        /// <summary>
        ///     重新载入用户信息
        /// </summary>
        public void ReloadLoginUserInfo(Guid token)
        {
            try
            {
                BusinessContext.Current.LoginUser = LoadUser(HttpContext.Current == null ? "::1" : HttpContext.Current.Request.UserHostAddress, token);
            }
            catch (Exception ex)
            {
                BusinessContext.Current.LoginUser = LoginUser.Anymouse;
                LogRecorder.Record(ex.ToString());
            }
        }

        #endregion

        #region 页面配置

        /// <summary>
        ///     载入页面配置
        /// </summary>
        /// <param name="url">不包含域名的相对url</param>
        /// <returns>页面配置</returns>
        public IPageItem LoadPageConfig(string url)
        {
            return PageItemLogical.GetPageItem(url);
        }

        /// <summary>
        ///     载入页面关联的按钮配置
        /// </summary>
        /// <param name="loginUser">登录用户</param>
        /// <param name="page">页面,不能为空</param>
        /// <returns>按钮配置集合</returns>
        List<string> IPowerChecker.LoadPageButtons(ILoginUser loginUser, IPageItem page)
        {
            if (page == null)
            {
                return new List<string>();
            }
            return PageItemLogical.LoadPageButtons(page.Id);
        }

        #endregion

        #region 角色权限

        /// <summary>
        ///     载入页面关联的按钮配置
        /// </summary>
        /// <param name="loginUser">登录用户</param>
        /// <param name="page">页面</param>
        /// <param name="action">动作</param>
        /// <returns>是否可执行页面动作</returns>
        bool IPowerChecker.CanDoAction(ILoginUser loginUser, IPageItem page, string action)
        {
            return true;
        }

        /// <summary>
        ///     载入用户的角色权限
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        List<IRolePower> IPowerChecker.LoadUserPowers(ILoginUser loginUser)
        {
            return new List<IRolePower>();
        }

        /// <summary>
        ///     载入用户的单页角色权限
        /// </summary>
        /// <param name="loginUser">登录用户</param>
        /// <param name="page">页面</param>
        /// <returns></returns>
        IRolePower IPowerChecker.LoadPagePower(ILoginUser loginUser, IPageItem page)
        {
            return null;
        }

        #endregion

        #region 查询历史

        /// <summary>
        ///     保存用户的查询历史
        /// </summary>
        /// <param name="loginUser">用户</param>
        /// <param name="page">关联页面</param>
        /// <param name="args">查询参数</param>
        void IPowerChecker.SaveQueryHistory(ILoginUser loginUser, IPageItem page, Dictionary<string, string> args)
        {
            if (page == null || loginUser == null)
            {
                return;
            }
            PageDataBusinessLogical.SaveQueryHistory(loginUser.Id, page.Id, args);
        }

        /// <summary>
        ///     读取用户的查询历史
        /// </summary>
        /// <param name="loginUser">用户</param>
        /// <param name="page">关联页面</param>
        /// <returns>返回的是参数字典的JSON格式的文本</returns>
        string IPowerChecker.LoadQueryHistory(ILoginUser loginUser, IPageItem page)
        {
            if (page == null || loginUser == null)
            {
                return null;
            }
            return PageDataBusinessLogical.LoadQueryHistory(loginUser.Id, page.Id);
        }

        #endregion


        #region 登录相关


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userHostAddress"></param>
        /// <param name="userName">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static Guid WebLogin(string userHostAddress, string userName, string pwd)
        {
            Guid token;
            using (var proxy = new RedisProxy(RedisProxy.DbAuthority))
            {
                var tk = DataKeyBuilder.ToKey("login", userName, "token");
                token = proxy.GetValue<Guid>(tk);
                if (token == Guid.Empty)
                {
                    token = Guid.NewGuid();
                    proxy.SetValue(tk, token);
                }
                proxy.Set(DataKeyBuilder.ToKey("login", token), new LoginToken
                {
                    UserId = 1,
                    Address = userHostAddress == "::1" ? "127.0.0.1" : userHostAddress,
                    Token = token,
                    LoginDateTime = DateTime.Now,
                    LastDateTime = DateTime.Now,
                    TimeOut = DateTime.Now.AddMinutes(30)
                });
            }
            BusinessContext.Current.Tooken = token;
            BusinessContext.Current.PowerChecker.ReloadLoginUserInfo(token);
            return token;
        }

        /// <summary>
        ///     载入用户
        /// </summary>
        /// <returns></returns>
        public static bool Logout(string userHostAddress, Guid token)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbAuthority))
            {
                var ik = DataKeyBuilder.ToKey("login", token);
                var info = proxy.Get(ik);
                if (info == null)
                {
                    return true;
                }
                var strs = info.Split(',');
                var ip = strs[1];
                if (ip != userHostAddress)
                {
                    return false;
                }
                proxy.Client.Remove(ik);
            }
            return true;
        }
        /// <summary>
        ///     载入用户
        /// </summary>
        /// <returns></returns>
        public static ILoginUser LoadUser(string userHostAddress, Guid token)
        {
            if (token == Guid.Empty)
                return LoginUser.Anymouse;
            if (userHostAddress == "::1")
                userHostAddress = "127.0.0.1";
            int uid = CheckToken(userHostAddress, token);
            if (uid == 0)
                return LoginUser.Anymouse;
            return LoadUserInfo(uid);
        }

        private static int CheckToken(string userHostAddress, Guid token)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbAuthority))
            {
                var ik = DataKeyBuilder.ToKey("login", token);
                var info = proxy.TryGet<LoginToken>(ik);
                if (info == null || info.TimeOut <= DateTime.Now)
                {
                    LogRecorder.RecordLoginLog("令牌{0}过期", token);
                    return 0;
                }
                if (info.Address != userHostAddress)
                {
                    LogRecorder.RecordLoginLog("IP【{0}】-【{1}】不相同", userHostAddress, info.Address);
                }
                info.TimeOut = DateTime.Now.AddMinutes(30);
                info.LastDateTime = DateTime.Now;
                proxy.Set(ik, info);
                return info.UserId;
            }
        }

        private static ILoginUser LoadUserInfo(int uid)
        {
            return new LoginUser
            {
                Id = 1,
            };
        }
        /// <summary>
        /// 用户登录的信息
        /// </summary>
        class LoginToken
        {
            /// <summary>
            /// 用户ID
            /// </summary>
            public int UserId { get; set; }

            /// <summary>
            /// 登录IP
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// 信息
            /// </summary>
            public Guid Token { get; set; }

            /// <summary>
            /// 登录时间
            /// </summary>
            public DateTime LoginDateTime { get; set; }

            /// <summary>
            /// 最后访问时间
            /// </summary>
            public DateTime LastDateTime { get; set; }

            /// <summary>
            /// 超时时间
            /// </summary>
            public DateTime TimeOut { get; set; }
        }
        #endregion


        #region 角色菜单

        /// <summary>
        ///     缓存角色的页面权限数据
        /// </summary>
        public void CacheRolePower()
        {
            using (SystemContextScope.CreateScope())
            {
                using (var proxy = new RedisProxy(RedisProxy.DbSystem))
                {
                    CreateMenu(proxy);
                }
            }
        }

        /// <summary>
        ///     载入角色菜单
        /// </summary>
        public static List<EasyUiTreeNode> LoadRoleMenu()
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return proxy.Get<List<EasyUiTreeNode>>(DataKeyBuilder.ToKey("auth", "menu", 0)) ?? CreateMenu(proxy);

            }
        }


        /// <summary>
        ///     载入角色菜单
        /// </summary>
        public static void CreateMenu()
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                CreateMenu(proxy);
            }
        }
        /// <summary>
        ///     生成角色菜单
        /// </summary>
        static List<EasyUiTreeNode> CreateMenu(RedisProxy proxy)
        {
            var root = new EasyUiTreeNode
            {
                ID = 0,
                IsOpen = true,
                Text = ConfigurationManager.AppSettings["ProjectName"],
                IsFolder = true
            };
            var access = new PageItemDataAccess();
            var pages = access.All(p => p.ItemType <= PageItemType.Page);
            foreach (var folder in pages.Where(p => p.ItemType == PageItemType.Root).OrderBy(p => p.Index))
            {
                var node = new EasyUiTreeNode
                {
                    IsOpen = true,
                    Text = folder.Caption,
                    Icon = "icon-item",
                    Attributes = "home.aspx",
                    Tag = folder.ExtendValue,
                    IsFolder = true
                };
                foreach (var page in pages.Where(p => p.ParentId == folder.Id && p.ItemType <= PageItemType.Page).OrderBy(p => p.Index))
                {
                    CreateRoleMenu(node, pages, page);
                }
                if (node.HaseChildren)
                {
                    root.Children.Add(node);
                }
            }
            var result = root.Children.OrderByDescending(p => p.ID).ToList( );
            proxy.Set(DataKeyBuilder.ToKey("auth", "menu", 0), result);
            return result;
        }

        /// <summary>
        ///     生成角色菜单
        /// </summary>
        static void CreateRoleMenu(EasyUiTreeNode parentNode, List<PageItemData> pages, PageItemData page)
        {
            var node = CreatePageNode(page);
            node.Attributes = page.Url;
            node.Tag = page.Url == "Folder" ? "folder" : "page";

            var array = pages.Where(p => p.ParentId == page.Id && p.ItemType <= PageItemType.Page && !p.Config.Hide).OrderBy(p => p.Index).ToArray();
            if (array.Length > 0)
                node.IsFolder = true;
            foreach (var ch in array)
            {
                CreateRoleMenu(node, pages, ch);
            }
            if (page.ItemType == PageItemType.Page || node.HaseChildren)
                parentNode.Children.Add(node);
        }

        private static EasyUiTreeNode CreatePageNode(PageItemData page)
        {
            var node = new EasyUiTreeNode
            {
                ID = page.Id,
                IsOpen = page.ItemType <= PageItemType.Folder,
                Tag = page.Url == "Folder" ? "folder" : "page",
                Text = page.Caption,
                Title = page.Name,
                IsFolder = page.ItemType <= PageItemType.Folder
            };
            if (!String.IsNullOrWhiteSpace(page.Icon))
                node.Icon = page.Icon;
            else
                switch (page.ItemType)
                {
                    case PageItemType.Folder:
                        node.Icon = "icon-tree-folder";
                        break;
                    case PageItemType.Page:
                        node.Icon = "icon-tree-page";
                        break;
                }
            return node;
        }
        #endregion
    }
}