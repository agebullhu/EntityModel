// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     ��ʾ��������֧�����
    /// </summary>
    public interface IAuditData
    {
        /// <summary>
        ///     ���״̬
        /// </summary>
        /// <value>AuditStateType</value>
        AuditStateType AuditState { get; set; }

        /// <summary>
        ///     �����
        /// </summary>
        /// <value>string</value>
        string AuditorId { get; set; }


        /// <summary>
        ///     �����
        /// </summary>
        /// <value>string</value>
        string Auditor { get; set; }


        /// <summary>
        ///     �������
        /// </summary>
        /// <value>DateTime</value>
        DateTime AuditDate { get; set; }

        ///// <summary>
        /////     �����(�����ڽ���)
        ///// </summary>
        ///// <value>string</value>
        //string ToUsers { get; set; }

        ///// <summary>
        /////     �ܷ����(�����ڽ���)
        ///// </summary>
        ///// <value>bool</value>
        //bool CanAudit { get; set; }
    }
}