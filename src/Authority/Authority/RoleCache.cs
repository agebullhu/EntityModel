using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Agebull.Common.DataModel.Redis;
using Agebull.Common.Logging;
using Agebull.ProjectDeveloper.WebDomain.Models;
using Agebull.SystemAuthority.Organizations.DataAccess;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.SystemModel;
using Gboxt.Common.SystemModel.DataAccess;
using NServiceKit.Text;

namespace Agebull.SystemAuthority.Organizations.BusinessLogic
{
    public class RoleCache
    {
        #region 实例基础


        private readonly PageItemDataAccess _piAccess = new PageItemDataAccess();
        private readonly RolePowerDataAccess _rpAccess = new RolePowerDataAccess();
        private readonly List<PageItemData> _pages;
        private List<RolePowerData> _allPowers { get; set; }
        private readonly Dictionary<int, List<PageItemData>> _actions;

        public RoleCache()
        {
            var _items = _piAccess.All();
            _pages = _items.Where(p => p.ItemType <= PageItemType.Page && !p.IsHide).ToList();

            _actions = new Dictionary<int, List<PageItemData>>();
            foreach (var page in _items.Where(p => p.ItemType >= PageItemType.Button).GroupBy(p => p.ParentId))
                _actions.Add(page.Key, page.ToList());
        }

        private static EasyUiTreeNode CreatePageNode(PageItemData page)
        {
            var node = new EasyUiTreeNode
            {
                ID = page.Id,
                IsOpen = page.ItemType <= PageItemType.Folder,
                Tag = page.Url == "Folder" ? "folder" : "page",
                Text = page.Caption,
                Title = page.Name,
                IsFolder = page.ItemType <= PageItemType.Folder
            };
            if (!String.IsNullOrWhiteSpace(page.Icon))
                node.Icon = page.Icon;
            else
                switch (page.ItemType)
                {
                    case PageItemType.Folder:
                        node.Icon = "icon-tree-folder";
                        break;
                    case PageItemType.Page:
                        node.Icon = "icon-tree-page";
                        break;
                }
            return node;
        }


        /// <summary>
        ///     生成角色对应的页面按钮信息的键
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="pageId">页面标识</param>
        /// <param name="types">类型</param>
        /// <returns></returns>
        public static string ToRolePageKey(int roleId, int pageId = -1, params string[] types)
        {
            if (roleId == 1)
                roleId = 0;
            return types.Length == 0
                ? $"role:{roleId}:{(pageId < 0 ? "*" : pageId.ToString())}"
                : $"role:{roleId}:{(pageId < 0 ? "*" : pageId.ToString())}:{String.Join(":", types)}";
        }

        #endregion

        #region 缓存操作

        /// <summary>
        /// 缓存
        /// </summary>
        public static RoleData GetRole(int id)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return proxy.GetEntity<RoleData>(id) ?? new RoleData { Role = "未法用户" };
            }
        }


        /// <summary>
        ///     缓存所有角色的页面权限数据
        /// </summary>
        public static void Cache()
        {
            using (SystemContextScope.CreateScope())
            {
                new RoleCache().DoCache();
            }
        }

        /// <summary>
        ///     缓存所有角色的页面权限数据
        /// </summary>
        public static void Cache(int id)
        {
            using (SystemContextScope.CreateScope())
            {
                new RoleCache().CacheRolePower(id);
            }
        }

        /// <summary>
        ///     缓存所有角色的页面权限数据
        /// </summary>
        public void DoCache()
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                proxy.FindAndRemoveKey("role:*");
            }
            using (SystemContextScope.CreateScope())
            {
                var access = new RoleDataAccess();
                var roles = access.All();
                using (var proxy = new RedisProxy(RedisProxy.DbSystem))
                {
                    proxy.CacheData(roles);
                }
                using (var proxy = new RedisProxy(RedisProxy.DbSystem))
                {
                    CreatePowerTree(proxy);
                }
                using (var proxy = new RedisProxy(RedisProxy.DbSystem))
                {
                    LoadAllPowers(0, proxy);
                    foreach (var items in roles)
                        LoadAllPowers(items.Id, proxy);
                }
            }
            CachePageAuditUser();
            CacheTypeUser();
        }
        /// <summary>
        ///     缓存角色的页面权限数据
        /// </summary>
        /// <param name="roleId">角色ID</param>
        public void CacheRolePower(int roleId)
        {
            using (SystemContextScope.CreateScope())
            {
                using (var proxy = new RedisProxy(RedisProxy.DbSystem))
                {
                    LoadAllPowers(roleId, proxy);
                }
            }
        }
        /// <summary>
        ///     缓存角色的页面权限数据
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="proxy"></param>
        public void LoadAllPowers(int roleId, RedisProxy proxy)
        {
            _allPowers = roleId <= 1 ? null : _rpAccess.All(p => p.RoleId == roleId);

            CacheRolePower(proxy, roleId);
            CreateMenu(roleId, proxy);
        }
        
        #endregion

        #region 角色菜单

        /// <summary>
        ///     载入角色菜单
        /// </summary>
        public static List<EasyUiTreeNode> LoadRoleMenu(int roleId)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return proxy.Get<List<EasyUiTreeNode>>(ToRolePageKey(roleId, 0, "menu"));
            }
        }

        /// <summary>
        ///     生成角色菜单
        /// </summary>
        private void CreateMenu(int roleId, RedisProxy proxy)
        {
            var root = new EasyUiTreeNode
            {
                ID = 0,
                IsOpen = true,
                Text = ConfigurationManager.AppSettings["ProjectName"],
                IsFolder = true
            };
            foreach (var folder in _pages.Where(p => p.ItemType == PageItemType.Root).OrderBy(p => p.Index))
            {
                var node = new EasyUiTreeNode
                {
                    IsOpen = true,
                    Text = folder.Caption,
                    Icon = "icon-item",
                    Attributes = "home.aspx",
                    Tag = folder.ExtendValue,
                    IsFolder = true
                };
                foreach (var page in _pages.Where(p => p.ParentId == folder.Id && p.ItemType <= PageItemType.Page).OrderBy(p => p.Index))
                {
                    CreateRoleMenu(node, roleId, page);
                }
                if (node.HaseChildren)
                {
                    root.Children.Add(node);
                }
            }
            proxy.Set(ToRolePageKey(roleId, 0, "menu"), root.Children.OrderByDescending(p => p.ID));
        }

        /// <summary>
        ///     生成角色菜单
        /// </summary>
        private void CreateRoleMenu(EasyUiTreeNode parentNode, int roleId, PageItemData page)
        {
            if (_allPowers != null && !_allPowers.Any(p => p.RoleId == roleId && p.PageItemId == page.Id))
                return;
            var node = CreatePageNode(page);
            node.Attributes = page.Url;
            node.Tag = page.Url == "Folder" ? "folder" : "page";

            var array = _pages.Where(p => p.ParentId == page.Id && !p.Config.Hide).OrderBy(p => p.Index).ToArray();
            if (array.Length > 0)
                node.IsFolder = true;
            foreach (var ch in array)
            {
                CreateRoleMenu(node, roleId, ch);
            }
            if (page.ItemType == PageItemType.Page || node.HaseChildren)
                parentNode.Children.Add(node);
        }

        #endregion

        #region 页面权限树形基础数据

        /// <summary>
        ///     载入页面权限树形基础数据
        /// </summary>
        public static EasyUiTreeNode LoadPowerTree()
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return proxy.Get<EasyUiTreeNode>("role:power:tree");
            }
        }

        /// <summary>
        ///     载入页面权限树形基础数据
        /// </summary>
        public static void CreatePowerTree()
        {
            var cache = new RoleCache();
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                cache.CreatePowerTree(proxy);
            }
        }

        /// <summary>
        ///     生成页面权限树形基础数据
        /// </summary>
        public void CreatePowerTree(RedisProxy proxy)
        {
            var root = new EasyUiTreeNode
            {
                ID = 0,
                IsOpen = true,
                Text = ConfigurationManager.AppSettings["ProjectName"],
                Title = ConfigurationManager.AppSettings["ProjectName"],
                IsFolder = true
            };
            foreach (var filter in _pages.Where(p => p.ItemType == PageItemType.Root ))
            {
                var node = new EasyUiTreeNode
                {
                    IsOpen = true,
                    Icon = "icon-item",
                    Text = filter.Caption,
                    Title = filter.Caption ,
                    Tag = filter.ExtendValue,
                    IsFolder = true
                };
                CreatePowerTree(node, filter);
                root.Children.Add(node);
            }
            proxy.Set("role:power:tree", root);
        }

        /// <summary>
        ///     生成页面权限树形基础数据
        /// </summary>
        private void CreatePowerTree(EasyUiTreeNode parentNode, PageItemData page)
        {
            parentNode.IsFolder = true;
            var node = CreatePageNode(page);
            foreach (var ch in _pages.Where(p => p.ParentId == page.Id).OrderBy(p => p.Index))
            {
                CreatePowerTree(node, ch);
            }
            parentNode.Attributes = null;
            parentNode.Children.Add(node);
            if (page.ItemType != PageItemType.Page)
            {
                parentNode.IsOpen = true;
                return;
            }
            node.IsFolder = true;
            var items = _piAccess.All(p => p.ParentId == page.Id && p.ItemType >= PageItemType.Button);
            foreach (var item in items)
            {
                node.Children.Add(new EasyUiTreeNode
                {
                    ID = item.ID,
                    Text = item.Caption,
                    Title = item.Name,
                    Tag = item.ExtendValue,
                    Memo = item.Memo,
                    IsOpen = true,
                    Icon = item.ItemType == PageItemType.Action ? "icon-cmd" : "icon-cus",
                });
            }
        }

        #endregion

        #region 缓存页面的审批用户


        /// <summary>
        ///     缓存页面的审批用户
        /// </summary>
        public void CachePageAuditUser()
        {
            //LogRecorder.BeginStepMonitor("CachePageAuditUser");
            using (SystemContextScope.CreateScope())
            {
                _allPowers = _rpAccess.All();
                using (var proxy = new RedisProxy(RedisProxy.DbSystem))
                {
                    CachePageAuditUser(proxy);
                }
            }
            //LogRecorder.EndStepMonitor();
        }

        readonly List<string> audits = new List<string>
        {
            "submit","pullback","deny","pass","reaudit","back"
        };

        /// <summary>
        ///     缓存页面的审批用户
        /// </summary>
        /// <param name="proxy"></param>
        private void CachePageAuditUser(RedisProxy proxy)
        {
            foreach (var page in _pages.Where(p => p.ItemType == PageItemType.Page))
            {
                if (!_actions.ContainsKey(page.Id))
                {
                    //Trace.WriteLine("错误页面", page.Caption + page.Id);
                    continue;
                }

                if (page.Config.AuditPage > 0)
                {
                    var friend = _pages.FirstOrDefault(p => p.Id == page.Config.AuditPage);
                    if (friend == null)
                    {
                        //Trace.WriteLine($"错误连接({page.Config.AuditPage})", page.Caption + page.Id);
                    }
                    else
                    {
                        proxy.SetValue($"audit:page:link:{page.Id}", page.Config.AuditPage);
                        //Trace.WriteLine($"连接到=>{friend.Caption}", page.Caption + page.Id);
                    }
                    continue;
                }

                var items = _actions[page.Id].Where(p => audits.Contains(p.ExtendValue)).ToArray();
                if (items.Length == 0)
                {
                    //Trace.WriteLine("没有审核操作", page.Caption + page.Id);
                    continue;
                }
                var item_ids = items.Select(p => p.Id).ToList();
                var pwoers = _allPowers.Where(p => item_ids.Contains(p.PageItemId)).ToArray();
                if (pwoers.Length == 0)
                {
                    //Trace.WriteLine("没有审核用户", page.Caption + page.Id);
                    continue;
                }
                var roles = pwoers.Select(p => p.RoleId).Distinct().ToArray();
                var posAccess = new OrganizePositionDataAccess();
                var ponAccess = new PositionPersonnelDataAccess();

                Dictionary<int, PositionPersonnelData> personnels = new Dictionary<int, PositionPersonnelData>();
                foreach (var role in roles)
                {
                    var pons = ponAccess.All(p => p.RoleId == role && p.AuditState == AuditStateType.Pass);
                    foreach (var p in pons)
                    {
                        if (personnels.ContainsKey(p.Id))
                            continue;
                        personnels.Add(p.Id, p);
                    }
                    var pos = posAccess.LoadValues(p => p.Id, p => p.RoleId == role);
                    foreach (var pid in pos)
                    {
                        pons = ponAccess.All(p => p.OrganizePositionId == pid && p.AuditState == AuditStateType.Pass);
                        foreach (var p in pons)
                        {
                            if (personnels.ContainsKey(p.Id))
                                continue;
                            personnels.Add(p.Id, p);
                        }
                    }
                }
                AuditNode(proxy, page.Id, personnels.Values.ToList());
            }
        }

        void AuditNode(RedisProxy proxy, int pageId, List<PositionPersonnelData> users)
        {
            var page = PageItemLogical.GetPageItem(pageId);
            if (!page.LevelAudit)
            {
                var orgs = OrganizationBusinessLogic.LoadAreaTreeForUi(0);
                AuditNode(orgs[0], users);
                proxy.Set($"audit:page:users:nodes:{pageId}:0", orgs);
                proxy.Set($"audit:page:users:ids:{pageId}:0", users.Select(p => p.UserId).LinkToString(","));
                return;
            }
            foreach (var levels in users.GroupBy(p => p.OrgLevel))
            {
                var orgs = OrganizationBusinessLogic.LoadAreaTreeForUi(0);
                AuditNode(orgs[0], levels.ToList());
                if (orgs[0].HaseChildren)
                {
                    foreach (var ch in orgs[0].Children.Where(p => p.Tag != "personnel").ToArray())
                    {
                        if (!ch.HaseChildren)
                            orgs[0].Children.Remove(ch);
                    }
                }
                proxy.Set($"audit:page:users:nodes:{pageId}:{levels.Key}", orgs);
                proxy.Set($"audit:page:users:ids:{pageId}:{levels.Key}", levels.Select(p => p.UserId).LinkToString(","));
            }
        }

        void AuditNode(EasyUiTreeNode orgNode, List<PositionPersonnelData> users)
        {
            if (orgNode.HaseChildren)
            {
                foreach (var ch in orgNode.Children)
                {
                    AuditNode(ch, users);
                }
            }
            var array = users.Where(p => p.DepartmentId == orgNode.ID).ToArray();
            if (array.Length == 0)
            {
                orgNode.IsFolder = false;
                return;
            }
            orgNode.IsFolder = true;
            foreach (var personnel in array)
            {
                orgNode.Children.Add(new EasyUiTreeNode
                {
                    ID = personnel.UserId,
                    IsOpen = true,
                    Text = $"{personnel.Personnel}({personnel.Position})",
                    Tag = "personnel"
                });
            }
            orgNode.ID = 0 - orgNode.ID;
        }
        /// <summary>
        /// 页面对应的审核用户树
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static List<EasyUiTreeNode> GetPageAuditUsersTree(int pid)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                int friend = proxy.GetValue<int>($"audit:page:link:{pid}");
                if (friend > 0)
                    pid = friend;

                var page = PageItemLogical.GetPageItem(pid);
                if (page == null)
                    return new List<EasyUiTreeNode>();
                int level;
                if (!page.LevelAudit)
                    level = 0;//BusinessContext.Current.LoginUser.DepartmentId;
                else
                {
                    level = BusinessContext.Current.LoginUser.DepartmentLevel - 1;
                    if (level < 1)
                        level = 1;
                    else if (level > 2)
                        level = 2;
                }
                return proxy.Get<List<EasyUiTreeNode>>($"audit:page:users:nodes:{pid}:{level}");
            }
        }

        #endregion

        #region 缓存页面的编辑用户


        /// <summary>
        ///     缓存类型对应的权限用户
        /// </summary>
        public void CacheTypeUser()
        {
            LogRecorder.BeginStepMonitor("CacheEditUser");
            using (SystemContextScope.CreateScope())
            {
                _allPowers = _rpAccess.All();
                using (var proxy = new RedisProxy(RedisProxy.DbAuthority))
                {
                    CacheUser(proxy, "edit", edits);
                    CacheUser(proxy, "audit", audits);
                }
            }
            LogRecorder.EndStepMonitor();
        }

        readonly List<string> edits = new List<string>
        {
            "submit","pullback","deny","pass","reaudit","back"
        };

        /// <summary>
        ///     缓存页面的审批用户
        /// </summary>
        private void CacheUser(RedisProxy proxy, string name, List<string> actions)
        {
            var posAccess = new OrganizePositionDataAccess();
            var ponAccess = new PositionPersonnelDataAccess();
            Dictionary<string, List<int>> types = new Dictionary<string, List<int>>();
            foreach (var page in _pages.Where(p => p.ItemType == PageItemType.Page))
            {
                if (!_actions.ContainsKey(page.Id))
                    continue;
                var items = _actions[page.Id].Where(p => actions.Contains(p.ExtendValue)).ToArray();
                if (items.Length == 0)
                    continue;
                var item_ids = items.Select(p => p.Id).ToList();
                var pwoers = _allPowers.Where(p => item_ids.Contains(p.PageItemId)).ToArray();
                if (pwoers.Length == 0)
                    continue;
                var roles = pwoers.Select(p => p.RoleId).Distinct().ToArray();

                List<int> personnels;
                if (types.ContainsKey(page.Config.SystemType))
                    personnels = types[page.Config.SystemType];
                else
                    types.Add(page.Config.SystemType, personnels = new List<int>());
                foreach (var role in roles)
                {
                    var pons = ponAccess.LoadValues(p => p.UserId, p => p.RoleId == role && p.AuditState == AuditStateType.Pass);
                    personnels.AddRange(pons);
                    var pos = posAccess.LoadValues(p => p.Id, p => p.RoleId == role);
                    foreach (var pid in pos)
                    {
                        pons = ponAccess.LoadValues(p => p.UserId, p => p.OrganizePositionId == pid && p.AuditState == AuditStateType.Pass);
                        personnels.AddRange(pons);
                    }
                }
            }
            foreach (var type in types)
            {
                var users = type.Value.Distinct().ToArray();
                var keys = $"users:{name}:{type.Key}";
                proxy.Set(keys, users);
                //System.Diagnostics.Trace.WriteLine(users.LinkToString(","), keys);
            }
        }

        /// <summary>
        ///     得到缓存页面的编辑用户
        /// </summary>
        public static List<int> GetEditUsers(Type type)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbAuthority))
            {
                return proxy.Get<List<int>>($"users:edit:{type.FullName}");
            }
        }

        /// <summary>
        ///     得到缓存页面的审批用户
        /// </summary>
        public static List<int> GetAuditUsers(Type type)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbAuthority))
            {
                return proxy.Get<List<int>>($"users:audit:{type.FullName}");
            }
        }
        #endregion

        #region 权限缓存

        /// <summary>
        ///     缓存角色的页面权限数据
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="roleId"></param>
        private void CacheRolePower(RedisProxy proxy, int roleId)
        {
            foreach (var page in _pages)
            {
                if (_allPowers == null || roleId <= 1)
                {
                    proxy.Set(ToRolePageKey(roleId, page.ID, "page"), new RolePowerData
                    {
                        RoleId = roleId,
                        PageItemId = page.Id,
                        Power = 1
                    });
                }
                else
                {
                    var power = _allPowers.FirstOrDefault(p => p.RoleId == roleId && p.PageItemId == page.Id);
                    if (power == null)
                        continue;
                    proxy.Set(ToRolePageKey(roleId, page.ID, "page"), power);
                }
                CacheRoleAction(roleId, page, proxy);
            }
        }
        /// <summary>
        /// 缓存角色的页面权限数据
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="page"></param>
        /// <param name="proxy"></param>
        private void CacheRoleAction(int roleId, PageItemData page, RedisProxy proxy)
        {
            if (!_actions.ContainsKey(page.Id))
                return;
            var dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            var actions = roleId <= 1
                ? _actions[page.Id].ToArray()
                : _actions[page.Id].Where(p => _allPowers.Any(a => a.PageItemId == p.Id)).ToArray();
            foreach (var bp in actions)
            {
                if (String.IsNullOrEmpty(bp.ExtendValue) || dictionary.ContainsKey(bp.ExtendValue))
                    continue;
                dictionary.Add(bp.ExtendValue, bp.ID);
            }

            if (page.AuditPage > 0 && _actions.ContainsKey(page.AuditPage))
            {
                var friendsItemDatas = roleId <= 1
                    ? _actions[page.AuditPage].ToArray()
                    : _actions[page.AuditPage].Where(p => _allPowers.Any(a => a.PageItemId == p.Id)).ToArray();
                foreach (var bp in friendsItemDatas)
                {
                    if (String.IsNullOrEmpty(bp.ExtendValue) || dictionary.ContainsKey(bp.ExtendValue))
                        continue;
                    dictionary.Add(bp.ExtendValue, bp.ID);
                }
            }
            AddActionSynonym(dictionary, "list","details");
            AddActionSynonym(dictionary, "update","details");
            foreach (var action in dictionary.Keys)
                proxy.SetValue(ToRolePageKey(roleId, page.ID, "action", action), 1);
            proxy.Set(ToRolePageKey(roleId, page.ID, "btns"), actions.Select(p => p.Name));
        }


        /// <summary>
        /// 同义词加入
        /// </summary>
        /// <param name="actions"></param>
        /// <param name="action"></param>
        /// <param name="synonyms"></param>
        private void AddActionSynonym(Dictionary<string, int> actions, string action, params string[] synonyms)
        {
            int id;
            if (!actions.TryGetValue(action, out id))
                return;
            foreach (var synonym in synonyms)
                if (!actions.ContainsKey(synonym))
                    actions.Add(synonym, id);
        }

        #endregion

        #region 权限校验

        /// <summary>
        ///     载入用户的单页角色权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="pageId">页面ID</param>
        /// <returns></returns>
        public static IRolePower LoadPagePower(int roleId, int pageId)
        {
            if (roleId == 1)
                roleId = 0;
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return proxy.Get<RolePowerData>(ToRolePageKey(roleId, pageId, "page"));
            }
        }

        /// <summary>
        ///     载入用户的角色权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public static List<IRolePower> LoadUserPowers(int roleId)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                var key = roleId == 1 || BusinessContext.Current.IsSystemMode
                    ? ToRolePageKey(roleId, 0, "page")
                    : ToRolePageKey(roleId, -1, "page");
                var vl = proxy.GetAll<RolePowerData>(key);
                return vl == null || vl.Count == 0 ? new List<IRolePower>() : vl.ToList(p => (IRolePower)p);
            }
        }

        /// <summary>
        ///     载入页面关联的按钮配置
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="pageId">页面ID</param>
        /// <param name="action">动作</param>
        /// <returns>是否可执行页面动作</returns>
        public static bool CanDoAction(int roleId, int pageId, string action)
        {
            if (BusinessContext.Current.IsSystemMode || roleId == 1 || roleId == Int32.MaxValue)
                return true;
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return proxy.GetValue<int>(ToRolePageKey(roleId, pageId, "action", action)) == 1;
            }
        }

        /// <summary>
        ///     载入页面关联的按钮配置
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="pageId">页面ID</param>
        /// <returns>按钮配置集合</returns>
        public static List<string> LoadButtons(int roleId, int pageId)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                if (BusinessContext.Current.IsSystemMode || roleId == 1 || roleId == Int32.MaxValue)
                    roleId = 0;
                return proxy.Get<List<string>>(ToRolePageKey(roleId, pageId, "btns"));
            }
        }

        #endregion
    }
}