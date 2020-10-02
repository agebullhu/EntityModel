// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using Agebull.Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
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
    ///     Sqlʵ�������
    /// </summary>
    /// <typeparam name="TEntity">ʵ��</typeparam>
    public sealed partial class DataAccess<TEntity> : DataQuery<TEntity>
         where TEntity : class, new()
    {
        #region ����

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="provider"></param>
        public DataAccess(DataAccessProvider<TEntity> provider)
            : base(provider)
        {
        }

        #endregion
        #region ʵ�����

        /// <summary>
        ///     ����������
        /// </summary>
        public async Task<bool> InsertAsync(TEntity entity, bool reload = false)
        {
            FlowTracer.BeginStepMonitor("InsertInnerAsync");
            try
            {
                if (!await InsertInnerAsync(entity))
                    return false;
                if (reload)
                    await ReLoadInnerAsync(entity);
                return true;
            }
            finally
            {
                FlowTracer.EndStepMonitor();
            }
        }

        /// <summary>
        ///     ����������
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
        ///     ��������
        /// </summary>
        /// <param name="entity">�������ݵ�ʵ��</param>
        private async Task<bool> InsertInnerAsync(TEntity entity)
        {
            await using var connectionScope = await DataBase.CreateConnectionScope();

            await Provider.DataOperator.BeforeSave(entity, DataOperatorType.Insert);
            {
                var sql = SqlBuilder.CreateInsertSqlCode(entity);
                await using var cmd = connectionScope.CreateCommand(sql);

                DataOperator.SetEntityParameter(cmd, entity);
                DataBase.TraceSql(cmd);
                if (!Option.IsIdentity)
                {
                    if (await cmd.ExecuteNonQueryAsync() == 0)
                        return false;
                }
                else
                {
                    var key = await cmd.ExecuteScalarAsync();
                    if (key == DBNull.Value || key == null)
                        return false;
                    Provider.EntityOperator.SetValue(entity, Option.PrimaryKey, key);
                }
            }
            await Provider.DataOperator.AfterSave(entity, DataOperatorType.Insert);

            return true;
        }


        /// <summary>
        ///     ��������
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
        ///     ��������
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
        ///     ɾ������
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
        ///     ɾ������
        /// </summary>
        public async Task<bool> DeleteAsync(TEntity entity)
        {
            return await DeleteInnerAsync(entity);
        }

        /// <summary>
        ///     ɾ��
        /// </summary>
        private async Task<bool> DeleteInnerAsync(TEntity entity)
        {
            if (entity == null)
                return false;
            await Provider.DataOperator.BeforeSave(entity, DataOperatorType.Delete);
            var para = ParameterCreater.CreateParameter(Option.PrimaryKey,
                Provider.EntityOperator.GetValue(entity, Option.PrimaryKey),
                SqlBuilder.GetDbType(Option.PrimaryKey));
            var result = await DeleteInnerAsync(SqlBuilder.PrimaryKeyCondition, para);
            if (result == 0)
                return false;
            await Provider.DataOperator.AfterSave(entity, DataOperatorType.Delete);
            return true;
        }


        /// <summary>
        ///     ��������
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
        ///     ��������
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
        ///     ����
        /// </summary>
        private async Task<bool> SaveInnerAsync(TEntity entity)
        {
            if (await ExistPrimaryKeyAsync(Provider.EntityOperator.GetValue(entity, Option.PrimaryKey)))
            {
                return await UpdateInnerAsync(entity);
            }
            else
            {
                return await InsertInnerAsync(entity);
            }
        }


        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="entity">�������ݵ�ʵ��</param>
        private async Task<bool> UpdateInnerAsync(TEntity entity)
        {
            await Provider.DataOperator.BeforeSave(entity, DataOperatorType.Update);
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
            await Provider.DataOperator.AfterSave(entity, DataOperatorType.Update);

            return true;
        }

        #endregion

        #region ɾ��

        /// <summary>
        ///     ɾ������
        /// </summary>
        public async Task<bool> DeletePrimaryKeyAsync(object key)
        {
            return 1 == await DeleteInnerAsync(SqlBuilder.PrimaryKeyCondition, ParameterCreater.CreateParameter(Option.PrimaryKey, key, SqlBuilder.GetDbType(Option.PrimaryKey)));
        }


        /// <summary>
        ///     ����ɾ��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public Task<int> DeleteAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return DeleteInnerAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ����ɾ������
        /// </summary>
        public async Task<bool> PhysicalDeleteAsync(object key)
        {
            var sql = SqlBuilder.PhysicalDeleteSqlCode(SqlBuilder.PrimaryKeyCondition);
            return 1 == await DoUpdateValueAsync(DataOperatorType.MulitDelete, sql, SqlBuilder.PrimaryKeyCondition,
                ParameterCreater.CreateParameter(Option.PrimaryKey, key, SqlBuilder.GetDbType(Option.PrimaryKey)));
        }

        /// <summary>
        ///     ǿ������ɾ��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ�ɾ���ɹ�</returns>
        public Task<int> PhysicalDeleteAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            if (string.IsNullOrEmpty(convert.ConditionSql))
                throw new EntityModelDbException(@"ɾ����������Ϊ��,��Ϊ������ִ��ȫ��ɾ��");
            var sql = SqlBuilder.PhysicalDeleteSqlCode(convert.ConditionSql);
            return DoUpdateValueAsync(DataOperatorType.MulitDelete, sql, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ����ɾ��
        /// </summary>
        public Task<int> DeleteAsync(string condition, params DbParameter[] args)
        {
            //throw new EntityModelDbException("����ɾ�����ܱ�����");
            if (string.IsNullOrWhiteSpace(condition))
                throw new EntityModelDbException(@"ɾ����������Ϊ��,��Ϊ������ִ��ȫ��ɾ��");
            return DeleteInnerAsync(condition, args);
        }

        /// <summary>
        ///     ɾ��
        /// </summary>
        private Task<int> DeleteInnerAsync(string condition, params DbParameter[] args)
        {
            if (string.IsNullOrEmpty(condition))
                throw new EntityModelDbException(@"ɾ����������Ϊ��,��Ϊ������ִ��ȫ��ɾ��");

            return DoUpdateValueAsync(DataOperatorType.MulitDelete, SqlBuilder.CreateDeleteSql(condition),
                SqlBuilder.PrimaryKeyCondition, args);
        }

        #endregion

        #region ��׼����

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="key">����</param>
        /// <returns>��������</returns>
        public Task<int> SetValueAsync(string field, object value, object key)
        {
            return SetValueInnerAsync(field, value, SqlBuilder.PrimaryKeyCondition, ParameterCreater.CreateParameter(Option.PrimaryKey, key, SqlBuilder.GetDbType(Option.PrimaryKey)));
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="key">����</param>
        /// <returns>��������</returns>
        public Task<int> SetValueAsync<TField, TKey>(Expression<Func<TEntity, TField>> fieldExpression,
            TField value, TKey key)
        {
            return SetValueInnerAsync(GetPropertyName(fieldExpression), value, SqlBuilder.PrimaryKeyCondition,
                ParameterCreater.CreateParameter(Option.PrimaryKey, key, SqlBuilder.GetDbType(Option.PrimaryKey)));
        }

        /// <summary>
        ///     ����ֶΰ��Զ�����ʽ����ֵ
        /// </summary>
        /// <param name="valueExpression">ֵ��SQL��ʽ</param>
        /// <param name="key">����</param>
        /// <returns>��������</returns>
        public Task<int> SetCoustomValueAsync<TKey>(string valueExpression, TKey key)
        {
            var sql = SqlBuilder.CreateUpdateSqlCode(valueExpression, SqlBuilder.PrimaryKeyCondition);
            return DoUpdateValueAsync(DataOperatorType.MulitUpdate, sql, SqlBuilder.PrimaryKeyCondition, new[]
            {
                ParameterCreater.CreateParameter(Option.PrimaryKey, key, SqlBuilder.GetDbType(Option.PrimaryKey))
            });
        }

        #endregion

        #region ��������

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�����ֶ�</param>
        /// <param name="value">����ֵ</param>
        /// <param name="conditionFiles">�����������ֶ�</param>
        /// <param name="conditionValues">����������ֵ</param>
        /// <returns>��������</returns>
        /// <remarks>
        /// ������ʹ��AND���,��Ϊ����
        /// </remarks>
        public async Task SaveValueAsync(string field, object value, string[] conditionFiles, object[] conditionValues)
        {
            var args = CreateFieldsParameters(conditionFiles, conditionValues);
            var condition = SqlBuilder.ConcatFieldCondition(true, conditionFiles);
            await SetValueByConditionAsync(field, value, condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        public async Task<int> SetValueAsync(string field, object value, string condition, params DbParameter[] args)
        {
            return await SetValueByConditionAsync(field, value, condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        public async Task<int> SetValueAsync(Expression<Func<TEntity, bool>> field, bool value, string condition,
            params DbParameter[] args)
        {
            return await SetValueByConditionAsync(GetPropertyName(field), value ? 0 : 1, condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        public async Task<int> SetValueAsync(Expression<Func<TEntity, Enum>> field, Enum value, string condition,
            params DbParameter[] args)
        {
            return await SetValueByConditionAsync(GetPropertyName(field), Convert.ToInt32(value), condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        public async Task<int> SetValueAsync<TField>(Expression<Func<TEntity, TField>> field, TField value,
            string condition,
            params DbParameter[] args)
        {
            return await SetValueByConditionAsync(GetPropertyName(field), value, condition, args);
        }


        /// <summary>
        ///     ȫ������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <returns>��������</returns>
        public async Task<int> SetValueAsync<TField>(Expression<Func<TEntity, TField>> field, TField value)
        {
            return await SetValueByConditionAsync(GetPropertyName(field), value, "-1", null);
        }
        /// <summary>
        ///     ��������ʵ�����Ѽ�¼���²���
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="lambda">����</param>
        /// <returns>��������</returns>
        public Task<int> SetValueAsync(TEntity entity, Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            string sql = SqlBuilder.CreateUpdateSqlCode(entity, convert.ConditionSql);
            return DoUpdateValueAsync(DataOperatorType.MulitUpdate, sql, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="lambda">����</param>
        /// <returns>��������</returns>
        public Task<int> SetValueAsync<TField>(Expression<Func<TEntity, TField>> field, TField value,
            Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return SetValueByConditionAsync(GetPropertyName(field), value, convert.ConditionSql,
                convert.Parameters);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        private Task<int> SetValueByConditionAsync(string field, object value, string condition,
            params DbParameter[] args)
        {
            return DoUpdateValueAsync(field, value, condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        private Task<int> SetValueInnerAsync(string field, object value, string condition,
            params DbParameter[] args)
        {
            return DoUpdateValueAsync(field, value, condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        private async Task<int> DoUpdateValueAsync(DataOperatorType operatorType, string sql, string condition, params DbParameter[] args)
        {
            if (sql == null)
                return -1;
            await Provider.DataOperator.BeforeExecute(operatorType, condition, args);
            var cnt = await DataBase.ExecuteAsync(sql, args);
            if (cnt <= 0)
                return 0;
            await Provider.DataOperator.AfterExecute(operatorType, condition, args);
            if (Provider.Injection != null)
                await Provider.Injection.AfterExecute(operatorType, condition, args);
            return cnt;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        private Task<int> DoUpdateValueAsync(string field, object value, string condition, DbParameter[] args)
        {
            field = Option.FieldMap[field];

            var parameters = new List<DbParameter>();
            if (args != null)
                parameters.AddRange(args);
            var sql = SqlBuilder.CreateUpdateSqlCode(SqlBuilder.FileUpdateSetCode(field, value, parameters), condition);
            return DoUpdateValueAsync(DataOperatorType.MulitUpdate, sql, condition, args);
        }

        /// <summary>
        ///     �Զ�����£����±��ʽ��д��
        /// </summary>
        /// <param name="expression">����SQL���ʽ</param>
        /// <param name="condition">����</param>
        /// <returns>��������</returns>
        public Task<int> SetValueAsync(string expression, LambdaItem<TEntity> condition)
        {
            var convert = SqlBuilder.Compile(condition);
            return SetMulitValueAsync(expression, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     �Զ�����£����±��ʽ��д��
        /// </summary>
        /// <param name="expression">����SQL���ʽ</param>
        /// <param name="condition">����</param>
        /// <returns>��������</returns>
        public Task<int> SetValueAsync(string expression, Expression<Func<TEntity, bool>> condition)
        {
            var convert = SqlBuilder.Compile(condition);
            return SetMulitValueAsync(expression, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     �Զ�����£����±��ʽ��д��
        /// </summary>
        /// <param name="expression">����SQL���ʽ</param>
        /// <param name="condition">����</param>
        /// <param name="args">����</param>
        /// <returns>��������</returns>
        public Task<int> SetMulitValueAsync(string expression, Expression<Func<TEntity, bool>> condition,
            params DbParameter[] args)
        {
            var convert = SqlBuilder.Compile(condition);
            var arg = new List<DbParameter>();
            if (convert.HaseParameters)
                arg.AddRange(convert.DbParameter);
            if (args.Length > 0)
                arg.AddRange(args);
            return SetMulitValueAsync(expression, convert.ConditionSql, arg.ToArray());
        }

        /// <summary>
        ///     �Զ�����£����±��ʽ��д��
        /// </summary>
        /// <param name="expression">����SQL���ʽ</param>
        /// <param name="condition">����</param>
        /// <param name="args">����</param>
        /// <returns>��������</returns>
        public Task<int> SetMulitValueAsync(string expression, string condition, DbParameter[] args)
        {
            var sql = SqlBuilder.CreateUpdateSqlCode(expression, condition);
            return DoUpdateValueAsync(DataOperatorType.MulitUpdate, sql, condition, args);
        }


        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="fields">�ֶ���ֵ���</param>
        /// <param name="condition">����</param>
        /// <returns>��������</returns>
        public async Task<int> SetValueAsync(string condition, params (string field, object value)[] fields)
        {
            return await SetValueAsync(condition, null, fields);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="fields">�ֶ���ֵ���</param>
        /// <param name="lambda">����</param>
        /// <returns>��������</returns>
        public Task<int> SetValueAsync(Expression<Func<TEntity, bool>> lambda, params (string field, object value)[] fields)
        {
            var convert = SqlBuilder.Compile(lambda);
            return SetValueAsync(convert.ConditionSql, convert.Parameters, fields);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="fields">�ֶ�</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        public Task<int> SetValueAsync(string condition, DbParameter[] args, IEnumerable<(string field, object value)> fields)
        {
            var parameters = new List<DbParameter>();
            if (args != null)
                parameters.AddRange(args);
            bool first = true;
            var code = new StringBuilder();
            foreach (var field in fields)
            {
                if (first)
                    first = false;
                else
                    code.AppendLine(",");
                code.Append(SqlBuilder.FileUpdateSetCode(Option.FieldMap[field.field], field.value, parameters));
            }

            var sql = SqlBuilder.CreateUpdateSqlCode(code.ToString(), condition);
            return DoUpdateValueAsync(DataOperatorType.MulitUpdate, sql, condition, args);

        }
        #endregion

        #region ��������

        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <returns></returns>
        public async Task<DbOperatorContext> BeginInsert()
        {
            var scope = await DataBase.CreateConnectionScope();
            var ctx = new DbOperatorContext
            {
                ConnectionScope = scope,
                Command = scope.CreateCommand(Option.InsertSqlCode)
            };
            DataOperator.CreateEntityParameter(ctx.Command);
            ctx.Command.CommandText = Option.InsertSqlCode;
            await ctx.Command.PrepareAsync();
            return ctx;
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <returns></returns>
        public async Task<DbOperatorContext> BeginInsert(IConnectionScope scope)
        {
            var ctx = new DbOperatorContext
            {
                Command = scope.CreateCommand(Option.InsertSqlCode)
            };
            DataOperator.CreateEntityParameter(ctx.Command);
            await ctx.Command.PrepareAsync();
            return ctx;
        }

        /// <summary>
        /// �첽����
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsync(DbOperatorContext context, TEntity entity)
        {
            await Provider.DataOperator.BeforeSave(entity, DataOperatorType.Insert);
            DataOperator.SetParameterValue(entity, context.Command);
            if (Option.IsIdentity)
            {
                var key = await context.Command.ExecuteScalarAsync();
                if (key == DBNull.Value || key == null)
                    return false;
                Provider.EntityOperator.SetValue(entity, Option.PrimaryKey, key);
            }
            else
            {
                var res = await context.Command.ExecuteNonQueryAsync();
                if (res == 0)
                    return false;
            }
            await Provider.DataOperator.AfterSave(entity, DataOperatorType.Insert);

            return true;
        }

        /// <summary>
        /// �첽����
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
        /// ��ʼ����
        /// </summary>
        /// <returns></returns>
        public async Task<DbOperatorContext> BeginUpdate()
        {
            var scope = await DataBase.CreateConnectionScope();
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
        /// ��ʼ����
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
        /// �첽����
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(DbOperatorContext context, TEntity entity)
        {
            await Provider.DataOperator.BeforeSave(entity, DataOperatorType.Update);
            DataOperator.SetParameterValue(entity, context.Command);
            var res = await context.Command.ExecuteNonQueryAsync();
            if (res == 0)
                return false;
            await Provider.DataOperator.AfterSave(entity, DataOperatorType.Update);

            return true;
        }

        /// <summary>
        /// �첽����
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