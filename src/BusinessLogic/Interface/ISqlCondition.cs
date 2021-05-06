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