// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace Gboxt.Common.DataModel.BusinessLogic
{
    /// <summary>
    /// 业务逻辑对象基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TAccess">数据访问对象</typeparam>
    public class BusinessLogicBase<TData, TAccess>
    where TData : EditDataObject, IIdentityData, new()
    where TAccess : class, IDataTable<TData>, new()
    {
        #region 基础支持对象

        /// <summary>
        ///     实体类型
        /// </summary>
        public virtual int EntityType => 0;

        private TAccess _access;

        /// <summary>
        ///     数据访问对象
        /// </summary>
        public TAccess Access => _access ?? (_access = CreateAccess());

        /// <summary>
        ///     数据访问对象
        /// </summary>
        protected virtual TAccess CreateAccess()
        {
            var access = new TAccess();
            return access;
        }
        /// <summary>
        /// 构造
        /// </summary>
        protected BusinessLogicBase()
        {
        }
        #endregion

        #region 便利操作

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        public bool DoByIds(string ids, Func<int, bool> func, Action onEnd = null)
        {
            return LoopIds(ids, func, onEnd);
        }

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        public bool LoopIds(string ids, Func<int, bool> func, Action onEnd = null)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return false;
            }
            using (Access.DataBase.CreateDataBaseScope())
            {
                using (var scope = Access.DataBase.CreateTransactionScope())
                {
                    var sids = ids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (sids.Length == 0)
                    {
                        return false;
                    }
                    foreach (var sid in sids)
                    {
                        int id;
                        if (!int.TryParse(sid, out id))
                        {
                            return false;
                        }
                        if (!func(id))
                        {
                            return false;
                        }
                    }
                    onEnd?.Invoke();
                    scope.SetState(true);
                }
            }
            return true;
        }

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        public bool DoByIds(string ids, Func<TData, bool> func, Action onEnd = null)
        {
            return LoopIdsToData(ids, func, onEnd);
        }

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        public bool LoopIdsToData(string ids, Func<TData, bool> func, Action onEnd = null)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return false;
            }
            using (Access.DataBase.CreateDataBaseScope())
            {
                using (var scope = Access.DataBase.CreateTransactionScope())
                {
                    var sids = ids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (sids.Length == 0)
                    {
                        return false;
                    }
                    foreach (var sid in sids)
                    {
                        int id;
                        if (!int.TryParse(sid, out id))
                        {
                            return false;
                        }
                        var data = Access.LoadByPrimaryKey(id);
                        if (data == null || !func(data))
                        {
                            return false;
                        }
                    }
                    onEnd?.Invoke();
                    scope.SetState(true);
                }
            }
            return true;
        }
        #endregion

        #region 读数据

        /// <summary>
        ///     取得列表数据
        /// </summary>
        public List<TData> All()
        {
            return Access.All();
        }
        
        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public List<TData> All(Expression<Func<TData, bool>> lambda)
        {
            return Access.All(lambda);
        }

        /// <summary>
        ///     载入当前操作的数据
        /// </summary>
        public TData FirstOrDefault(Expression<Func<TData, bool>> lambda)
        {
            return Access.FirstOrDefault(lambda);
        }

        /// <summary>
        ///     载入当前操作的数据
        /// </summary>
        public virtual TData Details(long id)
        {
            return id == 0 ? null : Access.LoadByPrimaryKey(id);
        }

        #endregion
    }
}