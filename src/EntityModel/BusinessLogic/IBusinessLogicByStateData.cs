// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System.Collections.Generic;
using Agebull.EntityModel.Common;

#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// ��������״̬��ҵ���߼�����
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    public interface IBusinessLogicByStateData<TData>
        : IUiBusinessLogicBase<TData>
        where TData : EditDataObject, IIdentityData,IStateData, new()
    {
        #region ��������


        /// <summary>
        ///     ���ö���
        /// </summary>
        bool Enable(IEnumerable<long> sels);

        /// <summary>
        ///     ���ö���
        /// </summary>
        bool Disable(IEnumerable<long> sels);

        /// <summary>
        ///     ���ö���
        /// </summary>
        bool Lock(IEnumerable<long> sels);
        #endregion

        #region ״̬����

        /// <summary>
        ///     ��������״̬
        /// </summary>
        /// <param name="data"></param>
        bool ResetState(TData data);


        /// <summary>
        ///     ��������״̬
        /// </summary>
        /// <param name="id"></param>
        bool Reset(long id);

        /// <summary>
        ///     ���ö���
        /// </summary>
        bool Enable(long id);

        /// <summary>
        ///     ���ö���
        /// </summary>
        bool Disable(long id);

        /// <summary>
        ///     ���ö���
        /// </summary>
        bool Discard(long id);

        /// <summary>
        ///     ��������
        /// </summary>
        bool Lock(long id);

        #endregion
    }
}