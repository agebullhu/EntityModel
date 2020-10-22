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
            if (_hereTransaction)
            {
                if (DataBase.TransactionSuccess == null)
                    await DataBase.Transaction.RollbackAsync();
                await DataBase.Transaction.DisposeAsync();
                DataBase.Transaction = null;
            }
            if (_isHereOpen)
            {
                _isHereOpen = false;
                await DataBase.CloseAsync();
            }
        }

        #region 生成命令对象

        /// <summary>
        ///     生成命令
        /// </summary>
        public DbCommand CreateCommand(string sql, DbParameter[] args)
        {
            var cmd = new MySqlCommand(sql, DataBase._connection, DataBase.Transaction);
            if (args != null)
                foreach (var para in args.OfType<MySqlParameter>())
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
            DataBase.TransactionSuccess = null;
            DataBase.Transaction = await DataBase._connection.BeginTransactionAsync();
            return true;
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public async Task Rollback()
        {
            if (DataBase.TransactionSuccess == true)
                return;
            DataBase.TransactionSuccess = false;
            if (DataBase.Transaction != null)
                await DataBase.Transaction.RollbackAsync();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public async Task Commit()
        {
            if (DataBase.TransactionSuccess == false)
                return;
            DataBase.TransactionSuccess = true;
            if (_hereTransaction && DataBase.Transaction != null)
                await DataBase.Transaction.CommitAsync();
        }
        #endregion

    }
}