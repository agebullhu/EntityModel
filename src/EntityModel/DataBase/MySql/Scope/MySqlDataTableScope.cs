// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-16
// // 修改:2016-06-16
// // *****************************************************/

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    ///     表对象操作范围
    /// </summary>
    /// <typeparam name="TEntity">实体对象</typeparam>
    public sealed class MySqlDataTableScope<TEntity> : MySqlDataBaseScope
        where TEntity : EditDataObject, new()
    {
        private MySqlDataTableScope(MySqlDataBase dataBase, MySqlTable<TEntity> table)
            : base(dataBase)
        {
            Table = table;
        }

        public MySqlTable<TEntity> Table { get; private set; }

        public static MySqlDataTableScope<TEntity> CreateScope(MySqlDataBase dataBase, MySqlTable<TEntity> table)
        {
            return new MySqlDataTableScope<TEntity>(dataBase, table);
        }
    }
}