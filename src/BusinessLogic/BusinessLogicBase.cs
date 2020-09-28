// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using Agebull.EntityModel.Common;
using Agebull.EntityModel.Excel;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.Context;
using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// ҵ���߼��������
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TPrimaryKey">��������</typeparam>
    public abstract class BusinessLogicBase<TData, TPrimaryKey> //: IBusinessLogicBase<TData, TPrimaryKey>
        where TData : EditDataObject, IIdentityData<TPrimaryKey>, new()
    {
        #region ����֧�ֶ���

        /// <summary>
        ///     ʵ������
        /// </summary>
        public virtual int EntityType => 0;

        private DataAccess<TData> _access;

        /// <summary>
        ///     ���ݷ��ʶ���
        /// </summary>
        public DataAccess<TData> Access => _access ??= CreateAccess();

        /// <summary>
        ///     ���ݷ��ʶ���
        /// </summary>
        protected abstract DataAccess<TData> CreateAccess();

        /// <summary>
        /// ����
        /// </summary>
        protected BusinessLogicBase()
        {
        }

        #endregion

        #region ��������

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        public Task<bool> DoByIds(IEnumerable<TPrimaryKey> ids, Func<TPrimaryKey, Task<bool>> func, Func<Task> onEnd = null)
        {
            return LoopIds(ids, func, onEnd);
        }

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        public async Task<bool> LoopIds(IEnumerable<TPrimaryKey> ids, Func<TPrimaryKey, Task<bool>> func, Func<Task> onEnd = null)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            await connectionScope.BeginTransaction();
            foreach (var id in ids)
            {
                if (!await func.Invoke(id))
                {
                    await connectionScope.Rollback();
                    return false;
                }
            }
            if (onEnd != null)
                await onEnd.Invoke();
            await connectionScope.Commit();
            return true;
        }

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        public async Task<bool> LoopIdsToData(IEnumerable<TPrimaryKey> ids, Func<TData, Task<bool>> func, Func<Task> onEnd = null)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            await connectionScope.BeginTransaction();
            foreach (var id in ids)
            {
                var data = await Access.LoadByPrimaryKeyAsync(id);
                if (!await func.Invoke(data))
                {
                    await connectionScope.Rollback();
                    return false;
                }
            }
            if (onEnd != null)
                await onEnd.Invoke();
            await connectionScope.Commit();
            return true;
        }
        #endregion

        #region ������
        /// <summary>
        ///     ���뵱ǰ����������
        /// </summary>
        public async Task<TData> Details(LambdaItem<TData> filter)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            var data = await Access.FirstOrDefaultAsync(filter);
            if (data == null)
                return null;
            await OnDetailsLoaded(data, false);
            return data;
        }

        /// <summary>
        ///     ���뵱ǰ����������
        /// </summary>
        public async Task<TData> Details(TPrimaryKey id)
        {
            if (Equals(id, default))
                return null;
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            var data = await Access.LoadByPrimaryKeyAsync(id);
            if (data == null)
                return null;
            await OnDetailsLoaded(data, false);
            return data;
        }

        /// <summary>
        ///     ��ϸ��������
        /// </summary>
        protected virtual Task OnDetailsLoaded(TData data, bool isNew)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     ����һ����Ĭ��ֵ������
        /// </summary>
        public virtual async Task<TData> CreateData()
        {
            var data = new TData();
            await OnDetailsLoaded(data, true);
            return data;
        }

        #endregion

        #region ��ҳ��ȡ


        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<ApiPageData<TData>> PageData(int page, int limit, string sort, bool desc, LambdaItem<TData> lambda)
        {
            var item = Access.SqlBuilder.Compile(lambda);
            return PageData(page, limit, sort, desc, item.ConditionSql, item.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<ApiPageData<TData>> PageData(int page, int limit, string sort, bool desc, Expression<Func<TData, bool>> lambda)
        {
            var item = Access.SqlBuilder.Compile(lambda);
            return PageData(page, limit, sort, desc, item.ConditionSql, item.Parameters);
        }

        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        public async Task<ApiPageData<TData>> PageData(int page, int limit, string sort, bool desc, string condition,
            params DbParameter[] args)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            var datas = await Access.PageAsync(page, limit, sort, desc, condition, args);
            await OnListLoaded(datas.Rows);
            return datas;
        }

        /// <summary>
        ///     ��������Ĵ���
        /// </summary>
        /// <param name="datas"></param>
        protected virtual Task OnListLoaded(IList<TData> datas) => Task.CompletedTask;

        #endregion

        #region ���뵼��

        /// <summary>
        /// ������Excel
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<(string name, string mime, byte[] bytes)> Export(LambdaItem<TData> filter)
        {
            var exporter = new ExcelExporter<TData, TPrimaryKey>
            {
                OnDataLoad = OnListLoaded
            };
            var data = new TData();
            var bytes = await exporter.ExportExcelAsync(filter, Access.Option.DataSturct.ImportName, null);
            return ($"OrderAddress-{DateTime.Now:yyyyMMDDHHmmSS}",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                bytes);
        }
        /// <summary>
        /// ������Excel
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<(bool success, byte[] state)> Import(byte[] stream)
        {
            var map = Access.Option.DataSturct.Properties.Where(p => p.CanImport).ToDictionary(p => p.Caption, p => p.PropertyName);
            var exporter = new ExcelImporter<TData>
            {
                Access = Access,
                FieldMap = map
            };
            exporter.Prepare(stream);
            if (await exporter.ImportExcel())
                return (true, null);
            return (false, exporter.ToStream());
        }

        #endregion


        #region д����


        /// <summary>
        ///     �Ƿ����ִ�б������
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual Task<bool> CanSave(TData data, bool isAdd)
        {
            if (isAdd || data.__status.IsModified)
                return Task.FromResult(true);
            GlobalContext.Current.Status.LastMessage = "����δ�޸�";
            GlobalContext.Current.Status.LastState = OperatorStatusCode.ArgumentError;
            return Task.FromResult(false);
        }

        /// <summary>
        ///     ����ǰ�Ĳ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual async Task<bool> PrepareSave(TData data, bool isAdd)
        {
            if (data.__status.IsFromClient && !await PrepareSaveByUser(data, isAdd))
                return false;
            return await OnSaving(data, isAdd);
        }

        /// <summary>
        ///     ������ɺ�Ĳ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual async Task<bool> LastSaved(TData data, bool isAdd)
        {
            if (data.__status.IsFromClient && !await LastSavedByUser(data, isAdd))
                return false;
            return await OnSaved(data, isAdd);
        }

        /// <summary>
        ///     ���û��༭�����ݵı���ǰ����
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual Task<bool> PrepareSaveByUser(TData data, bool isAdd)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        ///     ���û��༭�����ݵı���ǰ����
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual Task<bool> LastSavedByUser(TData data, bool isAdd)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        ///     ����ǰ�Ĳ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual Task<bool> OnSaving(TData data, bool isAdd)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        ///     ������ɺ�Ĳ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual Task<bool> OnSaved(TData data, bool isAdd)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        ///     ����
        /// </summary>
        public virtual Task<bool> Save(TData data)
        {
            return Equals(data.Id, default) ? AddNew(data) : Update(data);
        }

        /// <summary>
        ///     ����
        /// </summary>
        public virtual async Task<bool> AddNew(TData data)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            await connectionScope.BeginTransaction();
            {
                if (!await CanSave(data, true))
                {
                    return false;
                }

                if (!await PrepareSave(data, true))
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

                if (!await Saved(data, BusinessCommandType.AddNew))
                    return false;
            }
            await connectionScope.Commit();
            return true;
        }

        /// <summary>
        ///     ���¶���
        /// </summary>
        public virtual async Task<bool> Update(TData data)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            await connectionScope.BeginTransaction();
            {
                if (!await CanSave(data, false))
                {
                    return false;
                }
                if (!await PrepareSave(data, false))
                {
                    return false;
                }
                if (!await Access.UpdateAsync(data))
                    return false;
                if (!await Saved(data, BusinessCommandType.Update))
                    return false;
            }
            await connectionScope.Commit();
            return true;
        }

        /// <summary>
        ///     ���¶���
        /// </summary>
        private async Task<bool> Saved(TData data, BusinessCommandType type)
        {
            if (!await LastSaved(data, BusinessCommandType.AddNew == type))
                return false;
            await OnStateChanged(data, type);
            return true;
        }

        #endregion

        #region ɾ��

        /// <summary>
        ///     ɾ������
        /// </summary>
        public async Task<bool> Delete(IEnumerable<TPrimaryKey> ids)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            await connectionScope.BeginTransaction();
            {
                foreach (var id in ids)
                {
                    if (!await DeleteInner(id))
                    {
                        return false;
                    }
                }
            }

            await connectionScope.Commit();
            return true;
        }

        /// <summary>
        ///     ɾ������
        /// </summary>
        public async Task<bool> Delete(TPrimaryKey id)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            await connectionScope.BeginTransaction();

            if (!await DeleteInner(id))
            {
                return false;
            }
            await connectionScope.Commit();
            return true;
        }

        /// <summary>
        ///     ɾ������
        /// </summary>
        private async Task<bool> DeleteInner(TPrimaryKey id)
        {
            if (!await PrepareDelete(id))
            {
                return false;
            }
            if (!await DoDelete(id))
            {
                GlobalContext.Current.Status.LastMessage = $"����ֵΪ({id})�����ݲ�����,ɾ��ʧ��";
                return false;
            }
            await OnDeleted(id);
            await OnStateChanged(id, BusinessCommandType.Delete);
            return true;
        }

        /// <summary>
        ///     ɾ���������
        /// </summary>
        protected virtual Task<bool> DoDelete(TPrimaryKey id)
        {
            return Access.DeletePrimaryKeyAsync(id);
        }

        /// <summary>
        ///     ɾ������ǰ�ô���
        /// </summary>
        protected virtual Task<bool> PrepareDelete(TPrimaryKey id)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        ///     ɾ��������ô���
        /// </summary>
        protected virtual Task OnDeleted(TPrimaryKey id)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region ״̬����


        /// <summary>
        ///     �ڲ�����ִ����ɺ�Ĵ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="cmd">����</param>
        protected virtual Task OnInnerCommand(TData data, BusinessCommandType cmd)
        {
            return Task.CompletedTask;
        }


        /// <summary>
        ///     �ڲ�����ִ����ɺ�Ĵ���
        /// </summary>
        /// <param name="id">����</param>
        /// <param name="cmd">����</param>
        protected virtual Task OnStateChanged(TPrimaryKey id, BusinessCommandType cmd)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     ״̬�ı���ͳһ����
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="cmd">����</param>
        protected virtual Task OnStateChanged(TData data, BusinessCommandType cmd)
        {
            return OnInnerCommand(data, cmd);
        }

        #endregion
    }


    /// <summary>
    /// ҵ���߼��������
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    public abstract class BusinessLogicBase<TData> : BusinessLogicBase<TData, long>
        where TData : EditDataObject, IIdentityData<long>, new()
    {
    }
}