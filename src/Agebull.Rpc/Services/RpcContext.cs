using System;

namespace Agebull.Zmq.Rpc
{
    /// <summary>
    /// 远程调用上下文
    /// </summary>
    public class RpcContext
    {
        [ThreadStatic]
        internal static RpcContext _current;

        /// <summary>
        /// 当前线程单例
        /// </summary>
        public static RpcContext Current => _current ?? (_current = new RpcContext());

        /// <summary>
        /// 当前原始参数
        /// </summary>
        public RpcArgument Argument { get; set; }

        /// <summary>
        /// 当前原始参数
        /// </summary>
        public CommandArgument Command { get; set; }

        /// <summary>
        /// 当前请求的用户信息
        /// </summary>
        public UserContext UserInfo => Argument?.UserInfo;
    }

    /// <summary>
    /// 远程请求上下文范围
    /// </summary>
    public class RpcContextScope : IDisposable
    {
        private RpcContext _preContext;

        /// <summary>
        /// 构造一个当前请求的上下文范围
        /// </summary>
        /// <param name="command"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static RpcContextScope CreateScope(CommandArgument command, RpcArgument argument)
        {
            return new RpcContextScope(command, argument);
        }
        RpcContextScope(CommandArgument command, RpcArgument argument)
        {
            _preContext = RpcContext._current;
            RpcContext._current = new RpcContext
            {
                Command = command,
                Argument = argument
            };
        }

        // 添加此代码以正确实现可处置模式。
        void IDisposable.Dispose()
        {
            if (_preContext != null)
                RpcContext._current = _preContext;
            _preContext = null;
        }
    }

}
