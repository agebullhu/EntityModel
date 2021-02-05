
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Agebull.EntityModel.Common
{

    /// <summary>
    /// 数据库连接范围
    /// </summary>
    public interface IConnectionScope : IAsyncDisposable
    {
        /// <summary>
        /// 连接对象
        /// </summary>
        DbConnection Connection { get; }

        /// <summary>
        ///     生成命令
        /// </summary>
        DbCommand CreateCommand(string sql, IEnumerable<DbParameter> args = null);

        #region 事务

        /// <summary>
        ///     事务对象
        /// </summary>
        DbTransaction Transaction { get; }

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        Task<bool> BeginTransaction();

        /// <summary>
        /// 回滚事务
        /// </summary>
        Task Rollback();

        /// <summary>
        /// 提交事务
        /// </summary>
        Task Commit();
        #endregion

    }
}