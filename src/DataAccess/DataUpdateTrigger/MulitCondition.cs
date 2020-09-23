namespace Agebull.EntityModel.Events
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class MulitCondition
    {
        /// <summary>
        /// 条件
        /// </summary>
        public string Condition { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public ConditionParameter[] Parameters { get; set; }

    }

}