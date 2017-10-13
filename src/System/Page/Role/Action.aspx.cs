// // /*****************************************************
// // (c)2016-2016 Copy right 上海元亨详投资基金集团有限公司
// // 作者:
// // 工程:YhxBank.FundsManagement
// // 建立:2016-06-12
// // 修改:2016-06-29
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Linq;
using Agebull.ProjectDeveloper.WebDomain.Models;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.WebUI;
using Gboxt.Common.SystemModel.DataAccess;

#endregion

namespace Gboxt.Common.SystemModel.RolePage
{
    public class Action : ApiPageBaseEx<RoleData, RoleDataAccess, RoleBusinessLogic>
    {
        protected override void DoActinEx(string action)
        {
            switch (action)
            {
                case "powers":
                    LoadPowers();
                    break;
                case "roles":
                    LoadAll();
                    break;
                case "savepowers":
                    SavePowers();
                    break;
                default:
                    base.DoActinEx(action);
                    break;
            }
        }

        private void LoadAll()
        {
            SetCustomJsonResult(Business.All());
        }

        private void LoadPowers()
        {
            var pAccess = new RolePowerDataAccess();
            var powers = pAccess.All(p => p.RoleId == ContextDataId);
            var iAccess = new PageItemDataAccess();
            SetCustomJsonResult(
                new List<EasyUiTreeNode>
                {
                    new EasyUiTreeNode
                    {
                        ID = 0,
                        Text = "微信预约系统",
                        Title = "微信预约系统",
                        IsOpen = true,
                        IsSelect = true,
                        Children = LoadTree(iAccess, 0, powers)
                    }
                });
        }

        private static List<EasyUiTreeNode> LoadTree(PageItemDataAccess access, int parent, List<RolePowerData> powers)
        {
            var data = access.All(p => p.ParentId == parent);
            return data.Select(item => new EasyUiTreeNode
            {
                ID = item.ID,
                Text = item.Name,
                Title = item.Caption,
                Memo = item.Memo,
                IsOpen = true,
                IsSelect = powers.Any(p => p.PageItemId == item.ID),
                Children = LoadTree(access, item.ID, powers)
            }).ToList();
        }

        private void SavePowers()
        {
            var sels = GetArg("selects");
            var pAccess = new RolePowerDataAccess();
            pAccess.Delete(p => p.RoleId == ContextDataId);
            var sids = sels.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            if (sids.Length == 0)
            {
                return;
            }
            foreach (var id in sids.Select(int.Parse).Where(id => id > 0))
            {
                pAccess.Insert(new RolePowerData
                {
                    RoleId = this.ContextDataId,
                    PageItemId = id,
                    Power = 1
                });
            }
        }

        /// <summary>
        ///     读取Form传过来的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="convert">转化器</param>
        protected override void ReadFormData(RoleData data, FormConvert convert)
        {
            data.Role = convert.ToString("Role", false);
            data.Caption = convert.ToString("Caption", false);
            data.Memo = convert.ToString("Memo", true);
        }
    }
}