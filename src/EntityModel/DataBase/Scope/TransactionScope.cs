// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     数据库事务范围对象
    /// </summary>
    public class TransactionScope : ITransactionScope,IDisposable
    {
        /// <summary>
        ///     数据库连接对象
        /// </summary>
        private readonly IDataTable _table;

        /// <summary>
        ///     上一个范围
        /// </summary>
        private readonly TransactionScope _preScope;

        /// <summary>
        ///    是否此处开始事务
        /// </summary>
        private readonly bool _isBegin;

        private readonly DataTableScope _tableScope;
        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="table">数据库对象</param>
        private TransactionScope(IDataTable table)
        {
            _tableScope = DataTableScope.CreateScope(table);
            _preScope = _local.Value;
            _local.Value = this;

            _table = table;
            if (table.DataBase.Transaction != null)
            {
                return;
            }
            _isBegin = true;
            //dataBase.BeginTransaction();
        }

        private static readonly AsyncLocal<TransactionScope> _local = new AsyncLocal<TransactionScope>();

        /// <summary>
        ///     当前范围
        /// </summary>
        public static TransactionScope CurrentScope => _local.Value;

        /// <summary>
        ///     是否已成功
        /// </summary>
        public bool IsSucceed { get; private set; }

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

        public static TransactionScope CreateScope(IDataTable dataBase)
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
        }

        /// <summary>
        ///     析构
        /// </summary>
        public ValueTask DisposeAsync()
        {
            DoDispose();
            if (CurrentScope == this)
            {
                _local.Value = _preScope;
            }
            if (_isBegin)
            {
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
            return _tableScope.DisposeAsync();
        }

        /// <summary>
        ///     析构
        /// </summary>
        public void Dispose()
        {
            DoDispose();
            if (CurrentScope == this)
            {
                _local.Value = _preScope;
            }
            if (_isBegin)
            {
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
            _tableScope.Dispose();
        }
    }
}