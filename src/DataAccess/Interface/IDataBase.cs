// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     表示数据库对象
    /// </summary>
    public interface IDataBase : ICommandCreater, IConfig, IAsyncDisposable
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        DataBaseType DataBaseType
        {
            get;
        }

        #region 事务及连接范围

        /// <summary>
        /// 是否锁定连接对象(更新插入删除发生后自动启用)
        /// </summary>
        bool IsLockConnection { get; }

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

        #region 连接

        /// <summary>
        /// 连接字符串配置节点名称,用于取出
        /// </summary>
        string ConnectionStringName { get; }

        /// <summary>
        ///     打开连接
        /// </summary>
        /// <returns>是否打开,是则为此时打开,否则为之前已打开</returns>
        Task<bool> OpenAsync();

        #endregion

        /// <summary>
        /// 构造连接范围对象
        /// </summary>
        /// <returns></returns>
        Task<IConnectionScope> CreateConnectionScope();

        /// <summary>
        ///     对连接执行 Transact-SQL 语句并返回受影响的行数。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>被影响的行数</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        Task<int> ExecuteAsync(string sql, params DbParameter[] args);

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>被影响的行数</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        Task<int> ExecuteAsync(string sql, IEnumerable<DbParameter> args);

        /// <summary>
        ///     执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        Task<object> ExecuteScalarAsync(string sql, params DbParameter[] args);

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        Task<T> ExecuteScalarAsync<T>(string sql);

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        Task<T> ExecuteScalarAsync<T>(string sql, params DbParameter[] args);


        /// <summary>
        ///     记录SQL日志
        /// </summary>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        void TraceSql(DbCommand cmd);

        /// <summary>
        ///     记录SQL日志
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        void TraceSql(string sql, IEnumerable<DbParameter> args);


    }
}