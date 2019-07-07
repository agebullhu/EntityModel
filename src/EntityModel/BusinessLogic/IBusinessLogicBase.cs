// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Agebull.EntityModel.Common;
using Agebull.MicroZero.ZeroApis;

#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// ҵ���߼��������
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    public interface IBusinessLogicBase<TData>
        where TData : EditDataObject, new()
    {
        #region ��������

        /// <summary>
        /// ���ݷ��ʶ���
        /// </summary>
        IDataTable<TData> Access { get; }

        /// <summary>
        /// ʵ������
        /// </summary>
        int EntityType { get; }

        #endregion

        #region ��������

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        bool DoByIds(IEnumerable<long> ids, Func<long, bool> func, Action onEnd = null);

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        bool LoopIds(IEnumerable<long> ids, Func<long, bool> func, Action onEnd = null);

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        bool DoByIds(IEnumerable<long> ids, Func<TData, bool> func, Action onEnd = null);

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        bool LoopIdsToData(IEnumerable<long> ids, Func<TData, bool> func, Action onEnd = null);
        #endregion

        #region ������

        /// <summary>
        ///     ���뵱ǰ����������
        /// </summary>
        TData Details(long id);

        #endregion

    }
}