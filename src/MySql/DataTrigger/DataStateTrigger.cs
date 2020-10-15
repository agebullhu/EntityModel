using Agebull.EntityModel.Common;
using Agebull.EntityModel.Events;
using Agebull.EntityModel.Interfaces;
using Agebull.EntityModel.Permissions;
using System.Collections.Generic;
using System.Text;
using ZeroTeam.MessageMVC.Context;

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    /// 版本数据更新触发器
    /// </summary>
    internal class DataStateTrigger : IDataTrigger
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => DataBaseType.MySql;

        void IDataUpdateTrigger.AfterUpdateSql<TEntity>(IDataTable<TEntity> table, string condition, StringBuilder code)
        {
            bool hase = false;
            StringBuilder innerCode = new StringBuilder();
            if (DataUpdateHandler.IsType<TEntity>(DataUpdateHandler.TypeofIHistoryData))
            {
                innerCode.Append($@"
    `{table.FieldDictionary[nameof(IHistoryData.LastReviserId)]}` = '{GlobalContext.Current.User.UserId}',
    `{table.FieldDictionary[nameof(IHistoryData.LastModifyDate)]}` = Now()");
    //            var name = GlobalContext.Current.User.NickName?.Replace('\'', '’');
    //            innerCode.Append($@"
    //`{table.FieldDictionary[nameof(IHistoryData.LastReviserId)]}` = {GlobalContext.Current.User.UserId},
    //`{table.FieldDictionary[nameof(IHistoryData.LastReviser)]}` = '{name}',
    //`{table.FieldDictionary[nameof(IHistoryData.LastModifyDate)]}` = Now()");
                hase = true;
            }
    //        if (DataUpdateHandler.IsType<TEntity>(DataUpdateHandler.TypeofIOrganizationData) && GlobalContext.Current.User is IOrganizationData organizationData)
    //        {
    //            if (hase)
    //                code.Append(',');
    //            else
    //                hase = true;
    //            innerCode.Append($@"
    //`{table.FieldDictionary[nameof(IOrganizationData.OrganizationId)]}` = '{organizationData.OrganizationId}'");
    //        }
    //        if (DataUpdateHandler.IsType<TEntity>(DataUpdateHandler.TypeofIDepartmentData) && GlobalContext.Current.User is IDepartmentData departmentData)
    //        {
    //            if (hase)
    //                code.Append(',');
    //            else
    //                hase = true;
    //            innerCode.Append($@"
    //`{table.FieldDictionary[nameof(IDepartmentData.DepartmentId)]}` = '{departmentData.DepartmentId}',
    //`{table.FieldDictionary[nameof(IDepartmentData.DepartmentCode)]}` = '{departmentData.DepartmentCode}'");
    //        }

            if (hase)
            {
                code.AppendLine($@"
UPDATE `{ table.ContextWriteTable}` SET
{innerCode}");
                if (!string.IsNullOrEmpty(condition))
                {
                    code.Append($@"
WHERE {condition}");
                    code.AppendLine(";");
                }
            }
        }

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="table">当前数据操作对象</param>
        /// <param name="conditions">附加的条件集合</param>
        /// <returns></returns>
        void IDataUpdateTrigger.ConditionSqlCode<TEntity>(IDataTable<TEntity> table, List<string> conditions)
        {
            //是否启用数据范围限制
            if (!(GlobalContext.Current.User is IDepartmentScopeData scopeData) || scopeData.DataScope == DataScopeType.None || scopeData.DataScope == DataScopeType.Unlimited)
            {
                return;
            }
            StringBuilder code = new StringBuilder();
            bool first = true;

            //个人数据边界过滤
            if (DataUpdateHandler.IsType<TEntity>(DataUpdateHandler.TypeofIAuthorData) && scopeData.DataScope.HasFlag(DataScopeType.Person))
            {
                code.Append($"`{table.FieldDictionary[nameof(IAuthorData.AuthorId)]}` = '{GlobalContext.Current.User.UserId}'");
                first = false;
            }
            //部门数据边界过滤
            if (DataUpdateHandler.IsType<TEntity>(DataUpdateHandler.TypeofIDepartmentData) &&
                GlobalContext.Current.User is IDepartmentData departmentData && !string.IsNullOrEmpty(departmentData.DepartmentId))
            {
                if (scopeData.DataScope.HasFlag(DataScopeType.Lower))
                {
                    if (first)
                        first = false;
                    else
                        code.Append(" OR ");

                    if (!scopeData.DataScope.HasFlag(DataScopeType.Home))
                    {
                        code.Append($@"(`{table.FieldDictionary[nameof(IDepartmentData.DepartmentId)]}` <> '{departmentData.DepartmentId}' AND 
`{table.FieldDictionary[nameof(IDepartmentData.DepartmentCode)]}` LIKE '{departmentData.DepartmentCode}%')");
                    }
                    else
                    {
                        code.Append($"`{table.FieldDictionary[nameof(IDepartmentData.DepartmentCode)]}` LIKE '{departmentData.DepartmentCode}%'");
                    }
                }
                else if (scopeData.DataScope.HasFlag(DataScopeType.Home))
                {
                    if (first)
                        first = false;
                    else
                        code.Append(" OR ");
                    code.Append($"`{table.FieldDictionary[nameof(IDepartmentData.DepartmentId)]}` = '{departmentData.DepartmentId}'");
                }
            }
            //组织数据边界过滤
            if (DataUpdateHandler.IsType<TEntity>(DataUpdateHandler.TypeofIOrganizationData) &&
                GlobalContext.Current.User is IOrganizationData organizationData && !string.IsNullOrEmpty(organizationData.OrganizationId))
            {
                if (first)
                {
                    conditions.Add($"`{table.FieldDictionary[nameof(IOrganizationData.OrganizationId)]}` = '{organizationData.OrganizationId}'");
                }
                else
                {
                    conditions.Add($"`{table.FieldDictionary[nameof(IOrganizationData.OrganizationId)]}` = '{organizationData.OrganizationId}' AND ({code})");
                }
            }
        }
    }
}
