// /*****************************************************
// (c)2008-2013 Copy right www.Agebull.com
// ����:bull2
// ����:CodeRefactor-Agebull.Common.WpfMvvmBase
// ����:2014-11-29
// �޸�:2014-11-29
// *****************************************************/

namespace Agebull.Common.OAuth
{
    /// <summary>
    /// ��ʾһ����ɫ����
    /// </summary>
    public interface IRoleData
    {
        /// <summary>
        /// ��ɫ��ʶ
        /// </summary>
        /// <remarks>
        /// ��ɫ��ʶ
        /// </remarks>
        long Id { get; set; }

        /// <summary>
        /// ��ɫ����
        /// </summary>
        /// <remarks>
        /// ��ɫ����
        /// </remarks>
        string Role { get; set; }

        /// <summary>
        /// ��ɫ��ʾ���ı�
        /// </summary>
        /// <remarks>
        /// ��ɫ��ʾ���ı�
        /// </remarks>
        string Caption { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        /// <remarks>
        /// ��ע
        /// </remarks>
        string Memo { get; set; }
    }
}