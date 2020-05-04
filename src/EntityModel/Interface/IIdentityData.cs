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
    ///     表示有唯一数字标识的数据
    /// </summary>
    public interface IIdentityData
    {
        /// <summary>
        ///     数字标识
        /// </summary>
        /// <value>int</value>
        long Id { get; set; }
    }

    /// <summary>
    ///     表示使用雪花码的非自增主键数据
    /// </summary>
    public interface ISnowFlakeId : IIdentityData
    {

    }
    /// <summary>
    ///     表示有标题的数据
    /// </summary>
    public interface ITitle
    {
        /// <summary>
        ///     标题
        /// </summary>
        /// <value>int</value>
        string Title { get; }
    }
    /// <summary>
    ///     表示有唯一数字标识的数据
    /// </summary>
    public interface IUnionUniqueEntity
    {
    }

}