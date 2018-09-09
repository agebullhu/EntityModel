// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using Agebull.Common.Rpc;
using Agebull.Common.WebApi;
using Gboxt.Common.DataModel.Extends;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.DataModel;

#endregion

namespace Agebull.Common.DataModel.BusinessLogic
{
    /// <summary>
    ///     ���������չ��ҵ���߼�����
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TAccess">���ݷ��ʶ���</typeparam>
    /// <typeparam name="TDatabase">���ݿ����</typeparam>
    public class BusinessLogicByAudit<TData, TAccess, TDatabase> 
        : BusinessLogicByHistory<TData, TAccess, TDatabase>
        where TData : EditDataObject, IIdentityData, IHistoryData, IAuditData, IStateData, new()
        where TAccess : HitoryTable<TData, TDatabase>, new()
        where TDatabase : MySqlDataBase
    {
        #region ��Ϣ

        /// <summary>
        ///     ȡ��У��(���ʱ��Ч)
        /// </summary>
        public bool CancelValidate { get; set; }

        protected virtual string unAuditMessageLock => "���������ѹ鵵�����ݣ����ܽ��з���ˣ�";

        protected virtual string unAuditMessageNoSubmit => "δͨ����˵����ݣ����ܽ��з���ˣ�";

        protected virtual string SubmitMessage => "����˽��������ݲ������ύ��";

        protected virtual string BackMessage => "�����ύδ��˵����ݿ��˻أ�";

        protected virtual string AuditMessageReDo => "�������˳�����ʮ���ӵ����ݣ��޷��ٴ���ˣ�";

        protected virtual string AuditMessage => "�����ύδ��˵����ݿɽ�����ˣ�";

        #endregion

        #region ��������

        /// <summary>
        ///     �����ύ
        /// </summary>
        public bool Submit(string sels)
        {
            return DoByIds(sels, SubmitInner);
        }

        /// <summary>
        ///     �����˻�
        /// </summary>
        public bool Back(string sels)
        {
            return DoByIds(sels, BackInner);
        }

        /// <summary>
        ///     ����ͨ��
        /// </summary>
        public bool AuditPass(string sels)
        {
            return DoByIds(sels, AuditPassInner);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        public bool Pullback(string sels)
        {
            return DoByIds(sels, PullbackInner);
        }

        /// <summary>
        ///     �������
        /// </summary>
        public bool AuditDeny(string sels)
        {
            return DoByIds(sels, AuditDenyInner);
        }

        /// <summary>
        ///     ���������
        /// </summary>
        public bool UnAudit(string sels)
        {
            return DoByIds(sels, UnAuditInner);
        }
        /// <summary>
        ///     ��������У��
        /// </summary>
        public bool Validate(string sels, Action<ValidateResult> putError)
        {
            return LoopIdsToData(sels, data => DoValidateInner(data, putError));
        }

        #endregion

        #region ��������

        /// <summary>
        ///     ������ȷ��У��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Validate(long id)
        {
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                return DoValidateInner(Details(id));
            }
        }

        /// <summary>
        ///     ���ͨ��
        /// </summary>
        public bool AuditPass(long id)
        {
            using (var scope = TransactionScope.CreateScope(Access.DataBase))
            {
                var data = Details(id);
                if (data == null)
                    return false;
                if (AuditPassInner(data))
                    scope.SetState(true);
            }
            return true;
        }

        /// <summary>
        ///     ��˲�ͨ��
        /// </summary>
        public bool AuditDeny(long id)
        {
            using (var scope = TransactionScope.CreateScope(Access.DataBase))
            {
                var data = Details(id);
                if (data == null)
                    return false;

                if (!AuditDenyInner(data))
                    return false;
                scope.SetState(true);
                return true;
            }
        }

        /// <summary>
        ///     �����
        /// </summary>
        public bool UnAudit(long id)
        {
            using (var scope = TransactionScope.CreateScope(Access.DataBase))
            {
                var data = Details(id);
                if (data == null)
                {
                    return false;
                }
                if (UnAuditInner(data))
                    scope.SetState(true);
            }
            return true;
        }

        /// <summary>
        ///     �ύ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Submit(long id)
        {
            using (var scope = TransactionScope.CreateScope(Access.DataBase))
            {
                var data = Details(id);
                if (data == null)
                {
                    return false;
                }
                if (SubmitInner(data))
                    scope.SetState(true);
            }
            return true;
        }

        /// <summary>
        ///     �˻ر༭
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Back(long id)
        {
            using (var scope = TransactionScope.CreateScope(Access.DataBase))
            {
                var data = Details(id);
                if (data == null)
                {
                    return false;
                }
                if (BackInner(data))
                    scope.SetState(true);
            }
            return true;
        }

        #endregion

        #region ״̬����

        /// <summary>
        ///     ɾ������ǰ�ô���
        /// </summary>
        protected override bool PrepareDelete(long id)
        {
            if (Access.Any(p => p.Id == id && p.AuditState != AuditStateType.None))
            {
                ApiContext.Current.LastMessage = "��δ�����κ���˲��������ݿ��Ա�ɾ��";
                return false;
            }
            return base.PrepareDelete(id);
        }

        /// <summary>
        ///     ��������״̬
        /// </summary>
        /// <param name="data"></param>
        protected override bool DoResetState(TData data)
        {
            if (data == null)
                return false;
            data.AuditState = AuditStateType.None;
            data.AuditDate = DateTime.MinValue;
            data.AuditorId = 0;
            return base.DoResetState(data);
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public override bool Reset(long id)
        {
            ResetState(Access.First(id));
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                if (!Access.Any(p => p.Id == id && p.AuditState != AuditStateType.Pass))
                {
                    Access.SetValue(p => p.IsFreeze, false, id);
                    return true;
                }
                return base.Reset(id);
            }
        }


        /// <summary>
        ///     ���ö���
        /// </summary>
        public override bool Disable(long id)
        {
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                if (!Access.Any(p => p.Id == id && p.AuditState != AuditStateType.Pass ))
                {
                    Access.SetValue(p => p.IsFreeze, false, id);
                    return true;
                }
                return base.Disable(id);
            }
        }
        /// <summary>
        ///     ���ö���
        /// </summary>
        public override bool Discard(long id)
        {
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                if (!Access.Any(p => p.Id == id && p.AuditState <= AuditStateType.Again))
                {
                    Access.SetValue(p => p.IsFreeze, false, id);
                    return true;
                }
                return base.Discard(id);
            }
        }
        /// <summary>
        ///     ���ö���
        /// </summary>
        public override bool Enable(long id)
        {
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                if (!Access.Any(p => p.Id == id && p.AuditState == AuditStateType.Pass))
                {
                    Access.SetValue(p => p.IsFreeze, false, id);
                    return true;
                }
                return base.Enable(id);
            }
        }
        
        /// <summary>
        ///     ��������
        /// </summary>
        public override bool Lock(long id)
        {
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                if (!Access.Any(p => p.Id == id && p.AuditState == AuditStateType.Pass))
                    return false;
                if (base.Lock(id))
                {
                    Access.SetValue(p => p.AuditState, AuditStateType.End, id);
                }
            }
            return true;
        }

        #endregion

        #region ����ʵ��

        /// <summary>
        ///     ���ͨ��
        /// </summary>
        protected bool AuditPassInner(TData data)
        {
            return AuditInner(data, true);
        }

        /// <summary>
        ///     ��˲�ͨ��
        /// </summary>
        protected bool AuditDenyInner(TData data)
        {
            return AuditInner(data, false);
        }

        /// <summary>
        ///     ����
        /// </summary>
        protected bool PullbackInner(TData data)
        {
            if (data.AuditState != AuditStateType.Submit)
            {
                ApiContext.Current.LastMessage = "����˴���������޷�����";
                return false;
            }
            if (data.LastReviserId != ApiContext.Current.LoginUserId)
            {
                ApiContext.Current.LastMessage = "���Ǳ����ύ�������޷�����";
                return false;
            }
            if (data.LastModifyDate < DateTime.Now.AddMinutes(-10))
            {
                ApiContext.Current.LastMessage = "���ύ����ʮ���ӵ������޷�����";
                return false;
            }
            
            SetAuditState(data, AuditStateType.None, DataStateType.None);
            Access.Update(data);
            if (unityStateChanged)
                OnStateChanged(data, BusinessCommandType.Pullback);
            return true;
        }

        /// <summary>
        ///     ���
        /// </summary>
        protected bool AuditInner(TData data, bool pass)
        {
            //ApiContext.Current.IsSystemMode = true;
            if (pass)
            {
                AuditPrepare(data);
                if (!CancelValidate && !ValidateInner(data))
                {
                    return false;
                }
                if (!CanDoAuditAction(data))
                {
                    return false;
                }
                if (!CanAuditPass(data))
                {
                    return false;
                }
                SetAuditState(data, AuditStateType.Pass, DataStateType.Enable);
                DoAuditPass(data);
            }
            else
            {
                SetAuditState(data, AuditStateType.Deny, DataStateType.None);
                DoAuditDeny(data);
            }
            Access.Update(data);
            if (pass)
            {
                OnAuditPassed(data);
            }
            else
            {
                OnAuditDenyed(data);
            }
            if (unityStateChanged)
                OnStateChanged(data, pass ? BusinessCommandType.Pass : BusinessCommandType.Deny);
            return true;
        }


        /// <summary>
        ///     �����
        /// </summary>
        protected bool UnAuditInner(TData data)
        {
            if (!CanUnAudit(data))
            {
                return false;
            }
            SetAuditState(data, AuditStateType.Again, DataStateType.None);
            DoUnAudit(data);
            Access.Update(data);
            OnUnAudited(data);
            if (unityStateChanged)
                OnStateChanged(data, BusinessCommandType.ReAudit);
            return true;
        }

        /// <summary>
        ///     �˻ر༭
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected bool BackInner(TData data)
        {
            if (data.AuditState != AuditStateType.Submit)
            {
                ApiContext.Current.LastMessage = BackMessage;
                return false;
            }
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(Access.DataBase))
                {
                    if (!DoBack(data))
                    {
                        return false;
                    }
                    SetAuditState(data, AuditStateType.Again, DataStateType.None);
                    Access.Update(data);
                    OnBacked(data);
                    scope.SetState(true);
                }
                if (unityStateChanged)
                    OnStateChanged(data, BusinessCommandType.Back);
            }
            return true;
        }

        /// <summary>
        ///     �ύ���
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected bool SubmitInner(TData data)
        {
            //ApiContext.Current.IsSystemMode = true;
            if (data == null || data.IsDeleted())
            {
                ApiContext.Current.LastMessage = "���ݲ����ڻ���ɾ����";
                return false;
            }
            if (data.AuditState == AuditStateType.Submit)
            {
                return true;
            }
            if (data.AuditState > AuditStateType.Submit)
            {
                ApiContext.Current.LastMessage = SubmitMessage;
                return false;
            }
            
            if (!CancelValidate && !ValidateInner(data))
            {
                return false;
            }
            if (!CanDoAuditAction(data))
            {
                return false;
            }
            if (!DoSubmit(data))
            {
                return false;
            }
            SetAuditState(data, AuditStateType.Submit, DataStateType.None);
            Access.Update(data);
            if (unityStateChanged)
                OnStateChanged(data, BusinessCommandType.Submit);
            return true;
        }

        #endregion

        #region ��������ʵ��

        /// <summary>
        ///     �������״̬
        /// </summary>
        /// <param name="data"></param>
        /// <param name="audit"></param>
        /// <param name="state"></param>
        private void SetAuditState(TData data, AuditStateType audit, DataStateType state)
        {
            data.AuditState = audit;
            switch (audit)
            {
                case AuditStateType.Pass:
                    state = DataStateType.Enable;
                    data.AuditDate = DateTime.Now;
                    data.AuditorId = ApiContext.Current.LoginUserId;
                    break;
                case AuditStateType.Deny:
                    state = DataStateType.Discard;
                    data.AuditDate = DateTime.Now;
                    data.AuditorId = ApiContext.Current.LoginUserId;
                    break;
                case AuditStateType.End:
                    data.IsFreeze = true;
                    break;
            }
            data.DataState = state;
            OnAuditStateChanged(data);
        }
        /// <summary>
        ///     �������״̬
        /// </summary>
        /// <param name="data"></param>
        protected virtual void OnAuditStateChanged(TData data)
        {
        }
        /// <summary>
        ///     �ܷ�ͨ�����)���ж�
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool CanAuditPass(TData data)
        {
            return true;
        }

        /// <summary>
        ///     �ܷ������˵��ж�
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool CanDoAuditAction(TData data)
        {
            if (data == null || data.IsDeleted())
            {
                ApiContext.Current.LastMessage = "���ݲ����ڻ���ɾ����";
                return false;
            }
            if (data.AuditState <= AuditStateType.Submit)
                return true;
            if (data.LastModifyDate >= DateTime.Now.AddMinutes(-30) &&
                (data.AuditState == AuditStateType.Pass || data.AuditState == AuditStateType.Deny))
                return true;
            ApiContext.Current.LastMessage = AuditMessageReDo;
            return false;
        }
        /// <summary>
        ///     ������ȷ��У��
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected bool DoValidateInner(TData data)
        {
            AuditPrepare(data);
            return ValidateInner(data);
        }

        /// <summary>
        ///     ������ȷ��У��
        /// </summary>
        /// <param name="data"></param>
        /// <param name="putError"></param>
        /// <returns></returns>
        protected bool DoValidateInner(TData data, Action<ValidateResult> putError)
        {
            AuditPrepare(data);
            return ValidateInner(data, putError);
        }

        /// <summary>
        ///     ������ȷ��У��
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool ValidateInner(TData data)
        {
            return ValidateInner(data, r => { });
        }

        /// <summary>
        ///     ������ȷ��У��
        /// </summary>
        /// <param name="data"></param>
        /// <param name="putError"></param>
        /// <returns></returns>
        protected bool ValidateInner(TData data, Action<ValidateResult> putError)
        {
            if (data == null)
            {
                return false;
            }
            var result = data.Validate();
            if (!result.succeed)
            {
                putError?.Invoke(result);
                ApiContext.Current.LastMessage = result.ToString();
                return false;
            }
            return ValidateExtend(data);
        }


        /// <summary>
        ///     �ܷ���з���˵��ж�
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool CanUnAudit(TData data)
        {
            if (data.IsFreeze)
            {
                ApiContext.Current.LastMessage = unAuditMessageLock;
                return false;
            }
            if (data.AuditState < AuditStateType.Deny)
            {
                ApiContext.Current.LastMessage = unAuditMessageNoSubmit;
                return false;
            }
            return true;
        }

        #endregion

        #region ����չ����

        /// <summary>
        ///     ���ͨ��ǰ��׼��
        /// </summary>
        /// <param name="data"></param>
        protected virtual void AuditPrepare(TData data)
        {
        }


        /// <summary>
        ///     ִ�з���˵���չ����
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual void DoUnAudit(TData data)
        {
            data.DataState = DataStateType.None;
        }

        /// <summary>
        ///     ִ�з������ɺ�
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual void OnUnAudited(TData data)
        {
        }


        /// <summary>
        ///     �ύ���
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool DoSubmit(TData data)
        {
            return true;
        }

        /// <summary>
        ///     �˻�����
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool DoBack(TData data)
        {
            return true;
        }

        /// <summary>
        ///     �˻����
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual void OnBacked(TData data)
        {
        }

        /// <summary>
        ///     ��չ����У��
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool ValidateExtend(TData data)
        {
            return true;
        }

        /// <summary>
        ///     ִ����˵���չ����
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual void OnAuditDenyed(TData data)
        {
        }

        /// <summary>
        ///     ִ����˵���չ����
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool DoAuditDeny(TData data)
        {
            data.DataState = DataStateType.Discard;
            return true;
        }

        /// <summary>
        ///     ִ����˵���չ����
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool DoAuditPass(TData data)
        {
            data.DataState = DataStateType.Enable;
            return true;
        }

        /// <summary>
        ///     ִ����˵���չ����
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual void OnAuditPassed(TData data)
        {
        }

        #endregion
    }
}