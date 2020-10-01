/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/2 2:10:24*/
#region
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json;

using Agebull.Common;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.MySql;
using Agebull.EntityModel.BusinessLogic;
using MySqlConnector;

using Zeroteam.MessageMVC.EventBus.DataAccess;
#endregion

namespace Zeroteam.MessageMVC.EventBus.BusinessLogic
{
    /// <summary>
    /// 事件定义
    /// </summary>
    public partial class EventDefaultEntityBusinessLogic : BusinessLogicByStateData<EventDefaultEntity,long>
    {
        /// <summary>
        ///  构造
        /// </summary>
        public EventDefaultEntityBusinessLogic(IServiceProvider provider)
        {
            ServiceProvider = provider;
        }

        protected sealed override DataAccess<EventDefaultEntity> CreateAccess()
        {
            return ServiceProvider.CreateDataAccess<EventDefaultEntity>();
        }
        #region 基础继承

        /*// <summary>
        ///     保存前的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool OnSaving(EventDefaultEntity data, bool isAdd)
        {
             return base.OnSaving(data, isAdd);
        }

        /// <summary>
        ///     保存完成后的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool OnSaved(EventDefaultEntity data, bool isAdd)
        {
             return base.OnSaved(data, isAdd);
        }

        /// <summary>
        ///     被用户编辑的数据的保存前操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool LastSavedByUser(EventDefaultEntity data, bool isAdd)
        {
            return base.LastSavedByUser(data, isAdd);
        }

        /// <summary>
        ///     被用户编辑的数据的保存前操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool PrepareSaveByUser(EventDefaultEntity data, bool isAdd)
        {
            return base.PrepareSaveByUser(data, isAdd);
        }*/
        #endregion


    }
}