namespace Agebull.Common.DataModel
{
    /// <summary>
    /// ȫ�ֶ������
    /// </summary>
    public abstract class GlobalBase
    {
        /// <summary>
        /// ��ʼ
        /// </summary>
        public abstract void Initialize();


        /// <summary>
        /// ���»���
        /// </summary>
        public abstract void FlushCache();

        /// <summary>
        /// ����
        /// </summary>
        public abstract void Dispose();
    }
}