using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Agebull.EntityModel.Common
{

    /// <summary>
    ///     数据更新触发器
    /// </summary>
    public interface IDataUpdateTrigger
    {
        /// <summary>
        ///     与更新同时执行的SQL(更新之前立即执行)
        /// </summary>
        /// <param name="provider">当前数据操作适配器</param>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        /// <returns></returns>
        Task BeforeSave<TEntity>(IDataAccessProvider<TEntity> provider, TEntity entity, DataOperatorType operatorType)
            where TEntity : class, new() => Task.CompletedTask;

        /// <summary>
        ///     保存完成后期处理
        /// </summary>
        /// <param name="provider">当前数据操作适配器</param>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        Task AfterSave<TEntity>(IDataAccessProvider<TEntity> provider, TEntity entity, DataOperatorType operatorType)
            where TEntity : class, new() => Task.CompletedTask;

        /// <summary>
        ///     更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="provider">当前数据操作适配器</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        Task BeforeExecute<TEntity>(IDataAccessProvider<TEntity> provider, DataOperatorType operatorType, string condition, IEnumerable<DbParameter> args)
             where TEntity : class, new() => Task.CompletedTask;

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="provider">当前数据操作适配器</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        Task AfterExecute<TEntity>(IDataAccessProvider<TEntity> provider, DataOperatorType operatorType, string condition, IEnumerable<DbParameter> args)
             where TEntity : class, new() => Task.CompletedTask;
    }

}