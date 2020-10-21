namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// �߼��ڲ���������
    /// </summary>
    public enum BusinessCommandType
    {
        /// <summary>
        /// ��ԭ���޸�״̬
        /// </summary>
        SetState,
        /// <summary>
        /// ����
        /// </summary>
        AddNew,
        /// <summary>
        /// ����
        /// </summary>
        Update,
        /// <summary>
        /// ����
        /// </summary>
        Enable,
        /// <summary>
        /// ����
        /// </summary>
        Disable,
        /// <summary>
        /// ����
        /// </summary>
        Discard,
        /// <summary>
        /// ɾ��
        /// </summary>
        Delete,
        /// <summary>
        /// ����
        /// </summary>
        Reset,
        /// <summary>
        /// ����
        /// </summary>
        Lock,
        /// <summary>
        /// ����
        /// </summary>
        Unlock,
        /// <summary>
        /// �ύ
        /// </summary>
        Submit,
        /// <summary>
        /// ����
        /// </summary>
        Pullback,
        /// <summary>
        /// ��
        /// </summary>
        Back,
        /// <summary>
        /// ͨ��
        /// </summary>
        Pass,
        /// <summary>
        /// ���
        /// </summary>
        Deny,
        /// <summary>
        /// ���±༭
        /// </summary>
        ReAudit,
        /// <summary>
        /// ����
        /// </summary>
        End,
        /// <summary>
        /// �Զ���
        /// </summary>
        Custom
    }
}