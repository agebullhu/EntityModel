
using Agebull.Common.OAuth;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// �ȼ�Ȩ�޹�������
    /// </summary>
    /// <remark>
    /// �ȼ�Ȩ�޹�������
    /// </remark>
    public enum SubjectionType
    {
        /// <summary>
        /// û���κ�Ȩ����
        /// </summary>
        None = 0x0,
        /// <summary>
        /// ���ޱ��˵�����
        /// </summary>
        Self = 0x1,
        /// <summary>
        /// �����ŵ�����
        /// </summary>
        Department = 0x2,
        /// <summary>
        /// �����ż��¼�������
        /// </summary>
        DepartmentAndLower = 0x3,
        /// <summary>
        /// �����������
        /// </summary>
        Company = 0x4,
        /// <summary>
        /// �������¼������벿�ŵ�����
        /// </summary>
        CompanyAndLower = 0x5,
        /// <summary>
        /// �Զ���
        /// </summary>
        Custom = 0x6,
    }
    /// <summary>
    /// ö����չ
    /// </summary>
    public static class AuthEnumHelper
    {
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

        /// <summary>
        ///     �ȼ�Ȩ�޹�����������ת��
        /// </summary>
        public static string ToCaption(this SubjectionType value)
        {
            switch (value)
            {
                case SubjectionType.None:
                    return "û���κ�Ȩ����";
                case SubjectionType.Self:
                    return "���ޱ��˵�����";
                case SubjectionType.Department:
                    return "�����ŵ�����";
                case SubjectionType.DepartmentAndLower:
                    return "�����ż��¼�������";
                case SubjectionType.Company:
                    return "�����������";
                case SubjectionType.CompanyAndLower:
                    return "�������¼������벿�ŵ�����";
                case SubjectionType.Custom:
                    return "�Զ���";
                default:
                    return "�ȼ�Ȩ�޹�������(δ֪)";
            }
        }

    }
}