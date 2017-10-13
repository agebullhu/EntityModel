/*design by:agebull designer date:2017/6/9 11:34:09*/
using System;
using System.Collections.Generic;
using System.Text;
using Agebull.Common.DataModel;
using Agebull.Common.DataModel.Redis;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.Workflow.DataAccess;

namespace Gboxt.Common.Workflow.BusinessLogic
{
    /// <summary>
    /// 用户工作列表
    /// </summary>
    public sealed partial class UserJobBusinessLogic : BusinessLogicByStateData<UserJobData, UserJobDataAccess>
    {
        #region 操作

        /// <summary>
        ///     被用户编辑的数据的保存前操作
        /// </summary>
        /// <param name="job">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool PrepareSave(UserJobData job, bool isAdd)
        {
            if (job.ToUserId == 0)
            {//查找有权用户

            }
            if (isAdd)//查找旧数据,防止重复
            {
                job.Id = Access.LoadValue(p => p.Id, p => p.EntityType == job.EntityType && p.DataId == job.DataId
                                                       && p.ToUserId == job.ToUserId && p.JobStatus < JobStatusType.Succeed);
                isAdd = job.Id == 0;
                job.__EntityStatus.IsExist = !isAdd;//让后面的保存改为Update
            }
            return base.PrepareSave(job, isAdd);
        }


        ///// <summary>
        /////     新增
        ///// </summary>
        //public override bool AddNew(UserJobData data)
        //{
        //    var job = Access.First(p => p.EntityType == data.EntityType && p.LinkId == data.LinkId &&
        //                                p.ToUserId == data.ToUserId && p.FromUserId == data.FromUserId &&
        //                                p.JobStatus < JobStatusType.Succeed);
        //    if (job != null)
        //    {
        //        data.Id = job.Id;
        //        return Update(data);
        //    }
        //    return base.AddNew(data);
        //}

        /// <summary>
        /// 初始化UI操作的信息
        /// </summary>
        /// <param name="job"></param>
        public void InitJobByUi(UserJobData job)
        {
            switch (job.JobType)
            {
                case UserJobType.Audit:
                    job.UserJob = "audit";
                    break;
                case UserJobType.Edit:
                    job.UserJob = "edit";
                    break;
                default:
                    job.UserJob = "msg";
                    break;
            }
            WorkflowControler.SetJobUiInfomation(job);
        }
        /// <summary>
        /// 设置已读
        /// </summary>
        /// <param name="id"></param>
        public void Close(int id)
        {
            Access.SetValue(p => p.DataState, DataStateType.Discard, id);
        }
        #endregion

        #region ItemInfo

        public class JobInfo
        {
            public UserJobData Job;

            public IWorkflowData Data;

            public List<JobItem> Items = new List<JobItem>();
        }
        public class JobItem
        {
            public string Icon;

            public string Title;

            public string Message;
        }

        public IWorkflowData GetJobData(int id)
        {
            var job = Details(id);
            return WorkflowControler.LoadData(job.EntityType, job.LinkId);
        }

        public JobInfo GetJobInfo(int id)
        {
            JobInfo info = new JobInfo
            {
                Job = Details(id)
            };
            if (info.Job == null)
                return info;
            var alls = Access.All(p => p.LinkId == info.Job.LinkId && p.EntityType == info.Job.EntityType);
            InitJobByUi(info.Job);
            foreach (var job in alls)
            {
                job.FromUserName = UserJobDataAccess.GetUserName(job.FromUserId);
                job.ToUserName = UserJobDataAccess.GetUserName(job.ToUserId);
                JobItem item = new JobItem();
                StringBuilder head = new StringBuilder();
                if (job.JobType == UserJobType.Edit)
                {
                    item.Icon = "wf_edit.png";
                    if (!string.IsNullOrWhiteSpace(job.FromUserName))
                    {
                        head.Append(job.FromUserName);
                        if (job.Date > DateTime.MinValue)
                        {
                            head.Append($"({job.Date:yyyy-MM-dd HH:mm:ss})");
                        }
                    }
                    else if (info.Data != null)
                    {
                        head.Append(UserJobDataAccess.GetUserName(info.Data.AuthorID));
                        if (info.Data.AddDate > DateTime.MinValue)
                        {
                            head.Append($"({info.Data.AddDate:yyyy-MM-dd HH:mm:ss})");
                        }
                    }
                }
                else if (job.JobType == UserJobType.Audit)
                {
                    switch (job.JobStatus)
                    {
                        case JobStatusType.Trans:
                            item.Icon = "wf_trans.png";
                            break;
                        case JobStatusType.Notify:
                            item.Icon = "wf_notify.png";
                            break;
                        //case JobStatusType.None:
                        default:
                            item.Icon = "wf_audit.png";
                            break;
                        case JobStatusType.Succeed:
                            item.Icon = "wf_succeed.png";
                            break;
                        case JobStatusType.Canceled:
                            item.Icon = "wf_canceled.png";
                            break;
                        case JobStatusType.NoHit:
                            item.Icon = "wf_failed.png";
                            break;
                    }
                    head.AppendFormat("{0} > {1}", job.FromUserName ?? "系统", job.ToUserName ?? "系统");
                    if (job.Date > DateTime.MinValue)
                    {
                        head.Append($"({job.Date:yyyy-MM-dd HH:mm:ss})");
                    }
                }
                else
                {
                    if (job.ToUserId != BusinessContext.Current.LoginUserId)
                        continue;
                    item.Icon = "wf_notify.png";
                    head.AppendFormat("{0} > {1}", job.FromUserName ?? "系统", job.ToUserName ?? "系统");
                    if (job.Date > DateTime.MinValue)
                    {
                        head.Append($"({job.Date:yyyy-MM-dd HH:mm:ss})");
                    }
                }
                item.Title = head.ToString();
                item.Message = job.Message;
                info.Items.Add(item);
            }
            return info;
        }

        #endregion
    }
}