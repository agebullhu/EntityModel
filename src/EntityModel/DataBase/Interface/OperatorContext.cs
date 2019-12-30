using System;
using System.Data.Common;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 操作上下文
    /// </summary>
    public class DbOperatorContext<TCommand> : IDisposable
        where TCommand : DbCommand
    {
        /// <summary>
        /// 命令对象
        /// </summary>
        public TCommand Command { get; set; }

        /// <summary>
        /// 是否自增SQL（需要回读）
        /// </summary>
        public bool IsIdentitySql { get; set; }
        /// <summary>
        /// 析构
        /// </summary>
        public void Dispose()
        {
            Command.Dispose();
        }
    }
}