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
        /// <summary>
        /// �����
        /// </summary>
        public MySqlTable<TEntity> Table { get; private set; }
        /// <summary>
        /// ���ɶ���
        /// </summary>
        /// <param name="dataBase"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static MySqlDataTableScope<TEntity> CreateScope(MySqlDataBase dataBase, MySqlTable<TEntity> table)
        {
            return new MySqlDataTableScope<TEntity>(dataBase, table);
        }
    }
}