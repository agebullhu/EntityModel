using System;
using System.Data.SQLite;
using Agebull.Common.Base;


namespace Agebull.Common.DataModel.Sqlite
{
    /// <summary>
    ///     �޸Ķ�ȡ�ֶη�Χ
    /// </summary>
    /// <typeparam name="TEntity">ʵ�����</typeparam>
    /// <typeparam name="TSqliteDataBase">���ڵ����ݿ����,��ͨ��Ioc�Զ�����</typeparam>
    public sealed class SqliteReaderScope<TEntity, TSqliteDataBase> : ScopeBase
        where TEntity : EditDataObject, new()
        where TSqliteDataBase : SqLiteDataBase
    {
        private readonly SqliteTable<TEntity, TSqliteDataBase> _table;
        private readonly string _fields;
        private readonly Action<SQLiteDataReader, TEntity> _loadAction;

        private SqliteReaderScope(SqliteTable<TEntity, TSqliteDataBase> table, string fields, Action<SQLiteDataReader, TEntity> loadAction)
        {
            if (string.IsNullOrWhiteSpace(fields))
                fields = null;
            _table = table;
            _fields = table.DynamicReadFields ;
            _loadAction = table.DynamicLoadAction;
            table.DynamicReadFields = fields;
            table.DynamicLoadAction = loadAction;
        }

        /// <summary>
        /// �����޸Ķ�ȡ�ֶη�Χ
        /// </summary>
        /// <param name="table">���õı����</param>
        /// <param name="fields">�ֶ�</param>
        /// <param name="loadAction">��ȡ����</param>
        /// <returns>��ȡ����Χ</returns>
        public static SqliteReaderScope<TEntity, TSqliteDataBase> CreateScope(SqliteTable<TEntity, TSqliteDataBase> table, string fields, Action<SQLiteDataReader, TEntity> loadAction)
        {
            return new SqliteReaderScope<TEntity, TSqliteDataBase>(table, fields, loadAction);
        }

        /// <summary>
        /// ����
        /// </summary>
        protected override void OnDispose()
        {
            _table.DynamicReadFields = _fields;
            _table.DynamicLoadAction = _loadAction;
        }
    }
}