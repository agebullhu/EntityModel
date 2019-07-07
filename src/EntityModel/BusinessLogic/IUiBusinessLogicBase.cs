using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using Agebull.EntityModel.Common;
using Agebull.MicroZero.ZeroApis;

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// 支持界面操作的业务逻辑对象基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    public interface IUiBusinessLogicBase<TData> : IBusinessLogicBase<TData>
        where TData : EditDataObject, IIdentityData, new()
    {
        #region 读数据

        /// <summary>
        ///     取得列表数据
        /// </summary>
        ApiPageData<TData> PageData(int page, int limit, string condition, params DbParameter[] args);

        /// <summary>
        ///     取得列表数据
        /// </summary>
        ApiPageData<TData> PageData(int page, int limit, string sort, bool desc, string condition, params DbParameter[] args);

        /// <summary>
        ///     分页读取
        /// </summary>
        ApiPageData<TData> PageData(int page, int limit, Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     分页读取
        /// </summary>
        ApiPageData<TData> PageData(int page, int limit, LambdaItem<TData> lambda);
        #endregion

        #region 导出到Excel

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        ApiFileResult Import(string sheetName, LambdaItem<TData> filter);

        #endregion

        #region 写数据

        /// <summary>
        ///     新增
        /// </summary>
        bool Save(TData data);

        /// <summary>
        ///     新增
        /// </summary>
        bool AddNew(TData data);

        /// <summary>
        ///     更新对象
        /// </summary>
        bool Update(TData data);

        #endregion

        #region 删除

        /// <summary>
        ///     删除对象
        /// </summary>
        bool Delete(IEnumerable<long> lid);

        /// <summary>
        ///     删除对象
        /// </summary>
        bool Delete(long id);

        #endregion
    }
}