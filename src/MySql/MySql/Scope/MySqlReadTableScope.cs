// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-16
// // �޸�:2016-06-16
// // *****************************************************/

using Agebull.Common.Base;

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    ///     �޸Ķ�ȡ����Χ
    /// </summary>
    /// <typeparam name="TEntity">ʵ�����</typeparam>
    public sealed class MySqlReadTableScope<TEntity> : ScopeBase
        where TEntity : EditDataObject, new()
    {
        private readonly MySqlTable<TEntity> _table;
        private readonly string _oldName;
        private MySqlReadTableScope(MySqlTable<TEntity> table, string name)
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
        public static MySqlReadTableScope<TEntity> CreateScope(MySqlTable<TEntity> table, string name)
        {
            return new MySqlReadTableScope<TEntity>(table, name);
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