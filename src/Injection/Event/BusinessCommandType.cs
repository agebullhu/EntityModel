namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// 逻辑内部命令类型
    /// </summary>
    public enum BusinessCommandType
    {
        /// <summary>
        /// 无原因修改状态
        /// </summary>
        SetState,
        /// <summary>
        /// 新增
        /// </summary>
        AddNew,
        /// <summary>
        /// 更新
        /// </summary>
        Update,
        /// <summary>
        /// 启用
        /// </summary>
        Enable,
        /// <summary>
        /// 禁用
        /// </summary>
        Disable,
        /// <summary>
        /// 废弃
        /// </summary>
        Discard,
        /// <summary>
        /// 删除
        /// </summary>
        Delete,
        /// <summary>
        /// 重置
        /// </summary>
        Reset,
        /// <summary>
        /// 锁定
        /// </summary>
        Lock,
        /// <summary>
        /// 解锁
        /// </summary>
        Unlock,
        /// <summary>
        /// 提交
        /// </summary>
        Submit,
        /// <summary>
        /// 拉回
        /// </summary>
        Pullback,
        /// <summary>
        /// 退
        /// </summary>
        Back,
        /// <summary>
        /// 通过
        /// </summary>
        Pass,
        /// <summary>
        /// 否决
        /// </summary>
        Deny,
        /// <summary>
        /// 重新编辑
        /// </summary>
        ReAudit,
        /// <summary>
        /// 结束
        /// </summary>
        End,
        /// <summary>
        /// 自定义
        /// </summary>
        Custom
    }
}