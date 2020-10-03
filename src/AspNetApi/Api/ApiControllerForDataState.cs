// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.BusinessLogic;
using Agebull.EntityModel.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace ZeroTeam.AspNet.ModelApi
{
    /// <summary>
    ///     支持数据状态的启用禁用方法的页面的基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <typeparam name="TBusinessLogic">业务对象</typeparam>
    public abstract class ApiControllerForDataState<TData, TPrimaryKey, TBusinessLogic> :
        ApiController<TData, TPrimaryKey, TBusinessLogic>
        where TData : class, IStateData, IIdentityData<TPrimaryKey>, new()
        where TBusinessLogic : BusinessLogicByStateData<TData, TPrimaryKey>, new()
    {
        #region API

        /// <summary>
        ///     重置数据状态
        /// </summary>
        [Route("state/reset")]
        public async Task<ApiResult> Reset(List<TPrimaryKey> selects)
        {
            return !await Business.LoopIds(selects, Business.Reset) || IsFailed
                    ? ApiResultHelper.FromContext(Business.Context)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     废弃数据
        /// </summary>
        [Route("state/discard")]
        public async Task<ApiResult> Discard(List<TPrimaryKey> selects)
        {
            return !await Business.LoopIds(selects, Business.Discard) || IsFailed
                    ? ApiResultHelper.FromContext(Business.Context)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     禁用数据
        /// </summary>
        [Route("state/disable")]
        public async Task<ApiResult> Disable(List<TPrimaryKey> selects)
        {
            return !await Business.LoopIds(selects, Business.Disable) || IsFailed
                    ? ApiResultHelper.FromContext(Business.Context)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     启用数据
        /// </summary>
        [Route("state/enable")]
        public async Task<ApiResult> Enable(List<TPrimaryKey> selects)
        {
            return !await Business.LoopIds(selects, Business.Enable) || IsFailed
                    ? ApiResultHelper.FromContext(Business.Context)
                    : ApiResultHelper.Succees();
        }


        #endregion

        #region 列表数据

        /// <summary>
        ///     读取查询条件
        /// </summary>
        /// <param name="filter">筛选器</param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        protected override void GetQueryFilter(LambdaItem<TData> filter, string field, string value)
        {
            if (field != "_state_" || !int.TryParse(value, out int state) || state < 0 || state >= 0x100)
            {
                base.GetQueryFilter(filter, field, value);
                return;
            }
            filter.AddRoot(p => p.DataState == (DataStateType)state);
        }
        #endregion
    }
}