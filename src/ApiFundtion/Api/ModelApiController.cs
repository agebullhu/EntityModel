using ZeroTeam.MessageMVC.Context;
using ZeroTeam.MessageMVC.ZeroApis;

namespace ZeroTeam.MessageMVC.ModelApi
{
    /// <summary>
    /// ZeroApi控制器基类
    /// </summary>
    public abstract class ModelApiController : IApiController
    {
        /// <summary>
        /// 构造
        /// </summary>
        protected ModelApiController()
        {
            GlobalContext.Current.Status.LastState = OperatorStatusCode.Success;
        }

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
    }
}