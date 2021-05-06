/*****************************************************
(c)2016-2021 by ZeroTeam
作者: 胡天水
工程: Agebull.EntityModel.CoreAgebull.DataModel
建立:2016-06-16
修改: -
*****************************************************/

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     表示这条数据支持数据状态
    /// </summary>
    public interface IStateData
    {
        /// <summary>
        ///     数据状态
        /// </summary>
        DataStateType DataState { get; set; }
    }
}