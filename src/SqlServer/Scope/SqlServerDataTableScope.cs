// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:YhxBank.FundsManagement
// // 建立:2016-06-16
// // 修改:2016-06-16
// // *****************************************************/

using Agebull.EntityModel.Common;

namespace Agebull.EntityModel.SqlServer
{
    /// <summary>
    ///     表对象操作范围
    /// </summary>
    /// <typeparam name="TEntity">实体对象</typeparam>
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