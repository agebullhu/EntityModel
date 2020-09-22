// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 数据载入配置
    /// </summary>
    public abstract class DataAccessOption<TEntity> : DataAccessOption
        where TEntity : class, IDataObject, new()
    {
        #region 数据结构

        /// <summary>
        ///     是否作为基类存在的
        /// </summary>
        public bool IsBaseClass { get; set; }

        /// <summary>
        ///     数据访问对象
        /// </summary>
        public DataAccess<TEntity> DataAccess { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => DataAccess.DataBaseType;

        /// <summary>
        ///     Sql构造器
        /// </summary>
        public ISqlBuilder<TEntity> SqlBuilder => DataAccess.SqlBuilder;

        #endregion

        #region 方法实现


        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public async Task LoadEntity(DbDataReader reader, TEntity entity)
        {
            using (new EntityLoadScope(entity))
            {
                await ReadEntity(reader, entity);
                var entity2 = EntityLoaded(entity);
                if (entity != entity2)
                    entity.CopyValue(entity2);
            }
        }

        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        protected virtual async Task ReadEntity(DbDataReader reader, TEntity entity)
        {
            foreach (var pro in DataSturct.Properties.Values.Where(p => p.Featrue.HasFlag(PropertyFeatrue.DbCloumn)))
            {
                if (!await reader.IsDBNullAsync(pro.ColumnName))
                    entity.SetValue(pro.PropertyName, reader.GetValue(pro.ColumnName));
            }
        }

        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        public virtual void SetEntityParameter(DbCommand cmd, TEntity entity)
        {
            cmd.Parameters.Clear();
            foreach (var pro in DataSturct.Properties.Values.Where(p => p.Featrue.HasFlag(PropertyFeatrue.DbCloumn)))
            {
                cmd.Parameters.Add(SqlBuilder.CreateFieldParameter(pro.PropertyName, pro.DbType, entity.GetValue(pro.PropertyName)));
            }
        }

        /// <summary>
        ///     得到字段的MySqlDbType类型
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>参数</returns>
        public virtual int GetDbType(string field)
        {
            return DataSturct.PropertyMap.TryGetValue(field, out var pro)
                ? pro.DbType
                : 0;
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
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public void InjectionCondition(List<string> conditions)
        {
            if (!_baseConditionInited)
            {
                InitBaseCondition();
                _baseConditionInited = true;
            }
            if (!string.IsNullOrEmpty(BaseCondition))
                conditions.Add(BaseCondition);
            InjectionConditionInner(conditions);

            //DataUpdateHandler.ConditionSqlCode(this, conditions);

        }

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        protected virtual void InjectionConditionInner(List<string> conditions)
        {
        }

        /// <summary>
        /// 基本条件初始化完成的标识
        /// </summary>
        private bool _baseConditionInited;

        /// <summary>
        ///  初始化基本条件
        /// </summary>
        /// <returns></returns>
        protected virtual void InitBaseCondition()
        {
        }

        #endregion

        #region Sql注入

        /// <summary>
        ///     与更新同时执行的SQL(更新之前立即执行)
        /// </summary>
        /// <param name="condition">当前场景的执行条件</param>
        /// <returns></returns>
        public string BeforeUpdateSql(string condition)
        {
            if (NoInjection)
                return "";
            var code = new StringBuilder();
            BeforeUpdateSql(code, condition);
            //DataUpdateHandler.BeforeUpdateSql(Table, code, condition);
            return code.ToString();
        }

        /// <summary>
        ///     与更新同时执行的SQL(更新之后立即执行)
        /// </summary>
        /// <param name="condition">当前场景的执行条件</param>
        /// <returns></returns>
        public string AfterUpdateSql(string condition)
        {
            if (NoInjection)
                return "";
            var code = new StringBuilder();
            AfterUpdateSql(code, condition);
            //DataUpdateHandler.AfterUpdateSql(Table, code, condition);
            return code.ToString();
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

        #region 扩展 

        /// <summary>
        ///     保存前处理(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="operatorType">操作类型</param>
        public void PrepareSave(TEntity entity, DataOperatorType operatorType)
        {
            //if (!IsBaseClass)
            //{
            //    switch (operatorType)
            //    {
            //        case DataOperatorType.Insert:
            //            entity.LaterPeriodByModify(EntitySubsist.Adding);
            //            break;
            //        case DataOperatorType.Delete:
            //            entity.LaterPeriodByModify(EntitySubsist.Deleting);
            //            break;
            //        case DataOperatorType.Update:
            //            entity.LaterPeriodByModify(EntitySubsist.Modify);
            //            break;
            //    }
            //}
            OnPrepareSave(entity, operatorType);
        }

        /// <summary>
        ///     保存完成后期处理(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="operatorType">操作类型</param>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        public Task EndSaved(TEntity entity, DataOperatorType operatorType)
        {
            //if (!IsBaseClass)
            //{
            //    switch (operatorType)
            //    {
            //        case DataOperatorType.Insert:
            //            entity.OnStatusChanged(NotificationStatusType.Added);
            //            break;
            //        case DataOperatorType.Delete:
            //            entity.OnStatusChanged(NotificationStatusType.Deleted);
            //            break;
            //        case DataOperatorType.Update:
            //            entity.OnStatusChanged(NotificationStatusType.Modified);
            //            break;
            //    }
            //    entity.__status.AcceptChanged();
            //}
            OnDataSaved(entity, operatorType);
            return OnEvent(operatorType, entity);
        }
        #endregion

        #region 操作扩展



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
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        protected virtual void OnOperatorExecuting(DataOperatorType operatorType, string condition, IEnumerable<DbParameter> args)
        {
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        protected virtual void OnOperatorExecuted(DataOperatorType operatorType, string condition, IEnumerable<DbParameter> args)
        {
        }

        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        public void OnPrepareSave(TEntity entity, DataOperatorType operatorType)
        {
            OnPrepareSave(operatorType, entity);
            DataUpdateHandler.OnPrepareSave(entity, operatorType);
        }

        /// <summary>
        ///     保存完成后期处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        public void OnDataSaved(TEntity entity, DataOperatorType operatorType)
        {
            OnDataSaved(operatorType, entity);
            DataUpdateHandler.OnDataSaved(entity, operatorType);
        }

        /// <summary>
        ///     更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        public void OnOperatorExecuting(string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
        {
            var sqlParameters = args as DbParameter[] ?? args.ToArray();
            OnOperatorExecuting(operatorType, condition, sqlParameters);
            DataUpdateHandler.OnOperatorExecuting(this, condition, sqlParameters, operatorType);
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        public void OnOperatorExecuted(string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
        {
            var mySqlParameters = args as DbParameter[] ?? args.ToArray();
            OnOperatorExecuted(operatorType, condition, mySqlParameters);
            DataUpdateHandler.OnOperatorExecuted(this, condition, mySqlParameters, operatorType);
        }

        #endregion

        #region 数据更新事件支持

        /// <summary>
        /// 是否允许全局事件(如全局事件器,则永为否)
        /// </summary>
        public bool GlobalEvent
        {
            get;
            set;
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="operatorType">操作类型</param>
        /// <param name="key">其它参数</param>
        public Task OnKeyEvent(DataOperatorType operatorType, object key)
        {
            if (!GlobalEvent)
                return Task.CompletedTask;
            return DataUpdateHandler.OnStatusChanged(DataSturct.ProjectName, DataSturct.EntityName, operatorType, EntityEventValueType.Key, key?.ToString());
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        public Task OnMulitUpdateEvent(DataOperatorType operatorType, string condition, DbParameter[] args)
        {
            if (!GlobalEvent)
                return Task.CompletedTask;
            var queryCondition = new MulitCondition
            {
                Condition = condition,
                Parameters = new ConditionParameter[args.Length]
            };
            for (int i = 0; i < args.Length; i++)
            {
                queryCondition.Parameters[i] = new ConditionParameter
                {
                    Name = args[i].ParameterName,
                    Value = args[i].Value == DBNull.Value ? null : args[i].Value.ToString(),
                    Type = args[i].DbType
                };
            }
            return DataUpdateHandler.OnStatusChanged(DataSturct.ProjectName, DataSturct.EntityName, operatorType, EntityEventValueType.QueryCondition, JsonConvert.SerializeObject(queryCondition));
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作)
        /// </summary>
        /// <param name="operatorType">操作类型</param>
        /// <param name="entity">其它参数</param>
        public Task OnEvent(DataOperatorType operatorType, TEntity entity)
        {
            if (!GlobalEvent)
                return Task.CompletedTask;
            return DataUpdateHandler.OnStatusChanged(DataSturct.ProjectName, DataSturct.EntityName, operatorType, EntityEventValueType.EntityJson, JsonConvert.SerializeObject(entity));
        }

        #endregion

        /// <summary>
        /// 当前上下文的读取器
        /// </summary>
        public Action<DbDataReader, TEntity> DynamicLoadAction { get; set; }

        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="data">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        public void SetParameterValue(TEntity data, DbCommand cmd)
        {
            foreach (var pro in DataSturct.PropertyMap.Values)
            {
                if (pro.Featrue.HasFlag(PropertyFeatrue.DbCloumn))
                    continue;
                cmd.Parameters[pro.PropertyName].Value = data.GetValue(pro.PropertyName);
            }
        }


        /// <summary>
        ///     数据载入时给外部的处理方法
        /// </summary>
        public Action<TEntity> OnLoadAction;

        /// <summary>
        ///     载入数据
        /// </summary>
        /// <param name="entity">读取数据的实体</param>
        public TEntity EntityLoaded(TEntity entity)
        {
            entity = OnEntityLoaded(entity);
            OnLoadAction?.Invoke(entity);
            return entity;
        }

        /// <summary>
        ///     载入后的同步处理
        /// </summary>
        /// <param name="entity"></param>
        protected virtual TEntity OnEntityLoaded(TEntity entity)
        {
            return entity;
        }
    }
}