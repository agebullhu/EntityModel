// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using Gboxt.Common.DataModel.Extends;

#endregion

namespace Gboxt.Common.DataModel.BusinessLogic
{
    /// <summary>
    /// 基于历史记录的业务逻辑基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TAccess">数据访问对象</typeparam>
    public class BusinessLogicByHistory<TData, TAccess> : BusinessLogicByStateData<TData, TAccess>
        where TData : EditDataObject, IIdentityData, IHistoryData,  IStateData, new()
        where TAccess : class, IDataTable<TData>, new()
    {
        /// <summary>
        ///     重置数据状态
        /// </summary>
        /// <param name="data"></param>
        protected override bool DoResetState(TData data)
        {
            if (data == null)
                return false;
            data.AddDate = DateTime.MinValue;
            data.AuthorId = 0;
            data.LastModifyDate = DateTime.MinValue;
            data.LastReviserId = 0;
            return base.DoResetState(data);
        }
    }
}