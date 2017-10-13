namespace Gboxt.Common.Workflow
{
    /// <summary>
    /// 用户任务分类枚举类型
    /// </summary>
    /// <remark>
    /// 用户任务分类枚举类型
    /// </remark>
    public enum UserJobType
    {
        /// <summary>
        /// 未指定
        /// </summary>
        None = 0x0,
        /// <summary>
        /// 编辑任务
        /// </summary>
        Edit = 0x1,
        /// <summary>
        /// 审核任务
        /// </summary>
        Audit = 0x2,
        /// <summary>
        /// 消息通知
        /// </summary>
        Message = 0x3,
        /// <summary>
        /// 数据维护
        /// </summary>
        Data = 0x4,
        /// <summary>
        /// 其它命令
        /// </summary>
        Command = 0x5
    }

    /// <summary>
    /// 工作内容状态枚举类型
    /// </summary>
    /// <remark>
    /// 工作内容状态
    /// </remark>
    public enum JobStatusType
    {
        /// <summary>
        /// 未开始
        /// </summary>
        None = 0x0,
        /// <summary>
        /// 转发
        /// </summary>
        Trans = 0x1,
        /// <summary>
        /// 通知
        /// </summary>
        Notify = 0x2,
        /// <summary>
        /// 完成
        /// </summary>
        Succeed = 0x10,
        /// <summary>
        /// 取消
        /// </summary>
        Canceled = 0x11,
        /// <summary>
        /// 未命中
        /// </summary>
        NoHit = 0x12,
    }

    /// <summary>
    /// 命令类型枚举类型
    /// </summary>
    /// <remark>
    /// 命令类型
    /// </remark>
    public enum JobCommandType
    {
        /// <summary>
        /// 未指定
        /// </summary>
        None = 0x0,
        /// <summary>
        /// 新增
        /// </summary>
        AddNew = 0x1,
        /// <summary>
        /// 还原
        /// </summary>
        Reset = 0xA,
        /// <summary>
        /// 审批
        /// </summary>
        Audit = 0xB,
        /// <summary>
        /// 退回
        /// </summary>
        Back = 0xC,
        /// <summary>
        /// 通过
        /// </summary>
        Pass = 0xD,
        /// <summary>
        /// 否决
        /// </summary>
        Deny = 0xE,
        /// <summary>
        /// 归档
        /// </summary>
        Lock = 0xF,
        /// <summary>
        /// 其它
        /// </summary>
        Orther = 0x10,
        /// <summary>
        /// 编辑
        /// </summary>
        Edit = 0x2,
        /// <summary>
        /// 阅读
        /// </summary>
        Read = 0x3,
        /// <summary>
        /// 校验
        /// </summary>
        Validate = 0x4,
        /// <summary>
        /// 提交
        /// </summary>
        Submit = 0x5,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 0x6,
        /// <summary>
        /// 启用
        /// </summary>
        Enable = 0x7,
        /// <summary>
        /// 禁用
        /// </summary>
        Disable = 0x8,
        /// <summary>
        /// 废弃
        /// </summary>
        Discard = 0x9,
    }

    public static class EnumHelperWorkflow
    {

        /// <summary>
        ///     用户任务分类枚举类型名称转换
        /// </summary>
        public static string ToCaption(this UserJobType value)
        {
            switch (value)
            {
                case UserJobType.None:
                    return "未指定";
                case UserJobType.Edit:
                    return "编辑任务";
                case UserJobType.Audit:
                    return "审核任务";
                case UserJobType.Message:
                    return "消息通知";
                case UserJobType.Data:
                    return "数据维护";
                case UserJobType.Command:
                    return "其它命令";
                default:
                    return "用户任务分类枚举类型(未知)";
            }
        }

        /// <summary>
        ///     工作内容状态枚举类型名称转换
        /// </summary>
        public static string ToCaption(this JobStatusType value)
        {
            switch (value)
            {
                case JobStatusType.None:
                    return "未开始";
                case JobStatusType.Trans:
                    return "转发";
                case JobStatusType.Succeed:
                    return "完成";
                case JobStatusType.Canceled:
                    return "取消";
                case JobStatusType.NoHit:
                    return "未命中";
                case JobStatusType.Notify:
                    return "通知";
                default:
                    return "工作内容状态枚举类型(未知)";
            }
        }

        /// <summary>
        ///     命令类型枚举类型名称转换
        /// </summary>
        public static string ToCaption(this JobCommandType value)
        {
            switch (value)
            {
                case JobCommandType.None:
                    return "未指定";
                case JobCommandType.AddNew:
                    return "新增";
                case JobCommandType.Reset:
                    return "还原";
                case JobCommandType.Audit:
                    return "审批";
                case JobCommandType.Back:
                    return "退回";
                case JobCommandType.Pass:
                    return "通过";
                case JobCommandType.Deny:
                    return "否决";
                case JobCommandType.Lock:
                    return "归档";
                case JobCommandType.Orther:
                    return "其它";
                case JobCommandType.Edit:
                    return "编辑";
                case JobCommandType.Read:
                    return "阅读";
                case JobCommandType.Validate:
                    return "校验";
                case JobCommandType.Submit:
                    return "提交";
                case JobCommandType.Delete:
                    return "删除";
                case JobCommandType.Enable:
                    return "启用";
                case JobCommandType.Disable:
                    return "禁用";
                case JobCommandType.Discard:
                    return "废弃";
                default:
                    return "命令类型枚举类型(未知)";
            }
        }

    }
}
