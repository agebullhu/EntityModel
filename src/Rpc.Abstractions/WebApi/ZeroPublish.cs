using Agebull.Common.Ioc;

namespace Agebull.Common.Rpc
{
    /// <summary>
    /// ��Ϣ���з���
    /// </summary>
    public class MessageQueue
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
            return IocHelper.Create<IZeroPublisher>().Publish(station,title,sub,arg);
        }
    }
}