using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     数据更新触发器
    /// </summary>
    public interface IDataUpdateTrigger
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        DataBaseType DataBaseType
        {
            get;
        }

        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        void OnPrepareSave<TEntity>(TEntity entity, DataOperatorType operatorType) 
            where TEntity : class, new() { }

        /// <summary>
        ///     保存完成后期处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        void OnDataSaved<TEntity>(TEntity entity, DataOperatorType operatorType)  
            where TEntity : class, new() { }

        /// <summary>
        ///     与更新同时执行的SQL(更新之前立即执行)
        /// </summary>
        /// <param name="option">当前数据操作配置</param>
        /// <param name="condition">当前场景的执行条件</param>
        /// <param name="code">写入SQL的文本构造器</param>
        /// <returns></returns>
        void BeforeUpdateSql<TEntity>(DataAccessOption<TEntity> option, string condition, StringBuilder code) 
            where TEntity : class, new() { }

        /// <summary>
        ///     与更新同时执行的SQL(更新之后立即执行)
        /// </summary>
        /// <param name="option">当前数据操作配置</param>
        /// <param name="condition">当前场景的执行条件</param>
        /// <param name="code">写入SQL的文本构造器</param>
        /// <returns></returns>
        void AfterUpdateSql<TEntity>(DataAccessOption<TEntity> option, string condition, StringBuilder code)  
            where TEntity : class, new() { }

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="option">当前数据操作配置</param>
        /// <param name="conditions">附加的条件集合</param>
        /// <returns></returns>
        void ConditionSqlCode<TEntity>(DataAccessOption<TEntity> option, List<string> conditions)  
            where TEntity : class, new() { }

        /// <summary>
        ///     更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="option">当前数据操作配置</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        void OnOperatorExecuting<TEntity>(DataAccessOption<TEntity> option, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
             where TEntity : class, new()
        { }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="option">当前数据操作配置</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        void OnOperatorExecuted<TEntity>(DataAccessOption<TEntity> option, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
             where TEntity : class, new()
        { }
    }


    /// <summary>
    ///     数据更新触发器
    /// </summary>
    public interface IDataTrigger : IDataUpdateTrigger
    {
        /// <summary>
        ///     初始化类型
        /// </summary>
        /// <returns></returns>
        void InitType<TEntity>()  where TEntity : class, new() { }
    }
}