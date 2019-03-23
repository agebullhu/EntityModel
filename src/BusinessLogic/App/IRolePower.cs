// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

namespace Agebull.Common.OAuth
{
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
    ///     ��ɫȨ��
    /// </summary>
    public interface IRolePower
    {
        /// <summary>
        ///     �����ʶ
        /// </summary>
        long Id { get; set; }

        /// <summary>
        ///     ��ɫ��ʶ
        /// </summary>
        /// <remarks>
        ///     ��ɫ��ʶ
        /// </remarks>
        long RoleId { get; set; }

        /// <summary>
        ///     ҳ��ڵ��ʶ
        /// </summary>
        /// <remarks>
        ///     ҳ��ڵ��ʶ
        /// </remarks>
        long PageItemId { get; set; }

        /// <summary>
        ///     Ȩ��
        /// </summary>
        /// <remarks>
        ///     Ȩ��,0��ʾδ����,1��ʾ����,2��ʾ�ܾ�
        /// </remarks>
        RolePowerType Power { get; set; }
        
    }


    /// <summary>
    ///     ��ɫȨ��
    /// </summary>
    public class SimpleRolePower : IRolePower
    {
        /// <summary>
        ///     �����ʶ
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///     ��ɫ��ʶ
        /// </summary>
        /// <remarks>
        ///     ��ɫ��ʶ
        /// </remarks>
        public long RoleId { get; set; }

        /// <summary>
        ///     ҳ��ڵ��ʶ
        /// </summary>
        /// <remarks>
        ///     ҳ��ڵ��ʶ
        /// </remarks>
        public long PageItemId { get; set; }

        /// <summary>
        ///     Ȩ��
        /// </summary>
        /// <remarks>
        ///     Ȩ��,0��ʾδ����,1��ʾ����,2��ʾ�ܾ�
        /// </remarks>
        public RolePowerType Power { get; set; }

    }
}