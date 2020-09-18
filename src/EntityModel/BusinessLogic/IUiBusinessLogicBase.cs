using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.ZeroApis;

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// ֧�ֽ��������ҵ���߼��������
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TPrimaryKey">��������</typeparam>
    public interface IUiBusinessLogicBase<TData,TPrimaryKey> : IBusinessLogicBase<TData, TPrimaryKey>
        where TData : EditDataObject, IIdentityData<TPrimaryKey>, new()
    {
        #region ������

        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        [Obsolete]
        ApiPageData<TData> PageData(int page, int limit, string condition, params DbParameter[] args);

        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        ApiPageData<TData> PageData(int page, int limit, string sort, bool desc, string condition, params DbParameter[] args);

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        [Obsolete]
        ApiPageData<TData> PageData(int page, int limit, Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        [Obsolete]
        ApiPageData<TData> PageData(int page, int limit, LambdaItem<TData> lambda);
        #endregion

        #region ������Excel

        /// <summary>
        /// ������Excel
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        (string name, string mime, byte[] bytes) Export(string sheetName, LambdaItem<TData> filter);

        #endregion

        #region д����

        /// <summary>
        ///     ����
        /// </summary>
        bool Save(TData data);

        /// <summary>
        ///     ����
        /// </summary>
        Task<bool> SaveAsync(TData data);

        /// <summary>
        ///     ����
        /// </summary>
        bool AddNew(TData data);

        /// <summary>
        ///     ����
        /// </summary>
        Task<bool> AddNewAsync(TData data);

        /// <summary>
        ///     ���¶���
        /// </summary>
        bool Update(TData data);

        /// <summary>
        ///     ���¶���
        /// </summary>
        Task<bool> UpdateAsync(TData data);

        #endregion

        #region ɾ��

        /// <summary>
        ///     ɾ������
        /// </summary>
        bool Delete(IEnumerable<TPrimaryKey> lid);

        /// <summary>
        ///     ɾ������
        /// </summary>
        bool Delete(TPrimaryKey id);

        /// <summary>
        ///     ɾ������
        /// </summary>
        Task<bool> DeleteAsync(TPrimaryKey id);


        /// <summary>
        ///     ɾ������
        /// </summary>
        Task<bool> DeleteAsync(IEnumerable<TPrimaryKey> lid);

        #endregion
    }
}