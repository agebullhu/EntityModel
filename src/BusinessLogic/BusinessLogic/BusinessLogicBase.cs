// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using Agebull.EntityModel.Common;
using Agebull.EntityModel.Events;
using Agebull.EntityModel.Excel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        where TData : class, IIdentityData<TPrimaryKey>, new()
    {
        #region ����֧�ֶ���

        /// <summary>
        /// ��������
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

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

        #region ������

        IBusinessContext _context;

        /// <summary>
        /// ������
        /// </summary>
        public IBusinessContext Context
        {
            get => _context ??= ServiceProvider.GetService<IBusinessContext>();
            set => _context = value;
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
        public async Task<TData> Details(LambdaItem<TData> fiter)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            TData data;
            {
                using var fieldScope = Access.DynamicOption(fiter);
                data = await Access.FirstOrDefaultAsync(fiter);
            }
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
        public async Task<ApiPageData<TData>> PageData(PageArgument argument, LambdaItem<TData> fiter)
        {
            var item = Access.SqlBuilder.Compile(fiter);
            using var fieldScope = Access.DynamicOption(fiter);
            return await PageData(argument.Page, argument.PageSize, argument.Order, argument.Desc, item.ConditionSql, item.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<ApiPageData<TData>> PageData(int page, int limit, string sort, bool desc, LambdaItem<TData> fiter)
        {
            var item = Access.SqlBuilder.Compile(fiter);
            using var fieldScope = Access.DynamicOption(fiter);
            return await PageData(page, limit, sort, desc, item.ConditionSql, item.Parameters);
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
        async Task<ApiPageData<TData>> PageData(int page, int limit, string sort, bool desc, string condition,
            DbParameter[] args)
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
                DataQuery = Access,
                OnDataLoad = OnListLoaded
            };
            var data = new TData();
            var bytes = await exporter.ExportExcelAsync(filter, Access.Option.DataStruct.ImportName, null);
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
            var map = Access.Option.DataStruct.Properties.Where(p => p.CanImport).ToDictionary(p => p.Caption, p => p.PropertyName);
            var exporter = new ExcelImporter<TData>
            {
                DataAccess = Access,
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
            if (data is IValidate validate && !validate.Validate(out var result))
            {
                Context.LastMessage = result.ToString();
                Context.LastState = Context.ArgumentError;
                return Task.FromResult(false);
            }
            if (isAdd || !(data is IEditStatus status) || status.EditStatusRedorder == null || status.EditStatusRedorder.IsModified)
                return Task.FromResult(true);

            Context.LastMessage = "����δ�޸�";
            Context.LastState = Context.ArgumentError;
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
            if (data is IEditStatus status && status.EditStatusRedorder != null &&
                status.EditStatusRedorder.IsFromClient && !await PrepareSaveByUser(data, isAdd))
                return false;
            return true;
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
            if (!await CanSave(data, true))
            {
                return false;
            }

            if (!await PrepareSave(data, true))
            {
                return false;
            }

            if (!(data is IEditStatus status) || status.EditStatusRedorder == null)
            {
                if (!await Access.InsertAsync(data))
                    return false;
            }
            else
            {
                if (status.EditStatusRedorder.IsExist)
                {
                    if (!await Access.UpdateAsync(data))
                        return false;
                }
                else if (!await Access.InsertAsync(data))
                    return false;
                if (status.EditStatusRedorder.IsFromClient && !await SavedByUser(data, true))
                    return false;
            }

            await connectionScope.Commit();

            await OnCommandSuccess(data, default, BusinessCommandType.AddNew);
            return true;
        }

        /// <summary>
        ///     ���¶���
        /// </summary>
        public virtual async Task<bool> Update(TData data)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            await connectionScope.BeginTransaction();
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
            if (data is IEditStatus status && status.EditStatusRedorder != null)
            {
                if (status.EditStatusRedorder.IsFromClient && !await SavedByUser(data, true))
                    return false;
            }
            await connectionScope.Commit();

            await OnCommandSuccess(data, default, BusinessCommandType.Update);
            return true;
        }

        #endregion

        #region �û��ύ������չ����

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
        protected virtual Task<bool> SavedByUser(TData data, bool isAdd)
        {
            return Task.FromResult(true);
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
            foreach (var id in ids)
            {
                if (!await DeleteInner(id))
                {
                    return false;
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
            if (!await DoDelete(id))
            {
                Context.LastMessage = $"����ֵΪ({id})�����ݲ�����,ɾ��ʧ��";
                return false;
            }
            await OnCommandSuccess(default, id, BusinessCommandType.Delete);
            return true;
        }

        /// <summary>
        ///     ɾ���������
        /// </summary>
        protected virtual Task<bool> DoDelete(TPrimaryKey id)
        {
            return Access.DeletePrimaryKeyAsync(id);
        }

        #endregion

        #region ״̬����

        /// <summary>
        /// �Ƿ�ͳһ����״̬�仯
        /// </summary>
        protected bool unityStateChanged = false;

        /// <summary>
        ///     �ڲ�����ִ����ɺ�Ĵ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="id">��������</param>
        /// <param name="cmd">����</param>
        protected virtual async Task OnCommandSuccess(TData data, TPrimaryKey id, BusinessCommandType cmd)
        {
            if (!unityStateChanged)
                return;
            Context.LastState = 0;
            Context.LastMessage = null;

            var service = ServiceProvider.GetService<IEntityModelEventProxy>();
            if (service != null)
                await service.OnBusinessCommandSuccess(
                    Access.Option.DataStruct.ProjectName,
                    Access.Option.DataStruct.EntityName,
                    data, id.ToString(), cmd);

            if (data == null)
            {
                data = await Access.LoadByPrimaryKeyAsync(id);
                if (data == null)
                    return;
            }
            using var levelScope = Access.InjectionScope(InjectionLevel.NotCondition);
            await OnCommandSuccess(data, cmd);
        }

        /// <summary>
        ///     �ڲ�����ִ����ɺ�Ĵ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="cmd">����</param>
        protected virtual Task OnCommandSuccess(TData data, BusinessCommandType cmd)
        {
            return Task.CompletedTask;
        }

        #endregion
    }


    /// <summary>
    /// ҵ���߼��������
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    public abstract class BusinessLogicBase<TData> : BusinessLogicBase<TData, long>
        where TData : class, IEditStatus, IIdentityData<long>, new()
    {
    }
}