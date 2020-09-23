namespace Agebull.EntityModel.Events
{
    /// <summary>
    ///     数据操作状态
    /// </summary>
    public enum EntityEventValueType
    {
        /// <summary>
        ///     无
        /// </summary>
        None,

        /// <summary>
        ///     实例的Json数据
        /// </summary>
        EntityJson,

        /// <summary>
        ///     主键数据
        /// </summary>
        Key,

        /// <summary>
        ///     主键列表
        /// </summary>
        Keys,

        /// <summary>
        ///     查询条件
        /// </summary>
        QueryCondition
    }

}