using System;
using Agebull.Common.Base;
using Agebull.EntityModel.Common;
using MySql.Data.MySqlClient;

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    ///     �޸Ķ�ȡ�ֶη�Χ
    /// </summary>
    /// <typeparam name="TEntity">ʵ�����</typeparam>
    /// <typeparam name="TMySqlDataBase">���ڵ����ݿ����,��ͨ��Ioc�Զ�����</typeparam>
    public sealed class MySqlReaderScope<TEntity, TMySqlDataBase> : ScopeBase
        where TEntity : EditDataObject, new()
        where TMySqlDataBase : MySqlDataBase
    {
        private readonly MySqlTable<TEntity, TMySqlDataBase> _table;
        private readonly string _fields;
        private readonly Action<MySqlDataReader, TEntity> _loadAction;

        private MySqlReaderScope(MySqlTable<TEntity, TMySqlDataBase> table, string fields, Action<MySqlDataReader, TEntity> loadAction)
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
        public static MySqlReaderScope<TEntity, TMySqlDataBase> CreateScope(MySqlTable<TEntity, TMySqlDataBase> table, string fields, Action<MySqlDataReader, TEntity> loadAction)
        {
            return new MySqlReaderScope<TEntity, TMySqlDataBase>(table, fields, loadAction);
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