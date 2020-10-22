using System;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 代码注入层级
    /// </summary>
    [Flags]
    public enum InjectionLevel
    {
        /// <summary>
        /// 不注入
        /// </summary>
        None = 0x0,
        /// <summary>
        /// 查询条件
        /// </summary>
        QueryCondition = 0x1,
        /// <summary>
        /// 插入字段注入
        /// </summary>
        InsertField = 0x2,
        /// <summary>
        /// 更新字段注入
        /// </summary>
        UpdateField = 0x4,
        /// <summary>
        /// 更新条件注入
        /// </summary>
        UpdateCondition = 0x8,
        /// <summary>
        /// 删除条件注入
        /// </summary>
        DeleteCondition = 0x10,
        /// <summary>
        /// 全量
        /// </summary>
        All = QueryCondition | InsertField | UpdateField | UpdateCondition | DeleteCondition,

        /// <summary>
        /// 限制查询与写入条件
        /// </summary>
        OnlyCondition = QueryCondition | UpdateCondition,

        /// <summary>
        /// 不限制查询与写入条件
        /// </summary>
        NotCondition = InsertField | UpdateField,

        /// <summary>
        /// 不限制写入条件
        /// </summary>
        NotUpdateCondition = QueryCondition | InsertField | UpdateField
    }
}