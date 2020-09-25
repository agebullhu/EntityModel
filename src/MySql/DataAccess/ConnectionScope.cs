using Agebull.EntityModel.Common;
using MySqlConnector;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    /// 数据库连接范围
    /// </summary>
    public class ConnectionScope : IConnectionScope
    {
        readonly MySqlDataBase DataBase;

        /// <summary>
        ///     是否此处打开数据库
        /// </summary>
        private bool _isHereOpen;

        DbTransaction IConnectionScope.Transaction => DataBase.Transaction;

        DbConnection IConnectionScope.Connection => DataBase._connection;


        ConnectionScope(MySqlDataBase dataBase)
        {
            DataBase = dataBase;
        }

        internal static async Task<IConnectionScope> CreateScope(MySqlDataBase dataBase)
        {
            var scope = new ConnectionScope(dataBase);
            if (dataBase._connection == null)
            {
                scope._isHereOpen = true;
                await dataBase.OpenAsync();
            }
            return scope;
        }

        /// <summary>
        ///     清理资源
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (_isHereOpen)
            {
                _isHereOpen = false;
                await DataBase.CloseAsync();
            }
            if (_hereTransaction)
            {
                await DataBase.Transaction.DisposeAsync();
                DataBase.Transaction = null;
            }
        }

        #region 生成命令对象

        /// <summary>
        ///     生成命令
        /// </summary>
        public DbCommand CreateCommand(string sql, DbParameter[] args)
        {
            var cmd = new MySqlCommand(sql, DataBase._connection, DataBase.Transaction);

            foreach (var para in args?.OfType<MySqlParameter>())
                cmd.Parameters.Add(para);
            //TraceSql(cmd);
            return cmd;
        }

        #endregion
        #region 事务

        bool _hereTransaction;

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        public async Task<bool> BeginTransaction()
        {
            if (DataBase.Transaction != null)
                return false;
            _hereTransaction = true;
            DataBase.TransactionSuccess = false;
            DataBase.Transaction = await DataBase._connection.BeginTransactionAsync();
            return true;
        }
        
        /// <summary>
        /// 回滚事务
        /// </summary>
        public async Task Rollback()
        {
            DataBase.TransactionSuccess = false;
            if (DataBase.Transaction == null)
                await DataBase.Transaction.RollbackAsync();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public async Task Commit()
        {
            DataBase.TransactionSuccess = true;
            if (_hereTransaction && DataBase.Transaction != null)
                await DataBase.Transaction.CommitAsync();
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
        public async Task<(bool hase, object value)> ExecuteScalarAsync(string sql, params DbParameter[] args)
        {
            await using var cmd = CreateCommand(sql, args);
            var result = await cmd.ExecuteScalarAsync();
            return (result != null, result == DBNull.Value ? null : result);
        }

        #endregion
    }
}