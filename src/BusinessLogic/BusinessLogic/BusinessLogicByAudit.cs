/*****************************************************
(c)2016-2021 by ZeroTeam
����: ����ˮ
����: Agebull.EntityModel.CoreAgebull.DataModel
����: ��������
�޸�: -
*****************************************************/

#region ����

using Agebull.EntityModel.Common;
using Agebull.EntityModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    ///     ���������չ��ҵ���߼�����
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TPrimaryKey">��������</typeparam>
    public abstract class BusinessLogicByAudit<TData, TPrimaryKey>
        : BusinessLogicByStateData<TData, TPrimaryKey>
        where TData : class, IHistoryData, IIdentityData<TPrimaryKey>, IAuditData, IStateData, new()
    {
        #region ��Ϣ

        /// <summary>
        ///     ȡ��У��(���ʱ��Ч)
        /// </summary>
        public bool CancelValidate { get; set; }
        /// <summary>
        /// ��ʾ��
        /// </summary>
        protected virtual string UnAuditMessageLock => "���������ѹ鵵�����ݣ����ܽ��з���ˣ�";

        /// <summary>
        /// ��ʾ��
        /// </summary>
        protected virtual string UnAuditMessageNoSubmit => "δͨ����˵����ݣ����ܽ��з���ˣ�";

        /// <summary>
        /// ��ʾ��
        /// </summary>
        protected virtual string SubmitMessage => "����˽��������ݲ������ύ��";

        /// <summary>
        /// ��ʾ��
        /// </summary>
        protected virtual string BackMessage => "�����ύδ��˵����ݿ��˻أ�";

        /// <summary>
        /// ��ʾ��
        /// </summary>
        protected virtual string AuditMessageReDo => "�������˳�����ʮ���ӵ����ݣ��޷��ٴ���ˣ�";

        /// <summary>
        /// ��ʾ��
        /// </summary>
        protected virtual string AuditMessage => "�����ύδ��˵����ݿɽ�����ˣ�";

        #endregion

        #region ��������

        /// <summary>
        ///     �����ύ
        /// </summary>
        public Task<bool> Submit(IEnumerable<TPrimaryKey> sels)
        {
            return DoByIds(sels, Submit);
        }

        /// <summary>
        ///     �����˻�
        /// </summary>
        public Task<bool> Back(IEnumerable<TPrimaryKey> sels)
        {
            return DoByIds(sels, Back);
        }

        /// <summary>
        ///     ����ͨ��
        /// </summary>
        public Task<bool> AuditPass(IEnumerable<TPrimaryKey> sels)
        {
            return DoByIds(sels, AuditPass);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        public Task<bool> Pullback(IEnumerable<TPrimaryKey> sels)
        {
            return DoByIds(sels, Pullback);
        }

        /// <summary>
        ///     �������
        /// </summary>
        public Task<bool> AuditDeny(IEnumerable<TPrimaryKey> sels)
        {
            return DoByIds(sels, AuditDeny);
        }

        /// <summary>
        ///     ���������
        /// </summary>
        public Task<bool> UnAudit(IEnumerable<TPrimaryKey> sels)
        {
            return DoByIds(sels, UnAudit);
        }
        /// <summary>
        ///     ��������У��
        /// </summary>
        public Task<bool> Validate(IEnumerable<TPrimaryKey> sels, Action<ValidateResult> putError)
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
        public virtual async Task<bool> Validate(TPrimaryKey id)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            var data = await Access.LoadByPrimaryKeyAsync(id);
            if (data == null)
                return false;
            return await DoValidateInner(data);
        }

        /// <summary>
        ///     ���ͨ��
        /// </summary>
        public async Task<bool> AuditPass(TPrimaryKey id)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            var data = await Access.LoadByPrimaryKeyAsync(id);
            if (data == null)
                return false;
            return await AuditPassInner(data);
        }

        /// <summary>
        ///     ��˲�ͨ��
        /// </summary>
        public async Task<bool> AuditDeny(TPrimaryKey id)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            var data = await Access.LoadByPrimaryKeyAsync(id);
            if (data == null)
                return false;
            return await AuditDenyInner(data);
        }

        /// <summary>
        ///     �����
        /// </summary>
        public async Task<bool> UnAudit(TPrimaryKey id)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            var data = await Access.LoadByPrimaryKeyAsync(id);
            if (data == null)
                return false;
            return await UnAuditInner(data);
        }

        /// <summary>
        ///     �ύ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Submit(TPrimaryKey id)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            var data = await Access.LoadByPrimaryKeyAsync(id);
            if (data == null)
                return false;
            return await SubmitInner(data);
        }

        /// <summary>
        ///     �˻ر༭
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Back(TPrimaryKey id)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            var data = await Access.LoadByPrimaryKeyAsync(id);
            if (data == null)
                return false;
            return await BackInner(data);
        }

        /// <summary>
        ///     ���ر༭
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Pullback(TPrimaryKey id)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            var data = await Access.LoadByPrimaryKeyAsync(id);
            if (data == null)
                return false;
            return await PullbackInner(data);
        }

        #endregion

        #region ״̬����

        /// <summary>
        ///     ɾ������ǰ�ô���
        /// </summary>
        protected override async Task<bool> DoDelete(TPrimaryKey id)
        {
            if (await Access.AnyAsync(p => p.Id.Equals(id) && p.AuditState == AuditStateType.None))
                return await base.DoDelete(id);
            Context.LastMessage = "��δ�����κ���˲��������ݿ��Ա�ɾ��";
            return false;
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public override async Task<bool> Reset(TPrimaryKey id)
        {
            if (await Access.AnyAsync(p => p.Id.Equals(id) && p.AuditState != AuditStateType.Pass))
                return false;
            return await base.Reset(id);
        }


        /// <summary>
        ///     ���ö���
        /// </summary>
        public override async Task<bool> Disable(TPrimaryKey id)
        {
            if (await Access.AnyAsync(p => p.Id.Equals(id) && p.AuditState != AuditStateType.Pass))
                return false;
            return await base.Disable(id);
        }
        /// <summary>
        ///     ���ö���
        /// </summary>
        public override async Task<bool> Discard(TPrimaryKey id)
        {
            if (await Access.AnyAsync(p => p.Id.Equals(id) && p.AuditState != AuditStateType.Pass))
                return false;
            return await base.Discard(id);
        }
        /// <summary>
        ///     ���ö���
        /// </summary>
        public override async Task<bool> Enable(TPrimaryKey id)
        {
            if (await Access.AnyAsync(p => p.Id.Equals(id) && p.AuditState != AuditStateType.Pass))
                return false;
            return await base.Enable(id);
        }

        #endregion

        #region ����ʵ��


        /// <summary>
        ///     ���ͨ��
        /// </summary>
        protected Task<bool> AuditPassInner(TData data)
        {
            return AuditInner(data, true);
        }

        /// <summary>
        ///     ��˲�ͨ��
        /// </summary>
        protected Task<bool> AuditDenyInner(TData data)
        {
            return AuditInner(data, false);
        }

        /// <summary>
        ///     ����
        /// </summary>
        protected async Task<bool> PullbackInner(TData data)
        {
            if (data.AuditState != AuditStateType.Submit)
            {
                Context.LastMessage = "����˴���������޷�����";
                return false;
            }
            if (data.LastReviserId != Context.UserId)
            {
                Context.LastMessage = "���Ǳ����ύ�������޷�����";
                return false;
            }
            if (data.LastModifyDate < DateTime.Now.AddMinutes(-10))
            {
                Context.LastMessage = "���ύ����ʮ���ӵ������޷�����";
                return false;
            }

            await SetAuditState(data, AuditStateType.None, DataStateType.None);
            await SaveAuditData(data);
            await OnCommandSuccess(data, default, DataOperatorType.Pullback);
            return true;
        }

        /// <summary>
        ///     ���
        /// </summary>
        protected async Task<bool> AuditInner(TData data, bool pass)
        {
            //ApiContext.Current.IsSystemMode = true;
            if (pass)
            {
                await AuditPrepare(data);
                if (!CancelValidate && !await ValidateInner(data))
                {
                    return false;
                }
                if (!await CanDoAuditAction(data))
                {
                    return false;
                }
                if (!await CanAuditPass(data))
                {
                    return false;
                }
                await SetAuditState(data, AuditStateType.Pass, DataStateType.Enable);
            }
            else
            {
                await SetAuditState(data, AuditStateType.Deny, DataStateType.Discard);
            }
            await SaveAuditData(data);
            if (pass)
            {
                await OnAuditPassed(data);
            }
            else
            {
                await OnAuditDenyed(data);
            }
            await OnCommandSuccess(data, default, pass ? DataOperatorType.Pass : DataOperatorType.Deny);
            return true;
        }


        /// <summary>
        ///     �����
        /// </summary>
        protected async Task<bool> UnAuditInner(TData data)
        {
            if (!await CanUnAudit(data))
            {
                return false;
            }
            await SetAuditState(data, AuditStateType.Again, DataStateType.None);
            await SaveAuditData(data);
            await OnUnAudited(data);
            await OnCommandSuccess(data, default, DataOperatorType.ReAudit);
            return true;
        }

        /// <summary>
        ///     �˻ر༭
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected async Task<bool> BackInner(TData data)
        {
            if (data.AuditState != AuditStateType.Submit)
            {
                Context.LastMessage = BackMessage;
                return false;
            }
            await SetAuditState(data, AuditStateType.Again, DataStateType.None);
            await SaveAuditData(data);
            await OnBacked(data);
            await OnCommandSuccess(data, default, DataOperatorType.Back);
            return true;
        }

        /// <summary>
        ///     �ύ���
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected async Task<bool> SubmitInner(TData data)
        {
            //ApiContext.Current.IsSystemMode = true;
            if (data == null || data.IsDeleted())
            {
                Context.LastMessage = "���ݲ����ڻ���ɾ����";
                return false;
            }
            if (data.AuditState == AuditStateType.Submit)
            {
                return true;
            }
            if (data.AuditState > AuditStateType.Submit)
            {
                Context.LastMessage = SubmitMessage;
                return false;
            }

            if (!CancelValidate && !await ValidateInner(data))
            {
                return false;
            }
            if (!await CanDoAuditAction(data))
            {
                return false;
            }
            await SetAuditState(data, AuditStateType.Submit, DataStateType.None);
            await SaveAuditData(data);
            await OnSubmit(data);
            await OnCommandSuccess(data, default, DataOperatorType.Submit);
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
        private Task SetAuditState(TData data, AuditStateType audit, DataStateType state)
        {
            data.AuditState = audit;
            switch (audit)
            {
                case AuditStateType.Pass:
                    state = DataStateType.Enable;
                    data.AuditDate = DateTime.Now;
                    data.Auditor = Context.NickName;
                    data.AuditorId = Context.UserId;
                    break;
                case AuditStateType.Deny:
                    state = DataStateType.Discard;
                    data.AuditDate = DateTime.Now;
                    data.Auditor = Context.NickName;
                    data.AuditorId = Context.UserId;
                    break;
            }
            data.DataState = state;
            return OnAuditStateChanged(data);
        }

        /// <summary>
        ///     �������״̬
        /// </summary>
        /// <param name="data"></param>
        protected virtual Task OnAuditStateChanged(TData data) => Task.FromResult(true);

        /// <summary>
        ///     �ܷ�ͨ�����)���ж�
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task<bool> CanAuditPass(TData data) => Task.FromResult(true);

        /// <summary>
        ///     �ܷ������˵��ж�
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task<bool> CanDoAuditAction(TData data)
        {
            if (data == null || data.IsDeleted())
            {
                Context.LastMessage = "���ݲ����ڻ���ɾ����";
                return Task.FromResult(false);
            }
            if (data.AuditState <= AuditStateType.Submit)
                return Task.FromResult(true);
            if (data.LastModifyDate >= DateTime.Now.AddMinutes(-30) &&
                (data.AuditState == AuditStateType.Pass || data.AuditState == AuditStateType.Deny))
                return Task.FromResult(true);
            Context.LastMessage = AuditMessageReDo;
            return Task.FromResult(false);
        }
        /// <summary>
        ///     ������ȷ��У��
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected async Task<bool> DoValidateInner(TData data)
        {
            await AuditPrepare(data);
            return await ValidateInner(data);
        }

        /// <summary>
        ///     ������ȷ��У��
        /// </summary>
        /// <param name="data"></param>
        /// <param name="putError"></param>
        /// <returns></returns>
        protected async Task<bool> DoValidateInner(TData data, Action<ValidateResult> putError)
        {
            await AuditPrepare(data);
            return await ValidateInner(data, putError);
        }

        /// <summary>
        ///     ������ȷ��У��
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task<bool> ValidateInner(TData data)
        {
            return ValidateInner(data, r => { });
        }

        /// <summary>
        ///     ������ȷ��У��
        /// </summary>
        /// <param name="data"></param>
        /// <param name="putError"></param>
        /// <returns></returns>
        protected async Task<bool> ValidateInner(TData data, Action<ValidateResult> putError)
        {
            if (data == null)
            {
                return false;
            }
            if (data is IValidate validate)
            {
                if (validate.Validate(out var result))
                    return await ValidateExtend(data);
                putError?.Invoke(result);
                Context.LastMessage = result.ToString();
                return false;
            }
            return true;
        }


        /// <summary>
        ///     �ܷ���з���˵��ж�
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task<bool> CanUnAudit(TData data)
        {
            if (data.AuditState == AuditStateType.End)
            {
                Context.LastMessage = UnAuditMessageLock;
                return Task.FromResult(false);
            }

            if (data.AuditState >= AuditStateType.Deny)
                return Task.FromResult(true);
            Context.LastMessage = UnAuditMessageNoSubmit;
            return Task.FromResult(false);
        }

        #endregion

        #region ����չ����

        /// <summary>
        ///     ���ͨ��ǰ��׼��
        /// </summary>
        /// <param name="data"></param>
        protected virtual Task AuditPrepare(TData data) => Task.CompletedTask;

        /// <summary>
        ///     ִ�з������ɺ�
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task OnUnAudited(TData data) => Task.CompletedTask;


        /// <summary>
        ///     �ύ���
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task OnSubmit(TData data) => Task.CompletedTask;

        /// <summary>
        ///     �˻����
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task OnBacked(TData data) => Task.CompletedTask;

        /// <summary>
        ///     ��չ����У��
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task<bool> ValidateExtend(TData data)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        ///     ִ����˵���չ����
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task OnAuditDenyed(TData data) => Task.CompletedTask;

        /// <summary>
        ///     ִ����˵���չ����
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task OnAuditPassed(TData data) => Task.CompletedTask;

        #endregion

        #region ���ݿ�д��

        /// <summary>
        ///     ����״̬��SQL���
        /// </summary>
        protected string ResetStateFileSqlCode(TData data) => $@"";

        /// <summary>
        ///     ���ͨ��
        /// </summary>
        async Task<bool> SaveAuditData(TData data)
        {
            List<(string field, object value)> fields = new List<(string field, object value)>
            {
                (nameof(IAuditData.AuditState), data.AuditState),
                (nameof(IStateData.DataState), data.DataState),
            };
            using var levelScope = Access.InjectionScope(InjectionLevel.NotCondition);
            switch (data.AuditState)
            {
                case AuditStateType.Pass:
                case AuditStateType.Deny:
                    fields.Add((nameof(IAuditData.Auditor), data.Auditor));
                    fields.Add((nameof(IAuditData.AuditorId), data.AuditorId));
                    fields.Add(("is_freeze", 1));
                    break;
                case AuditStateType.Submit:
                case AuditStateType.End:
                    fields.Add(("is_freeze", 1));
                    break;
                case AuditStateType.None:
                case AuditStateType.Again:
                    fields.Add(("is_freeze", 0));
                    break;
            }
            return await Access.SetValueAsync(data.Id, fields.ToArray()) == 1;


        }
        #endregion
    }

    /// <summary>
    ///     ���������չ��ҵ���߼�����
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    public abstract class BusinessLogicByAudit<TData> : BusinessLogicByAudit<TData, long>
        where TData : class, IIdentityData<long>, IHistoryData, IAuditData, IStateData, new()
    {
    }
}