using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Agebull.Common.DataModel;
using Agebull.Common.DataModel.Redis;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.Workflow.BusinessLogic;

namespace Gboxt.Common.Workflow
{

    /// <summary>
    /// 命令同步工作列表
    /// </summary>
    public class UserJobTrigger<TData>
        where TData : EditDataObject, IAuditData, IHistoryData, IStateData, IIdentityData, IWorkflowData, new()
    {
        private readonly UserJobBusinessLogic business = new UserJobBusinessLogic();

        private int TriggerUserId;
        private TData TriggerData;
        private int TriggerEntityType;
        BusinessCommandType TriggerCommand;
        /// <summary>
        /// 数据状态检测
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <param name="entityType"></param>
        /// <param name="cmd"></param>
        /// <param name="userId"></param>
        public static void OnDataChanged(TData data, int entityType, BusinessCommandType cmd, int userId)
        {
            if (data == null)
                return;
            Task.Factory.StartNew(() =>
            {
                var trigger = new UserJobTrigger<TData>
                {
                    TriggerUserId = userId,
                    TriggerData = data,
                    TriggerEntityType = entityType,
                    TriggerCommand = cmd
                };
                trigger.OnTrigger();
            });
        }
        /// <summary>
        /// 数据状态检测
        /// </summary>
        void OnTrigger()
        {
            using (SystemContextScope.CreateScope())
            {
                using (MySqlDataBaseScope.CreateScope(MySqlDataBase.DefaultDataBase))
                {
                    DoDataChanged();
                }
            }
        }
        /// <summary>
        /// 数据状态检测
        /// </summary>
        void DoDataChanged()
        {
            switch (TriggerCommand)
            {
                case BusinessCommandType.Reset:
                    OnStart(TriggerData, TriggerEntityType, UserJobType.Edit);
                    break;
                case BusinessCommandType.AddNew:
                case BusinessCommandType.Update:
                    OnSave(TriggerData, TriggerEntityType);
                    break;
                case BusinessCommandType.Discard:
                    OnClose(TriggerData, TriggerEntityType, UserJobType.Edit, JobStatusType.Canceled);
                    break;
                case BusinessCommandType.Delete:
                    OnClose(TriggerData, TriggerEntityType, UserJobType.Edit, JobStatusType.Canceled);
                    break;
                case BusinessCommandType.Lock:
                    OnClose(TriggerData, TriggerEntityType, UserJobType.Edit);
                    break;
                case BusinessCommandType.Submit:
                    OnClose(TriggerData, TriggerEntityType, UserJobType.Edit);
                    break;
                case BusinessCommandType.Back:
                    OnBack(TriggerData, TriggerEntityType);
                    break;
                case BusinessCommandType.Pullback:
                    OnPullback(TriggerData, TriggerEntityType);
                    break;
                case BusinessCommandType.Deny:
                    OnAuditEnd(TriggerData, TriggerEntityType, false);
                    break;
                case BusinessCommandType.Pass:
                    OnAuditEnd(TriggerData, TriggerEntityType, true);
                    break;
                case BusinessCommandType.ReAudit:
                    OnReEdit(TriggerData, TriggerEntityType);
                    break;
            }
        }
        /// <summary>
        /// 流程处理
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <param name="entityType"></param>
        /// <param name="userJob"></param>
        private void OnStart(TData data, int entityType, UserJobType userJob)
        {
            var job = business.Access.First(p => p.EntityType == entityType && p.LinkId == data.Id &&
                                        p.JobType == userJob && p.JobStatus < JobStatusType.Succeed);
            if (job == null)
            {
                AddNew(new UserJobData
                {
                    EntityType = entityType,
                    LinkId = data.Id,
                    Title = data.Title,
                    JobType = userJob,
                    JobStatus = JobStatusType.None,
                    FromUserId = TriggerUserId,
                    ToUserId = TriggerUserId
                });
            }
            else
            {
                business.Access.SetValue(p => p.JobStatus, JobStatusType.None, job.Id);
                job.Title = data.Title;
                job.JobStatus = JobStatusType.None;
                job.ToUserId = TriggerUserId;
                job.ToUserName = GetUserName(TriggerUserId);
                business.Access.Update(job);
            }
        }
        /// <summary>
        /// 流程处理
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <param name="entityType"></param>
        private void OnSave(TData data, int entityType)

        {
            var job = business.Access.First(p => p.EntityType == entityType && p.LinkId == data.Id &&
                                        p.JobType == UserJobType.Edit && p.JobStatus < JobStatusType.Succeed);
            var user = GetUserName(TriggerUserId);
            StringBuilder msg = new StringBuilder();
            if (job == null)
            {
                msg.Append($"{GetUserName(data.AuthorID)}在{data.AddDate:yyyy-M-d HH:mm:ss}新增");
                if (data.LastReviserID > 0)
                    msg.Append($"；{GetUserName(data.AuthorID)}在{data.AddDate:yyyy-M-d HH:mm:ss}修改");
                AddNew(new UserJobData
                {
                    EntityType = entityType,
                    LinkId = data.Id,
                    Title = data.Title,
                    JobType = UserJobType.Edit,
                    JobStatus = JobStatusType.None,
                    FromUserId = TriggerUserId,
                    ToUserId = TriggerUserId,
                    FromUserName = GetUserName(TriggerUserId),
                    ToUserName = user,
                    Message = msg.ToString()
                });
            }
            else
            {
                job.Title = data.Title;
                job.JobStatus = JobStatusType.None;
                job.ToUserId = TriggerUserId;
                job.ToUserName = GetUserName(TriggerUserId);
                if (string.IsNullOrWhiteSpace(job.Message))
                {
                    if (data.LastReviserID > 0)
                        msg.Append($"；{GetUserName(data.AuthorID)}在{data.AddDate:yyyy-M-d HH:mm:ss}修改");
                    job.Message = msg.ToString();
                }
                else
                    job.Message += $"；{job.ToUserName}在{data.LastModifyDate:yyyy-M-d HH:mm:ss}修改";
                business.Access.Update(job);
            }
        }
        /// <summary>
        /// 流程处理
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <param name="entityType"></param>
        private void OnPullback(TData data, int entityType)
        {
            string message = $"被{GetUserName(TriggerUserId)}拉回编辑";
            var jobs = business.Access.All(p => p.EntityType == entityType && p.LinkId == data.Id &&
                                       p.JobType == UserJobType.Audit && p.JobStatus < JobStatusType.Succeed);
            foreach (var job in jobs)
            {
                if (string.IsNullOrWhiteSpace(job.Message))
                    job.Message = message;
                else
                    job.Message += "；" + message;
                job.JobStatus = JobStatusType.NoHit;
                job.DataState = DataStateType.Discard;
                business.Access.Update(job);
            }
            AddNew(new UserJobData
            {
                EntityType = entityType,
                LinkId = data.Id,
                Title = data.Title,
                Message = message,
                JobType = UserJobType.Edit,
                FromUserId = TriggerUserId,
                ToUserId = TriggerUserId
            });
        }
        /// <summary>
        /// 流程处理
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <param name="entityType"></param>
        private void OnReEdit(TData data, int entityType)

        {
            var now = business.Access.Last(p => p.EntityType == entityType && p.LinkId == data.Id &&
                                       p.ToUserId == TriggerUserId &&
                                       p.JobType == UserJobType.Audit && p.JobStatus < JobStatusType.Succeed) ??
                      business.Access.Last(p => p.EntityType == entityType && p.LinkId == data.Id &&
                                       p.JobType == UserJobType.Edit);
            AddNew(new UserJobData
            {
                EntityType = entityType,
                LinkId = data.Id,
                Title = data.Title,
                Message = $"由{GetUserName(TriggerUserId)}重新启动编辑",
                JobType = UserJobType.Edit,
                JobStatus = JobStatusType.None,
                FromUserId = TriggerUserId,
                ToUserId = now?.FromUserId ?? 0
            });
        }

        /// <summary>
        /// 关闭工作
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <param name="entityType"></param>
        /// <param name="userJob"></param>
        /// <param name="status"></param>
        private void OnClose(TData data, int entityType, UserJobType userJob, JobStatusType status = JobStatusType.Succeed)

        {
            var jobs = business.Access.All(p => p.EntityType == entityType && p.LinkId == data.Id &&
                                       p.JobType == userJob && p.JobStatus < JobStatusType.Succeed);
            if (userJob == UserJobType.Edit)
            {
                foreach (var job in jobs)
                {
                    if (job.ToUserId == TriggerUserId)
                    {
                        business.Close(job.Id);
                    }
                    business.Access.SetValue(p => p.JobStatus, status, job.Id);
                }
            }
            else
            {
                foreach (var job in jobs)
                {
                    if (job.ToUserId == TriggerUserId)
                    {
                        business.Access.SetValue(p => p.JobStatus, status, job.Id);
                        business.Close(job.Id);
                    }
                    else if (data.AuditState == AuditStateType.Deny)
                    {
                        business.Access.SetValue(p => p.JobStatus, JobStatusType.NoHit, job.Id);
                        business.Close(job.Id);
                    }
                }
            }
        }

        /// <summary>
        /// 流程处理
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <param name="entityType"></param>
        private void OnBack(TData data, int entityType)
        {
            var jobs = business.Access.All(p => p.EntityType == entityType && p.LinkId == data.Id &&
                                       p.JobType == UserJobType.Audit && p.JobStatus < JobStatusType.Succeed);
            foreach (var job in jobs)
            {
                if (job.ToUserId == TriggerUserId)
                    job.DataState = DataStateType.Enable;
                job.JobStatus = job.ToUserId == TriggerUserId ? JobStatusType.Canceled : JobStatusType.NoHit;
                business.Access.Update(job);
            }
            //回到申请者
            var now = business.Access.Last(p => p.EntityType == entityType && p.LinkId == data.Id &&
                                       p.ToUserId == TriggerUserId &&
                                       p.JobType == UserJobType.Audit && p.JobStatus < JobStatusType.Succeed) ??
                      business.Access.Last(p => p.EntityType == entityType && p.LinkId == data.Id &&
                                       p.JobType == UserJobType.Edit);
            AddNew(new UserJobData
            {
                EntityType = entityType,
                LinkId = data.Id,
                Title = data.Title,
                Message = $"由{GetUserName(TriggerUserId)}退回编辑",
                JobType = UserJobType.Edit,
                FromUserId = TriggerUserId,
                ToUserId = now?.FromUserId ?? 0
            });
        }
        /// <summary>
        /// 关闭工作
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <param name="entityType"></param>
        /// <param name="pass"></param>
        private void OnAuditEnd(TData data, int entityType, bool pass)
        {
            Expression<Func<UserJobData, bool>> lambda;

            if (data.AuditState == AuditStateType.Deny)
                lambda = p => p.EntityType == entityType &&
                              p.LinkId == data.Id &&
                              p.JobType == UserJobType.Audit &&
                              p.JobStatus < JobStatusType.Succeed;
            else
                lambda = p => p.EntityType == entityType &&
                              p.LinkId == data.Id &&
                              p.JobType == UserJobType.Audit &&
                              p.JobStatus < JobStatusType.Succeed &&
                              p.ToUserId == TriggerUserId;

            string message = $"已由{GetUserName(TriggerUserId)}{(!pass ? "否决" : "审批通过")}";
            var jobs = business.Access.All(lambda);
            foreach (var job in jobs)
            {
                job.JobStatus = job.ToUserId == TriggerUserId
                    ? JobStatusType.Succeed
                    : JobStatusType.NoHit;
                if (job.ToUserId == TriggerUserId)
                {
                    job.DataState = DataStateType.Discard;
                }
                business.Access.Update(job);
            }
            //回到申请者
            var last = business.Access.Last(p => p.EntityType == entityType && p.LinkId == data.Id &&
                                        p.JobType == UserJobType.Edit && p.JobStatus == JobStatusType.Succeed) ??
                       business.Access.Last(p => p.EntityType == entityType && p.LinkId == data.Id &&
                                        p.JobType == UserJobType.Edit);
            if(last != null)
                AddNew(new UserJobData
                {
                    LinkId = data.Id,
                    Title = data.Title,
                    EntityType = entityType,
                    Message = message,
                    JobType = UserJobType.Message,
                    FromUserId = TriggerUserId,
                    ToUserId = last.FromUserId
                });
        }

        /// <summary>
        ///     新增
        /// </summary>
        void AddNew(UserJobData data)
        {
            if (data.ToUserId > 0)
            {
                business.AddNew(data);
            }
            else
            {
                /*List<int> users = data.JobType == UserJobType.Audit
                    ? GetAuditUsers()
                    : GetEditUsers();
                foreach (var user in users)
                {
                    data.ToUserId = user;
                    business.AddNew(data);
                }*/
            }
        }
        /// <summary>
        ///     得到缓存页面的编辑用户
        /// </summary>
        public static List<int> GetEditUsers()
        {
            using (var proxy = new RedisProxy(RedisProxy.DbAuthority))
            {
                return proxy.Get<List<int>>($"users:edit:{typeof(TData).FullName}");
            }
        }

        /// <summary>
        ///     得到缓存页面的审批用户
        /// </summary>
        public static List<int> GetAuditUsers()
        {
            using (var proxy = new RedisProxy(RedisProxy.DbAuthority))
            {
                return proxy.Get<List<int>>($"users:audit:{typeof(TData).FullName}");
            }
        }

        /// <summary>
        /// 取用户的名字
        /// </summary>
        /// <param name="id"></param>
        internal string GetUserName(int id)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return proxy.Get(DataKeyBuilder.ToKey("user", "name", id))?.Trim('"');
            }
        }
    }
}
