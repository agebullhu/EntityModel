// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-12
// // *****************************************************/

#region 引用

using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;
using Newtonsoft.Json;

#endregion

namespace Gboxt.Common.SystemModel.DataAccess
{
    /// <summary>
    ///     页面节点
    /// </summary>
    sealed partial class PageItemDataAccess : MySqlTable<PageItemData, SystemDb>
    {
        ///// <summary>
        /////     保存完成后期处理(Insert或Update)
        ///// </summary>
        ///// <param name="entity"></param>
        //protected override void OnDataSaved(PageItemData entity)
        //{
        //    using (var proxy = new RedisProxy())
        //    {
        //        proxy.RefreshCache<PageItemData, PageItemDataAccess>(entity.Id);
        //        var keys = proxy.Client.SearchKeys("ui:PageTree:*");
        //        proxy.Client.RemoveAll(keys);
        //    }
        //    base.OnDataSaved(entity);
        //}

        protected override void OnPrepareSave(DataOperatorType operatorType, PageItemData entity)
        {
            entity.Json = JsonConvert.SerializeObject(entity.Config);
            base.OnPrepareSave(operatorType, entity);
        }
    }
}