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
using ZeroTeam.MessageMVC.Context;
using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// ��������״̬��ҵ���߼�����
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TAccess">���ݷ��ʶ���</typeparam>
    /// <typeparam name="TPrimaryKey">��������</typeparam>
    public class BusinessLogicByStateData<TData, TPrimaryKey, TAccess>
        : UiBusinessLogicBase<TData, TPrimaryKey, TAccess>, IBusinessLogicByStateData<TData, TPrimaryKey>
        where TData : EditDataObject, IIdentityData<TPrimaryKey>, IStateData, new()
        where TAccess : class, IStateDataTable<TData>, new()
    {
        #region ����״̬�߼�

        /// <summary>
        ///     �Ƿ����ִ�б������
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected override bool CanSave(TData data, bool isAdd)
        {
            if (!base.CanSave(data, isAdd))
                return false;
            if (!data.IsFreeze)
                return true;
            GlobalContext.Current.Status.LastMessage = "����������";
            GlobalContext.Current.Status.LastState = OperatorStatusCode.ArgumentError;
            return false;
        }

        /// <summary>
        ///     ɾ������ǰ�ô���
        /// </summary>
        protected override bool PrepareDelete(TPrimaryKey id)
        {
            if (Access.Any(p => id.Equals(p.Id ) && !p.IsFreeze))
                return base.PrepareDelete(id);
            GlobalContext.Current.Status.LastMessage = "����������";
            GlobalContext.Current.Status.LastState = OperatorStatusCode.ArgumentError;
            return false;
        }

        /// <summary>
        ///     ɾ��������ô���
        /// </summary>
        protected override void OnDeleted(TPrimaryKey id)
        {
            OnStateChanged(id, BusinessCommandType.Delete);
            base.OnDeleted(id);
        }

        /// <summary>
        ///     ɾ���������
        /// </summary>
        protected override bool DoDelete(TPrimaryKey id)
        {
            if (Access.Any(p => p.DataState == DataStateType.Delete && Equals(p.Id , id)))
                return Access.PhysicalDelete(id);
            return Access.DeletePrimaryKey(id);
        }

        #endregion

        #region ����״̬�޸�

        /// <summary>
        ///     ������ɺ�Ĳ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected override bool LastSaved(TData data, bool isAdd)
        {
            OnStateChanged(data, isAdd ? BusinessCommandType.AddNew : BusinessCommandType.Update);
            return base.LastSaved(data, isAdd);
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
        protected override void OnStateChanged(TData data, BusinessCommandType cmd)
        {
            if (!unityStateChanged)
                return;
            var old = Access.NoInjection;
            Access.NoInjection = true;
            try
            {
                DoStateChanged(data);
                OnInnerCommand(data, cmd);
            }
            finally
            {
                Access.NoInjection = old;
            }
        }

        /// <summary>
        ///     �ڲ�����ִ����ɺ�Ĵ���
        /// </summary>
        /// <param name="id">����</param>
        /// <param name="cmd">����</param>
        protected sealed override void OnStateChanged(TPrimaryKey id, BusinessCommandType cmd)
        {
            if (!unityStateChanged)
                return;
            var data = Access.LoadByPrimaryKey(id);
            if (data == null)
                return;
            OnStateChanged(data,cmd);
        }

        /// <summary>
        ///     ״̬�ı���ͳһ����(unityStateChanged������Ϊtrueʱ�����������--�������ܵĿ���)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual void DoStateChanged(TData data)
        {
        }

        #endregion

        #region ״̬����

        /// <summary>
        ///     ��������״̬
        /// </summary>
        /// <param name="id"></param>
        public virtual bool Reset(TPrimaryKey id)
        {
            if (!Access.ResetState(id))
                return false;
            OnStateChanged(id, BusinessCommandType.SetState);
            return true;
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual bool Enable(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Enable, true,
                p => Equals(p.Id , id) && (p.DataState == DataStateType.Disable || p.DataState == DataStateType.None));
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual bool Disable(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Disable, true,
                p => Equals(p.Id , id) && (p.DataState == DataStateType.Enable || p.DataState == DataStateType.None));
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual bool Discard(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Discard, true, p => Equals(p.Id , id) && p.DataState == DataStateType.None);
        }

        /// <summary>
        ///     �޸�״̬
        /// </summary>
        protected bool SetDataState(TPrimaryKey id, DataStateType state, bool isFreeze, Expression<Func<TData, bool>> filter)
        {
            if (filter != null && !Access.Any(filter))
                return false;
            if (filter == null && !Access.ExistPrimaryKey(id))
                return false;
            Access.SetState(state, isFreeze, id);
            using var scope = TransactionScope.CreateScope(Access.DataBase);
            Access.SetValue(p => p.IsFreeze, isFreeze, id);
            Access.SetValue(p => p.DataState, state, id);
            OnStateChanged(id, BusinessCommandType.SetState);
            return scope.Succeed();
        }

        #endregion
    }

    /// <summary>
    /// ��������״̬��ҵ���߼�����
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TAccess">���ݷ��ʶ���</typeparam>
    public class BusinessLogicByStateData<TData, TAccess>
        : BusinessLogicByStateData<TData,long, TAccess>
        where TData : EditDataObject, IIdentityData<long>, IStateData, new()
        where TAccess : class, IStateDataTable<TData>, new()
    {
    }
}