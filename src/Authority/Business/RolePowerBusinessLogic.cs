// // /*****************************************************
// // (c)2016-2017 Copy right Agebull
// // 作者:
// // 工程:Agebull.SystemAuthority.Organizations
// // 建立:2016-06-12
// // 修改:2016-06-12
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agebull.ProjectDeveloper.WebDomain.Models;
using Gboxt.Common.DataModel.BusinessLogic;
using Agebull.SystemAuthority.Organizations.DataAccess;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;

#endregion

namespace Agebull.SystemAuthority.Organizations.BusinessLogic
{
    public sealed partial class RolePowerBusinessLogic : BusinessLogicBase<RolePowerData, RolePowerDataAccess>
    {
        /// <summary>
        /// 保存角色页面权限设置
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="sels"></param>
        public static void SaveRolePagePower(int roleId, string sels)
        {
            var pAccess = new RolePowerDataAccess();
            pAccess.Delete(p => p.RoleId == roleId);
            if (!string.IsNullOrWhiteSpace(sels))
            {
                var sids = sels.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (sids.Length != 0)
                {
                    foreach (var line in sids.Select(p => p.Split(',')))
                    {
                        pAccess.Insert(new RolePowerData
                        {
                            RoleId = roleId,
                            PageItemId = int.Parse(line[0]),
                            Power = 1,
                            DataScope = (PositionDataScopeType)int.Parse(line[1])
                        });
                    }
                }
            }
            Task.Factory.StartNew(() => CacheTask(roleId));
        }

        static void CacheTask(int roleId)
        {
            using (SystemContextScope.CreateScope())
            {
                using (MySqlDataBaseScope.CreateScope(MySqlDataBase.DefaultDataBase))
                {
                    RoleCache.Cache(roleId);

                    RoleCache cache = new RoleCache();
                    cache.CachePageAuditUser();
                    cache.CacheTypeUser();
                }
            }
        }
        /// <summary>
        /// 载入角色的树形页面权限数据
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>树形页面权限数据</returns>
        public static List<EasyUiTreeNode> LoadPowers(int roleId)
        {
            return new RolePowerBusinessLogic().LoadRolePowers(roleId);
        }

        /// <summary>
        /// 载入角色的树形页面权限数据
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>树形页面权限数据</returns>
        private List<EasyUiTreeNode> LoadRolePowers(int roleId)
        {
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                var pAccess = new RolePowerDataAccess
                {
                    DataBase = Access.DataBase
                };
                var powers = pAccess.All(p => p.RoleId == roleId);

                Dictionary<int, RolePowerData> dictionary ;
                try
                {
                    dictionary = powers.ToDictionary(p => p.PageItemId);
                }
                catch// (Exception e)
                {
                    dictionary = new Dictionary<int, RolePowerData>();
                    foreach (var g in powers.GroupBy(p => p.PageItemId))
                    {
                        dictionary.Add(g.Key, g.First());
                        foreach (var item in g.Skip(1))
                        {
                            pAccess.DeletePrimaryKey(item.Id);
                        }
                    }
                }

                var tree = RoleCache.LoadPowerTree();
                SyncPower(tree, dictionary);
                return new List<EasyUiTreeNode>
                {
                    tree
                };
            }
        }
        /// <summary>
        /// 载入角色的树形页面权限数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="powers"></param>
        /// <returns></returns>
        static void SyncPower(EasyUiTreeNode node,  Dictionary<int, RolePowerData> powers)
        {
            if (node.Children == null)
                return;
            if (node.Tag == "page")
                node.IsOpen = false;
            foreach (var item in node.Children)
            {
                SyncPower(item, powers);
                RolePowerData pwoer;
                if (!powers.TryGetValue(item.ID, out pwoer))
                    continue;
                item.Extend = item.Extend;
                item.Attributes = ((int)pwoer.DataScope).ToString();
                item.IsSelect = true;
            }
        }
    }
}