// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

namespace Gboxt.Common.SystemModel
{
    /// <summary>
    ///     ��ɫȨ��
    /// </summary>
    public interface IRolePower
    {
        /// <summary>
        ///     �����ʶ
        /// </summary>
        int Id { get; set; }

        /// <summary>
        ///     ��ɫ��ʶ
        /// </summary>
        /// <remarks>
        ///     ��ɫ��ʶ
        /// </remarks>
        int RoleId { get; set; }

        /// <summary>
        ///     ҳ��ڵ��ʶ
        /// </summary>
        /// <remarks>
        ///     ҳ��ڵ��ʶ
        /// </remarks>
        int PageItemId { get; set; }

        /// <summary>
        ///     Ȩ��
        /// </summary>
        /// <remarks>
        ///     Ȩ��,0��ʾδ����,1��ʾ����,2��ʾ�ܾ�
        /// </remarks>
        int Power { get; set; }
        
    }


    /// <summary>
    ///     ��ɫȨ��
    /// </summary>
    public class SimpleRolePower : IRolePower
    {
        /// <summary>
        ///     �����ʶ
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     ��ɫ��ʶ
        /// </summary>
        /// <remarks>
        ///     ��ɫ��ʶ
        /// </remarks>
        public int RoleId { get; set; }

        /// <summary>
        ///     ҳ��ڵ��ʶ
        /// </summary>
        /// <remarks>
        ///     ҳ��ڵ��ʶ
        /// </remarks>
        public int PageItemId { get; set; }

        /// <summary>
        ///     Ȩ��
        /// </summary>
        /// <remarks>
        ///     Ȩ��,0��ʾδ����,1��ʾ����,2��ʾ�ܾ�
        /// </remarks>
        public int Power { get; set; }

    }
}