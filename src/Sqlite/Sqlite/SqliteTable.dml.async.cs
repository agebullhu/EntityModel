// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DbOperatorContext = Agebull.EntityModel.Common.DbOperatorContext<Microsoft.Data.Sqlite.SqliteCommand>;

#endregion

namespace Agebull.EntityModel.Sqlite
{
    partial class SqliteTable<TData, TDataBase>
    {
        #region 实体更新

        /// <summary>
        ///     插入新数据
        /// </summary>
        public async Task<bool> InsertAsync(TData entity)
        {
            //using (var scope = TransactionScope.CreateScope(DataBase))
            {
                if (!await InsertInnerAsync(entity))
                    return false;
                await ReLoadInnerAsync(entity);
                //scope.Succeed();
            }

            return true;
        }

        /// <summary>
        ///     插入新数据
        /// </summary>
        public async Task<int> InsertAsync(IEnumerable<TData> entities)
        {
            var datas = entities as TData[] ?? entities.ToArray();
            int cnt = 0;
            //using (var scope = TransactionScope.CreateScope(DataBase))
            {
                foreach (var entity in datas)
                {
                    if (await InsertInnerAsync(entity))
                        ++cnt;
                    else
                        return 0;
                }

                //scope.Succeed();
            }

            foreach (var entity in datas)
                await ReLoadInnerAsync(entity);
            return cnt;
        }

        /// <summary>
        ///     更新数据
        /// </summary>
        /// <param name="entity">更新数据的实体</param>
        protected async Task<bool> InsertInnerAsync(TData entity)
        {
            PrepareSave(entity, DataOperatorType.Insert);
            using (var cmd = DataBase.CreateCommand())
            {
                var isIdentitySql = SetInsertCommand(entity, cmd);
                SqliteDataBase.TraceSql(cmd);
                if (isIdentitySql)
                {
                    var key = await cmd.ExecuteScalarAsync();
                    if (key == DBNull.Value || key == null)
                        return false;
                    entity.SetValue(KeyField, key);
                }
                else
                {
                    var res = await cmd.ExecuteNonQueryAsync();
                    if (res == 0)
                        return false;
                }

                var sql = AfterUpdateSql(PrimaryKeyConditionSQL);
                if (!string.IsNullOrEmpty(sql))
                {
                    await DataBase.ExecuteAsync(sql, CreatePimaryKeyParameter(entity.GetValue(KeyField)));
                }
            }

            EndSaved(entity, DataOperatorType.Insert);
            return true;
        }


        /// <summary>
        ///     更新数据
        /// </summary>
        public async Task<bool> UpdateAsync(TData entity)
        {
            //using (var scope = TransactionScope.CreateScope(DataBase))
            {
                if (!await UpdateInnerAsync(entity))
                    return false;
                //scope.Succeed();
            }

            await ReLoadInnerAsync(entity);
            return true;
        }

        /// <summary>
        ///     更新数据
        /// </summary>
        public async Task<int> UpdateAsync(IEnumerable<TData> entities)
        {
            var datas = entities as TData[] ?? entities.ToArray();
            int cnt = 0;
            //using (var scope = TransactionScope.CreateScope(DataBase))
            {
                foreach (var entity in datas)
                {
                    if (await UpdateInnerAsync(entity))
                        ++cnt;
                }

                //scope.Succeed();
            }

            foreach (var entity in datas)
                await ReLoadInnerAsync(entity);
            return cnt;
        }

        /// <summary>
        ///     重新载入
        /// </summary>
        private async Task ReLoadInnerAsync(TData entity)
        {
            entity.__status.RejectChanged();
            using (var cmd = CreateLoadCommand(PrimaryKeyConditionSQL, CreatePimaryKeyParameter(entity)))
            {
                using var reader = cmd.ExecuteReader();
                if (!await reader.ReadAsync())
                    return;
                using (new EntityLoadScope(entity))
                {
                    if (DynamicLoadAction != null)
                        DynamicLoadAction(reader, entity);
                    else
                        LoadEntity(reader, entity);
                }
            }

            var entity2 = EntityLoaded(entity);
            if (entity != entity2)
                entity.CopyValue(entity2);
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        public async Task<int> DeleteAsync(IEnumerable<TData> entities)
        {
            var datas = entities as TData[] ?? entities.ToArray();
            int cnt = 0;
            //using (var scope = TransactionScope.CreateScope(DataBase))
            {
                foreach (var entity in datas)
                {
                    if (await UpdateInnerAsync(entity))
                        ++cnt;
                    else
                        return 0;
                }

                //scope.Succeed();
            }

            return cnt;
        }


        /// <summary>
        ///     删除数据
        /// </summary>
        public async Task<bool> DeleteAsync(TData entity)
        {
            //using (var scope = TransactionScope.CreateScope(DataBase))
            {
                if (!await DeleteInnerAsync(entity))
                    return false;
                //scope.Succeed();
            }

            return true;
        }

        /// <summary>
        ///     删除
        /// </summary>
        private async Task<bool> DeleteInnerAsync(TData entity)
        {
            if (entity == null)
                return false;
            entity.__status.IsDelete = true;
            PrepareSave(entity, DataOperatorType.Delete);
            var result = await DeleteInnerAsync(PrimaryKeyConditionSQL, CreatePimaryKeyParameter(entity));
            if (result == 0)
                return false;
            EndSaved(entity, DataOperatorType.Delete);
            return true;
        }


        /// <summary>
        ///     保存数据
        /// </summary>
        public async Task<int> SaveAsync(IEnumerable<TData> entities)
        {
            var datas = entities as TData[] ?? entities.ToArray();
            int cnt = 0;
            //using (var scope = TransactionScope.CreateScope(DataBase))
            {
                foreach (var entity in datas)
                {
                    if (await SaveInnerAsync(entity))
                        ++cnt;
                    else
                        return 0;
                }

                //scope.Succeed();
            }

            foreach (var entity in datas)
                await ReLoadInnerAsync(entity);
            return cnt;
        }

        /// <summary>
        ///     保存数据
        /// </summary>
        public async Task<bool> SaveAsync(TData entity)
        {
            //using (var scope = TransactionScope.CreateScope(DataBase))
            {
                if (!await SaveInnerAsync(entity))
                    return false;
                //scope.Succeed();
            }

            return true;
        }

        /// <summary>
        ///     保存
        /// </summary>
        private async Task<bool> SaveInnerAsync(TData entity)
        {
            if (entity.__status.IsDelete)
                return DeleteInner(entity);
            if (entity.__status.IsNew || !ExistPrimaryKey(entity.GetValue(KeyField)))
                return InsertInner(entity);
            return await UpdateInnerAsync(entity);
        }


        /// <summary>
        ///     插入数据
        /// </summary>
        /// <param name="entity">插入数据的实体</param>
        private async Task<bool> UpdateInnerAsync(TData entity)
        {
            if (UpdateByMidified && !entity.__status.IsModified)
                return false;
            int result;
            PrepareSave(entity, DataOperatorType.Update);
            string sql = GetModifiedSqlCode(entity);
            if (sql == null)
                return false;

            using (var cmd = DataBase.CreateCommand())
            {
                SetUpdateCommand(entity, cmd);
                cmd.CommandText = CreateUpdateSql(sql, PrimaryKeyConditionSQL);

                SqliteDataBase.TraceSql(cmd);

                result = await cmd.ExecuteNonQueryAsync();
            }

            if (result <= 0)
            {
                return false;
            }

            EndSaved(entity, DataOperatorType.Update);
            return true;
        }

        #endregion

        #region 删除

        /// <summary>
        ///     删除数据
        /// </summary>
        public async Task<bool> DeletePrimaryKeyAsync(object key)
        {
            var cnt = await DeleteInnerAsync(PrimaryKeyConditionSQL, CreatePimaryKeyParameter(key));
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
        public async Task<int> DeleteAsync(Expression<Func<TData, bool>> lambda)
        {
            //throw new Exception("批量删除功能被禁用");
            var convert = Compile(lambda);
            return await DeleteByConditionAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     物理删除数据
        /// </summary>
        public async Task<bool> PhysicalDeleteAsync(object key)
        {
            var condition = PrimaryKeyConditionSQL;
            var para = CreatePimaryKeyParameter(key);
            var paras = new[] { para };
            //using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(condition, paras, DataOperatorType.Delete);
                var result =
                    await DataBase.ExecuteAsync($@"DELETE FROM [{ContextWriteTable}] WHERE {condition};", para);
                if (result == 0)
                    return false;
                OnOperatorExecuted(condition, paras, DataOperatorType.Delete);
                //scope.Succeed();
            }

            OnKeyEvent(DataOperatorType.Delete, key);
            return true;
        }

        /// <summary>
        ///     强制物理删除
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否删除成功</returns>
        public async Task<int> PhysicalDeleteAsync(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            int cnt;
            //using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(convert.ConditionSql, convert.Parameters, DataOperatorType.MulitDelete);
                cnt = await DataBase.ExecuteAsync($@"DELETE FROM [{ContextWriteTable}] WHERE {convert.ConditionSql};",
                    convert.Parameters);
                if (cnt == 0)
                    return 0;
                OnOperatorExecuted(convert.ConditionSql, convert.Parameters, DataOperatorType.MulitDelete);
                //scope.Succeed();
            }

            OnMulitUpdateEvent(DataOperatorType.MulitDelete, convert.ConditionSql, convert.Parameters);
            return cnt;
        }

        /// <summary>
        ///     条件删除
        /// </summary>
        public async Task<int> DeleteAsync(string condition, params DbParameter[] args)
        {
            //throw new EntityModelDbException("批量删除功能被禁用");
            if (string.IsNullOrWhiteSpace(condition))
                throw new EntityModelDbException(@"删除条件不能为空,因为不允许执行全表删除");
            return await DeleteByConditionAsync(condition, args);
        }


        /// <summary>
        ///     条件删除
        /// </summary>
        private async Task<int> DeleteByConditionAsync(string condition, DbParameter[] args)
        {
            int cnt;
            //using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(condition, args, DataOperatorType.Delete);
                cnt = await DeleteInnerAsync(condition, args);
                if (cnt == 0)
                    return 0;
                OnOperatorExecuted(condition, args, DataOperatorType.Delete);
                //scope.Succeed();
            }

            OnMulitUpdateEvent(DataOperatorType.MulitDelete, condition, args);
            return cnt;
        }

        /// <summary>
        ///     删除
        /// </summary>
        private async Task<int> DeleteInnerAsync(string condition, params DbParameter[] args)
        {
            if (string.IsNullOrEmpty(DeleteSqlCode))
                return 0;
            if (!string.IsNullOrEmpty(condition))
                return await DataBase.ExecuteAsync(CreateDeleteSql(condition), args);
            throw new EntityModelDbException(@"删除条件不能为空,因为不允许执行全表删除");
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
        public async Task<int> SetValueAsync(string field, object value, object key)
        {
            int re = await SetValueInnerAsync(field, value, PrimaryKeyConditionSQL, CreatePimaryKeyParameter(key));
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
        public async Task<int> SetValueAsync<TField, TKey>(Expression<Func<TData, TField>> fieldExpression,
            TField value, TKey key)
        {
            int re = await SetValueInnerAsync(GetPropertyName(fieldExpression), value, PrimaryKeyConditionSQL,
                CreatePimaryKeyParameter(key));
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
        public async Task<int> SetCoustomValueAsync<TKey>(string valueExpression, TKey key)
        {
            var condition = PrimaryKeyConditionSQL;
            var sql = CreateUpdateSql(valueExpression, condition);
            var arg2 = new List<DbParameter>
            {
                CreateFieldParameter(KeyField, GetDbType(KeyField), key)
            };
            int result;
            //using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(condition, arg2, DataOperatorType.Update);
                result = await DataBase.ExecuteAsync(sql, arg2.ToArray());
                if (result == 0)
                    return 0;
                OnOperatorExecuted(condition, arg2, DataOperatorType.Update);
                //scope.Succeed();
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
        public async Task SaveValueAsync(string field, object value, string[] conditionFiles, object[] conditionValues)
        {
            var args = CreateFieldsParameters(conditionFiles, conditionValues);
            var condition = FieldConditionSQL(true, conditionFiles);
            await SetValueByConditionAsync(field, value, condition, args);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        public async Task<int> SetValueAsync(string field, object value, string condition, params DbParameter[] args)
        {
            return await SetValueByConditionAsync(field, value, condition, args);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        public async Task<int> SetValueAsync(Expression<Func<TData, bool>> field, bool value, string condition,
            params DbParameter[] args)
        {
            return await SetValueByConditionAsync(GetPropertyName(field), value ? 0 : 1, condition, args);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        public async Task<int> SetValueAsync(Expression<Func<TData, Enum>> field, Enum value, string condition,
            params DbParameter[] args)
        {
            return await SetValueByConditionAsync(GetPropertyName(field), Convert.ToInt32(value), condition, args);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        public async Task<int> SetValueAsync<TField>(Expression<Func<TData, TField>> field, TField value,
            string condition,
            params DbParameter[] args)
        {
            return await SetValueByConditionAsync(GetPropertyName(field), value, condition, args);
        }

        /// <summary>
        ///     条件更新实体中已记录更新部分
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        public async Task<int> SetValueAsync(TData entity, Expression<Func<TData, bool>> lambda)
        {
            if (UpdateByMidified && !entity.__status.IsModified)
                return -1;
            var setValueSql = GetModifiedSqlCode(entity);
            if (setValueSql == null)
                return -1;
            var convert = Compile(lambda);
            using var cmd = DataBase.CreateCommand();
            SetUpdateCommand(entity, cmd);
            cmd.CommandText = CreateUpdateSql(setValueSql, convert.ConditionSql);
            SqliteDataBase.TraceSql(cmd);
            return await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        ///     全量更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>更新行数</returns>
        public async Task<int> SetValueAsync<TField>(Expression<Func<TData, TField>> field, TField value)
        {
            return await SetValueByConditionAsync(GetPropertyName(field), value, "-1", null);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        public async Task<int> SetValueAsync<TField>(Expression<Func<TData, TField>> field, TField value,
            Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return await SetValueByConditionAsync(GetPropertyName(field), value, convert.ConditionSql,
                convert.Parameters);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        private async Task<int> SetValueByConditionAsync(string field, object value, string condition,
            params DbParameter[] args)
        {
            int result = await DoUpdateValueAsync(field, value, condition, args);
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
        private async Task<int> SetValueInnerAsync(string field, object value, string condition,
            params DbParameter[] args)
        {
            return await DoUpdateValueAsync(field, value, condition, args);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        private async Task<int> DoUpdateValueAsync(string field, object value, string condition, DbParameter[] args)
        {
            field = FieldDictionary[field];

            var arg2 = new List<DbParameter>();
            if (args != null)
                arg2.AddRange(args);
            var sql = CreateUpdateSql(field, value, condition, arg2);

            int result;
            //using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(condition, args, DataOperatorType.Update);
                result = await DataBase.ExecuteAsync(sql, arg2.ToArray());
                if (result <= 0)
                    return 0;
                OnOperatorExecuted(condition, args, DataOperatorType.MulitUpdate);
                //scope.Succeed();
            }

            return result;
        }

        /// <summary>
        ///     自定义更新（更新表达式自写）
        /// </summary>
        /// <param name="expression">更新SQL表达式</param>
        /// <param name="condition">条件</param>
        /// <returns>更新行数</returns>
        public async Task<int> SetValueAsync(string expression, Expression<Func<TData, bool>> condition)
        {
            var convert = Compile(condition);
            return await SetMulitValueAsync(expression, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     自定义更新（更新表达式自写）
        /// </summary>
        /// <param name="expression">更新SQL表达式</param>
        /// <param name="condition">条件</param>
        /// <param name="args">参数</param>
        /// <returns>更新行数</returns>
        public async Task<int> SetMulitValueAsync(string expression, Expression<Func<TData, bool>> condition,
            params DbParameter[] args)
        {
            var convert = Compile(condition);
            var arg = new List<DbParameter>();
            if (convert.HaseParameters)
                arg.AddRange(convert.DbParameter);
            if (args.Length > 0)
                arg.AddRange(args);
            return await SetMulitValueAsync(expression, convert.ConditionSql, arg.ToArray());
        }

        /// <summary>
        ///     自定义更新（更新表达式自写）
        /// </summary>
        /// <param name="expression">更新SQL表达式</param>
        /// <param name="condition">条件</param>
        /// <param name="args">参数</param>
        /// <returns>更新行数</returns>
        public async Task<int> SetMulitValueAsync(string expression, string condition, DbParameter[] args)
        {
            var sql = CreateUpdateSql(expression, condition);
            int result;
            //using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(condition, args, DataOperatorType.MulitUpdate);

                result = await DataBase.ExecuteAsync(sql, args);
                if (result == 0)
                    return 0;
                OnOperatorExecuted(condition, args, DataOperatorType.MulitUpdate);
                //scope.Succeed();
            }

            return result;
        }

        #endregion

        #region 批量操作

        /// <summary>
        /// 异步插入
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsync(DbOperatorContext context, TData entity)
        {
            PrepareSave(entity, DataOperatorType.Insert);
            SetParameterValue(entity, context.Command);
            if (context.IsIdentitySql)
            {
                var key = await context.Command.ExecuteScalarAsync();
                if (key == DBNull.Value || key == null)
                    return false;
                entity.SetValue(KeyField, key);
            }
            else
            {
                var res = await context.Command.ExecuteNonQueryAsync();
                if (res == 0)
                    return false;
            }

            var sql = AfterUpdateSql(PrimaryKeyConditionSQL);
            if (!string.IsNullOrEmpty(sql))
            {
                await DataBase.ExecuteAsync(sql, CreatePimaryKeyParameter(entity.GetValue(KeyField)));
            }

            EndSaved(entity, DataOperatorType.Insert);
            return true;
        }

        /// <summary>
        /// 异步插入
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsync(DbOperatorContext context, IEnumerable<TData> entities)
        {
            foreach (var entity in entities)
            {
                await InsertAsync(context, entity);
            }

            return true;
        }


        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(DbOperatorContext context, TData entity)
        {
            PrepareSave(entity, DataOperatorType.Update);
            SetParameterValue(entity, context.Command);
            var res = await context.Command.ExecuteNonQueryAsync();
            if (res == 0)
                return false;
            var sql = AfterUpdateSql(PrimaryKeyConditionSQL);
            if (!string.IsNullOrEmpty(sql))
            {
                DataBase.Execute(sql, CreatePimaryKeyParameter(entity.GetValue(KeyField)));
            }

            EndSaved(entity, DataOperatorType.Update);
            return true;
        }

        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(DbOperatorContext context, IEnumerable<TData> entities)
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