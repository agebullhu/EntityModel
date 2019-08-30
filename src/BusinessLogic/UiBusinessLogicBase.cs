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
    /// 支持界面操作的业务逻辑对象基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TAccess">数据访问对象</typeparam>
    public class UiBusinessLogicBase<TData, TAccess>
        : BusinessLogicBase<TData, TAccess>, IUiBusinessLogicBase<TData>
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : class, IDataTable<TData>, new()
    {

        #region 读数据


        /// <summary>
        ///     分页读取
        /// </summary>
        public ApiPageData<TData> PageData(int page, int limit, Expression<Func<TData, bool>> lambda)
        {
            var item = Access.Compile(lambda);
            return PageData(page, limit, null, false, item.ConditionSql, item.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public ApiPageData<TData> PageData(int page, int limit, LambdaItem<TData> lambda)
        {
            var item = Access.Compile(lambda);
            return PageData(page, limit, null, false, item.ConditionSql, item.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        [Obsolete]
        public ApiPageData<TData> PageData(int page, int limit, string sort, bool desc,
            Expression<Func<TData, bool>> lambda)
        {
            var item = Access.Compile(lambda);
            return PageData(page, limit, sort, desc, item.ConditionSql, item.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        [Obsolete]
        public ApiPageData<TData> PageData(int page, int limit, string sort, bool desc, LambdaItem<TData> lambda)
        {
            var item = Access.Compile(lambda);
            return PageData(page, limit, sort, desc, item.ConditionSql, item.Parameters);
        }

        /// <summary>
        ///     取得列表数据
        /// </summary>
        [Obsolete]
        public ApiPageData<TData> PageData(int page, int limit, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, null, false, condition, args);
        }

        /// <summary>
        ///     取得列表数据
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
        ///     数据载入的处理
        /// </summary>
        /// <param name="datas"></param>
        protected virtual void OnListLoaded(IList<TData> datas)
        {
        }

        #endregion

        #region 导入导出

        /// <summary>
        /// 导出到Excel
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

        #region 写数据


        /// <summary>
        ///     是否可以执行保存操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected virtual bool CanSave(TData data, bool isAdd)
        {
            return isAdd || data.__status.IsModified;
        }

        /// <summary>
        ///     保存前的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected virtual bool PrepareSave(TData data, bool isAdd)
        {
            if (data.__status.IsFromClient && !PrepareSaveByUser(data, isAdd))
                return false;
            return OnSaving(data, isAdd);
        }

        /// <summary>
        ///     保存完成后的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected virtual bool LastSaved(TData data, bool isAdd)
        {
            if (data.__status.IsFromClient && !LastSavedByUser(data, isAdd))
                return false;
            return OnSaved(data, isAdd);
        }

        /// <summary>
        ///     被用户编辑的数据的保存前操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected virtual bool PrepareSaveByUser(TData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     被用户编辑的数据的保存前操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected virtual bool LastSavedByUser(TData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     保存前的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected virtual bool OnSaving(TData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     保存完成后的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected virtual bool OnSaved(TData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     新增
        /// </summary>
        public virtual bool Save(TData data)
        {
            return data.Id == 0 ? AddNew(data) : Update(data);
        }

        /// <summary>
        ///     新增
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
        ///     更新对象
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
        ///     更新对象
        /// </summary>
        private bool Saved(TData data, BusinessCommandType type)
        {
            if (!LastSaved(data, BusinessCommandType.AddNew == type))
                return false;
            OnStateChanged(data, type);
            return true;
        }

        #endregion

        #region 删除

        /// <summary>
        ///     删除对象
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
        ///     删除对象
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
        ///     删除对象
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
        ///     删除对象操作
        /// </summary>
        protected virtual bool DoDelete(long id)
        {
            return Access.DeletePrimaryKey(id);
        }

        /// <summary>
        ///     删除对象前置处理
        /// </summary>
        protected virtual bool PrepareDelete(long id)
        {
            return true;
        }

        /// <summary>
        ///     删除对象后置处理
        /// </summary>
        protected virtual void OnDeleted(long id)
        {

        }

        #endregion

        #region 状态处理


        /// <summary>
        ///     内部命令执行完成后的处理
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="cmd">命令</param>
        protected virtual void OnInnerCommand(TData data, BusinessCommandType cmd)
        {

        }


        /// <summary>
        ///     内部命令执行完成后的处理
        /// </summary>
        /// <param name="id">数据</param>
        /// <param name="cmd">命令</param>
        protected virtual void OnStateChanged(long id, BusinessCommandType cmd)
        {

        }

        /// <summary>
        ///     状态改变后的统一处理
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="cmd">命令</param>
        protected virtual void OnStateChanged(TData data, BusinessCommandType cmd)
        {
            OnInnerCommand(data, cmd);
        }

        #endregion
    }


    /// <summary>
    /// 支持界面操作的业务逻辑对象基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TAccess">数据访问对象</typeparam>
    /// <typeparam name="TDatabase">数据库对象</typeparam>
    public class UiBusinessLogicBase<TData, TAccess, TDatabase>
        : UiBusinessLogicBase<TData, TAccess>
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : class, IDataTable<TData>, new()
    {
    }
}