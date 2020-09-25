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
    ///     Sql实体访问类
    /// </summary>
    public sealed class MySqlSqlBuilder<TEntity> : ParameterCreater, ISqlBuilder<TEntity>
    where TEntity : class, new()
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => DataBaseType.MySql;

        /// <summary>
        /// 自动构建数据库对象
        /// </summary>
        public IDataOperator<TEntity> DataOperator => Provider.DataOperator;

        /// <summary>
        /// 参数构造
        /// </summary>
        public IParameterCreater ParameterCreater => Provider.ParameterCreater;

        /// <summary>
        /// Sql语句构造器
        /// </summary>
        public DataAccessOption<TEntity> Option => Provider.Option;

        /// <summary>
        /// Sql语句构造器
        /// </summary>
        public ISqlBuilder<TEntity> SqlBuilder => Provider.SqlBuilder;

        /// <summary>
        /// 驱动提供者信息
        /// </summary>
        public DataAccessProvider<TEntity> Provider { get; set; }

        /// <summary>
        /// Sql对应的配置信息
        /// </summary>
        DataAccessOption<TEntity> ISqlBuilder<TEntity>.Option
        {
            get => Provider.Option;
            set => Provider.Option = value;
        }


        #region 数据结构支持

        /// <summary>
        /// 读取的字段
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
        /// 全量更新的字段
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
        /// 插入的代码
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
        /// 删除的代码
        /// </summary>
        /// <returns></returns>
        public string BuilderDeleteSqlCode() => $"DELETE FROM `{Option.DataSturct.WriteTableName}`";

        #endregion

        #region 更新

        /// <summary>
        ///     生成单个字段更新的SQL
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>单个字段更新的SQL</returns>
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
        ///     生成更新的SQL
        /// </summary>
        /// <param name="valueExpression">更新表达式(SQL)</param>
        /// <param name="condition">更新条件</param>
        /// <returns>更新的SQL</returns>
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
        ///     生成更新的SQL
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="condition">更新条件</param>
        /// <returns>更新的SQL</returns>
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

        #region 载入

        /// <summary>
        ///     删除的SQL语句
        /// </summary>
        public string FullLoadSqlCode => $@"SELECT {Option.LoadFields} FROM `{Option.ReadTableName}`";


        private string _primaryConditionSQL;

        /// <summary>
        ///     主键的条件部分SQL
        /// </summary>
        public string PrimaryKeyConditionSQL => _primaryConditionSQL ??= FieldConditionSQL(Option.PrimaryKey);


        string ISqlBuilder.OrderSql(bool desc, string field) => $"`{Option.FieldMap[field]}` {(desc ? "DESC" : "")}";

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
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
        ///     生成汇总的SQL语句
        /// </summary>
        /// <param name="fun">汇总函数名称</param>
        /// <param name="field">汇总字段</param>
        /// <param name="condition">汇总条件</param>
        /// <returns>汇总的SQL语句</returns>
        public string CreateCollectSql(string fun, string field, string condition)
        {
            if (field != "*")
                field = $"`{Option.FieldMap[field]}`";
            var sql = $@"SELECT {fun}({field}) FROM {Option.ReadTableName}{InjectionCondition(condition)};";
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
            return $@"SELECT `{Option.FieldMap[field]}` FROM {Option.ReadTableName}{InjectionCondition(condition)};";
        }

        /// <summary>
        ///     生成载入值的SQL
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="convert">条件</param>
        /// <returns>载入字段值的SQL语句</returns>
        public string CreateLoadValuesSql(string field, ConditionItem convert)
        {
            return $@"SELECT `{Option.FieldMap[field]}` 
FROM {Option.ReadTableName}{InjectionCondition(convert.ConditionSql)};";
        }

        /// <summary>
        ///     生成载入的SQL语句
        /// </summary>
        /// <param name="condition">数据条件</param>
        /// <param name="order">排序字段</param>
        /// <param name="limit"></param>
        /// <returns>载入的SQL语句</returns>
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

        #region 删除

        /// <summary>
        ///     生成删除的SQL语句
        /// </summary>
        /// <param name="condition">删除条件</param>
        /// <returns>删除的SQL语句</returns>
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
                throw new ArgumentException("删除数据必须有一个条件");
            }

            return $"{Option.DeleteSqlCode} WHERE {condition};";
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
                ? $"DELETE FROM `{Option.WriteTableName}"
                : $"DELETE FROM `{Option.WriteTableName}` WHERE {condition};";
        }
        #endregion

        #region 字段条件

        string ISqlBuilder.Condition(string fieldName, string paraName)
            => $"(`{Option.FieldMap[fieldName]}` = ?{paraName})";

        string ISqlBuilder.Condition(string fieldName, string paraName, string condition)
            => $"(`{Option.FieldMap[fieldName]}` {condition} ?{paraName})";

        /// <summary>
        ///     用在条件中的字段条件
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="expression">条件表达式</param>
        /// <returns>字段条件</returns>
        public string FieldConditionSQL(string field, string expression = "=")
            => $@"`{Option.FieldMap[field]}` {expression} ?{field}";

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

        #region 参数

        /// <summary>
        ///     得到字段的MySqlDbType类型
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>参数</returns>
        int ISqlBuilder.GetDbType(string field)
        {
            return DataOperator.GetDbType(field);
        }


        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        public MySqlParameter CreateFieldParameter(string field)
        {
            return new MySqlParameter(field, DataOperator.GetDbType(field));
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        /// <param name="entity">取值的实体</param>
        public DbParameter CreateFieldParameter(string field, TEntity entity)
        {
            return CreateParameter(field,
                Option.DataOperator.GetValue(entity, field),
                (MySqlDbType)DataOperator.GetDbType(field));
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        public DbParameter CreatePimaryKeyParameter()
        {
            return new MySqlParameter(Option.PrimaryKey, DataOperator.GetDbType(Option.PrimaryKey));
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        /// <param name="value">主键值</param>
        public DbParameter CreatePimaryKeyParameter(object value)
        {
            return CreateParameter(Option.PrimaryKey, value, (MySqlDbType)DataOperator.GetDbType(Option.PrimaryKey));
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        /// <param name="entity">取值的实体</param>
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