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
    ///     表示可以提供SQL的查询条件
    /// </summary>
    public interface ISqlCondition
    {
        /// <summary>
        ///     取得SQL的查询条件
        /// </summary>
        string GetSqlCondition();
    }
}