// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using Agebull.Common.Logging;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.Events;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
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

        /// <summary>
        /// Sql��Ӧ��������Ϣ
        /// </summary>
        DataAccessOption<TEntity> ISqlBuilder<TEntity>.Option
        {
            get => Provider.Option;
            set => Provider.Option = value;
        }


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
        public string FileUpdateSql(string field, object value, IList<DbParameter> parameters)
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
        public string CreateUpdateSql(string valueExpression, string condition)
        {
            if (Option.NoInjection)
            {
                return $@"UPDATE `{Option.WriteTableName}` 
   SET {valueExpression} 
 WHERE {condition};";
            }
            Provider.Injection?.CheckUpdateContition(ref condition);
            return $@"{Provider.Injection?.BeforeUpdateSql(condition)}
UPDATE `{Option.WriteTableName}` 
   SET {valueExpression} 
 WHERE {condition};
{Provider.Injection?.AfterUpdateSql(condition)}";
        }

        /// <summary>
        ///     ���ɸ��µ�SQL
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="condition">��������</param>
        /// <returns>���µ�SQL</returns>
        public string GetUpdateSql(TEntity entity, string condition)
        {
            if (!Option.UpdateByMidified)
            {
                return $@"UPDATE `{Option.WriteTableName}` 
   SET {Option.UpdateFields} 
 WHERE {condition};";
            }
            var sql = DataOperator.GetModifiedUpdateSql(entity);
            if (sql == null)
                return null;
            if (!Option.NoInjection)
            {
                Provider.Injection?.CheckUpdateContition(ref condition);
                return $@"{Provider.Injection?.BeforeUpdateSql(condition)}
UPDATE `{Option.WriteTableName}` 
   SET {sql} 
 WHERE {condition};
{Provider.Injection?.AfterUpdateSql(condition)}";
            }
            else
            {
                return $@"UPDATE `{Option.WriteTableName}` 
   SET {sql} 
 WHERE {condition};";
            }
        }


        #endregion

        #region ����

        /// <summary>
        ///     ɾ����SQL���
        /// </summary>
        public string FullLoadSqlCode => $@"SELECT {Option.LoadFields} FROM `{Option.ReadTableName}`";


        private string _primaryConditionSQL;

        /// <summary>
        ///     ��������������SQL
        /// </summary>
        public string PrimaryKeyConditionSQL => _primaryConditionSQL ??= FieldConditionSQL(Option.PrimaryKey);


        string ISqlBuilder.OrderSql(bool desc, string field) => $"`{Option.FieldMap[field]}` {(desc ? "DESC" : "")}";

        /// <summary>
        ///     �õ�����ȷƴ�ӵ�SQL������䣨������û�У�
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public string InjectionCondition(string condition)
        {
            if (Option.NoInjection)
            {
                return string.IsNullOrEmpty(condition) ? null : $"\nWHERE {condition}";
            }

            List<string> conditions = new List<string>();

            if (!string.IsNullOrEmpty(condition))
                conditions.Add(condition);
            Provider.Injection?.InjectionCondition(conditions);
            if (conditions.Count == 0)
                return null;
            var code = new StringBuilder();

            bool isFirst = true;
            foreach (var con in conditions)
            {
                if (isFirst)
                {
                    isFirst = false;
                    code.Append("\nWHERE ");
                }
                else
                {
                    code.Append(" AND ");
                }
                code.Append($"({con})");
            }
            return code.ToString();
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
            var sql = $@"SELECT {fun}({field}) FROM {Option.ReadTableName}{InjectionCondition(condition)};";
            return sql;
        }

        /// <summary>
        ///     ���������ֶ�ֵ��SQL���
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="condition">����</param>
        /// <returns>�����ֶ�ֵ��SQL���</returns>
        public string CreateLoadValueSql(string field, string condition)
        {
            return $@"SELECT `{Option.FieldMap[field]}` FROM {Option.ReadTableName}{InjectionCondition(condition)};";
        }

        /// <summary>
        ///     ��������ֵ��SQL
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="convert">����</param>
        /// <returns>�����ֶ�ֵ��SQL���</returns>
        public string CreateLoadValuesSql(string field, ConditionItem convert)
        {
            return $@"SELECT `{Option.FieldMap[field]}` 
FROM {Option.ReadTableName}{InjectionCondition(convert.ConditionSql)};";
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
            sql.Append($"SELECT {Option.LoadFields} FROM {Option.ReadTableName}");
            sql.Append(InjectionCondition(condition));
            if (order != null)
            {
                sql.Append($" ORDER BY {order}");
            }
            if (limit != null)
                sql.Append($" LIMIT {limit}");
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
            var orderField = string.IsNullOrWhiteSpace(order) || !Option.FieldMap.ContainsKey(order)
                ? Option.PrimaryKey
                : Option.FieldMap[order];

            var sql = new StringBuilder();
            sql.Append($@"SELECT {Option.LoadFields}
FROM {Option.ReadTableName}{InjectionCondition(condition)}
ORDER BY `{orderField}` {(desc ? "DESC" : "ASC")}");

            if (pageSize >= 0)
            {
                if (page <= 0)
                    page = 1;
                if (pageSize == 0)
                    pageSize = 20;
                else if (pageSize > 500)
                    pageSize = 500;
                sql.Append($" LIMIT {(page - 1) * pageSize},{pageSize}");
            }
            sql.Append(";");
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
            if (!Option.NoInjection)
            {
                Provider.Injection?.CheckUpdateContition(ref condition);
                return $@"{Provider.Injection?.BeforeUpdateSql(condition)}
{Option.DeleteSqlCode} WHERE {condition};
{Provider.Injection?.AfterUpdateSql(condition)}";

            }
            else if (string.IsNullOrEmpty(condition))
            {
                throw new ArgumentException("ɾ�����ݱ�����һ������");
            }

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
        public string PhysicalDeleteSql(string condition)
        {
            return string.IsNullOrEmpty(condition)
                ? $"DELETE FROM `{Option.WriteTableName}"
                : $"DELETE FROM `{Option.WriteTableName}` WHERE {condition};";
        }
        #endregion

        #region �ֶ�����

        string ISqlBuilder.Condition(string fieldName, string paraName)
            => $"(`{Option.FieldMap[fieldName]}` = ?{paraName})";

        string ISqlBuilder.Condition(string fieldName, string paraName, string condition)
            => $"(`{Option.FieldMap[fieldName]}` {condition} ?{paraName})";

        /// <summary>
        ///     ���������е��ֶ�����
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="expression">�������ʽ</param>
        /// <returns>�ֶ�����</returns>
        public string FieldConditionSQL(string field, string expression = "=")
            => $@"`{Option.FieldMap[field]}` {expression} ?{field}";

        /// <summary>
        ///     �������SQL
        /// </summary>
        /// <param name="isAnd">�Ƿ���AND���</param>
        /// <param name="conditions">����</param>
        public string JoinConditionSQL(bool isAnd, params string[] conditions)
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
        public string FieldConditionSQL(bool isAnd, params string[] fields)
        {
            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"û���ֶ����������������", nameof(fields));
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", FieldConditionSQL(fields[0]));
            for (var idx = 1; idx < fields.Length; idx++)
                sql.AppendFormat(@" {0} ({1}) ", isAnd ? "AND" : "OR", FieldConditionSQL(fields[idx]));
            return sql.ToString();
        }


        /// <summary>
        ///     �����ֶ�����SQL
        /// </summary>
        /// <param name="isAnd">�Ƿ���AND���</param>
        /// <param name="fields">���ɲ������ֶ�</param>
        public string FieldConditionSQL(bool isAnd, params (string field, object value)[] fields)
        {
            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"û���ֶ����������������", nameof(fields));
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", FieldConditionSQL(fields[0].field));
            var join = isAnd ? "AND" : "OR";
            for (var idx = 1; idx < fields.Length; idx++)
                sql.Append($" {join} ({FieldConditionSQL(fields[idx].field)}) ");
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
                Option.DataOperator.GetValue(entity, field),
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
                Option.DataOperator.GetValue(entity, Option.PrimaryKey),
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