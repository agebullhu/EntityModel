using Agebull.Common;
using Agebull.EntityModel.Interfaces;
using System;
using ZeroTeam.MessageMVC.Context;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///特性数据更新触发器
    /// </summary>
    public class CharacterDataUpdateTrigger : IDataTrigger
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
                //authorData.Author = GlobalContext.Current.User.NickName;
                authorData.AuthorId = GlobalContext.Current.User.UserId;
            }
            if (entity is IHistoryData historyData)
            {
                historyData.LastModifyDate = DateTime.Now;
                //historyData.LastReviser = GlobalContext.Current.User.NickName;
                historyData.LastReviserId = GlobalContext.Current.User.UserId;
            }
            if (entity is IOrganizationData organizationData && GlobalContext.Current.User is IOrganizationData organizationUser)
            {
                organizationData.OrganizationId = organizationUser.OrganizationId;
            }
            if (entity is IDepartmentData departmentData && GlobalContext.Current.User is IDepartmentData departmentUser)
            {
                departmentData.DepartmentId = departmentUser.DepartmentId;
                departmentData.DepartmentCode = departmentUser.DepartmentCode;
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
                //historyData.LastReviser = GlobalContext.Current.User.NickName;
                historyData.LastReviserId = GlobalContext.Current.User.UserId;
            }
        }
    }
}