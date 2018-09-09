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
using Agebull.Common.WebApi;
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
                Message = name + "[" + no + "]唯一";
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
            InitForm();
            OnReset();
            return IsFailed
                ? Request.ToResponse(ApiResult.Error(State, Message))
                : Request.ToResponse(ApiResult.Succees());
        }

        /// <summary>
        ///      修改数据状态
        /// </summary>
        [HttpPost]
        [Route("state/lock")]
        public ApiResponseMessage Lock()
        {
            InitForm();
            OnLock();
            return IsFailed
                ? Request.ToResponse(ApiResult.Error(State, Message))
                : Request.ToResponse(ApiResult.Succees());
        }

        /// <summary>
        ///      修改数据状态
        /// </summary>
        [HttpPost]
        [Route("state/discard")]
        public ApiResponseMessage Discard()
        {
            InitForm();
            OnDiscard();
            return IsFailed
                ? Request.ToResponse(ApiResult.Error(State, Message))
                : Request.ToResponse(ApiResult.Succees());
        }

        /// <summary>
        ///      修改数据状态
        /// </summary>
        [HttpPost]
        [Route("state/disable")]
        public ApiResponseMessage Disable()
        {
            InitForm();
            OnDisable();
            return IsFailed
                ? Request.ToResponse(ApiResult.Error(State, Message))
                : Request.ToResponse(ApiResult.Succees());
        }

        /// <summary>
        ///      修改数据状态
        /// </summary>
        [HttpPost]
        [Route("state/enable")]
        public ApiResponseMessage Enable()
        {
            InitForm();
            OnEnable();
            return IsFailed
                ? Request.ToResponse(ApiResult.Error(State, Message))
                : Request.ToResponse(ApiResult.Succees());
        }

        #endregion

        #region 操作

        /// <summary>
        ///     锁定对象
        /// </summary>
        protected virtual void OnLock()
        {
            foreach (var id in GetIntArrayArg("selects")) Business.Lock(id);
        }

        /// <summary>
        ///     恢复对象
        /// </summary>
        private void OnReset()
        {
            foreach (var id in GetIntArrayArg("selects")) Business.Reset(id);
        }

        /// <summary>
        ///     废弃对象
        /// </summary>
        private void OnDiscard()
        {
            foreach (var id in GetIntArrayArg("selects")) Business.Discard(id);
        }

        /// <summary>
        ///     启用对象
        /// </summary>
        private void OnEnable()
        {
            foreach (var id in GetIntArrayArg("selects")) Business.Enable(id);
        }

        /// <summary>
        ///     禁用对象
        /// </summary>
        private void OnDisable()
        {
            foreach (var id in GetIntArrayArg("selects")) Business.Disable(id);
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
                    lambda.AddRoot(p => p.DataState == (DataStateType) state);
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