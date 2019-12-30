
using Agebull.Common.AppManage;

namespace Agebull.Common.OAuth
{
    /// <summary>
    /// Ȩ�޷�Χö������
    /// </summary>
    /// <remark>
    /// Ȩ�޷�Χ
    /// </remark>
    public enum DataScopeType
    {
        /// <summary>
        /// ��
        /// </summary>
        None = 0x0,
        /// <summary>
        /// ����
        /// </summary>
        Person = 0x1,
        /// <summary>
        /// ����
        /// </summary>
        Home = 0x2,
        /// <summary>
        /// ���˼�����
        /// </summary>
        PersonAndHome = 0x3,
        /// <summary>
        /// �¼�
        /// </summary>
        Lower = 0x4,
        /// <summary>
        /// ����������
        /// </summary>
        HomeAndLower = 0x6,
        /// <summary>
        /// ���˱������¼�
        /// </summary>
        Full = 0x7,
        /// <summary>
        /// ������
        /// </summary>
        Unlimited = 0x8,
    }
    /// <summary>
    /// ��ɫȨ������
    /// </summary>
    public enum RolePowerType
    {
        /// <summary>
        /// δ����
        /// </summary>
        None,
        /// <summary>
        /// ����
        /// </summary>
        Allow,
        /// <summary>
        /// �ܾ�
        /// </summary>
        Deny
    }
    /// <summary>
    ///     ����״̬
    /// </summary>
    public enum UserStateType
    {
        /// <summary>
        ///     �ݸ�
        /// </summary>
        None = 0,

        /// <summary>
        ///     ����
        /// </summary>
        Enable = 1,

        /// <summary>
        ///     ����
        /// </summary>
        Disable = 2,

        /// <summary>
        ///     ����
        /// </summary>
        Discard = 0x10,

        /// <summary>
        ///     ɾ��
        /// </summary>
        Delete = 0xFF
    }
    /// <summary>
    /// ö����չ
    /// </summary>
    public static class AuthEnumHelper
    {
        /// <summary>
        ///     �ڵ�����ö����������ת��
        /// </summary>
        public static string ToCaption(this PageItemType value)
        {
            switch (value)
            {
                case PageItemType.Root:
                    return "����";
                case PageItemType.Folder:
                    return "�ļ���";
                case PageItemType.Page:
                    return "ҳ��";
                case PageItemType.Button:
                    return "��ť";
                case PageItemType.Action:
                    return "����";
                default:
                    return "�ڵ�����ö������(δ֪)";
            }
        }
        /// <summary>
        ///     Ȩ�޷�Χö����������ת��
        /// </summary>
        public static string ToCaption(this DataScopeType value)
        {
            switch (value)
            {
                case DataScopeType.None:
                    return "��";
                case DataScopeType.Person:
                    return "����";
                case DataScopeType.Home:
                    return "����";
                case DataScopeType.PersonAndHome:
                    return "���˼�����";
                case DataScopeType.Lower:
                    return "�¼�";
                case DataScopeType.HomeAndLower:
                    return "����������";
                case DataScopeType.Full:
                    return "���˱������¼�";
                case DataScopeType.Unlimited:
                    return "������";
                default:
                    return "Ȩ�޷�Χö������(����)";
            }
        }

        /// <summary>
        ///     Ȩ��ö����������ת��
        /// </summary>
        public static string ToCaption(this RolePowerType value)
        {
            switch (value)
            {
                case RolePowerType.None:
                    return "δ����";
                case RolePowerType.Allow:
                    return "����";
                case RolePowerType.Deny:
                    return "�ܾ�";
                default:
                    return "Ȩ��ö������(δ֪)";
            }
        }

    }
}