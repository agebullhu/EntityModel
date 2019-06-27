// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.Excel;
using Agebull.EntityModel.SqlServer;
using Agebull.MicroZero.ZeroApis;

#endregion

namespace Agebull.EntityModel.BusinessLogic.SqlServer
{
    /// <summary>
    /// 业务逻辑对象基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TAccess">数据访问对象</typeparam>
    /// <typeparam name="TDatabase">数据库对象</typeparam>
    public class BusinessLogicBase<TData, TAccess,TDatabase> : IBusinessLogicBase<TData>
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : SqlServerTable<TData, TDatabase>, new()
        where TDatabase : SqlServerDataBase
    {
        #region 基础支持对象

        /// <summary>
        ///     实体类型
        /// </summary>
        public virtual int EntityType => 0;

        private TAccess _access;

        /// <summary>
        /// 数据访问对象
        /// </summary>
        IDataTable<TData> IBusinessLogicBase<TData>.Access => Access;

        /// <summary>
        ///     数据访问对象
        /// </summary>
        public TAccess Access => _access ?? (_access = CreateAccess());

        /// <summary>
        ///     数据访问对象
        /// </summary>
        protected virtual TAccess CreateAccess()
        {
            var access = new TAccess();
            return access;
        }
        /// <summary>
        /// 构造
        /// </summary>
        protected BusinessLogicBase()
        {
        }
        #endregion

        #region 便利操作

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        public bool DoByIds(IEnumerable<long> ids, Func<long, bool> func, Action onEnd = null)
        {
            return LoopIds(ids, func, onEnd);
        }

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        public bool LoopIds(IEnumerable<long> ids, Func<long, bool> func, Action onEnd = null)
        {
            using (var scope = TransactionScope.CreateScope(Access.DataBase))
            {
                foreach (var id in ids)
                {
                    if (!func(id))
                    {
                        return false;
                    }
                }
                onEnd?.Invoke();
                scope.SetState(true);
            }
            return true;
        }

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        public bool DoByIds(IEnumerable<long> ids, Func<TData, bool> func, Action onEnd = null)
        {
            return LoopIdsToData(ids, func, onEnd);
        }

        /// <summary>
        ///     执行ID组合字串的操作（来自页面的,号组合的ID）
        /// </summary>
        public bool LoopIdsToData(IEnumerable<long> ids, Func<TData, bool> func, Action onEnd = null)
        {
            using (var scope = TransactionScope.CreateScope(Access.DataBase))
            {
                foreach (var id in ids)
                {
                    var data = Access.LoadByPrimaryKey(id);
                    if (data == null || !func(data))
                    {
                        return false;
                    }
                }
                onEnd?.Invoke();
                scope.SetState(true);
            }
            return true;
        }
        #endregion

        #region 读数据

        /// <summary>
        ///     取得列表数据
        /// </summary>
        public List<TData> All()
        {
            return Access.All();
        }

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public List<TData> All(LambdaItem<TData> lambda)
        {
            return Access.All(lambda);
        }

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public List<TData> All(Expression<Func<TData, bool>> lambda)
        {
            return Access.All(lambda);
        }

        /// <summary>
        ///     载入当前操作的数据
        /// </summary>
        public TData FirstOrDefault(Expression<Func<TData, bool>> lambda)
        {
            return Access.FirstOrDefault(lambda);
        }

        /// <summary>
        ///     载入当前操作的数据
        /// </summary>
        public virtual TData Details(long id)
        {
            return id == 0 ? null : Access.LoadByPrimaryKey(id);
        }

        #endregion

        #region 导入导出

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public ApiFileResult Import(string sheetName, LambdaItem<TData> filter)
        {
            var exporter = new ExcelExporter<TData, TAccess>();
            var bytes = exporter.ExportExcel(filter, sheetName, null);
            return new ApiFileResult
            {
                Mime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = $"OrderAddress-{DateTime.Now:yyyyMMDDHHmmSS}",
                Data = bytes
            };
        }

        #endregion
    }
}