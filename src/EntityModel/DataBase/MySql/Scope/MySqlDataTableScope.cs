// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-16
// // �޸�:2016-06-16
// // *****************************************************/

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    ///     ����������Χ
    /// </summary>
    /// <typeparam name="TEntity">ʵ�����</typeparam>
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