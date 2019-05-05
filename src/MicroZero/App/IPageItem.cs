// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

namespace Agebull.Common.AppManage
{
    /// <summary>
    ///     ��ʾҳ��ڵ�
    /// </summary>
    public interface IPageItem
    {
        /// <summary>
        ///     �����ʶ
        /// </summary>
        long Id { get; set; }
        
        /// <summary>
        ///     ���
        /// </summary>
        /// <remarks>
        ///     ���
        /// </remarks>
        int Index { get; set; }

        /// <summary>
        /// �ڵ�����
        /// </summary>
        PageItemType ItemType { get; set; }

        /// <summary>
        ///     Ŀ¼
        /// </summary>
        /// <remarks>
        ///     Ŀ¼
        /// </remarks>
        string Path { get; set; }

        /// <summary>
        ///     ͼ��
        /// </summary>
        /// <remarks>
        ///     ͼ��
        /// </remarks>
        string Icon { get; set; }

        /// <summary>
        ///     ����
        /// </summary>
        /// <remarks>
        ///     ����
        /// </remarks>
        string Name { get; set; }

        /// <summary>
        ///     ����
        /// </summary>
        /// <remarks>
        ///     ����
        /// </remarks>
        string Caption { get; set; }

        /// <summary>
        ///     ҳ������
        /// </summary>
        /// <remarks>
        ///     ҳ������
        /// </remarks>
        string Url { get; set; }

        /// <summary>
        ///     ��ע
        /// </summary>
        /// <remarks>
        ///     ��ע
        /// </remarks>
        string Memo { get; set; }

        /// <summary>
        ///     �ϼ��ڵ�
        /// </summary>
        /// <remarks>
        ///     �ϼ��ڵ�
        /// </remarks>
        long ParentId { get; set; }

        /// <summary>
        ///     ����ʾ
        /// </summary>
        bool IsHide { get; set; }

        /// <summary>
        ///     ��Ȩ�޿��ƵĹ���ҳ��
        /// </summary>
        bool IsPublic { get; set; }
    }
}