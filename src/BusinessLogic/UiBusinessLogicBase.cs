using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using Agebull.Common.Context;
using Agebull.Common.Logging;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.Excel;
using Agebull.MicroZero.ZeroApis;

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

        #region ���뵼��

        /// <summary>
        /// ������Excel
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public ApiFileResult Export(string sheetName, LambdaItem<TData> filter)
        {
            var exporter = new ExcelExporter<TData, TAccess>
            {
                OnDataLoad = OnListLoaded
            };
            var data = new TData();
            var bytes = exporter.ExportExcel(filter, sheetName ?? data.__Struct.ImportName, null);
            return new ApiFileResult
            {
                Mime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = $"OrderAddress-{DateTime.Now:yyyyMMDDHHmmSS}",
                Data = bytes
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
            return isAdd || data.__status.IsModified;
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

                if (data.__status.IsExist)
                {
                    if (!Access.Update(data))
                        return false;
                }
                else if (!Access.Insert(data))
                    return false;

                if (!Saved(data, BusinessCommandType.AddNew))
                    return false;
                scope.SetState(true);
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

                if (!Access.Update(data))
                    return false;
                if (!Saved(data, BusinessCommandType.Update))
                    return false;
                scope.SetState(true);
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

            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                foreach (var id in list)
                {
                    if (!DeleteInner(id))
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
            if (!DataExtendChecker.PrepareDelete<TData>(new[] { id }))
            {
                return false;
            }

            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                if (!DeleteInner(id))
                {
                    return false;
                }

                scope.SetState(true);
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

            using (ManageModeScope.CreateScope())
            {
                if (!DoDelete(id))
                    return false;
                OnDeleted(id);
                LogRecorderX.MonitorTrace("Delete");
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