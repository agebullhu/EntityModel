// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

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
        #region ʵ�����

        static MySqlTable()
        {
            DataUpdateHandler.InitType<TData>();
        }

        /// <summary>
        ///     ����������
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
        ///     ����������
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
        ///     ��������
        /// </summary>
        /// <param name="entity">�������ݵ�ʵ��</param>
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
        ///     ��������
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
        ///     ��������
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
        ///     ��������
        /// </summary>
        /// <param name="entity">�������ݵ�ʵ��</param>
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
        ///     ɾ������
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
        ///     ɾ������
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
        ///     ɾ��
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
        ///     ��������
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
        ///     ��������
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
        ///     ����
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
        ///     ����ǰ����(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="operatorType">��������</param>
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
        ///     ������ɺ��ڴ���(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="operatorType">��������</param>
        /// <remarks>
        ///     �Ե�ǰ��������Եĸ���,�����б���,���򽫶�ʧ
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

        #region ɾ��

        /// <summary>
        ///     ɾ������
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
        ///     ����ɾ��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public int Delete(Expression<Func<TData, bool>> lambda)
        {
            //throw new Exception("����ɾ�����ܱ�����");
            var convert = Compile(lambda);
            return DeleteByCondition(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ����ɾ������
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
        ///     ǿ������ɾ��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ�ɾ���ɹ�</returns>
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
        ///     ����ɾ��
        /// </summary>
        public int Delete(string condition, params MySqlParameter[] args)
        {
            //throw new Exception("����ɾ�����ܱ�����");
            if (string.IsNullOrWhiteSpace(condition))
                throw new ArgumentException(@"ɾ����������Ϊ��,��Ϊ������ִ��ȫ��ɾ��", GetType().FullName);
            return DeleteByCondition(condition, args);
        }


        /// <summary>
        ///     ����ɾ��
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
        ///     ɾ��
        /// </summary>
        private int DeleteInner(string condition, params MySqlParameter[] args)
        {
            if (string.IsNullOrEmpty(DeleteSqlCode))
                return 0;
            if (!string.IsNullOrEmpty(condition))
                return DataBase.Execute(CreateDeleteSql(condition), args);
            throw new ArgumentException(@"ɾ����������Ϊ��,��Ϊ������ִ��ȫ��ɾ��", GetType().FullName);
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
            int re = SetValueInner(field, value, PrimaryKeyConditionSQL, CreatePimaryKeyParameter(key));
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
        public int SetValue<TField, TKey>(Expression<Func<TData, TField>> fieldExpression, TField value, TKey key)
        {
            int re = SetValueInner(GetPropertyName(fieldExpression), value, PrimaryKeyConditionSQL, CreatePimaryKeyParameter(key));
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
            var condition = FieldConditionSQL(true, conditionFiles);
            SetValueByCondition(field, value, condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        public int SetValue(string field, object value, string condition, params MySqlParameter[] args)
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
        public int SetValue(Expression<Func<TData, bool>> field, bool value, string condition, params MySqlParameter[] args)
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
        public int SetValue(Expression<Func<TData, Enum>> field, Enum value, string condition,
            params MySqlParameter[] args)
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
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value, string condition,
            params MySqlParameter[] args)
        {
            return SetValueByCondition(GetPropertyName(field), value, condition, args);
        }

        /// <summary>
        ///     ȫ������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <returns>��������</returns>
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value)
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
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value, Expression<Func<TData, bool>> lambda)
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
        private int SetValueByCondition(string field, object value, string condition, params MySqlParameter[] args)
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
        private int SetValueInner(string field, object value, string condition, params MySqlParameter[] args)
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
        ///     �Զ�����£����±��ʽ��д��
        /// </summary>
        /// <param name="expression">����SQL���ʽ</param>
        /// <param name="condition">����</param>
        /// <returns>��������</returns>
        public int SetValue(string expression, Expression<Func<TData, bool>> condition)
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
        ///     �Զ�����£����±��ʽ��д��
        /// </summary>
        /// <param name="expression">����SQL���ʽ</param>
        /// <param name="condition">����</param>
        /// <param name="args">����</param>
        /// <returns>��������</returns>
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

        #region Sqlע��

        /// <summary>
        ///     �����ͬʱִ�е�SQL(����֮ǰ����ִ��)
        /// </summary>
        /// <param name="condition">��ǰ������ִ������</param>
        /// <returns></returns>
        private string BeforeUpdateSql(string condition)
        {
            var code = new StringBuilder();
            BeforeUpdateSql(code, condition);
            DataUpdateHandler.BeforeUpdateSql(this,code, TableId, condition);
            return code.ToString();
        }

        /// <summary>
        ///     �����ͬʱִ�е�SQL(����֮������ִ��)
        /// </summary>
        /// <param name="condition">��ǰ������ִ������</param>
        /// <returns></returns>
        private string AfterUpdateSql(string condition)
        {
            var code = new StringBuilder();
            AfterUpdateSql(code, condition);
            DataUpdateHandler.AfterUpdateSql(this, code, TableId, condition);
            return code.ToString();
        }


        /// <summary>
        ///     �����ͬʱִ�е�SQL(����֮ǰ����ִ��)
        /// </summary>
        /// <param name="code">д��SQL���ı�������</param>
        /// <param name="condition">��ǰ������ִ������</param>
        /// <returns></returns>
        protected virtual void BeforeUpdateSql(StringBuilder code, string condition)
        {
        }

        /// <summary>
        ///     �����ͬʱִ�е�SQL(����֮������ִ��)
        /// </summary>
        /// <param name="code">д��SQL���ı�������</param>
        /// <param name="condition">��ǰ������ִ������</param>
        protected virtual void AfterUpdateSql(StringBuilder code, string condition)
        {
        }

        #endregion

        #region ������չ



        /// <summary>
        ///     ����ǰ����
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="operatorType">��������</param>
        protected virtual void OnPrepareSave(DataOperatorType operatorType, TData entity)
        {

        }


        /// <summary>
        ///     ������ɺ��ڴ���
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="operatorType">��������</param>
        protected virtual void OnDataSaved(DataOperatorType operatorType, TData entity)
        {

        }


        /// <summary>
        ///    �������ǰ����(����ʵ�����������)
        /// </summary>
        /// <param name="condition">ִ������</param>
        /// <param name="args">����ֵ</param>
        /// <param name="operatorType">��������</param>
        protected virtual void OnOperatorExecuting(DataOperatorType operatorType, string condition, IEnumerable<MySqlParameter> args)
        {
        }

        /// <summary>
        ///     ����������(����ʵ�����������)
        /// </summary>
        /// <param name="condition">ִ������</param>
        /// <param name="args">����ֵ</param>
        /// <param name="operatorType">��������</param>
        protected virtual void OnOperatorExecutd(DataOperatorType operatorType, string condition, IEnumerable<MySqlParameter> args)
        {
        }

        /// <summary>
        ///     ����ǰ����
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="operatorType">��������</param>
        private void OnPrepareSave(TData entity, DataOperatorType operatorType)
        {
            OnPrepareSave(operatorType, entity);
            DataUpdateHandler.OnPrepareSave(entity, operatorType);
        }

        /// <summary>
        ///     ������ɺ��ڴ���
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="operatorType">��������</param>
        private void OnDataSaved(TData entity, DataOperatorType operatorType)
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
        private void OnOperatorExecuting(string condition, IEnumerable<MySqlParameter> args, DataOperatorType operatorType)
        {
            var sqlParameters = args as MySqlParameter[] ?? args.ToArray();
            OnOperatorExecuting(operatorType, condition, sqlParameters);
            DataUpdateHandler.OnOperatorExecuting(TableId, condition, sqlParameters, operatorType);
        }

        /// <summary>
        ///     ����������(����ʵ�����������)
        /// </summary>
        /// <param name="condition">ִ������</param>
        /// <param name="args">����ֵ</param>
        /// <param name="operatorType">��������</param>
        private void OnOperatorExecutd(string condition, IEnumerable<MySqlParameter> args, DataOperatorType operatorType)
        {
            var mySqlParameters = args as MySqlParameter[] ?? args.ToArray();
            OnOperatorExecutd(operatorType, condition, mySqlParameters);
            DataUpdateHandler.OnOperatorExecutd(TableId, condition, mySqlParameters, operatorType);
        }

        #endregion

        #region ���ݸ����¼�֧��

        private bool _globalEvent;

        /// <summary>
        /// �Ƿ�����ȫ���¼�(��ȫ���¼���,����Ϊ��)
        /// </summary>
        public virtual bool GlobalEvent
        {
            get => _globalEvent && DataUpdateHandler.EventProxy != null;
            set => _globalEvent = value;
        }

        /// <summary>
        ///     ����������(����ʵ�����������)
        /// </summary>
        /// <param name="operatorType">��������</param>
        /// <param name="key">��������</param>
        private void OnKeyEvent(DataOperatorType operatorType, object key)
        {
            if (GlobalEvent)
                DataUpdateHandler.EventProxy.OnStatusChanged(DataBase.Name, Name, operatorType, EntityEventValueType.Key, key?.ToString());
        }

        /// <summary>
        ///     ����������(����ʵ�����������)
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
        ///     ����������(����ʵ�����������)
        /// </summary>
        /// <param name="operatorType">��������</param>
        /// <param name="entity">��������</param>
        private void OnEvent(DataOperatorType operatorType, TData entity)
        {
            if (GlobalEvent)
                DataUpdateHandler.EventProxy.OnStatusChanged(DataBase.Name, Name, operatorType, EntityEventValueType.EntityJson, JsonConvert.SerializeObject(entity));
        }

        #endregion

    }
}