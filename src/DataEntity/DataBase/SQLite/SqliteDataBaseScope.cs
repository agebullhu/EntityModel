using System.Data.SQLite;
using System.Diagnostics;
using System.Threading;
using Agebull.Common.Base;

namespace Gboxt.Common.SimpleDataAccess.SQLite
{
    public sealed class SqliteDataBaseScope : ScopeBase
    {
        private readonly bool _isHearTransaction;
        private readonly SqliteDataBase _dataBase;
        public static SqliteDataBaseScope CreateScope(SqliteDataBase dataBase,bool trans=false)
        {
            return new SqliteDataBaseScope(dataBase, trans);
        }

        private SqliteDataBaseScope(SqliteDataBase dataBase, bool trans = false)
        {
            _dataBase = dataBase;
            if (dataBase.QuoteCount <= 0)
            {
                dataBase.Open();
            }
            else
            {
                dataBase.QuoteCount += 1;
            }
            if (!trans || dataBase.Transaction == null)
            {
                return;
            }
            this._isHearTransaction = true;
            dataBase.Transaction = dataBase.Connection.BeginTransaction();
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        protected override void OnDispose()
        {
            if (this._isHearTransaction)
            {
                _dataBase.Transaction.Commit();
                _dataBase.Transaction = null;
            }
            if (this._dataBase.QuoteCount <= 0)
                return;
            this._dataBase.QuoteCount -= 1;
            if (this._dataBase.QuoteCount != 0)
                return;
            this._dataBase.Close();
        }
    }
}