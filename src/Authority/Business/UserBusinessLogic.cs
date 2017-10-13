using System;
using System.Configuration;
using Agebull.Common.DataModel;
using Agebull.Common.DataModel.Redis;
using Agebull.Common.Logging;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.DataModel.MySql;
using Agebull.SystemAuthority.Organizations.DataAccess;

namespace Agebull.SystemAuthority.Organizations.BusinessLogic
{
    /// <summary>
    /// 系统用户
    /// </summary>
    public sealed partial class UserBusinessLogic : BusinessLogicByStateData<UserData, UserDataAccess>
    {
        #region 修改密码

        /// <summary>
        ///     修改密码
        /// </summary>
        public static bool OnModifyPwd(int uid, string oldPwd, string newPwd)
        {
            if (uid != BusinessContext.Current.LoginUserId || String.IsNullOrEmpty(oldPwd) || String.IsNullOrEmpty(newPwd))
                return false;
            var access = new UserDataAccess();
            var user = access.LoadByPrimaryKey(uid);
            if (user == null || access.SetValue(p => p.PassWord, newPwd, p => p.PassWord == oldPwd && p.Id == uid) <= 0)
            {
                return false;
            }
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                proxy.Set(BuildDataKey(user.UserName, "pwd"), newPwd);
            }

            return true;
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool ResetPassword(int[] ids)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                foreach (var id in ids)
                {
                    Access.SetValue(p => p.PassWord, default_password, id);

                    var name = Access.LoadValue(p => p.UserName, id);
                    proxy.Set(BuildDataKey(name, "pwd"), default_password);
                }
            }
            return true;
        }
        #endregion

        #region 缓存同步

        public UserBusinessLogic()
        {
            unityStateChanged = true;
        }
        /// <summary>
        ///     状态改变后的统一处理(unityStateChanged不设置为true时不会产生作用--基于性能的考虑)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override void DoStateChanged(UserData data)
        {
            Cache(data);
            base.DoStateChanged(data);
        }

        /// <summary>
        /// 缓存
        /// </summary>
        public static void Cache()
        {
            var orb = new UserBusinessLogic();
            orb.DoCache();
        }

        /// <summary>
        /// 缓存
        /// </summary>
        private void DoCache()
        {
            using (SystemContextScope.CreateScope())
            {
                using (MySqlDataBaseScope.CreateScope(Access.DataBase))
                {
                    var pAccess = new PositionPersonnelDataAccess();
                    foreach (var pos in pAccess.All())
                    {
                        SyncUser(pos);
                    }
                    var users = Access.All();
                    using (var proxy = new RedisProxy(RedisProxy.DbSystem))
                    {
                        foreach (var user in users)
                        {
                            Cache(proxy, user);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 设置Cache
        /// </summary>
        /// <param name="user"></param>
        private static void Cache(UserData user)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                Cache(proxy, user);
            }
        }


        /// <summary>
        /// 取用户的名字
        /// </summary>
        /// <param name="id"></param>
        public static string GetName(int id)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return proxy.Get(BuildDataKey("name", id))?.Trim('"');
            }
        }

        #endregion

        #region 同步用户权限

        private static readonly string default_password = ConfigurationManager.AppSettings["default_pwd"];

        internal readonly PositionPersonnelDataAccess _posAccess = new PositionPersonnelDataAccess();

        /// <summary>
        /// 同步用户权限
        /// </summary>
        internal void SyncUser(PositionPersonnelData personnel)
        {
            if (personnel == null)
                return;
            _posAccess.DataBase = Access.DataBase;

            if (personnel.AuditState != AuditStateType.Pass || personnel.DepartmentId <= 0)
            {
                DiscardUser(personnel);
                return;
            }
            if (personnel.UserId <= 0)
            {
                InsertUser(personnel);
                return;
            }

            var user = Access.First(personnel.UserId);
            if (user == null)
            {
                InsertUser(personnel);
                return;
            }
            EnableUser(user, personnel);
        }

        private void EnableUser(UserData user, PositionPersonnelData personnel)
        {
            user.UserName = personnel.Mobile;
            user.RealName = personnel.Personnel;
            if (personnel.RoleId > 0)
                user.RoleId = personnel.RoleId;
            else if (user.RoleId > 0)
                _posAccess.SetValue(p => p.RoleId, user.RoleId, personnel.PersonnelId);
            user.AuditState = AuditStateType.Pass;
            user.DataState = DataStateType.Enable;
            user.Memo = personnel.Department;
            Access.Update(user);
            LogRecorder.RecordLoginLog("用户{0}-{1}({2})已重新启用", personnel.Personnel, user.UserName, user.Id);
            CacheUser(personnel, user);

        }

        private void InsertUser(PositionPersonnelData personnel)
        {
            UserData user;
            Access.Insert(user = new UserData
            {
                Id = personnel.PersonnelId,
                UserName = personnel.Mobile,
                RealName = personnel.Personnel,
                RoleId = personnel.RoleId,
                PassWord = default_password,
                AuditState = AuditStateType.Pass,
                DataState = DataStateType.Enable,
                Memo = personnel.Department
            });
            var pAccess = new PersonnelDataAccess();
            pAccess.SetValue(p => p.UserId, user.Id, personnel.PersonnelId);
            LogRecorder.RecordLoginLog("用户{0}-{1}({2})已加入系统", user.UserName, personnel.Personnel, user.Id);
            CacheUser(personnel, user);
        }


        private void DiscardUser(PositionPersonnelData personnel)
        {
            if (personnel.UserId <= 0)
                return;
            var user = Details(personnel.UserId);
            if (user == null)
                return;
            user.DataState = DataStateType.Discard;
            user.AuditState = AuditStateType.None;
            Access.Update(user);
            LogRecorder.RecordLoginLog(personnel.DepartmentId == 0
                    ? "用户{0}-{1}因为没有分配职位而被系统废弃"
                    : "用户{0}-{1}职位分配数据未审核通过被系统废弃"
                , personnel.Personnel
                , personnel.UserId);
            CacheUser(personnel, user);
        }

        #region 缓存

        /// <summary>
        /// 缓存数据
        /// </summary>
        public static PositionPersonnelData GetPersonnel(int uid)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return proxy.GetEntity<PositionPersonnelData>(uid);
            }
        }


        private static void CacheUser(PositionPersonnelData personnel, UserData user)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                Cache(proxy, user);
                proxy.SetValue(BuildDataKey("depid", personnel.UserId), personnel.DepartmentId);
                proxy.SetEntity(personnel);
            }
        }

        /// <summary>
        /// 设置Cache
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="user"></param>
        static void Cache(RedisProxy proxy, UserData user)
        {
            if (user == null)
                return;
            proxy.SetEntity(user);
            var key = BuildDataKey("name", user.Id);
            proxy.Set(key, user.RealName);

            if (user.DataState >= DataStateType.Discard)
            {
                proxy.RemoveCache<UserData>(user.Id);
                proxy.RemoveKey(BuildDataKey(user.UserName, "id"));
                proxy.RemoveKey(BuildDataKey(user.UserName, "pwd"));
                return;
            }
            if (user.DataState == DataStateType.Enable)
            {
                proxy.Set(BuildDataKey(user.UserName, "pwd"), user.PassWord);
                proxy.SetValue(BuildDataKey(user.UserName, "id"), user.Id);
            }
            else
            {
                proxy.RemoveKey(BuildDataKey(user.UserName, "id"));
                proxy.RemoveKey(BuildDataKey(user.UserName, "pwd"));
            }
        }
        /// <summary>
        /// 生成用户相关的数据键
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string BuildDataKey(params object[] args)
        {
            return DataKeyBuilder.ToKey("user", args);
        }

        #endregion
        #endregion

    }
}
