/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/2 2:39:05*/
#region
using Agebull.EntityModel.BusinessLogic;
using Agebull.EntityModel.Common;
using System;
using Zeroteam.MessageMVC.EventBus.DataAccess;
#endregion

namespace Zeroteam.MessageMVC.EventBus.BusinessLogic
{
    /// <summary>
    /// 事件定义
    /// </summary>
    public partial class EventDefaultModelBusinessLogic : BusinessLogicBase<EventDefaultModel, long>
    {
        /// <summary>
        ///  构造
        /// </summary>
        public EventDefaultModelBusinessLogic(IServiceProvider provider)
        {
            ServiceProvider = provider;
        }

        protected sealed override DataAccess<EventDefaultModel> CreateAccess()
        {
            return ServiceProvider.CreateDataAccess<EventDefaultModel>();
        }
        #region 基础继承

        /*// <summary>
        ///     保存前的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool OnSaving(EventDefaultModel data, bool isAdd)
        {
             return base.OnSaving(data, isAdd);
        }

        /// <summary>
        ///     保存完成后的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool OnSaved(EventDefaultModel data, bool isAdd)
        {
             return base.OnSaved(data, isAdd);
        }

        /// <summary>
        ///     被用户编辑的数据的保存前操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool LastSavedByUser(EventDefaultModel data, bool isAdd)
        {
            return base.LastSavedByUser(data, isAdd);
        }

        /// <summary>
        ///     被用户编辑的数据的保存前操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool PrepareSaveByUser(EventDefaultModel data, bool isAdd)
        {
            return base.PrepareSaveByUser(data, isAdd);
        }*/
        #endregion


    }
}