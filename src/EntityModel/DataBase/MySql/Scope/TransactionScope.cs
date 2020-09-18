﻿// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using Agebull.Common.Logging;

#endregion

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    ///     数据库事务范围对象
    /// </summary>
    public class TransactionScope : ITransactionScope
    {
        /// <summary>
        ///     当前范围
        /// </summary>
        [ThreadStatic] private static TransactionScope _currentScope;

        /// <summary>
        ///     数据库连接对象
        /// </summary>
        //private readonly MySqlDataBase _dataBase;

        /*// <summary>
        ///     在范围之前的连接对象
        /// </summary>
        private readonly SqlConnection _oldConnection;

        /// <summary>
        ///     在范围之前的事务对象
        /// </summary>
        private readonly SqlTransaction _oldTransaction;*/

        /// <summary>
        ///     上一个范围
        /// </summary>
        //private readonly TransactionScope _preScope;

        /// <summary>
        ///     启用事务方式类型，0未起事务，1未建新连接起事务，2建新连接且起事务
        /// </summary>
        //private int _beginType;

        //private readonly MySqlDataBaseScope dbScope;
        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="dataBase">数据库对象</param>
        private TransactionScope(MySqlDataBase dataBase)
        {
            //dbScope = MySqlDataBaseScope.CreateScope(dataBase);
            //_preScope = CurrentScope;
            //CurrentScope = this;

            //_dataBase = dataBase;
            //dataBase.Open();
            //if (dataBase.Transaction != null)
            //{
            //    return;
            //}
            //_beginType = 1;
            //dataBase.Transaction = dataBase.Connection.BeginTransaction();
        }

        /// <summary>
        ///     当前范围
        /// </summary>
        public static TransactionScope CurrentScope
        {
            get => _currentScope;
            private set => _currentScope = value;
        }

        /// <summary>
        ///     是否已成功
        /// </summary>
        public bool IsSucceed { get; private set; }

        /// <summary>
        ///     析构
        /// </summary>
        void IDisposable.Dispose()
        {
            //DoDispose();
            //dbScope.Dispose();
        }

        /// <summary>
        ///     结束所有范围
        /// </summary>
        public static void EndAll()
        {
            //while (CurrentScope != null)
            //{
            //    CurrentScope.DoDispose();
            //}
        }

        /// <summary>
        ///     构造(自动构造了数据库对象的使用范围)
        /// </summary>
        /// <param name="dataBase">数据库对象</param>
        
        public static TransactionScope CreateScope(MySqlDataBase dataBase)
        {
            return new TransactionScope(dataBase);
        }

        /// <summary>
        ///     设置操作状态
        /// </summary>
        /// <param name="succeed">是否成功</param>
        public void SetState(bool succeed)
        {
            //IsSucceed = succeed;
            ////失败向上冒泡，成功由上层自行决定
            //if (_beginType > 0 && !succeed)
            //{
            //    _preScope?.SetState(false);
            //}
        }

        /// <summary>
        ///     析构
        /// </summary>
        private void DoDispose()
        {
            //if (CurrentScope == this)
            //{
            //    CurrentScope = _preScope;
            //}
            //if (_beginType == 0)
            //{
            //    return;
            //}
            //if (!IsSucceed)
            //{
            //    _dataBase.Transaction.Rollback();
            //    LogRecorder.MonitorTrace("事务回滚");
            //}
            //else
            //{
            //    _dataBase.Transaction.Commit();
            //    LogRecorder.MonitorTrace("事务提交");
            //}
            //if (_beginType == 2)
            //{
            //    _dataBase.Connection.Close();
            //}
            //_beginType = 0;
        }
    }
}