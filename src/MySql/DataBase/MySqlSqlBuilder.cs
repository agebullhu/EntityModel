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
using MySql.Data.MySqlClient;
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
    public class MySqlSqlBuilder<TEntity> : SimpleConfig, ISqlBuilder<TEntity>, IParameterCreater
        where TEntity : EditDataObject, new()
    {
        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public DataBaseType DataBaseType => DataBaseType.MySql;

        string ISqlBuilder<TEntity>.Condition(string fieldName, string paraName) => $"(`{SqlOption.FieldMap[fieldName]}` = ?{paraName})";

        string ISqlBuilder<TEntity>.Condition(string fieldName, string paraName, string condition) => $"(`{SqlOption.FieldMap[fieldName]}` {condition} ?{paraName})";

        /// <summary>
        /// Sql��Ӧ��������Ϣ
        /// </summary>
        public DataAccessSqlOption<TEntity> SqlOption { get; set; }

        /// <summary>
        /// ��������ע��
        /// </summary>
        public bool NoInjection { get; set; }


        #region ����

        /// <summary>
        /// ȡ�ý����µ�SQL���
        /// </summary>
        public string GetModifiedUpdateSql(EditDataObject data)
        {
            if (data.__status.IsReadOnly)
            {
                return SqlOption.UpdateSqlCode;
            }
            if (!data.__status.IsModified)
                return null;
            StringBuilder sql = new StringBuilder();
            bool first = true;
            foreach (var pro in data.__Struct.Properties.Where(p => p.Value.Featrue.HasFlag(PropertyFeatrue.Property)))
            {
                if (data.__status.Status.ModifiedProperties[pro.Key] <= 0 || !SqlOption.FieldMap.ContainsKey(pro.Value.Name))
                    continue;
                if (first)
                    first = false;
                else
                    sql.Append(',');
                sql.AppendLine($"       `{pro.Value.ColumnName}` = ?{pro.Value.Name}");
            }
            return first ? null : sql.ToString();
        }

        /// <summary>
        /// ȡ�ý����µ�SQL���
        /// </summary>
        public string GetFullUpdateSql(EditDataObject data)
        {
            StringBuilder sql = new StringBuilder();
            bool first = true;
            foreach (var pro in data.__Struct.Properties.Where(p => p.Value.Featrue.HasFlag(PropertyFeatrue.Property)))
            {
                if (!SqlOption.FieldMap.ContainsKey(pro.Value.Name))
                    continue;
                if (first)
                    first = false;
                else
                    sql.Append(',');
                sql.AppendLine($"       `{pro.Value.ColumnName}` = ?{pro.Value.Name}");
            }
            return first ? null : sql.ToString();
        }
        /// <summary>
        ///     ���ɵ����ֶθ��µ�SQL
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="parameters">�����б�</param>
        /// <returns>�����ֶθ��µ�SQL</returns>
        public string FileUpdateSql(string field, object value, IList<DbParameter> parameters)
        {
            field = SqlOption.FieldMap[field];
            if (value == null)
                return $"`{field}` = NULL";
            if (value is string || value is Guid || value is DateTime || value is byte[])
            {
                var name = "v_" + field;
                parameters.Add(CreateFieldParameter(name, GetDbType(field), value));
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
            if (!NoInjection)
                CheckUpdateContition(ref condition);
            return $@"{BeforeUpdateSql(condition)}
UPDATE `{SqlOption.WriteTableName}` 
   SET {valueExpression} 
 WHERE {condition};
{AfterUpdateSql(condition)}";
        }

        /// <summary>
        ///     �õ�����ȷ���µ�����
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected virtual void CheckUpdateContition(ref string condition)
        {
        }

        #endregion

        #region ����

        /// <summary>
        ///     ɾ����SQL���
        /// </summary>
        public virtual string FullLoadSqlCode => $@"SELECT {SqlOption.LoadFields} FROM `{SqlOption.ReadTableName}`";



        private string _primaryConditionSQL;

        /// <summary>
        ///     ��������������SQL
        /// </summary>
        public string PrimaryKeyConditionSQL => _primaryConditionSQL ??= FieldConditionSQL(SqlOption.KeyField);


        string ISqlBuilder<TEntity>.OrderSql(bool desc, string field) => $"`{SqlOption.FieldMap[field]}` {(desc ? "DESC" : "")}";

        /// <summary>
        /// ����������ʼ����ɵı�ʶ
        /// </summary>
        private bool _baseConditionInited;

        /// <summary>
        ///  ��ʼ����������
        /// </summary>
        /// <returns></returns>
        protected virtual void InitBaseCondition()
        {
        }

        /// <summary>
        ///     �õ�����ȷƴ�ӵ�SQL������䣨������û�У�
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public string InjectionCondition(string condition)
        {
            if (NoInjection)
            {
                return string.IsNullOrEmpty(condition) ? null : $"WHERE {condition}";
            }

            List<string> conditions = new List<string>();
            if (!_baseConditionInited)
            {
                InitBaseCondition();
                _baseConditionInited = true;
            }
            if (!string.IsNullOrEmpty(SqlOption.BaseCondition))
                conditions.Add(SqlOption.BaseCondition);
            if (!string.IsNullOrEmpty(condition))
                conditions.Add(condition);
            if (!NoInjection)
            {
                ConditionSqlCode(conditions);
                DataUpdateHandler.ConditionSqlCode(this, conditions);
            }
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
        ///     �õ�����ȷƴ�ӵ�SQL������䣨������û�У�
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        protected virtual void ConditionSqlCode(List<string> conditions)
        {
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
                field = $"`{SqlOption.FieldMap[field]}`";
            var sql = $@"SELECT {fun}({field}) FROM {SqlOption.ReadTableName}{InjectionCondition(condition)};";
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
            Debug.Assert(SqlOption.FieldMap.ContainsKey(field));
            return $@"SELECT `{SqlOption.FieldMap[field]}` FROM {SqlOption.ReadTableName}{InjectionCondition(condition)};";
        }

        /// <summary>
        ///     ��������ֵ��SQL
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="convert">����</param>
        /// <returns>�����ֶ�ֵ��SQL���</returns>
        public string CreateLoadValuesSql(string field, ConditionItem convert)
        {
            return $@"SELECT `{SqlOption.FieldMap[field]}` 
FROM {SqlOption.ReadTableName}{InjectionCondition(convert.ConditionSql)};";
        }

        /// <summary>
        ///     ���������SQL���
        /// </summary>
        /// <param name="condition">��������</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>�����SQL���</returns>
        public StringBuilder CreateLoadSql(string condition, string order)
        {
            var sql = new StringBuilder();
            sql.AppendLine(@"SELECT");
            sql.AppendLine(SqlOption.LoadFields);
            sql.AppendFormat(@"FROM {0}", SqlOption.ReadTableName);
            sql.AppendLine(InjectionCondition(condition));
            if (!string.IsNullOrWhiteSpace(order))
            {
                sql.AppendLine();
                sql.Append($"ORDER BY {order}");
            }
            sql.Append(";");
            return sql;
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
            var orderField = string.IsNullOrWhiteSpace(order) || !SqlOption.FieldMap.ContainsKey(order)
                ? SqlOption.KeyField
                : SqlOption.FieldMap[order];

            var sql = new StringBuilder();
            sql.Append($@"SELECT {SqlOption.LoadFields}
FROM {SqlOption.ReadTableName}{InjectionCondition(condition)}
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
            return $@"{BeforeUpdateSql(condition)}
{DeleteSqlCode} WHERE {condition};
{AfterUpdateSql(condition)}";
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
                ? $"DELETE FROM `{SqlOption.WriteTableName}"
                : $"DELETE FROM `{SqlOption.WriteTableName}` WHERE {condition};";
        }
        /// <summary>
        ///     ɾ����SQL���
        /// </summary>
        public virtual string DeleteSqlCode => $@"DELETE FROM `{SqlOption.ReadTableName}`";

        #endregion

        #region �ֶ�����

        /// <summary>
        ///     ���������е��ֶ�����
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="expression">�������ʽ</param>
        /// <returns>�ֶ�����</returns>
        public string FieldConditionSQL(string field, string expression = "=")
        {
            Debug.Assert(SqlOption.FieldMap.ContainsKey(field));
            return $@"`{SqlOption.FieldMap[field]}` {expression} ?{field}";
        }

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

        #region Sqlע��

        /// <summary>
        ///     �����ͬʱִ�е�SQL(����֮ǰ����ִ��)
        /// </summary>
        /// <param name="condition">��ǰ������ִ������</param>
        /// <returns></returns>
        public string BeforeUpdateSql(string condition)
        {
            if (NoInjection)
                return "";
            var code = new StringBuilder();
            BeforeUpdateSql(code, condition);
            //DataUpdateHandler.BeforeUpdateSql(Table, code, condition);
            return code.ToString();
        }

        /// <summary>
        ///     �����ͬʱִ�е�SQL(����֮������ִ��)
        /// </summary>
        /// <param name="condition">��ǰ������ִ������</param>
        /// <returns></returns>
        public string AfterUpdateSql(string condition)
        {
            if (NoInjection)
                return "";
            var code = new StringBuilder();
            AfterUpdateSql(code, condition);
            //DataUpdateHandler.AfterUpdateSql(Table, code, condition);
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

        #region ����

        /// <summary>
        ///     �õ��ֶε�MySqlDbType����
        /// </summary>
        /// <param name="field">�ֶ�����</param>
        /// <returns>����</returns>
        int ISqlBuilder<TEntity>.GetDbType(string field)
        {
            return (int)GetDbType(field);
        }

        /// <summary>
        ///     �õ��ֶε�MySqlDbType����
        /// </summary>
        /// <param name="field">�ֶ�����</param>
        /// <returns>����</returns>
        public virtual MySqlDbType GetDbType(string field)
        {
            return MySqlDbType.VarString;
        }


        /// <summary>
        ///     �����ֶεĲ���
        /// </summary>
        /// <param name="field">���ɲ������ֶ�</param>
        public MySqlParameter CreateFieldParameter(string field)
        {
            return new MySqlParameter(field, GetDbType(field));
        }

        /// <summary>
        ///     �����ֶεĲ���
        /// </summary>
        /// <param name="field">���ɲ������ֶ�</param>
        /// <param name="dbType"></param>
        /// <param name="value">ֵ</param>
        public DbParameter CreateFieldParameter(string field, int dbType, object value)
        {
            return CreateParameter(field, value, (MySqlDbType)dbType);
        }

        /// <summary>
        ///     �����ֶεĲ���
        /// </summary>
        /// <param name="field">���ɲ������ֶ�</param>
        /// <param name="type"></param>
        /// <param name="value">ֵ</param>
        public DbParameter CreateFieldParameter(string field, MySqlDbType type, object value)
        {
            return CreateParameter(field, value, type);
        }

        /// <summary>
        ///     �����ֶεĲ���
        /// </summary>
        /// <param name="field">���ɲ������ֶ�</param>
        /// <param name="entity">ȡֵ��ʵ��</param>
        public DbParameter CreateFieldParameter(string field, EditDataObject entity)
        {
            return CreateFieldParameter(field, GetDbType(field), entity.GetValue(field));
        }

        /// <summary>
        ///     �����ֶεĲ���
        /// </summary>
        /// <param name="field">���ɲ������ֶ�</param>
        /// <param name="entity">ȡֵ��ʵ��</param>
        /// <param name="entityField">ȡֵ���ֶ�</param>
        public DbParameter CreateFieldParameter(string field, DataObjectBase entity, string entityField)
        {
            return CreateFieldParameter(field, GetDbType(field), entity.GetValue(entityField));
        }

        /// <summary>
        ///     ���������ֶεĲ���
        /// </summary>
        public DbParameter CreatePimaryKeyParameter()
        {
            return new MySqlParameter(SqlOption.KeyField, GetDbType(SqlOption.KeyField));
        }

        /// <summary>
        ///     ���������ֶεĲ���
        /// </summary>
        /// <param name="value">����ֵ</param>
        public DbParameter CreatePimaryKeyParameter(object value)
        {
            return CreateParameter(SqlOption.KeyField, value, GetDbType(SqlOption.KeyField));
        }

        /// <summary>
        ///     ���������ֶεĲ���
        /// </summary>
        /// <param name="entity">ȡֵ��ʵ��</param>
        public DbParameter CreatePimaryKeyParameter(EditDataObject entity)
        {
            return CreateParameter(SqlOption.KeyField, entity.GetValue(SqlOption.KeyField), GetDbType(SqlOption.KeyField));
        }
        #endregion

        #region ����Sql����

        /// <summary>
        ///     ����Sql����
        /// </summary>
        /// <param name="csharpType">C#������</param>
        /// <param name="parameterName">��������</param>
        /// <param name="value">����ֵ</param>
        /// <returns>����</returns>
        public MySqlParameter CreateParameter(string csharpType, string parameterName, object value)
        {
            if (value is Enum)
            {
                return new MySqlParameter(parameterName, MySqlDbType.Int32)
                {
                    Value = Convert.ToInt32(value)
                };
            }
            if (value is bool b)
            {
                return new MySqlParameter(parameterName, MySqlDbType.Byte)
                {
                    Value = b ? (byte)1 : (byte)0
                };
            }
            return CreateParameter(parameterName, value, ToSqlDbType(csharpType));
        }

        DbParameter IParameterCreater.CreateParameter(string parameterName, object value)
        {
            return CreateParameter(parameterName, value);
        }

        DbParameter IParameterCreater.CreateParameter(string parameterName, string value)
        {
            return CreateParameter(parameterName, value);
        }

        DbParameter IParameterCreater.CreateParameter<T>(string parameterName, T value)
        {
            return CreateParameter(parameterName, value);
        }


        /// <summary>
        ///     ����Sql����
        /// </summary>
        /// <param name="dbType">��Ӧ���ݿ��DbType����MysqlDbType</param>
        /// <param name="parameterName">��������</param>
        /// <param name="value">����ֵ</param>
        /// <returns>����</returns>
        DbParameter IParameterCreater.CreateParameter(string parameterName, object value, int dbType)
        {
            return CreateParameter(parameterName, value, (MySqlDbType)dbType);
        }

        /// <summary>
        ///     ����Sql����
        /// </summary>
        /// <param name="parameterName">��������</param>
        /// <param name="value">����ֵ</param>
        /// <param name="type">����</param>
        /// <returns>����</returns>
        public MySqlParameter CreateParameter(string parameterName, object value, MySqlDbType type)
        {
            object val;
            switch (value)
            {
                case null:
                case DBNull _:
                    val = DBNull.Value;
                    break;
                case Enum _:
                    val = Convert.ToInt32(value);
                    break;
                case bool b:
                    val = b ? (byte)1 : (byte)0;
                    break;
                default:
                    val = value;
                    break;
                case string s:
                    return CreateParameter(parameterName, s);
            }

            return new MySqlParameter(parameterName, type)
            {
                Value = val
            };
        }

        DbParameter IParameterCreater.CreateDbParameter(string name, string typeFile, object value)
            => new MySqlParameter
            {
                ParameterName = name,
                MySqlDbType = GetDbType(typeFile),
                Value = value
            };

        DbParameter IParameterCreater.CreateParameter(string csharpType, string parameterName, object value)
        {
            return CreateParameter(csharpType, parameterName, value);
        }

        /// <summary>
        ///     ����Sql����
        /// </summary>
        /// <param name="parameterName">��������</param>
        /// <param name="value">����ֵ</param>
        /// <returns>����</returns>
        public DbParameter CreateParameter(string parameterName, object value)
        {
            return CreateParameter(parameterName, value, ToSqlDbType(value?.GetType().Name));
        }

        /// <summary>
        ///     ����Sql����
        /// </summary>
        /// <param name="parameterName">��������</param>
        /// <param name="value">����ֵ</param>
        /// <returns>����</returns>
        public static MySqlParameter CreateParameter(string parameterName, string value)
        {
            MySqlDbType type = MySqlDbType.VarString;
            if (value == null)
            {
                return new MySqlParameter(parameterName, MySqlDbType.VarString, 10);
            }
            if (value.Length >= 4000)
            {
                type = MySqlDbType.Text;
            }
            else if (value.Length >= ushort.MaxValue)
            {
                type = MySqlDbType.LongText;
            }
            return new MySqlParameter(parameterName, type, value.Length)
            {
                Value = value
            };
        }

        /// <summary>
        ///     ����Sql����
        /// </summary>
        /// <param name="parameterName">��������</param>
        /// <param name="value">����ֵ</param>
        /// <returns>����</returns>
        public static MySqlParameter CreateParameter(string parameterName, bool value)
        {
            return new MySqlParameter(parameterName, MySqlDbType.Byte)
            {
                Value = value ? (byte)1 : (byte)0
            };
        }

        /// <summary>
        ///     ����Sql����
        /// </summary>
        /// <param name="parameterName">��������</param>
        /// <param name="value">����ֵ</param>
        /// <returns>����</returns>
        public static MySqlParameter CreateParameter<T>(string parameterName, T value)
            where T : struct
        {
            if (value is Enum)
            {
                return new MySqlParameter(parameterName, MySqlDbType.Int32)
                {
                    Value = Convert.ToInt32(value)
                };
            }
            return new MySqlParameter(parameterName, ToSqlDbType(typeof(T).Name))
            {
                Value = value
            };
        }


        /// <summary>
        ///     ��C#������תΪDBType
        /// </summary>
        /// <param name="csharpType"> </param>
        public static MySqlDbType ToSqlDbType(string csharpType)
        {
            switch (csharpType)
            {
                case "Boolean":
                case "bool":
                    return MySqlDbType.Bit;
                case "byte":
                case "Byte":
                case "sbyte":
                case "SByte":
                case "Char":
                case "char":
                    return MySqlDbType.Byte;
                case "short":
                case "Int16":
                case "ushort":
                case "UInt16":
                    return MySqlDbType.Int16;
                case "int":
                case "Int32":
                case "IntPtr":
                case "uint":
                case "UInt32":
                case "UIntPtr":
                    return MySqlDbType.Int32;
                case "long":
                case "Int64":
                case "ulong":
                case "UInt64":
                    return MySqlDbType.Int64;
                case "float":
                case "Float":
                    return MySqlDbType.Float;
                case "double":
                case "Double":
                    return MySqlDbType.Double;
                case "decimal":
                case "Decimal":
                    return MySqlDbType.Decimal;
                case "Guid":
                    return MySqlDbType.Guid;
                case "DateTime":
                    return MySqlDbType.DateTime;
                case "String":
                case "string":
                    return MySqlDbType.VarChar;
                case "Binary":
                case "byte[]":
                case "Byte[]":
                    return MySqlDbType.Binary;
                default:
                    return MySqlDbType.Binary;
            }
        }

        /// <summary>
        ///     ��C#������תΪDBType
        /// </summary>
        /// <param name="csharpType"> </param>
        public static DbType ToDbType(string csharpType)
        {
            switch (csharpType)
            {
                case "Boolean":
                case "bool":
                    return DbType.Boolean;
                case "byte":
                case "Byte":
                    return DbType.Byte;
                case "sbyte":
                case "SByte":
                    return DbType.SByte;
                case "short":
                case "Int16":
                    return DbType.Int16;
                case "ushort":
                case "UInt16":
                    return DbType.UInt16;
                case "int":
                case "Int32":
                case "IntPtr":
                    return DbType.Int32;
                case "uint":
                case "UInt32":
                case "UIntPtr":
                    return DbType.UInt32;
                case "long":
                case "Int64":
                    return DbType.Int64;
                case "ulong":
                case "UInt64":
                    return DbType.UInt64;
                case "float":
                case "Float":
                    return DbType.Single;
                case "double":
                case "Double":
                    return DbType.Double;
                case "decimal":
                case "Decimal":
                    return DbType.Decimal;
                case "Guid":
                    return DbType.Guid;
                case "DateTime":
                    return DbType.DateTime;
                case "Binary":
                case "byte[]":
                case "Byte[]":
                    return DbType.Binary;
                case "string":
                case "String":
                    return DbType.String;
                default:
                    return DbType.String;
            }
        }

        ConditionItem ISqlBuilder<TEntity>.Compile(Dictionary<string, string> map, Expression<Func<TEntity, bool>> lambda)
        {
            return PredicateConvert.Convert(this, SqlOption.FieldMap, lambda);
        }

        ConditionItem ISqlBuilder<TEntity>.Compile(Dictionary<string, string> map, LambdaItem<TEntity> lambda)
        {
            return PredicateConvert.Convert(this, SqlOption.FieldMap, lambda);
        }
        #endregion
    }
}