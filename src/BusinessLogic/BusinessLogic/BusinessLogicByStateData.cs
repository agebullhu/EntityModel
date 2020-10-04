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
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// ��������״̬��ҵ���߼�����
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TPrimaryKey">��������</typeparam>
    public abstract class BusinessLogicByStateData<TData, TPrimaryKey> : BusinessLogicBase<TData, TPrimaryKey>
        where TData : class, IIdentityData<TPrimaryKey>, IStateData, new()
    {
        #region ����״̬�߼�

        /// <summary>
        ///     �Ƿ����ִ�б������
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected override async Task<bool> CanSave(TData data, bool isAdd)
        {
            if (!await base.CanSave(data, isAdd))
                return false;
            if (!data.IsFreeze)
                return true;
            Context.LastMessage = "����������";
            Context.LastState = Context.ArgumentError;
            return false;
        }

        /// <summary>
        ///     ɾ������ǰ�ô���
        /// </summary>
        protected override async Task<bool> PrepareDelete(TPrimaryKey id)
        {
            if (await Access.AnyAsync(p => id.Equals(p.Id) && !p.IsFreeze))
                return await base.PrepareDelete(id);
            Context.LastMessage = "����������";
            Context.LastState = Context.ArgumentError;
            return false;
        }

        /// <summary>
        ///     ɾ��������ô���
        /// </summary>
        protected override async Task OnDeleted(TPrimaryKey id)
        {
            await OnStateChanged(id, BusinessCommandType.Delete);
            await base.OnDeleted(id);
        }

        /// <summary>
        ///     ɾ���������
        /// </summary>
        protected override async Task<bool> DoDelete(TPrimaryKey id)
        {
            if (await Access.AnyAsync(p => p.DataState == DataStateType.Delete && Equals(p.Id, id)))
                return await Access.PhysicalDeleteAsync(id);
            return await Access.DeletePrimaryKeyAsync(id);
        }

        #endregion

        #region ����״̬�޸�

        /// <summary>
        ///     ������ɺ�Ĳ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected override async Task<bool> LastSaved(TData data, bool isAdd)
        {
            await OnStateChanged(data, isAdd ? BusinessCommandType.AddNew : BusinessCommandType.Update);
            return await base.LastSaved(data, isAdd);
        }

        /// <summary>
        /// �Ƿ�ͳһ����״̬�仯
        /// </summary>
        protected bool unityStateChanged = false;

        /// <summary>
        ///     ״̬�ı���ͳһ����(unityStateChanged������Ϊtrueʱ�����������--�������ܵĿ���)
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="cmd">����</param>
        protected override async Task OnStateChanged(TData data, BusinessCommandType cmd)
        {
            if (!unityStateChanged)
                return;
            var old = Access.Option.NoInjection;
            Access.Option.NoInjection = true;
            try
            {
                await DoStateChanged(data);
                await OnInnerCommand(data, cmd);
            }
            finally
            {
                Access.Option.NoInjection = old;
            }
        }

        /// <summary>
        ///     �ڲ�����ִ����ɺ�Ĵ���
        /// </summary>
        /// <param name="id">����</param>
        /// <param name="cmd">����</param>
        protected sealed override async Task OnStateChanged(TPrimaryKey id, BusinessCommandType cmd)
        {
            if (!unityStateChanged)
                return;
            var data = await Access.LoadByPrimaryKeyAsync(id);
            if (data == null)
                return;
            await OnStateChanged(data, cmd);
        }

        /// <summary>
        ///     ״̬�ı���ͳһ����(unityStateChanged������Ϊtrueʱ�����������--�������ܵĿ���)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task DoStateChanged(TData data) => Task.CompletedTask;

        #endregion

        #region ״̬����

        /// <summary>
        ///     ��������״̬
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task<bool> Reset(TPrimaryKey id)
        {
            if (!await ResetState(id))
                return false;
            await OnStateChanged(id, BusinessCommandType.SetState);
            return true;
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual Task<bool> Enable(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Enable, true,
                p => Equals(p.Id, id) && (p.DataState == DataStateType.Disable || p.DataState == DataStateType.None));
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual Task<bool> Disable(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Disable, true,
                p => Equals(p.Id, id) && (p.DataState == DataStateType.Enable || p.DataState == DataStateType.None));
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual Task<bool> Discard(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Discard, true, p => Equals(p.Id, id) && p.DataState == DataStateType.None);
        }

        /// <summary>
        ///     �޸�״̬
        /// </summary>
        protected async Task<bool> SetDataState(TPrimaryKey id, DataStateType state, bool isFreeze, Expression<Func<TData, bool>> filter)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            if (filter != null && !await Access.AnyAsync(filter))
                return false;
            if (filter == null && !await Access.ExistPrimaryKeyAsync(id))
                return false;
            await SetState(state, isFreeze, id);
            await OnStateChanged(id, BusinessCommandType.SetState);
            await connectionScope.Commit();
            return true;
        }

        #endregion

        #region MyRegion

        /// <summary>
        ///     ɾ����SQL���
        /// </summary>
        protected string DeleteSqlCode => $@"UPDATE {Access.Option.ReadTableName} SET 
`{Access.Option.FieldMap[nameof(IStateData.DataState)]}`=255";

        /// <summary>
        ///     ����״̬��SQL���
        /// </summary>
        protected string ResetStateFileSqlCode(int state = 0, int isFreeze = 0) => $@"
{Access.Option.FieldMap[nameof(IStateData.DataState)]}={state},
{Access.Option.FieldMap[nameof(IStateData.IsFreeze)]}={isFreeze}";


        /// <summary>
        /// �޸�״̬
        /// </summary>
        protected virtual async Task<bool> SetState(DataStateType state, bool isFreeze, TPrimaryKey id)
        {
            var para = Access.ParameterCreater.CreateParameter(Access.Option.PrimaryKey,
                id,
                Access.SqlBuilder.GetDbType(Access.Option.PrimaryKey));


            var sql = $@"UPDATE `{Access.Option.ReadTableName}` 
SET {ResetStateFileSqlCode((int)state, isFreeze ? 1 : 0)} 
WHERE {Access.SqlBuilder.PrimaryKeyCondition}";
            return await Access.DataBase.ExecuteAsync(sql, para) == 1;
        }


        /// <summary>
        /// ����״̬
        /// </summary>
        protected virtual async Task<bool> ResetState(TPrimaryKey id)
        {
            var para = Access.ParameterCreater.CreateParameter(Access.Option.PrimaryKey,
                id,
                Access.SqlBuilder.GetDbType(Access.Option.PrimaryKey));

            var sql = $@"UPDATE `{Access.Option.ReadTableName}` 
SET {ResetStateFileSqlCode()} 
WHERE {Access.SqlBuilder.PrimaryKeyCondition}";

            return await Access.DataBase.ExecuteAsync(sql, para) == 1;
        }

        /// <summary>
        /// ����״̬
        /// </summary>
        protected virtual async Task<bool> ResetState(Expression<Func<TData, bool>> lambda)
        {
            var convert = Access.SqlBuilder.Compile(lambda);
            var sql = $@"UPDATE `{Access.Option.ReadTableName}` 
SET {ResetStateFileSqlCode()} 
WHERE {convert.ConditionSql}";

            return await Access.DataBase.ExecuteAsync(sql, convert.Parameters) > 0;
        }

        #endregion
    }

    /// <summary>
    /// ��������״̬��ҵ���߼�����
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    public abstract class BusinessLogicByStateData<TData> : BusinessLogicByStateData<TData, long>
        where TData : class, IIdentityData<long>, IStateData, new()
    {
    }
}