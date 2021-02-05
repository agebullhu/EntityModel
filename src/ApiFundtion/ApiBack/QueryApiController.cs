using Agebull.EntityModel.BusinessLogic;
using Agebull.EntityModel.Common;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.Context;
using ZeroTeam.MessageMVC.ZeroApis;

#pragma warning disable IDE0060 // 删除未使用的参数
namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    ///     自动实现基本增删改查API页面的基类
    /// </summary>
    public abstract class QueryApiController<TData, TPrimaryKey, TBusinessLogic> : ModelApiController
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

        #region 读取支持

        /// <summary>
        ///     列表数据
        /// </summary>
        /// <remarks>
        /// 参数中可传递实体字段具体的查询条件,所有的条件按AND组合查询
        /// </remarks>
        /// <returns></returns>
        protected async Task<IApiResult<ApiPageData<TData>>> List(PageArgument pageArgument,LambdaItem<TData> filter)
        {
            var data = await Business.PageData(pageArgument, filter);
            GlobalContext.Current.Status.Feature = 0;
            return IsFailed
                ? ApiResultHelper.State<ApiPageData<TData>>(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                : ApiResultHelper.Succees(data);
        }

        /// <summary>
        ///     单条数据查询
        /// </summary>
        protected async Task<IApiResult<TData>> QueryFirst(LambdaItem<TData> filter)
        {
            var data = await Business.Details(filter);
            return IsFailed
                    ? ApiResultHelper.State<TData>(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees(data);
        }

        /// <summary>
        ///     单条详细数据
        /// </summary>
        protected async Task<IApiResult<TData>> Details(TPrimaryKey id)
        {
            var data = await Business.Details(id);
            return IsFailed
                    ? ApiResultHelper.State<TData>(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees(data);
        }

        /// <summary>
        ///     导出到Excel
        /// </summary>
        /// <remarks>
        /// 参数中可传递实体字段具体的查询条件,所有的条件按AND组合查询
        /// </remarks>
        /// <returns></returns>
        protected async Task<IApiResult> Export(LambdaItem<TData> filter)
        {
            var (name, mime, bytes) = await Business.Export(filter);
            return ApiResultHelper.Succees(new
            {
                name,
                mime,
                bytes
            });
        }
        #endregion

        #region 虚方法

        /// <summary>
        ///     导出到Excel
        /// </summary>
        /// <remarks>
        /// 参数中可传递实体字段具体的查询条件,所有的条件按AND组合查询
        /// </remarks>
        /// <returns></returns>
        protected (bool, TPrimaryKey) GetPrimaryKey()
        {
            if (!RequestArgumentConvert.TryGetId<TData, TPrimaryKey>(Convert, out TPrimaryKey id))
            {
                GlobalContext.Current.Status.LastState = OperatorStatusCode.ArgumentError;
                GlobalContext.Current.Status.LastMessage = "参数[id]不是有效的主键";
                return (false, default);
            }
            return (true, id);
        }

        /// <summary>
        ///     导出到Excel
        /// </summary>
        /// <remarks>
        /// 参数中可传递实体字段具体的查询条件,所有的条件按AND组合查询
        /// </remarks>
        /// <returns></returns>
        protected PageArgument GetPageArgument()
        {
            return new PageArgument
            {
                Page = RequestArgumentConvert.GetInt("_page_", 1),
                PageSize = RequestArgumentConvert.GetInt("_size_", 20),
                Order = !RequestArgumentConvert.TryGet("_sort_", out string sort) && string.IsNullOrEmpty(sort)
                    ? Business.Access.Option.PrimaryKey
                    : sort,
                Desc = RequestArgumentConvert.TryGet("_order_", out string order) && order?.ToLower() == "desc"
            };
        }

        /// <summary>
        ///     导出到Excel
        /// </summary>
        /// <remarks>
        /// 参数中可传递实体字段具体的查询条件,所有的条件按AND组合查询
        /// </remarks>
        /// <returns></returns>
        protected LambdaItem<TData> GetQueryFilter()
        {
            GlobalContext.Current.Status.Feature = 1;
            var filter = new LambdaItem<TData>();
            GetQueryFilter(filter);
            return filter;
        }

        /// <summary>
        ///     读取查询条件
        /// </summary>
        /// <param name="filter">筛选器</param>
        protected abstract void GetQueryFilter(LambdaItem<TData> filter);

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
    public abstract class QueryApiController<TData, TBusinessLogic> : QueryApiController<TData, long, TBusinessLogic>
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