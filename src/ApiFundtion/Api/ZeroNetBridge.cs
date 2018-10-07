using System.Collections.Generic;
using Agebull.Common.Configuration;
using Agebull.Common.Rpc;

namespace Agebull.Common.WebApi
{
    /// <summary>
    /// 与ZeroNet互通类
    /// </summary>
    internal class ZeroNetBridge : IZeroPublisher
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="station">站点</param>
        /// <param name="title">消息主标题</param>
        /// <param name="sub">消息子标题</param>
        /// <param name="arg">消息参数(JSON)</param>
        /// <returns>发布是否成功</returns>
        bool IZeroPublisher.Publish(string station,string title,string sub,string arg)
        {
            var caller = new WebApiCaller(ConfigurationManager.AppSettings["ZeroGratewayAddress"]);
            var result = caller.Post("publish", new Dictionary<string, string>
            {
                {"Host", station},
                { "Title",title},
                {"Sub", sub},
                {"Arg", arg }
            });
            return result.Success;
        }
    }
}