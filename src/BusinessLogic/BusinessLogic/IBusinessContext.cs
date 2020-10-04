namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// 业务上下文
    /// </summary>
    public interface IBusinessContext
    {

        /// <summary>
        ///     最后一个的操作消息
        /// </summary>
        string LastMessage
        {
            get;
            set;
        }

        /// <summary>
        ///     最后操作的操作状态
        /// </summary>
        int LastState
        {
            get;
            set;
        }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        string NickName { get; }

        /// <summary>
        ///     是否操作失败
        /// </summary>
        bool IsFailed { get; }

        /// <summary>
        ///     成功
        /// </summary>
        int Success { get; }// 0;

        /// <summary>
        ///     参数错误
        /// </summary>
        int ArgumentError { get; }// /*ArgumentError*/-1;

        /// <summary>
        ///     发生处理业务错误
        /// </summary>
        int BusinessError { get; }// /*BusinessError*/-2;

        /// <summary>
        ///     发生未处理业务异常
        /// </summary>
        int BusinessException { get; }// /*BusinessException*/-3;

        /// <summary>
        ///     发生未处理系统异常
        /// </summary>
        int UnhandleException { get; }// /*UnhandleException*/-4;

        /// <summary>
        ///     网络错误
        /// </summary>
        int NetworkError { get; }// /*NetworkError*/-5;

        /// <summary>
        ///     执行超时
        /// </summary>
        int TimeOut { get; }// /*TimeOut*/-6;

        /// <summary>
        ///     拒绝访问
        /// </summary>
        int DenyAccess { get; }// /*DenyAccess*/-7;

        /// <summary>
        ///     未知的令牌
        /// </summary>
        int TokenUnknow { get; }// /*TokenUnknow*/-8;

        /// <summary>
        ///     令牌过期
        /// </summary>
        int TokenTimeOut { get; }// /*TokenTimeOut*/-9;

        /// <summary>
        ///     系统未就绪
        /// </summary>
        int NoReady { get; }// /*NoReady*/-0xA;

        /// <summary>
        ///     异常中止
        /// </summary>
        int Ignore { get; }// /*Ignore*/-0xB;

        /// <summary>
        ///     重试
        /// </summary>
        int ReTry { get; }// /*ReTry*/-0xC;

        /// <summary>
        ///     方法不存在
        /// </summary>
        int NoFind { get; }// /*NoFind*/-0xD;

        /// <summary>
        ///     服务不可用
        /// </summary>
        int Unavailable { get; }// /*Unavailable*/-0xE;

        /// <summary>
        ///     未知结果
        /// </summary>
        int Unknow { get; }// /*Unknow*/0xF;
    }
}