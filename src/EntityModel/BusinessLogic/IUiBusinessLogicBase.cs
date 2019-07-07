using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using Agebull.EntityModel.Common;
using Agebull.MicroZero.ZeroApis;

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// ֧�ֽ��������ҵ���߼��������
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    public interface IUiBusinessLogicBase<TData> : IBusinessLogicBase<TData>
        where TData : EditDataObject, IIdentityData, new()
    {
        #region ������

        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        ApiPageData<TData> PageData(int page, int limit, string condition, params DbParameter[] args);

        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        ApiPageData<TData> PageData(int page, int limit, string sort, bool desc, string condition, params DbParameter[] args);

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        ApiPageData<TData> PageData(int page, int limit, Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        ApiPageData<TData> PageData(int page, int limit, LambdaItem<TData> lambda);
        #endregion

        #region ������Excel

        /// <summary>
        /// ������Excel
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        ApiFileResult Import(string sheetName, LambdaItem<TData> filter);

        #endregion

        #region д����

        /// <summary>
        ///     ����
        /// </summary>
        bool Save(TData data);

        /// <summary>
        ///     ����
        /// </summary>
        bool AddNew(TData data);

        /// <summary>
        ///     ���¶���
        /// </summary>
        bool Update(TData data);

        #endregion

        #region ɾ��

        /// <summary>
        ///     ɾ������
        /// </summary>
        bool Delete(IEnumerable<long> lid);

        /// <summary>
        ///     ɾ������
        /// </summary>
        bool Delete(long id);

        #endregion
    }
}