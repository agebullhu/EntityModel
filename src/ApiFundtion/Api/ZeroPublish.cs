using System.Collections.Generic;
using System.Configuration;

namespace Agebull.Common.WebApi
{
    /// <summary>
    /// ��ZeroNet��ͨ��
    /// </summary>
    public class ZeroNetBridge
    {
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="station">վ��</param>
        /// <param name="title">��Ϣ������</param>
        /// <param name="sub">��Ϣ�ӱ���</param>
        /// <param name="arg">��Ϣ����(JSON)</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        public static bool Publish(string station,string title,string sub,string arg)
        {
            var caller = new WebApiCaller(ConfigurationManager.AppSettings["ZeroNetBridgeAddress"]);
            var result = caller.Post("publish", new Dictionary<string, string>
            {
                {"Host", "MachineEventMQ"},
                { "Title",title},
                {"Sub", sub},
                {"Arg", arg }
            });
            return result.Success;
        }
    }
}