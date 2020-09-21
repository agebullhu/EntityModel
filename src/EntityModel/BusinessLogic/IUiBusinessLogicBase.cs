using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.ZeroApis;

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// 支持界面操作的业务逻辑对象基类
    /// </summary>
    /// <typeparam name="TEntity">数据对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IUiBusinessLogicBase<TEntity,TPrimaryKey> : IBusinessLogicBase<TEntity, TPrimaryKey>
        where TEntity : EditDataObject, IIdentityData<TPrimaryKey>, new()
    {
        #region 读数据

        /// <summary>
        ///     取得列表数据
        /// </summary>
        [Obsolete]
        ApiPageData<TEntity> PageData(int page, int limit, string condition, params DbParameter[] args);

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