namespace Zeroteam.MessageMVC.EventBus
{


    /// <summary>
    /// 事件类型类型
    /// </summary>
    /// <remark>
    /// 事件类型类型
    /// </remark>
    public enum EventType
    {
        /// <summary>
        /// 广播
        /// </summary>
        Broadcast = 0x0,
        /// <summary>
        /// 日志
        /// </summary>
        Log = 0x1,
        /// <summary>
        /// 顺序聚合
        /// </summary>
        SequenceAggregation = 0x2,
        /// <summary>
        /// 并行聚合
        /// </summary>
        ParallelAggregation = 0x3,
    }


    /// <summary>
    /// 事件区域类型
    /// </summary>
    /// <remark>
    /// 事件区域类型
    /// </remark>
    public enum RegionType
    {
        /// <summary>
        /// 系统事件
        /// </summary>
        SystemEvent = 0x0,
        /// <summary>
        /// 数据事件
        /// </summary>
        DataEvent = 0x1,
        /// <summary>
        /// 业务事件
        /// </summary>
        BusinessEvent = 0x2,
        /// <summary>
        /// 流程事件
        /// </summary>
        FlowEvent = 0x3,
    }


    /// <summary>
    /// 处理结果设置类型
    /// </summary>
    /// <remark>
    /// 处理结果设置类型
    /// </remark>
    public enum ResultOptionType
    {
        /// <summary>
        /// 不需要
        /// </summary>
        None = 0x0,
        /// <summary>
        /// 记录结果
        /// </summary>
        Result = 0x1,
        /// <summary>
        /// 背包传递
        /// </summary>
        BagPack = 0x2,
    }


    /// <summary>
    /// 判断成功配置类型
    /// </summary>
    /// <remark>
    /// 判断成功配置类型
    /// </remark>
    public enum SuccessOptionType
    {
        /// <summary>
        /// 不判断
        /// </summary>
        None = 0x0,
        /// <summary>
        /// 部分成功
        /// </summary>
        Once = 0x1,
        /// <summary>
        /// 所有成功
        /// </summary>
        All = 0x2,
    }

}
