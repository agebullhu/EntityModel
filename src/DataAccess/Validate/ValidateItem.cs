
namespace Agebull.EntityModel.Common
{

    /// <summary>
    /// 校验节点
    /// </summary>
    public class ValidateItem
    {
        /// <summary>
        /// 正确
        /// </summary>
        public bool Succeed { get; set; }

        /// <summary>
        /// 1警告
        /// </summary>
        public bool Warning { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字段标题目
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}