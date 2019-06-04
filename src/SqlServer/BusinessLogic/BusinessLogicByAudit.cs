// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;

using Agebull.EntityModel.Interfaces;
using Agebull.EntityModel.SqlServer;
using Agebull.EntityModel.Common;
using Agebull.Common.Context;

#endregion

namespace Agebull.EntityModel.BusinessLogic.SqlServer
{
    /// <summary>
    ///     基于审核扩展的业务逻辑基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TAccess">数据访问对象</typeparam>
    /// <typeparam name="TDatabase">数据库对象</typeparam>
    public class BusinessLogicByAudit<TData, TAccess, TDatabase>
        : BusinessLogicByStateData<TData, TAccess, TDatabase>, IBusinessLogicByAudit<TData>
        where TData : EditDataObject, IIdentityData, IHistoryData, IAuditData, IStateData, new()
        where TAccess : DataStateTable<TData, TDatabase>, new()
        where TDatabase : SqlServerDataBase
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
        public bool Submit(IEnumerable<long> sels)
        {
            return DoByIds(sels, SubmitInner);
        }

        /// <summary>
        ///     批量退回
        /// </summary>
        public bool Back(IEnumerable<long> sels)
        {
            return DoByIds(sels, BackInner);
        }

        /// <summary>
        ///     批量通过
        /// </summary>
        public bool AuditPass(IEnumerable<long> sels)
        {
            return DoByIds(sels, AuditPassInner);
        }

        /// <summary>
        ///     批量拉回
        /// </summary>
        public bool Pullback(IEnumerable<long> sels)
        {
            return DoByIds(sels, PullbackInner);
        }

        /// <summary>
        ///     批量否决
        /// </summary>
        public bool AuditDeny(IEnumerable<long> sels)
        {
            return DoByIds(sels, AuditDenyInner);
        }

        /// <summary>
        ///     批量反审核
        /// </summary>
        public bool UnAudit(IEnumerable<long> sels)
        {
            return DoByIds(sels, UnAuditInner);
        }
        /// <summary>
        ///     批量数据校验
        /// </summary>
        public bool Validate(IEnumerable<long> sels, Action<ValidateResult> putError)
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
        public virtual bool Validate(long id)
        {
            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                return scope.SetState(DoValidateInner(Details(id)));
            }
        }

        /// <summary>
        ///     审核通过
        /// </summary>
        public bool AuditPass(long id)
        {
            var data = Details(id);
            if (data == null)
                return false;
            using (var scope = TransactionScope.CreateScope(Access.DataBase))
            {
                return scope.SetState(AuditPassInner(data));
            }
        }

        /// <summary>
        ///     审核不通过
        /// </summary>
        public bool AuditDeny(long id)
        {
            var data = Details(id);
            if (data == null)
                return false;

            using (var scope = TransactionScope.CreateScope(Access.DataBase))
            {
                return scope.SetState(AuditDenyInner(data));
            }
        }

        /// <summary>
        ///     反审核
        /// </summary>
        public bool UnAudit(long id)
        {
            var data = Details(id);
            if (data == null)
            {
                return false;
            }
            using (var scope = TransactionScope.CreateScope(Access.DataBase))
            {
                return scope.SetState(UnAuditInner(data));
            }
        }

        /// <summary>
        ///     提交审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Submit(long id)
        {
            var data = Details(id);
            if (data == null)
            {
                return false;
            }
            using (var scope = TransactionScope.CreateScope(Access.DataBase))
            {
                return scope.SetState(SubmitInner(data));
            }
        }

        /// <summary>
        ///     退回编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Back(long id)
        {
            var data = Details(id);
            if (data == null)
            {
                return false;
            }
            using (var scope = TransactionScope.CreateScope(Access.DataBase))
            {
                return scope.SetState(BackInner(data));
            }
        }

        #endregion

        #region 状态重载

        /// <summary>
        ///     删除对象前置处理
        /// </summary>
        protected override bool PrepareDelete(long id)
        {
            if (Access.Any(p => p.Id == id && p.AuditState == AuditStateType.None))
                return base.PrepareDelete(id);
            GlobalContext.Current.LastMessage = "仅未进行任何审核操作的数据可以被删除";
            return false;
        }

        ///// <summary>
        /////     重置数据状态
        ///// </summary>
        ///// <param name="data"></param>
        //protected override bool DoResetState(TData data)
        //{
        //    if (data == null)
        //        return false;
        //    data.AuditState = AuditStateType.None;
        //    data.AuditDate = DateTime.MinValue;
        //    data.AuditorId = 0;
        //    return base.DoResetState(data);
        //}

        /// <summary>
        ///     禁用对象
        /// </summary>
        public override bool Reset(long id)
        {
            if (Access.Any(p => p.Id == id && p.AuditState != AuditStateType.Pass))
                return false;
            return ResetState(Access.First(id));
        }


        /// <summary>
        ///     禁用对象
        /// </summary>
        public override bool Disable(long id)
        {
            if (Access.Any(p => p.Id == id && p.AuditState != AuditStateType.Pass))
                return false;
            return base.Disable(id);
        }
        /// <summary>
        ///     弃用对象
        /// </summary>
        public override bool Discard(long id)
        {
            if (Access.Any(p => p.Id == id && p.AuditState != AuditStateType.Pass))
                return false;
            return base.Discard(id);
        }
        /// <summary>
        ///     启用对象
        /// </summary>
        public override bool Enable(long id)
        {
            if (Access.Any(p => p.Id == id && p.AuditState != AuditStateType.Pass))
                return false;
            return base.Enable(id);
        }

        /// <summary>
        ///     锁定对象
        /// </summary>
        public override bool Lock(long id)
        {
            if (Access.Any(p => p.Id == id && p.AuditState != AuditStateType.Pass))
                return false;
            return base.Lock(id);
        }

        #endregion

        #region 基本实现

        /// <summary>
        ///     审核通过
        /// </summary>
        protected bool AuditPassInner(TData data)
        {
            return AuditInner(data, true);
        }

        /// <summary>
        ///     审核不通过
        /// </summary>
        protected bool AuditDenyInner(TData data)
        {
            return AuditInner(data, false);
        }

        /// <summary>
        ///     拉回
        /// </summary>
        protected bool PullbackInner(TData data)
        {
            if (data.AuditState != AuditStateType.Submit)
            {
                GlobalContext.Current.LastMessage = "已审核处理的数据无法拉回";
                return false;
            }
            if (data.LastReviserId != GlobalContext.Current.LoginUserId)
            {
                GlobalContext.Current.LastMessage = "不是本人提交的数据无法拉回";
                return false;
            }
            if (data.LastModifyDate < DateTime.Now.AddMinutes(-10))
            {
                GlobalContext.Current.LastMessage = "已提交超过十分钟的数据无法拉回";
                return false;
            }

            SetAuditState(data, AuditStateType.None, DataStateType.None);
            Access.Update(data);
            OnStateChanged(data, BusinessCommandType.Pullback);
            return true;
        }

        /// <summary>
        ///     审核
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
            OnStateChanged(data, pass ? BusinessCommandType.Pass : BusinessCommandType.Deny);
            return true;
        }


        /// <summary>
        ///     反审核
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
            OnStateChanged(data, BusinessCommandType.ReAudit);
            return true;
        }

        /// <summary>
        ///     退回编辑
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected bool BackInner(TData data)
        {
            if (data.AuditState != AuditStateType.Submit)
            {
                GlobalContext.Current.LastMessage = BackMessage;
                return false;
            }
            if (!DoBack(data))
            {
                return false;
            }
            SetAuditState(data, AuditStateType.Again, DataStateType.None);
            if (!Access.Update(data))
                return false;
            OnBacked(data);
            OnStateChanged(data, BusinessCommandType.Back);
            return true;
        }

        /// <summary>
        ///     提交审核
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected bool SubmitInner(TData data)
        {
            //ApiContext.Current.IsSystemMode = true;
            if (data == null || data.IsDeleted())
            {
                GlobalContext.Current.LastMessage = "数据不存在或已删除！";
                return false;
            }
            if (data.AuditState == AuditStateType.Submit)
            {
                return true;
            }
            if (data.AuditState > AuditStateType.Submit)
            {
                GlobalContext.Current.LastMessage = SubmitMessage;
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
            OnStateChanged(data, BusinessCommandType.Submit);
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
        private void SetAuditState(TData data, AuditStateType audit, DataStateType state)
        {
            data.AuditState = audit;
            switch (audit)
            {
                case AuditStateType.Pass:
                    state = DataStateType.Enable;
                    data.AuditDate = DateTime.Now;
                    data.Auditor = GlobalContext.Current.User.NickName;
                    data.AuditorId = GlobalContext.Current.LoginUserId;
                    break;
                case AuditStateType.Deny:
                    state = DataStateType.Discard;
                    data.AuditDate = DateTime.Now;
                    data.Auditor = GlobalContext.Current.User.NickName;
                    data.AuditorId = GlobalContext.Current.LoginUserId;
                    break;
                case AuditStateType.End:
                    data.IsFreeze = true;
                    break;
            }
            data.DataState = state;
            OnAuditStateChanged(data);
        }
        /// <summary>
        ///     设置审核状态
        /// </summary>
        /// <param name="data"></param>
        protected virtual void OnAuditStateChanged(TData data)
        {
        }
        /// <summary>
        ///     能否通过审核)的判断
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool CanAuditPass(TData data)
        {
            return true;
        }

        /// <summary>
        ///     能否进行审核的判断
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool CanDoAuditAction(TData data)
        {
            if (data == null || data.IsDeleted())
            {
                GlobalContext.Current.LastMessage = "数据不存在或已删除！";
                return false;
            }
            if (data.AuditState <= AuditStateType.Submit)
                return true;
            if (data.LastModifyDate >= DateTime.Now.AddMinutes(-30) &&
                (data.AuditState == AuditStateType.Pass || data.AuditState == AuditStateType.Deny))
                return true;
            GlobalContext.Current.LastMessage = AuditMessageReDo;
            return false;
        }
        /// <summary>
        ///     数据正确性校验
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected bool DoValidateInner(TData data)
        {
            AuditPrepare(data);
            return ValidateInner(data);
        }

        /// <summary>
        ///     数据正确性校验
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
        ///     数据正确性校验
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool ValidateInner(TData data)
        {
            return ValidateInner(data, r => { });
        }

        /// <summary>
        ///     数据正确性校验
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
            if (result.Succeed) return ValidateExtend(data);
            putError?.Invoke(result);
            GlobalContext.Current.LastMessage = result.ToString();
            return false;
        }


        /// <summary>
        ///     能否进行反审核的判断
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool CanUnAudit(TData data)
        {
            if (data.AuditState == AuditStateType.End)
            {
                GlobalContext.Current.LastMessage = UnAuditMessageLock;
                return false;
            }

            if (data.AuditState >= AuditStateType.Deny) return true;
            GlobalContext.Current.LastMessage = UnAuditMessageNoSubmit;
            return false;
        }

        #endregion

        #region 可扩展处理

        /// <summary>
        ///     审核通过前的准备
        /// </summary>
        /// <param name="data"></param>
        protected virtual void AuditPrepare(TData data)
        {
        }


        /// <summary>
        ///     执行反审核的扩展流程
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual void DoUnAudit(TData data)
        {
            data.DataState = DataStateType.None;
        }

        /// <summary>
        ///     执行反审核完成后
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual void OnUnAudited(TData data)
        {
        }


        /// <summary>
        ///     提交审核
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool DoSubmit(TData data)
        {
            return true;
        }

        /// <summary>
        ///     退回重做
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool DoBack(TData data)
        {
            data.DataState = DataStateType.None;
            return true;
        }

        /// <summary>
        ///     退回完成
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual void OnBacked(TData data)
        {
        }

        /// <summary>
        ///     扩展数据校验
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool ValidateExtend(TData data)
        {
            return true;
        }

        /// <summary>
        ///     执行审核的扩展流程
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual void OnAuditDenyed(TData data)
        {
        }

        /// <summary>
        ///     执行审核的扩展流程
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool DoAuditDeny(TData data)
        {
            data.DataState = DataStateType.Discard;
            return true;
        }

        /// <summary>
        ///     执行审核的扩展流程
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool DoAuditPass(TData data)
        {
            data.DataState = DataStateType.Enable;
            return true;
        }

        /// <summary>
        ///     执行审核的扩展流程
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual void OnAuditPassed(TData data)
        {
        }

        #endregion
    }
}