using Agebull.EntityModel.BusinessLogic;
using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.Context;
using ZeroTeam.MessageMVC.ZeroApis;

#pragma warning disable IDE0060 // 删除未使用的参数
namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    ///     自动实现基本增删改查API页面的基类
    /// </summary>
    public abstract class ApiController<TData, TPrimaryKey, TBusinessLogic> : ModelApiController
        where TData : class, IIdentityData<TPrimaryKey>, new()
        where TBusinessLogic : BusinessLogicBase<TData, TPrimaryKey>, new()
    {
        #region 基础变量

        private TBusinessLogic _business;

        /// <summary>
        ///     业务逻辑对象
        /// </summary>
        protected TBusinessLogic Business
        {
            get => _business ??= new TBusinessLogic();
            set => _business = value;
        }

        #endregion

        #region API

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        [Route("edit/unique")]
        [ApiOption(ApiOption.Public | ApiOption.Readonly | ApiOption.DictionaryArgument)]
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
        public async Task<IApiResult<ApiPageData<TData>>> List()
        {
            GlobalContext.Current.Status.Feature = 1;
            var filter = new LambdaItem<TData>();
            GetQueryFilter(filter);

            var data = await GetListData(filter);
            GlobalContext.Current.Status.Feature = 0;
            return IsFailed
                ? ApiResultHelper.State<ApiPageData<TData>>(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                : ApiResultHelper.Succees(data);
        }

        /// <summary>
        ///     单条数据查询
        /// </summary>
        [Route("edit/first")]
        [ApiOption(ApiOption.Public | ApiOption.Readonly | ApiOption.DictionaryArgument)]
        public async Task<IApiResult<TData>> QueryFirst()
        {
            var filter = new LambdaItem<TData>();
            GetQueryFilter(filter);
            var data = await Business.Details(filter);
            return IsFailed
                    ? ApiResultHelper.State<TData>(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees(data);
        }

        /// <summary>
        ///     单条详细数据
        /// </summary>
        [Route("edit/details")]
        [ApiOption(ApiOption.Public | ApiOption.Readonly | ApiOption.DictionaryArgument)]
        public async Task<IApiResult<TData>> Details()
        {
            if (!RequestArgumentConvert.TryGetId<TData, TPrimaryKey>(Convert, out TPrimaryKey id))
                return ApiResultHelper.State<TData>(OperatorStatusCode.ArgumentError, "参数[id]不是有效的主键");
            var data = await Business.Details(id);
            return IsFailed
                    ? ApiResultHelper.State<TData>(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees(data);
        }

        /// <summary>
        ///     新增数据
        /// </summary>
        [Route("edit/addnew")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult<TData>> AddNew(TData arg)
        {
            var data = new TData();
            if(data is IEditStatus status)
            {
                status.EditStatusRedorder.IsExist = false;
                status.EditStatusRedorder.IsFromClient = true;
            }
            var convert = new FormConvert();
            ReadFormData(data, convert);
            if (convert.Failed)
            {
                return ApiResultHelper.State<TData>(OperatorStatusCode.ArgumentError, convert.Message);
            }
            return await Business.AddNew(data)
                    ? ApiResultHelper.Succees(data)
                    : ApiResultHelper.State<TData>(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage);
        }

        /// <summary>
        ///     更新数据
        /// </summary>
        [Route("edit/update")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult<TData>> Update(TData arg)
        {
            var data = new TData();

            if (data is IEditStatus status)
            {
                status.EditStatusRedorder.IsExist = true;
                status.EditStatusRedorder.IsFromClient = true;
            }
            var convert = new FormConvert();
            if (!convert.Arguments.TryGetValue("id", out var id) )
                return ApiResultHelper.State<TData>(OperatorStatusCode.ArgumentError, "id必传");
            
            data.Id = Convert(id).Item2;
            ReadFormData(data, convert);
            if (convert.Failed)
            {
                return ApiResultHelper.State<TData>(OperatorStatusCode.ArgumentError, convert.Message);
            }
            return await Business.Update(data)
                    ? ApiResultHelper.Succees(data)
                    : ApiResultHelper.State<TData>(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage);

        }

        /// <summary>
        ///     删除多条数据
        /// </summary>
        [Route("edit/delete")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult> Delete()
        {
            if (!RequestArgumentConvert.TryGetIDs("selects", Convert, out List<TPrimaryKey> ids))
            {
                return ApiResultHelper.State(OperatorStatusCode.ArgumentError);
            }
            return await Business.Delete(ids)
                    ? ApiResultHelper.Succees()
                    : ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage);
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
        public async Task<IApiResult> Export(TData args)
        {
            GlobalContext.Current.Status.Feature = 1;
            var filter = new LambdaItem<TData>();
            GetQueryFilter(filter);
            var res = await Business.Export(filter);
            GlobalContext.Current.Status.Feature = 0;
            return ApiResultHelper.Succees(res);
        }

        /// <summary>
        ///     从Excep导入
        /// </summary>
        /// <remarks>
        /// 参数中可传递实体字段具体的查询条件,所有的条件按AND组合查询
        /// </remarks>
        /// <returns></returns>
        [Obsolete]
        [Route("import/xlsx")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult<byte[]>> Import(byte[] stream)
        {
            var (success, state) = await Business.Import(stream);
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

        #region 列表读取支持

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected virtual Task<ApiPageData<TData>> GetListData(LambdaItem<TData> lambda)
        {
            var page = RequestArgumentConvert.GetInt("_page_", 1);
            var size = RequestArgumentConvert.GetInt("_size_", 20);
            if (!RequestArgumentConvert.TryGet("_sort_", out string sort) && string.IsNullOrEmpty(sort))
                sort = Business.Access.Option.PrimaryKey;
            var desc = RequestArgumentConvert.TryGet("_order_", out string order) && order?.ToLower() == "desc";
            return Business.PageData(page, size, sort, desc, lambda);

        }

        #endregion

        #region 虚方法

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
        protected abstract void ReadFormData(TData entity, FormConvert convert);

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