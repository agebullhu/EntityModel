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
        /// 类型接口实现的数字表示
        /// </summary>
        public static readonly Dictionary<Type, int> TypeInterfaces = new Dictionary<Type, int>();
        /// <summary>
        /// 表示
        /// </summary>
        public const int TypeofIAuthorData = 1;
        /// <summary>
        /// 表示
        /// </summary>
        public const int TypeofIHistoryData = 2;
        /// <summary>
        /// 表示
        /// </summary>
        public const int TypeofIOrganizationData = 4;
        /// <summary>
        /// 表示
        /// </summary>
        public const int TypeofIVersionData = 8;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => DataBaseType.Full;

        /// <summary>
        /// 初始化类型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        void IDataTrigger.InitType<TEntity>()
        {
            if (TypeInterfaces.TryGetValue(typeof(TEntity), out var type))
                return;
            var entity = new TEntity();
            type = 0;
            if (entity is IAuthorData)
            {
                type |= TypeofIAuthorData;
            }
            if (entity is IHistoryData)
            {
                type |= TypeofIHistoryData;
            }
            if (entity is IOrganizationData)
            {
                type |= TypeofIOrganizationData;
            }
            if (entity is IVersionData)
            {
                type |= TypeofIVersionData;
            }
            TypeInterfaces.Add(typeof(TEntity), type);
        }

        /// <summary>
        /// 是否指定类型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsType<TEntity>(int type)
        {
            return TypeInterfaces.TryGetValue(typeof(TEntity), out var def) && (type & def) == type;
        }

        /// <summary>
        /// 是否指定类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsType(object obj, int type)
        {
            return obj != null && TypeInterfaces.TryGetValue(obj.GetType(), out var def) && (type & def) == type;
        }

        #endregion

        #region SQL注入

        void IDataUpdateTrigger.BeforeUpdateSql<TEntity>(IDataTable<TEntity> table, string condition, StringBuilder code)
        {
        }

        void IDataUpdateTrigger.AfterUpdateSql<TEntity>(IDataTable<TEntity> table, string condition, StringBuilder code)
        {
        }
        #endregion

        void IDataUpdateTrigger.ContitionSqlCode<T>(List<string> conditions)
        {
        }

        void IDataUpdateTrigger.OnPrepareSave(EditDataObject entity, DataOperatorType operatorType)
        {
            if (operatorType == DataOperatorType.Insert)
            {
                if (entity is IAuthorData authorData)
                {
                    authorData.AddDate = DateTime.Now;
                    authorData.Author = GlobalContext.Current.User.NickName;
                    authorData.AuthorId = GlobalContext.Current.LoginUserId;
                }
                if (entity is IHistoryData historyData)
                {
                    historyData.LastModifyDate = DateTime.Now;
                    historyData.LastReviser = GlobalContext.Current.User.NickName;
                    historyData.LastReviserId = GlobalContext.Current.LoginUserId;
                }
            }
        }

        void IDataUpdateTrigger.OnOperatorExecuted(int entityId, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
        {

        }

        void IDataUpdateTrigger.OnOperatorExecuting(int entityId, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
        {

        }

        void IDataUpdateTrigger.OnDataSaved(EditDataObject entity, DataOperatorType operatorType)
        {
            if (operatorType == DataOperatorType.Update)
            {
                if (entity is IAuthorData authorData)
                {
                    authorData.AddDate = DateTime.Now;
                    authorData.Author = GlobalContext.Current.User.NickName;
                    authorData.AuthorId = GlobalContext.Current.LoginUserId;
                }
                if (entity is IHistoryData historyData)
                {
                    historyData.LastModifyDate = DateTime.Now;
                    historyData.LastReviser = GlobalContext.Current.User.NickName;
                    historyData.LastReviserId = GlobalContext.Current.LoginUserId;
                }
            }
        }
    }
}