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
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

#endregion

namespace Agebull.Common.DataModel.Sqlite
{
    partial class SqliteTable<TData, TSqliteDataBase>
    {
        #region ���ݽṹ

        /// <summary>
        /// ��ʽ��Ϊ����SQL������
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ToSqlName(string name) => $"{FieldLeftChar}{name}{FieldRightChar}";

        /// <summary>
        /// ��ʽ��Ϊ����SQL������
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public string PropertyToSqlName(string property) => $"{FieldLeftChar}{FieldDictionary[property]}{FieldRightChar}";


        /// <summary>
        /// �ֶ�����ַ�
        /// </summary>
        public char FieldLeftChar => '[';

        /// <summary>
        /// �ֶ��Ҳ��ַ�
        /// </summary>
        public char FieldRightChar => ']';

        /// <summary>
        /// ����ǰ���ַ�
        /// </summary>
        public char ArgumentChar => '$';

        /// <summary>
        ///     ȫ���ȡ��SQL���
        /// </summary>
        protected abstract string FullLoadFields { get; }

        /// <summary>
        ///     ������
        /// </summary>
        protected abstract string ReadTableName { get; }
        /// <summary>
        ///     д����
        /// </summary>
        protected abstract string WriteTableName { get; }

        /// <summary>
        ///     ɾ����SQL���
        /// </summary>
        protected virtual string DeleteSqlCode => $@"DELETE FROM [{WriteTableName}]";

        /// <summary>
        ///     �����SQL���
        /// </summary>
        protected abstract string InsertSqlCode { get; }

        /// <summary>
        ///     ȫ�����µ�SQL���
        /// </summary>
        protected abstract string UpdateSqlCode { get; }

        /// <summary>
        ///     ������ѯ����
        /// </summary>
        public string BaseCondition { get; set; }

        #endregion

        #region ��ѯ�������(����lambda����)

        /// <summary>
        ///     �����ѯ����
        /// </summary>
        /// <param name="lambda">����</param>
        public ConditionItem Compile(Expression<Func<TData, bool>> lambda)
        {
            return PredicateConvert.Convert(FieldDictionary, lambda);
        }

        /// <summary>
        ///     �����ѯ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        public ConditionItem Compile(LambdaItem<TData> lambda)
        {
            return PredicateConvert.Convert(FieldDictionary, lambda);
        }

        /// <summary>
        ///     ȡ��������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<TData, T>> action)
        {
            if (action.Body is MemberExpression expression)
                return expression.Member.Name;
            if (!(action.Body is UnaryExpression body))
                throw new Exception("���ʽ̫����");

            expression = (MemberExpression)body.Operand;
            return expression.Member.Name;
        }


        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public TData FirstOrDefault(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return LoadFirst($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters));
        }

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public TData Last(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return LoadLast($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters));
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public TData First(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return LoadFirst($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters));
        }


        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public TData LastOrDefault(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return LoadLast($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters));
        }

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>����</returns>
        public List<TData> Select(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return LoadDataInner($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }


        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>����</returns>
        public List<TData> All(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return LoadDataInner($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�Ƿ��������</returns>
        public List<TData> Where(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return LoadDataInner($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public long Count(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            var obj = CollectInner("Count", "*",
                $"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public bool Any(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return ExistInner($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <param name="condition2">����2��Ĭ��Ϊ��</param>
        public decimal Sum<TValue>(Expression<Func<TData, TValue>> field, Expression<Func<TData, bool>> lambda,
            string condition2 = null)
        {
            var expression = (MemberExpression)field.Body;
            var convert = Compile(lambda);
            var condition = condition2 == null
                ? convert.ConditionSql
                : convert.ConditionSql == null
                    ? condition2
                    : $"({convert.ConditionSql}) AND ({condition2})";
            var obj = CollectInner("Sum", FieldMap[expression.Member.Name], condition, convert.Parameters);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public decimal Sum<TValue>(Expression<Func<TData, TValue>> field, Expression<Func<TData, bool>> a,
            Expression<Func<TData, bool>> b)
        {
            var expression = (MemberExpression)field.Body;
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            var obj = CollectInner("Sum", FieldMap[expression.Member.Name],
                $"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }
        #endregion

        #region ����

        /// <summary>
        ///     ���ɵ����ֶθ��µ�SQL
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="parameters">�����б�</param>
        /// <returns>�����ֶθ��µ�SQL</returns>
        private string FileUpdateSql(string field, object value, IList<SQLiteParameter> parameters)
        {
            field = FieldDictionary[field];
            if (value == null)
                return $"[{field}] = NULL";
            if (value is string || value is DateTime || value is byte[])
            {
                var name = "v_" + field;
                parameters.Add(CreateFieldParameter(name, value));
                return $"[{field}] = ${name}";
            }
            if (value is bool)
                value = (bool)value ? 1 : 0;
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
UPDATE [{WriteTableName}] 
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
        private string CreateUpdateSql(string field, object value, string condition, IList<SQLiteParameter> parameters)
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
            int cnt = 0;
            foreach (var con in conditions)
            {
                if (isFirst)
                {
                    isFirst = false;
                    code.AppendLine();
                    code.Append("WHERE ");
                }
                else
                {
                    if (++cnt == 5)
                    {
                        cnt = 0;
                        code.AppendLine(" AND ");
                    }
                    else
                    {
                        code.Append(" AND ");
                    }
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
            return $@"SELECT {PropertyToSqlName(field)} FROM {ContextReadTable}{ContitionSqlCode(condition)};";
        }

        /// <summary>
        ///     ��������ֵ��SQL
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="convert">����</param>
        /// <returns>�����ֶ�ֵ��SQL���</returns>
        private string CreateLoadValuesSql(string field, ConditionItem convert)
        {
            return $@"SELECT {PropertyToSqlName(field)}
FROM {ContextReadTable}{ContitionSqlCode(convert.ConditionSql)};";
        }

        /// <summary>
        ///     ���������SQL���
        /// </summary>
        /// <param name="condition">��������</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>�����SQL���</returns>
        private StringBuilder CreateLoadSql(string condition, string order)
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
        private string CreatePageSql(int page, int pageSize, string order, bool desc, string condition)
        {
            var orderField = string.IsNullOrWhiteSpace(order) || !FieldDictionary.ContainsKey(order)
                ? KeyField
                : FieldDictionary[order];

            var sql = new StringBuilder();
            sql.Append($@"SELECT DISTINCT {ContextLoadFields}
FROM {ContextReadTable}{ContitionSqlCode(condition)}
ORDER BY [{orderField}] {(desc ? "DESC" : "ASC")}");

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

        #region ����У��֧��

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="condition"></param>
        public bool IsUnique<TValue>(Expression<Func<TData, TValue>> field, object val, Expression<Func<TData, bool>> condition)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            var convert = Compile(condition);
            Debug.Assert(FieldDictionary.ContainsKey(fieldName));
            convert.AddAndCondition($"({PropertyToSqlName(fieldName)} = $c_vl_)", new SQLiteParameter
            {
                ParameterName = "c_vl_",
                DbType = GetDbType(fieldName),
                Value = val
            });
            return Exist(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="key"></param>
        public bool IsUnique<TValue>(Expression<Func<TData, TValue>> field, object val, object key)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            Debug.Assert(FieldDictionary.ContainsKey(fieldName));
            return Exist($"({PropertyToSqlName(fieldName)} = $c_vl_ AND {PrimaryKeyConditionSQL}",
                new SQLiteParameter
                {
                    ParameterName = "c_vl_",
                    DbType = GetDbType(fieldName),
                    Value = val
                },
                CreatePimaryKeyParameter(key));
        }

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        public bool IsUnique<TValue>(Expression<Func<TData, TValue>> field, object val)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            return Exist($"({PropertyToSqlName(fieldName)} = $c_vl_)", 
                new SQLiteParameter
                {
                    ParameterName = "c_vl_",
                    DbType = GetDbType(fieldName),
                    Value = val
                });
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
            return $@"{PropertyToSqlName(field)} {expression} ${field}";
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