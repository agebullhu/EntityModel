// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

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
    ///     Sqlʵ�������
    /// </summary>
    public sealed class DataInterfaceFeatureInjection : ISqlInjection
    {
        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public DataBaseType DataBaseType => DataBaseType.MySql;

        /// <summary>
        ///     �õ�����ȷƴ�ӵ�SQL������䣨������û�У�
        /// </summary>
        /// <param name="provider">��ǰ���ݲ���������</param>
        /// <param name="conditions">���ӵ���������</param>
        /// <returns></returns>
        public void InjectionQueryCondition<TEntity>(DataAccessProvider<TEntity> provider, List<string> conditions)
            where TEntity : class, new()
        { }

        /// <summary>
        ///     ע�����ݲ������
        /// </summary>
        /// <param name="provider">��ǰ���ݲ���������</param>
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
        ///     ע�����ݸ��´���
        /// </summary>
        /// <param name="provider">��ǰ���ݲ���������</param>
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
        ///     ע�����ݸ��´���
        /// </summary>
        /// <param name="provider">��ǰ���ݲ���������</param>
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