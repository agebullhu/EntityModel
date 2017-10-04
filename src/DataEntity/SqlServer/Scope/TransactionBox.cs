using System;
using System.Data.SqlClient;
using Gboxt.Common.DataModel.SqlServer;
using MS.Lib.BusinessEntity;

namespace MS.Lib
{
    /// <summary>
    /// 表示一个事务盒子(即共用一个事务,以便启用多个不关联的事务而不冲突)
    /// </summary>
    public class TransactionBox
    {
        /// <summary>
        /// 构造并起新事务
        /// </summary>
        /// <param name="dataBase"></param>
        public TransactionBox(SqlServerDataBase dataBase)
        {
            DataBase = dataBase;
            if (dataBase.Transaction != null)
                return;
            _isBegin = true;
            _oldTransaction = dataBase.Transaction;
            _oldConnection = dataBase.Connection;

            dataBase.Connection = new SqlConnection(dataBase.ConnectionString);
            dataBase.Transaction = dataBase.Connection.BeginTransaction();
        }

        private bool _isBegin;
        /// <summary>
        ///     事务
        /// </summary>
        public SqlTransaction Transaction { get; internal set; }

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public SqlServerDataBase DataBase { get; private set; }


        private SqlConnection _oldConnection;

        private readonly SqlTransaction _oldTransaction;

         

        /// <summary>
        /// 是否处于错误状态
        /// </summary>
        public bool IsFailed { get; private set; }

        /// <summary>
        /// 事件请求数量,成对增减
        /// </summary>
        private int _transCount;

        /// <summary>
        /// 是否在数据库事务中
        /// </summary>
        public bool InDbTransaction
        {
            get { return _transCount > 0; }
        }

        /// <summary>
        /// 开始数据库事务
        /// </summary>
        internal void BeginDbTransaction()
        {

            if (IsFailed)
            {
                throw new Exception("失败的事务不能继续使用");
            }
            if (TransactionManager == null)
            {
                throw new Exception("终结的事务不能继续使用");
            }
            if (_transCount <= 0)
            {
                TransactionManager.Begin(ConnectionManager);
                _transCount = 0;
            }
            _transCount++;
        }
        /// <summary>
        /// 结束数据库事务
        /// </summary>
        /// <param name="succeed">已成功吗</param>
        internal void EndDbTransaction(bool succeed)
        {
            _transCount--;
            if (IsFailed)
                return;
            if (!succeed)
            {
                IsFailed = true;
                TransactionManager.Abort(ConnectionManager);
            }
            else if (_transCount > 0)
            {
                return;
            }
            else
            {
                TransactionManager.Commit(ConnectionManager);
            }
            TransactionManager = null;
        }
    }
}