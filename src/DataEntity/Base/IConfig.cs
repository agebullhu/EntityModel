namespace Agebull.Common.DataModel
{
    /// <summary>
    /// 基本配置对象
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// 语言使用名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        string Caption { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        string Description { get; set; }
    }
}