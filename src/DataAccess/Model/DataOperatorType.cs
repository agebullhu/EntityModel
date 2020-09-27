// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     数据操作状态
    /// </summary>
    public enum DataOperatorType
    {
        /// <summary>
        ///     未知
        /// </summary>
        None,

        /// <summary>
        ///     新增
        /// </summary>
        Insert,

        /// <summary>
        ///     更新
        /// </summary>
        Update,

        /// <summary>
        ///     删除
        /// </summary>
        Delete,

        /// <summary>
        ///     批量更新
        /// </summary>
        MulitUpdate,

        /// <summary>
        ///     批量删除
        /// </summary>
        MulitDelete
    }
}