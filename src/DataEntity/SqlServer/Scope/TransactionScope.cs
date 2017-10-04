// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:YhxBank.FundsManagement
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;

#endregion

namespace Gboxt.Common.DataModel.SqlServer
{
    /// <summary>
    ///     数据库事务范围对象
    /// </summary>
    public class TransactionScope : IDisposable
    {
        /// <summary>
        ///     当前范围
        /// </summary>
        [ThreadStatic] private static TransactionScope _currentScope;

        /// <summary>
        ///     数据库连接对象
        /// </summary>
        private readonly SqlServerDataBase _dataBase;

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
        private readonly TransactionScope _preScope;

        /// <summary>
        ///     启用事务方式类型，0未起事务，1未建新连接起事务，2建新连接且起事务
        /// </summary>
        private int _beginType;

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="dataBase">数据库对象</param>
        private TransactionScope(SqlServerDataBase dataBase)
        {
            this._preScope = CurrentScope;
            CurrentScope = this;

            this._dataBase = dataBase;
            dataBase.Open();
            if (dataBase.Transaction != null)
            {
                return;
            }
            this._beginType = 1;
            dataBase.Transaction = dataBase.Connection.BeginTransaction();
        }

        /// <summary>
        ///     当前范围
        /// </summary>
        public static TransactionScope CurrentScope
        {
            get { return _currentScope; }
            private set { _currentScope = value; }
        }

        /// <summary>
        ///     是否已成功
        /// </summary>
        public bool IsSucceed { get; private set; } = true;

        /// <summary>
        ///     析构
        /// </summary>
        void IDisposable.Dispose()
        {
            this.DoDispose();
        }

        /// <summary>
        ///     结束所有范围
        /// </summary>
        public static void EndAll()
        {
            while (CurrentScope != null)
            {
                CurrentScope.DoDispose();
            }
        }

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="dataBase">数据库对象</param>
        
        public static TransactionScope CreateScope(SqlServerDataBase dataBase)
        {
            return new TransactionScope(dataBase);
        }

        /// <summary>
        ///     设置操作状态
        /// </summary>
        /// <param name="succeed">是否成功</param>
        public void SetState(bool succeed)
        {
            this.IsSucceed = succeed;
            //失败向上冒泡，成功由上层自行决定
            if (this._beginType > 0 && !succeed && this._preScope != null)
            {
                this._preScope.SetState(false);
            }
        }

        /// <summary>
        ///     析构
        /// </summary>
        private void DoDispose()
        {
            if (CurrentScope == this)
            {
                CurrentScope = this._preScope;
            }
            if (this._beginType == 0)
            {
                return;
            }
            if (!this.IsSucceed)
            {
                this._dataBase.Transaction.Rollback();
            }
            else
            {
                this._dataBase.Transaction.Commit();
            }
            if (this._beginType == 2)
            {
                this._dataBase.Connection.Close();
            }
            this._beginType = 0;
        }
    }
}