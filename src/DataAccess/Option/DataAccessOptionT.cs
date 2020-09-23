// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Events;
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
        where TEntity : class, new()
    {
        #region 数据库

        /// <summary>
        /// 数据更新事件处理器
        /// </summary>
        public IDataUpdateHandler DataUpdateHandler { get; set; }

        /// <summary>
        /// 按修改更新
        /// </summary>
        public bool UpdateByMidified { get; set; }

        /// <summary>
        /// 自动构建数据库对象
        /// </summary>
        public IDataOperator<TEntity> DataOperator { get; set; }

        /// <summary>
        /// 自动构建数据库对象
        /// </summary>
        public Func<IDataBase> CreateDataBase { get; set; }

        /// <summary>
        ///     数据访问对象
        /// </summary>
        public DataAccess<TEntity> DataAccess { get; set; }

        /// <summary>
        ///     Sql构造器
        /// </summary>
        public ISqlBuilder<TEntity> SqlBuilder { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType { get; set; }


        /// <summary>
        /// 构造
        /// </summary>
        public void Init()
        {
            FieldMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            PropertyMap = new Dictionary<string, EntitiyProperty>(StringComparer.OrdinalIgnoreCase);
            var properties = Properties;
            foreach (var pro in properties)
            {
                if (!pro.Featrue.HasFlag(PropertyFeatrue.DbCloumn))
                    continue;
                PropertyMap[pro.ColumnName] = pro;
                PropertyMap[pro.PropertyName] = pro;

                FieldMap[pro.ColumnName] = pro.ColumnName;
                FieldMap[pro.PropertyName] = pro.ColumnName;
            }

            LoadFields ??= SqlBuilder.BuilderLoadFields();
            UpdateFields ??= SqlBuilder.BuilderUpdateFields();
            InsertSqlCode ??= SqlBuilder.BuilderInsertSqlCode();
            DeleteSqlCode ??= SqlBuilder.BuilderDeleteSqlCode();
        }

        #endregion

        #region 迭代

        /// <summary>
        /// 迭代循环属性
        /// </summary>
        public void FroeachProperties(PropertyFeatrue propertyFeatrue, Action<EntitiyProperty> action)
        {
            var properties = Properties;

            foreach (var pro in properties)
            {
                if (pro.Featrue.HasFlag(propertyFeatrue))
                    action(pro);
            }
        }

        /// <summary>
        /// 迭代循环属性
        /// </summary>
        public void FroeachProperties(PropertyFeatrue propertyFeatrue, ReadWriteFeatrue readWrite, Action<EntitiyProperty> action)
        {
            var properties = Properties;

            foreach (var pro in properties)
            {
                if (pro.Featrue.HasFlag(propertyFeatrue) && pro.DbReadWrite.HasFlag(readWrite))
                    action(pro);
            }
        }

        /// <summary>
        /// 迭代循环属性
        /// </summary>
        public void FroeachDbProperties(Action<EntitiyProperty> action)
        {
            var properties = Properties;

            foreach (var pro in properties)
            {
                if (pro.Featrue.HasFlag(PropertyFeatrue.Property | PropertyFeatrue.DbCloumn))
                    action(pro);
            }
        }

        /// <summary>
        /// 迭代循环属性
        /// </summary>
        public void FroeachDbProperties(ReadWriteFeatrue readWrite, Action<EntitiyProperty> action)
        {
            var properties = Properties;

            foreach (var pro in properties)
            {
                if (pro.Featrue.HasFlag(PropertyFeatrue.Property | PropertyFeatrue.DbCloumn) && pro.DbReadWrite.HasFlag(readWrite))
                    action(pro);
            }
        }

        /// <summary>
        /// 迭代循环属性
        /// </summary>
        public async Task FroeachDbProperties(ReadWriteFeatrue readWrite, Func<EntitiyProperty, Task> action)
        {
            var properties = Properties;

            foreach (var pro in properties)
            {
                if (pro.Featrue.HasFlag(PropertyFeatrue.Property | PropertyFeatrue.DbCloumn) && pro.DbReadWrite.HasFlag(readWrite))
                    await action(pro);
            }
        }
        #endregion


        #region 方法实现

        public string GetUpdateSql(TEntity entity)
        {
            if (UpdateByMidified)
            {
                return SqlBuilder.GetModifiedUpdateSql(entity);
            }
            else
            {
                return UpdateFields;
            }
        }

        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public virtual Task LoadEntity(DbDataReader reader, TEntity entity)
        {
            return FroeachDbProperties(ReadWriteFeatrue.Read, async pro =>
              {
                  if (await reader.IsDBNullAsync(pro.ColumnName))
                  {
                      DataOperator.SetValue(entity, pro.PropertyName, null);
                  }
                  else
                  {
                      DataOperator.SetValue(entity, pro.PropertyName, reader.GetValue(pro.ColumnName));
                  }
              });
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

            FroeachDbProperties(ReadWriteFeatrue.Read,
                pro => cmd.Parameters.Add(SqlBuilder.CreateFieldParameter(pro.PropertyName,
                            pro.DbType,
                            DataOperator.GetValue(entity, pro.PropertyName)))
            );
        }

        /// <summary>
        ///     得到字段的MySqlDbType类型
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>参数</returns>
        public virtual int GetDbType(string field)
        {
            return PropertyMap.TryGetValue(field, out var pro)
                ? pro.DbType
                : 0;
        }

        #endregion

        #region Sql注入

        /// <summary>
        /// 不做代码注入
        /// </summary>
        public bool NoInjection { get; set; }

        /// <summary>
        ///     载入数据后处理
        /// </summary>
        /// <param name="entity">读取数据的实体</param>
        public virtual void EntityLoaded(TEntity entity)
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
            DataUpdateHandler.InjectionCondition(this, conditions);

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

        /// <summary>
        ///     得到可正确更新的条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public virtual void CheckUpdateContition(ref string condition)
        {
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
            DataUpdateHandler.BeforeUpdateSql(this, code, condition);
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
            DataUpdateHandler.AfterUpdateSql(this, code, condition);
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


        bool _canRaiseEvent;
        /// <summary>
        /// 是否允许全局事件(如全局事件器,则永为否)
        /// </summary>
        public bool CanRaiseEvent
        {
            get => _canRaiseEvent && DataUpdateHandler != null;
            set => _canRaiseEvent = value;
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="operatorType">操作类型</param>
        /// <param name="key">其它参数</param>
        public Task OnKeyEvent(DataOperatorType operatorType, object key)
        {
            if (!CanRaiseEvent)
                return Task.CompletedTask;
            return DataUpdateHandler.OnStatusChanged(DataSturct.ProjectName, DataSturct.EntityName, operatorType, EntityEventValueType.Key, key?.ToString());
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        public Task OnMulitUpdateEvent(DataOperatorType operatorType, string condition, DbParameter[] args)
        {
            if (!CanRaiseEvent)
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
            return DataUpdateHandler.OnStatusChanged(DataSturct.ProjectName, DataSturct.EntityName, operatorType, EntityEventValueType.QueryCondition, queryCondition);
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
            OnDataSaved(entity, operatorType);
            if (!CanRaiseEvent)
                return Task.CompletedTask;
            return DataUpdateHandler.OnStatusChanged(DataSturct.ProjectName, DataSturct.EntityName, operatorType, EntityEventValueType.EntityJson, entity);
        }

        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        public void PrepareSave(TEntity entity, DataOperatorType operatorType)
        {
            OnPrepareSave(operatorType, entity);
            if (!CanRaiseEvent)
                return;
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
            if (!CanRaiseEvent)
                return;
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
            if (!CanRaiseEvent)
                return;
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
            if (!CanRaiseEvent)
                return;
            DataUpdateHandler.OnOperatorExecuted(this, condition, mySqlParameters, operatorType);
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

        #endregion

        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="data">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        public void SetParameterValue(TEntity data, DbCommand cmd)
        {
            FroeachDbProperties(pro => cmd.Parameters[pro.PropertyName].Value = DataOperator.GetValue(data, pro.PropertyName));
        }

    }
}