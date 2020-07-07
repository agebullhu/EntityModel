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

    /// <summary>
    ///     数据状态扩展类
    /// </summary>
    public static class DataStateHelper
    {
        /// <summary>
        ///     是否为删除数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>是否为删除数据</returns>
        public static bool IsDeleted(this IStateData data)
        {
            return data.DataState == DataStateType.Delete;
        }

        /// <summary>
        ///     数据状态枚举类型
        /// </summary>
        public static string ToCaption(this DataStateType value)
        {
            switch (value)
            {
                case DataStateType.None:
                    return "草稿";
                case DataStateType.Discard:
                    return "废弃";
                case DataStateType.State:
                    return "不正确的状态";
                case DataStateType.Orther:
                    return "其它人编辑中";
                case DataStateType.Lock:
                    return "锁定";
                case DataStateType.Delete:
                    return "删除";
                case DataStateType.Enable:
                    return "启用";
                case DataStateType.Disable:
                    return "禁用";
                default:
                    return "未知数据状态";
            }
        }
    }
}