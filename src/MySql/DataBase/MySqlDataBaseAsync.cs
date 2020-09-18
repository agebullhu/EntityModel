// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    ///     表示MySql数据库对象
    /// </summary>
    partial class MySqlDataBase
    {
        #region 事务

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        public async Task<bool> BeginTransactionAsync()
        {
            if (Transaction != null)
                return false;
            await OpenAsync();
            Transaction = await _connection.BeginTransactionAsync();
            return true;
        }

        #endregion

        #region 数据库连接对象

        /// <summary>
        /// 初始化连接对象
        /// </summary>
        /// <returns></returns>
        private Task<MySqlConnection> InitConnectionAsync()
        {
            var connection = MySqlConnectionsManager.InitConnection(ConnectionStringName);

            return Task.FromResult(connection);
        }

        #endregion

        #region 连接

        /// <summary>
        ///     打开连接
        /// </summary>
        /// <returns>是否打开,是则为此时打开,否则为之前已打开</returns>
        public Task<bool> OpenAsync()
        {
            var res = Open();
            return Task.FromResult(res);
        }

        #endregion

        #region 数据库特殊操作

        /// <summary>
        ///     清除所有数据
        /// </summary>
        public async Task ClearAsync(string table)
        {
            await ExecuteAsync($@"TRUNCATE TABLE `{table}`;");
        }

        /// <summary>
        ///     清除所有数据
        /// </summary>
        public async Task ClearAllAsync()
        {
            var sql = new StringBuilder();
            foreach (var table in TableSql.Values)
            {
                await ClearAsync(table.TableName);
            }
            await ExecuteAsync(sql.ToString());
        }

        #endregion

        #region 内部方法

        /// <summary>
        ///     对连接执行 Transact-SQL 语句并返回受影响的行数。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>被影响的行数</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public async Task<int> ExecuteAsync(string sql, params DbParameter[] args)
        {
            using var scope = new ConnectionScope(this);
            using var cmd = CreateCommand(scope, sql, args);
            return await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>被影响的行数</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public async Task<int> ExecuteAsync(string sql, IEnumerable<DbParameter> args)
        {
            using var scope = new ConnectionScope(this);
            await using var cmd = CreateCommand(scope, sql, args);
            return await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        ///     执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        public async Task<object> ExecuteScalarAsync(string sql, params DbParameter[] args)
        {
            using var scope = new ConnectionScope(this);
            using var cmd = CreateCommand(scope, sql, args);
            var result = await cmd.ExecuteScalarAsync();
            return result == DBNull.Value ? null : result;
        }

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        public async Task<T> ExecuteScalarAsync<T>(string sql)
        {
            var result = await ExecuteScalarAsync(sql);
            return (T)result;
        }

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        public async Task<T> ExecuteScalarAsync<T>(string sql, params DbParameter[] args)
        {
            var result = args.Length == 0
                ? await ExecuteScalarAsync(sql)
                : await ExecuteScalarAsync(sql, args);
            return (T)result;
        }
        #endregion

    }

}