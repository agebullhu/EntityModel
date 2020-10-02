// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-16
// // 修改:2016-06-16
// // *****************************************************/

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

        /// <summary>
        ///     数据是否已冻结，如果是，则为只读数据
        /// </summary>
        /// <value>bool</value>
        bool IsFreeze { get; set; }
    }

}