namespace Agebull.Common.AppManage
{
    /// <summary>
    /// ҳ��ڵ�����
    /// </summary>
    public enum PageItemType
    {
        /// <summary>
        /// ����
        /// </summary>
        Root,
        /// <summary>
        /// �ļ���
        /// </summary>
        Folder,
        /// <summary>
        /// ҳ��
        /// </summary>
        Page,
        /// <summary>
        /// ��ť
        /// </summary>
        Button,
        /// <summary>
        /// ����
        /// </summary>
        Action
    }
    /// <summary>
    /// ҳ��ڵ�����
    /// </summary>
    public static class PageItemTypeEx
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
    }
}