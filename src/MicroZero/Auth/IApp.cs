namespace Agebull.Common.OAuth
{
    /// <summary>
    /// 角色节点
    /// </summary>
    public interface IApp
    {
        /// <summary>
        ///     用户角色标识
        /// </summary>
        string AppId { get; set; }

        /// <summary>
        ///     登录设备的应用
        /// </summary>
        string AppName { get; set; }

        /// <summary>
        ///     登录设备的应用
        /// </summary>
        string AppKey { get; set; }
    }
}