// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;

#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    ///     基于审核扩展的业务逻辑基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IBusinessLogicByAudit<TData, TPrimaryKey> : IBusinessLogicByStateData<TData, TPrimaryKey>
        where TData : EditDataObject, IIdentityData<TPrimaryKey>, IStateData, IAuditData, new()
    {
        #region 消息

        /// <summary>
        ///     取消校验(审核时有效)
        /// </summary>
        bool CancelValidate { get; set; }

        #endregion

        #region 批量操作

        /// <summary>
        ///     批量提交
        /// </summary>
        bool Submit(IEnumerable<TPrimaryKey> sels);

        /// <summary>
        ///     批量退回
        /// </summary>
        bool Back(IEnumerable<TPrimaryKey> sels);

        /// <summary>
        ///     批量通过
        /// </summary>
        bool AuditPass(IEnumerable<TPrimaryKey> sels);

        /// <summary>
        ///     批量拉回
        /// </summary>
        bool Pullback(IEnumerable<TPrimaryKey> sels);

        /// <summary>
        ///     批量否决
        /// </summary>
        bool AuditDeny(IEnumerable<TPrimaryKey> sels);

        /// <summary>
        ///     批量反审核
        /// </summary>
        bool UnAudit(IEnumerable<TPrimaryKey> sels);

        /// <summary>
        ///     批量数据校验
        /// </summary>
        bool Validate(IEnumerable<TPrimaryKey> sels, Action<ValidateResult> putError);

        #endregion

        #region 单个操作

        /// <summary>
        ///     数据正确性校验
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Validate(TPrimaryKey id);

        /// <summary>
        ///     审核通过
        /// </summary>
        bool AuditPass(TPrimaryKey id);

        /// <summary>
        ///     审核不通过
        /// </summary>
        bool AuditDeny(TPrimaryKey id);

        /// <summary>
        ///     反审核
        /// </summary>
        bool UnAudit(TPrimaryKey id);

        /// <summary>
        ///     提交审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Submit(TPrimaryKey id);

        /// <summary>
        ///     退回编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Back(TPrimaryKey id);

        #endregion
    }
}