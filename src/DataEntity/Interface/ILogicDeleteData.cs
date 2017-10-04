// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     表示这条数据支持逻辑删除
    /// </summary>
    public interface ILogicDeleteData
    {
        /// <summary>
        ///     是否已逻辑删除
        /// </summary>
        bool IsDelete { get; set; }
    }
}