// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Common;

#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// 基于数据状态的业务逻辑基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IBusinessLogicByStateData<TData, TPrimaryKey> : IUiBusinessLogicBase<TData, TPrimaryKey>
        where TData : EditDataObject, IIdentityData<TPrimaryKey>, IStateData, new()
    {
        #region 状态处理

        /// <summary>
        ///     重置数据状态
        /// </summary>
        /// <param name="id"></param>
        bool Reset(TPrimaryKey id);

        /// <summary>
        ///     启用对象
        /// </summary>
        bool Enable(TPrimaryKey id);

        /// <summary>
        ///     禁用对象
        /// </summary>
        bool Disable(TPrimaryKey id);

        /// <summary>
        ///     弃用对象
        /// </summary>
        bool Discard(TPrimaryKey id);

        #endregion
    }
}