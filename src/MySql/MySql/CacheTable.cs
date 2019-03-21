// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

#endregion

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    ///     Sql实体访问类(支持本地缓存,用于防止并发带来的数据不一致)
    /// </summary>
    /// <typeparam name="TData">实体</typeparam>
    public abstract class CacheTable<TData> : MySqlTable<TData>
        where TData : EditDataObject, IIdentityData, new()
    {
        /// <summary>
        ///     主键读取
        /// </summary>
        public override TData LoadByPrimaryKey(object key)
        {
            var data = MySqlDataBase.DefaultDataBase.GetData<TData>(TableId, (int) key);
            return data ?? base.LoadByPrimaryKey(key);
        }

        /// <summary>
        ///     载入后的同步处理
        /// </summary>
        /// <param name="entity"></param>
        protected override TData OnEntityLoaded(TData entity)
        {
            return MySqlDataBase.DefaultDataBase.TryAddToCache(TableId, entity.Id, entity);
        }
    }
}