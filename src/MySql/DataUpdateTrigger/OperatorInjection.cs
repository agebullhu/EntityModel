// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-16
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Common;
using Agebull.EntityModel.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    /// 操作注入
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class OperatorInjection<TEntity> : IOperatorInjection<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 构造
        /// </summary>
        public OperatorInjection()
        {
            DataUpdateHandler = ServiceProvider.GetService<IDataUpdateHandler>();
            DataUpdateHandler?.InitType<TEntity>();
        }

        /// <summary>
        /// 依赖对象
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 配置对象
        /// </summary>
        public DataAccessOption<TEntity> Option { get; set; }

        #region 操作注入

        /// <summary>
        /// 数据更新事件处理器
        /// </summary>
        public IDataUpdateHandler DataUpdateHandler { get; set; }

        /// <summary>
        /// 不做代码注入
        /// </summary>
        public bool NoInjection => Option.NoInjection;

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public void InjectionCondition(List<string> conditions)
        {
            if (!string.IsNullOrEmpty(Option.BaseCondition))
                conditions.Add(Option.BaseCondition);
            InjectionConditionInner(conditions);
            DataUpdateHandler.InjectionCondition(Option, conditions);

        }
        /// <summary>
        ///     与更新同时执行的SQL(更新之前立即执行)
        /// </summary>
        /// <param name="condition">当前场景的执行条件</param>
        /// <returns></returns>
        public string BeforeUpdateSql(string condition)
        {
            var code = new StringBuilder();
            BeforeUpdateSql(code, condition);
            DataUpdateHandler.BeforeUpdateSql(Option, code, condition);
            return code.ToString();
        }

        /// <summary>
        ///     与更新同时执行的SQL(更新之后立即执行)
        /// </summary>
        /// <param name="condition">当前场景的执行条件</param>
        /// <returns></returns>
        public string AfterUpdateSql(string condition)
        {
            var code = new StringBuilder();
            AfterUpdateSql(code, condition);
            DataUpdateHandler.AfterUpdateSql(Option, code, condition);
            return code.ToString();
        }

        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        public void PrepareSave(TEntity entity, DataOperatorType operatorType)
        {
            if (NoInjection)
                return;
            OnPrepareSave(operatorType, entity);
            DataUpdateHandler.OnPrepareSave(entity, operatorType);
        }

        /// <summary>
        ///     保存完成后期处理(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="operatorType">操作类型</param>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        public void EndSaved(TEntity entity, DataOperatorType operatorType)
        {
            if (NoInjection)
            {
                return;
            }
            OnDataSaved(operatorType, entity);
            DataUpdateHandler.OnDataSaved(entity, operatorType);
        }

        /// <summary>
        ///     更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="parameter">参数值</param>
        /// <param name="operatorType">操作类型</param>
        public void OnOperatorExecuting(string condition, DbParameter[] parameter, DataOperatorType operatorType)
        {
            if (NoInjection)
                return;
            OnOperatorExecuting(operatorType, condition, parameter);
            DataUpdateHandler.OnOperatorExecuting(Option, condition, parameter, operatorType);
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="parameter">参数值</param>
        /// <param name="operatorType">操作类型</param>
        public void OnOperatorExecuted(string condition, DbParameter[] parameter, DataOperatorType operatorType)
        {
            if (NoInjection)
                return;
            OnOperatorExecuted(operatorType, condition, parameter);
            DataUpdateHandler.OnOperatorExecuted(Option, condition, parameter, operatorType);
        }

        #endregion

        #region 扩展方法


        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        protected virtual void InjectionConditionInner(List<string> conditions)
        {
        }

        /// <summary>
        ///     得到可正确更新的条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public virtual void CheckUpdateContition(ref string condition)
        {
        }

        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        protected virtual void OnPrepareSave(DataOperatorType operatorType, TEntity entity)
        {

        }


        /// <summary>
        ///     保存完成后期处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        protected virtual void OnDataSaved(DataOperatorType operatorType, TEntity entity)
        {

        }


        /// <summary>
        ///    更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="parameter">参数值</param>
        /// <param name="operatorType">操作类型</param>
        protected virtual void OnOperatorExecuting(DataOperatorType operatorType, string condition, DbParameter[] parameter)
        {
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="parameter">参数值</param>
        /// <param name="operatorType">操作类型</param>
        protected virtual void OnOperatorExecuted(DataOperatorType operatorType, string condition, DbParameter[] parameter)
        {
        }


        /// <summary>
        ///     与更新同时执行的SQL(更新之前立即执行)
        /// </summary>
        /// <param name="code">写入SQL的文本构造器</param>
        /// <param name="condition">当前场景的执行条件</param>
        /// <returns></returns>
        protected virtual void BeforeUpdateSql(StringBuilder code, string condition)
        {
        }

        /// <summary>
        ///     与更新同时执行的SQL(更新之后立即执行)
        /// </summary>
        /// <param name="code">写入SQL的文本构造器</param>
        /// <param name="condition">当前场景的执行条件</param>
        protected virtual void AfterUpdateSql(StringBuilder code, string condition)
        {
        }

        #endregion


        #region 事件 

        /// <summary>
        /// 是否允许全局事件(如全局事件器,则永为否)
        /// </summary>
        public bool CanRaiseEvent => Option.CanRaiseEvent && DataUpdateHandler != null;

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="operatorType">操作类型</param>
        /// <param name="key">其它参数</param>
        public Task OnKeyEvent(DataOperatorType operatorType, object key)
        {
            if (!CanRaiseEvent)
                return Task.CompletedTask;
            return DataUpdateHandler.OnStatusChanged(Option.DataSturct.ProjectName, Option.DataSturct.EntityName, operatorType, EntityEventValueType.Key, key?.ToString());
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        public Task OnMulitUpdateEvent(DataOperatorType operatorType, string condition, DbParameter[] parameter)
        {
            if (!CanRaiseEvent)
                return Task.CompletedTask;
            var queryCondition = new MulitCondition
            {
                Condition = condition,
                Parameters = new ConditionParameter[parameter.Length]
            };
            for (int i = 0; i < parameter.Length; i++)
            {
                queryCondition.Parameters[i] = new ConditionParameter
                {
                    Name = parameter[i].ParameterName,
                    Value = parameter[i].Value == DBNull.Value ? null : parameter[i].Value.ToString(),
                    Type = parameter[i].DbType
                };
            }
            return DataUpdateHandler.OnStatusChanged(Option.DataSturct.ProjectName, Option.DataSturct.EntityName, operatorType, EntityEventValueType.QueryCondition, queryCondition);
        }

        /// <summary>
        ///     保存完成后期处理(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="operatorType">操作类型</param>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        public Task OnEndSavedEvent(TEntity entity, DataOperatorType operatorType)
        {
            if (CanRaiseEvent)
                return DataUpdateHandler.OnStatusChanged(Option.DataSturct.ProjectName, Option.DataSturct.EntityName, operatorType, EntityEventValueType.EntityJson, entity);
            return Task.CompletedTask;
        }

        #endregion
    }
}