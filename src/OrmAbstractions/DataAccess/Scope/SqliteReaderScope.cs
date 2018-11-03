using System;
using System.Data.Common;
using System.Data.SQLite;
using Agebull.Common.Base;
using Gboxt.Common.DataModel;


namespace Agebull.Orm.Abstractions
{
    /// <summary>
    ///     �޸Ķ�ȡ�ֶη�Χ
    /// </summary>
    /// <typeparam name="TEntity">ʵ�����</typeparam>
    /// <typeparam name="TSqliteDataBase">���ڵ����ݿ����,��ͨ��Ioc�Զ�����</typeparam>
    public sealed class ReaderScope<TEntity, TSqliteDataBase> : ScopeBase
        where TEntity : EditDataObject, new()
        where TSqliteDataBase : OrmDataBase
    {
        private readonly DataTable<TEntity, TSqliteDataBase> _table;
        private readonly string _fields;
        private readonly Action<DbDataReader, TEntity> _loadAction;

        private ReaderScope(DataTable<TEntity, TSqliteDataBase> table, string fields, Action<DbDataReader, TEntity> loadAction)
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
        public static ReaderScope<TEntity, TSqliteDataBase> CreateScope(DataTable<TEntity, TSqliteDataBase> table, string fields, Action<DbDataReader, TEntity> loadAction)
        {
            return new ReaderScope<TEntity, TSqliteDataBase>(table, fields, loadAction);
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