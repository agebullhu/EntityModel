/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/2 2:10:24*/
#region
using Agebull.EntityModel.BusinessLogic;
using Agebull.EntityModel.Common;
using System;
using Zeroteam.MessageMVC.EventBus.DataAccess;
#endregion

namespace Zeroteam.MessageMVC.EventBus.BusinessLogic
{
    /// <summary>
    /// 事件订阅
    /// </summary>
    public partial class EventSubscribeEntityBusinessLogic : BusinessLogicByStateData<EventSubscribeEntity, long>
    {
        /// <summary>
        ///  构造
        /// </summary>
        public EventSubscribeEntityBusinessLogic(IServiceProvider provider)
        {
            ServiceProvider = provider;
        }

        protected sealed override DataAccess<EventSubscribeEntity> CreateAccess()
        {
            return ServiceProvider.CreateDataAccess<EventSubscribeEntity>();
        }
        #region 基础继承

        /*// <summary>
        ///     保存前的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool OnSaving(EventSubscribeEntity data, bool isAdd)
        {
             return base.OnSaving(data, isAdd);
        }

        /// <summary>
        ///     保存完成后的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool OnSaved(EventSubscribeEntity data, bool isAdd)
        {
             return base.OnSaved(data, isAdd);
        }

        /// <summary>
        ///     被用户编辑的数据的保存前操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool LastSavedByUser(EventSubscribeEntity data, bool isAdd)
        {
            return base.LastSavedByUser(data, isAdd);
        }

        /// <summary>
        ///     被用户编辑的数据的保存前操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool PrepareSaveByUser(EventSubscribeEntity data, bool isAdd)
        {
            return base.PrepareSaveByUser(data, isAdd);
        }*/
        #endregion


    }
}