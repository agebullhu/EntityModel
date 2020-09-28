// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

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
        ///     生成更新的SQL
        /// </summary>
        /// <param name="valueExpression">更新表达式(SQL)</param>
        /// <param name="condition">更新条件</param>
        /// <returns>更新的SQL</returns>
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
        ///     生成更新的SQL
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="condition">更新条件</param>
        /// <returns>更新的SQL</returns>
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

        #region 载入

        /// <summary>
        ///     删除的SQL语句
        /// </summary>
        public string FullLoadSqlCode => $@"SELECT {Option.LoadFields} FROM `{Option.ReadTableName}`";


        private string _primaryKeyCondition;

        /// <summary>
        ///     主键的条件部分SQL
        /// </summary>
        public string PrimaryKeyCondition => _primaryKeyCondition ??= FieldCondition(Option.PrimaryKey);


        string ISqlBuilder.OrderCode(bool desc, string field) => $"`{Option.FieldMap[field]}` {(desc ? "DESC" : "")}";

        /// <summary>
        ///     查询条件注入
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
        ///     查询条件注入
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
            condition = InjectionLoadCondition(condition);
            return $@"SELECT {fun}({field}) FROM {Option.ReadTableName}{condition};";
        }

        /// <summary>
        ///     生成载入字段值的SQL语句
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="condition">条件</param>
        /// <returns>载入字段值的SQL语句</returns>
        public string CreateLoadValueSql(string field, string condition)
        {
            condition = InjectionLoadCondition(condition);
            return $@"SELECT `{Option.FieldMap[field]}` FROM {Option.ReadTableName}{condition};";
        }

        /// <summary>
        ///     生成载入值的SQL
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="convert">条件</param>
        /// <returns>载入字段值的SQL语句</returns>
        public string CreateLoadValuesSql(string field, ConditionItem convert)
        {
            var condition = InjectionLoadCondition(convert.ConditionSql);
            return $@"SELECT `{Option.FieldMap[field]}` 
FROM {Option.ReadTableName}{condition};";
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

        #region 删除

        /// <summary>
        ///     生成删除的SQL语句
        /// </summary>
        /// <param name="condition">删除条件</param>
        /// <returns>删除的SQL语句</returns>
        public string CreateDeleteSql(string condition)
        {
            if (string.IsNullOrEmpty(condition))
            {
                throw new ArgumentException("删除数据必须有一个条件");
            }
            if (!Option.NoInjection && Provider.Injection != null)
                condition = Provider.Injection.InjectionDeleteCondition(condition);
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
        public string PhysicalDeleteSqlCode(string condition)
        {
            if (string.IsNullOrEmpty(condition))
            {
                throw new ArgumentException("删除数据必须有一个条件");
            }
            if (!Option.NoInjection && Provider.Injection != null)
                condition = Provider.Injection.InjectionDeleteCondition(condition);

            return $"DELETE FROM `{Option.WriteTableName}` WHERE {condition};";
        }
        #endregion

        #region 字段条件

        string ISqlBuilder.Condition(string fieldName, string paraName)
            => $"(`{Option.FieldMap[fieldName]}` = ?{paraName})";

        string ISqlBuilder.Condition(string fieldName, string paraName, string expression)
            => $"(`{Option.FieldMap[fieldName]}` {expression} ?{paraName})";

        /// <summary>
        ///     组合条件SQL
        /// </summary>
        /// <param name="isAnd">是否用AND组合</param>
        /// <param name="conditions">条件</param>
        public string ConcatCondition(bool isAnd, params string[] conditions)
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
        public string ConcatCondition(bool isAnd, params (string field, object value)[] fields)
        {

            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"没有字段用于生成组合条件", nameof(fields));
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", FieldCondition(fields[0].field));
            var join = isAnd ? "AND" : "OR";
            for (var idx = 1; idx < fields.Length; idx++)
                sql.Append($" {join} ({FieldCondition(fields[idx].field)}) ");
            return sql.ToString();
        }

        #endregion
        #region 字段条件

        /// <summary>
        ///     用在条件中的字段条件
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="expression">条件表达式</param>
        /// <returns>字段条件</returns>
        public string FieldCondition(string field, string expression = "=")
            => $@"`{Option.FieldMap[field]}` {expression} ?{field}";

        /// <summary>
        ///     连接字段条件SQL
        /// </summary>
        /// <param name="isAnd">是否用AND组合</param>
        /// <param name="fields">生成参数的字段</param>
        public string ConcatFieldCondition(bool isAnd, params string[] fields)
        {
            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"没有字段用于生成组合条件", nameof(fields));
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", FieldCondition(fields[0]));
            for (var idx = 1; idx < fields.Length; idx++)
                sql.AppendFormat(@" {0} ({1}) ", isAnd ? "AND" : "OR", FieldCondition(fields[idx]));
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
                DataOperator.GetValue(entity, field),
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