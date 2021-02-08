using Agebull.Common.Ioc;
using Agebull.EntityModel.BusinessLogic;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.Vue;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.ZeroApis;

#pragma warning disable IDE0060 // 删除未使用的参数
namespace ZeroTeam.MessageMVC.ModelApi
{
    /// <summary>
    ///     自动实现基本增删改查API页面的基类
    /// </summary>
    public abstract class ApiController<TData, TPrimaryKey, TBusinessLogic> : IApiController
        where TData : class, IIdentityData<TPrimaryKey>, new()
        where TBusinessLogic : BusinessLogicBase<TData, TPrimaryKey>, new()
    {
        #region Business.Context

        /// <summary>
        ///     业务逻辑对象
        /// </summary>
        protected TBusinessLogic Business { get; }

        /// <summary>
        /// 构造
        /// </summary>
        protected ApiController()
        {
            Business = new TBusinessLogic
            {
                Context = DependencyHelper.GetService<IBusinessContext>(),
                ServiceProvider = DependencyHelper.ServiceProvider
            };
            Business.Context.LastState = OperatorStatusCode.Success;
        }

        /// <summary>
        ///     是否操作失败
        /// </summary>
        protected internal bool IsFailed => Business.Context.LastState != OperatorStatusCode.Success;

        /// <summary>
        ///     设置当前操作失败
        /// </summary>
        /// <param name="message"></param>
        protected internal void SetFailed(string message)
        {
            Business.Context.LastState = OperatorStatusCode.BusinessError;
            Business.Context.LastMessage = message;
        }
        /// <summary>
        /// 根据上下文状态返回
        /// </summary>
        /// <returns></returns>
        protected IApiResult ContextStatusResult()
        {
            return ApiResultHelper.State(Business.Context.LastState, Business.Context.LastMessage);
        }

        /// <summary>
        /// 根据上下文状态返回
        /// </summary>
        /// <returns></returns>
        protected IApiResult<T> ContextStatusResult<T>()
        {
            return ApiResultHelper.State<T>(Business.Context.LastState, Business.Context.LastMessage);
        }

        /// <summary>
        /// 根据上下文状态返回
        /// </summary>
        /// <returns></returns>
        protected IApiResult ContextResult()
        {
            return Business.Context.LastState != OperatorStatusCode.Success
                ? ApiResultHelper.State(Business.Context.LastState, Business.Context.LastMessage)
                : ApiResultHelper.Succees();
        }


        /// <summary>
        /// 根据上下文状态返回
        /// </summary>
        /// <returns></returns>
        protected IApiResult<T> ContextResult<T>(T value)
        {
            return Business.Context.LastState != OperatorStatusCode.Success
                ? ApiResultHelper.State<T>(Business.Context.LastState, Business.Context.LastMessage)
                : ApiResultHelper.Succees(value);
        }

        #endregion

        #region API

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <param name="field">检查的字段</param>
        /// <param name="value">检查的值</param>
        /// <param name="id">应排除的ID，即不检查自身</param>
        [Route("edit/unique")]
        [ApiOption(ApiOption.Public | ApiOption.Readonly)]
        public async Task<IApiResult> Unique(string field, string value, TPrimaryKey id)
        {
            var result = id.Equals(default)
                ? await Business.Access.IsUniqueAsync(field, value)
                : await Business.Access.IsUniqueAsync(field, value, id);

            return result
                ? ApiResultHelper.Succees()
                : ApiResultHelper.State(OperatorStatusCode.ArgumentError, $"{value} 已存在");
        }

        /// <summary>
        ///     列表数据
        /// </summary>
        /// <remarks>
        /// 参数中可传递实体字段具体的查询条件,所有的条件按AND组合查询
        /// </remarks>
        /// <returns></returns>
        [Route("edit/list")]
        [ApiOption(ApiOption.Public | ApiOption.Readonly | ApiOption.DictionaryArgument)]
        public async Task<IApiResult<ApiPageData<TData>>> List(TData args)
        {
            var filter = GetQueryFilter();
            using var scope = GetFieldsFilter();
            var data = await GetListData(filter);
            return ContextResult(data);
        }

        /// <summary>
        ///     单条数据查询
        /// </summary>
        [Route("edit/first")]
        [ApiOption(ApiOption.Public | ApiOption.Readonly | ApiOption.DictionaryArgument)]
        public async Task<IApiResult<TData>> QueryFirst(TData args)
        {
            var filter = GetQueryFilter();
            using var scope = GetFieldsFilter();
            var data = await Business.Details(filter);
            return ContextResult(data);
        }

        /// <summary>
        ///     单条详细数据
        /// </summary>
        /// <param name="arg">查询参数</param>
        [Route("edit/details")]
        [ApiOption(ApiOption.Public | ApiOption.Readonly | ApiOption.DictionaryArgument)]
        public async Task<IApiResult<TData>> Details(IdArgument<TPrimaryKey> arg)
        {
            if (!RequestArgumentConvert.TryGetId<TData, TPrimaryKey>(Convert, out TPrimaryKey id))
                return ApiResultHelper.State<TData>(OperatorStatusCode.ArgumentError, "参数[id]不是有效的主键");
            using var scope = GetFieldsFilter();
            var data = await Business.Details(id);
            return ContextResult(data);
        }

        /// <summary>
        ///     新增数据
        /// </summary>
        [Route("edit/addnew")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult<TData>> AddNew(TData arg)
        {
            var data = new TData();
            if (data is IEditStatus status && status.EditStatusRecorder != null)
            {
                status.EditStatusRecorder.IsExist = false;
                status.EditStatusRecorder.IsFromClient = true;
            }
            var convert = new FormConvert();
            await ReadFormData(data, convert);
            if (convert.Failed)
            {
                return ApiResultHelper.State<TData>(OperatorStatusCode.ArgumentError, convert.Message);
            }
            return await Business.AddNew(data)
                    ? ApiResultHelper.Succees(data)
                    : ApiResultHelper.State<TData>(Business.Context.LastState, Business.Context.LastMessage);
        }

        /// <summary>
        ///     更新数据
        /// </summary>
        [Route("edit/update")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult<TData>> Update(TData arg)
        {
            if (!RequestArgumentConvert.TryGet("id", out string id))
                return ApiResultHelper.State<TData>(OperatorStatusCode.ArgumentError, "参数[id]无效");
            var key = Convert(id).Item2;
            var data = await Business.Access.LoadByPrimaryKeyAsync(key);
            if (data == null)
                return ApiResultHelper.State<TData>(OperatorStatusCode.ArgumentError, "参数[id]无效");
            var convert = new FormConvert();
            if (data is IEditStatus status && status.EditStatusRecorder != null)
            {
                status.EditStatusRecorder.IsExist = true;
                status.EditStatusRecorder.IsFromClient = true;
            }

            await ReadFormData(data, convert);
            if (convert.Failed)
            {
                return ApiResultHelper.State<TData>(OperatorStatusCode.ArgumentError, convert.Message);
            }
            return await Business.Update(data)
                    ? ApiResultHelper.Succees(data)
                    : ApiResultHelper.State<TData>(Business.Context.LastState, Business.Context.LastMessage);

        }

        /// <summary>
        ///     删除多条数据
        /// </summary>
        [Route("edit/delete")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult> Delete(IdsArgument<TPrimaryKey> args)
        {
            if (!RequestArgumentConvert.TryGetIDs("selects", Convert, out List<TPrimaryKey> ids))
            {
                return ApiResultHelper.State(OperatorStatusCode.ArgumentError);
            }
            return await Business.Delete(ids)
                    ? ApiResultHelper.Succees()
                    : ApiResultHelper.State(Business.Context.LastState, Business.Context.LastMessage);
        }

        /// <summary>
        ///     导出到Excel
        /// </summary>
        /// <remarks>
        /// 参数中可传递实体字段具体的查询条件,所有的条件按AND组合查询
        /// </remarks>
        /// <returns></returns>
        [Route("export/xlsx")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult> Export(TData args, [FromServices] IHttpContextAccessor accessor)
        {
            var filter = GetQueryFilter();
            var res = await Business.Export(Business.Access.Option.DataStruct.Caption, filter);
            //accessor.HttpContext.Response.ContentType = res.mime;
            //await accessor.HttpContext.Response.Body.WriteAsync(res.bytes);

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
        [ApiOption(ApiOption.Public)]
        public async Task<IApiResult<byte[]>> Import(byte[] file)
        {
            var (success, state) = await Business.Import(file);
            var result = ApiResultHelper.Succees(state);
            if (!success)
            {
                result.Code = OperatorStatusCode.BusinessError;
                result.Message = "导入发生错误，请检查";
                result.Success = false;
            }
            return result;
        }

        #endregion

        #region 读取字段过滤器

        /// <summary>
        /// 读取字段过滤器
        /// </summary>
        /// <returns></returns>
        IDisposable GetFieldsFilter()
        {
            var scope = new AgentScope();
            if (RequestArgumentConvert.TryGet("_fields_", out string fields) && fields.IsNotNull())
            {
                scope.Client = Business.Access.Select(fields.Split(','));
            }
            return scope;
        }


        #endregion

        #region 列表读取支持

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected virtual Task<ApiPageData<TData>> GetListData(LambdaItem<TData> lambda)
        {
            var page = RequestArgumentConvert.GetInt("_page_", 1);
            var size = RequestArgumentConvert.GetInt("_size_", 20);
            RequestArgumentConvert.TryGet("_sort_", out string sort);
            var desc = RequestArgumentConvert.TryGet("_order_", out string order) && (order?.ToLower() == "true" || order?.ToLower() == "desc");

            return Business.PageData(page, size, sort, desc, lambda);
        }

        #endregion

        #region 虚方法

        /// <summary>
        ///     读取查询条件
        /// </summary>
        LambdaItem<TData> GetQueryFilter()
        {
            //if (RequestArgumentConvert.TryGet("_value_", out string value) && !string.IsNullOrEmpty(value))
            //{
            //    var field = RequestArgumentConvert.GetString("_field_");
            //    if (field != "_any_")
            //        RequestArgumentConvert.SetArgument(field, value);
            //}
            var filter = new LambdaItem<TData>();
            if (RequestArgumentConvert.TryGet("_filters_", out string f))
            {
                var fields = f.Split(new char[] { '[', ',', ']' }, StringSplitOptions.RemoveEmptyEntries);
                if (fields.Length > 0)
                    filter.Fields = fields;
            }
            //if (RequestArgumentConvert.TryGet("_orderbys_", out string orderby))
            //{
            //    var fields = f.Split(new char[] { '[', ',', ']' }, StringSplitOptions.RemoveEmptyEntries);
            //    if (fields.Length > 0)
            //        filter.Fields = fields;
            //}
            GetCustomFilter(filter);
            GetQueryFilter(filter);
            return filter;
        }

        /// <summary>
        ///     读取查询条件
        /// </summary>
        /// <param name="filter">筛选器</param>
        protected virtual void GetCustomFilter(LambdaItem<TData> filter)
        {

        }

        /// <summary>
        ///     读取查询条件
        /// </summary>
        /// <param name="filter">筛选器</param>
        public virtual void GetQueryFilter(LambdaItem<TData> filter)
        {

        }

        /// <summary>
        ///     读取Form传过来的数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="convert">转化器</param>
        protected abstract Task ReadFormData(TData entity, FormConvert convert);

        /// <summary>
        /// 转换方法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        abstract protected (bool, TPrimaryKey) Convert(string value);

        #endregion
    }

    /// <summary>
    ///     自动实现基本增删改查API页面的基类
    /// </summary>
    public abstract class ApiController<TData, TBusinessLogic> : ApiController<TData, long, TBusinessLogic>
        where TData : class, IIdentityData<long>, new()
        where TBusinessLogic : BusinessLogicBase<TData, long>, new()
    {
        ///<inheritdoc/>
        protected sealed override (bool, long) Convert(string value)
        {
            if (value != null && long.TryParse(value, out var id))
            {
                return (true, id);
            }
            return (false, 0);
        }
    }

}
#pragma warning restore IDE0060 // 删除未使用的参数