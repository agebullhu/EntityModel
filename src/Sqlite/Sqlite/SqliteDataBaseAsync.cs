// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Agebull.EntityModel.Common;
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Agebull.EntityModel.Sqlite
{
    /// <summary>
    ///     表示Sqlite数据库对象
    /// </summary>
    partial class SqliteDataBase
    {
        #region 事务

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        Task<bool> IDataBase.BeginTransactionAsync()
        {
            if (Transaction != null)
                return Task.FromResult(false);
            Transaction = Connection.BeginTransaction();
            return Task.FromResult(true);
        }

        #endregion

        #region 数据库连接对象

        /// <summary>
        /// 初始化连接对象
        /// </summary>
        /// <returns></returns>
        private async Task<SqliteConnection> InitConnectionAsync()
        {
            var connection = new SqliteConnection(ConnectionString);
            DependencyScope.DisposeFunc.Add(() => Close(connection));
            int cnt;
            lock (Connections)
            {
                Connections.Add(connection);
                cnt = Connections.Count;
            }
            DependencyScope.Logger.Debug("打开连接数：{0}", cnt);
            //Trace.WriteLine(_count++, "Open");
            //Trace.WriteLine("Opened _connection", "SqliteDataBase");
            await connection.OpenAsync();
            return connection;
        }

        #endregion

        #region 连接

        /// <summary>
        ///     打开连接
        /// </summary>
        /// <returns>是否打开,是则为此时打开,否则为之前已打开</returns>
        public async Task<bool> OpenAsync()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                return false;
            }
            //if (_isClosed)
            //{
            //    //throw new Exception("已关闭的数据库对象不能再次使用");
            //}
            if (_connection == null)
            {
                _connection = await InitConnectionAsync();
                return true;
                //Trace.WriteLine("Create _connection", "SqliteDataBase");
            }
            if (string.IsNullOrEmpty(_connection.ConnectionString))
            {
                //Trace.WriteLine("Set ConnectionString", "SqliteDataBase");
                _connection.ConnectionString = ConnectionString;
            }
            //Trace.WriteLine(_count++, "Open");
            //Trace.WriteLine("Opened _connection", "SqliteDataBase");
            await _connection.OpenAsync();
            return true;
        }

        /// <summary>
        ///     关闭连接
        /// </summary>
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        public async Task CloseAsync()
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        {
            Close(_connection);
            _connection = null;
        }

        #endregion

        #region 数据库特殊操作

        /// <summary>
        ///     清除所有数据
        /// </summary>
        public async Task ClearAsync(string table)
        {
            await ExecuteAsync($@"TRUNCATE TABLE [{table}];");
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
            using var cmd = CreateCommand(sql, args);
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
            object result;
            using (var cmd = CreateCommand(sql, args))
            {
                result = await cmd.ExecuteScalarAsync();
            }
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