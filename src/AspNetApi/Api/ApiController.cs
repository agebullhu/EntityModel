using Agebull.Common.Ioc;
using Agebull.EntityModel.BusinessLogic;
using Agebull.EntityModel.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ZeroTeam.MessageMVC.ZeroApis;

#pragma warning disable IDE0060 // 删除未使用的参数
namespace ZeroTeam.AspNet.ModelApi
{
    /// <summary>
    ///     自动实现基本增删改查API页面的基类
    /// </summary>
    [ApiController]
    public abstract class ApiController<TData, TPrimaryKey, TBusinessLogic> : ControllerBase
        where TData : class, IIdentityData<TPrimaryKey>, new()
        where TBusinessLogic : BusinessLogicBase<TData, TPrimaryKey>, new()
    {
        #region 基础变量

        private TBusinessLogic _business;

        /// <summary>
        ///     业务逻辑对象
        /// </summary>
        protected TBusinessLogic Business => _business ??= new TBusinessLogic
        {
            Context = new BusinessContext(),
            ServiceProvider = DependencyHelper.ServiceProvider
        };

        /// <summary>
        ///     是否操作失败
        /// </summary>
        protected internal bool IsFailed => Business.Context.IsFailed;

        /// <summary>
        ///     设置当前操作失败
        /// </summary>
        /// <param name="message"></param>
        protected internal void SetFailed(string message)
        {
            Business.Context.LastState = Business.Context.BusinessError;
            Business.Context.LastMessage = message;
        }
        #endregion

        #region API

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        [Route("edit/unique")]
        public async Task<ApiResult> Unique(string field, string value, TPrimaryKey id)
        {
            var result = id.Equals(default)
                ? await Business.Access.IsUniqueAsync(field, value)
                : await Business.Access.IsUniqueAsync(field, value, id);

            return result
                ? ApiResultHelper.Succees()
                : ApiResultHelper.State(Business.Context.ArgumentError, $"{value} 已存在");
        }

        /// <summary>
        ///     列表数据
        /// </summary>
        /// <remarks>
        /// 参数中可传递实体字段具体的查询条件,所有的条件按AND组合查询
        /// </remarks>
        /// <returns></returns>
        [Route("edit/list")]
        public async Task<ApiResult<ApiPageData<TData>>> List([FromForm] PageArgument pageArgument, [FromBody] Dictionary<string, string> args)
        {
            var filter = GetQueryFilter(args);

            var data = await Business.PageData(pageArgument, filter);
            return IsFailed
                ? ApiResultHelper.FromContext<ApiPageData<TData>>(Business.Context)
                : ApiResultHelper.Succees(data);
        }

        /// <summary>
        ///     单条数据查询
        /// </summary>
        [Route("edit/first")]
        public async Task<ApiResult<TData>> QueryFirst([FromBody] Dictionary<string, string> args)
        {
            var filter = GetQueryFilter(args);
            var data = await Business.Details(filter);
            return IsFailed
                    ? ApiResultHelper.FromContext<TData>(Business.Context)
                    : ApiResultHelper.Succees(data);
        }

        /// <summary>
        ///     单条详细数据
        /// </summary>
        [Route("edit/details")]
        public async Task<ApiResult<TData>> Details(TPrimaryKey id)
        {
            var data = await Business.Details(id);
            return IsFailed
                    ? ApiResultHelper.FromContext<TData>(Business.Context)
                    : ApiResultHelper.Succees(data);
        }

        /// <summary>
        ///     新增数据
        /// </summary>
        [Route("edit/addnew")]
        public async Task<ApiResult<TData>> AddNew(TData data)
        {
            return await Business.AddNew(data)
                    ? ApiResultHelper.Succees(data)
                    : ApiResultHelper.FromContext<TData>(Business.Context);
        }

        /// <summary>
        ///     更新数据
        /// </summary>
        [Route("edit/update")]
        public async Task<ApiResult<TData>> Update(TData data)
        {
            return await Business.Update(data)
                    ? ApiResultHelper.Succees(data)
                    : ApiResultHelper.FromContext<TData>(Business.Context);

        }

        /// <summary>
        ///     删除多条数据
        /// </summary>
        [Route("edit/delete")]
        public async Task<ApiResult> Delete(List<TPrimaryKey> selects)
        {
            return await Business.Delete(selects)
                    ? ApiResultHelper.Succees()
                    : ApiResultHelper.FromContext(Business.Context);
        }

        /// <summary>
        ///     导出到Excel
        /// </summary>
        /// <remarks>
        /// 参数中可传递实体字段具体的查询条件,所有的条件按AND组合查询
        /// </remarks>
        /// <returns></returns>
        [Route("export/xlsx")]
        public async Task<ApiResult> Export([FromBody] Dictionary<string, string> args)
        {
            var filter = GetQueryFilter(args);
            var res = await Business.Export(filter);
            return ApiResultHelper.Succees(res);
        }

        /// <summary>
        ///     从Excep导入
        /// </summary>
        /// <remarks>
        /// 参数中可传递实体字段具体的查询条件,所有的条件按AND组合查询
        /// </remarks>
        /// <returns></returns>
        [Route("import/xlsx")]
        public async Task<ApiResult<byte[]>> Import(byte[] stream)
        {
            var (success, state) = await Business.Import(stream);
            var result = ApiResultHelper.Succees(state);
            if (!success)
            {
                result.Code = Business.Context.BusinessError;
                result.Message = "导入发生错误，请检查";
                result.Success = false;
            }
            return result;
        }

        #endregion

        #region 虚方法

        /// <summary>
        ///     读取查询条件
        /// </summary>
        /// <param name="args">筛选器</param>
        protected LambdaItem<TData> GetQueryFilter(Dictionary<string, string> args)
        {
            var filter = new LambdaItem<TData>();

            foreach (var arg in args)
                GetQueryFilter(filter, arg.Key, arg.Value);
            return filter;
        }

        /// <summary>
        ///     读取查询条件
        /// </summary>
        /// <param name="filter">筛选器</param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        protected virtual void GetQueryFilter(LambdaItem<TData> filter, string field, string value)
        {
            if (Business.Access.Option.PropertyMap.TryGetValue(field, out var property))
                filter.AddOr(p => property.FieldName == value);
        }

        #endregion
    }

}
#pragma warning restore IDE0060 // 删除未使用的参数