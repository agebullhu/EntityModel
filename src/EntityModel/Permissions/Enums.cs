namespace Agebull.EntityModel.Permissions
{
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