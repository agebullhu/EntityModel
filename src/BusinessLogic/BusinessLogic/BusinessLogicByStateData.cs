/*****************************************************
(c)2016-2021 by ZeroTeam
����: ����ˮ
����: Agebull.EntityModel.CoreAgebull.DataModel
����: ��������
�޸�: -
*****************************************************/

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
        ///     ɾ������ǰ�ô���
        /// </summary>
        protected override async Task<bool> DoDelete(TPrimaryKey id)
        {
            if (await Access.AnyAsync(p => p.DataState == DataStateType.Delete && p.Id.Equals(id)))
                return await Access.PhysicalDeleteAsync(id);
            return await Access.DeletePrimaryKeyAsync(id);
        }

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
            await OnCommandSuccess(default, id, DataOperatorType.SetState);
            return true;
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual Task<bool> Enable(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Enable, true,
                p => p.Id.Equals(id) && (p.DataState == DataStateType.Disable || p.DataState == DataStateType.None));
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual Task<bool> Disable(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Disable, true,
                p => p.Id.Equals(id) && (p.DataState == DataStateType.Enable || p.DataState == DataStateType.None));
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual Task<bool> Discard(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Discard, true, p => p.Id.Equals(id) && p.DataState == DataStateType.None);
        }

        /// <summary>
        ///     �޸�״̬
        /// </summary>
        protected async Task<bool> SetDataState(TPrimaryKey id, DataStateType state, bool isFreeze, Expression<Func<TData, bool>> filter)
        {
            using var levelScope = Access.InjectionScope(InjectionLevel.NotCondition);
            if (filter != null && !await Access.AnyAsync(filter))
                return false;
            await SetState(state, isFreeze, id);
            var cmd = state switch
            {
                DataStateType.Enable => DataOperatorType.Enable,
                DataStateType.Disable => DataOperatorType.Disable,
                DataStateType.Discard => DataOperatorType.Discard,
                DataStateType.None => DataOperatorType.Reset,
                _ => DataOperatorType.SetState,
            };
            if (cmd != DataOperatorType.SetState && Access.Provider.Option.CanRaiseEvent)
                await OnCommandSuccess(await Access.LoadByPrimaryKeyAsync(id), id, cmd);
            return true;
        }
        #endregion

        #region �޸�״̬

        /// <summary>
        /// �޸�״̬
        /// </summary>
        protected async Task<bool> SetState(DataStateType state, bool isFreeze, TPrimaryKey id)
        {
            using var levelScope = Access.InjectionScope(InjectionLevel.NotCondition);
            return await Access.SetValueAsync(id,
                (nameof(IStateData.DataState), state),
                ("is_freeze", isFreeze)) == 1;
        }

        /// <summary>
        /// �޸�״̬
        /// </summary>
        protected async Task<bool> SetState(DataStateType state, bool isFreeze, Expression<Func<TData, bool>> lambda)
        {
            using var levelScope = Access.InjectionScope(InjectionLevel.NotCondition);
            return await Access.SetValueAsync(lambda,
                (nameof(IStateData.DataState), state),
                ("is_freeze", isFreeze)) == 1;
        }



        /// <summary>
        /// ����״̬
        /// </summary>
        public Task<bool> ResetState(TPrimaryKey id)
        {
            return SetState(DataStateType.None, false, id);
        }

        /// <summary>
        /// ����״̬
        /// </summary>
        public Task<bool> ResetState(Expression<Func<TData, bool>> lambda)
        {
            return SetState(DataStateType.None, false, lambda);
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