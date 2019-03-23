// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:YhxBank.FundsManagement
// // ����:2016-06-16
// // �޸�:2016-06-16
// // *****************************************************/

using System;
using System.Data.SqlClient;
using Agebull.Common.Base;
using Agebull.EntityModel.Common;

namespace Agebull.EntityModel.SqlServer
{
    /// <summary>
    ///     �޸Ķ�ȡ�ֶη�Χ
    /// </summary>
    /// <typeparam name="TEntity">ʵ�����</typeparam>
    /// <typeparam name="TSqlServerDataBase">���ڵ����ݿ����,��ͨ��Ioc�Զ�����</typeparam>
    public sealed class SqlServerReadTableScope<TEntity, TSqlServerDataBase> : ScopeBase
        where TEntity : EditDataObject, new()
        where TSqlServerDataBase : SqlServerDataBase
    {
        private readonly SqlServerTable<TEntity, TSqlServerDataBase> _table;
        private readonly string _fields;
        private readonly Action<SqlDataReader, TEntity> _loadAction;

        private SqlServerReadTableScope(SqlServerTable<TEntity, TSqlServerDataBase> table, string fields, Action<SqlDataReader, TEntity> loadAction)
        {
            if (string.IsNullOrWhiteSpace(fields))
                fields = null;
            _table = table;
            _fields = table.DynamicReadFields;
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
        public static SqlServerReadTableScope<TEntity, TSqlServerDataBase> CreateScope(SqlServerTable<TEntity, TSqlServerDataBase> table, string fields, Action<SqlDataReader, TEntity> loadAction)
        {
            return new SqlServerReadTableScope<TEntity, TSqlServerDataBase>(table, fields, loadAction);
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