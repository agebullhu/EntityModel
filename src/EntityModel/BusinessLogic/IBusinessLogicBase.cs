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
using Agebull.EntityModel.Common;

#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// 业务逻辑对象基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    public interface IBusinessLogicBase<TData>
        where TData : EditDataObject, new()
    {
        /// <summary>
        /// 数据访问对象
        /// </summary>
        IDataTable<TData> Access { get; }

        /// <summary>
        /// 实体类型
        /// </summary>
        int EntityType { get; }
        #region 便利操作

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        bool DoByIds(IEnumerable<long> ids, Func<long, bool> func, Action onEnd = null);

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        bool LoopIds(IEnumerable<long> ids, Func<long, bool> func, Action onEnd = null);

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        bool DoByIds(IEnumerable<long> ids, Func<TData, bool> func, Action onEnd = null);

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        bool LoopIdsToData(IEnumerable<long> ids, Func<TData, bool> func, Action onEnd = null);
        #endregion

        #region 读数据

        /// <summary>
        ///     取得列表数据
        /// </summary>
        List<TData> All();

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        List<TData> All(LambdaItem<TData> lambda);

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        List<TData> All(Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     载入当前操作的数据
        /// </summary>
        TData FirstOrDefault(Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     载入当前操作的数据
        /// </summary>
        TData Details(long id);

        #endregion
    }
}