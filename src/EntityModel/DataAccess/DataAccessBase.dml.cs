// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using Agebull.EntityModel.Common;
using Agebull.EntityModel.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
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
        public bool Insert(TEntity entity)
        {
            //await using (var scope = TransactionScope.CreateScope(DataBase))
            {
                if (!InsertInner(entity))
                    return false;
                ReLoadInner(entity);
                //scope.Succeed();
            }
            return true;
        }

        /// <summary>
        ///     ����������
        /// </summary>
        public int Insert(IEnumerable<TEntity> entities)
        {
            var datas = entities as TEntity[] ?? entities.ToArray();
            int cnt = 0;
            //await using (var scope = TransactionScope.CreateScope(DataBase))
            {
                foreach (var entity in datas)
                {
                    if (InsertInner(entity))
                        ++cnt;
                    else
                        return 0;
                }
                //scope.Succeed();
            }

            foreach (var entity in datas)
                ReLoadInner(entity);
            return cnt;
        }
        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="entity">�������ݵ�ʵ��</param>
        protected bool InsertInner(TEntity entity)
        {
            PrepareSave(entity, DataOperatorType.Insert);
            //using (TransactionScope.CreateScope(DataBase))
            {
                using var scope = DataBase.CreateConnectionScope();
                using var cmd = CommandCreater.CreateCommand(scope);
                SqlBuilder.SqlOption.SetEntityParameter(cmd, entity);

                DataBase.TraceSql(cmd);
                if (SqlBuilder.SqlOption.IsIdentity)
                {
                    var key = cmd.ExecuteScalar(); ;
                    if (key == DBNull.Value || key == null)
                        return false;
                    entity.SetValue(SqlBuilder.SqlOption.KeyField, key);
                }
                else
                {
                    if (cmd.ExecuteNonQuery() == 0)
                        return false;
                }

                var sql = SqlBuilder.AfterUpdateSql(SqlBuilder.PrimaryKeyConditionSQL);
                if (!string.IsNullOrEmpty(sql))
                {
                    DataBase.Execute(sql, ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, entity.GetValue(SqlBuilder.SqlOption.KeyField), SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
                }
            }

            EndSaved(entity, DataOperatorType.Insert);
            return true;
        }


        /// <summary>
        ///     ��������
        /// </summary>
        public bool Update(TEntity entity)
        {
            //await using (var scope = TransactionScope.CreateScope(DataBase))
            {
                if (!UpdateInner(entity))
                    return false;
                //scope.Succeed();
            }
            ReLoadInner(entity);
            return true;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        public int Update(IEnumerable<TEntity> entities)
        {
            var datas = entities as TEntity[] ?? entities.ToArray();
            int cnt = 0;
            //await using (var scope = TransactionScope.CreateScope(DataBase))
            {
                foreach (var entity in datas)
                {
                    if (UpdateInner(entity))
                        ++cnt;
                }
                //scope.Succeed();
            }

            foreach (var entity in datas)
                ReLoadInner(entity);
            return cnt;
        }

        /// <summary>
        ///     ɾ������
        /// </summary>
        public int Delete(IEnumerable<TEntity> entities)
        {
            var datas = entities as TEntity[] ?? entities.ToArray();
            int cnt = 0;
            //await using (var scope = TransactionScope.CreateScope(DataBase))
            {
                foreach (var entity in datas)
                {
                    if (UpdateInner(entity))
                        ++cnt;
                    else
                        return 0;
                }
                //scope.Succeed();
            }
            return cnt;
        }


        /// <summary>
        ///     ɾ������
        /// </summary>
        public bool Delete(TEntity entity)
        {
            //await using (var scope = TransactionScope.CreateScope(DataBase))
            {
                if (!DeleteInner(entity))
                    return false;
                //scope.Succeed();
            }
            return true;
        }

        /// <summary>
        ///     ɾ��
        /// </summary>
        private bool DeleteInner(TEntity entity)
        {
            if (entity == null)
                return false;
            entity.__status.IsDelete = true;
            //using (TransactionScope.CreateScope(DataBase))
            {
                PrepareSave(entity, DataOperatorType.Delete);
                var para = ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, entity.GetValue(SqlBuilder.SqlOption.KeyField), SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField));
                var result = DeleteInner(SqlBuilder.PrimaryKeyConditionSQL, para);
                if (result == 0)
                    return false;
                EndSaved(entity, DataOperatorType.Delete);
            }
            return true;
        }


        /// <summary>
        ///     ��������
        /// </summary>
        public int Save(IEnumerable<TEntity> entities)
        {
            var datas = entities as TEntity[] ?? entities.ToArray();
            int cnt = 0;
            //await using (var scope = TransactionScope.CreateScope(DataBase))
            {
                foreach (var entity in datas)
                {
                    if (SaveInner(entity))
                        ++cnt;
                    else
                        return 0;
                }
                //scope.Succeed();
            }

            foreach (var entity in datas)
                ReLoadInner(entity);
            return cnt;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        public bool Save(TEntity entity)
        {
            //await using (var scope = TransactionScope.CreateScope(DataBase))
            {
                if (!SaveInner(entity))
                    return false;
                //scope.Succeed();
            }
            return true;
        }

        /// <summary>
        ///     ����
        /// </summary>
        private bool SaveInner(TEntity entity)
        {
            if (entity.__status.IsDelete)
                return DeleteInner(entity);
            if (entity.__status.IsNew || !ExistPrimaryKey(entity.GetValue(SqlBuilder.SqlOption.KeyField)))
                return InsertInner(entity);
            return UpdateInner(entity);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="entity">�������ݵ�ʵ��</param>
        private bool UpdateInner(TEntity entity)
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

            using var scope = DataBase.CreateConnectionScope();
            using var cmd = CommandCreater.CreateCommand(scope);
            SqlBuilder.SqlOption.SetEntityParameter(cmd, entity);
            cmd.CommandText = SqlBuilder.CreateUpdateSql(sql, SqlBuilder.PrimaryKeyConditionSQL);

            DataBase.TraceSql(cmd);

            var result = cmd.ExecuteNonQuery();
            if (result <= 0)
            {
                return false;
            }
            EndSaved(entity, DataOperatorType.Update);
            return true;
        }
        /// <summary>
        ///     ����ǰ����(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="operatorType">��������</param>
        protected void PrepareSave(TEntity entity, DataOperatorType operatorType)
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
        ///     ������ɺ��ڴ���(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="operatorType">��������</param>
        /// <remarks>
        ///     �Ե�ǰ��������Եĸ���,�����б���,���򽫶�ʧ
        /// </remarks>
        private Task EndSaved(TEntity entity, DataOperatorType operatorType)
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
                entity.__status.AcceptChanged();
            }
            OnDataSaved(entity, operatorType);
            return OnEvent(operatorType, entity);
        }
        #endregion

        #region ɾ��

        /// <summary>
        ///     ɾ������
        /// </summary>
        public bool DeletePrimaryKey(object key)
        {
            //using (TransactionScope.CreateScope(DataBase))
            {
                var cnt = DeleteInner(SqlBuilder.PrimaryKeyConditionSQL, ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, key, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
                if (cnt == 0)
                    return false;
                OnKeyEvent(DataOperatorType.Delete, key);
            }
            return true;
        }


        /// <summary>
        ///     ����ɾ��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public int Delete(Expression<Func<TEntity, bool>> lambda)
        {
            //throw new EntityModelDbException("����ɾ�����ܱ�����");
            var convert = Compile(lambda);
            return DeleteByCondition(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ����ɾ������
        /// </summary>
        public bool PhysicalDelete(object key)
        {
            var condition = SqlBuilder.PrimaryKeyConditionSQL;
            var para = ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, key, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField));
            var paras = new[] { para };
            //await using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(condition, paras, DataOperatorType.Delete);
                var result = DataBase.Execute(SqlBuilder.PhysicalDeleteSql(condition), para);
                if (result == 0)
                    return false;
                OnOperatorExecuted(condition, paras, DataOperatorType.Delete);
                //scope.Succeed();
            }

            OnKeyEvent(DataOperatorType.Delete, key);
            return true;
        }
        /// <summary>
        ///     ǿ������ɾ��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ�ɾ���ɹ�</returns>
        public int PhysicalDelete(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            int cnt;
            //await using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(convert.ConditionSql, convert.Parameters, DataOperatorType.MulitDelete);
                cnt = DataBase.Execute(SqlBuilder.PhysicalDeleteSql(convert.ConditionSql), convert.Parameters);
                if (cnt == 0)
                    return 0;
                OnOperatorExecuted(convert.ConditionSql, convert.Parameters, DataOperatorType.MulitDelete);
                //scope.Succeed();
            }
            OnMulitUpdateEvent(DataOperatorType.MulitDelete, convert.ConditionSql, convert.Parameters);
            return cnt;
        }

        /// <summary>
        ///     ����ɾ��
        /// </summary>
        public int Delete(string condition, params DbParameter[] args)
        {
            //throw new EntityModelDbException("����ɾ�����ܱ�����");
            if (string.IsNullOrWhiteSpace(condition))
                throw new EntityModelDbException(@"ɾ����������Ϊ��,��Ϊ������ִ��ȫ��ɾ��");
            return DeleteByCondition(condition, args);
        }


        /// <summary>
        ///     ����ɾ��
        /// </summary>
        private int DeleteByCondition(string condition, DbParameter[] args)
        {
            int cnt;
            //await using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(condition, args, DataOperatorType.Delete);
                cnt = DeleteInner(condition, args);
                if (cnt == 0)
                    return 0;
                OnOperatorExecuted(condition, args, DataOperatorType.Delete);
                //scope.Succeed();
            }
            OnMulitUpdateEvent(DataOperatorType.MulitDelete, condition, args);
            return cnt;
        }

        /// <summary>
        ///     ɾ��
        /// </summary>
        private int DeleteInner(string condition, params DbParameter[] args)
        {
            if (!string.IsNullOrEmpty(condition))
                return DataBase.Execute(SqlBuilder.CreateDeleteSql(condition), args);
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
        public int SetValue(string field, object value, object key)
        {
            int re = SetValueInner(field, value, SqlBuilder.PrimaryKeyConditionSQL, ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, key, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
            if (re > 0)
                OnKeyEvent(DataOperatorType.Update, key);
            return re;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="key">����</param>
        /// <returns>��������</returns>
        public int SetValue<TField, TKey>(Expression<Func<TEntity, TField>> fieldExpression, TField value, TKey key)
        {
            int re = SetValueInner(GetPropertyName(fieldExpression), value, SqlBuilder.PrimaryKeyConditionSQL, ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, key, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
            if (re > 0)
                OnKeyEvent(DataOperatorType.Update, key);
            return re;
        }

        /// <summary>
        ///     ����ֶΰ��Զ�����ʽ����ֵ
        /// </summary>
        /// <param name="valueExpression">ֵ��SQL��ʽ</param>
        /// <param name="key">����</param>
        /// <returns>��������</returns>
        public int SetCoustomValue<TKey>(string valueExpression, TKey key)
        {
            var condition = SqlBuilder.PrimaryKeyConditionSQL;
            var sql = SqlBuilder.CreateUpdateSql(valueExpression, condition);
            var arg2 = new List<DbParameter>
            {
                ParameterCreater.CreateFieldParameter(SqlBuilder.SqlOption.KeyField,SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField), key)
            };
            int result;
            //await using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(condition, arg2, DataOperatorType.Update);
                result = DataBase.Execute(sql, arg2);
                if (result == 0)
                    return 0;
                OnOperatorExecuted(condition, arg2, DataOperatorType.Update);
                //scope.Succeed();
            }
            OnKeyEvent(DataOperatorType.Delete, key);
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
        public void SaveValue(string field, object value, string[] conditionFiles, object[] conditionValues)
        {
            var args = CreateFieldsParameters(conditionFiles, conditionValues);
            var condition = SqlBuilder.FieldConditionSQL(true, conditionFiles);
            SetValueByCondition(field, value, condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�����ֶ�</param>
        /// <param name="conditions">����������ֵ</param>
        /// <returns>��������</returns>
        /// <remarks>
        /// ������ʹ��AND���,��Ϊ����
        /// </remarks>
        public void SaveValue((string field, object value) field, (string field, object value)[] conditions)
        {
            if (conditions == null || conditions.Length == 0)
                throw new ArgumentException(@"û���ֶ��������ɲ���", nameof(conditions));
            var args = new DbParameter[conditions.Length];
            for (var i = 0; i < conditions.Length; i++)
                args[i] = ParameterCreater.CreateFieldParameter(conditions[i].field, SqlBuilder.GetDbType(conditions[i].field), conditions[i].value);


            var condition = SqlBuilder.FieldConditionSQL(true, conditions);
            SetValueByCondition(field.field, field.value, condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        public int SetValue(string field, object value, string condition, params DbParameter[] args)
        {
            return SetValueByCondition(field, value, condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        public int SetValue(Expression<Func<TEntity, bool>> field, bool value, string condition, params DbParameter[] args)
        {
            return SetValueByCondition(GetPropertyName(field), value ? 0 : 1, condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        public int SetValue(Expression<Func<TEntity, Enum>> field, Enum value, string condition,
            params DbParameter[] args)
        {
            return SetValueByCondition(GetPropertyName(field), Convert.ToInt32(value), condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        public int SetValue<TField>(Expression<Func<TEntity, TField>> field, TField value, string condition,
            params DbParameter[] args)
        {
            return SetValueByCondition(GetPropertyName(field), value, condition, args);
        }

        /// <summary>
        ///     ��������ʵ�����Ѽ�¼���²���
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="lambda">����</param>
        /// <returns>��������</returns>
        public int SetValue(TEntity entity, Expression<Func<TEntity, bool>> lambda)
        {
            if (UpdateByMidified && !entity.__status.IsModified)
                return -1;
            var setValueSql = SqlBuilder.GetModifiedUpdateSql(entity);
            if (setValueSql == null)
                return -1;
            var convert = Compile(lambda);
            var sql = SqlBuilder.CreateUpdateSql(setValueSql, convert.ConditionSql);
            int result;
            //using (TransactionScope.CreateScope(DataBase))
            {
                using var scope = DataBase.CreateConnectionScope();
                using var cmd = CommandCreater.CreateCommand(scope);
                SqlBuilder.SqlOption.SetEntityParameter(cmd, entity);
                cmd.CommandText = SqlBuilder.CreateUpdateSql(sql, convert.ConditionSql);
                DataBase.TraceSql(cmd);
                result = cmd.ExecuteNonQuery();
            }

            return result;
        }

        /// <summary>
        ///     ȫ������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <returns>��������</returns>
        public int SetValue<TField>(Expression<Func<TEntity, TField>> field, TField value)
        {
            return SetValueByCondition(GetPropertyName(field), value, "-1", null);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="lambda">����</param>
        /// <returns>��������</returns>
        public int SetValue<TField>(Expression<Func<TEntity, TField>> field, TField value, Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            return SetValueByCondition(GetPropertyName(field), value, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        private int SetValueByCondition(string field, object value, string condition, params DbParameter[] args)
        {
            int result = DoUpdateValue(field, value, condition, args);
            OnMulitUpdateEvent(DataOperatorType.MulitUpdate, condition, args);
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
        private int SetValueInner(string field, object value, string condition, params DbParameter[] args)
        {
            return DoUpdateValue(field, value, condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        private int DoUpdateValue(string field, object value, string condition, DbParameter[] args)
        {
            field = SqlBuilder.SqlOption.FieldMap[field];

            var parameters = new List<DbParameter>();
            if (parameters != null)
                parameters.AddRange(args);
            var sql = SqlBuilder.CreateUpdateSql(SqlBuilder.FileUpdateSql(field, value, parameters), condition);

            int result;
            //await using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(condition, args, DataOperatorType.Update);
                result = DataBase.Execute(sql, parameters.ToArray());
                if (result <= 0)
                    return 0;
                OnOperatorExecuted(condition, args, DataOperatorType.MulitUpdate);
                //scope.Succeed();
            }

            return result;
        }

        /// <summary>
        ///     �Զ�����£����±��ʽ��д��
        /// </summary>
        /// <param name="expression">����SQL���ʽ</param>
        /// <param name="condition">����</param>
        /// <returns>��������</returns>
        public int SetValue(string expression, Expression<Func<TEntity, bool>> condition)
        {
            var convert = Compile(condition);
            return SetMulitValue(expression, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     �Զ�����£����±��ʽ��д��
        /// </summary>
        /// <param name="expression">����SQL���ʽ</param>
        /// <param name="condition">����</param>
        /// <param name="args">����</param>
        /// <returns>��������</returns>
        public int SetMulitValue(string expression, Expression<Func<TEntity, bool>> condition, params DbParameter[] args)
        {
            var convert = Compile(condition);
            var arg = new List<DbParameter>();
            if (convert.HaseParameters)
                arg.AddRange(convert.DbParameter);
            if (args.Length > 0)
                arg.AddRange(args);
            return SetMulitValue(expression, convert.ConditionSql, arg.ToArray());
        }

        /// <summary>
        ///     �Զ�����£����±��ʽ��д��
        /// </summary>
        /// <param name="expression">����SQL���ʽ</param>
        /// <param name="condition">����</param>
        /// <param name="args">����</param>
        /// <returns>��������</returns>
        public int SetMulitValue(string expression, string condition, DbParameter[] args)
        {
            var sql = SqlBuilder.CreateUpdateSql(expression, condition);
            int result;
            //await using (var scope = TransactionScope.CreateScope(DataBase))
            {
                OnOperatorExecuting(condition, args, DataOperatorType.MulitUpdate);

                result = DataBase.Execute(sql, args);
                if (result == 0)
                    return 0;
                OnOperatorExecuted(condition, args, DataOperatorType.MulitUpdate);
                //scope.Succeed();
            }
            return result;
        }

        #endregion


        #region ������չ



        /// <summary>
        ///     ����ǰ����
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="operatorType">��������</param>
        protected virtual void OnPrepareSave(DataOperatorType operatorType, TEntity entity)
        {

        }


        /// <summary>
        ///     ������ɺ��ڴ���
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="operatorType">��������</param>
        protected virtual void OnDataSaved(DataOperatorType operatorType, TEntity entity)
        {

        }


        /// <summary>
        ///    �������ǰ����(����ʵ�����������)
        /// </summary>
        /// <param name="condition">ִ������</param>
        /// <param name="args">����ֵ</param>
        /// <param name="operatorType">��������</param>
        protected virtual void OnOperatorExecuting(DataOperatorType operatorType, string condition, IEnumerable<DbParameter> args)
        {
        }

        /// <summary>
        ///     ����������(����ʵ�����������)
        /// </summary>
        /// <param name="condition">ִ������</param>
        /// <param name="args">����ֵ</param>
        /// <param name="operatorType">��������</param>
        protected virtual void OnOperatorExecuted(DataOperatorType operatorType, string condition, IEnumerable<DbParameter> args)
        {
        }

        /// <summary>
        ///     ����ǰ����
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="operatorType">��������</param>
        private void OnPrepareSave(TEntity entity, DataOperatorType operatorType)
        {
            OnPrepareSave(operatorType, entity);
            DataUpdateHandler.OnPrepareSave(entity, operatorType);
        }

        /// <summary>
        ///     ������ɺ��ڴ���
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="operatorType">��������</param>
        private void OnDataSaved(TEntity entity, DataOperatorType operatorType)
        {
            OnDataSaved(operatorType, entity);
            DataUpdateHandler.OnDataSaved(entity, operatorType);
        }

        /// <summary>
        ///     �������ǰ����(����ʵ�����������)
        /// </summary>
        /// <param name="condition">ִ������</param>
        /// <param name="args">����ֵ</param>
        /// <param name="operatorType">��������</param>
        private void OnOperatorExecuting(string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
        {
            var sqlParameters = args as DbParameter[] ?? args.ToArray();
            OnOperatorExecuting(operatorType, condition, sqlParameters);
            DataUpdateHandler.OnOperatorExecuting(this, condition, sqlParameters, operatorType);
        }

        /// <summary>
        ///     ����������(����ʵ�����������)
        /// </summary>
        /// <param name="condition">ִ������</param>
        /// <param name="args">����ֵ</param>
        /// <param name="operatorType">��������</param>
        private void OnOperatorExecuted(string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
        {
            var mySqlParameters = args as DbParameter[] ?? args.ToArray();
            OnOperatorExecuted(operatorType, condition, mySqlParameters);
            DataUpdateHandler.OnOperatorExecuted(this, condition, mySqlParameters, operatorType);
        }

        #endregion

        #region ���ݸ����¼�֧��

        /// <summary>
        /// �Ƿ�����ȫ���¼�(��ȫ���¼���,����Ϊ��)
        /// </summary>
        public bool GlobalEvent
        {
            get;
            set;
        }

        /// <summary>
        ///     ����������(����ʵ�����������)
        /// </summary>
        /// <param name="operatorType">��������</param>
        /// <param name="key">��������</param>
        private Task OnKeyEvent(DataOperatorType operatorType, object key)
        {
            if (!GlobalEvent)
                return Task.CompletedTask;
            return DataUpdateHandler.OnStatusChanged(DataBase.Name, Name, operatorType, EntityEventValueType.Key, key?.ToString());
        }

        /// <summary>
        ///     ����������(����ʵ�����������)
        /// </summary>
        private Task OnMulitUpdateEvent(DataOperatorType operatorType, string condition, DbParameter[] args)
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
            return DataUpdateHandler.OnStatusChanged(DataBase.Name, Name, operatorType, EntityEventValueType.QueryCondition, JsonConvert.SerializeObject(queryCondition));
        }

        /// <summary>
        ///     ����������(����ʵ�����)
        /// </summary>
        /// <param name="operatorType">��������</param>
        /// <param name="entity">��������</param>
        private Task OnEvent(DataOperatorType operatorType, TEntity entity)
        {
            if (!GlobalEvent)
                return Task.CompletedTask;
            return DataUpdateHandler.OnStatusChanged(DataBase.Name, Name, operatorType, EntityEventValueType.EntityJson, JsonConvert.SerializeObject(entity));
        }

        #endregion


        #region ��������


        /// <summary>
        /// ���ò������ݵ�����
        /// </summary>
        /// <param name="data">ʵ�����</param>
        /// <param name="cmd">����</param>
        /// <returns>������˵��Ҫȡ����</returns>
        protected virtual void SetParameterValue(TEntity data, DbCommand cmd)
        {
            foreach (var pro in data.__Struct.Properties)
            {
                if (!SqlBuilder.SqlOption.FieldMap.ContainsKey(pro.Value.Name))
                    continue;
                cmd.Parameters[pro.Value.PropertyName].Value = data.GetValue(pro.Value.PropertyName);
            }
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <returns></returns>
        public DbOperatorContext BeginInsert()
        {
            var scope = DataBase.CreateConnectionScope();
            var ctx = new DbOperatorContext
            {
                Scope = scope,
                Command = CommandCreater.CreateCommand(scope)
            };
            var entity = new TEntity();
            SqlBuilder.SqlOption.SetEntityParameter(ctx.Command, entity);
            ctx.IsIdentitySql = SqlBuilder.SqlOption.IsIdentity;
            ctx.Command.Prepare();
            return ctx;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(DbOperatorContext context, TEntity entity)
        {
            PrepareSave(entity, DataOperatorType.Insert);
            SetParameterValue(entity, context.Command);
            if (context.IsIdentitySql)
            {
                var key = context.Command.ExecuteScalar(); ;
                if (key == DBNull.Value || key == null)
                    return false;
                entity.SetValue(SqlBuilder.SqlOption.KeyField, key);
            }
            else
            {
                if (context.Command.ExecuteNonQuery() == 0)
                    return false;
            }

            var sql = SqlBuilder.AfterUpdateSql(SqlBuilder.PrimaryKeyConditionSQL);
            if (!string.IsNullOrEmpty(sql))
            {

                DataBase.Execute(sql, ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, entity.GetValue(SqlBuilder.SqlOption.KeyField), SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
            }
            ReLoadInner(entity);

            EndSaved(entity, DataOperatorType.Insert);
            return true;
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool Insert(DbOperatorContext context, IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Insert(context, entity);
            }
            return true;
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <returns></returns>
        public DbOperatorContext BeginUpdate()
        {
            var scope = DataBase.CreateConnectionScope();
            var ctx = new DbOperatorContext
            {
                Scope = scope,
                Command = CommandCreater.CreateCommand(scope)
            };
            var entity = new TEntity();
            SqlBuilder.SqlOption.SetEntityParameter(ctx.Command, entity);
            ctx.IsIdentitySql = SqlBuilder.SqlOption.IsIdentity;
            ctx.Command.CommandText = SqlBuilder.SqlOption.UpdateSqlCode;
            ctx.Command.Prepare();
            return ctx;
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(DbOperatorContext context, TEntity entity)
        {
            PrepareSave(entity, DataOperatorType.Update);
            SetParameterValue(entity, context.Command);
            if (context.Command.ExecuteNonQuery() == 0)
                return false;
            var sql = SqlBuilder.AfterUpdateSql(SqlBuilder.PrimaryKeyConditionSQL);
            if (!string.IsNullOrEmpty(sql))
            {
                //using (TransactionScope.CreateScope(DataBase))
                DataBase.Execute(sql, ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, entity.GetValue(SqlBuilder.SqlOption.KeyField), SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
            }

            ReLoadInner(entity);
            EndSaved(entity, DataOperatorType.Update);
            return true;
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool Update(DbOperatorContext context, IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Update(context, entity);
            }
            return true;
        }

        #endregion
    }
}