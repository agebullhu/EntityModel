// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System.Collections.Generic;
using Agebull.EntityModel.Common;

#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// 基于数据状态的业务逻辑基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    public interface IBusinessLogicByStateData<TData>
        : IUiBusinessLogicBase<TData>
        where TData : EditDataObject, IIdentityData,IStateData, new()
    {
        #region 批量操作


        /// <summary>
        ///     启用对象
        /// </summary>
        bool Enable(IEnumerable<long> sels);

        /// <summary>
        ///     禁用对象
        /// </summary>
        bool Disable(IEnumerable<long> sels);

        /// <summary>
        ///     禁用对象
        /// </summary>
        bool Lock(IEnumerable<long> sels);
        #endregion

        #region 状态处理

        /// <summary>
        ///     重置数据状态
        /// </summary>
        /// <param name="data"></param>
        bool ResetState(TData data);


        /// <summary>
        ///     重置数据状态
        /// </summary>
        /// <param name="id"></param>
        bool Reset(long id);

        /// <summary>
        ///     启用对象
        /// </summary>
        bool Enable(long id);

        /// <summary>
        ///     禁用对象
        /// </summary>
        bool Disable(long id);

        /// <summary>
        ///     弃用对象
        /// </summary>
        bool Discard(long id);

        /// <summary>
        ///     锁定对象
        /// </summary>
        bool Lock(long id);

        #endregion
    }
}