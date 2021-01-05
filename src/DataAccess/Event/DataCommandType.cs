namespace Agebull.EntityModel.Events
{
    /// <summary>
    /// �߼��ڲ���������
    /// </summary>
    public enum DataCommandType
    {
        /// <summary>
        /// ����
        /// </summary>
        AddNew=1,
        /// <summary>
        /// ����
        /// </summary>
        Update=2,
        /// <summary>
        /// ��ԭ���޸�״̬
        /// </summary>
        SetState = 0x100,
        /// <summary>
        /// ����
        /// </summary>
        Enable = 0x101,
        /// <summary>
        /// ����
        /// </summary>
        Disable = 0x102,
        /// <summary>
        /// ����
        /// </summary>
        Discard = 0x103,
        /// <summary>
        /// ɾ��
        /// </summary>
        Delete = 0x104,
        /// <summary>
        /// ����
        /// </summary>
        Reset = 0x105,
        /// <summary>
        /// ����
        /// </summary>
        Lock = 0x106,
        /// <summary>
        /// ����
        /// </summary>
        Unlock = 0x107,
        /// <summary>
        /// �ύ
        /// </summary>
        Submit = 0x200,
        /// <summary>
        /// ����
        /// </summary>
        Pullback = 0x201,
        /// <summary>
        /// ��
        /// </summary>
        Back = 0x202,
        /// <summary>
        /// ͨ��
        /// </summary>
        Pass = 0x203,
        /// <summary>
        /// ���
        /// </summary>
        Deny = 0x204,
        /// <summary>
        /// ���±༭
        /// </summary>
        ReAudit = 0x205,
        /// <summary>
        /// ����
        /// </summary>
        End = 0x206,
        /// <summary>
        /// �Զ���
        /// </summary>
        Custom = 0xFFFFFF
    }
}