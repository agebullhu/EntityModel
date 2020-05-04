// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

#endregion

using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ZeroTeam.MessageMVC.Context;

namespace Agebull.EntityModel.SqlServer
{
    /// <summary>
    /// ����״̬����
    /// </summary>
    /// <typeparam name="TData">ʵ��</typeparam>
    /// <typeparam name="TSqlServerDataBase">���ڵ����ݿ����,��ͨ��Ioc�Զ�����</typeparam>
    public abstract class DataStateTable<TData, TSqlServerDataBase> : SqlServerTable<TData, TSqlServerDataBase>, IStateDataTable<TData>
        where TData : EditDataObject, IStateData, IIdentityData, new()
        where TSqlServerDataBase : SqlServerDataBase
    {
        static DataStateTable()
        {
            DependencyHelper.ServiceCollection.TryAddSingleton<IDataTrigger, DataStateTrigger>();
        }

        /// <summary>
        ///     ɾ����SQL���
        /// </summary>
        protected sealed override string DeleteSqlCode => $@"UPDATE `{ContextWriteTable}` SET `{FieldDictionary[nameof(IStateData.DataState)]}`=255";

        /// <summary>
        ///     ����״̬��SQL���
        /// </summary>
        protected virtual string ResetStateFileSqlCode(int state = 0, int isFreeze = 0) =>
            $@"`{FieldDictionary[nameof(IStateData.DataState)]}`={state},`{FieldDictionary[nameof(IStateData.IsFreeze)]}`={isFreeze}";

        /// <summary>
        ///     �õ�����ȷƴ�ӵ�SQL������䣨������û�У�
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        protected override void ContitionSqlCode(List<string> conditions)
        {
            if (GlobalContext.Current.Status.IsManageMode)
                return;
            conditions.Add($"`{FieldDictionary[nameof(IStateData.DataState)]}` < 255");
        }

        /// <summary>
        ///     �õ�����ȷ���µ�����
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected override void CheckUpdateContition(ref string condition)
        {
            if (GlobalContext.Current.Status.IsManageMode)
                return;
            if (condition == null)
                condition = $"`{FieldDictionary[nameof(IStateData.IsFreeze)]}` = 0";
            else
                condition = $"`{FieldDictionary[nameof(IStateData.IsFreeze)]}` = 0 AND ({condition})";
        }

        /// <summary>
        /// �޸�״̬
        /// </summary>
        public virtual bool SetState(DataStateType state, bool isFreeze, long id)
        {
            var sql = $@"UPDATE `{ContextWriteTable}` 
SET {ResetStateFileSqlCode((int)state, isFreeze ? 1 : 0)} 
WHERE {PrimaryKeyConditionSQL}";
            return DataBase.Execute(sql, CreatePimaryKeyParameter(id)) == 1;
        }


        /// <summary>
        /// ����״̬
        /// </summary>
        public virtual bool ResetState(long id)
        {
            var sql = $@"UPDATE `{ContextWriteTable}` 
SET {ResetStateFileSqlCode()} 
WHERE {PrimaryKeyConditionSQL}";

            return DataBase.Execute(sql, CreatePimaryKeyParameter(id)) == 1;
        }

        /// <summary>
        /// ����״̬
        /// </summary>
        public virtual bool ResetState(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            var sql = $@"UPDATE `{ContextWriteTable}` 
SET {ResetStateFileSqlCode()} 
WHERE {convert.ConditionSql}";
            return DataBase.Execute(sql, convert.Parameters) > 0;
        }
    }
}