// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.Common.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     数据库事务范围对象
    /// </summary>
    public class TransactionScope : ITransactionScope, IDisposable
    {

        private static readonly AsyncLocal<TransactionScope> local = new AsyncLocal<TransactionScope>();

        /// <summary>
        ///     当前范围
        /// </summary>
        static TransactionScope CurrentScope => local.Value;

        /// <summary>
        ///     上一个范围
        /// </summary>
        private readonly TransactionScope PreScope;

        /// <summary>
        ///     是否已成功
        /// </summary>
        public bool? IsSucceed { get; private set; }

        /// <summary>
        ///    是否此处开始事务
        /// </summary>
        private readonly bool IsBegin;
        /// <summary>
        /// 数据库对象
        /// </summary>
        private readonly IDataBase DataBase;
        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="dataBase">数据库对象</param>
        private TransactionScope(IDataBase dataBase)
        {
            PreScope = CurrentScope;
            local.Value = this;
            DataBase = dataBase;
            if (DataBase.Transaction != null)
            {
                return;
            }
            IsBegin = true;
            //dataBase.BeginTransaction();
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
        ///     设置操作成功
        /// </summary>
        /// <remarks>如果之前被设置为失败,此处即使设置成功也无效</remarks>
        public bool Succeed()
        {
            return SetState(true);
        }

        /// <summary>
        ///     设置操作失败
        /// </summary>
        /// <remarks>被设置为失败,后续即使设置成功也无效</remarks>
        public bool Failed()
        {
            return SetState(true);
        }

        /// <summary>
        ///     设置操作状态
        /// </summary>
        /// <param name="state">是否成功</param>
        /// <remarks>如果之前被设置为失败,此处即使设置成功也无效</remarks>
        public bool SetState(bool state)
        {
            if (IsSucceed == null || !state)
                IsSucceed = state;
            //向上冒泡
            PreScope?.SetState(state);
            return IsSucceed == true;
        }

        /// <summary>
        ///     析构
        /// </summary>
        public ValueTask DisposeAsync()
        {
            Dispose();
            return new ValueTask(Task.CompletedTask);
        }
        bool isDisposed;
        /// <summary>
        ///     析构
        /// </summary>
        public void Dispose()
        {
            if (isDisposed)
                return;
            isDisposed = true;
            if (CurrentScope == this)
            {
                local.Value = PreScope;
            }
            if (!IsBegin)
            {
                return;
            }
            local.Value = null;
            //if (IsSucceed == true)
            //{
            //    DataBase.Rollback();
            //    LogRecorder.MonitorTrace("事务回滚{0}", DataBase.Name);
            //}
            //else
            //{
            //    DataBase.Commit();
            //    LogRecorder.MonitorTrace("事务提交{0}", DataBase.Name);
            //}
        }
    }
}