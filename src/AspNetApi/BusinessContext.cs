using Agebull.EntityModel.BusinessLogic;

namespace ZeroTeam.AspNet.ModelApi
{
    /// <summary>
    /// 业务上下文
    /// </summary>
    internal class BusinessContext : IBusinessContext
    {
        /// <summary>
        ///     最后一个的操作消息
        /// </summary>
        public string LastMessage
        {
            get;
            set;
        }

        /// <summary>
        ///     最后操作的操作状态
        /// </summary>
        public int LastState
        {
            get;
            set;
        }

        /// <summary>
        ///     是否操作失败
        /// </summary>
        public bool IsFailed => LastState != Success;



        /// <summary>
        /// 当前登录用户
        /// </summary>
        public string UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public string NickName
        {
            get;
            set;
        }


        /// <summary>
        ///     成功
        /// </summary>
        public int Success => 0;

        /// <summary>
        ///     参数错误
        /// </summary>
        public int ArgumentError => /*Business.Context.ArgumentError*/-1;

        /// <summary>
        ///     发生处理业务错误
        /// </summary>
        public int BusinessError => /*Business.Context.BusinessError*/-2;

        /// <summary>
        ///     发生未处理业务异常
        /// </summary>
        public int BusinessException => /*Business.Context.BusinessException*/-3;

        /// <summary>
        ///     发生未处理系统异常
        /// </summary>
        public int UnhandleException => /*Business.Context.UnhandleException*/-4;

        /// <summary>
        ///     网络错误
        /// </summary>
        public int NetworkError => /*Business.Context.NetworkError*/-5;

        /// <summary>
        ///     执行超时
        /// </summary>
        public int TimeOut => /*Business.Context.TimeOut*/-6;

        /// <summary>
        ///     拒绝访问
        /// </summary>
        public int DenyAccess => /*Business.Context.DenyAccess*/-7;

        /// <summary>
        ///     未知的令牌
        /// </summary>
        public int TokenUnknow => /*Business.Context.TokenUnknow*/-8;

        /// <summary>
        ///     令牌过期
        /// </summary>
        public int TokenTimeOut => /*Business.Context.TokenTimeOut*/-9;

        /// <summary>
        ///     系统未就绪
        /// </summary>
        public int NoReady => /*Business.Context.NoReady*/-0xA;

        /// <summary>
        ///     异常中止
        /// </summary>
        public int Ignore => /*Business.Context.Ignore*/-0xB;

        /// <summary>
        ///     重试
        /// </summary>
        public int ReTry => /*Business.Context.ReTry*/-0xC;

        /// <summary>
        ///     方法不存在
        /// </summary>
        public int NoFind => /*Business.Context.NoFind*/-0xD;

        /// <summary>
        ///     服务不可用
        /// </summary>
        public int Unavailable => /*Business.Context.Unavailable*/-0xE;

        /// <summary>
        ///     未知结果
        /// </summary>
        public int Unknow => /*Business.Context.Unknow*/0xF;
    }
}