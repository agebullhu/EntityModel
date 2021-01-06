// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Common;
using Agebull.EntityModel.DataEvents;
using Agebull.EntityModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    ///     基于审核扩展的业务逻辑基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class BusinessLogicByAudit<TData, TPrimaryKey>
        : BusinessLogicByStateData<TData, TPrimaryKey>
        where TData : class, IHistoryData, IIdentityData<TPrimaryKey>, IAuditData, IStateData, new()
    {
        #region 消息

        /// <summary>
        ///     取消校验(审核时有效)
        /// </summary>
        public bool CancelValidate { get; set; }
        /// <summary>
        /// 提示语
        /// </summary>
        protected virtual string UnAuditMessageLock => "完成审核且已归档的数据，不能进行反审核！";

        /// <summary>
        /// 提示语
        /// </summary>
        protected virtual string UnAuditMessageNoSubmit => "未通过审核的数据，不能进行反审核！";

        /// <summary>
        /// 提示语
        /// </summary>
        protected virtual string SubmitMessage => "已审核结束的数据不可以提交！";

        /// <summary>
        /// 提示语
        /// </summary>
        protected virtual string BackMessage => "仅已提交未审核的数据可退回！";

        /// <summary>
        /// 提示语
        /// </summary>
        protected virtual string AuditMessageReDo => "已完成审核超过三十分钟的数据，无法再次审核！";

        /// <summary>
        /// 提示语
        /// </summary>
        protected virtual string AuditMessage => "仅已提交未审核的数据可进行审核！";

        #endregion

        #region 批量操作

        /// <summary>
        ///     批量提交
        /// </summary>
        public Task<bool> Submit(IEnumerable<TPrimaryKey> sels)
        {
            return DoByIds(sels, Submit);
        }

        /// <summary>
        ///     批量退回
        /// </summary>
        public Task<bool> Back(IEnumerable<TPrimaryKey> sels)
        {
            return DoByIds(sels, Back);
        }

        /// <summary>
        ///     批量通过
        /// </summary>
        public Task<bool> AuditPass(IEnumerable<TPrimaryKey> sels)
        {
            return DoByIds(sels, AuditPass);
        }

        /// <summary>
        ///     批量拉回
        /// </summary>
        public Task<bool> Pullback(IEnumerable<TPrimaryKey> sels)
        {
            return DoByIds(sels, Pullback);
        }

        /// <summary>
        ///     批量否决
        /// </summary>
        public Task<bool> AuditDeny(IEnumerable<TPrimaryKey> sels)
        {
            return DoByIds(sels, AuditDeny);
        }

        /// <summary>
        ///     批量反审核
        /// </summary>
        public Task<bool> UnAudit(IEnumerable<TPrimaryKey> sels)
        {
            return DoByIds(sels, UnAudit);
        }
        /// <summary>
        ///     批量数据校验
        /// </summary>
        public Task<bool> Validate(IEnumerable<TPrimaryKey> sels, Action<ValidateResult> putError)
        {
            return LoopIdsToData(sels, data => DoValidateInner(data, putError));
        }

        #endregion

        #region 单个操作

        /// <summary>
        ///     数据正确性校验
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
        ///     审核通过
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
        ///     审核不通过
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
        ///     反审核
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
        ///     提交审核
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
        ///     退回编辑
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
        ///     拉回编辑
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

        #region 状态重载

        /// <summary>
        ///     删除对象前置处理
        /// </summary>
        protected override async Task<bool> DoDelete(TPrimaryKey id)
        {
            if (await Access.AnyAsync(p => p.Id.Equals(id) && p.AuditState == AuditStateType.None))
                return await base.DoDelete(id);
            Context.LastMessage = "仅未进行任何审核操作的数据可以被删除";
            return false;
        }

        /// <summary>
        ///     禁用对象
        /// </summary>
        public override async Task<bool> Reset(TPrimaryKey id)
        {
            if (await Access.AnyAsync(p => p.Id.Equals(id) && p.AuditState != AuditStateType.Pass))
                return false;
            return await base.Reset(id);
        }


        /// <summary>
        ///     禁用对象
        /// </summary>
        public override async Task<bool> Disable(TPrimaryKey id)
        {
            if (await Access.AnyAsync(p => p.Id.Equals(id) && p.AuditState != AuditStateType.Pass))
                return false;
            return await base.Disable(id);
        }
        /// <summary>
        ///     弃用对象
        /// </summary>
        public override async Task<bool> Discard(TPrimaryKey id)
        {
            if (await Access.AnyAsync(p => p.Id.Equals(id) && p.AuditState != AuditStateType.Pass))
                return false;
            return await base.Discard(id);
        }
        /// <summary>
        ///     启用对象
        /// </summary>
        public override async Task<bool> Enable(TPrimaryKey id)
        {
            if (await Access.AnyAsync(p => p.Id.Equals(id) && p.AuditState != AuditStateType.Pass))
                return false;
            return await base.Enable(id);
        }

        #endregion

        #region 基本实现


        /// <summary>
        ///     审核通过
        /// </summary>
        protected Task<bool> AuditPassInner(TData data)
        {
            return AuditInner(data, true);
        }

        /// <summary>
        ///     审核不通过
        /// </summary>
        protected Task<bool> AuditDenyInner(TData data)
        {
            return AuditInner(data, false);
        }

        /// <summary>
        ///     拉回
        /// </summary>
        protected async Task<bool> PullbackInner(TData data)
        {
            if (data.AuditState != AuditStateType.Submit)
            {
                Context.LastMessage = "已审核处理的数据无法拉回";
                return false;
            }
            if (data.LastReviserId != Context.UserId)
            {
                Context.LastMessage = "不是本人提交的数据无法拉回";
                return false;
            }
            if (data.LastModifyDate < DateTime.Now.AddMinutes(-10))
            {
                Context.LastMessage = "已提交超过十分钟的数据无法拉回";
                return false;
            }

            await SetAuditState(data, AuditStateType.None, DataStateType.None);
            await SaveAuditData(data);
            await OnCommandSuccess(data, default, DataOperatorType.Pullback);
            return true;
        }

        /// <summary>
        ///     审核
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
        ///     反审核
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
        ///     退回编辑
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
        ///     提交审核
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected async Task<bool> SubmitInner(TData data)
        {
            //ApiContext.Current.IsSystemMode = true;
            if (data == null || data.IsDeleted())
            {
                Context.LastMessage = "数据不存在或已删除！";
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

        #region 审批流程实现

        /// <summary>
        ///     设置审核状态
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
        ///     设置审核状态
        /// </summary>
        /// <param name="data"></param>
        protected virtual Task OnAuditStateChanged(TData data) => Task.FromResult(true);

        /// <summary>
        ///     能否通过审核)的判断
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task<bool> CanAuditPass(TData data) => Task.FromResult(true);

        /// <summary>
        ///     能否进行审核的判断
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task<bool> CanDoAuditAction(TData data)
        {
            if (data == null || data.IsDeleted())
            {
                Context.LastMessage = "数据不存在或已删除！";
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
        ///     数据正确性校验
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected async Task<bool> DoValidateInner(TData data)
        {
            await AuditPrepare(data);
            return await ValidateInner(data);
        }

        /// <summary>
        ///     数据正确性校验
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
        ///     数据正确性校验
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task<bool> ValidateInner(TData data)
        {
            return ValidateInner(data, r => { });
        }

        /// <summary>
        ///     数据正确性校验
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
        ///     能否进行反审核的判断
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

        #region 可扩展处理

        /// <summary>
        ///     审核通过前的准备
        /// </summary>
        /// <param name="data"></param>
        protected virtual Task AuditPrepare(TData data) => Task.CompletedTask;

        /// <summary>
        ///     执行反审核完成后
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task OnUnAudited(TData data) => Task.CompletedTask;


        /// <summary>
        ///     提交审核
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task OnSubmit(TData data) => Task.CompletedTask;

        /// <summary>
        ///     退回完成
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task OnBacked(TData data) => Task.CompletedTask;

        /// <summary>
        ///     扩展数据校验
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task<bool> ValidateExtend(TData data)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        ///     执行审核的扩展流程
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task OnAuditDenyed(TData data) => Task.CompletedTask;

        /// <summary>
        ///     执行审核的扩展流程
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task OnAuditPassed(TData data) => Task.CompletedTask;

        #endregion

        #region 数据库写入

        /// <summary>
        ///     重置状态的SQL语句
        /// </summary>
        protected string ResetStateFileSqlCode(TData data) => $@"";

        /// <summary>
        ///     审核通过
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
    ///     基于审核扩展的业务逻辑基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    public abstract class BusinessLogicByAudit<TData> : BusinessLogicByAudit<TData, long>
        where TData : class, IIdentityData<long>, IHistoryData, IAuditData, IStateData, new()
    {
    }
}