using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Agebull.Common.Logging;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.Excel;
using ZeroTeam.MessageMVC.Context;
using ZeroTeam.MessageMVC.ZeroApis;

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// ֧�ֽ��������ҵ���߼��������
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TAccess">���ݷ��ʶ���</typeparam>
    public class UiBusinessLogicBase<TData, TAccess>
        : BusinessLogicBase<TData, TAccess>, IUiBusinessLogicBase<TData>
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : class, IDataTable<TData>, new()
    {

        #region ������


        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public ApiPageData<TData> PageData(int page, int limit, Expression<Func<TData, bool>> lambda)
        {
            var item = Access.Compile(lambda);
            return PageData(page, limit, null, false, item.ConditionSql, item.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public ApiPageData<TData> PageData(int page, int limit, LambdaItem<TData> lambda)
        {
            var item = Access.Compile(lambda);
            return PageData(page, limit, null, false, item.ConditionSql, item.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        [Obsolete]
        public ApiPageData<TData> PageData(int page, int limit, string sort, bool desc,
            Expression<Func<TData, bool>> lambda)
        {
            var item = Access.Compile(lambda);
            return PageData(page, limit, sort, desc, item.ConditionSql, item.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        [Obsolete]
        public ApiPageData<TData> PageData(int page, int limit, string sort, bool desc, LambdaItem<TData> lambda)
        {
            var item = Access.Compile(lambda);
            return PageData(page, limit, sort, desc, item.ConditionSql, item.Parameters);
        }

        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        [Obsolete]
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
            if (!string.IsNullOrEmpty(BaseQueryCondition))
            {
                if (string.IsNullOrEmpty(condition))
                    condition = BaseQueryCondition;
                else if (condition != BaseQueryCondition && !condition.Contains(BaseQueryCondition))
                    condition = $"({BaseQueryCondition}) AND ({condition})";
            }

            if (!DataExtendChecker.PrepareQuery<TData>(Access, ref condition, ref args))
            {
                return null;
            }

            var datas = Access.Page(page, limit, sort, desc, condition, args);
            OnListLoaded(datas.Rows);
            return datas;
        }

        /// <summary>
        ///     ��������Ĵ���
        /// </summary>
        /// <param name="datas"></param>
        protected virtual void OnListLoaded(IList<TData> datas)
        {
        }

        #endregion

        #region ������


        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<ApiPageData<TData>> PageDataAsync(int page, int limit, Expression<Func<TData, bool>> lambda)
        {
            var item = Access.Compile(lambda);
            return await PageDataAsync(page, limit, null, false, item.ConditionSql, item.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<ApiPageData<TData>> PageDataAsync(int page, int limit, LambdaItem<TData> lambda)
        {
            var item = Access.Compile(lambda);
            return await PageDataAsync(page, limit, null, false, item.ConditionSql, item.Parameters);
        }

        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        public async Task<ApiPageData<TData>> PageDataAsync(int page, int limit, string sort, bool desc, string condition,
            params DbParameter[] args)
        {
            if (!string.IsNullOrEmpty(BaseQueryCondition))
            {
                if (string.IsNullOrEmpty(condition))
                    condition = BaseQueryCondition;
                else if (condition != BaseQueryCondition && !condition.Contains(BaseQueryCondition))
                    condition = $"({BaseQueryCondition}) AND ({condition})";
            }

            if (!DataExtendChecker.PrepareQuery<TData>(Access, ref condition, ref args))
            {
                return null;
            }

            var datas = await Access.PageAsync(page, limit, sort, desc, condition, args);
            OnListLoaded(datas.Rows);
            return datas;
        }

        #endregion

        #region ���뵼��

        /// <summary>
        /// ������Excel
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public (string name,string mime,byte[] bytes) Export(string sheetName, LambdaItem<TData> filter)
        {
            var exporter = new ExcelExporter<TData, TAccess>
            {
                OnDataLoad = OnListLoaded
            };
            var data = new TData();
            var bytes = exporter.ExportExcel(filter, sheetName ?? data.__Struct.ImportName, null);
            return ($"OrderAddress-{DateTime.Now:yyyyMMDDHHmmSS}",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                bytes);
        }

        /// <summary>
        /// ������Excel
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<(string name, string mime, byte[] bytes)> ExportAsync(string sheetName, LambdaItem<TData> filter)
        {
            var exporter = new ExcelExporter<TData, TAccess>
            {
                OnDataLoad = OnListLoaded
            };
            var data = new TData();
            var bytes = await exporter.ExportExcelAsync(filter, sheetName ?? data.__Struct.ImportName, null);
            return ($"OrderAddress-{DateTime.Now:yyyyMMDDHHmmSS}",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                bytes);
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
            if (isAdd || data.__status.IsModified)
                return true;
            GlobalContext.Current.Status.LastMessage = "����δ�޸�";
            GlobalContext.Current.Status.LastState = OperatorStatusCode.ArgumentError;
            return false;
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
            if (!DataExtendChecker.PrepareAddnew(data))
            {
                return false;
            }

            using var scope = TransactionScope.CreateScope(Access.DataBase);
            {
                if (!CanSave(data, true))
                {
                    return false;
                }

                if (!PrepareSave(data, true))
                {
                    return false;
                }

                if (data.__status.IsExist)
                {
                    if (!Access.Update(data))
                        return false;
                }
                else if (!Access.Insert(data))
                    return false;

                if (!Saved(data, BusinessCommandType.AddNew))
                    return false;
                scope.Succeed();
            }

            return true;
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

            if (!DataExtendChecker.PrepareUpdate(data))
            {
                return false;
            }

            using var scope = TransactionScope.CreateScope(Access.DataBase);
            {
                if (!CanSave(data, false))
                {
                    return false;
                }

                if (!PrepareSave(data, false))
                {
                    return false;
                }

                if (!Access.Update(data))
                    return false;
                if (!Saved(data, BusinessCommandType.Update))
                    return false;
                scope.Succeed();
            }

            return true;
        }

        /// <summary>
        ///     ���¶���
        /// </summary>
        private bool Saved(TData data, BusinessCommandType type)
        {
            if (!LastSaved(data, BusinessCommandType.AddNew == type))
                return false;
            OnStateChanged(data, type);
            return true;
        }

        #endregion

        #region д����

        /// <summary>
        ///     ����
        /// </summary>
        public virtual async Task<bool> SaveAsync(TData data)
        {
            return data.Id == 0 ? await AddNewAsync(data) : await UpdateAsync(data);
        }

        /// <summary>
        ///     ����
        /// </summary>
        public virtual async Task<bool> AddNewAsync(TData data)
        {
            if (!DataExtendChecker.PrepareAddnew(data))
            {
                return false;
            }

            using var scope = TransactionScope.CreateScope(Access.DataBase);
            {
                if (!CanSave(data, true))
                {
                    return false;
                }

                if (!PrepareSave(data, true))
                {
                    return false;
                }

                if (data.__status.IsExist)
                {
                    if (!await Access.UpdateAsync(data))
                        return false;
                }
                else if (!await Access.InsertAsync(data))
                    return false;

                if (!Saved(data, BusinessCommandType.AddNew))
                    return false;
                scope.Succeed();
            }

            return true;
        }

        /// <summary>
        ///     ���¶���
        /// </summary>
        public virtual async Task<bool> UpdateAsync(TData data)
        {
            if (data.Id <= 0)
            {
                return await AddNewAsync(data);
            }

            if (!DataExtendChecker.PrepareUpdate(data))
            {
                return false;
            }

            using var scope = TransactionScope.CreateScope(Access.DataBase);
            {
                if (!CanSave(data, false))
                {
                    return false;
                }

                if (!PrepareSave(data, false))
                {
                    return false;
                }

                if (!await Access.UpdateAsync(data))
                    return false;
                if (!Saved(data, BusinessCommandType.Update))
                    return false;
                scope.Succeed();
            }

            return true;
        }

        #endregion

        #region ɾ��

        /// <summary>
        ///     ɾ������
        /// </summary>
        public bool Delete(IEnumerable<long> ids)
        {
            var list = ids.ToArray();
            if (!DataExtendChecker.PrepareDelete<TData>(list))
            {
                return false;
            }

            using var scope = TransactionScope.CreateScope(Access.DataBase);
            {
                foreach (var id in list)
                {
                    if (!DeleteInner(id))
                    {
                        return false;
                    }
                }

                scope.Succeed();
            }

            return true;
        }

        /// <summary>
        ///     ɾ������
        /// </summary>
        public bool Delete(long id)
        {
            if (!DataExtendChecker.PrepareDelete<TData>(new[] { id }))
            {
                return false;
            }

            using var scope = TransactionScope.CreateScope(Access.DataBase);
            {
                if (!DeleteInner(id))
                {
                    return false;
                }

                scope.Succeed();
            }

            return true;
        }

        /// <summary>
        ///     ɾ������
        /// </summary>
        private bool DeleteInner(long id)
        {
            if (!PrepareDelete(id))
            {
                return false;
            }

            //BUG: using (ManageModeScope.CreateScope())
            {
                if (!DoDelete(id))
                {
                    GlobalContext.Current.Status.LastMessage = $"����ֵΪ({id})�����ݲ�����,ɾ��ʧ��";
                    return false;
                }
                OnDeleted(id);
                OnStateChanged(id, BusinessCommandType.Delete);
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

        #region ɾ��

        /// <summary>
        ///     ɾ������
        /// </summary>
        public async Task<bool> DeleteAsync(IEnumerable<long> ids)
        {
            var list = ids.ToArray();
            if (!DataExtendChecker.PrepareDelete<TData>(list))
            {
                return false;
            }

            using var scope = TransactionScope.CreateScope(Access.DataBase);
            {
                foreach (var id in list)
                {
                    if (!await DeleteAsyncInner(id))
                        return false;
                }

                scope.Succeed();
            }

            return true;
        }

        /// <summary>
        ///     ɾ������
        /// </summary>
        public async Task<bool> DeleteAsync(long id)
        {
            if (!DataExtendChecker.PrepareDelete<TData>(new[] { id }))
            {
                return false;
            }
            using var scope = TransactionScope.CreateScope(Access.DataBase);
            {
                var res = await DeleteAsyncInner(id);
                scope.Succeed();
                return res;
            }
        }

        /// <summary>
        ///     ɾ������
        /// </summary>
        private async Task<bool> DeleteAsyncInner(long id)
        {
            if (!PrepareDelete(id))
            {
                return false;
            }

            //BUG:using (ManageModeScope.CreateScope())
            {
                if (!await DoDeleteAsync(id))
                {
                    GlobalContext.Current.Status.LastMessage = $"����ֵΪ({id})�����ݲ�����,ɾ��ʧ��";
                    return false; 
                }
                OnDeleted(id);
                OnStateChanged(id, BusinessCommandType.Delete);
            }
            return true;
        }

        /// <summary>
        ///     ɾ���������
        /// </summary>
        protected virtual async Task<bool> DoDeleteAsync(long id)
        {
            return await Access.DeletePrimaryKeyAsync(id);
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


    /// <summary>
    /// ֧�ֽ��������ҵ���߼��������
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TAccess">���ݷ��ʶ���</typeparam>
    /// <typeparam name="TDatabase">���ݿ����</typeparam>
    public class UiBusinessLogicBase<TData, TAccess, TDatabase>
        : UiBusinessLogicBase<TData, TAccess>
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : class, IDataTable<TData>, new()
    {
    }
}