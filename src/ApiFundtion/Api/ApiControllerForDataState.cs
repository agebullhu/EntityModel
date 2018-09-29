// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Linq.Expressions;
using System.Web.Http;
using Agebull.Common.DataModel.BusinessLogic;
using Agebull.Common.Rpc;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;

#endregion

namespace Agebull.Common.WebApi
{
    /// <summary>
    ///     支持数据状态的启用禁用方法的页面的基类
    /// </summary>
    public abstract class ApiControllerForDataState<TData, TAccess, TDatabase, TBusinessLogic> :
        ApiController<TData, TAccess, TDatabase, TBusinessLogic>
        where TData : EditDataObject, IStateData, IIdentityData, new()
        where TAccess : DataStateTable<TData, TDatabase>, new()
        where TBusinessLogic : BusinessLogicByStateData<TData, TAccess, TDatabase>, new()
        where TDatabase : MySqlDataBase
    {
        #region 数据校验支持

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="name"></param>
        /// <param name="field"></param>
        protected override void CheckUnique<TValue>(string name, Expression<Func<TData, TValue>> field)
        {
            var no = GetArg("No");
            if (string.IsNullOrEmpty(no))
            {
                SetFailed(name + "为空");
                return;
            }

            var id = GetIntArg("id", 0);
            Expression<Func<TData, bool>> condition;
            if (id == 0)
                condition = p => p.DataState < DataStateType.Delete;
            else
                condition = p => p.Id != id && p.DataState < DataStateType.Delete;
            if (Business.Access.IsUnique(field, no, condition))
                SetFailed(name + "[" + no + "]不唯一");
            else
                GlobalContext.Current.LastMessage = name + "[" + no + "]唯一";
        }

        #endregion

        #region API

        /// <summary>
        ///      修改数据状态
        /// </summary>
        [HttpPost]
        [Route("state/reset")]
        public ApiResponseMessage Reset()
        {
            
            OnReset();
            return IsFailed
                ? Request.ToResponse(new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
                : Request.ToResponse(ApiResult.Succees());
        }

        /// <summary>
        ///      修改数据状态
        /// </summary>
        [HttpPost]
        [Route("state/lock")]
        public ApiResponseMessage Lock()
        {
            
            OnLock();
            return IsFailed
                ? Request.ToResponse(new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
                : Request.ToResponse(ApiResult.Succees());
        }

        /// <summary>
        ///      修改数据状态
        /// </summary>
        [HttpPost]
        [Route("state/discard")]
        public ApiResponseMessage Discard()
        {
            
            OnDiscard();
            return IsFailed
                ? Request.ToResponse(new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
                : Request.ToResponse(ApiResult.Succees());
        }

        /// <summary>
        ///      修改数据状态
        /// </summary>
        [HttpPost]
        [Route("state/disable")]
        public ApiResponseMessage Disable()
        {
            
            OnDisable();
            return IsFailed
                ? Request.ToResponse(new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
                : Request.ToResponse(ApiResult.Succees());
        }

        /// <summary>
        ///      修改数据状态
        /// </summary>
        [HttpPost]
        [Route("state/enable")]
        public ApiResponseMessage Enable()
        {
            
            OnEnable();
            return IsFailed
                ? Request.ToResponse(new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
                : Request.ToResponse(ApiResult.Succees());
        }

        #endregion

        #region 操作

        /// <summary>
        ///     锁定对象
        /// </summary>
        protected virtual void OnLock()
        {
            var ids = GetLongArrayArg("selects");
            if (!Business.LoopIds(ids, Business.Lock))
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
        }

        /// <summary>
        ///     恢复对象
        /// </summary>
        private void OnReset()
        {
            var ids = GetLongArrayArg("selects");
            if (!Business.LoopIds(ids, Business.Reset))
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
        }

        /// <summary>
        ///     废弃对象
        /// </summary>
        private void OnDiscard()
        {
            var ids = GetLongArrayArg("selects");
            if (!Business.LoopIds(ids, Business.Discard))
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
        }

        /// <summary>
        ///     启用对象
        /// </summary>
        private void OnEnable()
        {
            var ids = GetLongArrayArg("selects");
            if (!Business.LoopIds(ids, Business.Enable))
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
        }

        /// <summary>
        ///     禁用对象
        /// </summary>
        private void OnDisable()
        {
            var ids = GetLongArrayArg("selects");
            if (!Business.LoopIds(ids, Business.Disable))
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
        }

        #endregion


        #region 列表数据

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected override ApiPageData<TData> GetListData()
        {
            var root = new LambdaItem<TData>();
            return GetListData(root);
        }

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected override ApiPageData<TData> GetListData(LambdaItem<TData> lambda)
        {
            var state = GetIntArg("dataState", 0x100);
            if (state >= 0)
            {
                if (state < 0x100)
                    lambda.AddRoot(p => p.DataState == (DataStateType)state);
                else
                    lambda.AddRoot(p => p.DataState < DataStateType.Delete);
            }

            return DoGetListData(lambda);
        }

        #endregion
    }

    /// <summary>
    ///     支持数据状态的启用禁用方法的页面的基类
    /// </summary>
    public abstract class ApiControllerForDataState<TData, TAccess, TDatabase> :
        ApiController<TData, TAccess, TDatabase, BusinessLogicByStateData<TData, TAccess, TDatabase>>
        where TData : EditDataObject, IStateData, IIdentityData, new()
        where TAccess : DataStateTable<TData, TDatabase>, new()
        where TDatabase : MySqlDataBase
    {
    }
}