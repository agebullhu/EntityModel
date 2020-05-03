using Agebull.Common;
using Agebull.EntityModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using ZeroTeam.MessageMVC.Context;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 默认的数据更新触发器
    /// </summary>
    public class DefaultDataUpdateTrigger : IDataTrigger
    {
        #region 接口实现的侵入准备

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => DataBaseType.Full;


        #endregion

        void IDataUpdateTrigger.OnPrepareSave<TEntity>(TEntity entity, DataOperatorType operatorType)
        {
            if (operatorType != DataOperatorType.Insert)
            {
                return;
            }
            if (entity is ISnowFlakeId flakeId && flakeId.Id <= 0)
            {
                flakeId.Id = SnowFlake.NewId;
            }
            if (entity is IAuthorData authorData)
            {
                authorData.AddDate = DateTime.Now;
                authorData.Author = GlobalContext.Current.User.NickName;
                authorData.AuthorId = GlobalContext.Current.User.UserId;
            }
            if (entity is IHistoryData historyData)
            {
                historyData.LastModifyDate = DateTime.Now;
                historyData.LastReviser = GlobalContext.Current.User.NickName;
                historyData.LastReviserId = GlobalContext.Current.User.UserId;
            }
        }

        void IDataUpdateTrigger.OnDataSaved<TEntity>(TEntity entity, DataOperatorType operatorType)
        {
            if (operatorType != DataOperatorType.Update)
            {
                return;
            }
            if (entity is IHistoryData historyData)
            {
                historyData.LastModifyDate = DateTime.Now;
                historyData.LastReviser = GlobalContext.Current.User.NickName;
                historyData.LastReviserId = GlobalContext.Current.User.UserId;
            }
        }
    }
}