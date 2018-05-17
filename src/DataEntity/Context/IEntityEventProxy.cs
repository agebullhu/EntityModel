namespace Gboxt.Common.DataModel
{
    /// <summary>
    /// 实体事件代理
    /// </summary>
    public interface IEntityEventProxy
    {
        /// <summary>
        /// 状态修改事件
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="entity">实体</param>
        /// <param name="type">状态类型</param>
        /// <param name="value">对应实体</param>
        void OnStatusChanged(string database, string entity, DataOperatorType type, string value);

    }
}