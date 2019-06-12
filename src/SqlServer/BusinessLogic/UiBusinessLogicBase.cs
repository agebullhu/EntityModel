using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using Agebull.Common.Logging;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.SqlServer;
using Agebull.MicroZero.ZeroApis;

namespace Agebull.EntityModel.BusinessLogic.SqlServer
{
    /// <summary>
    /// ֧�ֽ��������ҵ���߼��������
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TAccess">���ݷ��ʶ���</typeparam>
    /// <typeparam name="TDatabase">���ݿ����</typeparam>
    public class UiBusinessLogicBase<TData, TAccess, TDatabase> 
        : BusinessLogicBase<TData, TAccess, TDatabase>, IUiBusinessLogicBase<TData>
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : SqlServerTable<TData, TDatabase>, new()
        where TDatabase : SqlServerDataBase
    {

        #region ������

        ApiPageData<TData> IUiBusinessLogicBase<TData>.PageData(int page, int limit, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, condition, args);
        }

        ApiPageData<TData> IUiBusinessLogicBase<TData>.PageData(int page, int limit, string sort, bool desc, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, sort, desc, condition, args);
        }

        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        public ApiPageData<TData> PageData(int page, int limit, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, null, false, condition, args);
        }

        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        public ApiPageData<TData> PageData(int page, int limit, string sort, bool desc, string condition,
            params DbParameter[] args)
        {
            var data = Access.PageData(page, limit, sort, desc, condition, args);
            var count = (int)Access.Count(condition, args);
            return new ApiPageData<TData>
            {
                RowCount = count,
                Rows = data,
                PageIndex = page,
                PageSize = limit,
                PageCount = count / limit + (((count % limit) > 0 ? 1 : 0))
            };
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public ApiPageData<TData> PageData(int page, int limit, Expression<Func<TData, bool>> lambda)
        {
            if (limit <= 0 || limit >= 999)
            {
                limit = 30;
            }
            var data = Access.PageData(page, limit, lambda);
            var count = (int)Access.Count(lambda);
            return new ApiPageData<TData>
            {
                RowCount = count,
                Rows = data,
                PageIndex = page,
                PageSize = limit,
                PageCount = count / limit + (((count % limit) > 0 ? 1 : 0))
            };
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public ApiPageData<TData> PageData(int page, int limit, LambdaItem<TData> lambda)
        {
            if (limit <= 0 || limit >= 999)
            {
                limit = 30;
            }
            var data = Access.PageData(page, limit, lambda);
            var count = (int)Access.Count(lambda);
            return new ApiPageData<TData>
            {
                RowCount = count,
                Rows = data,
                PageIndex = page,
                PageSize = limit,
                PageCount = count / limit + (((count % limit) > 0 ? 1 : 0))
            };
        }
        #endregion

        #region д����


        /// <summary>
        ///     �Ƿ����ִ�б������
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual bool CanSave(TData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     ����ǰ�Ĳ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual bool PrepareSave(TData data, bool isAdd)
        {
            if (data.__status.IsFromClient && !PrepareSaveByUser(data, isAdd))
                return false;
            return OnSaving(data, isAdd);
        }

        /// <summary>
        ///     ������ɺ�Ĳ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual bool LastSaved(TData data, bool isAdd)
        {
            if (data.__status.IsFromClient && !LastSavedByUser(data, isAdd))
                return false;
            return OnSaved(data, isAdd);
        }

        /// <summary>
        ///     ���û��༭�����ݵı���ǰ����
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual bool PrepareSaveByUser(TData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     ���û��༭�����ݵı���ǰ����
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual bool LastSavedByUser(TData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     ����ǰ�Ĳ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual bool OnSaving(TData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     ������ɺ�Ĳ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual bool OnSaved(TData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     ����
        /// </summary>
        public virtual bool Save(TData data)
        {
            return data.Id == 0 ? AddNew(data) : Update(data);
        }
        /// <summary>
        ///     ����
        /// </summary>
        public virtual bool AddNew(TData data)
        {
            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                if (!CanSave(data, true))
                {
                    return false;
                }
                if (!PrepareSave(data, true))
                {
                    return false;
                }
                if (!data.__status.IsExist)
                    Access.Update(data);
                else
                    Access.Insert(data);
                var result = LastSaved(data, true);
                OnStateChanged(data, BusinessCommandType.AddNew);
                scope.SetState(true);
                return result;
            }
        }

        /// <summary>
        ///     ���¶���
        /// </summary>
        public virtual bool Update(TData data)
        {
            if (data.Id <= 0)
            {
                return AddNew(data);
            }
            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                if (!CanSave(data, false))
                {
                    return false;
                }
                if (!PrepareSave(data, false))
                {
                    return false;
                }
                Access.Update(data);
                var result = LastSaved(data, false);
                OnStateChanged(data, BusinessCommandType.Update);
                scope.SetState(true);
                return result;
            }
        }


        #endregion

        #region ɾ��

        /// <summary>
        ///     ɾ������
        /// </summary>
        public bool Delete(IEnumerable<long> lid)
        {
            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                foreach (var id in lid)
                {
                    if (!Delete(id))
                        return false;
                }
                scope.SetState(true);
            }
            return true;
        }

        /// <summary>
        ///     ɾ������
        /// </summary>
        public bool Delete(long id)
        {
            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                if (!PrepareDelete(id))
                {
                    return false;
                }
                if (!DoDelete(id))
                    return false;
                OnDeleted(id);
                LogRecorderX.MonitorTrace("Delete");
                OnStateChanged(id, BusinessCommandType.Delete);
                scope.SetState(true);
            }
            return true;
        }

        /// <summary>
        ///     ɾ���������
        /// </summary>
        protected virtual bool DoDelete(long id)
        {
            return Access.DeletePrimaryKey(id);
        }

        /// <summary>
        ///     ɾ������ǰ�ô���
        /// </summary>
        protected virtual bool PrepareDelete(long id)
        {
            return true;
        }

        /// <summary>
        ///     ɾ��������ô���
        /// </summary>
        protected virtual void OnDeleted(long id)
        {

        }
        #endregion

        #region ״̬����


        /// <summary>
        ///     �ڲ�����ִ����ɺ�Ĵ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="cmd">����</param>
        protected virtual void OnInnerCommand(TData data, BusinessCommandType cmd)
        {

        }


        /// <summary>
        ///     �ڲ�����ִ����ɺ�Ĵ���
        /// </summary>
        /// <param name="id">����</param>
        /// <param name="cmd">����</param>
        protected virtual void OnStateChanged(long id, BusinessCommandType cmd)
        {

        }

        /// <summary>
        ///     ״̬�ı���ͳһ����
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="cmd">����</param>
        protected virtual void OnStateChanged(TData data, BusinessCommandType cmd)
        {
            OnInnerCommand(data, cmd);
        }

        #endregion
    }
}