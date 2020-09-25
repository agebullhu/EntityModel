
using System;
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
        DbCommand CreateCommand(string sql, params DbParameter[] args);

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

        #region 执行

        /// <summary>
        ///     对连接执行SQL 语句并返回受影响的行数。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>被影响的行数</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        Task<int> ExecuteAsync(string sql, params DbParameter[] args);

        /// <summary>
        ///     执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        Task<(bool hase, object value)> ExecuteScalarAsync(string sql, params DbParameter[] args);
        #endregion
    }
}