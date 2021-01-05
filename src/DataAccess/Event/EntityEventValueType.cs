namespace Agebull.EntityModel.Events
{
    /// <summary>
    ///     数据事件内容类型
    /// </summary>
    public enum EntityEventValueType
    {
        /// <summary>
        ///     无
        /// </summary>
        None,

        /// <summary>
        /// 主键
        /// </summary>
        Id,

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