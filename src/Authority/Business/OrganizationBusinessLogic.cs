using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agebull.Common.DataModel;
using Agebull.Common.DataModel.Redis;
using Agebull.ProjectDeveloper.WebDomain.Models;
using Agebull.SystemAuthority.Organizations.DataAccess;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.WebUI;
using Gboxt.Common.Workflow;

namespace Agebull.SystemAuthority.Organizations.BusinessLogic
{
    /// <summary>
    ///     组织机构
    /// </summary>
    public sealed partial class OrganizationBusinessLogic : BusinessLogicByHistory<OrganizationData, OrganizationDataAccess>
    {

        #region 上下级同步

        public OrganizationBusinessLogic()
        {
            unityStateChanged = true;
        }
        /// <summary>
        ///     状态改变后的统一处理(unityStateChanged不设置为true时不会产生作用--基于性能的考虑)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override void DoStateChanged(OrganizationData data)
        {
            base.DoStateChanged(data);
            SyncTreeInfo(data);
            Cache();
        }

        /// <summary>
        ///     保存前的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool OnSaving(OrganizationData data, bool isAdd)
        {
            if (isAdd)
            {
                if (data.ParentId <= 0)
                {
                    BusinessContext.Current.LastMessage = "顶级区域不能通过用户界面添加";
                    return false;
                }
                if (data.ParentId > 0)
                    return Access.ExistPrimaryKey(data.ParentId);

            }
            return true;
        }


        /// <summary>
        ///     设置一个对象的隶属关系
        /// </summary>
        /// <param name="entity">数据</param>
        private void SyncTreeInfo(OrganizationData entity)
        {
            if (entity == null)
                return;
            if (entity.ParentId == 0)
            {
                entity.OrgId = entity.Id;
                entity.FullName = entity.ShortName;
                entity.TreeName = entity.ShortName;
                SyncChild(entity);
                return;
            }
            var parent = Access.LoadByPrimaryKey(entity.ParentId);
            if (parent == null)
            {
                entity.OrgId = entity.Id;
                entity.ParentId = 1;
                entity.FullName = entity.ShortName;
                entity.TreeName = entity.ShortName;
                SyncChild(entity);
                return;
            }
            SyncTreeInfo(parent, entity);
        }

        /// <summary>
        ///     同步树关系
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="entity">数据</param>
        private void SyncTreeInfo(OrganizationData parent, OrganizationData entity)
        {
            if (parent.DataState >= DataStateType.Disable)
                entity.DataState = parent.DataState;
            if (entity.Type.HasFlag(OrganizationType.Area))
            {
                entity.OrgId = entity.Id;
                entity.FullName = entity.ShortName;
                entity.TreeName = entity.FullName;
            }
            else
            {
                entity.OrgId = parent.OrgId;
                entity.TreeName = parent.TreeName + ">" + entity.ShortName;
                entity.FullName = parent.TreeName + entity.ShortName;
            }
            SyncChild(entity);
        }


        /// <summary>
        ///     同步更改到下级
        /// </summary>
        /// <param name="entity">数据</param>
        private void SyncChild(OrganizationData entity)
        {
            Access.Update(entity);
            var children = Access.All(p => p.ParentId == entity.Id);
            foreach (var child in children)
            {
                SyncTreeInfo(entity, child);
            }
        }

        #endregion

        #region 编辑

        /// <summary>
        ///     载入完整的组织结构树(用于编辑)
        /// </summary>
        /// <returns></returns>
        internal OrganizationData[] LoadEditTree()
        {
            var root = new OrganizationData();
            var lists = Access.All(p => p.DataState < DataStateType.Delete);
            SetChildren(root, 0, lists);
            return root.Children;
        }

        /// <summary>
        ///     载入完整的组织结构树
        /// </summary>
        /// <returns></returns>
        internal OrganizationData[] LoadTree(int pid)
        {
            var lists = Access.All(p => p.DataState < DataStateType.Delete);
            var root = lists.FirstOrDefault(p => p.Id == pid);
            if (root == null)
                return null;
            SetChildren(root, pid, lists);
            return new[] { root };
        }

        /// <summary>
        ///     设置子级以构成树
        /// </summary>
        /// <param name="par"></param>
        /// <param name="pid"></param>
        /// <param name="lists"></param>
        private void SetChildren(OrganizationData par, int pid, List<OrganizationData> lists)
        {
            var childs = lists.Where(p => p.ParentId == pid).ToArray();
            if (childs.Length == 0)
                return;
            foreach (var child in childs)
                SetChildren(child, child.Id, lists);
            par.Children = childs;
        }
        #endregion

        #region 树

        #region 读取
        

        /// <summary>
        ///     载入完整的组织结构树(UI相关）
        /// </summary>
        /// <returns></returns>
        public static OrganizationData Get(int oid)
        {
            if (oid < 1)
                oid = 1;
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return proxy.GetEntity<OrganizationData>(oid);
            }
        }

        /// <summary>
        ///     载入完整的地区树(UI相关）
        /// </summary>
        /// <returns></returns>
        public static List<EasyUiTreeNode> LoadAreaTreeForUi(int oid)
        {
            if (oid < 1)
                oid = 1;
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return new List<EasyUiTreeNode> { proxy.Get<EasyUiTreeNode>("ui:org:areaTree:" + oid) };
            }
        }

        /// <summary>
        ///     载入完整的组织结构树(UI相关）
        /// </summary>
        /// <returns></returns>
        public static List<EasyUiTreeNode> LoadTreeForUi(int oid)
        {
            if (oid < 1)
                oid = 1;
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return new List<EasyUiTreeNode> { proxy.Get<EasyUiTreeNode>("ui:org:OrgTree:" + oid) };
            }
        }

        /// <summary>
        ///     载入完整的组织结构树(包含职位）
        /// </summary>
        /// <returns></returns>
        public static List<EasyUiTreeNode> LoadPostTreeForUi()
        {
            return LoadPostTreeForUi(BusinessContext.Current.LoginUser.DepartmentId);
        }

        /// <summary>
        ///     载入完整的组织结构树(包含职位）
        /// </summary>
        /// <returns></returns>
        public static List<EasyUiTreeNode> LoadPostTreeForUi(int oid)
        {
            if (oid < 1)
                oid = 1;
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return new List<EasyUiTreeNode> { proxy.Get<EasyUiTreeNode>("ui:org:PostTree:" + oid) };
            }
        }

        /// <summary>
        ///     生成完整的组织结构树(UI相关）
        /// </summary>
        /// <returns></returns>
        public static void Cache()
        {
            using (SystemContextScope.CreateScope())
            {
                var bl = new OrganizationBusinessLogic();
                using (var proxy = new RedisProxy(RedisProxy.DbSystem))
                {
                    bl.CreateFullOrgTree(proxy);
                    bl.CreateOrgPosTree(proxy);
                    bl.CreateAreaTree(proxy);
                    proxy.CacheData<OrganizationData, OrganizationDataAccess>();
                    proxy.CacheData<OrganizePositionData, OrganizePositionDataAccess>();
                    proxy.CacheData<PositionPersonnelData, PositionPersonnelDataAccess>(p => $"e:pp:{p.UserId}");
                }
            }
        }

        #endregion

        #region Helper

        public EasyUiTreeNode CreateRootNode()
        {
            return CreateNode(Access.First(1));
        }


        public static EasyUiTreeNode CreateNode(OrganizationData data, string attribute = "org")
        {
            return new EasyUiTreeNode
            {
                ID = data.Id,
                Icon = Icon(data.Type),
                IsOpen = true,
                IsFolder = true,
                Attributes = attribute,
                Title = data.TreeName,
                Text = data.ShortName
            };
        }

        public static EasyUiTreeNode CreateNode(OrganizePositionData data)
        {
            return new EasyUiTreeNode
            {
                ID = data.Id,
                Icon = "icon-post",
                IsOpen = true,
                IsFolder=true,
                Attributes = "post",
                Text = data.Position,
                Title = data.Department + data.Position,
                Tag = data.OrganizationId.ToString()
            };
        }

        public static string Icon(OrganizationType type)
        {
            switch (type)
            {
                case OrganizationType.Area:
                    return "icon-area";

                case OrganizationType.Organization:
                    return "icon-com";
                default:
                    //case OrganizationType.Department:
                    return "icon-bm";

            }
            //return "icon-bm";
        }
        #endregion

        #region 生成职位结构树

        /// <summary>
        ///     载入完整的组织结构树(包含职位）
        /// </summary>
        /// <returns></returns>
        internal void CreateOrgPosTree(RedisProxy proxy)
        {
            var root = CreateRootNode();
            var orgs = Access.All(p => p.DataState < DataStateType.Delete);
            var opAccess = new OrganizePositionDataAccess();
            var posts = opAccess.All(p => p.DataState < DataStateType.Delete);
            CreateOrgPosTree(0, proxy, root, orgs, posts);
        }

        /// <summary>
        ///     设置子级以构成树
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="proxy"></param>
        /// <param name="par"></param>
        /// <param name="orgs"></param>
        /// <param name="posts"></param>
        internal static void CreateOrgPosTree(int pid, RedisProxy proxy, EasyUiTreeNode par, List<OrganizationData> orgs, List<OrganizePositionData> posts)
        {
            par.IsFolder = true;
            par.Children.AddRange(posts.Where(p => p.OrganizationId == pid).Select(CreateNode));
            var childs = orgs.Where(p => p.ParentId == pid).ToArray();
            if (childs.Length != 0)
            {
                foreach (var data in childs.OrderBy(p => p.Type))
                {
                    var node = new EasyUiTreeNode
                    {
                        Icon = Icon(data.Type),
                        IsOpen = true,
                        IsFolder = true,
                        Attributes = "org",
                        Title = data.TreeName,
                        Text = data.ShortName,
                        Tag = data.Id.ToString()
                    };
                    par.Children.Add(node);
                    CreateOrgPosTree(data.Id, proxy, node, orgs, posts);
                }
            }
            if (!par.HaseChildren)
            {
                par.IsFolder = false;
                par.IsOpen = true;
            }
            proxy.Set("ui:org:PostTree:" + pid, par);
        }

        #endregion

        #region 生成组织结构树

        /// <summary>
        ///     生成完整的组织结构树(UI相关）
        /// </summary>
        /// <returns></returns>
        private void CreateFullOrgTree(RedisProxy proxy)
        {
            using (SystemContextScope.CreateScope())
            {
                var root = CreateRootNode();
                var lists = Access.All(p => p.DataState < DataStateType.Delete);
                CreateOrgTree(root, lists, proxy);
            }
        }

        /// <summary>
        ///     设置子级以构成树
        /// </summary>
        /// <param name="par"></param>
        /// <param name="lists"></param>
        /// <param name="proxy"></param>
        private void CreateOrgTree(EasyUiTreeNode par, List<OrganizationData> lists, RedisProxy proxy)
        {
            var childs = lists.Where(p => p.ParentId == par.ID).ToArray();
            if (childs.Length != 0)
            {
                par.IsFolder = true;
                foreach (var child in childs)
                {
                    var node = CreateNode(child);
                    par.Children.Add(node);
                    CreateOrgTree(node, lists, proxy);
                }
            }
            proxy.Set("ui:org:OrgTree:" + par.ID, par);
        }

        #endregion

        #region 生成区域树

        /// <summary>
        ///     生成完整的组织结构树(UI相关）
        /// </summary>
        /// <returns></returns>
        public void CreateAreaTree(RedisProxy proxy)
        {
            var root = CreateRootNode();
            var lists = Access.All(p => p.DataState < DataStateType.Delete);
            CreateAreaTree(root, lists, proxy);
        }

        /// <summary>
        ///     设置子级以构成树
        /// </summary>
        /// <param name="par"></param>
        /// <param name="lists"></param>
        /// <param name="proxy"></param>
        private void CreateAreaTree(EasyUiTreeNode par, List<OrganizationData> lists, RedisProxy proxy)
        {
            var childs = lists.Where(p => p.ParentId == par.ID).ToArray();
            if (childs.Length != 0)
            {
                par.IsFolder = true;
                foreach (var child in childs)
                {
                    var node = CreateNode(child);
                    node.Attributes = "area";
                    CreateAreaTree(node, lists, proxy);
                    par.Children.Add(node);
                }
            }
            proxy.Set("ui:org:areaTree:" + par.ID, par);
        }

        #endregion

        #endregion

        #region 载入完整的组织结构树(自定义回调）
        /*
        /// <summary>
        ///     载入完整的组织结构树(自定义回调）
        /// </summary>
        /// <returns></returns>
        public EasyUiTreeNode LoadTreeForCustom(int oid, string name, Func<OrganizationData, EasyUiTreeNode, bool> func)
        {
            var key = "tr:org:" + oid;
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                var root = new EasyUiTreeNode
                {
                    ID = 0,
                    Icon = "icon-global",
                    IsOpen = true,
                    Attributes = "root",
                    Text = name,
                    Children = new List<EasyUiTreeNode>()
                };
                LoadTreeForCustomInner(root, func);
                proxy.Set(key, root);
                return root;
            }
        }

        /// <summary>
        ///     载入完整的组织结构树
        /// </summary>
        /// <returns></returns>
        private void LoadTreeForCustomInner(EasyUiTreeNode root, Func<OrganizationData, EasyUiTreeNode, bool> func)
        {
            var lists = Access.All(p => p.DataState < DataStateType.Delete);
            LoadTreeForCustomInner(root, lists, func);
        }

        /// <summary>
        ///     设置子级以构成树
        /// </summary>
        /// <param name="par"></param>
        /// <param name="lists"></param>
        /// <param name="func"></param>
        private int LoadTreeForCustomInner(EasyUiTreeNode par, List<OrganizationData> lists,
            Func<OrganizationData, EasyUiTreeNode, bool> func)
        {
            var childs = lists.Where(p => p.ParentId == par.ID).ToArray();
            if (childs.Length == 0)
                return 0;
            foreach (var child in childs)
            {
                var node = new EasyUiTreeNode
                {
                    ID = child.Id,
                    Icon = child.Type == OrganizationType.Organization ? "icon-com" : "icon-bm",
                    IsOpen = true,
                    Text = child.FullName,
                    Children = new List<EasyUiTreeNode>()
                };
                if (func != null && !func(child, node))
                    continue;
                if (LoadTreeForCustomInner(node, lists, func) > 0)
                    par.Children.Add(node);
            }
            return childs.Length;
        }
        */
        #endregion


        #region 两个组织是否存在隶属关系

        /// <summary>
        ///     两个组织是否存在隶属关系
        /// </summary>
        /// <param name="parOid">上级组织</param>
        /// <param name="chdOid">下级组织</param>
        /// <returns>是否存在隶属关系</returns>
        public static bool IsSubjection(int parOid, int chdOid)
        {
            if (parOid == 0)
                return true;
            if (parOid == chdOid)
                return true;
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return proxy.Client.SIsMember("org:sub:" + parOid, chdOid.ToByte()) == 1;
            }
        }

        #endregion
    }
}