// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.Events;

#endregion

namespace Agebull.EntityModel.SqlServer
{
    partial class SqlServerTable<TData, TDataBase>
    {
        #region 更新

        /// <summary>
        ///     生成单个字段更新的SQL
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>单个字段更新的SQL</returns>
        private string FileUpdateSql(string field, object value, IList<DbParameter> parameters)
        {
            field = FieldDictionary[field];
            if (value == null)
                return $"[{field}] = NULL";
            if (value is string || value is DateTime || value is byte[])
            {
                var name = "v_" + field;
                parameters.Add(CreateFieldParameter(name, GetDbType(field), value));
                return $"[{field}] = @{name}";
            }
            if (value is bool b)
                value = b ? 1 : 0;
            else if (value is Enum)
                value = Convert.ToInt32(value);
            return $"[{field}] = {value}";
        }


        /// <summary>
        ///     生成更新的SQL
        /// </summary>
        /// <param name="valueExpression">更新表达式(SQL)</param>
        /// <param name="condition">更新条件</param>
        /// <returns>更新的SQL</returns>
        private string CreateUpdateSql(string valueExpression, string condition)
        {
            return $@"{BeforeUpdateSql(condition)}
UPDATE [{ContextWriteTable}]
   SET {valueExpression} 
 WHERE {condition};
{AfterUpdateSql(condition)}";
        }


        /// <summary>
        ///     生成更新的SQL
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">条件</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>更新的SQL</returns>
        private string CreateUpdateSql(string field, object value, string condition, IList<DbParameter> parameters)
        {
            return CreateUpdateSql(FileUpdateSql(field, value, parameters), condition);
        }
        #endregion

        #region 载入

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
        private string ContitionSqlCode(string condition)
        {
            List<string> conditions = new List<string>();
            if (!_baseConditionInited)
            {
                InitBaseCondition();
                _baseConditionInited = true;
            }
            if (!string.IsNullOrEmpty(BaseCondition))
                conditions.Add(BaseCondition);
            if (!string.IsNullOrEmpty(condition))
                conditions.Add(condition);
            ContitionSqlCode(conditions);
            DataUpdateHandler.ContitionSqlCode<TData>(TableId, conditions); 
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
        protected virtual void ContitionSqlCode(List<string> conditions)
        {
        }

        /// <summary>
        ///     生成汇总的SQL语句
        /// </summary>
        /// <param name="fun">汇总函数名称</param>
        /// <param name="field">汇总字段</param>
        /// <param name="condition">汇总条件</param>
        /// <returns>汇总的SQL语句</returns>
        private string CreateCollectSql(string fun, string field, string condition)
        {
            if (field != "*")
                field = $"[{FieldMap[field]}]";
            var sql = $@"SELECT {fun}({field}) FROM {ContextReadTable}{ContitionSqlCode(condition)};";
            return sql;
        }

        /// <summary>
        ///     生成载入字段值的SQL语句
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="condition">条件</param>
        /// <returns>载入字段值的SQL语句</returns>
        private string CreateLoadValueSql(string field, string condition)
        {
            Debug.Assert(FieldDictionary.ContainsKey(field));
            return $@"SELECT [{FieldMap[field]}] FROM {ContextReadTable}{ContitionSqlCode(condition)};";
        }

        /// <summary>
        ///     生成载入值的SQL
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="convert">条件</param>
        /// <returns>载入字段值的SQL语句</returns>
        private string CreateLoadValuesSql(string field, ConditionItem convert)
        {
            return $@"SELECT [{FieldMap[field]}] 
FROM {ContextReadTable}{ContitionSqlCode(convert.ConditionSql)};";
        }

        /// <summary>
        ///     生成载入的SQL语句
        /// </summary>
        /// <param name="condition">数据条件</param>
        /// <param name="order">排序字段</param>
        /// <param name="desc">是否反序</param>
        /// <returns>载入的SQL语句</returns>
        private StringBuilder CreateOnceSql(string condition, string order,bool desc)
        {
            var sql = new StringBuilder();
            sql.AppendLine(@"SELECT TOP 1");
            sql.AppendLine(ContextLoadFields);
            sql.AppendFormat(@"FROM {0}", ContextReadTable);
            sql.AppendLine(ContitionSqlCode(condition));
            if (!string.IsNullOrWhiteSpace(order))
            {
                sql.AppendLine();
                sql.Append($"ORDER BY {order}");
            }
            sql.Append(";");
            return sql;
        }
        /// <summary>
        ///     生成载入的SQL语句
        /// </summary>
        /// <param name="condition">数据条件</param>
        /// <param name="order">排序字段</param>
        /// <returns>载入的SQL语句</returns>
        private string CreateLoadSql(string condition, string order)
        {
            var sql = new StringBuilder();
            sql.AppendLine(@"SELECT");
            sql.AppendLine(ContextLoadFields);
            sql.AppendFormat(@"FROM {0}", ContextReadTable);
            sql.AppendLine(ContitionSqlCode(condition));
            if (!string.IsNullOrWhiteSpace(order))
            {
                sql.AppendLine();
                sql.Append($"ORDER BY {order}");
            }
            sql.Append(";");
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
        private string CreatePageSql(int page, int pageSize, string order, bool desc, string condition)
        {
            if(pageSize <=0 || page > 0)
            {
                return CreateLoadSql(condition, $@" [{order}] {(desc ? "DESC" : "ASC")}");
            }
            var orderField = string.IsNullOrWhiteSpace(order) || !FieldDictionary.ContainsKey(order)
                ? KeyField
                : FieldDictionary[order];

            return $@"SELECT * FROM (
    SELECT {ContextLoadFields},
           ROW_NUMBER() OVER (ORDER BY [{orderField}] {(desc ? "DESC" : "ASC")}) AS __rs
      FROM {ContextReadTable}{ContitionSqlCode(condition)}
) t WHERE __rs > {(page - 1) * pageSize} AND __rs <= {page * pageSize};";
        }

        #endregion

        #region 删除

        /// <summary>
        ///     生成删除的SQL语句
        /// </summary>
        /// <param name="condition">删除条件</param>
        /// <returns>删除的SQL语句</returns>
        private string CreateDeleteSql(string condition)
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
        private string CreateDeleteSql(ConditionItem convert)
        {
            return CreateDeleteSql(convert.ConditionSql);
        }

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
            Debug.Assert(FieldDictionary.ContainsKey(field));
            return $@"[{FieldMap[field]}] {expression} @{field}";
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

        #endregion
    }
}