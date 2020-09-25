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
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DbOperatorContext = Agebull.EntityModel.Common.DbOperatorContext<System.Data.Common.DbCommand>;

#endregion

namespace Agebull.EntityModel.Common
{
    partial class DataAccess<TEntity>
    {
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
            Provider.Injection?.PrepareSave(entity, DataOperatorType.Insert);
            await using var connectionScope = await DataBase.CreateConnectionScope();
            await using var cmd = connectionScope.CreateCommand(Option.InsertSqlCode);

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
                Provider.DataOperator.SetValue(entity, Option.PrimaryKey, key);
            }
            if(Provider.Injection != null)
            {
                Provider.Injection.EndSaved(entity, DataOperatorType.Insert);
                await Provider.Injection.OnEndSavedEvent(entity, DataOperatorType.Insert);
            }
            
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
        ///     ��������
        /// </summary>
        private async Task<bool> ReLoadInnerAsync(TEntity entity)
        {
            await using var connectionScope = await DataBase.CreateConnectionScope();
            var para = ParameterCreater.CreateParameter(Option.PrimaryKey, Provider.DataOperator.GetValue(entity, Option.PrimaryKey), SqlBuilder.GetDbType(Option.PrimaryKey));
            var sql = SqlBuilder.CreateLoadSql(SqlBuilder.PrimaryKeyConditionSQL, null, null);
            await using var cmd = connectionScope.CreateCommand(sql, para); 
            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return false;
            await DataOperator.LoadEntity(reader, entity);
            return true;
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
            Provider.Injection?.PrepareSave(entity, DataOperatorType.Delete);
            var para = ParameterCreater.CreateParameter(Option.PrimaryKey,
                Provider.DataOperator.GetValue(entity, Option.PrimaryKey),
                SqlBuilder.GetDbType(Option.PrimaryKey));
            var result = await DeleteInnerAsync(SqlBuilder.PrimaryKeyConditionSQL, para);
            if (result == 0)
                return false;
            if (Provider.Injection != null)
            {
                Provider.Injection.EndSaved(entity, DataOperatorType.Delete);
                await Provider.Injection.OnEndSavedEvent(entity, DataOperatorType.Delete);
            }
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
            if (await ExistPrimaryKeyAsync(Provider.DataOperator.GetValue(entity, Option.PrimaryKey)))
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
            Provider.Injection?.PrepareSave(entity, DataOperatorType.Update);
            string sql;
            if (Option.UpdateByMidified)
            {
                sql = SqlBuilder.GetUpdateSql(entity, SqlBuilder.PrimaryKeyConditionSQL);
                if (sql == null)
                    return false;
            }
            else
            {
                sql = Option.UpdateSqlCode;
            }
            await using var connectionScope = await DataBase.CreateConnectionScope();
            await using var cmd = connectionScope.CreateCommand(sql);
            DataOperator.SetEntityParameter(cmd, entity);

            //DataBase.TraceSql(cmd);

            if (await cmd.ExecuteNonQueryAsync() <= 0)
            {
                return false;
            }

            if (Provider.Injection != null)
            {
                Provider.Injection.EndSaved(entity, DataOperatorType.Update);
                await Provider.Injection.OnEndSavedEvent(entity, DataOperatorType.Update);
            }
            return true;
        }

        #endregion

        #region ɾ��

        /// <summary>
        ///     ɾ������
        /// </summary>
        public async Task<bool> DeletePrimaryKeyAsync(object key)
        {
            await DeleteInnerAsync(SqlBuilder.PrimaryKeyConditionSQL, ParameterCreater.CreateParameter(Option.PrimaryKey, key, SqlBuilder.GetDbType(Option.PrimaryKey)));
            await Provider.Injection?.OnKeyEvent(DataOperatorType.Delete, key);
            return true;
        }


        /// <summary>
        ///     ����ɾ��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> lambda)
        {
            //throw new EntityModelDbException("����ɾ�����ܱ�����");
            var convert = SqlBuilder.Compile(lambda);
            return await DeleteByConditionAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ����ɾ������
        /// </summary>
        public async Task<bool> PhysicalDeleteAsync(object key)
        {
            var condition = SqlBuilder.PrimaryKeyConditionSQL;
            var para = ParameterCreater.CreateParameter(Option.PrimaryKey, key, SqlBuilder.GetDbType(Option.PrimaryKey));
            var paras = new[] { para };
            Provider.Injection?.OnOperatorExecuting(condition, paras, DataOperatorType.Delete);
            var result = await DataBase.ExecuteAsync(SqlBuilder.PhysicalDeleteSql(condition), para);
            if (result == 0)
                return false;
            Provider.Injection?.OnOperatorExecuted(condition, paras, DataOperatorType.Delete);
            await Provider.Injection?.OnKeyEvent(DataOperatorType.Delete, key);
            return true;
        }

        /// <summary>
        ///     ǿ������ɾ��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ�ɾ���ɹ�</returns>
        public async Task<int> PhysicalDeleteAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            int cnt;
            Provider.Injection?.OnOperatorExecuting(convert.ConditionSql, convert.Parameters, DataOperatorType.MulitDelete);

            cnt = await DataBase.ExecuteAsync(SqlBuilder.PhysicalDeleteSql(convert.ConditionSql), convert.Parameters);

            if (cnt == 0)
                return 0;
            Provider.Injection?.OnOperatorExecuted(convert.ConditionSql, convert.Parameters, DataOperatorType.MulitDelete);
            await Provider.Injection?.OnMulitUpdateEvent(DataOperatorType.MulitDelete, convert.ConditionSql, convert.Parameters);
            return cnt;
        }

        /// <summary>
        ///     ����ɾ��
        /// </summary>
        public async Task<int> DeleteAsync(string condition, params DbParameter[] args)
        {
            //throw new EntityModelDbException("����ɾ�����ܱ�����");
            if (string.IsNullOrWhiteSpace(condition))
                throw new EntityModelDbException(@"ɾ����������Ϊ��,��Ϊ������ִ��ȫ��ɾ��");
            return await DeleteByConditionAsync(condition, args);
        }


        /// <summary>
        ///     ����ɾ��
        /// </summary>
        private async Task<int> DeleteByConditionAsync(string condition, DbParameter[] args)
        {
            int cnt;
            Provider.Injection?.OnOperatorExecuting(condition, args, DataOperatorType.Delete);
            cnt = await DeleteInnerAsync(condition, args);
            if (cnt == 0)
                return 0;
            Provider.Injection?.OnOperatorExecuted(condition, args, DataOperatorType.Delete);
            await Provider.Injection?.OnMulitUpdateEvent(DataOperatorType.MulitDelete, condition, args);
            return cnt;
        }

        /// <summary>
        ///     ɾ��
        /// </summary>
        private async Task<int> DeleteInnerAsync(string condition, params DbParameter[] args)
        {
            if (!string.IsNullOrEmpty(condition))
                return await DataBase.ExecuteAsync(SqlBuilder.CreateDeleteSql(condition), args);
            throw new EntityModelDbException(@"ɾ����������Ϊ��,��Ϊ������ִ��ȫ��ɾ��");
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
        public async Task<int> SetValueAsync(string field, object value, object key)
        {
            int re = await SetValueInnerAsync(field, value, SqlBuilder.PrimaryKeyConditionSQL, ParameterCreater.CreateParameter(Option.PrimaryKey, key, SqlBuilder.GetDbType(Option.PrimaryKey)));
            if (re > 0)
                await Provider.Injection?.OnKeyEvent(DataOperatorType.Update, key);
            return re;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="key">����</param>
        /// <returns>��������</returns>
        public async Task<int> SetValueAsync<TField, TKey>(Expression<Func<TEntity, TField>> fieldExpression,
            TField value, TKey key)
        {
            int re = await SetValueInnerAsync(GetPropertyName(fieldExpression), value, SqlBuilder.PrimaryKeyConditionSQL,
                ParameterCreater.CreateParameter(Option.PrimaryKey, key, SqlBuilder.GetDbType(Option.PrimaryKey)));
            if (re > 0)
                await Provider.Injection?.OnKeyEvent(DataOperatorType.Update, key);
            return re;
        }

        /// <summary>
        ///     ����ֶΰ��Զ�����ʽ����ֵ
        /// </summary>
        /// <param name="valueExpression">ֵ��SQL��ʽ</param>
        /// <param name="key">����</param>
        /// <returns>��������</returns>
        public async Task<int> SetCoustomValueAsync<TKey>(string valueExpression, TKey key)
        {
            var condition = SqlBuilder.PrimaryKeyConditionSQL;
            var sql = SqlBuilder.CreateUpdateSql(valueExpression, condition);
            var parameter = new[]
            {
                ParameterCreater.CreateParameter(Option.PrimaryKey, key, SqlBuilder.GetDbType(Option.PrimaryKey))
            };
            int result;
            Provider.Injection?.OnOperatorExecuting(condition, parameter, DataOperatorType.Update);
            result = await DataBase.ExecuteAsync(sql, parameter);
            if (result == 0)
                return 0;
            Provider.Injection?.OnOperatorExecuted(condition, parameter, DataOperatorType.Update);
            await Provider.Injection?.OnKeyEvent(DataOperatorType.Delete, key);
            return result;
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
            var condition = SqlBuilder.FieldConditionSQL(true, conditionFiles);
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
        ///     ��������ʵ�����Ѽ�¼���²���
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="lambda">����</param>
        /// <returns>��������</returns>
        public async Task<int> SetValueAsync(TEntity entity, Expression<Func<TEntity, bool>> lambda)
        {
            Provider.Injection?.PrepareSave(entity, DataOperatorType.Update);
            var convert = SqlBuilder.Compile(lambda);
            string sql = SqlBuilder.GetUpdateSql(entity, convert.ConditionSql);
            if (sql == null)
                return -1;
            await using var connectionScope = await DataBase.CreateConnectionScope();
            await using var cmd = connectionScope.CreateCommand(sql);
            DataOperator.SetEntityParameter(cmd, entity);

            DataBase.TraceSql(cmd);
            return await cmd.ExecuteNonQueryAsync();
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
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="lambda">����</param>
        /// <returns>��������</returns>
        public async Task<int> SetValueAsync<TField>(Expression<Func<TEntity, TField>> field, TField value,
            Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return await SetValueByConditionAsync(GetPropertyName(field), value, convert.ConditionSql,
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
        private async Task<int> SetValueByConditionAsync(string field, object value, string condition,
            params DbParameter[] args)
        {
            int result = await DoUpdateValueAsync(field, value, condition, args);
            await Provider.Injection?.OnMulitUpdateEvent(DataOperatorType.MulitUpdate, condition, args);
            return result;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        private async Task<int> SetValueInnerAsync(string field, object value, string condition,
            params DbParameter[] args)
        {
            return await DoUpdateValueAsync(field, value, condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        private async Task<int> DoUpdateValueAsync(string field, object value, string condition, DbParameter[] args)
        {
            field = Option.FieldMap[field];

            var parameters = new List<DbParameter>();
            if (args != null)
                parameters.AddRange(args);
            var sql = SqlBuilder.CreateUpdateSql(SqlBuilder.FileUpdateSql(field, value, parameters), condition);

            int result;
            Provider.Injection?.OnOperatorExecuting(condition, args, DataOperatorType.Update);
            result = await DataBase.ExecuteAsync(sql, parameters.ToArray());
            if (result <= 0)
                return 0;
            Provider.Injection?.OnOperatorExecuted(condition, args, DataOperatorType.MulitUpdate);
            return result;
        }

        /// <summary>
        ///     �Զ�����£����±��ʽ��д��
        /// </summary>
        /// <param name="expression">����SQL���ʽ</param>
        /// <param name="condition">����</param>
        /// <returns>��������</returns>
        public async Task<int> SetValueAsync(string expression, Expression<Func<TEntity, bool>> condition)
        {
            var convert = SqlBuilder.Compile(condition);
            return await SetMulitValueAsync(expression, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     �Զ�����£����±��ʽ��д��
        /// </summary>
        /// <param name="expression">����SQL���ʽ</param>
        /// <param name="condition">����</param>
        /// <param name="args">����</param>
        /// <returns>��������</returns>
        public async Task<int> SetMulitValueAsync(string expression, Expression<Func<TEntity, bool>> condition,
            params DbParameter[] args)
        {
            var convert = SqlBuilder.Compile(condition);
            var arg = new List<DbParameter>();
            if (convert.HaseParameters)
                arg.AddRange(convert.DbParameter);
            if (args.Length > 0)
                arg.AddRange(args);
            return await SetMulitValueAsync(expression, convert.ConditionSql, arg.ToArray());
        }

        /// <summary>
        ///     �Զ�����£����±��ʽ��д��
        /// </summary>
        /// <param name="expression">����SQL���ʽ</param>
        /// <param name="condition">����</param>
        /// <param name="args">����</param>
        /// <returns>��������</returns>
        public async Task<int> SetMulitValueAsync(string expression, string condition, DbParameter[] args)
        {
            var sql = SqlBuilder.CreateUpdateSql(expression, condition);
            int result;
            Provider.Injection?.OnOperatorExecuting(condition, args, DataOperatorType.MulitUpdate);

            result = await DataBase.ExecuteAsync(sql, args);
            if (result == 0)
                return 0;
            Provider.Injection?.OnOperatorExecuted(condition, args, DataOperatorType.MulitUpdate);
            return result;
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
        public async Task<int> SetValueAsync(Expression<Func<TEntity, bool>> lambda, params (string field, object value)[] fields)
        {
            var convert = SqlBuilder.Compile(lambda);
            return await SetValueAsync(convert.ConditionSql, convert.Parameters, fields);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="fields">�ֶ�</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        public async Task<int> SetValueAsync(string condition, DbParameter[] args, IEnumerable<(string field, object value)> fields)
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
                code.Append(SqlBuilder.FileUpdateSql(Option.FieldMap[field.field], field.value, parameters));
            }

            var sql = SqlBuilder.CreateUpdateSql(code.ToString(), condition);

            int result;
            Provider.Injection?.OnOperatorExecuting(condition, args, DataOperatorType.Update);
            result = await DataBase.ExecuteAsync(sql, parameters.ToArray());
            if (result <= 0)
                return 0;
            Provider.Injection?.OnOperatorExecuted(condition, args, DataOperatorType.MulitUpdate);
            return result;
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
            Provider.Injection?.PrepareSave(entity, DataOperatorType.Insert);
            DataOperator.SetParameterValue(entity, context.Command);
            if (Option.IsIdentity)
            {
                var key = await context.Command.ExecuteScalarAsync();
                if (key == DBNull.Value || key == null)
                    return false;
                Provider.DataOperator.SetValue(entity, Option.PrimaryKey, key);
            }
            else
            {
                var res = await context.Command.ExecuteNonQueryAsync();
                if (res == 0)
                    return false;
            }

            if (Provider.Injection != null)
            {
                Provider.Injection.EndSaved(entity, DataOperatorType.Insert);
                await Provider.Injection.OnEndSavedEvent(entity, DataOperatorType.Insert);
            }
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
                Command = scope.CreateCommand(SqlBuilder.CreateUpdateSql(Option.UpdateSqlCode, SqlBuilder.PrimaryKeyConditionSQL))
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
                Command = scope.CreateCommand(SqlBuilder.CreateUpdateSql(Option.UpdateSqlCode, SqlBuilder.PrimaryKeyConditionSQL))
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
            Provider.Injection?.PrepareSave(entity, DataOperatorType.Update);
            DataOperator.SetParameterValue(entity, context.Command);
            var res = await context.Command.ExecuteNonQueryAsync();
            if (res == 0)
                return false;
            if (Provider.Injection != null)
            {
                Provider.Injection.EndSaved(entity, DataOperatorType.Update);
                await Provider.Injection.OnEndSavedEvent(entity, DataOperatorType.Update);
            }
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

        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <returns></returns>
        public async Task<DbOperatorContext> BeginDelete()
        {
            var scope = await DataBase.CreateConnectionScope();
            var ctx = new DbOperatorContext
            {
                ConnectionScope = scope,
                Command = scope.CreateCommand(SqlBuilder.CreateDeleteSql(SqlBuilder.PrimaryKeyConditionSQL))
            };
            ctx.Command.Parameters.Add(ParameterCreater.CreateParameter(Option.PrimaryKey, SqlBuilder.GetDbType(Option.PrimaryKey)));
            await ctx.Command.PrepareAsync();
            return ctx;
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <returns></returns>
        public async Task<DbOperatorContext> BeginDelete(IConnectionScope scope)
        {
            var ctx = new DbOperatorContext
            {
                Command = scope.CreateCommand(SqlBuilder.CreateDeleteSql(SqlBuilder.PrimaryKeyConditionSQL))
            };
            ctx.Command.Parameters.Add(ParameterCreater.CreateParameter(Option.PrimaryKey, SqlBuilder.GetDbType(Option.PrimaryKey)));
            await ctx.Command.PrepareAsync();
            return ctx;
        }

        /// <summary>
        /// �첽����
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(DbOperatorContext context, object key)
        {
            context.Command.Parameters[Option.PrimaryKey].Value = key;
            return await context.Command.ExecuteNonQueryAsync() == 1;
        }

        /// <summary>
        /// �첽����
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(DbOperatorContext context, IEnumerable<object> keys)
        {
            foreach (var key in keys)
            {
                context.Command.Parameters[Option.PrimaryKey].Value = key;
                await context.Command.ExecuteNonQueryAsync();
            }
            return true;
        }

        #endregion
    }
}