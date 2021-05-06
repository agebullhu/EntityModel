/*****************************************************
(c)2016-2021 by ZeroTeam
作者: 胡天水
工程: Agebull.EntityModel.CoreAgebull.DataModel
建立:2016-06-07
修改: -
*****************************************************/

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 数据事件类型
    /// </summary>
    public enum DataEventType
    {
        /// <summary>
        /// 不确定,即可忽略的
        /// </summary>
        None = 0x0,
        /// <summary>
        /// 实体事件
        /// </summary>
        Entity = 0x1,
        /// <summary>
        /// 数据状态
        /// </summary>
        DataState = 0x22,
        /// <summary>
        /// 审核
        /// </summary>
        Audit = 0x4
    }
}