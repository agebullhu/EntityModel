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
    ///     MySql��SQL�������
    /// </summary>
    public sealed class MySqlSqlBuilder<TEntity> : ParameterCreater, ISqlBuilder<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// ���ݿ�����
        /// </summary>
        DataBaseType ISqlBuilder.DataBaseType => DataBaseType.MySql;

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
        public DataAccessOption Option => Provider.Option;

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
        ///     �õ��ֶε�MySqlDbType����
        /// </summary>
        /// <param name="field">�ֶ�����</param>
        /// <returns>����</returns>
        int ISqlBuilder.GetDbType(string field)
        {
            return DataOperator.GetDbType(field);
        }

        /// <summary>
        /// ��ȡ���ֶ�
        /// </summary>
        /// <returns></returns>
        string ISqlBuilder.BuilderLoadFields()
        {
            var code = new StringBuilder();
            bool first = true;
            foreach (var pro in Option.ReadProperties)
            {
                if (first)
                    first = false;
                else
                    code.Append(',');
                code.Append('`');
                code.Append(pro.FieldName);
                code.Append('`');
            };
            return code.ToString();
        }

        /// <summary>
        /// ȫ�����µ��ֶ�
        /// </summary>
        /// <returns></returns>
        string ISqlBuilder.BuilderUpdateFields()
        {
            var fields = new StringBuilder();
            bool first = true;
            Option.FroeachDbProperties(ReadWriteFeatrue.Update, pro =>
           {
               if (pro.Entity != Option.DataStruct.Name)
                   return;
               if (first)
                   first = false;
               else
                   fields.Append(',');

               fields.Append('`');
               fields.Append(pro.FieldName);
               fields.Append("`=?");
               fields.Append(pro.PropertyName);
           });
            return fields.ToString();
        }

        /// <summary>
        /// ����Ĵ���
        /// </summary>
        /// <returns></returns>
        string ISqlBuilder.BuilderInsertSqlCode()
        {
            var fields = new StringBuilder();
            var paras = new StringBuilder();
            bool first = true;
            Option.FroeachDbProperties(ReadWriteFeatrue.Insert, pro =>
            {
                if (pro.Entity != Option.DataStruct.Name)
                    return;
                if (first)
                    first = false;
                else
                {
                    paras.Append(',');
                    fields.Append(',');
                }
                fields.Append('`');
                fields.Append(pro.FieldName);
                fields.Append('`');

                paras.Append('?');
                paras.Append(pro.PropertyName);
            });
            paras.Append(");");
            if (Option.DataStruct.IsIdentity)
            {
                paras.Append("\nSELECT @@IDENTITY;");
            }
            return $"INSERT INTO `{Option.DataStruct.WriteTableName}`({fields})\nVALUES({paras}";
        }

        /// <summary>
        /// ɾ���Ĵ���
        /// </summary>
        /// <returns></returns>
        string ISqlBuilder.BuilderDeleteSqlCode() => $"DELETE FROM `{Option.DataStruct.WriteTableName}`";

        #endregion

        #region ����

        /// <summary>
        ///     ���ɵ����ֶθ��µ�SQL
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="parameters">�����б�</param>
        /// <returns>�����ֶθ��µ�SQL</returns>
        string ISqlBuilder.FileUpdateSetCode(string field, object value, IList<DbParameter> parameters)
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
        string ISqlBuilder.CreateUpdateSqlCode(string valueExpression, string condition)
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
        /// <returns>���µ�SQL</returns>
        string ISqlBuilder<TEntity>.CreateInsertSqlCode(TEntity entity)
        {
            if (Option.NoInjection || Provider.Injection == null)
            {
                return Option.InsertSqlCode;
            }
            var fields = new StringBuilder();
            var paras = new StringBuilder('(');
            bool first = true;
            Option.FroeachDbProperties(ReadWriteFeatrue.Insert, pro =>
            {
                if (pro.Entity != Option.DataStruct.Name)
                    return;
                if (first)
                    first = false;
                else
                {
                    paras.Append(',');
                    fields.Append(',');
                }
                fields.Append('`');
                fields.Append(pro.FieldName);
                fields.Append('`');

                paras.Append('?');
                paras.Append(pro.PropertyName);
            });

            Provider.Injection.InjectionInsertCode(fields, paras);
            paras.Append(");");
            if (Option.DataStruct.IsIdentity)
            {
                paras.Append("\nSELECT @@IDENTITY;");
            }
            return $"INSERT INTO `{Option.WriteTableName}`({fields})\nVALUES{paras}";
        }

        /// <summary>
        ///     ���ɸ��µ�SQL
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="condition">��������</param>
        /// <returns>���µ�SQL</returns>
        string ISqlBuilder<TEntity>.CreateUpdateSqlCode(TEntity entity, string condition)
        {
            string valueExpression;
            if (entity is IEditStatus status && status.EditStatusRedorder != null && !status.EditStatusRedorder.IsSetFullModify)
            {
                var code = new List<string>();
                var properties = Option.Properties;
                Option.FroeachDbProperties(ReadWriteFeatrue.Update, pro =>
                {
                    if (pro.Entity == Option.DataStruct.Name && status.EditStatusRedorder.IsChanged(pro.PropertyName))
                        code.Add($"`{pro.FieldName}` = ?{pro.PropertyName}");
                });
                if (code.Count == 0)
                    return null;
                valueExpression = string.Join(",\n", code);
            }
            else
            {
                valueExpression = Option.UpdateFields;
            }
            return ((ISqlBuilder)this).CreateUpdateSqlCode(valueExpression, condition);
        }


        #endregion

        #region ����

        private string _primaryKeyCondition;

        /// <summary>
        ///     ��������������SQL
        /// </summary>
        string ISqlBuilder.PrimaryKeyCondition => _primaryKeyCondition ??= ((ISqlBuilder)this).Condition(Option.PrimaryProperty);

        /// <summary>
        /// ��������SQLƬ��
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public string OrderCode(bool desc, string field) => $"`{Option.FieldMap[field]}` {(desc ? "DESC" : "")}";

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
        string ISqlBuilder.CreateCollectSql(string fun, string field, string condition)
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
        string ISqlBuilder.CreateLoadValueSql(string field, string condition)
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
        string ISqlBuilder.CreateLoadValuesSql(string field, ConditionItem convert)
        {
            var condition = InjectionLoadCondition(convert.ConditionSql);
            return $@"SELECT `{Option.FieldMap[field]}` FROM {Option.ReadTableName}{condition};";
        }

        /// <summary>
        ///     ���������SQL���
        /// </summary>
        /// <param name="condition">��������</param>
        /// <param name="orderSql">����Ƭ��</param>
        /// <param name="limit"></param>
        /// <returns>�����SQL���</returns>
        string ISqlBuilder.CreateLoadSql(string condition, string orderSql, string limit)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT {Option.LoadFields}\nFROM {Option.ReadTableName}");
            sql.Append(Option.Having);
            InjectionLoadCondition(sql, condition);
            if (orderSql != null)
            {
                sql.Append($"\nORDER BY {orderSql}");
            }
            sql.Append(Option.GroupFields);
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
        /// <param name="orderField">�����ֶ�</param>
        /// <param name="desc">�Ƿ���</param>
        /// <param name="condition">��������</param>
        /// <returns></returns>
        string ISqlBuilder.CreatePageSql(int page, int pageSize, string orderField, bool desc, string condition)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT {Option.LoadFields}\nFROM {Option.ReadTableName}");
            InjectionLoadCondition(sql, condition);
            if (!string.IsNullOrWhiteSpace(orderField))
                sql.Append($"\nORDER BY {OrderCode(desc, orderField)}");

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
        string ISqlBuilder.CreateDeleteSql(string condition)
        {
            if (!Option.NoInjection && Provider.Injection != null)
                condition = Provider.Injection.InjectionDeleteCondition(condition);
            return $"{Option.DeleteSqlCode} WHERE {condition};";
        }

        /// <summary>
        ///     ����ɾ������
        /// </summary>
        string ISqlBuilder.PhysicalDeleteSqlCode(string condition)
        {
            if (!Option.NoInjection && Provider.Injection != null)
                condition = Provider.Injection.InjectionDeleteCondition(condition);

            return $"DELETE FROM `{Option.WriteTableName}` WHERE {condition};";
        }
        #endregion

        #region �ֶ�����

        string ISqlBuilder.Condition(string fieldName, string expression, string paraName) => $"(`{Option.FieldMap[fieldName]}` {expression} ?{paraName ?? fieldName})";

        #endregion

        #region Expression

        ConditionItem ISqlBuilder<TEntity>.Compile(Expression<Func<TEntity, bool>> lambda) => PredicateConvert.Convert(this, Option, lambda);

        ConditionItem ISqlBuilder<TEntity>.Compile(LambdaItem<TEntity> lambda) => PredicateConvert.Convert(this, Option, lambda);

        #endregion
    }
}