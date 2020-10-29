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
        /// 实体
        /// </summary>
        Entity,

        /// <summary>
        /// 自定义SQL操作
        /// </summary>
        CustomSQL
    }
}