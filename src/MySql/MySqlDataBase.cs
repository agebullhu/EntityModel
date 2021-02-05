// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Common;
using Microsoft.Extensions.Logging;
using MySqlConnector;
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
    public abstract partial class MySqlDataBase : SimpleConfig, IDataBase
    {
        #region 构造

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
        ///     事务成功
        /// </summary>
        internal bool? TransactionSuccess { get; set; }

        /// <summary>
        ///     事务对象
        /// </summary>
        internal MySqlTransaction Transaction { get; set; }

        #endregion

        #region 连接

        /// <summary>
        /// 构造连接范围对象
        /// </summary>
        /// <returns></returns>

        public Task<IConnectionScope> CreateConnectionScope() => ConnectionScope.CreateScope(this);

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 连接对象
        /// </summary>
        internal MySqlConnection _connection;

        /// <summary>
        ///     打开连接
        /// </summary>
        /// <returns>是否打开,是则为此时打开,否则为之前已打开</returns>
        internal async Task<bool> OpenAsync()
        {
            if (_connection != null/* && _connection.State == ConnectionState.Open*/)
                return false;
            _connection = await OpenConnection(ConnectionString);
            return true;
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        internal async Task CloseAsync()
        {
            if (_connection == null)
            {
                return;
            }
            var con = _connection;
            _connection = null;
            if (Transaction != null)
            {
                if (TransactionSuccess.Value)
                    await Transaction.CommitAsync();
                else
                    await Transaction.RollbackAsync();
                Transaction = null;
            }
            await CloseConnection(con);
        }


        /// <summary>
        /// 初始化连接对象
        /// </summary>
        /// <returns></returns>
        public static async Task<MySqlConnection> OpenConnection(string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }

        /// <summary>
        /// 关闭连接对象
        /// </summary>
        /// <returns></returns>
        public static async Task CloseConnection(MySqlConnection connection)
        {
            if (connection == null)
            {
                //Console.WriteLine($"【MySqlDataBase.CloseConnection】xxx");
                return;
            }
            //Console.WriteLine($"【MySqlDataBase.CloseConnection】{DateTime.Now}");
            if (connection.State == ConnectionState.Open)
            {
                try
                {
                    await connection.CloseAsync();
                }
                catch
                {
                }
            }
            try
            {
                await connection.DisposeAsync();
            }
            catch
            {
            }
        }

        #endregion

        #region 析构

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            await CloseAsync();
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            _ = CloseAsync();
        }

        /// <summary>
        /// 析构
        /// </summary>
        ~MySqlDataBase()
        {
            _ = CloseAsync();
        }
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
        public async Task<int> ExecuteAsync(string sql, IEnumerable<DbParameter> args)
        {
            await using var scope = await CreateConnectionScope();
            using var cmd = scope.CreateCommand(sql, args);
            TraceSql(cmd);
            return await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        ///     执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        public async Task<(bool hase, object value)> ExecuteScalarAsync(string sql, IEnumerable<DbParameter> parameters)
        {
            await using var scope = await CreateConnectionScope();
            await using var cmd = scope.CreateCommand(sql, parameters);
            TraceSql(cmd);
            var result = await cmd.ExecuteScalarAsync();
            return (result != null, result == DBNull.Value ? null : result);
        }

        #endregion

        #region SQL日志

        /// <summary>
        /// 日志
        /// </summary>
        public ILogger Logger
        {
            get;
            set;
        }

        /// <summary>
        ///     记录SQL日志
        /// </summary>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        public void TraceSql(DbCommand cmd)
        {
            if (Logger == null || !Logger.IsEnabled(LogLevel.Debug))
                return;
            StringBuilder code = new StringBuilder();
            var parameters = cmd.Parameters.OfType<MySqlParameter>();
            foreach (var par in parameters)
            {
                code.AppendLine($"DECLARE ?{par.ParameterName} {par.DbType};");
            }
            foreach (var par in parameters)
            {
                if (par.Value == null || par.IsNullable)
                    code.AppendLine($"SET ?{par.ParameterName} = NULL;");
                else
                    code.AppendLine($"SET ?{par.ParameterName} = '{par.Value}';");
            }
            code.AppendLine(cmd.CommandText);
            Logger.LogDebug(code.ToString());
        }

        #endregion
    }

}