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
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    ///     表示MySql数据库对象
    /// </summary>
    public abstract partial class MySqlDataBase : ParameterCreater, IDataBase
    {
        #region 构造

        /// <summary>
        /// 构造
        /// </summary>
        static MySqlDataBase()
        {
            MySqlConnectionsManager.InternalInitialize();
        }

        /// <summary>
        /// 构造
        /// </summary>
        protected MySqlDataBase()
        {
            MySqlConnectionsManager.InternalInitialize();
            DependencyScope.DisposeFunc.Add(() => _ = DisposeAsync());
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => DataBaseType.MySql;

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DataBaseName { get; private set; }

        #endregion

        #region 事务

        /// <summary>
        ///     事务对象
        /// </summary>
        public MySqlTransaction Transaction { get; internal set; }

        /// <inheritdoc />
        /// <summary>
        ///     事务对象
        /// </summary>
        DbTransaction IDataBase.Transaction => Transaction;

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        public async Task<bool> BeginTransaction()
        {
            if (Transaction != null)
                return false;
            await OpenAsync();
            Transaction = await _connection.BeginTransactionAsync();
            return true;
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        async Task IDataBase.Rollback()
        {
            await Transaction?.RollbackAsync();
            await Transaction?.DisposeAsync().AsTask();
            Transaction = null;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        async Task IDataBase.Commit()
        {
            await Transaction?.CommitAsync();
            await Transaction?.DisposeAsync().AsTask();
            Transaction = null;
        }
        #endregion

        #region 连接

        /// <summary>
        /// 连接字符串配置节点名称,用于取出
        /// </summary>
        public string ConnectionStringName { get; set; }

        /// <summary>
        /// 是否锁定连接对象(更新插入删除发生后自动启用)
        /// </summary>
        public bool IsLockConnection { get; set; }

        internal MySqlConnection _connection;

        #endregion

        #region 析构
        private bool _isDisposed;

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;
            await DoDispose();

            if (_connection == null)
            {
                return;
            }
            _connection = null;
            IsLockConnection = false;
            if (Transaction != null)
            {
                await Transaction.RollbackAsync();
                Transaction = null;
            }
            await MySqlConnectionsManager.Close(_connection, ConnectionStringName);
        }

        /// <summary>
        /// 析构
        /// </summary>
        ~MySqlDataBase()
        {
            _ = DisposeAsync();
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        protected virtual Task DoDispose()
        {
            return Task.CompletedTask;
        }

        #endregion

        #region 连接

        /// <summary>
        ///     打开连接
        /// </summary>
        /// <returns>是否打开,是则为此时打开,否则为之前已打开</returns>
        public async Task<bool> OpenAsync()
        {
            if (_connection != null/* && _connection.State == ConnectionState.Open*/)
                return false;
            _connection = await MySqlConnectionsManager.InitConnection(ConnectionStringName);
            IsLockConnection = true;
            return IsLockConnection;
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
            using var scope = await ConnectionScope.CreateScope(this);
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
            using var scope = await ConnectionScope.CreateScope(this);
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
            using var scope = await ConnectionScope.CreateScope(this);
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

        #region 生成命令对象

        /// <summary>
        /// 构造连接范围对象
        /// </summary>
        /// <returns></returns>

        Task<IConnectionScope> IDataBase.CreateConnectionScope() => ConnectionScope.CreateScope(this);

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
        public DbCommand CreateCommand(IConnectionScope scope, string sql, IEnumerable<DbParameter> args = null)
        {
            var cmd = scope.Connection.CreateCommand();

            if (scope.Transaction != null)
            {
                cmd.Transaction = scope.Transaction;
            }
            if (sql != null)
            {
                cmd.CommandText = sql;
            }
            if (args != null)
            {
                var parameters = args.OfType<MySqlParameter>().ToArray();
                if (parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
            }
            TraceSql(cmd);
            return cmd;
        }

        #endregion


        #region SQL日志

        /// <summary>
        ///     记录SQL日志
        /// </summary>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        public void TraceSql(DbCommand cmd)
        {
            TraceSql(cmd.CommandText, cmd.Parameters.Cast<MySqlParameter>());
        }

        /// <summary>
        ///     记录SQL日志
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        public void TraceSql(string sql, IEnumerable<DbParameter> args)
        {
            if (!LoggerExtend.LogDataSql)
                return;
            StringBuilder code = new StringBuilder();
            code.AppendLine("/***************************************************************/");
            var parameters = args as MySqlParameter[] ?? args.ToArray();
            foreach (var par in parameters)
            {
                code.AppendLine($"declare ?{par.ParameterName} {par.DbType};");
            }
            foreach (var par in parameters)
            {
                if (par.Value == null || par.IsNullable)
                    code.AppendLine($"SET ?{par.ParameterName} = NULL;");
                else
                    code.AppendLine($"SET ?{par.ParameterName} = '{par.Value}';");
            }
            code.AppendLine(sql);

            Console.WriteLine(code.ToString());
        }

        #endregion
    }

}