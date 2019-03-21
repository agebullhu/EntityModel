// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-16
// // �޸�:2016-06-16
// // *****************************************************/

using Agebull.Common.Base;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     �޸Ķ�ȡ����Χ
    /// </summary>
    /// <typeparam name="TEntity">ʵ�����</typeparam>
    public sealed class ReadTableScope<TEntity> : ScopeBase
        where TEntity : EditDataObject, new()
    {
        private readonly IDataTable<TEntity> _table;
        private readonly string _oldName;
        private ReadTableScope(IDataTable<TEntity> table, string name)
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
        public static ReadTableScope<TEntity> CreateScope(IDataTable<TEntity> table, string name)
        {
            return new ReadTableScope<TEntity>(table, name);
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