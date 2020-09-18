// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:YhxBank.FundsManagement
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

#endregion

namespace Gboxt.Common.DataModel.SqlServer
{
    /// <summary>
    ///     Sqlʵ�������(֧�ֱ��ػ���,���ڷ�ֹ�������������ݲ�һ��)
    /// </summary>
    /// <typeparam name="TData">ʵ��</typeparam>
    public abstract class CacheTable<TData> : SqlServerTable<TData>
        where TData : EditDataObject, IIdentityData, new()
    {
        /// <summary>
        ///     ������ȡ
        /// </summary>
        public override TData LoadByPrimaryKey(object key)
        {
            var data = SqlServerDataBase.DefaultDataBase.GetData<TData>(TableId, (int) key);
            return data ?? base.LoadByPrimaryKey(key);
        }

        /// <summary>
        ///     ������ͬ������
        /// </summary>
        /// <param name="entity"></param>
        protected override TData OnEntityLoaded(TData entity)
        {
            return SqlServerDataBase.DefaultDataBase.TryAddToCache(TableId, entity.Id, entity);
        }
    }
}