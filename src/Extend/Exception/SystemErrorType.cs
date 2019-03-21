// ���ڹ��̣�Agebull.EntityModel
// �����û���bull2
// ����ʱ�䣺2012-08-13 5:35
// ����ʱ�䣺2012-08-30 3:12
namespace Agebull.Common
{
    /// <summary>
    ///   ϵͳ��������
    /// </summary>
    public enum SystemErrorType
    {
        /// <summary>
        ///   ����ϵͳ����
        /// </summary>
        OSError ,

        /// <summary>
        ///   ���ݿ����
        /// </summary>
        DataBaseError ,

        /// <summary>
        ///   �ڴ����
        /// </summary>
        MemoryError ,

        /// <summary>
        ///   ���̴���
        /// </summary>
        DiskError ,

        /// <summary>
        ///   �������
        /// </summary>
        NetError ,

        /// <summary>
        ///   δ֪����
        /// </summary>
        UnknowError
    }
}
