// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

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
    ///     Sql实体访问类
    /// </summary>
    public class MySqlSqlBuilder<TEntity> : SimpleConfig, ISqlBuilder<TEntity>, IParameterCreater
        where TEntity : EditDataObject, new()
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => DataBaseType.MySql;

        string ISqlBuilder<TEntity>.Condition(string fieldName, string paraName) => $"(`{SqlOption.FieldMap[fieldName]}` = ?{paraName})";

        string ISqlBuilder<TEntity>.Condition(string fieldName, string paraName, string condition) => $"(`{SqlOption.FieldMap[fieldName]}` {condition} ?{paraName})";

        /// <summary>
        /// Sql对应的配置信息
        /// </summary>
        public DataAccessSqlOption<TEntity> SqlOption { get; set; }

        /// <summary>
        /// 不做代码注入
        /// </summary>
        public bool NoInjection { get; set; }


        #region 更新

        /// <summary>
        /// 取得仅更新的SQL语句
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
        /// 取得仅更新的SQL语句
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
        ///     生成单个字段更新的SQL
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>单个字段更新的SQL</returns>
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
        ///     生成更新的SQL
        /// </summary>
        /// <param name="valueExpression">更新表达式(SQL)</param>
        /// <param name="condition">更新条件</param>
        /// <returns>更新的SQL</returns>
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
        ///     得到可正确更新的条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected virtual void CheckUpdateContition(ref string condition)
        {
        }

        #endregion

        #region 载入

        /// <summary>
        ///     删除的SQL语句
        /// </summary>
        public virtual string FullLoadSqlCode => $@"SELECT {SqlOption.LoadFields} FROM `{SqlOption.ReadTableName}`";



        private string _primaryConditionSQL;

        /// <summary>
        ///     主键的条件部分SQL
        /// </summary>
        public string PrimaryKeyConditionSQL => _primaryConditionSQL ??= FieldConditionSQL(SqlOption.KeyField);


        string ISqlBuilder<TEntity>.OrderSql(bool desc, string field) => $"`{SqlOption.FieldMap[field]}` {(desc ? "DESC" : "")}";

        /// <summary>
        /// 基本条件初始化完成的标识
        /// </summary>
        private bool _baseConditionInited;

        /// <summary>
        ///  初始化基本条件
        /// </summary>
        /// <returns></returns>
        protected virtual void InitBaseCondition()
        {
        }

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
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
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        protected virtual void ConditionSqlCode(List<string> conditions)
        {
        }

        /// <summary>
        ///     生成汇总的SQL语句
        /// </summary>
        /// <param name="fun">汇总函数名称</param>
        /// <param name="field">汇总字段</param>
        /// <param name="condition">汇总条件</param>
        /// <returns>汇总的SQL语句</returns>
        public string CreateCollectSql(string fun, string field, string condition)
        {
            if (field != "*")
                field = $"`{SqlOption.FieldMap[field]}`";
            var sql = $@"SELECT {fun}({field}) FROM {SqlOption.ReadTableName}{InjectionCondition(condition)};";
            return sql;
        }

        /// <summary>
        ///     生成载入字段值的SQL语句
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="condition">条件</param>
        /// <returns>载入字段值的SQL语句</returns>
        public string CreateLoadValueSql(string field, string condition)
        {
            Debug.Assert(SqlOption.FieldMap.ContainsKey(field));
            return $@"SELECT `{SqlOption.FieldMap[field]}` FROM {SqlOption.ReadTableName}{InjectionCondition(condition)};";
        }

        /// <summary>
        ///     生成载入值的SQL
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="convert">条件</param>
        /// <returns>载入字段值的SQL语句</returns>
        public string CreateLoadValuesSql(string field, ConditionItem convert)
        {
            return $@"SELECT `{SqlOption.FieldMap[field]}` 
FROM {SqlOption.ReadTableName}{InjectionCondition(convert.ConditionSql)};";
        }

        /// <summary>
        ///     生成载入的SQL语句
        /// </summary>
        /// <param name="condition">数据条件</param>
        /// <param name="order">排序字段</param>
        /// <returns>载入的SQL语句</returns>
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
        ///     生成分页的SQL
        /// </summary>
        /// <param name="page">页号</param>
        /// <param name="pageSize">每页几行(强制大于0,小于500行)</param>
        /// <param name="order">排序字段</param>
        /// <param name="desc">是否倒序</param>
        /// <param name="condition">数据条件</param>
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

        #region 删除

        /// <summary>
        ///     生成删除的SQL语句
        /// </summary>
        /// <param name="condition">删除条件</param>
        /// <returns>删除的SQL语句</returns>
        public string CreateDeleteSql(string condition)
        {
            return $@"{BeforeUpdateSql(condition)}
{DeleteSqlCode} WHERE {condition};
{AfterUpdateSql(condition)}";
        }

        /// <summary>
        ///     生成删除的SQL语句
        /// </summary>
        /// <param name="convert">删除条件</param>
        /// <returns>删除的SQL语句</returns>
        public string CreateDeleteSql(ConditionItem convert)
        {
            return CreateDeleteSql(convert.ConditionSql);
        }

        /// <summary>
        ///     物理删除数据
        /// </summary>
        public string PhysicalDeleteSql(string condition)
        {
            return string.IsNullOrEmpty(condition)
                ? $"DELETE FROM `{SqlOption.WriteTableName}"
                : $"DELETE FROM `{SqlOption.WriteTableName}` WHERE {condition};";
        }
        /// <summary>
        ///     删除的SQL语句
        /// </summary>
        public virtual string DeleteSqlCode => $@"DELETE FROM `{SqlOption.ReadTableName}`";

        #endregion

        #region 字段条件

        /// <summary>
        ///     用在条件中的字段条件
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="expression">条件表达式</param>
        /// <returns>字段条件</returns>
        public string FieldConditionSQL(string field, string expression = "=")
        {
            Debug.Assert(SqlOption.FieldMap.ContainsKey(field));
            return $@"`{SqlOption.FieldMap[field]}` {expression} ?{field}";
        }

        /// <summary>
        ///     组合条件SQL
        /// </summary>
        /// <param name="isAnd">是否用AND组合</param>
        /// <param name="conditions">条件</param>
        public string JoinConditionSQL(bool isAnd, params string[] conditions)
        {
            if (conditions == null || conditions.Length == 0)
                throw new ArgumentException(@"没有条件用于组合", nameof(conditions));
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", conditions[0]);
            for (var idx = 1; idx < conditions.Length; idx++)
                sql.Append($@" {(isAnd ? "AND" : "OR")} ({conditions[idx]}) ");
            return sql.ToString();
        }

        /// <summary>
        ///     连接字段条件SQL
        /// </summary>
        /// <param name="isAnd">是否用AND组合</param>
        /// <param name="fields">生成参数的字段</param>
        public string FieldConditionSQL(bool isAnd, params string[] fields)
        {
            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"没有字段用于生成组合条件", nameof(fields));
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", FieldConditionSQL(fields[0]));
            for (var idx = 1; idx < fields.Length; idx++)
                sql.AppendFormat(@" {0} ({1}) ", isAnd ? "AND" : "OR", FieldConditionSQL(fields[idx]));
            return sql.ToString();
        }


        /// <summary>
        ///     连接字段条件SQL
        /// </summary>
        /// <param name="isAnd">是否用AND组合</param>
        /// <param name="fields">生成参数的字段</param>
        public string FieldConditionSQL(bool isAnd, params (string field, object value)[] fields)
        {
            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"没有字段用于生成组合条件", nameof(fields));
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", FieldConditionSQL(fields[0].field));
            var join = isAnd ? "AND" : "OR";
            for (var idx = 1; idx < fields.Length; idx++)
                sql.Append($" {join} ({FieldConditionSQL(fields[idx].field)}) ");
            return sql.ToString();
        }

        #endregion

        #region Sql注入

        /// <summary>
        ///     与更新同时执行的SQL(更新之前立即执行)
        /// </summary>
        /// <param name="condition">当前场景的执行条件</param>
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
        ///     与更新同时执行的SQL(更新之后立即执行)
        /// </summary>
        /// <param name="condition">当前场景的执行条件</param>
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
        ///     与更新同时执行的SQL(更新之前立即执行)
        /// </summary>
        /// <param name="code">写入SQL的文本构造器</param>
        /// <param name="condition">当前场景的执行条件</param>
        /// <returns></returns>
        protected virtual void BeforeUpdateSql(StringBuilder code, string condition)
        {
        }

        /// <summary>
        ///     与更新同时执行的SQL(更新之后立即执行)
        /// </summary>
        /// <param name="code">写入SQL的文本构造器</param>
        /// <param name="condition">当前场景的执行条件</param>
        protected virtual void AfterUpdateSql(StringBuilder code, string condition)
        {
        }

        #endregion

        #region 参数

        /// <summary>
        ///     得到字段的MySqlDbType类型
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>参数</returns>
        int ISqlBuilder<TEntity>.GetDbType(string field)
        {
            return (int)GetDbType(field);
        }

        /// <summary>
        ///     得到字段的MySqlDbType类型
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>参数</returns>
        public virtual MySqlDbType GetDbType(string field)
        {
            return MySqlDbType.VarString;
        }


        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        public MySqlParameter CreateFieldParameter(string field)
        {
            return new MySqlParameter(field, GetDbType(field));
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        /// <param name="dbType"></param>
        /// <param name="value">值</param>
        public DbParameter CreateFieldParameter(string field, int dbType, object value)
        {
            return CreateParameter(field, value, (MySqlDbType)dbType);
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        /// <param name="type"></param>
        /// <param name="value">值</param>
        public DbParameter CreateFieldParameter(string field, MySqlDbType type, object value)
        {
            return CreateParameter(field, value, type);
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        /// <param name="entity">取值的实体</param>
        public DbParameter CreateFieldParameter(string field, EditDataObject entity)
        {
            return CreateFieldParameter(field, GetDbType(field), entity.GetValue(field));
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        /// <param name="entity">取值的实体</param>
        /// <param name="entityField">取值的字段</param>
        public DbParameter CreateFieldParameter(string field, DataObjectBase entity, string entityField)
        {
            return CreateFieldParameter(field, GetDbType(field), entity.GetValue(entityField));
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        public DbParameter CreatePimaryKeyParameter()
        {
            return new MySqlParameter(SqlOption.KeyField, GetDbType(SqlOption.KeyField));
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        /// <param name="value">主键值</param>
        public DbParameter CreatePimaryKeyParameter(object value)
        {
            return CreateParameter(SqlOption.KeyField, value, GetDbType(SqlOption.KeyField));
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        /// <param name="entity">取值的实体</param>
        public DbParameter CreatePimaryKeyParameter(EditDataObject entity)
        {
            return CreateParameter(SqlOption.KeyField, entity.GetValue(SqlOption.KeyField), GetDbType(SqlOption.KeyField));
        }
        #endregion

        #region 生成Sql参数

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="csharpType">C#的类型</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
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
        ///     生成Sql参数
        /// </summary>
        /// <param name="dbType">对应数据库的DbType，如MysqlDbType</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        DbParameter IParameterCreater.CreateParameter(string parameterName, object value, int dbType)
        {
            return CreateParameter(parameterName, value, (MySqlDbType)dbType);
        }

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">类型</param>
        /// <returns>参数</returns>
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
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public DbParameter CreateParameter(string parameterName, object value)
        {
            return CreateParameter(parameterName, value, ToSqlDbType(value?.GetType().Name));
        }

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
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
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static MySqlParameter CreateParameter(string parameterName, bool value)
        {
            return new MySqlParameter(parameterName, MySqlDbType.Byte)
            {
                Value = value ? (byte)1 : (byte)0
            };
        }

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
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
        ///     从C#的类型转为DBType
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
        ///     从C#的类型转为DBType
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