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
    /// <typeparam name="TPrimaryKey">��������</typeparam>
    public interface IBusinessLogicByAudit<TData, TPrimaryKey> : IBusinessLogicByStateData<TData, TPrimaryKey>
        where TData : EditDataObject, IIdentityData<TPrimaryKey>, IStateData, IAuditData, new()
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
        bool Submit(IEnumerable<TPrimaryKey> sels);

        /// <summary>
        ///     �����˻�
        /// </summary>
        bool Back(IEnumerable<TPrimaryKey> sels);

        /// <summary>
        ///     ����ͨ��
        /// </summary>
        bool AuditPass(IEnumerable<TPrimaryKey> sels);

        /// <summary>
        ///     ��������
        /// </summary>
        bool Pullback(IEnumerable<TPrimaryKey> sels);

        /// <summary>
        ///     �������
        /// </summary>
        bool AuditDeny(IEnumerable<TPrimaryKey> sels);

        /// <summary>
        ///     ���������
        /// </summary>
        bool UnAudit(IEnumerable<TPrimaryKey> sels);

        /// <summary>
        ///     ��������У��
        /// </summary>
        bool Validate(IEnumerable<TPrimaryKey> sels, Action<ValidateResult> putError);

        #endregion

        #region ��������

        /// <summary>
        ///     ������ȷ��У��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Validate(TPrimaryKey id);

        /// <summary>
        ///     ���ͨ��
        /// </summary>
        bool AuditPass(TPrimaryKey id);

        /// <summary>
        ///     ��˲�ͨ��
        /// </summary>
        bool AuditDeny(TPrimaryKey id);

        /// <summary>
        ///     �����
        /// </summary>
        bool UnAudit(TPrimaryKey id);

        /// <summary>
        ///     �ύ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Submit(TPrimaryKey id);

        /// <summary>
        ///     �˻ر༭
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Back(TPrimaryKey id);

        #endregion
    }
}