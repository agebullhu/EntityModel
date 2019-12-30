// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

using System;
using System.Collections.Generic;

namespace Agebull.Common.OAuth
{
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

    /// <summary>
    /// ��λְ��
    /// </summary>
    public interface IDuty
    {
        /// <summary>
        /// ��λְ���ʶ
        /// </summary>
        long DutyId { get; set; }

        /// <summary>
        /// ��Ӧ��Ȩ�޽�ɫ����
        /// </summary>
        long RoleId { get; set; }

        /// <summary>
        ///     ��֯��ʶ
        /// </summary>
        long GroupId { get; set; }


        /// <summary>
        ///     ��֯��ʶ
        /// </summary>
        long OrganizationId { get; set; }

        /// <summary>
        ///     ����
        /// </summary>
        /// <remarks>
        ///     ����
        /// </remarks>
        string Organization { get; set; }

        /// <summary>
        ///     ְλ
        /// </summary>
        /// <remarks>
        ///     ְλ
        /// </remarks>
        string Position { get; set; }

        /// <summary>
        ///     ��ɫ
        /// </summary>
        string Role { get; set; }


        /// <summary>
        /// ְ��������֯
        /// </summary>
        List<long> OrgIds { get; set; }

        /// <summary>
        /// ��ʼʱ�䣨��Ӧ���ˣ�
        /// </summary>
        DateTime Start { get; set; }

        /// <summary>
        /// ����ʱ�䣨��Ӧ���ˣ�
        /// </summary>
        DateTime End { get; set; }
    }
}