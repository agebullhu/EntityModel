namespace Gboxt.Common.DataModel
{
    /// <summary>
    /// 业务上下文
    /// </summary>
    public interface IBusinessContext
    {
        /// <summary>
        ///     最后状态(当前时间)
        /// </summary>
        IOperatorStatus LastStatus { get; set; }

        /// <summary>
        ///     当前登录的用户ID
        /// </summary>
        long LoginUserId { get; }

        /// <summary>
        /// 用户令牌
        /// </summary>
        string UserToken { get; set; }

    }
}