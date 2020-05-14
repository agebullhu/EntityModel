// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Common;
using Agebull.EntityModel.Events;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;

#endregion

namespace Agebull.EntityModel.Sqlite
{
    partial class SqliteTable<TData, TDataBase>
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
            if (value is string || value is DateTime || value is Guid || value is byte[])
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
        ///     得到可正确更新的条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected virtual void CheckUpdateContition(ref string condition)
        {
        }


        /// <summary>
        ///     生成更新的SQL
        /// </summary>
        /// <param name="valueExpression">更新表达式(SQL)</param>
        /// <param name="condition">更新条件</param>
        /// <returns>更新的SQL</returns>
        private string CreateUpdateSql(string valueExpression, string condition)
        {
            CheckUpdateContition(ref condition);
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

            var sql = new StringBuilder();
            CreateLoadSql(sql, $"{fun}({field})", condition, null, false);
            return sql.ToString();
        }

        /// <summary>
        ///     生成载入字段值的SQL语句
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="condition">条件</param>
        /// <returns>载入字段值的SQL语句</returns>
        private string CreateLoadValueSql(string field, string condition)
        {
            var sql = new StringBuilder();
            CreateLoadSql(sql, FieldMap[field], condition, null, false,1);
            return sql.ToString();
        }

        /// <summary>
        ///     生成载入值的SQL
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="convert">条件</param>
        /// <returns>载入字段值的SQL语句</returns>
        private string CreateLoadValuesSql(string field, ConditionItem convert)
        {
            var sql = new StringBuilder();
            CreateLoadSql(sql, FieldMap[field], convert.ConditionSql, null, false,1);
            return sql.ToString();
        }

        /// <summary>
        ///     生成载入的SQL语句
        /// </summary>
        /// <param name="condition">数据条件</param>
        /// <param name="order">排序字段</param>
        /// <param name="desc">是否反序</param>
        /// <returns>载入的SQL语句</returns>
        private string CreateOnceSql(string condition, string order, bool desc)
        {
            var sql = new StringBuilder();
            CreateLoadSql(sql, ContextLoadFields, condition, order, desc,1);
            return sql.ToString();
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
            CreateLoadSql(sql, ContextLoadFields, condition, order, false);
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
            var orderField = string.IsNullOrWhiteSpace(order) || !FieldDictionary.ContainsKey(order)
                ? KeyField
                : FieldDictionary[order];

            var sql = new StringBuilder();
            CreateLoadSql(sql, ContextLoadFields, condition, orderField, desc,pageSize,page);
            return sql.ToString();
        }

        /// <summary>
        ///     生成载入的SQL语句
        /// </summary>
        /// <param name="sql">StringBuilder对象</param>
        /// <param name="fields">字段</param>
        /// <param name="condition">数据条件</param>
        /// <param name="order">排序字段</param>
        /// <param name="desc">正序或反序</param>
        /// <param name="page">从1开始的页号</param>
        /// <param name="pageSize">每页几行(强制大于0,小于500行)</param>
        /// <returns>载入的SQL语句</returns>
        private void CreateLoadSql(StringBuilder sql, string fields, string condition, string order, bool desc, int pageSize = 0, int page=-1)
        {
            sql.AppendLine(@"SELECT");
            sql.AppendLine(fields);
            sql.AppendLine($"FROM {ContextReadTable}");
            ConditionSqlCode(sql, condition);
            if (!string.IsNullOrWhiteSpace(order))
            {
                sql.Append($"ORDER BY {order}");
                if (desc)
                    sql.Append(" DESC");
                sql.AppendLine();
            }
            if (pageSize > 0 )
            {
                sql.Append($"Limit {pageSize}");
                if (page > 1)
                {
                    sql.Append($"Offset {(page - 1) * pageSize};");
                }
            }
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

{DeleteSqlCode}
WHERE {condition};

{AfterUpdateSql(condition)}";
        }

        #endregion

        #region 字段条件


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
            var join = isAnd ? "AND" : "OR";
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", FieldConditionSQL(fields[0]));
            for (var idx = 1; idx < fields.Length; idx++)
                sql.AppendFormat(@" {0} ({1}) ", join, FieldConditionSQL(fields[idx]));
            return sql.ToString();
        }


        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="code"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private void ConditionSqlCode(StringBuilder code, string condition)
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
            ConditionSqlCode(conditions);
            DataUpdateHandler.ConditionSqlCode(this, conditions);
            if (conditions.Count == 0)
                return;
            code.Append("WHERE ");
            bool isFirst = true;
            foreach (var con in conditions)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    code.Append(" AND ");
                }
                code.Append($"({con})");
            }
            code.AppendLine();
        }

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        protected virtual void ConditionSqlCode(List<string> conditions)
        {
        }

        #endregion
    }
}