using System;
using System.Web;
using System.Web.Security;

namespace Agebull.SystemAuthority.Organizations
{
    /// <summary>
    /// 基于组织构架的行级权限的页面辅助类
    /// </summary>
    public static class PageHelper
    {
        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public static void Logout(HttpRequest request,HttpResponse response)
        {
            AuthorityChecker.Logout(request.UserHostAddress, AuthorityChecker.GetLoginUserInfo());
            response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, string.Empty));
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public static string Login(HttpRequest request, HttpResponse response)
        {
            var name = request["UserName"];
            var pwd = request["PassWord"];
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd))
            {
                return ("用户名或密码不正确");
            }
            var token = AuthorityChecker.WebLogin(request.UserHostAddress, name.Trim(), pwd.Trim());
            if (token == Guid.Empty)
            {
                return ("用户名或密码不正确");

            }
            //创造票据
            var ticket = new FormsAuthenticationTicket(1, "tokey", DateTime.Now, DateTime.Now.AddHours(1), false, token.ToString());
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            //输出到客户端
            request.Cookies.Add(cookie);
            response.Cookies.Add(cookie);
            return null;
        }
    }
}
