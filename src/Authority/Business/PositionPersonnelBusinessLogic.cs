using System.Threading.Tasks;
using Agebull.Common.DataModel.Redis;
using Gboxt.Common.DataModel;
using Agebull.SystemAuthority.Organizations.DataAccess;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.Workflow;

namespace Agebull.SystemAuthority.Organizations.BusinessLogic
{
    /// <summary>
    /// 员工职位关联
    /// </summary>
    public sealed partial class PositionPersonnelBusinessLogic : AutoJobBusinessLogicBase<PositionPersonnelData, PositionPersonnelDataAccess>
    {
        #region 同步用户

        /// <summary>
        ///     构造
        /// </summary>
        public PositionPersonnelBusinessLogic()
        {
            unityStateChanged = true;
        }
        /// <summary>
        ///     状态改变后的统一处理(unityStateChanged不设置为true时不会产生作用--基于性能的考虑)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override void DoStateChanged(PositionPersonnelData data)
        {
            //Task.Factory.StartNew(() => CacheTask(data.PersonnelId));
            //base.DoStateChanged(data);
        }

        protected override void OnAuditPassed(PositionPersonnelData data)
        {
            base.OnAuditPassed(data);
            CacheTask(data.PersonnelId);
        }

        static void CacheTask(int pid)
        {
            var orb = new UserBusinessLogic();
            using (SystemContextScope.CreateScope())
            {
                using (MySqlDataBaseScope.CreateScope(MySqlDataBase.DefaultDataBase))
                {
                    orb.SyncUser(orb._posAccess.First(pid));
                    RoleCache cache = new RoleCache();
                    cache.CachePageAuditUser();
                    cache.CacheTypeUser();
                }
            }
        }
        #endregion

        #region 扩展保存
        /// <summary>
        ///     被用户编辑的数据的保存前操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool PrepareSaveByUser(PositionPersonnelData data, bool isAdd)
        {
            if (null == data.Mobile)
            {
                BusinessContext.Current.LastMessage = "手机号码用于登录系统不能为空";
                return false;
            }
            if (Access.Any(p => p.Id != data.Id && p.Mobile == data.Mobile))
            {
                BusinessContext.Current.LastMessage = "手机号码用于登录系统必须唯一";
                return false;
            }
            return base.PrepareSaveByUser(data, isAdd);
        }

        /// <summary>
        ///     保存前的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool OnSaving(PositionPersonnelData data, bool isAdd)
        {
            var personnel = new PersonnelData
            {
                FullName = data.Personnel,
                Six = data.Six,
                Birthday = data.Birthday,
                Tel = data.Tel,
                RoleId = data.RoleId,
                Mobile = data.Mobile
            };
            var access = new PersonnelDataAccess();
            if (data.PersonnelId == 0)
            {
                access.Insert(personnel);
                data.PersonnelId = personnel.Id;
            }
            else
            {
                personnel.Id = data.PersonnelId;
                access.Update(personnel);
            }
            return base.OnSaving(data, isAdd);
        }

        #endregion

    }
}
