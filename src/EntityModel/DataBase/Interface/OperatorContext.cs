using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 操作上下文
    /// </summary>
    public class DbOperatorContext<TCommand> : IAsyncDisposable, IDisposable
        where TCommand : DbCommand
    {
        /// <summary>
        /// 其它范围
        /// </summary>
        public IDisposable Scope { get; set; }

        /// <summary>
        /// 命令对象
        /// </summary>
        public TCommand Command { get; set; }

        /// <summary>
        /// 是否自增SQL（需要回读）
        /// </summary>
        public bool IsIdentitySql { get; set; }


        bool IsDisposed;

        /// <summary>
        /// 析构
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;
            IsDisposed = true;
            Command?.Dispose();
            Scope?.Dispose();
        }
        /// <summary>
        ///     清理资源
        /// </summary>

        ValueTask IAsyncDisposable.DisposeAsync()
        {
            Dispose();
            return new ValueTask(Task.CompletedTask);
        }

    }
}