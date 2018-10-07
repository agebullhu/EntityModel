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
using MySql.Data.MySqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Gboxt.Common.DataModel.ExtendEvents;
using Newtonsoft.Json;

#endregion

namespace Gboxt.Common.DataModel.MySql
{
    partial class MySqlTable<TData, TMySqlDataBase>
    {
        #region 实体更新

        static MySqlTable()
        {
            DataUpdateHandler.InitType<TData>();
        }

        /// <summary>
        ///     插入新数据
        /// </summary>
        public bool Insert(TData entity)
        {
            using (var scope = TransactionScope.CreateScope(DataBase))
            {
                if (!InsertInner(entity))
                    return false;
                ReLoadInner(entity);
                scope.SetState(true);
            }
            return true;
        }

        /// <summary>
        ///     插入新数据
        /// </summary>
        public int Insert(IEnumerable<TData> entities)
        {
            var datas = entities as TData[] ?? entities.ToArray();
            int cnt = 0;
            using (var scope = TransactionScope.CreateScope(DataBase))
            {
                foreach (var entity in datas)
                {
                    if (InsertInner(entity))
                        ++cnt;
                    else
                        return 0;
                }
                scope.SetState(true);
            }

            foreach (var entity in datas)
                ReLoadInner(entity);
            return cnt;
        }
        /// <summary>
        ///     更新数据
        /// </summary>
        /// <param name="entity">更新数据的实体</param>
        protected bool InsertInner(TData entity)
        {
            PrepareSave(entity, DataOperatorType.Insert);
            using (var cmd = DataBase.CreateCommand())
            {
                var isIdentitySql = SetInsertCommand(entity, cmd);
                MySqlDataBase.TraceSql(cmd);
                if (isIdentitySql)
                {
                    var key = cmd.ExecuteScalar();
                    if (key == DBNull.Value || key == null)
                        return false;
                    entity.SetValue(KeyField, key);
                }
                else
                {
                    if (cmd.ExecuteNonQuery() == 0)
                        return false;
                }
            }
            EndSaved(entity, DataOperatorType.Insert);
            return true;
        }


        /// <summary>
        ///     更新数据
        /// </summary>
        public bool Update(TData entity)
        {
            using (var scope = TransactionScope.CreateScope(DataBase))
            {
                if (!UpdateInner(entity))
                    return false;
                scope.SetState(true);
            }
            ReLoadInner(entity);
            return true;
        }

        /// <summary>
        ///     更新数据
        /// </summary>
        public int Update(IEnumerable<TData> entities)
        {
            var datas = entities as TData[] ?? entities.ToArray();
            int cnt = 0;
            using (var scope = TransactionScope.CreateScope(DataBase))
            {
                foreach (var entity in datas)
                {
                    if (UpdateInner(entity))
                        ++cnt;
                    else
                        return 0;
                }
                scope.SetState(true);
            }

            foreach (var entity in datas)
                ReLoadInner(entity);
            return cnt;
        }

        /// <summary>
        ///     插入数据
        /// </summary>
        /// <param name="entity">插入数据的实体</param>
        private bool UpdateInner(TData entity)
        {
            if (UpdateByMidified && !entity.__EntityStatus.IsModified)
                return false;
            int result;
            PrepareSave(entity, DataOperatorType.Update);
            using (var cmd = DataBase.CreateCommand())
            {
                SetUpdateCommand(entity, cmd);
                MySqlDataBase.TraceSql(cmd);
                cmd.CommandText = $@"{BeforeUpdateSql(PrimaryKeyConditionSQL)}
{UpdateSqlCode}
{AfterUpdateSql(PrimaryKeyConditionSQL)}";

                result = cmd.ExecuteNonQuery();
            }

            if (result <= 0)
            {
                return false;
            }
            EndSaved(entity, DataOperatorType.Update);
            return true;
        }
        /// <summary>
        ///     删除数据
        /// </summary>
        public int Delete(IEnumerable<TData> entities)
        {
            var datas = entities as TData[] ?? entities.ToArray();
            int cnt = 0;
            using (var scope = TransactionScope.CreateScope(DataBase))
            {
                foreach (var entity in datas)
                {
                    if (UpdateInner(entity))
                        ++cnt;
                    else
                        return 0;
                }
                scope.SetState(true);
            }
            return cnt;
        }


        /// <summary>
        ///     删除数据
        /// </summary>
        public bool Delete(TData entity)
        {
            using (var scope = TransactionScope.CreateScope(DataBase))
            {
                if (!DeleteInner(entity))
                    return false;
                scope.SetState(true);
            }
            return true;
        }

        /// <summary>
        ///     删除
        /// </summary>
        private bool DeleteInner(TData entity)
        {
            if (entity == null)
                return false;
            entity.__EntityStatus.IsDelete = true;
            PrepareSave(entity, DataOperatorType.Delete);
            var result = DeleteInner(PrimaryKeyConditionSQL, CreatePimaryKeyParameter(entity));
            if (result == 0)
                return false;
            EndSaved(entity, DataOperatorType.Delete);
            return true;
        }


        /// <summary>
        ///     保存数据
        /// </summary>
        public int Save(IEnumerable<TData> entities)
        {
            var datas = entities as TData[] ?? entities.ToArray();
            int cnt = 0;
            using (var scope = TransactionScope.CreateScope(DataBase))
            {
                foreach (var entity in datas)
                {
                    if (SaveInner(entity))
                        ++cnt;
                    else
                        return 0;
                }
                scope.SetState(true);
            }

            foreach (var entity in datas)
                ReLoadInner(entity);
            return cnt;
        }

        /// <summary>
        ///     保存数据
        /// </summary>
        public bool Save(TData entity)
        {
            using (var scope = TransactionScope.CreateScope(DataBase))
            {
                if (!SaveInner(entity))
                    return false;
                scope.SetState(true);
            }
            return true;
        }

        /// <summary>
        ///     保存
        /// </summary>
        private bool SaveInner(TData entity)
        {
            if (entity.__EntityStatus.IsDelete)
                return DeleteInner(entity);
            if (entity.__EntityStatus.IsNew || !ExistPrimaryKey(entity.GetValue(KeyField)))
                return InsertInner(entity);
            return UpdateInner(entity);
        }

        /// <summary>
        ///     保存前处理(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="operatorType">操作类型</param>
        protected void PrepareSave(TData entity, DataOperatorType operatorType)
        {
            if (!IsBaseClass)
            {
                switch (operatorType)
                {
                    case DataOperatorType.Insert:
                        entity.LaterPeriodByModify(EntitySubsist.Adding);
                        break;
                    case DataOperatorType.Delete:
                        entity.LaterPeriodByModify(EntitySubsist.Deleting);
                        break;
                    case DataOperatorType.Update:
                        entity.LaterPeriodByModify(EntitySubsist.Modify);
                        break;
                }
            }
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
        private void EndSaved(TData entity, DataOperatorType operatorType)
        {
            if (!IsBaseClass)
            {
                switch (operatorType)
                {
                    case DataOperatorType.Insert:
                        entity.OnStatusChanged(NotificationStatusType.Added);
                        break;
                    case DataOperatorType.Delete:
                        entity.OnStatusChanged(NotificationStatusType.Deleted);
                        break;
                    case DataOperatorType.Update:
                        entity.OnStatusChanged(NotificationStatusType.Modified);
                        break;
                }
                entity.AcceptChanged();
            }
            OnDataSaved(entity, operatorType);
            OnEvent(operatorType, entity);
        }
        #endregion

        #region 删除

        /// <summary>
        ///     删除数据
        /// </summary>
        public bool DeletePrimaryKey(object key)
        {
            var cnt = DeleteInner(PrimaryKeyConditionSQL, CreatePimaryKeyParameter(key));
            if (cnt == 0)
                return false;
            OnKeyEvent(DataOperatorType.Delete, key);
            return true;
        }


        /// <summary>
        ///     条件删除
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public int Delete(Expression<Func<TData, bool>> lambda)
        {
            //throw new Exception("批量删除功能被禁用");
            var convert = Compile(lambda);
            return DeleteByCondition(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     物理删除数据
        /// </summary>
        public bool PhysicalDelete(object key)
        {
            var condition = PrimaryKeyConditionSQL;
            var para = CreatePimaryKeyParameter(key);
            var paras = new[] { para };
            using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(condition, paras, DataOperatorType.Delete);
                var result = DataBase.Execute($@"DELETE FROM `{WriteTableName}` WHERE {condition};", para);
                if (result == 0)
                    return false;
                OnOperatorExecutd(condition, paras, DataOperatorType.Delete);
                scope.SetState(true);
            }

            OnKeyEvent(DataOperatorType.Delete, key);
            return true;
        }
        /// <summary>
        ///     强制物理删除
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否删除成功</returns>
        public int PhysicalDelete(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            int cnt;
            using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(convert.ConditionSql, convert.Parameters, DataOperatorType.MulitDelete);
                cnt = DataBase.Execute(CreateDeleteSql(convert), convert.Parameters);
                if (cnt == 0)
                    return 0;
                OnOperatorExecutd(convert.ConditionSql, convert.Parameters, DataOperatorType.MulitDelete);
                scope.SetState(true);
            }
            OnMulitUpdateEvent(DataOperatorType.MulitDelete, convert.ConditionSql, convert.Parameters);
            return cnt;
        }

        /// <summary>
        ///     条件删除
        /// </summary>
        public int Delete(string condition, params MySqlParameter[] args)
        {
            //throw new Exception("批量删除功能被禁用");
            if (string.IsNullOrWhiteSpace(condition))
                throw new ArgumentException(@"删除条件不能为空,因为不允许执行全表删除", GetType().FullName);
            return DeleteByCondition(condition, args);
        }


        /// <summary>
        ///     条件删除
        /// </summary>
        private int DeleteByCondition(string condition, MySqlParameter[] args)
        {
            int cnt;
            using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(condition, args, DataOperatorType.Delete);
                cnt = DeleteInner(condition, args);
                if (cnt == 0)
                    return 0;
                OnOperatorExecutd(condition, args, DataOperatorType.Delete);
                scope.SetState(true);
            }
            OnMulitUpdateEvent(DataOperatorType.MulitDelete, condition, args);
            return cnt;
        }

        /// <summary>
        ///     删除
        /// </summary>
        private int DeleteInner(string condition, params MySqlParameter[] args)
        {
            if (string.IsNullOrEmpty(DeleteSqlCode))
                return 0;
            if (!string.IsNullOrEmpty(condition))
                return DataBase.Execute(CreateDeleteSql(condition), args);
            throw new ArgumentException(@"删除条件不能为空,因为不允许执行全表删除", GetType().FullName);
        }

        #endregion

        #region 精准更新


        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        public int SetValue(string field, object value, object key)
        {
            int re = SetValueInner(field, value, PrimaryKeyConditionSQL, CreatePimaryKeyParameter(key));
            if (re > 0)
                OnKeyEvent(DataOperatorType.Update, key);
            return re;
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="value">值</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        public int SetValue<TField, TKey>(Expression<Func<TData, TField>> fieldExpression, TField value, TKey key)
        {
            int re = SetValueInner(GetPropertyName(fieldExpression), value, PrimaryKeyConditionSQL, CreatePimaryKeyParameter(key));
            if (re > 0)
                OnKeyEvent(DataOperatorType.Update, key);
            return re;
        }

        /// <summary>
        ///     设计字段按自定义表达式更新值
        /// </summary>
        /// <param name="valueExpression">值的SQL方式</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        public int SetCoustomValue<TKey>(string valueExpression, TKey key)
        {
            var condition = PrimaryKeyConditionSQL;
            var sql = CreateUpdateSql(valueExpression, condition);
            var arg2 = new List<MySqlParameter>
            {
                CreateFieldParameter(KeyField, key)
            };
            int result;
            using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(condition, arg2, DataOperatorType.Update);
                result = DataBase.Execute(sql, arg2);
                if (result == 0)
                    return 0;
                OnOperatorExecutd(condition, arg2, DataOperatorType.Update);
                scope.SetState(true);
            }
            OnKeyEvent(DataOperatorType.Delete, key);
            return result;
        }

        #endregion

        #region 条件更新

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">更新字段</param>
        /// <param name="value">更新值</param>
        /// <param name="conditionFiles">条件包含的字段</param>
        /// <param name="conditionValues">条件包含的值</param>
        /// <returns>更新行数</returns>
        /// <remarks>
        /// 条件中使用AND组合,均为等于
        /// </remarks>
        public void SaveValue(string field, object value, string[] conditionFiles, object[] conditionValues)
        {
            var args = CreateFieldsParameters(conditionFiles, conditionValues);
            var condition = FieldConditionSQL(true, conditionFiles);
            SetValueByCondition(field, value, condition, args);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        public int SetValue(string field, object value, string condition, params MySqlParameter[] args)
        {
            return SetValueByCondition(field, value, condition, args);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        public int SetValue(Expression<Func<TData, bool>> field, bool value, string condition, params MySqlParameter[] args)
        {
            return SetValueByCondition(GetPropertyName(field), value ? 0 : 1, condition, args);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        public int SetValue(Expression<Func<TData, Enum>> field, Enum value, string condition,
            params MySqlParameter[] args)
        {
            return SetValueByCondition(GetPropertyName(field), Convert.ToInt32(value), condition, args);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value, string condition,
            params MySqlParameter[] args)
        {
            return SetValueByCondition(GetPropertyName(field), value, condition, args);
        }

        /// <summary>
        ///     全量更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>更新行数</returns>
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value)
        {
            return SetValueByCondition(GetPropertyName(field), value, "-1", null);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value, Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return SetValueByCondition(GetPropertyName(field), value, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        private int SetValueByCondition(string field, object value, string condition, params MySqlParameter[] args)
        {
            int result = DoUpdateValue(field, value, condition, args);
            OnMulitUpdateEvent(DataOperatorType.MulitUpdate, condition, args);
            return result;
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        private int SetValueInner(string field, object value, string condition, params MySqlParameter[] args)
        {
            return DoUpdateValue(field, value, condition, args);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        private int DoUpdateValue(string field, object value, string condition, MySqlParameter[] args)
        {
            field = FieldDictionary[field];

            var arg2 = new List<MySqlParameter>();
            if (args != null)
                arg2.AddRange(args);
            var sql = CreateUpdateSql(field, value, condition, arg2);

            int result;
            using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(condition, args, DataOperatorType.Update);
                result = DataBase.Execute(sql, arg2.ToArray());
                if (result <= 0)
                    return 0;
                OnOperatorExecutd(condition, args, DataOperatorType.MulitUpdate);
                scope.SetState(true);
            }

            return result;
        }

        /// <summary>
        ///     自定义更新（更新表达式自写）
        /// </summary>
        /// <param name="expression">更新SQL表达式</param>
        /// <param name="condition">条件</param>
        /// <returns>更新行数</returns>
        public int SetValue(string expression, Expression<Func<TData, bool>> condition)
        {
            var convert = Compile(condition);
            return SetMulitValue(expression, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     自定义更新（更新表达式自写）
        /// </summary>
        /// <param name="expression">更新SQL表达式</param>
        /// <param name="condition">条件</param>
        /// <param name="args">参数</param>
        /// <returns>更新行数</returns>
        public int SetMulitValue(string expression, Expression<Func<TData, bool>> condition, params MySqlParameter[] args)
        {
            var convert = Compile(condition);
            var arg = new List<MySqlParameter>();
            if (convert.HaseParameters)
                arg.AddRange(convert.MySqlParameter);
            if (args.Length > 0)
                arg.AddRange(args);
            return SetMulitValue(expression, convert.ConditionSql, arg.ToArray());
        }

        /// <summary>
        ///     自定义更新（更新表达式自写）
        /// </summary>
        /// <param name="expression">更新SQL表达式</param>
        /// <param name="condition">条件</param>
        /// <param name="args">参数</param>
        /// <returns>更新行数</returns>
        public int SetMulitValue(string expression, string condition, MySqlParameter[] args)
        {
            var sql = CreateUpdateSql(expression, condition);
            int result;
            using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(condition, args, DataOperatorType.MulitUpdate);

                result = DataBase.Execute(sql, args);
                if (result == 0)
                    return 0;
                OnOperatorExecutd(condition, args, DataOperatorType.MulitUpdate);
                scope.SetState(true);
            }
            return result;
        }

        #endregion

        #region Sql注入

        /// <summary>
        ///     与更新同时执行的SQL(更新之前立即执行)
        /// </summary>
        /// <param name="condition">当前场景的执行条件</param>
        /// <returns></returns>
        private string BeforeUpdateSql(string condition)
        {
            var code = new StringBuilder();
            BeforeUpdateSql(code, condition);
            DataUpdateHandler.BeforeUpdateSql(this,code, TableId, condition);
            return code.ToString();
        }

        /// <summary>
        ///     与更新同时执行的SQL(更新之后立即执行)
        /// </summary>
        /// <param name="condition">当前场景的执行条件</param>
        /// <returns></returns>
        private string AfterUpdateSql(string condition)
        {
            var code = new StringBuilder();
            AfterUpdateSql(code, condition);
            DataUpdateHandler.AfterUpdateSql(this, code, TableId, condition);
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

        #region 操作扩展



        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        protected virtual void OnPrepareSave(DataOperatorType operatorType, TData entity)
        {

        }


        /// <summary>
        ///     保存完成后期处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        protected virtual void OnDataSaved(DataOperatorType operatorType, TData entity)
        {

        }


        /// <summary>
        ///    更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        protected virtual void OnOperatorExecuting(DataOperatorType operatorType, string condition, IEnumerable<MySqlParameter> args)
        {
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        protected virtual void OnOperatorExecutd(DataOperatorType operatorType, string condition, IEnumerable<MySqlParameter> args)
        {
        }

        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        private void OnPrepareSave(TData entity, DataOperatorType operatorType)
        {
            OnPrepareSave(operatorType, entity);
            DataUpdateHandler.OnPrepareSave(entity, operatorType);
        }

        /// <summary>
        ///     保存完成后期处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        private void OnDataSaved(TData entity, DataOperatorType operatorType)
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
        private void OnOperatorExecuting(string condition, IEnumerable<MySqlParameter> args, DataOperatorType operatorType)
        {
            var sqlParameters = args as MySqlParameter[] ?? args.ToArray();
            OnOperatorExecuting(operatorType, condition, sqlParameters);
            DataUpdateHandler.OnOperatorExecuting(TableId, condition, sqlParameters, operatorType);
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        private void OnOperatorExecutd(string condition, IEnumerable<MySqlParameter> args, DataOperatorType operatorType)
        {
            var mySqlParameters = args as MySqlParameter[] ?? args.ToArray();
            OnOperatorExecutd(operatorType, condition, mySqlParameters);
            DataUpdateHandler.OnOperatorExecutd(TableId, condition, mySqlParameters, operatorType);
        }

        #endregion

        #region 数据更新事件支持

        private bool _globalEvent;

        /// <summary>
        /// 是否允许全局事件(如全局事件器,则永为否)
        /// </summary>
        public virtual bool GlobalEvent
        {
            get => _globalEvent && DataUpdateHandler.EventProxy != null;
            set => _globalEvent = value;
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="operatorType">操作类型</param>
        /// <param name="key">其它参数</param>
        private void OnKeyEvent(DataOperatorType operatorType, object key)
        {
            if (GlobalEvent)
                DataUpdateHandler.EventProxy.OnStatusChanged(DataBase.Name, Name, operatorType, EntityEventValueType.Key, key?.ToString());
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        private void OnMulitUpdateEvent(DataOperatorType operatorType, string condition, MySqlParameter[] args)
        {
            if (!GlobalEvent)
                return;
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
            DataUpdateHandler.EventProxy.OnStatusChanged(DataBase.Name, Name, operatorType, EntityEventValueType.QueryCondition, JsonConvert.SerializeObject(queryCondition));
        }
        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="operatorType">操作类型</param>
        /// <param name="entity">其它参数</param>
        private void OnEvent(DataOperatorType operatorType, TData entity)
        {
            if (GlobalEvent)
                DataUpdateHandler.EventProxy.OnStatusChanged(DataBase.Name, Name, operatorType, EntityEventValueType.EntityJson, JsonConvert.SerializeObject(entity));
        }

        #endregion

    }
}