using System.Collections.Generic;
using Agebull.Common.Configuration;
using Agebull.Common.Rpc;

namespace Agebull.Common.WebApi
{
    /// <summary>
    /// ��ZeroNet��ͨ��
    /// </summary>
    internal class ZeroNetBridge : IZeroPublisher
    {
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="station">վ��</param>
        /// <param name="title">��Ϣ������</param>
        /// <param name="sub">��Ϣ�ӱ���</param>
        /// <param name="arg">��Ϣ����(JSON)</param>
        /// <returns>�����Ƿ�ɹ�</returns>
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