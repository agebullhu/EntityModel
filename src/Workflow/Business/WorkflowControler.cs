using System;
using Gboxt.Common.DataModel;

namespace Gboxt.Common.Workflow
{
    /// <summary>
    /// 项目管理工作流控制器
    /// </summary>
    public class WorkflowControler
    {
        /// <summary>
        /// 流程检查
        /// </summary>
        public static Action<IAuditData> CheckWordflow;
        /// <summary>
        /// 工作UI信息设置
        /// </summary>
        public static Action<UserJobData> SetJobUiInfomation;
        /// <summary>
        /// 数据载入
        /// </summary>
        public static Func<int, int, IWorkflowData> LoadData;
    }
}

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     表示工作流的数据
    /// </summary>
    public interface IWorkflowData : ITitle, IIdentityData, IStateData, IHistoryData, IAuditData
    {
    }
}