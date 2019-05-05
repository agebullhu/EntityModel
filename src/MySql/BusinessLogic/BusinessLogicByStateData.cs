// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Agebull.Common.Logging;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.MySql;

#endregion

namespace Agebull.EntityModel.BusinessLogic.MySql
{
    /// <summary>
    /// ��������״̬��ҵ���߼�����
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TAccess">���ݷ��ʶ���</typeparam>
    /// <typeparam name="TDatabase">���ݿ����</typeparam>
    public class BusinessLogicByStateData<TData, TAccess, TDatabase>
        : UiBusinessLogicBase<TData, TAccess, TDatabase>, IBusinessLogicByStateData<TData>
        where TData : EditDataObject, IIdentityData, IStateData, new()
        where TAccess : DataStateTable<TData, TDatabase>, new()
        where TDatabase : MySqlDataBase
    {
        #region ��������


        /// <summary>
        ///     ���ö���
        /// </summary>
        public bool Enable(IEnumerable<long> sels)
        {
            return DoByIds(sels, Enable);
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public bool Disable(IEnumerable<long> sels)
        {
            return DoByIds(sels, Disable);
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public bool Lock(IEnumerable<long> sels)
        {
            return DoByIds(sels, Lock);
        }
        #endregion

        #region ����״̬�߼�

        /// <summary>
        ///     �Ƿ����ִ�б������
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected override bool CanSave(TData data, bool isAdd)
        {
            return !data.IsFreeze && data.DataState < DataStateType.Discard;
        }

        /// <summary>
        ///     ɾ������ǰ�ô���
        /// </summary>
        protected override bool PrepareDelete(long id)
        {
            if (Access.Any(p => p.Id == id && p.IsFreeze))
                return false;
            return base.PrepareDelete(id);
        }
        /// <summary>
        ///     ɾ��������ô���
        /// </summary>
        protected override void OnDeleted(long id)
        {
            OnStateChanged(id, BusinessCommandType.Delete);
            base.OnDeleted(id);
        }

        /// <summary>
        ///     ɾ���������
        /// </summary>
        protected override bool DoDelete(long id)
        {
            if (Access.Any(p => p.DataState == DataStateType.Delete && p.Id == id))
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
        protected sealed override void OnStateChanged(TData data, BusinessCommandType cmd)
        {
            if (!unityStateChanged)
                return;
            LogRecorderX.MonitorTrace("OnStateChanged");
            OnInnerCommand(data, cmd);
            DoStateChanged(data);
        }

        /// <summary>
        ///     �ڲ�����ִ����ɺ�Ĵ���
        /// </summary>
        /// <param name="id">����</param>
        /// <param name="cmd">����</param>
        protected sealed override void OnStateChanged(long id, BusinessCommandType cmd)
        {
            if (!unityStateChanged)
                return;
            var data = Access.LoadByPrimaryKey(id);
            if (data != null)
                OnStateChanged(data, cmd);
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
        /// <param name="data"></param>
        public bool ResetState(TData data)
        {
            if (data == null)
                return false;
            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                //if (!DoResetState(data))
                //    return false;

                data.DataState = DataStateType.None;
                data.IsFreeze = false;
                Access.ResetState(data.Id);
                OnStateChanged(data, BusinessCommandType.Reset);
                return scope.SetState(true);
            }
        }

        ///// <summary>
        /////     ��������״̬
        ///// </summary>
        ///// <param name="data"></param>
        //protected virtual bool DoResetState(TData data)
        //{
        //    data.DataState = DataStateType.None;
        //    data.IsFreeze = false;
        //    return true;
        //}

        /// <summary>
        ///     ��������״̬
        /// </summary>
        /// <param name="id"></param>
        public virtual bool Reset(long id)
        {
            return SetDataState(id, DataStateType.None, null, false);
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual bool Enable(long id)
        {
            return SetDataState(id, DataStateType.Enable, p => p.Id == id && (p.DataState == DataStateType.Disable || p.DataState == DataStateType.None), true);
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual bool Disable(long id)
        {
            return SetDataState(id, DataStateType.Disable, p => p.Id == id && p.DataState == DataStateType.Enable, true);
        }
        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual bool Discard(long id)
        {
            return SetDataState(id, DataStateType.Discard, p => p.Id == id && p.DataState == DataStateType.None, true);
        }
        /// <summary>
        ///     ��������
        /// </summary>
        public virtual bool Lock(long id)
        {
            if (!Access.Any(p => p.Id == id && p.DataState < DataStateType.Discard && !p.IsFreeze))
                return false;
            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                Access.SetValue(p => p.IsFreeze, true, id);
                Access.SetValue(p => p.DataState, DataStateType.Disable, p => p.Id == id && p.DataState == DataStateType.None);

                OnStateChanged(id, BusinessCommandType.Lock);
                return scope.SetState(true);
            }
        }

        /// <summary>
        ///     �޸�״̬
        /// </summary>
        protected bool SetDataState(long id, DataStateType state, Expression<Func<TData, bool>> filter, bool? setFreeze)
        {
            if (filter != null && !Access.Any(filter))
                return false;
            if (filter == null && !Access.ExistPrimaryKey(id))
                return false;
            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                Access.SetValue(p => p.DataState, state, id);
                if (setFreeze != null)
                    Access.SetValue(p => p.IsFreeze, setFreeze.Value, id);
                OnStateChanged(id, BusinessCommandType.SetState);
                return scope.SetState(true);
            }
        }
        #endregion
    }
}