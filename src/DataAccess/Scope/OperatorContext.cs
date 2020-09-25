using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 操作上下文
    /// </summary>
    public class DbOperatorContext<TCommand> : IAsyncDisposable
        where TCommand : DbCommand
    {
        /// <summary>
        /// 其它范围
        /// </summary>
        public IConnectionScope ConnectionScope { get; set; }

        /// <summary>
        /// 命令对象
        /// </summary>
        public TCommand Command { get; set; }

        bool IsDisposed;

        /// <summary>
        /// 析构
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;
            IsDisposed = true;
            _= Command.DisposeAsync();
            if (ConnectionScope != null)
                _ = ConnectionScope.DisposeAsync();
        }

        /// <summary>
        ///     清理资源
        /// </summary>

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (IsDisposed)
                return;
            IsDisposed = true;
            await Command.DisposeAsync();
            if (ConnectionScope != null)
                await ConnectionScope.DisposeAsync();
        }
    }
}