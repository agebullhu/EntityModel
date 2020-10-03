// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Agebull.EntityModel.MySql.GlobalDataInterfaces;
using IStateData = Agebull.EntityModel.MySql.GlobalDataInterfaces.IStateData;
#endregion

namespace Agebull.EntityModel.MySql
{

    /// <summary>
    ///     Sql实体访问类
    /// </summary>
    public sealed class DataInterfaceFeatureInjection : ISqlInjection
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => DataBaseType.MySql;

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="provider">当前数据操作适配器</param>
        /// <param name="conditions">附加的条件集合</param>
        /// <returns></returns>
        public void InjectionQueryCondition<TEntity>(DataAccessProvider<TEntity> provider, List<string> conditions)
            where TEntity : class, new()
        { }

        /// <summary>
        ///     注入数据插入代码
        /// </summary>
        /// <param name="provider">当前数据操作适配器</param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public void InjectionInsertCode<TEntity>(DataAccessProvider<TEntity> provider, StringBuilder fields, StringBuilder values)
            where TEntity : class, new()
        {
            if (provider.Option.DataStruct.InterfaceFeature == null || provider.Option.DataStruct.InterfaceFeature.Length == 0)
                return;
            if (provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IAuthorData)))
            {
                AddValue(fields, values, IAuthorData.AddDate, DateTime.Now);
                //AddValue(fields, values, IAuthorData.Author, GlobalContext.Current?.User.NickName);
                //AddValue(fields, values, IAuthorData.AuthorId, GlobalContext.Current?.User.UserId);
            }
            if (provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IHistoryData)))
            {
                AddValue(fields, values, IHistoryData.LastModifyDate, DateTime.Now);
                //AddValue(fields, values, IHistoryData.LastReviser, GlobalContext.Current?.User.NickName);
                //AddValue(fields, values, IHistoryData.LastReviserId, GlobalContext.Current?.User.UserId);
            }
            if (provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IStateData)))
            {
                AddValue(fields, values, IStateData.IsFreeze, 0);
                AddValue(fields, values, IStateData.DataState, 0);
            }
            if (provider.Option.DataStruct.InterfaceFeature.Contains(nameof(GlobalDataInterfaces.ILogicDeleteData)))
            {
                AddValue(fields, values, GlobalDataInterfaces.ILogicDeleteData.IsDeleted, 0);
            }
            if (provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IDepartmentData)))
            {
                //AddValue(fields, values, IDepartmentData.DepartmentId, GlobalContext.Current.User.OrganizationId);
                //AddValue(fields, values, IDepartmentData.DepartmentId, GlobalContext.Current?.User.OrganizationId);
            }
            if (provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IVersionData)))
            {
                AddValue(fields, values, IVersionData.DataVersion, DateTime.Now.Ticks);
            }
        }

        void AddValue<T>(StringBuilder fields, StringBuilder values, PropertyDefault field, T value)
        {
            fields.Append($",`{field.FieldName}`");
            values.Append($",'{value}'");
        }

        void AddValue(StringBuilder fields, StringBuilder values, PropertyDefault field, DateTime value)
        {
            fields.Append($",`{field.FieldName}`");
            values.Append($",'{value:yyyy-MM-dd HH:mm:ss}'");
        }

        /// <summary>
        ///     注入数据更新代码
        /// </summary>
        /// <param name="provider">当前数据操作适配器</param>
        /// <param name="condition"></param>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public void InjectionUpdateCode<TEntity>(DataAccessProvider<TEntity> provider, StringBuilder valueExpression, List<string> condition)
            where TEntity : class, new()
        {
            if (provider.Option.DataStruct.InterfaceFeature == null || provider.Option.DataStruct.InterfaceFeature.Length == 0)
                return;
            if (provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IHistoryData)))
            {
                SetValue(valueExpression, IHistoryData.LastModifyDate, DateTime.Now);
                //SetValue(valueExpression, IHistoryData.LastReviser, GlobalContext.Current?.User.NickName);
                //SetValue(valueExpression, IHistoryData.LastReviserId, GlobalContext.Current?.User.UserId);
            }
            if (provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IVersionData)))
            {
                SetValue(valueExpression, IVersionData.DataVersion, DateTime.Now.Ticks);
            }
            if (provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IStateData)))
            {
                condition.Add($"{IStateData.IsFreeze.FieldName} = 0");
                condition.Add($"{IStateData.DataState.FieldName} < 255");
            }
            if (provider.Option.DataStruct.InterfaceFeature.Contains(nameof(GlobalDataInterfaces.ILogicDeleteData)))
            {
                condition.Add($"{GlobalDataInterfaces.ILogicDeleteData.IsDeleted.FieldName} = 0");
            }
        }

        void SetValue(StringBuilder valueExpression, PropertyDefault field, DateTime value)
        {
            valueExpression.Append($",`{field.FieldName}` = '{value:yyyy-MM-dd HH:mm:ss}'");
        }

        void SetValue<T>(StringBuilder valueExpression, PropertyDefault field, T value)
        {
            valueExpression.Append($",`{field.FieldName}` = '{value}'");
        }
        /// <summary>
        ///     注入数据更新代码
        /// </summary>
        /// <param name="provider">当前数据操作适配器</param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public void InjectionDeleteCondition<TEntity>(DataAccessProvider<TEntity> provider, List<string> condition)
            where TEntity : class, new()
        {
            if (provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IStateData)))
            {
                condition.Add($"{IStateData.IsFreeze.FieldName} = 0");
                condition.Add($"{IStateData.DataState.FieldName} < 255");
            }
            if (provider.Option.DataStruct.InterfaceFeature.Contains(nameof(GlobalDataInterfaces.ILogicDeleteData)))
            {
                condition.Add($"{GlobalDataInterfaces.ILogicDeleteData.IsDeleted.FieldName} = 0");
            }
        }
    }
}