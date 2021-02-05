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

        DataAccessOption _option;
        /// <summary>
        /// Sql��乹����
        /// </summary>
        public DataAccessOption Option
        {
            get => _option ?? Provider.Option;
            set => _option = value;
        }

        /// <summary>
        /// Sql��乹����
        /// </summary>
        public ISqlBuilder<TEntity> SqlBuilder => Provider.SqlBuilder;

        /// <summary>
        /// �����ṩ����Ϣ
        /// </summary>
        public IDataAccessProvider<TEntity> Provider { get; set; }

        #region ���ݽṹ֧��

        /// <summary>
        /// ����ѭ������
        /// </summary>
        void FroeachDbProperties(ReadWriteFeatrue readWrite, Action<EntityProperty> action)
        {
            var properties = Option.Properties;
            foreach (var pro in properties)
            {
                if (pro.TableName == Option.WriteTableName &&
                    pro.DbReadWrite.HasFlag(readWrite) &&
                    pro.PropertyFeatrue.HasFlag(PropertyFeatrue.Property | PropertyFeatrue.Field))
                    action(pro);
            }
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
                code.Append($"`{pro.TableName}`.`{pro.FieldName}`");
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
            FroeachDbProperties(ReadWriteFeatrue.Update, pro =>
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
        ///     ���ɸ��µ�SQL
        /// </summary>
        /// <param name="valueExpression">���±��ʽ(SQL)</param>
        /// <param name="condition">��������</param>
        /// <returns>���µ�SQL</returns>
        string ISqlBuilder.BuilderUpdateCode(string valueExpression, string condition)
        {
            return $@"UPDATE `{Option.WriteTableName}` 
   SET {valueExpression} 
 WHERE {condition};";
        }

        /// <summary>
        /// ����Ĵ���(BUG)
        /// </summary>
        /// <returns></returns>
        (string fielsd, string values) ISqlBuilder.BuilderInsertSqlCode()
        {
            var fields = new StringBuilder();
            var values = new StringBuilder();
            bool first = true;
            FroeachDbProperties(ReadWriteFeatrue.Insert, pro =>
            {
                if (pro.Entity != Option.DataStruct.Name)
                    return;
                if (first)
                    first = false;
                else
                {
                    values.Append(',');
                    fields.Append(',');
                }
                fields.Append('`');
                fields.Append(pro.FieldName);
                fields.Append('`');

                values.Append('?');
                values.Append(pro.PropertyName);
            });
            values.Append(");");
            if (Option.DataStruct.IsIdentity)
            {
                values.Append("\nSELECT @@IDENTITY;");
            }
            return (fields.ToString(), values.ToString());
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
        string ISqlBuilder.FieldUpdateSetCode(string field, object value, IList<DbParameter> parameters)
        {
            field = Option.PropertyMap[field].FieldName;
            if (value == null)
                return $"`{field}` = NULL";
            if (value is string || value is Guid || value is DateTime || value is byte[])
            {
                var name = "v_" + field;
                parameters.Add(new MySqlParameter(name, value));
                return $"`{field}` = ?{name}";
            }
            if (value is bool bl)
                value = bl ? 1 : 0;
            else if (value is Enum)
                value = Convert.ToInt32(value);
            return $"`{field}` = {value}";
        }

        /// <summary>
        /// �ֶ��ۼӸ���
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string ISqlBuilder.FieldAddCode(string field, object value, IList<DbParameter> parameters)
        {
            var pro = Option.PropertyMap[field];
            return $"`{pro.FieldName}` = `{pro.FieldName}` + {value}";
        }

        /// <summary>
        ///     ���ɸ��µ�SQL
        /// </summary>
        /// <param name="valueExpression">���±��ʽ(SQL)</param>
        /// <param name="condition">��������</param>
        /// <returns>���µ�SQL</returns>
        string ISqlBuilder.CreateUpdateSqlCode(string valueExpression, string condition)
        {
            if ((Provider.Option.InjectionLevel.HasFlag(InjectionLevel.UpdateField) || Provider.Option.InjectionLevel.HasFlag(InjectionLevel.UpdateCondition)) &&
                Provider.Injection != null)
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
        /// <returns>���µ�SQL</returns>
        string ISqlBuilder.CreateInsertSqlCode()
        {
            var fields = new StringBuilder();
            fields.Append($"INSERT INTO `{Option.WriteTableName}`(");
            var values = new StringBuilder();
            values.Append("VALUES(");
            if (string.IsNullOrWhiteSpace(Provider.Option.InsertFieldCode))
            {
                fields.Append(Provider.Option.InsertFieldCode);
                values.Append(Provider.Option.InsertValueCode);
            }
            else
            {
                bool first = true;
                FroeachDbProperties(ReadWriteFeatrue.Insert, pro =>
                {
                    if (first)
                        first = false;
                    else
                    {
                        values.Append(',');
                        fields.Append(',');
                    }
                    fields.Append('`');
                    fields.Append(pro.FieldName);
                    fields.Append('`');

                    values.Append('?');
                    values.Append(pro.PropertyName);
                });
            }

            if (Provider.Injection != null && Provider.Option.InjectionLevel.HasFlag(InjectionLevel.InsertField))
            {
                Provider.Injection.InjectionInsertCode(fields, values);
            }
            values.Append(");");
            if (Option.DataStruct.IsIdentity)
            {
                values.Append("\nSELECT @@IDENTITY;");
            }

            fields.AppendLine(")");
            fields.Append(values);
            return fields.ToString();
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
            if (entity is IEditStatus status && status.EditStatusRecorder != null && !status.EditStatusRecorder.IsSetFullModify)
            {
                var code = new List<string>();
                var properties = Option.Properties;
                FroeachDbProperties(ReadWriteFeatrue.Update, pro =>
                {
                    if (pro.Entity == Option.DataStruct.Name && status.EditStatusRecorder.IsChanged(pro.PropertyName))
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
            {
                var pro = Option.PropertyMap[field];
                field = $"`{pro.TableName}`.`{pro.FieldName}`";
            }
            condition = InjectionLoadCondition(condition);
            return $@"SELECT {fun}({field}) FROM {Option.ReadTableName}{condition};";
        }

        /// <summary>
        ///     ���������ֶ�ֵ��SQL���
        /// </summary>
        /// <param name="fields">�ֶ�</param>
        /// <param name="condition">����</param>
        /// <param name="orderSql">����Ƭ��</param>
        /// <param name="limit"></param>
        /// <returns>�����ֶ�ֵ��SQL���</returns>
        string CreateLoadSql(string fields, string condition, string orderSql, string limit)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT {fields}\nFROM {Option.ReadTableName} {Option.Having}");

            InjectionLoadCondition(sql, condition);
            orderSql ??= Option.OrderbyFields;
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
        ///     ���������ֶ�ֵ��SQL���
        /// </summary>
        /// <param name="fields">�ֶ�</param>
        /// <param name="condition">����</param>
        /// <param name="orderSql">����Ƭ��</param>
        /// <param name="limit"></param>
        /// <returns>�����ֶ�ֵ��SQL���</returns>
        string ISqlBuilder.CreateFullLoadSql(string fields, string condition, string orderSql, string limit)
        {
            return CreateLoadSql(fields, condition, orderSql, limit);
        }

        /// <summary>
        ///     ���������ֶ�ֵ��SQL���
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="condition">����</param>
        /// <param name="orderSql">����Ƭ��</param>
        /// <param name="limit"></param>
        /// <returns>�����ֶ�ֵ��SQL���</returns>
        string ISqlBuilder.CreateSingleLoadSql(string field, string condition, string orderSql, string limit)
        {
            var pro = Option.PropertyMap[field];
            return CreateLoadSql($"`{pro.TableName}`.`{pro.FieldName}`", condition, orderSql, limit);
        }

        #endregion

        #region ע��

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

            if (Provider.Injection == null || !Provider.Option.InjectionLevel.HasFlag(InjectionLevel.QueryCondition))
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

        #endregion

        #region ɾ��

        /// <summary>
        ///     ����ɾ����SQL���
        /// </summary>
        /// <param name="condition">ɾ������</param>
        /// <returns>ɾ����SQL���</returns>
        string ISqlBuilder.CreateDeleteSql(string condition)
        {
            if (Provider.Injection != null && Provider.Option.InjectionLevel.HasFlag(InjectionLevel.DeleteCondition))
                condition = Provider.Injection.InjectionDeleteCondition(condition);
            return $"{Option.DeleteSqlCode} WHERE {condition};";
        }

        /// <summary>
        ///     ����ɾ������
        /// </summary>
        string ISqlBuilder.PhysicalDeleteSqlCode(string condition)
        {
            if (Provider.Injection != null && Provider.Option.InjectionLevel.HasFlag(InjectionLevel.DeleteCondition))
                condition = Provider.Injection.InjectionDeleteCondition(condition);

            return $"DELETE FROM `{Option.WriteTableName}` WHERE {condition};";
        }
        #endregion

        #region ��չ

        string ISqlBuilder.Condition(string field, string expression, string paraName)
        {
            var pro = Option.PropertyMap[field];
            return $"(`{pro.TableName}`.`{pro.FieldName}` {expression} ?{paraName ?? pro.PropertyName})";
        }

        private string _primaryKeyCondition;

        /// <summary>
        ///     ��������������SQL
        /// </summary>
        string ISqlBuilder.PrimaryKeyCondition => _primaryKeyCondition ??= ((ISqlBuilder)this).Condition(Option.PrimaryProperty);

        /// <summary>
        /// ��������SQLƬ��
        /// </summary>
        /// <param name="field"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public string OrderCode(string field, bool asc)
        {
            if (string.IsNullOrEmpty(field))
                return null;
            var pro = Option.PropertyMap[field];
            return $"`{pro.TableName}`.`{pro.FieldName}` {(asc ? "ASC" : "DESC")}";
        }

        /// <summary>
        /// ��������SQLƬ��
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public string OrderCode(string[] fields)
        {
            if (fields == null || fields.Length == 0)
                return null;
            var code = new StringBuilder();
            foreach (var field in fields)
            {
                var pro = Option.PropertyMap[field];
                code.Append($"`{pro.TableName}`.`{pro.FieldName}`");
            }
            return code.ToString();
        }

        ///<inheritdoc/>
        public ConditionItem Compile(Expression<Func<TEntity, bool>> lambda) => PredicateConvert.Convert(this, Option, lambda);

        ///<inheritdoc/>
        public ConditionItem Compile(LambdaItem<TEntity> lambda) => PredicateConvert.Convert(this, Option, lambda);

        #endregion
    }
}