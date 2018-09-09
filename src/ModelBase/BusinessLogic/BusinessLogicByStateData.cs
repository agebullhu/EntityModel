// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.Linq.Expressions;
using Agebull.Common.Logging;
using Agebull.Common.WebApi;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;

#endregion

namespace Agebull.Common.DataModel.BusinessLogic
{
    /// <summary>
    /// ��������״̬��ҵ���߼�����
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TAccess">���ݷ��ʶ���</typeparam>
    /// <typeparam name="TDatabase">���ݿ����</typeparam>
    public class BusinessLogicByStateData<TData, TAccess, TDatabase>
        : UiBusinessLogicBase<TData, TAccess, TDatabase>
        where TData : EditDataObject, IIdentityData, IStateData, new()
        where TAccess : DataStateTable<TData, TDatabase>, new()
        where TDatabase : MySqlDataBase
    {
        #region ��������


        /// <summary>
        ///     ���ö���
        /// </summary>
        public bool Enable(string sels)
        {
            return DoByIds(sels, Enable);
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public bool Disable(string sels)
        {
            return DoByIds(sels, Disable);
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public bool Lock(string sels)
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
            if (Access.Any(p => p.Id == id && (p.IsFreeze || p.DataState == DataStateType.Disable || p.DataState == DataStateType.Enable)))
                return false;
            return base.PrepareDelete(id);
        }
        /// <summary>
        ///     ɾ���������
        /// </summary>
        protected override bool DoDelete(long id)
        {
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                if (!Access.Any(p => p.Id == id && p.DataState == DataStateType.Delete))
                    return Access.SetValue(p => p.DataState, DataStateType.Delete, p => p.Id == id && p.DataState == DataStateType.None) > 0;
                //if (BusinessContext.Context.CanDoCurrentPageAction("physical_delete"))
                //    return Access.PhysicalDelete(id);
                //BusinessContext.Context.LastMessage = "����������ִ������ɾ������";
                return false;
            }
        }

        /// <summary>
        ///     ɾ��������ô���
        /// </summary>
        protected override void OnDeleted(long id)
        {
            if (unityStateChanged)
            {
                var data = Access.LoadData(id);
                OnStateChanged(data, BusinessCommandType.Delete);
            }
            base.OnDeleted(id);
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
            if (unityStateChanged)
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
        protected void OnStateChanged(TData data, BusinessCommandType cmd)
        {
            if (!unityStateChanged) return;
            LogRecorder.MonitorTrace("OnStateChanged");
            OnInnerCommand(data, cmd);
            DoStateChanged(data);
        }

        /// <summary>
        ///     �ڲ�����ִ����ɺ�Ĵ���
        /// </summary>
        /// <param name="id">����</param>
        /// <param name="cmd">����</param>
        protected override void OnInnerCommand(long id, BusinessCommandType cmd)
        {
            if (!unityStateChanged)
                return;
            OnInnerCommand(Access.LoadByPrimaryKey(id), cmd);
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
            if (!DoResetState(data))
                return false;
            Access.Update(data);
            if (unityStateChanged)
                OnStateChanged(data, BusinessCommandType.Reset);
            return true;
        }

        /// <summary>
        ///     ��������״̬
        /// </summary>
        /// <param name="data"></param>
        protected virtual bool DoResetState(TData data)
        {
            data.DataState = DataStateType.None;
            data.IsFreeze = false;
            return true;
        }

        /// <summary>
        ///     ��������״̬
        /// </summary>
        /// <param name="id"></param>
        public virtual bool Reset(long id)
        {
            return SetDataState(id, DataStateType.None, p => p.Id == id);
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual bool Enable(long id)
        {
            if (Access.LoadValue(p => p.IsFreeze, id))
            {
                Access.SetValue(p => p.IsFreeze, false, id);
                return true;
            }
            return SetDataState(id, DataStateType.Enable, p => p.Id == id && (p.DataState == DataStateType.Disable || p.DataState == DataStateType.None));
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual bool Disable(long id)
        {
            return SetDataState(id, DataStateType.Disable, p => p.Id == id && p.DataState == DataStateType.Enable);
        }
        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual bool Discard(long id)
        {
            return SetDataState(id, DataStateType.Discard, p => p.Id == id && p.DataState == DataStateType.None);
        }
        /// <summary>
        ///     ��������
        /// </summary>
        public virtual bool Lock(long id)
        {
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                if (!Access.Any(p => p.Id == id && p.DataState < DataStateType.Discard && !p.IsFreeze))
                    return false;
                Access.SetValue(p => p.IsFreeze, true, id);
                Access.SetValue(p => p.DataState, DataStateType.Enable, id);
                if (!unityStateChanged)
                    return true;
                OnStateChanged(Access.LoadByPrimaryKey(id), BusinessCommandType.Lock);
                return true;
            }
        }

        /// <summary>
        ///     �޸�״̬
        /// </summary>
        protected bool SetDataState(long id, DataStateType state, Expression<Func<TData, bool>> filter)
        {
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                if (!Access.ExistPrimaryKey(id) || !Access.Any(filter))
                    return false;
                Access.SetValue(p => p.DataState, state, id);
                if (!unityStateChanged)
                    return true;
                OnStateChanged(Access.LoadByPrimaryKey(id), BusinessCommandType.SetState);
                return true;
            }
        }
        #endregion
    }
}