using System.Linq;
using Agebull.Common.DataModel.Redis;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.DataModel.MySql;
using Agebull.SystemAuthority.Organizations.DataAccess;
using Gboxt.Common.Workflow;

namespace Agebull.SystemAuthority.Organizations.BusinessLogic
{
    /// <summary>
    /// 职位组织关联
    /// </summary>
    public sealed partial class OrganizePositionBusinessLogic : BusinessLogicByHistory<OrganizePositionData, OrganizePositionDataAccess>
    {
        /// <summary>
        /// 添加所有主管与办事员
        /// </summary>
        public void CreateAll()
        {
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                var oAccess = new OrganizationDataAccess();
                var orgs = oAccess.All(p => p.DataState < DataStateType.Delete);
                foreach (var org in orgs)
                {
                    var leader = org.FullName.Last() + "长";
                    if (!Access.Any(p => p.OrganizationId == org.Id && p.RoleId == 3))
                    {
                        Access.Insert(new OrganizePositionData
                        {
                            Position = leader,
                            OrganizationId = org.Id,
                            RoleId = 3,
                            Memo = org.FullName + leader
                        });
                    }
                    else
                    {
                        Access.SetValue(p => p.DataState, DataStateType.None,
                            p => p.OrganizationId == org.Id && p.RoleId == 3 && p.DataState == DataStateType.Delete);
                    }
                    if (!Access.Any(p => p.OrganizationId == org.Id && p.RoleId == 4))
                    {
                        Access.Insert(new OrganizePositionData
                        {
                            Position = "办事员",
                            OrganizationId = org.Id,
                            RoleId = 4,
                            Memo = org.FullName + "办事员"
                        });
                    }
                    else
                    {
                        Access.SetValue(p => p.DataState, DataStateType.None,
                            p => p.OrganizationId == org.Id && p.RoleId == 4 && p.DataState == DataStateType.Delete);
                    }
                }
            }
        }

        #region 缓存


        /// <summary>
        ///     状态改变后的统一处理(unityStateChanged不设置为true时不会产生作用--基于性能的考虑)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override void DoStateChanged(OrganizePositionData data)
        {
            using (SystemContextScope.CreateScope())
            {
                using (var proxy = new RedisProxy(RedisProxy.DbSystem))
                {
                    var bl = new OrganizationBusinessLogic();
                    bl.CreateOrgPosTree(proxy);
                }
            }
            base.DoStateChanged(data);
        }
        
        #endregion
    }
}
