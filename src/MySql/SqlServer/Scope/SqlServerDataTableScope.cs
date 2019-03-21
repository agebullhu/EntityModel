// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:YhxBank.FundsManagement
// // ����:2016-06-16
// // �޸�:2016-06-16
// // *****************************************************/

namespace Gboxt.Common.DataModel.SqlServer
{
    /// <summary>
    ///     ����������Χ
    /// </summary>
    /// <typeparam name="TEntity">ʵ�����</typeparam>
    public sealed class SqlServerDataTableScope<TEntity> : SqlServerDataBaseScope
        where TEntity : EditDataObject, new()
    {
        private SqlServerDataTableScope(SqlServerDataBase dataBase, SqlServerTable<TEntity> table)
            : base(dataBase)
        {
            Table = table;
        }

        public SqlServerTable<TEntity> Table { get; private set; }

        public static SqlServerDataTableScope<TEntity> CreateScope(SqlServerDataBase dataBase, SqlServerTable<TEntity> table)
        {
            return new SqlServerDataTableScope<TEntity>(dataBase, table);
        }
    }
}