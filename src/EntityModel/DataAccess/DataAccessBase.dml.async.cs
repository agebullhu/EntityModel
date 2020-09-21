// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using Agebull.EntityModel.Common;
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
        /// <summary>
        /// ��ǰ�����ĵĶ�ȡ��
        /// </summary>
        public Action<DbDataReader, TEntity> DynamicLoadAction { get; set; }

        #region ʵ�����

        /// <summary>
        ///     ����������
        /// </summary>
        public async Task<bool> InsertAsync(TEntity entity)
        {
            await using var connectionScope = DataBase.CreateConnectionScope();

            if (!await InsertInnerAsync(entity))
                return false;
            await ReLoadInnerAsync(entity);
            return true;
        }

        /// <summary>
        ///     ����������
        /// </summary>
        public async Task<int> InsertAsync(IEnumerable<TEntity> entities)
        {
            var datas = entities as TEntity[] ?? entities.ToArray();
            int cnt = 0;
            await using var connectionScope = DataBase.CreateConnectionScope();

            foreach (var entity in datas)
            {
                if (await InsertInnerAsync(entity))
                    ++cnt;
                else
                    return 0;
            }
            foreach (var entity in datas)
                await ReLoadInnerAsync(entity);
            return cnt;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="entity">�������ݵ�ʵ��</param>
        protected async Task<bool> InsertInnerAsync(TEntity entity)
        {
            PrepareSave(entity, DataOperatorType.Insert);
            await using var connectionScope = DataBase.CreateConnectionScope();
            await using var cmd = CommandCreater.CreateCommand(connectionScope);

            SqlBuilder.SqlOption.SetEntityParameter(cmd, entity);

            DataBase.TraceSql(cmd);
            if (SqlBuilder.SqlOption.IsIdentity)
            {
                var key = await cmd.ExecuteScalarAsync();
                if (key == DBNull.Value || key == null)
                    return false;
                entity.SetValue(SqlBuilder.SqlOption.KeyField, key);
            }
            else if (await cmd.ExecuteNonQueryAsync() == 0)
                return false;

            var sql = SqlBuilder.AfterUpdateSql(SqlBuilder.PrimaryKeyConditionSQL);
            if (!string.IsNullOrEmpty(sql))
            {
                await DataBase.ExecuteAsync(sql, ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, entity.GetValue(SqlBuilder.SqlOption.KeyField), SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
            }
            await EndSaved(entity, DataOperatorType.Insert);
            return true;
        }


        /// <summary>
        ///     ��������
        /// </summary>
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            await using var connectionScope = DataBase.CreateConnectionScope();
            if (!await UpdateInnerAsync(entity))
                return false;
            await ReLoadInnerAsync(entity);
            return true;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        public async Task<int> UpdateAsync(IEnumerable<TEntity> entities)
        {
            var datas = entities as TEntity[] ?? entities.ToArray();
            int cnt = 0;
            await using var connectionScope = DataBase.CreateConnectionScope();
            foreach (var entity in datas)
            {
                if (await UpdateInnerAsync(entity))
                    ++cnt;
            }
            foreach (var entity in datas)
                await ReLoadInnerAsync(entity);
            return cnt;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        private async Task<bool> ReLoadInnerAsync(TEntity entity)
        {
            entity.__status.RejectChanged();
            await using var connectionScope = DataBase.CreateConnectionScope();
            var para = ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, entity.GetValue(SqlBuilder.SqlOption.KeyField), SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField));
            await using var cmd = CreateLoadCommand(connectionScope, SqlBuilder.PrimaryKeyConditionSQL, para);
            await using var reader = (DbDataReader)await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return false;
            using (new EntityLoadScope(entity))
            {
                if (DynamicLoadAction != null)
                    DynamicLoadAction(reader, entity);
                else
                    SqlBuilder.SqlOption.LoadEntity(reader, entity);
            }

            var entity2 = EntityLoaded(entity);
            if (entity != entity2)
                entity.CopyValue(entity2);
            return true;
        }

        /// <summary>
        ///     ɾ������
        /// </summary>
        public async Task<int> DeleteAsync(IEnumerable<TEntity> entities)
        {
            var datas = entities as TEntity[] ?? entities.ToArray();
            int cnt = 0;
            await using var connectionScope = DataBase.CreateConnectionScope();
            foreach (var entity in datas)
            {
                if (await UpdateInnerAsync(entity))
                    ++cnt;
                else
                    return 0;
            }
            foreach (var entity in datas)
                await ReLoadInnerAsync(entity);
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
            entity.__status.IsDelete = true;
            PrepareSave(entity, DataOperatorType.Delete);

            var para = ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, entity.GetValue(SqlBuilder.SqlOption.KeyField), SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField));
            var result = await DeleteInnerAsync(SqlBuilder.PrimaryKeyConditionSQL, para);
            if (result == 0)
                return false;
            await EndSaved(entity, DataOperatorType.Delete);
            return true;
        }


        /// <summary>
        ///     ��������
        /// </summary>
        public async Task<int> SaveAsync(IEnumerable<TEntity> entities)
        {
            int cnt = 0;
            await using var connectionScope = DataBase.CreateConnectionScope();
            var datas = entities as TEntity[] ?? entities.ToArray();
            foreach (var entity in datas)
            {
                if (await SaveInnerAsync(entity))
                    ++cnt;
                else
                    return 0;
            }
            foreach (var entity in datas)
                await ReLoadInnerAsync(entity);
            return cnt;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        public async Task<bool> SaveAsync(TEntity entity)
        {
            return await SaveInnerAsync(entity);
        }

        /// <summary>
        ///     ����
        /// </summary>
        private async Task<bool> SaveInnerAsync(TEntity entity)
        {
            if (entity.__status.IsDelete)
                return await DeleteInnerAsync(entity);
            else if (entity.__status.IsNew || !ExistPrimaryKey(entity.GetValue(SqlBuilder.SqlOption.KeyField)))
                return await InsertInnerAsync(entity);
            else
                return await UpdateInnerAsync(entity);
        }


        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="entity">�������ݵ�ʵ��</param>
        private async Task<bool> UpdateInnerAsync(TEntity entity)
        {
            string sql;
            if (UpdateByMidified)
            {
                if (UpdateByMidified && !entity.__status.IsModified)
                    return false;
                PrepareSave(entity, DataOperatorType.Update);
                sql = SqlBuilder.GetModifiedUpdateSql(entity);
            }
            else
            {
                PrepareSave(entity, DataOperatorType.Update);
                sql = SqlBuilder.GetFullUpdateSql(entity);
            }
            if (sql == null)
                return false;

            await using var connectionScope = DataBase.CreateConnectionScope();
            await using var cmd = CommandCreater.CreateCommand(connectionScope);
            SqlBuilder.SqlOption.SetEntityParameter(cmd, entity);
            
            cmd.CommandText = SqlBuilder.CreateUpdateSql(sql, SqlBuilder.PrimaryKeyConditionSQL);

            DataBase.TraceSql(cmd);

            if (await cmd.ExecuteNonQueryAsync() <= 0)
            {
                return false;
            }

            await EndSaved(entity, DataOperatorType.Update);
            return true;
        }

        #endregion

        #region ɾ��

        /// <summary>
        ///     ɾ������
        /// </summary>
        public async Task<bool> DeletePrimaryKeyAsync(object key)
        {
            await DeleteInnerAsync(SqlBuilder.PrimaryKeyConditionSQL, ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, key, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
            await OnKeyEvent(DataOperatorType.Delete, key);
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
            var convert = Compile(lambda);
            return await DeleteByConditionAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ����ɾ������
        /// </summary>
        public async Task<bool> PhysicalDeleteAsync(object key)
        {
            var condition = SqlBuilder.PrimaryKeyConditionSQL;
            var para = ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, key, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField));
            var paras = new[] { para };
            OnOperatorExecuting(condition, paras, DataOperatorType.Delete);
            var result = await DataBase.ExecuteAsync(SqlBuilder.PhysicalDeleteSql(condition), para);
            if (result == 0)
                return false;
            OnOperatorExecuted(condition, paras, DataOperatorType.Delete);
            await OnKeyEvent(DataOperatorType.Delete, key);
            return true;
        }

        /// <summary>
        ///     ǿ������ɾ��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ�ɾ���ɹ�</returns>
        public async Task<int> PhysicalDeleteAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            int cnt;
            OnOperatorExecuting(convert.ConditionSql, convert.Parameters, DataOperatorType.MulitDelete);

            cnt = await DataBase.ExecuteAsync(SqlBuilder.PhysicalDeleteSql(convert.ConditionSql), convert.Parameters);

            if (cnt == 0)
                return 0;
            OnOperatorExecuted(convert.ConditionSql, convert.Parameters, DataOperatorType.MulitDelete);
            await OnMulitUpdateEvent(DataOperatorType.MulitDelete, convert.ConditionSql, convert.Parameters);
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
            OnOperatorExecuting(condition, args, DataOperatorType.Delete);
            cnt = await DeleteInnerAsync(condition, args);
            if (cnt == 0)
                return 0;
            OnOperatorExecuted(condition, args, DataOperatorType.Delete);
            await OnMulitUpdateEvent(DataOperatorType.MulitDelete, condition, args);
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
            int re = await SetValueInnerAsync(field, value, SqlBuilder.PrimaryKeyConditionSQL, ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, key, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
            if (re > 0)
                await OnKeyEvent(DataOperatorType.Update, key);
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
                ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, key, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
            if (re > 0)
                await OnKeyEvent(DataOperatorType.Update, key);
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
            var arg2 = new List<DbParameter>
            {
                ParameterCreater.CreateFieldParameter(SqlBuilder.SqlOption.KeyField, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField), key)
            };
            int result;
            OnOperatorExecuting(condition, arg2, DataOperatorType.Update);
            result = await DataBase.ExecuteAsync(sql, arg2);
            if (result == 0)
                return 0;
            OnOperatorExecuted(condition, arg2, DataOperatorType.Update);
            await OnKeyEvent(DataOperatorType.Delete, key);
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
            if (UpdateByMidified && !entity.__status.IsModified)
                return -1;
            var setValueSql = SqlBuilder.GetModifiedUpdateSql(entity);
            if (setValueSql == null)
                return -1;
            var convert = Compile(lambda);
            await using var connectionScope = DataBase.CreateConnectionScope();
            await using var cmd = CommandCreater.CreateCommand(connectionScope);
            SqlBuilder.SqlOption.SetEntityParameter(cmd, entity);
            cmd.CommandText = SqlBuilder.CreateUpdateSql(setValueSql, convert.ConditionSql);
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
            var convert = Compile(lambda);
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
            await OnMulitUpdateEvent(DataOperatorType.MulitUpdate, condition, args);
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
            field = SqlBuilder.SqlOption.FieldMap[field];

            var parameters = new List<DbParameter>();
            if (args != null)
                parameters.AddRange(args);
            var sql = SqlBuilder.CreateUpdateSql(SqlBuilder.FileUpdateSql(field, value, parameters), condition);

            int result;
            OnOperatorExecuting(condition, args, DataOperatorType.Update);
            result = await DataBase.ExecuteAsync(sql, parameters.ToArray());
            if (result <= 0)
                return 0;
            OnOperatorExecuted(condition, args, DataOperatorType.MulitUpdate);
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
            var convert = Compile(condition);
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
            var convert = Compile(condition);
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
            OnOperatorExecuting(condition, args, DataOperatorType.MulitUpdate);

            result = await DataBase.ExecuteAsync(sql, args);
            if (result == 0)
                return 0;
            OnOperatorExecuted(condition, args, DataOperatorType.MulitUpdate);
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
            var convert = Compile(lambda);
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
                code.Append(SqlBuilder.FileUpdateSql(SqlBuilder.SqlOption.FieldMap[field.field], field.value, parameters));
            }

            var sql = SqlBuilder.CreateUpdateSql(code.ToString(), condition);

            int result;
            OnOperatorExecuting(condition, args, DataOperatorType.Update);
            result = await DataBase.ExecuteAsync(sql, parameters.ToArray());
            if (result <= 0)
                return 0;
            OnOperatorExecuted(condition, args, DataOperatorType.MulitUpdate);
            return result;
        }
        #endregion

        #region ��������

        /// <summary>
        /// �첽����
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsync(DbOperatorContext context, TEntity entity)
        {
            PrepareSave(entity, DataOperatorType.Insert);
            SetParameterValue(entity, context.Command);
            if (context.IsIdentitySql)
            {
                var key = await context.Command.ExecuteScalarAsync();
                if (key == DBNull.Value || key == null)
                    return false;
                entity.SetValue(SqlBuilder.SqlOption.KeyField, key);
            }
            else
            {
                var res = await context.Command.ExecuteNonQueryAsync();
                if (res == 0)
                    return false;
            }

            var sql = SqlBuilder.AfterUpdateSql(SqlBuilder.PrimaryKeyConditionSQL);
            if (!string.IsNullOrEmpty(sql))
            {
                await DataBase.ExecuteAsync(sql, 
                    ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, entity.GetValue(SqlBuilder.SqlOption.KeyField), SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
            }

            await EndSaved(entity, DataOperatorType.Insert);
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
        /// �첽����
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(DbOperatorContext context, TEntity entity)
        {
            PrepareSave(entity, DataOperatorType.Update);
            SetParameterValue(entity, context.Command);
            var res = await context.Command.ExecuteNonQueryAsync();
            if (res == 0)
                return false;
            var sql = SqlBuilder.AfterUpdateSql(SqlBuilder.PrimaryKeyConditionSQL);
            if (!string.IsNullOrEmpty(sql))
            {
                DataBase.Execute(sql,ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, entity.GetValue(SqlBuilder.SqlOption.KeyField), SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
            }

            await EndSaved(entity, DataOperatorType.Update);
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