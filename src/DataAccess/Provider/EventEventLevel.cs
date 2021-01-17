namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 实体事件层级
    /// </summary>
    public enum EventEventLevel
    {
        /// <summary>
        /// 无事件
        /// </summary>
        None,
        /// <summary>
        /// 仅事件名称,无参数
        /// </summary>
        Simple,
        /// <summary>
        /// 事件名称+参数
        /// </summary>
        Details
    }
}