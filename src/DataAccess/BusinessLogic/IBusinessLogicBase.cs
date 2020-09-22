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
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// ҵ���߼��������
    /// </summary>
    /// <typeparam name="TEntity">���ݶ���</typeparam>
    /// <typeparam name="TPrimaryKey">��������</typeparam>
    public interface IBusinessLogicBase<TEntity,TPrimaryKey>
        where TEntity : EditDataObject, new()
    {
        #region ��������

        /// <summary>
        /// ���ݷ��ʶ���
        /// </summary>
        IDataAccess<TEntity> Access { get; }

        /// <summary>
        /// ʵ������
        /// </summary>
        int EntityType { get; }

        #endregion

        #region ������������

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        bool DoByIds(IEnumerable<TPrimaryKey> ids, Func<TPrimaryKey, bool> func, Action onEnd = null);

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        bool LoopIds(IEnumerable<TPrimaryKey> ids, Func<TPrimaryKey, bool> func, Action onEnd = null);

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        bool DoByIds(IEnumerable<TPrimaryKey> ids, Func<TEntity, bool> func, Action onEnd = null);

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        bool LoopIdsToData(IEnumerable<TPrimaryKey> ids, Func<TEntity, bool> func, Action onEnd = null);
        #endregion

        #region ������

        /// <summary>
        ///     ���뵱ǰ����������
        /// </summary>
        TEntity Details(TPrimaryKey id);

        #endregion

        #region ��ҳ��ȡ

        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        ApiPageData<TEntity> PageData(int page, int limit, string sort, bool desc, string condition, params DbParameter[] args);

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        [Obsolete]
        ApiPageData<TEntity> PageData(int page, int limit, Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        [Obsolete]
        ApiPageData<TEntity> PageData(int page, int limit, LambdaItem<TEntity> lambda);
        #endregion

        #region ������Excel

        /// <summary>
        /// ������Excel
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        (string name, string mime, byte[] bytes) Export(string sheetName, LambdaItem<TEntity> filter);

        #endregion

        #region д����

        /// <summary>
        ///     ����
        /// </summary>
        bool Save(TEntity data);

        /// <summary>
        ///     ����
        /// </summary>
        Task<bool> SaveAsync(TEntity data);

        /// <summary>
        ///     ����
        /// </summary>
        bool AddNew(TEntity data);

        /// <summary>
        ///     ����
        /// </summary>
        Task<bool> AddNewAsync(TEntity data);

        /// <summary>
        ///     ���¶���
        /// </summary>
        bool Update(TEntity data);

        /// <summary>
        ///     ���¶���
        /// </summary>
        Task<bool> UpdateAsync(TEntity data);

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