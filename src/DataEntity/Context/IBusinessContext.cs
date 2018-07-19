namespace Gboxt.Common.DataModel
{
    /// <summary>
    /// 业务上下文
    /// </summary>
    public interface IBusinessContext
    {
        /// <summary>
        ///     最后一个的操作消息
        /// </summary>
        string LastMessage { get; set; }

        /// <summary>
        ///     最后一个的错误消息ID
        /// </summary>
        int LastError { get; set; }

        /// <summary>
        ///     是否工作在不安全模式下
        /// </summary>
        bool IsUnSafeMode { get; set; }

        /// <summary>
        ///     是否工作在系统模式下
        /// </summary>
        bool IsSystemMode { get; set; }

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