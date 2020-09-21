using System.Collections.Generic;
using System.Data.Common;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 参数生成器
    /// </summary>
    public interface ICommandCreater
    {
        /// <summary>
        ///     生成命令
        /// </summary>
        public DbCommand CreateCommand(IConnectionScope scope, params DbParameter[] args)
        {
            return CreateCommand(scope, null, args);
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        public DbCommand CreateCommand(IConnectionScope scope, string sql, DbParameter arg)
        {
            return CreateCommand(scope, sql, new[] { arg });
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        public DbCommand CreateCommand(IConnectionScope scope)
        {
            var cmd = scope.Connection.CreateCommand();

            if (scope.Transaction != null)
            {
                cmd.Transaction = scope.Transaction;
            }
            return cmd;
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        public DbCommand CreateCommand(IConnectionScope scope, string sql, IEnumerable<DbParameter> args = null);

    }

}