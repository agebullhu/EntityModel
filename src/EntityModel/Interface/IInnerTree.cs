namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     表示这条数据支持内联层级
    /// </summary>
    public interface IInnerTree<TPrimaryKey>
    {
        /// <summary>
        ///     上级
        /// </summary>
        /// <value>string</value>
        TPrimaryKey ParentId { get; set; }

    }

    /// <summary>
    ///     表示这条数据支持内联层级
    /// </summary>
    public interface IInnerTree : IInnerTree<long>
    {

    }
}