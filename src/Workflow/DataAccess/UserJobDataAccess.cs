/*design by:agebull designer date:2017/6/9 11:34:09*/


using System;
using Agebull.Common.DataModel;
using Agebull.Common.DataModel.Redis;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;

namespace Gboxt.Common.Workflow.DataAccess
{
    /// <summary>
    /// 用户工作列表
    /// </summary>
    sealed partial class UserJobDataAccess : MySqlTable<UserJobData>
    {
        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="job">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        protected override void OnPrepareSave(DataOperatorType operatorType, UserJobData job)
        {
            if (operatorType == DataOperatorType.Insert)
            {
                job.Date = DateTime.Now;
                job.FromUserName = GetUserName(job.FromUserId);
                job.ToUserName = GetUserName(job.ToUserId);
                job.DepartmentId = GetDepartmentId(job.ToUserId);
            }
            base.OnPrepareSave(operatorType, job);
        }


        /// <summary>
        /// 取用户的名字
        /// </summary>
        /// <param name="id"></param>
        internal static string GetUserName(int id)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return proxy.Get(DataKeyBuilder.ToKey("user", "name", id))?.Trim('"');
            }
        }
        /// <summary>
        /// 取用户的名字
        /// </summary>
        /// <param name="uid"></param>
        static int GetDepartmentId(int uid)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return proxy.GetValue<int>(DataKeyBuilder.ToKey("user", "depid", uid));
            }
        }
    }
}