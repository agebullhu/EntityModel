/*design by:agebull designer date:2017/6/9 11:34:09*/

using System;
using System.Collections.Generic;
using System.Linq;
using Agebull.Common;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.WebUI;
using Gboxt.Common.Workflow.BusinessLogic;
using Gboxt.Common.Workflow.DataAccess;

namespace Gboxt.Common.Workflow.UserJobPage
{
    public partial class Action : ApiPageBaseForDataState<UserJobData, UserJobDataAccess, UserJobBusinessLogic>
    {
        public Action()
        {
            IsPublicPage = true;
        }
        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected override void GetListData()
        {
            var filter = new LambdaItem<UserJobData>();
            if (LoginUser.Id > 1)
            {
                filter.Root = p => p.ToUserId == LoginUser.Id && p.DataState == DataStateType.None;
            }
            else
            {
                filter.Root = p => p.DataState == DataStateType.None;
            }
            SetArg("order", "desc");
            SetArg("order", "Id");
            var size = LoginUser.Id > 1?GetIntArg("size", 6):9999;
            var type = GetArg("type");
            List<UserJobData> datas;
            switch (type)
            {
                case "msg":
                    filter.AddAnd(p => p.JobType == UserJobType.Message);
                    break;
                case "audit":
                    filter.AddAnd(p => p.JobType == UserJobType.Audit);
                    if (LoginUser.Id > 1)
                        filter.AddAnd(p => p.JobStatus < JobStatusType.Succeed);
                    break;
                case "edit":
                    filter.AddAnd(p => p.JobType == UserJobType.Edit);
                    if (LoginUser.Id > 1)
                        filter.AddAnd(p => p.JobStatus < JobStatusType.Succeed);
                    break;
                default:
                    datas = Business.Access.PageData(1, 100, p => p.Date, true, filter);
                    SetResult(datas);
                    return;
            }
            datas = Business.Access.PageData(1, size, p => p.Date, true, filter);
            SetResult(datas);
        }
        /// <summary>
        /// 设置操作数据
        /// </summary>
        /// <param name="datas"></param>
        protected void SetResult(IList<UserJobData> datas)
        {
            foreach (var data in datas)
            {
                Business.InitJobByUi(data);
                if (LoginUser.Id == 1)
                    data.Title += $"({data.ToUserName})";
            }
            SetDataGirdResult(datas);
        }

        /// <summary>
        ///     执行操作
        /// </summary>
        /// <param name="action">传入的动作参数,已转为小写</param>
        protected override void DoActinEx(string action)
        {
            switch (action)
            {
                case "close":
                    Business.Close(GetIntArg("id"));
                    SetResultData(true);
                    break;
                default:
                    base.DoActinEx(action);
                    break;
            }
        }

        /// <summary>
        ///     载入当前操作的数据
        /// </summary>
        protected override void OnDetailsLoaded(UserJobData data, bool isNew)
        {
            base.OnDetailsLoaded(data, isNew);
            if (!isNew)
                Business.InitJobByUi(data);
        }

        /// <summary>
        ///     新增一条带默认值的数据
        /// </summary>
        public override UserJobData CreateData()
        {
            return new UserJobData
            {
                FromUserId = LoginUser.Id,
                LinkId = GetIntArg("lid", 0),
                EntityType = GetIntArg("eid", 0),
                JobType = UserJobType.Audit,
                Date = DateTime.Now,
                Message = "内容已编辑完成,请领导审核通过为盼"
            };
        }

        /// <summary>
        /// 读取Form传过来的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="convert">转化器</param>
        protected override void ReadFormData(UserJobData data, FormConvert convert)
        {
            DefaultReadFormData(data, convert);
            data.FromUserId = LoginUser.Id;
            data.LinkId = GetIntArg("lid", 0);
            data.EntityType = GetIntArg("eid", 0);
            data.JobType = UserJobType.Audit;
            var users = GetIntArrayArg("ToUserIds");
            if (users.Length == 0)
            {
                throw new AgebullBusinessException("必须有接收者");
            }
            if (BusinessContext.Current.LoginUser.RoleId > 1 && users.Any(p => p == BusinessContext.Current.LoginUserId))
            {
                throw new AgebullBusinessException("不得提交给自己");
            }
            users = users.Where(p => p > 0).Distinct().ToArray();
            if (users.Length == 0)
            {
                throw new AgebullBusinessException("必须有接收者");
            }
            var ids = GetIntArrayArg("ids").Distinct().ToArray();
            if (ids.Length == 0)
            {
                throw new AgebullBusinessException("必须有数据");
            }
            bool first = true;
            var cpy = new UserJobData();
            cpy.CopyValue(data);
            string title = data.Title;
            foreach (var user in users)
            {
                foreach (var id in ids)
                {
                    var wd = WorkflowControler.LoadData(data.EntityType, id);

                    if (first)
                    {
                        data.ToUserId = user;
                        data.LinkId = id;
                        if (title != null && wd != null)
                            data.Title = title.Replace("*", wd.Title);
                        first = false;
                    }
                    else
                    {
                        cpy.ToUserId = user;
                        cpy.LinkId = id;
                        if (title != null && wd != null)
                            cpy.Title = title.Replace("*", wd.Title);
                        Business.AddNew(cpy);
                    }
                }
            }
        }
    }
}