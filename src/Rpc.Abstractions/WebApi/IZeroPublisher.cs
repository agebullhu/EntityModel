namespace Agebull.Common.Rpc
{
    /// <summary>
    /// ZeroNet消息队列发布类
    /// </summary>
    public interface IZeroPublisher
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="station">站点</param>
        /// <param name="title">消息主标题</param>
        /// <param name="sub">消息子标题</param>
        /// <param name="arg">消息参数(JSON)</param>
        /// <returns>发布是否成功</returns>
        bool Publish(string station, string title, string sub, string arg);
    }
}