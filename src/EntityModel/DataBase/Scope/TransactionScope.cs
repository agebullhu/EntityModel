// // /*****************************************************
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

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     数据库事务范围对象
    /// </summary>
    public class TransactionScope : ITransactionScope
    {
        /// <summary>
        ///     数据库连接对象
        /// </summary>
        private readonly IDataBase _dataBase;
        
        /// <summary>
        ///     上一个范围
        /// </summary>
        private readonly TransactionScope _preScope;

        /// <summary>
        ///    是否此处开始事务
        /// </summary>
        private readonly bool _isBegin;
        
        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="dataBase">数据库对象</param>
        private TransactionScope(IDataBase dataBase)
        {
            _preScope = CurrentScope;
            CurrentScope = this;

            _dataBase = dataBase;
            if (dataBase.Transaction != null)
            {
                return;
            }
            _isBegin = true;
            //dataBase.BeginTransaction();
        }

        /// <summary>
        ///     当前范围
        /// </summary>
        [field: ThreadStatic]
        public static TransactionScope CurrentScope { get; private set; }

        /// <summary>
        ///     是否已成功
        /// </summary>
        public bool IsSucceed { get; private set; }

        /// <summary>
        ///     析构
        /// </summary>
        void IDisposable.Dispose()
        {
            DoDispose();
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
        
        public static TransactionScope CreateScope(IDataBase dataBase)
        {
            return new TransactionScope(dataBase);
        }

        /// <summary>
        ///     设置操作状态
        /// </summary>
        /// <param name="succeed">是否成功</param>
        public bool SetState(bool succeed)
        {
            IsSucceed = succeed;
            //失败向上冒泡，成功由上层自行决定
            if (!succeed)
            {
                _preScope?.SetState(false);
            }
            return IsSucceed;
        }

        /// <summary>
        ///     析构
        /// </summary>
        private void DoDispose()
        {
            if (CurrentScope == this)
            {
                CurrentScope = _preScope;
            }
            if (!_isBegin)
            {
                return;
            }
            //if (!IsSucceed)
            //{
            //    _dataBase.Rollback();
            //    LogRecorderX.MonitorTrace("事务回滚");
            //}
            //else
            //{
            //    _dataBase.Commit();
            //    LogRecorderX.MonitorTrace("事务提交");
            //}
        }
    }
}