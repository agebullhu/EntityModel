using Gboxt.Common.DataModel;

namespace Agebull.Common
{
    /// <summary>
    ///     全局上下文
    /// </summary>
    public interface IGlobalContext
    {
        /// <summary>
        ///     最后状态(当前时间)
        /// </summary>
        IOperatorStatus LastStatus { get; }

        /// <summary>
        ///     当前登录的用户ID
        /// </summary>
        long LoginUserId { get; }

        /// <summary>
        ///     令牌
        /// </summary>
        string Token { get; }
    }
}