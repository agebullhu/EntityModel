using Agebull.EntityModel.Common;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    /// 数据库连接范围
    /// </summary>
    public class ConnectionScope : IAsyncDisposable, IDisposable, IConnectionScope
    {
        readonly MySqlDataBase DataBase;
        readonly bool IsLockConnection;

        ConnectionScope(MySqlDataBase dataBase)
        {
            IsLockConnection = dataBase.IsLockConnection;
            DataBase = dataBase;
            Transaction = dataBase.Transaction;
        }

        internal static async Task<IConnectionScope> CreateScope(MySqlDataBase dataBase)
        {
            var scope = new ConnectionScope(dataBase);

            if (dataBase.IsLockConnection)
                scope.Connection = dataBase._connection;
            else
                scope.Connection = await MySqlConnectionsManager.GetConnection(dataBase.ConnectionStringName);

            return scope;
        }
        /// <summary>
        ///     事务对象
        /// </summary>
        internal MySqlTransaction Transaction { get; private set; }

        internal MySqlConnection Connection { get; private set; }

        DbTransaction IConnectionScope.Transaction => Transaction;

        DbConnection IConnectionScope.Connection => Connection;


        /// <summary>
        ///     清理资源
        /// </summary>

        ValueTask IAsyncDisposable.DisposeAsync()
        {
            Dispose();
            return new ValueTask(Task.CompletedTask);
        }

        bool IsDisposed;

        /// <summary>
        /// 析构
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;
            IsDisposed = true;
            if (!IsLockConnection)
                MySqlConnectionsManager.Close(Connection, DataBase.ConnectionStringName);
        }
    }
}