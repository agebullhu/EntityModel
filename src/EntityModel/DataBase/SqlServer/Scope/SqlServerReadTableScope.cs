// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:YhxBank.FundsManagement
// // ����:2016-06-16
// // �޸�:2016-06-16
// // *****************************************************/

using Agebull.Common.Base;

namespace Gboxt.Common.DataModel.SqlServer
{
    /// <summary>
    ///     �޸Ķ�ȡ����Χ
    /// </summary>
    /// <typeparam name="TEntity">ʵ�����</typeparam>
    public sealed class SqlServerReadTableScope<TEntity> : ScopeBase
        where TEntity : EditDataObject, new()
    {
        private readonly SqlServerTable<TEntity> _table;
        private readonly string _oldName;
        private SqlServerReadTableScope(SqlServerTable<TEntity> table, string name)
        {
            _table = table;
            _oldName = table.SetDynamicReadTable(name);
        }

        /// <summary>
        /// ���ɶ�ȡ����Χ
        /// </summary>
        /// <param name="table">���õı����</param>
        /// <param name="name">����</param>
        /// <returns>��ȡ����Χ</returns>
        public static SqlServerReadTableScope<TEntity> CreateScope(SqlServerTable<TEntity> table, string name)
        {
            return new SqlServerReadTableScope<TEntity>(table, name);
        }

        /// <summary>
        /// ����
        /// </summary>
        protected override void OnDispose()
        {
            _table.SetDynamicReadTable(_oldName);
        }
    }
}