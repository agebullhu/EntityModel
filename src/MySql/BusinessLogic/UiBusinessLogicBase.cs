using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using Agebull.Common.Logging;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.MySql;
using Agebull.MicroZero.ZeroApis;
using MySql.Data.MySqlClient;

namespace Agebull.EntityModel.BusinessLogic.MySql
{
    /// <summary>
    /// 支持界面操作的业务逻辑对象基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TAccess">数据访问对象</typeparam>
    /// <typeparam name="TDatabase">数据库对象</typeparam>
    public class UiBusinessLogicBase<TData, TAccess, TDatabase>
        : BusinessLogicBase<TData, TAccess, TDatabase>, IUiBusinessLogicBase<TData>
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : MySqlTable<TData, TDatabase>, new()
        where TDatabase : MySqlDataBase
    {

        #region 读数据

        /// <summary>
        ///     取得列表数据
        /// </summary>
        public ApiPageData<TData> PageData(int page, int limit, string condition, params MySqlParameter[] args)
        {
            return PageData(page, limit, null, false, condition, args);
        }

        /// <summary>
        ///     取得列表数据
        /// </summary>
        public ApiPageData<TData> PageData(int page, int limit, string sort, bool desc, string condition,
            params MySqlParameter[] args)
        {
            var paras = args.Cast<DbParameter>().ToArray();
            var data = Access.PageData(page, limit, sort, desc, condition, paras);
            var count = (int)Access.Count(condition, paras);
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
        ///     分页读取
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
        ///     分页读取
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
                LogRecorderX.MonitorTrace("Delete");
                OnStateChanged(id, BusinessCommandType.Delete);
                scope.SetState(true);
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

        ApiPageData<TData> IUiBusinessLogicBase<TData>.PageData(int page, int limit, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, condition, args.Cast<MySqlParameter>().ToArray());
        }

        ApiPageData<TData> IUiBusinessLogicBase<TData>.PageData(int page, int limit, string sort, bool desc, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, sort, desc, condition, args.Cast<MySqlParameter>().ToArray());
        }


        #endregion
    }
}