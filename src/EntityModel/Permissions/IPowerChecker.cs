// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System.Collections.Generic;

#endregion

namespace Agebull.EntityModel.Permissions
{
    /// <summary>
    ///     权限操作后期注入对象
    /// </summary>
    public interface IPowerChecker
    {
        /// <summary>
        ///     载入用户账号信息
        /// </summary>
        bool LoadAuthority(string page);

        /// <summary>
        /// 保存页面的动作
        /// </summary>
        void SavePageAction(int pageid, string name, string title, string action,string type);

        /// <summary>
        /// 重新载入用户信息
        /// </summary>
        void ReloadLoginUserInfo();

        /// <summary>
        ///     载入页面配置
        /// </summary>
        /// <param name="url">不包含域名的相对url</param>
        /// <returns>页面配置</returns>
        IPageItem LoadPageConfig(string url);

        /// <summary>
        ///     载入页面关联的按钮配置
        /// </summary>
        /// <param name="loginUser">登录用户</param>
        /// <param name="page">页面,不能为空</param>
        /// <returns>按钮配置集合</returns>
        List<string> LoadPageButtons(ILoginUserInfo loginUser, IPageItem page);

        /// <summary>
        ///     载入页面关联的按钮配置
        /// </summary>
        /// <param name="loginUser">登录用户</param>
        /// <param name="page">页面</param>
        /// <param name="action">动作</param>
        /// <returns>是否可执行页面动作</returns>
        bool CanDoAction(ILoginUserInfo loginUser, IPageItem page, string action);

        /// <summary>
        ///     载入用户的角色权限
        /// </summary>
        /// <param name="loginUser">登录用户</param>
        /// <returns></returns>
        List<IRolePower> LoadUserPowers(ILoginUserInfo loginUser);

        /// <summary>
        ///     载入用户的单页角色权限
        /// </summary>
        /// <param name="loginUser">登录用户</param>
        /// <param name="page">页面</param>
        /// <returns></returns>
        IRolePower LoadPagePower(ILoginUserInfo loginUser, IPageItem page);

        /// <summary>
        ///     保存用户的查询历史
        /// </summary>
        /// <param name="loginUser">登录用户</param>
        /// <param name="page">关联页面</param>
        /// <param name="args">查询参数</param>
        void SaveQueryHistory(ILoginUserInfo loginUser, IPageItem page, Dictionary<string, string> args);

        /// <summary>
        ///     读取用户的查询历史
        /// </summary>
        /// <param name="loginUser">登录用户</param>
        /// <param name="page">关联页面</param>
        /// <returns>返回的是参数字典的JSON格式的文本</returns>
        string LoadQueryHistory(ILoginUserInfo loginUser, IPageItem page);
    }

}