// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

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
        ///     ���ɸ��µ�SQL
        /// </summary>
        /// <param name="valueExpression">���±��ʽ(SQL)</param>
        /// <param name="condition">��������</param>
        /// <returns>���µ�SQL</returns>
        private string CreateUpdateSql(string valueExpression, string condition)
        {
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
        ///     �õ�����ȷƴ�ӵ�SQL������䣨������û�У�
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        protected virtual void ContitionSqlCode(List<string> conditions)
        {
        }

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
            var sql = $@"SELECT {fun}({field}) FROM {ContextReadTable}{ContitionSqlCode(condition)};";
            return sql;
        }

        /// <summary>
        ///     ���������ֶ�ֵ��SQL���
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="condition">����</param>
        /// <returns>�����ֶ�ֵ��SQL���</returns>
        private string CreateLoadValueSql(string field, string condition)
        {
            Debug.Assert(FieldDictionary.ContainsKey(field));
            return $@"SELECT [{FieldMap[field]}] FROM {ContextReadTable}{ContitionSqlCode(condition)};";
        }

        /// <summary>
        ///     ��������ֵ��SQL
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="convert">����</param>
        /// <returns>�����ֶ�ֵ��SQL���</returns>
        private string CreateLoadValuesSql(string field, ConditionItem convert)
        {
            return $@"SELECT [{FieldMap[field]}] 
FROM {ContextReadTable}{ContitionSqlCode(convert.ConditionSql)};";
        }

        /// <summary>
        ///     ���������SQL���
        /// </summary>
        /// <param name="condition">��������</param>
        /// <param name="order">�����ֶ�</param>
        /// <param name="desc">�Ƿ���</param>
        /// <returns>�����SQL���</returns>
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
        ///     ���������SQL���
        /// </summary>
        /// <param name="condition">��������</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>�����SQL���</returns>
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

        #region ɾ��

        /// <summary>
        ///     ����ɾ����SQL���
        /// </summary>
        /// <param name="condition">ɾ������</param>
        /// <returns>ɾ����SQL���</returns>
        private string CreateDeleteSql(string condition)
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
        private string CreateDeleteSql(ConditionItem convert)
        {
            return CreateDeleteSql(convert.ConditionSql);
        }

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
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", FieldConditionSQL(fields[0]));
            for (var idx = 1; idx < fields.Length; idx++)
                sql.AppendFormat(@" {0} ({1}) ", isAnd ? "AND" : "OR", FieldConditionSQL(fields[idx]));
            return sql.ToString();
        }

        #endregion
    }
}