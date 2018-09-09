// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

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
    ///     基于审核扩展的业务逻辑基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TAccess">数据访问对象</typeparam>
    /// <typeparam name="TDatabase">数据库对象</typeparam>
    public class BusinessLogicByAudit<TData, TAccess, TDatabase> 
        : BusinessLogicByHistory<TData, TAccess, TDatabase>
        where TData : EditDataObject, IIdentityData, IHistoryData, IAuditData, IStateData, new()
        where TAccess : HitoryTable<TData, TDatabase>, new()
        where TDatabase : MySqlDataBase
    {
        #region 消息

        /// <summary>
        ///     取消校验(审核时有效)
        /// </summary>
        public bool CancelValidate { get; set; }

        protected virtual string unAuditMessageLock => "完成审核且已归档的数据，不能进行反审核！";

        protected virtual string unAuditMessageNoSubmit => "未通过审核的数据，不能进行反审核！";

        protected virtual string SubmitMessage => "已审核结束的数据不可以提交！";

        protected virtual string BackMessage => "仅已提交未审核的数据可退回！";

        protected virtual string AuditMessageReDo => "已完成审核超过三十分钟的数据，无法再次审核！";

        protected virtual string AuditMessage => "仅已提交未审核的数据可进行审核！";

        #endregion

        #region 批量操作

        /// <summary>
        ///     批量提交
        /// </summary>
        public bool Submit(string sels)
        {
            return DoByIds(sels, SubmitInner);
        }

        /// <summary>
        ///     批量退回
        /// </summary>
        public bool Back(string sels)
        {
            return DoByIds(sels, BackInner);
        }

        /// <summary>
        ///     批量通过
        /// </summary>
        public bool AuditPass(string sels)
        {
            return DoByIds(sels, AuditPassInner);
        }

        /// <summary>
        ///     批量拉回
        /// </summary>
        public bool Pullback(string sels)
        {
            return DoByIds(sels, PullbackInner);
        }

        /// <summary>
        ///     批量否决
        /// </summary>
        public bool AuditDeny(string sels)
        {
            return DoByIds(sels, AuditDenyInner);
        }

        /// <summary>
        ///     批量反审核
        /// </summary>
        public bool UnAudit(string sels)
        {
            return DoByIds(sels, UnAuditInner);
        }
        /// <summary>
        ///     批量数据校验
        /// </summary>
        public bool Validate(string sels, Action<ValidateResult> putError)
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
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                return DoValidateInner(Details(id));
            }
        }

        /// <summary>
        ///     审核通过
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
        ///     审核不通过
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
        ///     反审核
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
        ///     提交审核
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
        ///     退回编辑
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

        #region 状态重载

        /// <summary>
        ///     删除对象前置处理
        /// </summary>
        protected override bool PrepareDelete(long id)
        {
            if (Access.Any(p => p.Id == id && p.AuditState != AuditStateType.None))
            {
                ApiContext.Current.LastMessage = "仅未进行任何审核操作的数据可以被删除";
                return false;
            }
            return base.PrepareDelete(id);
        }

        /// <summary>
        ///     重置数据状态
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
        ///     禁用对象
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
        ///     禁用对象
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
        ///     弃用对象
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
        ///     启用对象
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
        ///     锁定对象
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
                ApiContext.Current.LastMessage = "已审核处理的数据无法拉回";
                return false;
            }
            if (data.LastReviserId != ApiContext.Current.LoginUserId)
            {
                ApiContext.Current.LastMessage = "不是本人提交的数据无法拉回";
                return false;
            }
            if (data.LastModifyDate < DateTime.Now.AddMinutes(-10))
            {
                ApiContext.Current.LastMessage = "已提交超过十分钟的数据无法拉回";
                return false;
            }
            
            SetAuditState(data, AuditStateType.None, DataStateType.None);
            Access.Update(data);
            if (unityStateChanged)
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
            if (unityStateChanged)
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
            if (unityStateChanged)
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
        ///     提交审核
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected bool SubmitInner(TData data)
        {
            //ApiContext.Current.IsSystemMode = true;
            if (data == null || data.IsDeleted())
            {
                ApiContext.Current.LastMessage = "数据不存在或已删除！";
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
                ApiContext.Current.LastMessage = "数据不存在或已删除！";
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
            if (!result.succeed)
            {
                putError?.Invoke(result);
                ApiContext.Current.LastMessage = result.ToString();
                return false;
            }
            return ValidateExtend(data);
        }


        /// <summary>
        ///     能否进行反审核的判断
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