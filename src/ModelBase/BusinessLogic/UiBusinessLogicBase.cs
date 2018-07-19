using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using Agebull.Common.Logging;

namespace Gboxt.Common.DataModel.BusinessLogic
{
    /// <summary>
    /// 支持界面操作的业务逻辑对象基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TAccess">数据访问对象</typeparam>
    public class UiBusinessLogicBase<TData, TAccess> : BusinessLogicBase<TData, TAccess>
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : class, IDataTable<TData>, new()
    {
        #region 读数据
        
        /// <summary>
        ///     取得列表数据
        /// </summary>
        public ApiPageResult<TData> PageData(int page, int limit, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, null, false, condition, args);
        }

        /// <summary>
        ///     取得列表数据
        /// </summary>
        public ApiPageResult<TData> PageData(int page, int limit, string sort, bool desc, string condition,
            params DbParameter[] args)
        {
            using (Access.DataBase.CreateDataBaseScope())
            {
                //using (MySqlReaderScope<TData>.CreateScope(Access, Access.SimpleFields, Access.SimpleLoad))
                {
                    var data = Access.PageData(page, limit, sort, desc, condition, args);
                    var count = (int)Access.Count(condition, args);
                    return new ApiPageResult<TData>
                    {
                        ResultData = new ApiPageData<TData>
                        {
                            RowCount= count,
                            Rows = data
                        }
                    };
                }
            }
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public ApiPageResult<TData> PageData(int page, int limit, Expression<Func<TData, bool>> lambda)
        {
            using (Access.DataBase.CreateDataBaseScope())
            {
                //using (MySqlReaderScope<TData>.CreateScope(Access, Access.SimpleFields, Access.SimpleLoad))
                {
                    if (limit <= 0 || limit >= 999)
                    {
                        limit = 30;
                    }
                    var data = Access.PageData(page, limit, lambda);
                    var count = (int)Access.Count(lambda);
                    return new ApiPageResult<TData>
                    {
                        ResultData = new ApiPageData<TData>
                        {
                            RowCount = count,
                            Rows = data
                        }
                    };
                }
            }
        }
        /* */
        #endregion

        #region 写数据

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
        protected virtual void OnInnerCommand(long id, BusinessCommandType cmd)
        {

        }


        /// <summary>
        ///     是否可以执行保存操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected virtual bool CanSave(TData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     保存前的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected virtual bool PrepareSave(TData data, bool isAdd)
        {
            if (data.__IsFromUser && !PrepareSaveByUser(data, isAdd))
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
            if (data.__IsFromUser && !LastSavedByUser(data, isAdd))
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
                if (!data.__EntityStatusNull && data.__EntityStatus.IsExist)
                    Access.Update(data);
                else
                    Access.Insert(data);
                var result = LastSaved(data, true);
                scope.SetState(true);
                return result;
            }
        }

        /// <summary>
        ///     更新对象
        /// </summary>
        public virtual bool Update(TData data)
        {
            if (data.Id == 0)
            {
                return AddNew(data);
            }
            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                if (!CanSave(data, true))
                {
                    return false;
                }
                if (!PrepareSave(data, false))
                {
                    return false;
                }
                Access.Update(data);
                var result = LastSaved(data, false);
                scope.SetState(true);
                return result;
            }
        }


        #endregion

        #region 删除

        /// <summary>
        ///     删除对象
        /// </summary>
        public bool Delete(IEnumerable<long> lid)
        {
            using (Access.DataBase.CreateDataBaseScope())
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
            }
            return true;
        }

        /// <summary>
        ///     删除对象
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
                LogRecorder.MonitorTrace("Delete");
                OnInnerCommand(id, BusinessCommandType.Delete);
                scope.SetState(true);
                return true;
            }
        }

        /// <summary>
        ///     删除对象操作
        /// </summary>
        protected virtual bool DoDelete(long id)
        {
            return Access.DeletePrimaryKey(id) == 1;
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
    }
}