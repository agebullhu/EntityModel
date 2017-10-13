// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-29
// // *****************************************************/

#region 引用

using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.WebUI;
using Gboxt.Common.SystemModel.DataAccess;

#endregion

namespace Gboxt.Common.SystemModel.PageItemPage
{
    public class PageItemPageAction : ApiPageBaseEx<PageItemData, PageItemDataAccess, PageItemLogical>
    {
        public PageItemPageAction()
        {
            AllAction = true;
            IsPublicPage = true;
        }
        

        public override PageItemData CreateData()
        {
            return new PageItemData
            {
                ParentId = GetIntArg("fid")
            };
        }

        /// <summary>
        ///     执行操作
        /// </summary>
        /// <param name="action">传入的动作参数,已转为小写</param>
        protected override void DoActinEx(string action)
        {
            switch (action)
            {
                case "set_parent":
                    OnSetParent();
                    break;
                case "flush_cache":
                    OnSetParent();
                    break;
                case "normal_buttons":
                    Business.CheckNormalButtons(GetIntArg("id"), GetArg("type")); ;
                    break;
                case "tree":
                    GetTree();
                    break; 
            }
        }

        private void GetTree()
        {
            SetCustomJsonResult(Business.LoadTreeForUi(GetIntArg("id",0)));
        }

        private void OnSetParent()
        {
           IsFailed = !Business.SetParent(GetArg("selects"), GetArg("parent"));

        }
        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected override void GetListData()
        {
            var root = new LambdaItem<PageItemData>();
            var fid = GetIntArg("fid");
            if (fid >= 0)
            {
                root.Root = p => p.ParentId == fid;
            }
            else
            {
                root.Root = p => p.ItemType <= PageItemType.Page ;
            }
            var keyWord = GetArg("keyWord");
            if (!string.IsNullOrEmpty(keyWord))
            {
                root.AddAnd(p => p.Name.Contains(keyWord) || p.Caption.Contains(keyWord) || p.Url.Contains(keyWord));
            }
            base.GetListData(root);
        }
        
        /// <summary>
        ///     读取Form传过来的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="convert">转化器</param>
        protected override void ReadFormData(PageItemData data, FormConvert convert)
        {
            data.Name = convert.ToString("Name", false);
            data.Json = convert.ToString("Json", false);
            data.ExtendValue = convert.ToString("ExtendValue", false);
            data.Caption = convert.ToString("Caption", false);
            //data.Folder = convert.ToString("Folder", false);
            data.Url = convert.ToString("Url", true);
            data.Icon = convert.ToString("Icon", true);
            data.ItemType = (PageItemType)convert.ToInteger("ItemType", 0);
            data.Index = convert.ToInteger("Index", 0);
            data.ParentId = convert.ToInteger("ParentId", 0);
            data.Memo = convert.ToString("Memo", true);


            data.SystemType = convert.ToString("type");
            data.IsHide = convert.ToBoolean("hide");
            data.Audit = convert.ToBoolean("audit");
            data.LevelAudit = convert.ToBoolean("level_audit");
            data.DataState = convert.ToBoolean("data_state");
            data.AuditPage = convert.ToInteger("audit_page");
            data.Edit = convert.ToBoolean("edit");
            data.MasterPage = convert.ToInteger("master_page"); 
        }
        
    }
}