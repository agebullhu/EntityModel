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
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IUiBusinessLogicBase<TData,TPrimaryKey> : IBusinessLogicBase<TData, TPrimaryKey>
        where TData : EditDataObject, IIdentityData<TPrimaryKey>, new()
    {
        #region 读数据

        /// <summary>
        ///     取得列表数据
        /// </summary>
        [Obsolete]
        ApiPageData<TData> PageData(int page, int limit, string condition, params DbParameter[] args);

        /// <summary>
        ///     取得列表数据
        /// </summary>
        ApiPageData<TData> PageData(int page, int limit, string sort, bool desc, string condition, params DbParameter[] args);

        /// <summary>
        ///     分页读取
        /// </summary>
        [Obsolete]
        ApiPageData<TData> PageData(int page, int limit, Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     分页读取
        /// </summary>
        [Obsolete]
        ApiPageData<TData> PageData(int page, int limit, LambdaItem<TData> lambda);
        #endregion

        #region 导出到Excel

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        (string name, string mime, byte[] bytes) Export(string sheetName, LambdaItem<TData> filter);

        #endregion

        #region 写数据

        /// <summary>
        ///     新增
        /// </summary>
        bool Save(TData data);

        /// <summary>
        ///     新增
        /// </summary>
        Task<bool> SaveAsync(TData data);

        /// <summary>
        ///     新增
        /// </summary>
        bool AddNew(TData data);

        /// <summary>
        ///     新增
        /// </summary>
        Task<bool> AddNewAsync(TData data);

        /// <summary>
        ///     更新对象
        /// </summary>
        bool Update(TData data);

        /// <summary>
        ///     更新对象
        /// </summary>
        Task<bool> UpdateAsync(TData data);

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