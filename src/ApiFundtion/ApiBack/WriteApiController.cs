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
    public abstract class WriteApiController<TData, TPrimaryKey, TBusinessLogic> : QueryApiController<TData, TPrimaryKey, TBusinessLogic>
        where TData : class, IIdentityData<TPrimaryKey>, new()
        where TBusinessLogic : BusinessLogicBase<TData, TPrimaryKey>, new()
    {
        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        protected async Task<IApiResult> Unique(string field, string value, TPrimaryKey id)
        {
            var result = id.Equals(default)
                ? await Business.Access.IsUniqueAsync(field, value)
                : await Business.Access.IsUniqueAsync(field, value, id);

            return result
                ? ApiResultHelper.Succees()
                : ApiResultHelper.State(OperatorStatusCode.ArgumentError, $"{value} 已存在");
        }

        /// <summary>
        ///     新增数据
        /// </summary>
        protected async Task<IApiResult<TData>> AddNew(TData data)
        {
            return await Business.AddNew(data)
                    ? ApiResultHelper.Succees(data)
                    : ApiResultHelper.State<TData>(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage);
        }

        /// <summary>
        ///     更新数据
        /// </summary>
        protected async Task<IApiResult<TData>> Update(TData data)
        {
            if (data is IEditStatus status && status.EditStatusRedorder != null)
            {
                status.EditStatusRedorder.IsExist = true;
                status.EditStatusRedorder.IsFromClient = true;
            }
            return await Business.Update(data)
                    ? ApiResultHelper.Succees(data)
                    : ApiResultHelper.State<TData>(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage);

        }

        /// <summary>
        ///     删除多条数据
        /// </summary>
        protected async Task<IApiResult> Delete(List<TPrimaryKey> ids)
        {
            return await Business.Delete(ids)
                    ? ApiResultHelper.Succees()
                    : ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage);
        }


        /// <summary>
        ///     从Excep导入
        /// </summary>
        /// <remarks>
        /// 参数中可传递实体字段具体的查询条件,所有的条件按AND组合查询
        /// </remarks>
        /// <returns></returns>
        protected async Task<IApiResult<byte[]>> Import(byte[] stream)
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

    }

    /// <summary>
    ///     自动实现基本增删改查API页面的基类
    /// </summary>
    public abstract class WriteApiController<TData, TBusinessLogic> : WriteApiController<TData, long, TBusinessLogic>
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