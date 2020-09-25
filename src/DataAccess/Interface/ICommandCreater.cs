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
        DbCommand CreateCommand(IConnectionScope scope);

        /// <summary>
        ///     生成命令
        /// </summary>
        DbCommand CreateCommand(IConnectionScope scope, string sql, params DbParameter[] args);

    }

}