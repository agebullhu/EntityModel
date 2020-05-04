// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;

#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    ///     ���������չ��ҵ���߼�����
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    public interface IBusinessLogicByAudit<TData>
        : IBusinessLogicByStateData<TData>
        where TData : EditDataObject, IIdentityData, IStateData, IAuditData, new()
    {
        #region ��Ϣ

        /// <summary>
        ///     ȡ��У��(���ʱ��Ч)
        /// </summary>
        bool CancelValidate { get; set; }

        #endregion

        #region ��������

        /// <summary>
        ///     �����ύ
        /// </summary>
        bool Submit(IEnumerable<long> sels);

        /// <summary>
        ///     �����˻�
        /// </summary>
        bool Back(IEnumerable<long> sels);

        /// <summary>
        ///     ����ͨ��
        /// </summary>
        bool AuditPass(IEnumerable<long> sels);

        /// <summary>
        ///     ��������
        /// </summary>
        bool Pullback(IEnumerable<long> sels);

        /// <summary>
        ///     �������
        /// </summary>
        bool AuditDeny(IEnumerable<long> sels);

        /// <summary>
        ///     ���������
        /// </summary>
        bool UnAudit(IEnumerable<long> sels);

        /// <summary>
        ///     ��������У��
        /// </summary>
        bool Validate(IEnumerable<long> sels, Action<ValidateResult> putError);

        #endregion

        #region ��������

        /// <summary>
        ///     ������ȷ��У��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Validate(long id);

        /// <summary>
        ///     ���ͨ��
        /// </summary>
        bool AuditPass(long id);

        /// <summary>
        ///     ��˲�ͨ��
        /// </summary>
        bool AuditDeny(long id);

        /// <summary>
        ///     �����
        /// </summary>
        bool UnAudit(long id);

        /// <summary>
        ///     �ύ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Submit(long id);

        /// <summary>
        ///     �˻ر༭
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Back(long id);

        #endregion


    }
}