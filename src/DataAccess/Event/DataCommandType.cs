namespace Agebull.EntityModel.Events
{
    /// <summary>
    /// 逻辑内部命令类型
    /// </summary>
    public enum DataCommandType
    {
        /// <summary>
        /// 新增
        /// </summary>
        AddNew=1,
        /// <summary>
        /// 更新
        /// </summary>
        Update=2,
        /// <summary>
        /// 无原因修改状态
        /// </summary>
        SetState = 0x100,
        /// <summary>
        /// 启用
        /// </summary>
        Enable = 0x101,
        /// <summary>
        /// 禁用
        /// </summary>
        Disable = 0x102,
        /// <summary>
        /// 废弃
        /// </summary>
        Discard = 0x103,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 0x104,
        /// <summary>
        /// 重置
        /// </summary>
        Reset = 0x105,
        /// <summary>
        /// 锁定
        /// </summary>
        Lock = 0x106,
        /// <summary>
        /// 解锁
        /// </summary>
        Unlock = 0x107,
        /// <summary>
        /// 提交
        /// </summary>
        Submit = 0x200,
        /// <summary>
        /// 拉回
        /// </summary>
        Pullback = 0x201,
        /// <summary>
        /// 退
        /// </summary>
        Back = 0x202,
        /// <summary>
        /// 通过
        /// </summary>
        Pass = 0x203,
        /// <summary>
        /// 否决
        /// </summary>
        Deny = 0x204,
        /// <summary>
        /// 重新编辑
        /// </summary>
        ReAudit = 0x205,
        /// <summary>
        /// 结束
        /// </summary>
        End = 0x206,
        /// <summary>
        /// 自定义
        /// </summary>
        Custom = 0xFFFFFF
    }
}