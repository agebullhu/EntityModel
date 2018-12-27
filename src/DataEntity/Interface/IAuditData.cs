// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;

#endregion

namespace Agebull.Common.DataModel
{
    /// <summary>
    ///     ��ʾ��������֧�����
    /// </summary>
    public interface IAuditData
    {
        /// <summary>
        ///     ���״̬
        /// </summary>
        /// <value>AuditStateType</value>
        AuditStateType AuditState { get; set; }

        /// <summary>
        ///     �����
        /// </summary>
        /// <value>string</value>
        long AuditorId { get; set; }


        /// <summary>
        ///     �������
        /// </summary>
        /// <value>DateTime</value>
        DateTime AuditDate { get; set; }

        ///// <summary>
        /////     �����(�����ڽ���)
        ///// </summary>
        ///// <value>string</value>
        //string ToUsers { get; set; }

        ///// <summary>
        /////     �ܷ����(�����ڽ���)
        ///// </summary>
        ///// <value>bool</value>
        //bool CanAudit { get; set; }
    }

    /// <summary>
    ///     ���״̬����
    /// </summary>
    public enum AuditStateType
    {
        /// <summary>
        ///     �ݸ�
        /// </summary>
        None,

        /// <summary>
        ///     �����
        /// </summary>
        Again,

        /// <summary>
        ///     �ύ���
        /// </summary>
        Submit,

        /// <summary>
        ///     ��˲�ͨ��
        /// </summary>
        Deny,

        /// <summary>
        ///     ���ͨ��
        /// </summary>
        Pass,

        /// <summary>
        ///     ����
        /// </summary>
        End,
        /// <summary>
        /// ����ȷ��״̬
        /// </summary>
        Error
    }
    /// <summary>
    /// ���״̬��չ��
    /// </summary>
    public static class AuditStateTypeHelper
    {
        /// <summary>
        ///     ����������
        /// </summary>
        /// <param name="state">���״̬����</param>
        /// <returns>��������</returns>
        public static string ToCaption(this AuditStateType state)
        {
            switch (state)
            {
                case AuditStateType.Submit:
                    return "�ύ���";
                case AuditStateType.Deny:
                    return "�ѷ��";
                case AuditStateType.Pass:
                    return "��ͨ��";
                case AuditStateType.Again:
                    return "�����";
                case AuditStateType.End:
                    return "����";
                case AuditStateType.None:
                    return "�ݸ�";
                default:
                    return "����";
            }
        }
        /// <summary>
        ///     ����������
        /// </summary>
        /// <param name="state">���״̬����</param>
        /// <returns>��������</returns>
        public static string ToChiness(this AuditStateType state)
        {
            switch (state)
            {
                case AuditStateType.Submit:
                    return "�ύ���";
                case AuditStateType.Deny:
                    return "�ѷ��";
                case AuditStateType.Pass:
                    return "��ͨ��";
                case AuditStateType.Again:
                    return "�����";
                case AuditStateType.End:
                    return "����";
                case AuditStateType.None:
                    return "�ݸ�";
                default:
                    return "����";
            }
        }

        /// <summary>
        ///     �Ƿ�������
        /// </summary>
        public static bool CanAudit(this IAuditData data)
        {
            switch (data.AuditState)
            {
                case AuditStateType.None:
                case AuditStateType.Again:
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     �Ƿ���Է����
        /// </summary>
        public static bool CanAgainAudit(this IAuditData data)
        {
            switch (data.AuditState)
            {
                case AuditStateType.Pass:
                    return true;
            }
            return false;
        }
    }
}