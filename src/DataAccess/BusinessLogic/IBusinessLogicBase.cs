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
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// 业务逻辑对象基类
    /// </summary>
    /// <typeparam name="TEntity">数据对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IBusinessLogicBase<TEntity,TPrimaryKey>
        where TEntity : EditDataObject, new()
    {
        #region 基础属性

        /// <summary>
        /// 数据访问对象
        /// </summary>
        IDataAccess<TEntity> Access { get; }

        /// <summary>
        /// 实体类型
        /// </summary>
        int EntityType { get; }

        #endregion

        #region 主键迭代操作

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        bool DoByIds(IEnumerable<TPrimaryKey> ids, Func<TPrimaryKey, bool> func, Action onEnd = null);

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        bool LoopIds(IEnumerable<TPrimaryKey> ids, Func<TPrimaryKey, bool> func, Action onEnd = null);

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        bool DoByIds(IEnumerable<TPrimaryKey> ids, Func<TEntity, bool> func, Action onEnd = null);

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        bool LoopIdsToData(IEnumerable<TPrimaryKey> ids, Func<TEntity, bool> func, Action onEnd = null);
        #endregion

        #region 读数据

        /// <summary>
        ///     载入当前操作的数据
        /// </summary>
        TEntity Details(TPrimaryKey id);

        #endregion

        #region 分页读取

        /// <summary>
        ///     取得列表数据
        /// </summary>
        ApiPageData<TEntity> PageData(int page, int limit, string sort, bool desc, string condition, params DbParameter[] args);

        /// <summary>
        ///     分页读取
        /// </summary>
        [Obsolete]
        ApiPageData<TEntity> PageData(int page, int limit, Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     分页读取
        /// </summary>
        [Obsolete]
        ApiPageData<TEntity> PageData(int page, int limit, LambdaItem<TEntity> lambda);
        #endregion

        #region 导出到Excel

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        (string name, string mime, byte[] bytes) Export(string sheetName, LambdaItem<TEntity> filter);

        #endregion

        #region 写数据

        /// <summary>
        ///     新增
        /// </summary>
        bool Save(TEntity data);

        /// <summary>
        ///     新增
        /// </summary>
        Task<bool> SaveAsync(TEntity data);

        /// <summary>
        ///     新增
        /// </summary>
        bool AddNew(TEntity data);

        /// <summary>
        ///     新增
        /// </summary>
        Task<bool> AddNewAsync(TEntity data);

        /// <summary>
        ///     更新对象
        /// </summary>
        bool Update(TEntity data);

        /// <summary>
        ///     更新对象
        /// </summary>
        Task<bool> UpdateAsync(TEntity data);

        #endregion

        #region 删除

        /// <summary>
        ///     删除对象
        /// </summary>
        bool Delete(IEnumerable<TPrimaryKey> lid);

        /// <summary>
        ///     删除对象
        /// </summary>
        bool Delete(TPrimaryKey id);

        /// <summary>
        ///     删除对象
        /// </summary>
        Task<bool> DeleteAsync(TPrimaryKey id);


        /// <summary>
        ///     删除对象
        /// </summary>
        Task<bool> DeleteAsync(IEnumerable<TPrimaryKey> lid);

        #endregion
    }
}