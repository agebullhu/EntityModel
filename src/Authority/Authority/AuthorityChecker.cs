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
using System.Web;
using System.Web.Security;
using Agebull.Common.DataModel;
using Agebull.Common.DataModel.Redis;
using Agebull.Common.Logging;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.SystemModel;
using Agebull.SystemAuthority.Organizations.BusinessLogic;

#endregion

namespace Agebull.SystemAuthority.Organizations
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
        void IPowerChecker.SavePageAction(int pageid, string name, string title, string action, string type)
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
            return BusinessContext.Current.IsSystemMode
                ? PageItemLogical.LoadPageButtons(page.Id)
                : RoleCache.LoadButtons(loginUser.RoleId, page.ID);
        }

        #endregion

        #region 角色权限

        /// <summary>
        /// 默认写死的权限
        /// </summary>
        private static readonly Dictionary<string, bool> defaults = new Dictionary<string, bool>
        {
            {"list",true },
            {"details",true },
            {"tree",true },
            {"eid",true }
        };

        /// <summary>
        ///     载入页面关联的按钮配置
        /// </summary>
        /// <param name="loginUser">登录用户</param>
        /// <param name="page">页面</param>
        /// <param name="action">动作</param>
        /// <returns>是否可执行页面动作</returns>
        bool IPowerChecker.CanDoAction(ILoginUser loginUser, IPageItem page, string action)
        {
            if (BusinessContext.Current.IsSystemMode)
                return true;

            if (defaults.ContainsKey(action))
                return defaults[action];

            return page != null && RoleCache.CanDoAction(loginUser.RoleId, page.Id, action);
        }

        /// <summary>
        ///     载入用户的角色权限
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        List<IRolePower> IPowerChecker.LoadUserPowers(ILoginUser loginUser)
        {
            return RoleCache.LoadUserPowers(loginUser.RoleId);
        }

        /// <summary>
        ///     载入用户的单页角色权限
        /// </summary>
        /// <param name="loginUser">登录用户</param>
        /// <param name="page">页面</param>
        /// <returns></returns>
        IRolePower IPowerChecker.LoadPagePower(ILoginUser loginUser, IPageItem page)
        {
            return page == null ? null : RoleCache.LoadPagePower(loginUser.RoleId, page.Id);
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
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(pwd))
            {
                LogRecorder.RecordLoginLog($@"用户名({userName})或密码{pwd}为空,来自{userHostAddress}");
                return Guid.Empty;
            }
            Guid token;
            int uid;
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                var pwd2 = proxy.Get(DataKeyBuilder.ToKey("user", userName, "pwd"));
                if (pwd2 == null || !string.Equals(pwd, pwd2))
                {
                    LogRecorder.RecordLoginLog($@"{userName}密码不对{pwd2}-[测试]{pwd},来自{userHostAddress}");
                    return Guid.Empty;
                }
                uid = proxy.GetValue<int>(DataKeyBuilder.ToKey("user", userName, "id"));
                if (uid == 0)
                {
                    LogRecorder.RecordLoginLog($@"{userName}用户不存在,来自{userHostAddress}");
                    return Guid.Empty;
                }
            }
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
                    UserId = uid,
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
            LoginUser lu = new LoginUser();
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                lu.User = proxy.GetEntity<UserData>(uid);
                if (lu.User == null)
                {
                    LogRecorder.RecordLoginLog("用户ID{0}无效", uid);
                    return LoginUser.Anymouse;
                }
                if (uid == 1)
                {
                    lu.Personnel = LoginUser.SystemUser.Personnel;
                    lu.Position = LoginUser.SystemUser.Position;
                    return lu;
                }
                lu.Personnel = proxy.GetEntity($"e:pp:{uid}", LoginUser.Anymouse.Personnel);
                if (lu.Personnel == LoginUser.Anymouse.Personnel)
                {
                    LogRecorder.RecordLoginLog("{0}({1})人员信息为空", lu.User.RealName, uid);
                }
                lu.Position = proxy.GetEntity(lu.Personnel.OrganizePositionId, LoginUser.Anymouse.Position);
                if (lu.Position == LoginUser.Anymouse.Position)
                {
                    LogRecorder.RecordLoginLog("{0}({1})职位信息为空", lu.User.RealName, uid);
                }
            }
            return lu;
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

    }
}