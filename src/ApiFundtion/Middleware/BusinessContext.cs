using Agebull.EntityModel.BusinessLogic;
using ZeroTeam.MessageMVC.Context;
using ZeroTeam.MessageMVC.ZeroApis;

namespace ZeroTeam.MessageMVC.ModelApi
{
    /// <summary>
    /// 业务上下文
    /// </summary>
    public class BusinessContext : IBusinessContext
    {
        /// <summary>
        ///     最后一个的操作消息
        /// </summary>
        public BusinessContext()
        {
            User = GlobalContext.User;
            Status = new ContextStatus();
        }

        /// <summary>
        ///     当前登录用户
        /// </summary>
        public IUser User { get; }

        /// <summary>
        /// 状态
        /// </summary>
        public ContextStatus Status { get; }

        /// <summary>
        ///     最后一个的操作消息
        /// </summary>
        public string LastMessage
        {
            get => Status.LastMessage;
            set => Status.LastMessage = value;
        }

        /// <summary>
        ///     最后操作的操作状态
        /// </summary>
        public int LastState
        {
            get => Status.LastState;
            set => Status.LastState = value;
        }

        /// <summary>
        ///     是否操作失败
        /// </summary>
        public bool IsFailed => Status.IsFailed;

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public string UserId => User?.UserId;

        /// <summary>
        /// 当前登录用户所在组织
        /// </summary>
        public string OrganizationId => User?.OrganizationId;

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public string NickName => User?.NickName;

        /// <summary>
        ///     正在排队
        /// </summary>
        public int Queue => OperatorStatusCode.Queue;

        /// <summary>
        ///     成功
        /// </summary>
        public int Success { get; }// 0;

        /// <summary>
        ///     参数错误
        /// </summary>
        public int ArgumentError => OperatorStatusCode.ArgumentError;//-1;

        /// <summary>
        ///     发生处理业务错误
        /// </summary>
        public int BusinessError => OperatorStatusCode.BusinessError;//-2;

        /// <summary>
        ///     发生未处理业务异常
        /// </summary>
        public int BusinessException => OperatorStatusCode.BusinessException;//-3;

        /// <summary>
        ///     发生未处理系统异常
        /// </summary>
        public int UnhandleException => OperatorStatusCode.UnhandleException;//-4;

        /// <summary>
        ///     网络错误
        /// </summary>
        public int NetworkError => OperatorStatusCode.NetworkError;//-5;

        /// <summary>
        ///     执行超时
        /// </summary>
        public int TimeOut => OperatorStatusCode.TimeOut;//-6;

        /// <summary>
        ///     拒绝访问
        /// </summary>
        public int DenyAccess => OperatorStatusCode.DenyAccess;//-7;

        /// <summary>
        ///     未知的令牌
        /// </summary>
        public int TokenUnknow => OperatorStatusCode.TokenUnknow;//-8;

        /// <summary>
        ///     令牌过期
        /// </summary>
        public int TokenTimeOut => OperatorStatusCode.TokenTimeOut;//-9;

        /// <summary>
        ///     系统未就绪
        /// </summary>
        public int NoReady => OperatorStatusCode.NoReady;//-0xA;

        /// <summary>
        ///     异常中止
        /// </summary>
        public int Ignore => OperatorStatusCode.Ignore;//-0xB;

        /// <summary>
        ///     重试
        /// </summary>
        public int ReTry => OperatorStatusCode.ReTry;//-0xC;

        /// <summary>
        ///     方法不存在
        /// </summary>
        public int NoFind => OperatorStatusCode.NoFind;//-0xD;

        /// <summary>
        ///     服务不可用
        /// </summary>
        public int Unavailable => OperatorStatusCode.Unavailable;//-0xE;

        /// <summary>
        ///     未知结果
        /// </summary>
        public int Unknow => OperatorStatusCode.Unknow;//0xF;
    }
}