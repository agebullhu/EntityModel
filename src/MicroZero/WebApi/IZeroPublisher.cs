namespace Agebull.MicroZero
{
    /// <summary>
    /// ZeroNet��Ϣ���з�����
    /// </summary>
    public interface IZeroPublisher
    {
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="station">վ��</param>
        /// <param name="title">��Ϣ������</param>
        /// <param name="sub">��Ϣ�ӱ���</param>
        /// <param name="arg">��Ϣ����(JSON)</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        bool Publish(string station, string title, string sub, string arg);
    }
}