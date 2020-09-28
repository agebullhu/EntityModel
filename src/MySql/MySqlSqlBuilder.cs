// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using Agebull.EntityModel.Common;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;

#endregion

namespace Agebull.EntityModel.MySql
{

    /// <summary>
    ///     Sqlʵ�������
    /// </summary>
    public sealed class MySqlSqlBuilder<TEntity> : ParameterCreater, ISqlBuilder<TEntity>
    where TEntity : class, new()
    {
        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public DataBaseType DataBaseType => DataBaseType.MySql;

        /// <summary>
        /// �Զ��������ݿ����
        /// </summary>
        public IDataOperator<TEntity> DataOperator => Provider.DataOperator;

        /// <summary>
        /// ��������
        /// </summary>
        public IParameterCreater ParameterCreater => Provider.ParameterCreater;

        /// <summary>
        /// Sql��乹����
        /// </summary>
        public DataAccessOption<TEntity> Option => Provider.Option;

        /// <summary>
        /// Sql��乹����
        /// </summary>
        public ISqlBuilder<TEntity> SqlBuilder => Provider.SqlBuilder;

        /// <summary>
        /// �����ṩ����Ϣ
        /// </summary>
        public DataAccessProvider<TEntity> Provider { get; set; }

        #region ���ݽṹ֧��

        /// <summary>
        /// ��ȡ���ֶ�
        /// </summary>
        /// <returns></returns>
        public string BuilderLoadFields()
        {
            var code = new StringBuilder();
            bool first = true;
            Option.FroeachDbProperties(ReadWriteFeatrue.Read, pro =>
            {
                if (first)
                    first = false;
                else
                    code.Append(',');
                code.Append('`');
                code.Append(pro.ColumnName);
                code.Append('`');
            });
            return code.ToString();
        }
        /// <summary>
        /// ȫ�����µ��ֶ�
        /// </summary>
        /// <returns></returns>
        public string BuilderUpdateFields()
        {
            var fields = new StringBuilder();
            bool first = true;
            Option.FroeachDbProperties(ReadWriteFeatrue.Update, pro =>
           {
               if (first)
                   first = false;
               else
                   fields.Append(',');

               fields.Append('`');
               fields.Append(pro.ColumnName);
               fields.Append("`=?");
               fields.Append(pro.PropertyName);
           });
            return fields.ToString();
        }

        /// <summary>
        /// ����Ĵ���
        /// </summary>
        /// <returns></returns>
        public string BuilderInsertSqlCode()
        {
            var fields = new StringBuilder();
            var paras = new StringBuilder();
            bool first = true;
            Option.FroeachDbProperties(ReadWriteFeatrue.Insert, pro =>
             {
                 if (first)
                     first = false;
                 else
                 {
                     paras.Append(',');
                     fields.Append(',');
                 }
                 fields.Append('`');
                 fields.Append(pro.ColumnName);
                 fields.Append('`');

                 paras.Append('?');
                 paras.Append(pro.PropertyName);
             });
            paras.Append(");");
            if (Option.DataSturct.IsIdentity)
            {
                paras.Append("\nSELECT @@IDENTITY;");
            }
            return $"INSERT INTO `{Option.DataSturct.WriteTableName}`({fields})\nVALUES({paras}";
        }

        /// <summary>
        /// ɾ���Ĵ���
        /// </summary>
        /// <returns></returns>
        public string BuilderDeleteSqlCode() => $"DELETE FROM `{Option.DataSturct.WriteTableName}`";

        #endregion

        #region ����

        /// <summary>
        ///     ���ɵ����ֶθ��µ�SQL
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="parameters">�����б�</param>
        /// <returns>�����ֶθ��µ�SQL</returns>
        public string FileUpdateSetCode(string field, object value, IList<DbParameter> parameters)
        {
            field = Option.FieldMap[field];
            if (value == null)
                return $"`{field}` = NULL";
            if (value is string || value is Guid || value is DateTime || value is byte[])
            {
                var name = "v_" + field;
                parameters.Add(CreateParameter(name, value, (MySqlDbType)DataOperator.GetDbType(field)));
                return $"`{field}` = ?{name}";
            }
            if (value is bool bl)
                value = bl ? 1 : 0;
            else if (value is Enum)
                value = Convert.ToInt32(value);
            return $"`{field}` = {value}";
        }

        /// <summary>
        ///     ���ɸ��µ�SQL
        /// </summary>
        /// <param name="valueExpression">���±��ʽ(SQL)</param>
        /// <param name="condition">��������</param>
        /// <returns>���µ�SQL</returns>
        public string CreateUpdateSqlCode(string valueExpression, string condition)
        {
            if (!Option.NoInjection && Provider.Injection != null)
            {
                Provider.Injection.InjectionUpdateCode(ref valueExpression, ref condition);
            }
            return $@"UPDATE `{Option.WriteTableName}` 
   SET {valueExpression} 
 WHERE {condition};";
        }

        /// <summary>
        ///     ���ɸ��µ�SQL
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="condition">��������</param>
        /// <returns>���µ�SQL</returns>
        public string CreateUpdateSqlCode(TEntity entity, string condition)
        {
            string valueExpression;
            if (!Option.UpdateByMidified)
            {
                valueExpression = Option.UpdateFields;
            }
            else
            {
                valueExpression = DataOperator.GetModifiedUpdateSql(entity);
            }
            if (valueExpression == null)
                return null;
            return CreateUpdateSqlCode(valueExpression, condition);
        }


        #endregion

        #region ����

        /// <summary>
        ///     ɾ����SQL���
        /// </summary>
        public string FullLoadSqlCode => $@"SELECT {Option.LoadFields} FROM `{Option.ReadTableName}`";


        private string _primaryKeyCondition;

        /// <summary>
        ///     ��������������SQL
        /// </summary>
        public string PrimaryKeyCondition => _primaryKeyCondition ??= FieldCondition(Option.PrimaryKey);


        string ISqlBuilder.OrderCode(bool desc, string field) => $"`{Option.FieldMap[field]}` {(desc ? "DESC" : "")}";

        /// <summary>
        ///     ��ѯ����ע��
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        string InjectionLoadCondition(string condition)
        {
            var code = new StringBuilder();
            InjectionLoadCondition(code, condition);
            return code.ToString();
        }

        /// <summary>
        ///     ��ѯ����ע��
        /// </summary>
        /// <param name="code"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        void InjectionLoadCondition(StringBuilder code, string condition)
        {
            bool isFirst = true;
            if (!string.IsNullOrEmpty(condition))
            {
                isFirst = false;
                code.Append("\nWHERE (");
                code.Append(condition);
                code.Append(')');
            }
            if (!string.IsNullOrEmpty(Option.BaseCondition))
            {
                if (isFirst)
                {
                    isFirst = false;
                    code.Append("\nWHERE (");
                }
                else
                {
                    code.Append(" AND (");
                }
                code.Append(Option.BaseCondition);
                code.Append(')');
            }

            if (Option.NoInjection || Provider.Injection == null)
            {
                return;
            }
            List<string> conditions = new List<string>();
            Provider.Injection.InjectionQueryCondition(conditions);
            foreach (var con in conditions)
            {
                if (isFirst)
                {
                    isFirst = false;
                    code.Append("\nWHERE (");
                }
                else
                {
                    code.Append(" AND (");
                }
                code.Append(con);
                code.Append(')');
            }
        }

        /// <summary>
        ///     ���ɻ��ܵ�SQL���
        /// </summary>
        /// <param name="fun">���ܺ�������</param>
        /// <param name="field">�����ֶ�</param>
        /// <param name="condition">��������</param>
        /// <returns>���ܵ�SQL���</returns>
        public string CreateCollectSql(string fun, string field, string condition)
        {
            if (field != "*")
                field = $"`{Option.FieldMap[field]}`";
            condition = InjectionLoadCondition(condition);
            return $@"SELECT {fun}({field}) FROM {Option.ReadTableName}{condition};";
        }

        /// <summary>
        ///     ���������ֶ�ֵ��SQL���
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="condition">����</param>
        /// <returns>�����ֶ�ֵ��SQL���</returns>
        public string CreateLoadValueSql(string field, string condition)
        {
            condition = InjectionLoadCondition(condition);
            return $@"SELECT `{Option.FieldMap[field]}` FROM {Option.ReadTableName}{condition};";
        }

        /// <summary>
        ///     ��������ֵ��SQL
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="convert">����</param>
        /// <returns>�����ֶ�ֵ��SQL���</returns>
        public string CreateLoadValuesSql(string field, ConditionItem convert)
        {
            var condition = InjectionLoadCondition(convert.ConditionSql);
            return $@"SELECT `{Option.FieldMap[field]}` 
FROM {Option.ReadTableName}{condition};";
        }

        /// <summary>
        ///     ���������SQL���
        /// </summary>
        /// <param name="condition">��������</param>
        /// <param name="order">�����ֶ�</param>
        /// <param name="limit"></param>
        /// <returns>�����SQL���</returns>
        public string CreateLoadSql(string condition, string order, string limit)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT {Option.LoadFields}\nFROM {Option.ReadTableName}");
            InjectionLoadCondition(sql, condition);
            if (order != null)
            {
                sql.Append($"\nORDER BY {order}");
            }
            if (limit != null)
                sql.Append($"\nLIMIT {limit}");
            sql.Append(';');
            return sql.ToString();
        }

        /// <summary>
        ///     ���ɷ�ҳ��SQL
        /// </summary>
        /// <param name="page">ҳ��</param>
        /// <param name="pageSize">ÿҳ����(ǿ�ƴ���0,С��500��)</param>
        /// <param name="order">�����ֶ�</param>
        /// <param name="desc">�Ƿ���</param>
        /// <param name="condition">��������</param>
        /// <returns></returns>
        public string CreatePageSql(int page, int pageSize, string order, bool desc, string condition)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT {Option.LoadFields}\nFROM {Option.ReadTableName}");
            InjectionLoadCondition(sql, condition);

            var orderField = string.IsNullOrWhiteSpace(order) || !Option.FieldMap.ContainsKey(order)
                ? Option.PrimaryKey
                : Option.FieldMap[order];
            sql.Append($"\nORDER BY `{orderField}` {(desc ? "DESC" : "ASC")}");

            if (pageSize >= 0)
            {
                sql.Append($"\nLIMIT {(page - 1) * pageSize},{pageSize}");
            }
            sql.Append(';');
            return sql.ToString();
        }

        #endregion

        #region ɾ��

        /// <summary>
        ///     ����ɾ����SQL���
        /// </summary>
        /// <param name="condition">ɾ������</param>
        /// <returns>ɾ����SQL���</returns>
        public string CreateDeleteSql(string condition)
        {
            if (string.IsNullOrEmpty(condition))
            {
                throw new ArgumentException("ɾ�����ݱ�����һ������");
            }
            if (!Option.NoInjection && Provider.Injection != null)
                condition = Provider.Injection.InjectionDeleteCondition(condition);
            return $"{Option.DeleteSqlCode} WHERE {condition};";
        }

        /// <summary>
        ///     ����ɾ����SQL���
        /// </summary>
        /// <param name="convert">ɾ������</param>
        /// <returns>ɾ����SQL���</returns>
        public string CreateDeleteSql(ConditionItem convert)
        {
            return CreateDeleteSql(convert.ConditionSql);
        }

        /// <summary>
        ///     ����ɾ������
        /// </summary>
        public string PhysicalDeleteSqlCode(string condition)
        {
            if (string.IsNullOrEmpty(condition))
            {
                throw new ArgumentException("ɾ�����ݱ�����һ������");
            }
            if (!Option.NoInjection && Provider.Injection != null)
                condition = Provider.Injection.InjectionDeleteCondition(condition);

            return $"DELETE FROM `{Option.WriteTableName}` WHERE {condition};";
        }
        #endregion

        #region �ֶ�����

        string ISqlBuilder.Condition(string fieldName, string paraName)
            => $"(`{Option.FieldMap[fieldName]}` = ?{paraName})";

        string ISqlBuilder.Condition(string fieldName, string paraName, string expression)
            => $"(`{Option.FieldMap[fieldName]}` {expression} ?{paraName})";

        /// <summary>
        ///     �������SQL
        /// </summary>
        /// <param name="isAnd">�Ƿ���AND���</param>
        /// <param name="conditions">����</param>
        public string ConcatCondition(bool isAnd, params string[] conditions)
        {
            if (conditions == null || conditions.Length == 0)
                throw new ArgumentException(@"û�������������", nameof(conditions));
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", conditions[0]);
            for (var idx = 1; idx < conditions.Length; idx++)
                sql.Append($@" {(isAnd ? "AND" : "OR")} ({conditions[idx]}) ");
            return sql.ToString();
        }


        /// <summary>
        ///     �����ֶ�����SQL
        /// </summary>
        /// <param name="isAnd">�Ƿ���AND���</param>
        /// <param name="fields">���ɲ������ֶ�</param>
        public string ConcatCondition(bool isAnd, params (string field, object value)[] fields)
        {

            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"û���ֶ����������������", nameof(fields));
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", FieldCondition(fields[0].field));
            var join = isAnd ? "AND" : "OR";
            for (var idx = 1; idx < fields.Length; idx++)
                sql.Append($" {join} ({FieldCondition(fields[idx].field)}) ");
            return sql.ToString();
        }

        #endregion
        #region �ֶ�����

        /// <summary>
        ///     ���������е��ֶ�����
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="expression">�������ʽ</param>
        /// <returns>�ֶ�����</returns>
        public string FieldCondition(string field, string expression = "=")
            => $@"`{Option.FieldMap[field]}` {expression} ?{field}";

        /// <summary>
        ///     �����ֶ�����SQL
        /// </summary>
        /// <param name="isAnd">�Ƿ���AND���</param>
        /// <param name="fields">���ɲ������ֶ�</param>
        public string ConcatFieldCondition(bool isAnd, params string[] fields)
        {
            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"û���ֶ����������������", nameof(fields));
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", FieldCondition(fields[0]));
            for (var idx = 1; idx < fields.Length; idx++)
                sql.AppendFormat(@" {0} ({1}) ", isAnd ? "AND" : "OR", FieldCondition(fields[idx]));
            return sql.ToString();
        }

        #endregion
        #region ����

        /// <summary>
        ///     �õ��ֶε�MySqlDbType����
        /// </summary>
        /// <param name="field">�ֶ�����</param>
        /// <returns>����</returns>
        int ISqlBuilder.GetDbType(string field)
        {
            return DataOperator.GetDbType(field);
        }


        /// <summary>
        ///     �����ֶεĲ���
        /// </summary>
        /// <param name="field">���ɲ������ֶ�</param>
        public MySqlParameter CreateFieldParameter(string field)
        {
            return new MySqlParameter(field, DataOperator.GetDbType(field));
        }

        /// <summary>
        ///     �����ֶεĲ���
        /// </summary>
        /// <param name="field">���ɲ������ֶ�</param>
        /// <param name="entity">ȡֵ��ʵ��</param>
        public DbParameter CreateFieldParameter(string field, TEntity entity)
        {
            return CreateParameter(field,
                DataOperator.GetValue(entity, field),
                (MySqlDbType)DataOperator.GetDbType(field));
        }

        /// <summary>
        ///     ���������ֶεĲ���
        /// </summary>
        public DbParameter CreatePimaryKeyParameter()
        {
            return new MySqlParameter(Option.PrimaryKey, DataOperator.GetDbType(Option.PrimaryKey));
        }

        /// <summary>
        ///     ���������ֶεĲ���
        /// </summary>
        /// <param name="value">����ֵ</param>
        public DbParameter CreatePimaryKeyParameter(object value)
        {
            return CreateParameter(Option.PrimaryKey, value, (MySqlDbType)DataOperator.GetDbType(Option.PrimaryKey));
        }

        /// <summary>
        ///     ���������ֶεĲ���
        /// </summary>
        /// <param name="entity">ȡֵ��ʵ��</param>
        public DbParameter CreatePimaryKeyParameter(TEntity entity)
        {
            return CreateParameter(Option.PrimaryKey,
                DataOperator.GetValue(entity, Option.PrimaryKey),
                (MySqlDbType)DataOperator.GetDbType(Option.PrimaryKey));
        }
        #endregion

        #region Expression

        ConditionItem ISqlBuilder<TEntity>.Compile(Expression<Func<TEntity, bool>> lambda)
            => PredicateConvert.Convert(this, Option.FieldMap, lambda);

        ConditionItem ISqlBuilder<TEntity>.Compile(LambdaItem<TEntity> lambda)
            => PredicateConvert.Convert(this, Option.FieldMap, lambda);

        #endregion
    }
}