namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    /// ZeroApi控制器基类
    /// </summary>
    public abstract class ModelApi
    {
        /// <summary>
        /// 构造
        /// </summary>
        protected ModelApiController()
        {
            GlobalContext.Current.Status.LastState = OperatorStatusCode.Success;
        }

        #region 基本属性

        /// <summary>
        /// 原始调用帧消息
        /// </summary>
        public IInlineMessage Message => GlobalContext.Current.Message;

        #endregion

        #region 状态

        /// <summary>
        ///     是否操作失败
        /// </summary>
        protected internal bool IsFailed => GlobalContext.Current.Status.LastState != OperatorStatusCode.Success;

        /// <summary>
        ///     设置当前操作失败
        /// </summary>
        /// <param name="message"></param>
        protected internal void SetFailed(string message)
        {
            GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
            GlobalContext.Current.Status.LastMessage = message;
        }

        #endregion

        #region 权限相关

        /// <summary>
        ///     当前用户是否已登录成功
        /// </summary>
        protected internal bool UserIsLogin => !string.IsNullOrEmpty(UserInfo.UserId);

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

        #endregion

    }
}