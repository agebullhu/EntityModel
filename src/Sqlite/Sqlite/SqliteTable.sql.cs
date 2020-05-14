// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

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
        #region ����

        /// <summary>
        ///     ���ɵ����ֶθ��µ�SQL
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="parameters">�����б�</param>
        /// <returns>�����ֶθ��µ�SQL</returns>
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
        ///     �õ�����ȷ���µ�����
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected virtual void CheckUpdateContition(ref string condition)
        {
        }


        /// <summary>
        ///     ���ɸ��µ�SQL
        /// </summary>
        /// <param name="valueExpression">���±��ʽ(SQL)</param>
        /// <param name="condition">��������</param>
        /// <returns>���µ�SQL</returns>
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
        ///     ���ɸ��µ�SQL
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">����</param>
        /// <param name="parameters">�����б�</param>
        /// <returns>���µ�SQL</returns>
        private string CreateUpdateSql(string field, object value, string condition, IList<DbParameter> parameters)
        {
            return CreateUpdateSql(FileUpdateSql(field, value, parameters), condition);
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
        private string CreateCollectSql(string fun, string field, string condition)
        {
            if (field != "*")
                field = $"[{FieldMap[field]}]";

            var sql = new StringBuilder();
            CreateLoadSql(sql, $"{fun}({field})", condition, null, false);
            return sql.ToString();
        }

        /// <summary>
        ///     ���������ֶ�ֵ��SQL���
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="condition">����</param>
        /// <returns>�����ֶ�ֵ��SQL���</returns>
        private string CreateLoadValueSql(string field, string condition)
        {
            var sql = new StringBuilder();
            CreateLoadSql(sql, FieldMap[field], condition, null, false,1);
            return sql.ToString();
        }

        /// <summary>
        ///     ��������ֵ��SQL
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="convert">����</param>
        /// <returns>�����ֶ�ֵ��SQL���</returns>
        private string CreateLoadValuesSql(string field, ConditionItem convert)
        {
            var sql = new StringBuilder();
            CreateLoadSql(sql, FieldMap[field], convert.ConditionSql, null, false,1);
            return sql.ToString();
        }

        /// <summary>
        ///     ���������SQL���
        /// </summary>
        /// <param name="condition">��������</param>
        /// <param name="order">�����ֶ�</param>
        /// <param name="desc">�Ƿ���</param>
        /// <returns>�����SQL���</returns>
        private string CreateOnceSql(string condition, string order, bool desc)
        {
            var sql = new StringBuilder();
            CreateLoadSql(sql, ContextLoadFields, condition, order, desc,1);
            return sql.ToString();
        }

        /// <summary>
        ///     ���������SQL���
        /// </summary>
        /// <param name="condition">��������</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>�����SQL���</returns>
        private string CreateLoadSql(string condition, string order)
        {
            var sql = new StringBuilder();
            CreateLoadSql(sql, ContextLoadFields, condition, order, false);
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
        ///     ���������SQL���
        /// </summary>
        /// <param name="sql">StringBuilder����</param>
        /// <param name="fields">�ֶ�</param>
        /// <param name="condition">��������</param>
        /// <param name="order">�����ֶ�</param>
        /// <param name="desc">�������</param>
        /// <param name="page">��1��ʼ��ҳ��</param>
        /// <param name="pageSize">ÿҳ����(ǿ�ƴ���0,С��500��)</param>
        /// <returns>�����SQL���</returns>
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

        #region ɾ��

        /// <summary>
        ///     ����ɾ����SQL���
        /// </summary>
        /// <param name="condition">ɾ������</param>
        /// <returns>ɾ����SQL���</returns>
        private string CreateDeleteSql(string condition)
        {
            return $@"{BeforeUpdateSql(condition)}

{DeleteSqlCode}
WHERE {condition};

{AfterUpdateSql(condition)}";
        }

        #endregion

        #region �ֶ�����


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
        ///     ���������е��ֶ�����
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="expression">�������ʽ</param>
        /// <returns>�ֶ�����</returns>
        public string FieldConditionSQL(string field, string expression = "=")
        {
            Debug.Assert(FieldDictionary.ContainsKey(field));
            return $@"[{FieldMap[field]}] {expression} @{field}";
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
            var join = isAnd ? "AND" : "OR";
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", FieldConditionSQL(fields[0]));
            for (var idx = 1; idx < fields.Length; idx++)
                sql.AppendFormat(@" {0} ({1}) ", join, FieldConditionSQL(fields[idx]));
            return sql.ToString();
        }


        /// <summary>
        ///     �õ�����ȷƴ�ӵ�SQL������䣨������û�У�
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
        ///     �õ�����ȷƴ�ӵ�SQL������䣨������û�У�
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        protected virtual void ConditionSqlCode(List<string> conditions)
        {
        }

        #endregion
    }
}