// ���ڹ��̣�Agebull.EntityModel
// �����û���bull2
// ����ʱ�䣺2012-08-13 5:35
// ����ʱ�䣺2012-08-30 3:12

#region

using System.ComponentModel ;


#endregion

namespace Agebull.Common
{
    /// <summary>
    ///   �ļ�����
    /// </summary>
    public enum FileType
    {
        /// <summary>
        ///   �����ļ�
        /// </summary>
        [Description("�����ļ�")]
        None ,

        /// <summary>
        ///   ͼ��
        /// </summary>
        [Description("ͼ��")]
        Image ,

        /// <summary>
        ///   ��Ƶ
        /// </summary>
        [Description("��Ƶ")]
        Audio ,

        /// <summary>
        ///   ��Ƶ
        /// </summary>
        [Description("��Ƶ")]
        Vedio ,

        /// <summary>
        ///   WORD�ĵ�
        /// </summary>
        [Description("WORD�ĵ�")]
        Doc ,

        /// <summary>
        ///   EXCEL�ĵ�
        /// </summary>
        [Description("EXCEL�ĵ�")]
        Xls ,

        /// <summary>
        ///   PPT�ĵ�
        /// </summary>
        [Description("PPT�ĵ�")]
        PPT ,

        /// <summary>
        ///   PDF�ĵ�
        /// </summary>
        [Description("PDF�ĵ�")]
        PDF,

        /// <summary>
        /// WPS�ļ�
        /// </summary>
        [Description("WPS�ļ�")]
        WPS,

        /// <summary>
        /// �ı��ļ�
        /// </summary>
        [Description("�ı��ļ�")]
        TEXT,

        /// <summary>
        /// ��ҳ
        /// </summary>
        [Description("��ҳ")]
        HTML,

        /// <summary>
        /// Ӧ�ó���
        /// </summary>
        [Description("Ӧ�ó���")]
        EXE,

        /// <summary>
        /// ѹ���ļ�
        /// </summary>
        [Description("ѹ���ļ�")]
        ZIP,

        /// <summary>
        /// �����ļ�
        /// </summary>
        [Description("�����ļ�")]
        CHM,

        /// <summary>
        /// �������
        /// </summary>
        [Description("�������")]
        CODE,

        /// <summary>
        /// �����ļ�
        /// </summary>
        [Description("�����ļ�")]
        Data
    }
}
