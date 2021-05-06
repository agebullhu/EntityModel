/*****************************************************
(c)2016-2021 by ZeroTeam
作者: 胡天水
工程: Agebull.EntityModel.Core Agebull.EntityModel.Core
建立: 忘了日期
修改: -
*****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DbOperatorContext = Agebull.EntityModel.Common.DbOperatorContext<System.Data.Common.DbCommand>;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     Sql实体访问类
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    public sealed partial class DataAccess<TEntity> : DataQuery<TEntity>
         where TEntity : class, new()
    {
        #region 构造

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="provider"></param>
        public DataAccess(DataAccessProvider<TEntity> provider)
            : base(provider)
        {
        }

        #endregion

        #region 实体更新

        /// <summary>
        ///     插入新数据
        /// </summary>
        public async Task<bool> InsertAsync(TEntity entity, bool reload = false)
        {
            if (!await InsertInnerAsync(entity))
                return false;
            if (reload)
                await ReLoadInnerAsync(entity);
            return true;
        }

        /// <summary>
        ///     插入新数据
        /// </summary>
        public async Task<int> InsertAsync(IEnumerable<TEntity> entities, bool reload = false)
        {
            var datas = entities as TEntity[] ?? entities.ToArray();
            int cnt = 0;
            foreach (var entity in datas)
            {
                if (await InsertInnerAsync(entity))
                    ++cnt;
                else
                    return 0;
            }
            if (!reload)
                return cnt;
            foreach (var entity in datas)
                await ReLoadInnerAsync(entity);
            return cnt;
        }

        /// <summary>
        ///     更新数据
        /// </summary>
        /// <param name="entity">更新数据的实体</param>
        private async Task<bool> InsertInnerAsync(TEntity entity)
        {
            await using var connectionScope = await DataBase.CreateConnectionScope();
            await Provider.Injection?.BeforeSave(entity, DataOperatorType.Insert);
            {
                var sql = SqlBuilder.CreateInsertSqlCode();
                await using var cmd = connectionScope.CreateCommand(sql);

                DataOperator.SetEntityParameter(cmd, entity);
                DataBase.TraceSql(cmd);
                if (Option.IsIdentity)
                {
                    var key = await cmd.ExecuteScalarAsync();
                    if (key == DBNull.Value || key == null)
                        return false;
                    Provider.EntityOperator.SetValue(entity, Option.PrimaryProperty, key);
                }
                else
                {
                    if (await cmd.ExecuteNonQueryAsync() == 0)
                        return false;
                }
            }
            await DataOperator.AfterSave(this, entity, DataOperatorType.Insert);
            if (Provider.Injection != null)
                await Provider.Injection.AfterSave(entity, DataOperatorType.Insert);

            return true;
        }


        /// <summary>
        ///     更新数据
        /// </summary>
        public async Task<bool> UpdateAsync(TEntity entity, bool reload = false)
        {
            await using var connectionScope = await DataBase.CreateConnectionScope();
            if (!await UpdateInnerAsync(entity))
                return false;
            if (reload)
                await ReLoadInnerAsync(entity);
            return true;
        }

        /// <summary>
        ///     更新数据
        /// </summary>
        public async Task<int> UpdateAsync(IEnumerable<TEntity> entities, bool reload = false)
        {
            var datas = entities as TEntity[] ?? entities.ToArray();
            int cnt = 0;
            await using var connectionScope = await DataBase.CreateConnectionScope();
            foreach (var entity in datas)
            {
                if (await UpdateInnerAsync(entity))
                    ++cnt;
            }
            if (reload)
                foreach (var entity in datas)
                    await ReLoadInnerAsync(entity);
            return cnt;
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        public async Task<int> DeleteAsync(IEnumerable<TEntity> entities)
        {
            var datas = entities as TEntity[] ?? entities.ToArray();
            int cnt = 0;
            await using var connectionScope = await DataBase.CreateConnectionScope();
            foreach (var entity in datas)
            {
                if (await UpdateInnerAsync(entity))
                    ++cnt;
                else
                    return 0;
            }
            return cnt;
        }


        /// <summary>
        ///     删除数据
        /// </summary>
        public async Task<bool> DeleteAsync(TEntity entity)
        {
            return await DeleteInnerAsync(entity);
        }

        /// <summary>
        ///     删除
        /// </summary>
        private async Task<bool> DeleteInnerAsync(TEntity entity)
        {
            if (entity == null)
                return false;
            if (Provider.Injection != null)
                await Provider.Injection.BeforeSave(entity, DataOperatorType.Delete);
            var para = ParameterCreater.CreateParameter(Option.PrimaryProperty,
                Provider.EntityOperator.GetValue(entity, Option.PrimaryProperty),
                DataOperator.GetDbType(Option.PrimaryProperty));
            var result = await DeleteInnerAsync(SqlBuilder.PrimaryKeyCondition, para);
            if (result == 0)
                return false;
            await DataOperator.AfterSave(this, entity, DataOperatorType.Delete);
            if (Provider.Injection != null)
                await Provider.Injection.AfterSave(entity, DataOperatorType.Delete);
            return true;
        }


        /// <summary>
        ///     保存数据
        /// </summary>
        public async Task<int> SaveAsync(IEnumerable<TEntity> entities, bool reload = false)
        {
            int cnt = 0;
            await using var connectionScope = await DataBase.CreateConnectionScope();
            var datas = entities as TEntity[] ?? entities.ToArray();
            foreach (var entity in datas)
            {
                if (await SaveAsync(entity))
                    ++cnt;
                else
                    return 0;
            }
            if (reload)
                foreach (var entity in datas)
                    await ReLoadInnerAsync(entity);
            return cnt;
        }

        /// <summary>
        ///     保存数据
        /// </summary>
        public async Task<bool> SaveAsync(TEntity entity, bool reload = false)
        {
            if (!await SaveInnerAsync(entity))
                return false;
            if (reload)
                await ReLoadInnerAsync(entity);
            return true;
        }

        /// <summary>
        ///     保存
        /// </summary>
        private async Task<bool> SaveInnerAsync(TEntity entity)
        {
            if (await ExistPrimaryKeyAsync(Provider.EntityOperator.GetValue(entity, Option.PrimaryProperty)))
            {
                return await UpdateInnerAsync(entity);
            }
            else
            {
                return await InsertInnerAsync(entity);
            }
        }


        /// <summary>
        ///     更新数据
        /// </summary>
        /// <param name="entity">更新数据的实体</param>
        private async Task<bool> UpdateInnerAsync(TEntity entity)
        {
            if (Provider.Injection != null)
                await Provider.Injection.BeforeSave(entity, DataOperatorType.Update);
            string sql;
            if (Option.UpdateByMidified)
            {
                sql = SqlBuilder.CreateUpdateSqlCode(entity, SqlBuilder.PrimaryKeyCondition);
                if (sql == null)
                    return false;
            }
            else
            {
                sql = Option.UpdateSqlCode;
            }
            await using var connectionScope = await DataBase.CreateConnectionScope();
            {
                await using var cmd = connectionScope.CreateCommand(sql);
                DataOperator.SetEntityParameter(cmd, entity);

                DataBase.TraceSql(cmd);

                if (await cmd.ExecuteNonQueryAsync() <= 0)
                {
                    return false;
                }
            }
            await DataOperator.AfterSave(this, entity, DataOperatorType.Update);
            if (Provider.Injection != null)
                await Provider.Injection.AfterSave(entity, DataOperatorType.Update);

            return true;
        }

        #endregion

        #region 删除

        /// <summary>
        ///     删除数据
        /// </summary>
        public async Task<bool> DeletePrimaryKeyAsync(object key)
        {
            return 1 == await DeleteInnerAsync(SqlBuilder.PrimaryKeyCondition, ParameterCreater.CreateParameter(Option.PrimaryProperty, key));
        }


        /// <summary>
        ///     条件删除
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public Task<int> DeleteAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return DeleteInnerAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     物理删除数据
        /// </summary>
        public async Task<bool> PhysicalDeleteAsync(object key)
        {
            var sql = SqlBuilder.PhysicalDeleteSqlCode(SqlBuilder.PrimaryKeyCondition);
            return 1 == await UpdateValue(DataOperatorType.MulitDelete, sql, SqlBuilder.PrimaryKeyCondition,
                new[] { ParameterCreater.CreateParameter(Option.PrimaryProperty, key) });
        }

        /// <summary>
        ///     强制物理删除
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否删除成功</returns>
        public Task<int> PhysicalDeleteAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            if (string.IsNullOrEmpty(convert.ConditionSql))
                throw new EntityModelDbException(@"删除条件不能为空,因为不允许执行全表删除");
            var sql = SqlBuilder.PhysicalDeleteSqlCode(convert.ConditionSql);
            return UpdateValue(DataOperatorType.MulitDelete, sql, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     条件删除
        /// </summary>
        public Task<int> DeleteAsync(string condition, params DbParameter[] parameters)
        {
            //throw new EntityModelDbException("批量删除功能被禁用");
            if (string.IsNullOrWhiteSpace(condition))
                throw new EntityModelDbException(@"删除条件不能为空,因为不允许执行全表删除");
            return DeleteInnerAsync(condition, parameters);
        }

        /// <summary>
        ///     删除
        /// </summary>
        private Task<int> DeleteInnerAsync(string condition, params DbParameter[] parameters)
        {
            if (string.IsNullOrEmpty(condition))
                throw new EntityModelDbException(@"删除条件不能为空,因为不允许执行全表删除");

            return UpdateValue(DataOperatorType.MulitDelete, SqlBuilder.CreateDeleteSql(condition),
                SqlBuilder.PrimaryKeyCondition, parameters);
        }

        #endregion

        #region 累计更新


        /// <summary>
        ///     累加值
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="value">值</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        public Task<int> AddValueAsync<TField, TKey>(Expression<Func<TEntity, TField>> fieldExpression,
            TField value, TKey key)
        {
            return DoAddValueAsync(GetPropertyName(fieldExpression), value,
                SqlBuilder.PrimaryKeyCondition,
                new[] { ParameterCreater.CreateParameter(Option.PrimaryProperty, key) });
        }

        /// <summary>
        ///     累加值
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="value">值</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        public Task<int> AddValueAsync<TField, TKey>(Expression<Func<TEntity, TField>> fieldExpression,
            TField value, Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);

            return DoAddValueAsync(GetPropertyName(fieldExpression), value,
                convert.ConditionSql, convert.Parameters);
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
        public Task<int> SetValueAsync(string field, object value, object key)
        {
            return SetValueInnerAsync(field, value, SqlBuilder.PrimaryKeyCondition, ParameterCreater.CreateParameter(Option.PrimaryProperty, key));
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="value">值</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        public Task<int> SetValueAsync<TField, TKey>(Expression<Func<TEntity, TField>> fieldExpression,
            TField value, TKey key)
        {
            return SetValueInnerAsync(GetPropertyName(fieldExpression), value, SqlBuilder.PrimaryKeyCondition,
                ParameterCreater.CreateParameter(Option.PrimaryProperty, key));
        }

        /// <summary>
        ///     设计字段按自定义表达式更新值
        /// </summary>
        /// <param name="valueExpression">值的SQL方式</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        public Task<int> SetCoustomValueAsync<TKey>(string valueExpression, TKey key)
        {
            var sql = SqlBuilder.CreateUpdateSqlCode(valueExpression, SqlBuilder.PrimaryKeyCondition);
            return UpdateValue(DataOperatorType.MulitUpdate, sql, SqlBuilder.PrimaryKeyCondition, new[]
            {
                ParameterCreater.CreateParameter(Option.PrimaryProperty, key)
            });
        }

        #endregion

        #region 条件更新

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="parameters">条件参数</param>
        /// <returns>更新行数</returns>
        public Task<int> SetValueAsync(string field, object value, string condition, params DbParameter[] parameters)
        {
            return SetValueByConditionAsync(field, value, condition, parameters);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="parameters">条件参数</param>
        /// <returns>更新行数</returns>
        public Task<int> SetValueAsync(Expression<Func<TEntity, bool>> field, bool value, string condition,
            params DbParameter[] parameters)
        {
            return SetValueByConditionAsync(GetPropertyName(field), value ? 0 : 1, condition, parameters);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="parameters">条件参数</param>
        /// <returns>更新行数</returns>
        public Task<int> SetValueAsync(Expression<Func<TEntity, Enum>> field, Enum value, string condition,
            params DbParameter[] parameters)
        {
            return SetValueByConditionAsync(GetPropertyName(field), Convert.ToInt32(value), condition, parameters);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="parameters">条件参数</param>
        /// <returns>更新行数</returns>
        public Task<int> SetValueAsync<TField>(Expression<Func<TEntity, TField>> field, TField value,
            string condition, params DbParameter[] parameters)
        {
            return SetValueByConditionAsync(GetPropertyName(field), value, condition, parameters);
        }


        /// <summary>
        ///     全量更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>更新行数</returns>
        public Task<int> SetValueAsync<TField>(Expression<Func<TEntity, TField>> field, TField value)
        {
            return SetValueByConditionAsync(GetPropertyName(field), value, "-1", null);
        }

        /// <summary>
        ///     条件更新实体中已记录更新部分
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        public Task<int> SetValueAsync(TEntity entity, Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            string sql = SqlBuilder.CreateUpdateSqlCode(entity, convert.ConditionSql);
            return UpdateValue(DataOperatorType.MulitUpdate, sql, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        public Task<int> SetValueAsync<TField>(Expression<Func<TEntity, TField>> field, TField value,
            Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return SetValueByConditionAsync(GetPropertyName(field), value, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="parameters">条件参数</param>
        /// <returns>更新行数</returns>
        private Task<int> SetValueByConditionAsync(string field, object value, string condition, params DbParameter[] parameters)
        {
            return DoUpdateValueAsync(field, value, condition, parameters);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="parameters">条件参数</param>
        /// <returns>更新行数</returns>
        private Task<int> SetValueInnerAsync(string field, object value, string condition, params DbParameter[] parameters)
        {
            return DoUpdateValueAsync(field, value, condition, parameters);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="operatorType">操作类型</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="condition">更新条件</param>
        /// <param name="parameters">条件参数</param>
        /// <returns>更新行数</returns>
        private async Task<int> UpdateValue(DataOperatorType operatorType, string sql, string condition, IEnumerable<DbParameter> parameters)
        {
            if (sql == null)
                return -1;
            if (Provider.Injection != null)
                await Provider.Injection.BeforeExecute(operatorType, condition, parameters);
            var cnt = await DataBase.ExecuteAsync(sql, parameters);
            if (cnt <= 0)
                return 0;
            if (Provider.Injection != null)
                await Provider.Injection.AfterExecute(operatorType, sql, condition, parameters);
            return cnt;
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="parameter">条件参数</param>
        /// <returns>更新行数</returns>
        private Task<int> DoUpdateValueAsync(string field, object value, string condition, DbParameter[] parameter)
        {
            field = Option.PropertyMap[field].FieldName;

            var parameters = new List<DbParameter>();
            if (parameters != null)
                parameters.AddRange(parameter);
            var sql = SqlBuilder.CreateUpdateSqlCode(SqlBuilder.FieldUpdateSetCode(field, value, parameters), condition);
            return UpdateValue(DataOperatorType.MulitUpdate, sql, condition, parameters);
        }
        /// <summary>
        ///     条件累加更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="parameter">条件参数</param>
        /// <returns>更新行数</returns>
        private Task<int> DoAddValueAsync(string field, object value, string condition, DbParameter[] parameter)
        {
            field = Option.PropertyMap[field].FieldName;

            var parameters = new List<DbParameter>();
            if (parameters != null)
                parameters.AddRange(parameter);
            var sql = SqlBuilder.CreateUpdateSqlCode(SqlBuilder.FieldAddCode(field, value, parameters), condition);
            return UpdateValue(DataOperatorType.MulitUpdate, sql, condition, parameters);
        }
        #endregion

        #region 多字段更新


        /// <summary>
        ///     自定义更新（更新表达式自写）
        /// </summary>
        /// <param name="expression">更新SQL表达式</param>
        /// <param name="condition">条件</param>
        /// <returns>更新行数</returns>
        public Task<int> SetValueAsync(string expression, LambdaItem<TEntity> condition)
        {
            var convert = SqlBuilder.Compile(condition);
            var sql = SqlBuilder.CreateUpdateSqlCode(expression, convert.ConditionSql);
            return UpdateValue(DataOperatorType.MulitUpdate, sql, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     自定义更新（更新表达式自写）
        /// </summary>
        /// <param name="expression">更新SQL表达式</param>
        /// <param name="condition">条件</param>
        /// <returns>更新行数</returns>
        public Task<int> SetValueAsync(string expression, Expression<Func<TEntity, bool>> condition)
        {
            var convert = SqlBuilder.Compile(condition);
            var sql = SqlBuilder.CreateUpdateSqlCode(expression, convert.ConditionSql);
            return UpdateValue(DataOperatorType.MulitUpdate, sql, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     自定义更新（更新表达式自写）
        /// </summary>
        /// <param name="expression">更新SQL表达式</param>
        /// <param name="condition">条件</param>
        /// <param name="parameters">参数</param>
        /// <returns>更新行数</returns>
        public Task<int> SetMulitValueAsync(string expression, string condition, params DbParameter[] parameters)
        {
            var sql = SqlBuilder.CreateUpdateSqlCode(expression, condition);
            return UpdateValue(DataOperatorType.MulitUpdate, sql, condition, parameters);
        }


        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="fields">字段与值组合</param>
        /// <param name="condition">条件</param>
        /// <returns>更新行数</returns>
        public Task<int> SetValueAsync(string condition, params (string field, object value)[] fields)
        {
            return SetMulitValue(condition, null, fields);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="fields">字段与值组合</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        public Task<int> SetValueAsync(object key, params (string field, object value)[] fields)
        {
            return SetMulitValue(
                SqlBuilder.PrimaryKeyCondition,
                new DbParameter[] { ParameterCreater.CreateParameter(Option.PrimaryProperty, key) },
                fields);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="fields">字段与值组合</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        public Task<int> SetValueAsync(Expression<Func<TEntity, bool>> lambda, params (string field, object value)[] fields)
        {
            var convert = SqlBuilder.Compile(lambda);
            return SetMulitValue(convert.ConditionSql, convert.Parameters, fields);
        }


        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="fields">字段与值组合</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        public Task<int> SetValueAsync(LambdaItem<TEntity> lambda, params (string field, object value)[] fields)
        {
            var convert = SqlBuilder.Compile(lambda);
            return SetMulitValue(convert.ConditionSql, convert.Parameters, fields);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="fields">字段</param>
        /// <param name="condition">更新条件</param>
        /// <param name="parameters">条件参数</param>
        /// <returns>更新行数</returns>
        public Task<int> SetValueAsync(string condition, DbParameter[] parameters, IEnumerable<(string field, object value)> fields)
        {
            return SetMulitValue(condition, parameters, fields);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="fields">字段</param>
        /// <param name="condition">更新条件</param>
        /// <param name="parameter">条件参数</param>
        /// <returns>更新行数</returns>
        private Task<int> SetMulitValue(string condition, DbParameter[] parameter, IEnumerable<(string field, object value)> fields)
        {
            var parameters = new List<DbParameter>();
            if (parameter != null)
                parameters.AddRange(parameter);
            bool first = true;
            var code = new StringBuilder();
            foreach (var field in fields)
            {
                if (first)
                    first = false;
                else
                    code.AppendLine(",");
                code.Append(SqlBuilder.FieldUpdateSetCode(Option.PropertyMap[field.field].FieldName, field.value, parameters));
            }

            var sql = SqlBuilder.CreateUpdateSqlCode(code.ToString(), condition);
            return UpdateValue(DataOperatorType.MulitUpdate, sql, condition, parameters);

        }
        #endregion

        #region 批量操作

        /// <summary>
        /// 开始插入
        /// </summary>
        /// <returns></returns>
        public async Task<DbOperatorContext> BeginInsert()
        {
            await using var scope = await DataBase.CreateConnectionScope();
            var sql = SqlBuilder.CreateInsertSqlCode();
            var ctx = new DbOperatorContext
            {
                ConnectionScope = scope,
                Command = scope.CreateCommand(sql)
            };
            DataOperator.CreateEntityParameter(ctx.Command);
            ctx.Command.CommandText = sql;
            await ctx.Command.PrepareAsync();
            return ctx;
        }

        /// <summary>
        /// 开始插入
        /// </summary>
        /// <returns></returns>
        public async Task<DbOperatorContext> BeginInsert(IConnectionScope scope)
        {
            var sql = SqlBuilder.CreateInsertSqlCode();
            var ctx = new DbOperatorContext
            {
                Command = scope.CreateCommand(sql)
            };
            DataOperator.CreateEntityParameter(ctx.Command);
            await ctx.Command.PrepareAsync();
            return ctx;
        }

        /// <summary>
        /// 异步插入
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsync(DbOperatorContext context, TEntity entity)
        {
            if (Provider.Injection != null)
                await Provider.Injection.BeforeSave(entity, DataOperatorType.Insert);
            DataOperator.SetEntityParameter(context.Command, entity);
            if (Option.IsIdentity)
            {
                var key = await context.Command.ExecuteScalarAsync();
                if (key == DBNull.Value || key == null)
                    return false;
                Provider.EntityOperator.SetValue(entity, Option.PrimaryProperty, key);
            }
            else
            {
                var res = await context.Command.ExecuteNonQueryAsync();
                if (res == 0)
                    return false;
            }
            await DataOperator.AfterSave(this, entity, DataOperatorType.Insert);
            if (Provider.Injection != null)
                await Provider.Injection.AfterSave(entity, DataOperatorType.Insert);

            return true;
        }

        /// <summary>
        /// 异步插入
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsync(DbOperatorContext context, IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await InsertAsync(context, entity);
            }

            return true;
        }

        /// <summary>
        /// 开始插入
        /// </summary>
        /// <returns></returns>
        public async Task<DbOperatorContext> BeginUpdate()
        {
            await using var scope = await DataBase.CreateConnectionScope();
            var ctx = new DbOperatorContext
            {
                ConnectionScope = scope,
                Command = scope.CreateCommand(SqlBuilder.CreateUpdateSqlCode(Option.UpdateSqlCode, SqlBuilder.PrimaryKeyCondition))
            };
            DataOperator.CreateEntityParameter(ctx.Command);
            await ctx.Command.PrepareAsync();
            return ctx;
        }

        /// <summary>
        /// 开始插入
        /// </summary>
        /// <returns></returns>
        public async Task<DbOperatorContext> BeginUpdate(IConnectionScope scope)
        {
            var ctx = new DbOperatorContext
            {
                Command = scope.CreateCommand(SqlBuilder.CreateUpdateSqlCode(Option.UpdateSqlCode, SqlBuilder.PrimaryKeyCondition))
            };
            DataOperator.CreateEntityParameter(ctx.Command);
            await ctx.Command.PrepareAsync();
            return ctx;
        }

        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(DbOperatorContext context, TEntity entity)
        {
            if (Provider.Injection != null)
                await Provider.Injection.BeforeSave(entity, DataOperatorType.Update);
            DataOperator.SetEntityParameter(context.Command, entity);
            var res = await context.Command.ExecuteNonQueryAsync();
            if (res == 0)
                return false;
            await DataOperator.AfterSave(this, entity, DataOperatorType.Update);
            if (Provider.Injection != null)
                await Provider.Injection.AfterSave(entity, DataOperatorType.Update);

            return true;
        }

        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(DbOperatorContext context, IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(context, entity);
            }

            return true;
        }

        #endregion
    }
}