using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    /// 数据库连接范围
    /// </summary>
    public class ConnectionScope : IAsyncDisposable, IDisposable
    {
        MySqlDataBase DataBase;
        bool IsLockConnection;
        internal ConnectionScope(MySqlDataBase dataBase)
        {
            IsLockConnection = dataBase.IsLockConnection;
            DataBase = dataBase;
            Transaction = dataBase.Transaction;
            if (IsLockConnection)
                Connection = dataBase._connection;
            else
                Connection = MySqlConnectionsManager.GetConnection(dataBase.ConnectionStringName);
        }

        /// <summary>
        ///     事务对象
        /// </summary>
        internal MySqlTransaction Transaction { get; private set; }

        internal MySqlConnection Connection { get; private set; }


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