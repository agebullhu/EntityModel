// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// 业务逻辑对象基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TAccess">数据访问对象</typeparam>
    public class BusinessLogicBase<TData, TAccess> : IBusinessLogicBase<TData>
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
        /// 数据访问对象
        /// </summary>
        IDataTable<TData> IBusinessLogicBase<TData>.Access => Access;

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

        /// <summary>
        ///     基本查询条件(SQL表述方式)
        /// </summary>
        protected virtual string BaseQueryCondition => null;

        #endregion

        #region 便利操作

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        public bool DoByIds(IEnumerable<long> ids, Func<long, bool> func, Action onEnd = null)
        {
            return LoopIds(ids, func, onEnd);
        }

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        public bool LoopIds(IEnumerable<long> ids, Func<long, bool> func, Action onEnd = null)
        {
            using (var scope = TransactionScope.CreateScope(Access.DataBase))
            {
                foreach (var id in ids)
                {
                    if (!func(id))
                    {
                        return false;
                    }
                }
                onEnd?.Invoke();
                scope.Succeed();
            }
            return true;
        }

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        public bool DoByIds(IEnumerable<long> ids, Func<TData, bool> func, Action onEnd = null)
        {
            return LoopIdsToData(ids, func, onEnd);
        }

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        public bool LoopIdsToData(IEnumerable<long> ids, Func<TData, bool> func, Action onEnd = null)
        {
            using (var scope = TransactionScope.CreateScope(Access.DataBase))
            {
                foreach (var id in ids)
                {
                    var data = Access.LoadByPrimaryKey(id);
                    if (data == null || !func(data))
                    {
                        return false;
                    }
                }
                onEnd?.Invoke();
                scope.Succeed();
            }
            return true;
        }
        #endregion

        #region 读数据

        /// <summary>
        ///     载入当前操作的数据
        /// </summary>
        [Obsolete]
        public TData FirstOrDefault(Expression<Func<TData, bool>> lambda)
        {
            var data = Access.FirstOrDefault(lambda);
            if (data == null)
                return null;
            OnDetailsLoaded(data, false);
            return data;
        }

        /// <summary>
        ///     载入当前操作的数据
        /// </summary>
        public virtual TData Details(long id)
        {
            if (id == 0)
                return null;
            var data = Access.LoadByPrimaryKey(id);
            if (data == null)
                return null;
            OnDetailsLoaded(data, false);
            return data;
        }

        /// <summary>
        ///     载入当前操作的数据
        /// </summary>
        public virtual async Task<TData> DetailAsync(long id)
        {
            if (id == 0)
                return null;
            var data = await Access.LoadByPrimaryKeyAsync(id);
            if (data == null)
                return null;
            OnDetailsLoaded(data, false);
            return data;
        }

        /// <summary>
        ///     详细数据载入
        /// </summary>
        protected virtual void OnDetailsLoaded(TData data, bool isNew)
        {
        }

        /// <summary>
        ///     新增一条带默认值的数据
        /// </summary>
        public virtual TData CreateData()
        {
            var data = new TData();
            OnDetailsLoaded(data, true);
            return data;
        }

        #endregion

    }
}