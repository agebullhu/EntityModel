using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Agebull.Common.DataModel;

namespace Agebull.Orm.Abstractions
{
    /// <summary>
    ///     数据更新触发器
    /// </summary>
    public interface IDataTrigger : IDataUpdateTrigger
    {
        /// <summary>
        ///     初始化类型
        /// </summary>
        /// <returns></returns>
        void InitType<TEntity>() where TEntity : EditDataObject, new();
    }

    /// <summary>
    ///     数据更新触发器
    /// </summary>
    public interface IDataUpdateTrigger
    {
        /// <summary>
        ///     与更新同时执行的SQL(更新之前立即执行)
        /// </summary>
        /// <param name="table">当前数据操作对象</param>
        /// <param name="condition">当前场景的执行条件</param>
        /// <param name="code">写入SQL的文本构造器</param>
        /// <returns></returns>
        void BeforeUpdateSql<TEntity>(IDataTable<TEntity> table,string condition, StringBuilder code) where TEntity : EditDataObject, new();

        /// <summary>
        ///     与更新同时执行的SQL(更新之后立即执行)
        /// </summary>
        /// <param name="table">当前数据操作对象</param>
        /// <param name="condition">当前场景的执行条件</param>
        /// <param name="code">写入SQL的文本构造器</param>
        /// <returns></returns>
        void AfterUpdateSql<TEntity>(IDataTable<TEntity> table, string condition, StringBuilder code) where TEntity : EditDataObject, new();

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="conditions">当前场景的执行条件,互相使用AND</param>
        /// <returns></returns>
        void ContitionSqlCode<TEntity>(List<string> conditions);

        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        void OnPrepareSave(EditDataObject entity, DataOperatorType operatorType);

        /// <summary>
        ///     保存完成后期处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        void OnDataSaved(EditDataObject entity, DataOperatorType operatorType);

        /// <summary>
        ///     更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="entityId">实体类型ID</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        void OnOperatorExecuting(int entityId, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType);

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="entityId">实体类型ID</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        void OnOperatorExecutd(int entityId, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType);
    }



}