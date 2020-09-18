using System;
using Agebull.Common.Base;
using MySql.Data.MySqlClient;

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    ///     �޸Ķ�ȡ�ֶη�Χ
    /// </summary>
    /// <typeparam name="TEntity">ʵ�����</typeparam>
    public sealed class MySqlReaderScope<TEntity> : ScopeBase
        where TEntity : EditDataObject, new()
    {
        private readonly MySqlTable<TEntity> _table;
        private readonly string _fields;
        private readonly Action<MySqlDataReader, TEntity> _loadAction;

        private MySqlReaderScope(MySqlTable<TEntity> table, string fields, Action<MySqlDataReader, TEntity> loadAction)
        {
            if (string.IsNullOrWhiteSpace(fields))
                fields = null;
            _table = table;
            _fields = table._contextReadFields;
            _loadAction = table.ContentLoadAction;
            table._contextReadFields = fields;
            table.ContentLoadAction = loadAction;
        }

        /// <summary>
        /// �����޸Ķ�ȡ�ֶη�Χ
        /// </summary>
        /// <param name="table">���õı����</param>
        /// <param name="fields">�ֶ�</param>
        /// <param name="loadAction">��ȡ����</param>
        /// <returns>��ȡ����Χ</returns>
        public static MySqlReaderScope<TEntity> CreateScope(MySqlTable<TEntity> table, string fields, Action<MySqlDataReader, TEntity> loadAction)
        {
            return new MySqlReaderScope<TEntity>(table, fields, loadAction);
        }

        /// <summary>
        /// ����
        /// </summary>
        protected override void OnDispose()
        {
            _table._contextReadFields = _fields;
            _table.ContentLoadAction = _loadAction;
        }
    }
}