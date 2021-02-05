/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/7 0:49:52*/
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


using Zeroteam.MessageMVC.EventBus.DataAccess;
#endregion

namespace Zeroteam.MessageMVC.EventBus
{
    /// <summary>
    /// 事件订阅
    /// </summary>
    public partial class EventSubscribeEntityBusinessLogic : BusinessLogicByStateData<EventSubscribeEntity,long>
    {
        /// <summary>
        ///  生成数据访问对象
        /// </summary>
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