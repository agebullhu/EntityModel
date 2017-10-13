using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Agebull.Common.DataModel.Redis;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.Workflow.BusinessLogic;
using Gboxt.Common.Workflow.DataAccess;

namespace Gboxt.Common.Workflow
{
    /// <summary>
    ///     表示一个工作流业务逻辑基类
    /// </summary>
    public class AutoJobBusinessLogicBase<TData, TAccess> : BusinessLogicByAudit<TData, TAccess>
        where TData : AutoJobEntity, IIdentityData, IWorkflowData, IHistoryData, IAuditData, IStateData, new()
        where TAccess : HitoryTable<TData>, new()
    {
        /// <summary>
        /// 构造
        /// </summary>
        protected AutoJobBusinessLogicBase()
        {
            unityStateChanged = true;
        }

        /// <summary>
        ///     载入当前操作的数据
        /// </summary>
        public override TData Details(int id)
        {
            var data = base.Details(id);
            CheckCanAudit(data);
            return data;
        }

        private UserJobDataAccess _userJobDataAccess;

        /// <summary>
        /// 用户任务数据访问器
        /// </summary>
        protected UserJobDataAccess UserJobDataAccess => _userJobDataAccess ?? (_userJobDataAccess = new UserJobDataAccess
        {
            DataBase = Access.DataBase
        });
        /// <summary>
        /// 检查审核信息
        /// </summary>
        /// <param name="data"></param>
        public void CheckCanAudit(TData data)
        {
            if (data == null || data.AuditState < AuditStateType.Submit)
                return;
            var jobs = UserJobDataAccess.All(p => p.EntityType == EntityType && p.DataId == data.Id && p.JobType == UserJobType.Audit);
            if (jobs.Count == 0)
                return;
            data.ToUsers = jobs.Select(p => $"{p.ToUserName}({p.JobStatus.ToCaption() })").LinkToString("；");
            var me = jobs.Any(p => p.ToUserId == BusinessContext.Current.LoginUserId &&
                                   p.JobStatus == JobStatusType.None);
            if (me)
                data.CanAudit = true;
        }


        /// <summary>
        ///     能否通过审核)的判断
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override bool CanAuditPass(TData data)
        {
            if (!CheckJobCanDo(data))
                return false;
            return base.CanAuditPass(data);
        }
        /// <summary>
        /// 检查任务数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CheckJobCanDo(TData data)
        {
            var jobs = UserJobDataAccess.All(p => p.EntityType == EntityType && p.DataId == data.Id &&
                                                  p.JobType == UserJobType.Audit);
            if (jobs.Count == 0)
            {
                return true;
            }
            if (jobs.All(p => p.ToUserId != BusinessContext.Current.LoginUserId))
            {
                BusinessContext.Current.LastMessage = $"你不在{data.Title}的审批人之中";
                return false;
            }
            return true;
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override bool DoSubmit(TData data)
        {
            var supper = data as ILevelAuditData;
            if (supper != null)
                supper.DepartmentLevel -= 1;
            return base.DoSubmit(data);
        }

        /// <summary>
        ///     执行审核的扩展流程
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override bool DoAuditPass(TData data)
        {
            var jobs = UserJobDataAccess.All(p => p.EntityType == EntityType && p.DataId == data.Id && p.JobType == UserJobType.Audit);
            var nomy = jobs.Where(p => p.ToUserId != BusinessContext.Current.LoginUserId).ToList();
            if (nomy.Count(p => p.JobStatus == JobStatusType.Succeed) != nomy.Count)
            {
                data.AuditState = AuditStateType.Submit;
                return base.DoAuditPass(data);
            }
            var supper = data as ILevelAuditData;
            if (supper == null || supper.LastLevel >= supper.DepartmentLevel)
                return base.DoAuditPass(data);
            if (supper.DepartmentLevel > 1)
                supper.DepartmentLevel -= 1;
            var users = GetUpAuditUsers(supper.DepartmentLevel);
            if (users.Length == 0)
                return base.DoAuditPass(data);
            UserJobData job = jobs.FirstOrDefault();
            if (job == null)
            {
                job = new UserJobData
                {
                    EntityType = EntityType,
                    LinkId = data.Id,
                    Title = data.Title,
                    JobType = UserJobType.Audit,
                    FromUserId = BusinessContext.Current.LoginUserId,
                    FromUserName = BusinessContext.Current.LoginUser.RealName
                };
            }
            else
            {
                job.JobStatus = JobStatusType.None;
                job.DataState = DataStateType.None;
                job.IsFreeze = false;
            }
            var bl = new UserJobBusinessLogic();
            foreach (var user in users)
            {
                job.Id = 0;
                job.ToUserId = user;
                bl.AddNew(job);
            }
            data.AuditState = AuditStateType.Submit;
            return base.DoAuditPass(data);
        }

        /// <summary>
        /// 取更高一级提交者
        /// </summary>
        private int[] GetUpAuditUsers(int level)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return proxy.Get($"audit:page:users:ids:{BusinessContext.Current.PageItem.Id}:{level}").ToIntegers();
            }
        }
        /// <summary>
        ///     内部命令执行完成后的处理(unityStateChanged不设置为true时不会产生作用--基于性能的考虑)
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="cmd">命令</param>
        protected override void OnInnerCommand(TData data, BusinessCommandType cmd)
        {
            UserJobTrigger<TData>.OnDataChanged(data, EntityType, cmd, BusinessContext.Current.LoginUserId);
        }

    }
}