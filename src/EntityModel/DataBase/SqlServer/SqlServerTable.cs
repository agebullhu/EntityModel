// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:YhxBank.FundsManagement
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

#endregion

namespace Gboxt.Common.DataModel.SqlServer
{
    /// <summary>
    ///     Sqlʵ�������
    /// </summary>
    /// <typeparam name="TData">ʵ��</typeparam>
    public abstract class SqlServerTable<TData> : IDataTable<TData>
        where TData : EditDataObject, new()
    {
        #region ���ݿ�

        /// <summary>
        ///     ���Ψһ��ʶ
        /// </summary>
        public abstract int TableId { get; }

        /// <summary>
        ///     �Զ��������Ӷ���
        /// </summary>
        private SqlServerDataBase _dataBase;

        /// <summary>
        ///     �Զ��������Ӷ���
        /// </summary>
        public SqlServerDataBase DataBase
        {
            get { return _dataBase ?? (_dataBase = SqlServerDataBase.DefaultDataBase ?? (_dataBase = SqlServerDataBase.DefaultDataBase = CreateDefaultDataBase())); }
            set { _dataBase = value; }
        }

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        public void Initialize()
        {
            DataBase = SqlServerDataBase.DefaultDataBase;
        }

        /// <summary>
        ///     ����һ��ȱʡ���õ����ݿ����
        /// </summary>
        /// <returns></returns>
        protected abstract SqlServerDataBase CreateDefaultDataBase();

        #endregion

        #region ���ݽṹ
        /// <summary>
        ///     ���ʱ�������ֶ�
        /// </summary>
        string IDataTable<TData>.PrimaryKey => PrimaryKey;

        /// <summary>
        ///     �Ƿ���Ϊ������ڵ�
        /// </summary>
        public bool IsBaseClass { get; set; }


        /// <summary>
        ///     ȫ���ȡ��SQL���
        /// </summary>
        protected abstract string FullLoadFields { get; }


        /// <summary>
        ///     ��̬��ȡ���ֶ�
        /// </summary>
        private string _contextReadFields;

        /// <summary>
        ///     ��̬��ȡ���ֶ�
        /// </summary>
        protected string ContextLoadFields
        {
            get { return _contextReadFields ?? FullLoadFields; }
            set { _contextReadFields = string.IsNullOrWhiteSpace(value) ? null : value; }
        }

        /// <summary>
        ///     ����
        /// </summary>
        protected abstract string ReadTableName { get; }

        /// <summary>
        ///     ��̬��ȡ�ı�
        /// </summary>
        protected string _dynamicReadTable;

        /// <summary>
        ///     ȡ�ö�̬��ȡ�ı���
        /// </summary>
        /// <returns>��̬��ȡ�ı���</returns>
        internal string GetDynamicReadTable()
        {
            return _dynamicReadTable;
        }

        /// <summary>
        ///     ȡ��ʵ�����õ�ContextReadTable��̬��ȡ�ı�
        /// </summary>
        /// <returns>֮ǰ�Ķ�̬��ȡ�ı���</returns>
        internal string SetDynamicReadTable(string table)
        {
            var old = _dynamicReadTable;
            _dynamicReadTable = string.IsNullOrWhiteSpace(table) ? null : table;
            return old;
        }

        /// <summary>
        ///     ��ǰ�����Ķ�ȡ�ı���
        /// </summary>
        protected virtual string ContextReadTable
        {
            get { return _dynamicReadTable ?? ReadTableName; }
        }

        /// <summary>
        ///     ������ѯ����
        /// </summary>
        protected string BaseCondition { get; set; }

        /// <summary>
        ///     ����
        /// </summary>
        protected abstract string WriteTableName { get; }

        /// <summary>
        ///     ���ʱ�������ֶ�
        /// </summary>
        protected abstract string PrimaryKey { get; }

        /// <summary>
        ///     �����ֶ�(�ɶ�̬����PrimaryKey)
        /// </summary>
        private string _keyField;

        /// <summary>
        ///     �����ֶ�(�ɶ�̬����PrimaryKey)
        /// </summary>
        public string KeyField
        {
            get { return _keyField ?? PrimaryKey; }
            set { _keyField = value; }
        }

        /// <summary>
        ///     ɾ����SQL���
        /// </summary>
        protected virtual string DeleteSqlCode
        {
            get { return $@"DELETE FROM [{WriteTableName}]"; }
        }

        /// <summary>
        ///     �����SQL���
        /// </summary>
        protected abstract string InsertSqlCode { get; }

        /// <summary>
        ///     ȫ�����µ�SQL���
        /// </summary>
        protected abstract string UpdateSqlCode { get; }

        /// <summary>
        ///     �����ֶ�
        /// </summary>
        public abstract string[] Fields { get; }

        /// <summary>
        ///     �ֶ��ֵ�
        /// </summary>
        private Dictionary<string, string> _fieldMap;

        /// <summary>
        ///     �ֶ��ֵ�(���ʱ)
        /// </summary>
        public virtual Dictionary<string, string> FieldMap
        {
            get { return _fieldMap ?? (_fieldMap = Fields.ToDictionary(p => p, p => p)); }
        }

        /// <summary>
        ///     �ֶ��ֵ�(����ʱ)
        /// </summary>
        public Dictionary<string, string> FieldDictionary
        {
            get { return OverrideFieldMap ?? FieldMap; }
        }

        /// <summary>
        ///     �ֶ��ֵ�(��̬����)
        /// </summary>
        public Dictionary<string, string> OverrideFieldMap { get; set; }

        #endregion

        #region ��

        #region ��������

        /// <summary>
        ///     ��������
        /// </summary>
        public void FeachAll(Action<TData> action, Action<List<TData>> end)
        {
            //Debug.WriteLine(typeof(TData).Name, "����");
            var list = All();
            //Debug.WriteLine(list.Count, "����");
            if (list.Count == 0)
            {
                return;
            }
            //DateTime s = DateTime.Now;
            list.ForEach(p => p.DoLaterPeriodByAllModified());
            list.ForEach(action);
            end(list);
            //Debug.WriteLine((DateTime.Now - s).TotalSeconds, "��ʱ");
            //Debug.WriteLine((DateTime.Now - s).TotalSeconds / list.Count, "��ʱ");
        }

        #endregion

        #region ��ѯ�������(����lambda����)

        /// <summary>
        ///     �õ�����ȷƴ�ӵ�SQL������䣨������û�У�
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected string ContitionSqlCode(string condition)
        {
            if (string.IsNullOrWhiteSpace(BaseCondition) && string.IsNullOrWhiteSpace(condition))
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.Append("WHERE ");
            if (!string.IsNullOrWhiteSpace(BaseCondition))
            {
                sb.AppendLine($"({BaseCondition})");
                if (!string.IsNullOrWhiteSpace(condition))
                {
                    sb.Append(" AND ");
                }
            }
            if (!string.IsNullOrWhiteSpace(condition))
            {
                sb.AppendLine($"({condition})");
            }
            return sb.ToString();
        }

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
            var expression = (MemberExpression)action.Body;
            return expression.Member.Name;
        }
        #endregion

        #region ����

        /// <summary>
        ///     ��������
        /// </summary>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public TData First()
        {
            return LoadFirst();
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public TData FirstOrDefault()
        {
            return LoadFirst();
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public TData First(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);

            return LoadFirst(convert.ConditionSql, convert.Parameters);
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
        ///     �Ƿ��������
        /// </summary>
        /// <returns>�Ƿ��������</returns>
        public TData First(string condition, SqlParameter[] args)
        {
            return LoadFirst(condition, args);
        }


        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public TData FirstOrDefault(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);

            return LoadFirst(convert.ConditionSql, convert.Parameters);
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
        ///     ��������
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <param name="args">����</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public TData FirstOrDefault(string condition, SqlParameter[] args)
        {
            return LoadFirst(condition, args);
        }

        #endregion

        #region β��

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public TData Last()
        {
            return LoadLast();
        }

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public TData LastOrDefault()
        {
            return LoadLast();
        }

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public TData Last(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);

            return LoadLast(convert.ConditionSql, convert.Parameters);
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
        ///     �Ƿ��������
        /// </summary>
        /// <returns>�Ƿ��������</returns>
        public TData Last(string condition, SqlParameter[] args)
        {
            return LoadLast(condition, args);
        }


        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public TData LastOrDefault(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);

            return LoadLast(convert.ConditionSql, convert.Parameters);
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
        ///     ����β��
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <param name="args">����</param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public TData LastOrDefault(string condition, SqlParameter[] args)
        {
            return LoadLast(condition, args);
        }

        #endregion

        #region Select

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <returns>�Ƿ��������</returns>
        public List<TData> Select()
        {
            return LoadDataInner();
        }

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>����</returns>
        public List<TData> Select(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return LoadDataInner(convert.ConditionSql, convert.Parameters);
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

        #endregion

        #region All

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <returns>����</returns>
        public List<TData> All()
        {
            return LoadDataInner();
        }


        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <returns>����</returns>
        public List<TData> All(string condition, SqlParameter[] args)
        {
            return LoadDataInner(condition, args);
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
        ///     ��ȡ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <param name="orderBys">����</param>
        /// <returns>����</returns>
        public List<TData> All(LambdaItem<TData> lambda, params string[] orderBys)
        {
            var convert = Compile(lambda);
            return LoadDataInner(convert.ConditionSql, convert.Parameters, orderBys.Length == 0 ? null : string.Join(",", orderBys));
        }

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <param name="orderBys">����</param>
        /// <returns>����</returns>
        public List<TData> All(Expression<Func<TData, bool>> lambda, params string[] orderBys)
        {
            var convert = Compile(lambda);
            return LoadDataInner(convert.ConditionSql, convert.Parameters, orderBys.Length == 0 ? null : string.Join(",", orderBys));
        }

        #endregion

        #region Where

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ��������</returns>
        public List<TData> Where(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return LoadDataInner(convert.ConditionSql, convert.Parameters);
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

        #endregion


        #region �ۺϺ���֧��


        #region Collect

        /// <summary>
        ///     ���ܷ���
        /// </summary>
        public object Collect(string fun, string field, string condition, params SqlParameter[] args)
        {
            return CollectInner(fun, field, condition, args);
        }


        /// <summary>
        ///     ���ܷ���
        /// </summary>
        public object Collect(string fun, string field, Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return CollectInner(fun, field, convert.ConditionSql, convert.Parameters);
        }
        #endregion

        #region Exist

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        public bool Exist()
        {
            return Count() > 0;
        }

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        public bool Exist(string condition, params SqlParameter[] args)
        {
            return Count(condition, args) > 0;
        }

        /// <summary>
        ///     ���������Ƿ����
        /// </summary>
        public bool ExistPrimaryKey(object key)
        {
            return ExistInner(PrimaryKeyConditionSQL, CreatePimaryKeyParameter(key));
        }
        #endregion

        #region Count

        /// <summary>
        ///     ����
        /// </summary>
        public long Count()
        {
            return CountInner();
        }

        /// <summary>
        ///     ����
        /// </summary>
        public long Count(string condition, params SqlParameter[] args)
        {
            var obj = CollectInner("Count", "*", condition, args);
            return obj == DBNull.Value ? 0L : Convert.ToInt64(obj);
        }

        #endregion

        #region SUM

        /// <summary>
        ///     ����
        /// </summary>
        public decimal Sum(string field)
        {
            var obj = CollectInner("Sum", field, null, null);
            return obj == DBNull.Value ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     ����
        /// </summary>
        public decimal Sum(string field, string condition, params SqlParameter[] args)
        {
            var obj = CollectInner("Sum", field, condition, args);
            return obj == DBNull.Value ? 0M : Convert.ToDecimal(obj);
        }

        #endregion


        #region Any

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <returns>�Ƿ��������</returns>
        public bool Any()
        {
            return ExistInner();
        }

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <returns>�Ƿ��������</returns>
        public bool Any(string condition, SqlParameter[] args)
        {
            return ExistInner(condition, args);
        }

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ��������</returns>
        public bool Any(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return ExistInner(convert.ConditionSql, convert.Parameters);
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

        #endregion

        #region Count

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ��������</returns>
        public long Count(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            var obj = CollectInner("Count", "*", convert.ConditionSql, convert.Parameters);
            return obj == DBNull.Value ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ��������</returns>
        public long Count(LambdaItem<TData> lambda)
        {
            var convert = Compile(lambda);
            var obj = CollectInner("Count", "*", convert.ConditionSql, convert.Parameters);
            return obj == DBNull.Value ? 0 : Convert.ToInt64(obj);
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
            return obj == DBNull.Value ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     ����
        /// </summary>
        public long Count(string condition, params DbParameter[] args)
        {
            var obj = CollectInner("Count", "*", condition, args.Cast<SqlParameter>().ToArray());
            return obj == DBNull.Value || obj == null ? 0L : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">��ѯ���ʽ</param>
        /// <param name="args"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public long Count<TValue>(Expression<Func<TData, TValue>> field, string condition, params SqlParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = CollectInner("Count", expression.Member.Name, condition, args);
            return obj == DBNull.Value ? 0 : Convert.ToInt64(obj);
        }

        #endregion

        #region Sum

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <param name="condition2">����2��Ĭ��Ϊ��</param>
        public decimal Sum<TValue>(Expression<Func<TData, TValue>> field, Expression<Func<TData, bool>> lambda, string condition2 = null)
        {
            var expression = (MemberExpression)field.Body;
            var convert = Compile(lambda);
            var condition = condition2 == null
                ? convert.ConditionSql
                : convert.ConditionSql == null
                    ? condition2
                    : $"({convert.ConditionSql}) AND ({condition2})";
            var obj = CollectInner("Sum", expression.Member.Name, condition, convert.Parameters);
            return obj == DBNull.Value ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public decimal Sum<TValue>(Expression<Func<TData, TValue>> field, Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var expression = (MemberExpression)field.Body;
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            var obj = CollectInner("Sum", expression.Member.Name,
                $"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
            return obj == DBNull.Value ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">��ѯ���ʽ</param>
        /// <param name="args"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public decimal Sum<TValue>(Expression<Func<TData, TValue>> field, string condition, params SqlParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = CollectInner("Sum", expression.Member.Name, condition, args);
            return obj == DBNull.Value ? 0M : Convert.ToDecimal(obj);
        }

        #endregion

        #region ʵ��

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        protected bool ExistInner(string condition = null, SqlParameter args = null)
        {
            return ExistInner(condition, args == null ? new SqlParameter[0] : new[] { args });
        }

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        protected bool ExistInner(string condition, SqlParameter[] args)
        {
            var obj = CollectInner("Count", "*", condition, args);
            return obj != DBNull.Value && Convert.ToInt64(obj) > 0;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        protected long CountInner(string condition = null, SqlParameter args = null)
        {
            var obj = CollectInner("Count", "*", condition, args);
            return obj == DBNull.Value ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        protected object CollectInner(string fun, string field, string condition, params SqlParameter[] args)
        {
            var sql = $@"SELECT {fun}({field}) FROM {ContextReadTable}{ContitionSqlCode(condition)};";
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                return DataBase.ExecuteScalar(sql, args);
            }
        }
        #endregion


        #endregion

        #region ��ҳ��ȡ

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> LoadData(int page, int limit, string order, bool desc, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, order, desc, condition, args.Cast<SqlParameter>().ToArray());
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> PageData(int page, int limit, string order, bool desc, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, order, desc, condition, args.Cast<SqlParameter>().ToArray());
        }
        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> PageData(int page, int limit)
        {
            return PageData(page, limit, KeyField, false, null, null);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> PageData(int page, int limit, string condition, params SqlParameter[] args)
        {
            return PageData(page, limit, KeyField, false, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> PageData(int page, int limit, Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return PageData(page, limit, KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> PageData(int page, int limit, LambdaItem<TData> lambda)
        {
            var convert = Compile(lambda);
            return PageData(page, limit, KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> LoadData(int page, int limit, string order, string condition, params SqlParameter[] args)
        {
            return PageData(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> PageData(int page, int limit, string order, string condition, params SqlParameter[] args)
        {
            return PageData(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> PageData<TField>(int page, int limit, Expression<Func<TData, TField>> field, Expression<Func<TData, bool>> lambda)
        {
            return PageData(page, limit, field, false, lambda);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> PageData<TField>(int page, int limit, Expression<Func<TData, TField>> field, bool desc, Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return PageData(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        /// <param name="page">ҳ��(��1��ʼ)</param>
        /// <param name="limit">ÿҳ����</param>
        /// <param name="order">�����ֶ�</param>
        /// <param name="desc">�Ƿ���</param>
        /// <param name="condition">��ѯ����</param>
        /// <param name="args">��ѯ����</param>
        /// <returns></returns>
        public List<TData> PageData(int page, int limit, string order, bool desc, string condition, params SqlParameter[] args)
        {
            var results = new List<TData>();
#if DEBUG
            var orderField = string.IsNullOrWhiteSpace(order) || !FieldDictionary.ContainsKey(order) ? KeyField : FieldDictionary[order];
#else
            var orderField = order == null ? this.KeyField : this.FieldDictionary[order];
#endif
            var sql = $@"SELECT * FROM (
    SELECT {ContextLoadFields},
           ROW_NUMBER() OVER (ORDER BY [{orderField}] {(desc ? "DESC" : "ASC")}) AS __rs
      FROM {ContextReadTable}{ContitionSqlCode(condition)}
) t WHERE __rs > {(page - 1) * limit} AND __rs <= {page * limit};";

            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var cmd = DataBase.CreateCommand(sql, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(LoadEntity(reader));
                        }
                    }
                }
                for (var index = 0; index < results.Count; index++)
                {
                    results[index] = EntityLoaded(results[index]);
                }
            }
            return results;
        }

        #endregion
        #region ���ж�ȡ

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="lambda">����</param>
        /// <returns>��������</returns>
        public TField LoadValue<TField>(Expression<Func<TData, TField>> field, Expression<Func<TData, bool>> lambda)
        {
            var fn = GetPropertyName(field);
            var convert = Compile(lambda);
            var val = LoadValueInner(fn, convert.ConditionSql, convert.Parameters);
            return val == null || val == DBNull.Value ? default(TField) : (TField)val;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="key">����</param>
        /// <returns>��������</returns>
        public TField LoadValue<TField, TKey>(Expression<Func<TData, TField>> field, TKey key)
        {
            var fn = GetPropertyName(field);
            var condition = string.Format(@"[{0}]=@{0}", KeyField);
            var args = new[]
            {
                CreateFieldParameter(KeyField, key)
            };
            var vl = LoadValueInner(fn, condition, args);
            return vl == DBNull.Value ? default(TField) : (TField)vl;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="lambda">����</param>
        /// <returns>��������</returns>
        public List<TField> LoadValues<TField>(Expression<Func<TData, TField>> fieldExpression, Expression<Func<TData, bool>> lambda)
        {
            var field = GetPropertyName(fieldExpression);
            var convert = Compile(lambda);
            var result = LoadValuesInner(field, convert.ConditionSql, convert.Parameters);
            return result.Count == 0 ? new List<TField>() : result.Select(p => (TField)p).ToList();
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="parse">ת���������ͷ���</param>
        /// <param name="lambda">����</param>
        /// <returns>��������</returns>
        public List<TField> LoadValues<TField>(Expression<Func<TData, TField>> fieldExpression, Func<object, TField> parse, Expression<Func<TData, bool>> lambda)
        {
            var field = GetPropertyName(fieldExpression);
            var convert = Compile(lambda);
            var result = LoadValuesInner(field, convert.ConditionSql, convert.Parameters);
            return result.Count == 0 ? new List<TField>() : result.Select(parse).ToList();
        }

        /// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        public object LoadValue(string field, string condition, params SqlParameter[] args)
        {
            return LoadValueInner(field, condition, args);
        }
        /// <summary>
        ///     ��ȡֵ
        /// </summary>
        protected object LoadValueInner(string field, string condition, params SqlParameter[] args)
        {
            var sql = $@"SELECT [{field}] FROM {ContextReadTable}{ContitionSqlCode(condition)};";

            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                return DataBase.ExecuteScalar(sql, args);
            }
        }

        /// <summary>
        ///     ��ȡ���ֵ(SQL�еĵ�һ���ֶ�)
        /// </summary>
        protected List<object> LoadValuesInner(string field, string condition, params SqlParameter[] args)
        {
            var sql = $@"SELECT [{field}] FROM {ContextReadTable}{ContitionSqlCode(condition)};";
            var values = new List<object>();
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var cmd = DataBase.CreateCommand(sql, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            values.Add(reader.GetValue(0));
                        }
                    }
                }
            }
            return values;
        }
        #endregion
        #region ���ݶ�ȡ

        /// <summary>
        ///     ȫ���ȡ
        /// </summary>
        public List<TData> LoadData()
        {
            return LoadDataInner();
        }


        /// <summary>
        ///     ������ȡ
        /// </summary>
        public List<TData> LoadData(string condition, params SqlParameter[] args)
        {
            return LoadDataInner(condition, args);
        }

        /// <summary>
        ///     ������ȡ
        /// </summary>
        public virtual TData LoadByPrimaryKey(object key)
        {
            return LoadFirstInner(PrimaryKeyConditionSQL, CreatePimaryKeyParameter(key));
        }

        /// <summary>
        ///     ������ȡ
        /// </summary>
        public List<TData> LoadByPrimaryKeies(IEnumerable<object> keies)
        {
            var list = new List<TData>();
            var par = CreatePimaryKeyParameter();
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                foreach (var key in keies)
                {
                    par.Value = key;
                    list.Add(LoadFirstInner(PrimaryKeyConditionSQL, par));
                }
            }
            return list;
        }



        /// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        public TData LoadFirst(string condition = null)
        {
            return LoadFirstInner(condition);
        }

        /// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        public TData LoadFirst(string condition, params SqlParameter[] args)
        {
            return LoadFirstInner(condition, args);
        }

        /// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        public TData LoadFirst(string foreignKey, object key)
        {
            return LoadFirstInner(FieldConditionSQL(foreignKey), CreateFieldParameter(foreignKey, key));
        }

        /// <summary>
        ///     ������ڵĻ���ȡβ��
        /// </summary>
        public TData LoadLast(string condition = null)
        {
            return LoadLastInner(condition);
        }

        /// <summary>
        ///     ������ڵĻ���ȡβ��
        /// </summary>
        public TData LoadLast(string condition, params SqlParameter[] args)
        {
            return LoadLastInner(condition, args);
        }

        /// <summary>
        ///     ������ڵĻ���ȡβ��
        /// </summary>
        public TData LoadLast(string foreignKey, object key)
        {
            return LoadLastInner(FieldConditionSQL(foreignKey), CreateFieldParameter(foreignKey, key));
        }

        /// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        public List<TData> LoadByForeignKey(string foreignKey, object key)
        {
            return LoadDataInner(FieldConditionSQL(foreignKey), CreateFieldParameter(foreignKey, key));
        }

        /// <summary>
        ///     ���¶�ȡ
        /// </summary>
        public void ReLoad(TData entity)
        {
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                ReLoadInner(entity);
            }
            entity.OnStatusChanged(NotificationStatusType.Refresh);
        }

        /// <summary>
        ///     ���¶�ȡ
        /// </summary>
        public void ReLoad(ref TData entity)
        {
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                ReLoadInner(entity);
            }
            entity.OnStatusChanged(NotificationStatusType.Refresh);
        }

        #endregion

        #region �����¼�

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="entity">��ȡ���ݵ�ʵ��</param>
        private TData EntityLoaded(TData entity)
        {
            entity = OnEntityLoaded(entity);
            OnLoadAction?.Invoke(entity);
            return entity;
        }

        /// <summary>
        ///     ������ͬ������
        /// </summary>
        /// <param name="entity"></param>
        protected virtual TData OnEntityLoaded(TData entity)
        {
            return entity;
        }

        /// <summary>
        ///     ��������ʱ���ⲿ�Ĵ�����
        /// </summary>
        public Action<TData> OnLoadAction;

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="reader">���ݶ�ȡ��</param>
        /// <returns>��ȡ���ݵ�ʵ��</returns>
        private TData LoadEntity(SqlDataReader reader)
        {
            var entity = new TData();
            using (new EntityLoadScope(entity))
            {
                LoadEntity(reader, entity);
            }
            return entity;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        private void ReLoadInner(TData entity)
        {
            entity.RejectChanged();
            using (var cmd = CreateLoadCommand(PrimaryKeyConditionSQL, CreatePimaryKeyParameter(entity)))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return;
                    }
                     using (new EntityLoadScope(entity))
                    {
                        LoadEntity(reader, entity);
                    }
                }
            }
            var entity2 = EntityLoaded(entity);
            if (entity != entity2)
            {
                entity.CopyValue(entity2);
            }
        }


        /// <summary>
        ///     ��ȡ����
        /// </summary>
        protected TData LoadFirstInner(string condition = null, SqlParameter args = null)
        {
            return LoadFirstInner(condition, args == null ? new SqlParameter[0] : new[] { args });
        }

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        protected TData LoadFirstInner(string condition, SqlParameter[] args)
        {
            TData entity = null;
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var cmd = CreateLoadCommand(condition, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            entity = LoadEntity(reader);
                        }
                    }
                }
                if (entity != null)
                {
                    entity = EntityLoaded(entity);
                }
            }
            return entity;
        }


        /// <summary>
        ///     ��ȡβ��
        /// </summary>
        protected TData LoadLastInner(string condition = null, SqlParameter args = null)
        {
            return LoadLastInner(condition, args == null ? new SqlParameter[0] : new[] { args });
        }

        /// <summary>
        ///     ��ȡβ��
        /// </summary>
        protected TData LoadLastInner(string condition, SqlParameter[] args)
        {
            TData entity = null;
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var cmd = CreateLoadCommand(KeyField, true, condition, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            entity = LoadEntity(reader);
                        }
                    }
                }
                if (entity != null)
                {
                    entity = EntityLoaded(entity);
                }
            }
            return entity;
        }

        /// <summary>
        ///     ��ȡȫ��
        /// </summary>
        protected List<TData> LoadDataInner(string condition = null, SqlParameter args = null)
        {
            return LoadDataInner(condition, args == null ? new SqlParameter[0] : new[] { args }, null);
        }

        /// <summary>
        ///     ��ȡȫ��
        /// </summary>
        protected List<TData> LoadDataInner(string condition, SqlParameter[] args)
        {
            return LoadDataInner(condition, args, null);
        }

        /// <summary>
        ///     ��ȡȫ��
        /// </summary>
        protected List<TData> LoadDataInner(string condition, SqlParameter[] args, string orderBy)
        {
            var results = new List<TData>();
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var cmd = CreateLoadCommand(condition, orderBy, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(LoadEntity(reader));
                        }
                    }
                }
                for (var index = 0; index < results.Count; index++)
                {
                    results[index] = EntityLoaded(results[index]);
                }
            }
            return results;
        }

        /// <summary>
        ///     ��ȡȫ��(SQL�������д��,�ֶ����ƺ�˳�������ʱ��ͬ)
        /// </summary>
        protected List<TData> LoadDataBySql(string sql, SqlParameter[] args)
        {
            var results = new List<TData>();
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var cmd = DataBase.CreateCommand(sql, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(LoadEntity(reader));
                        }
                    }
                }
                for (var index = 0; index < results.Count; index++)
                {
                    results[index] = EntityLoaded(results[index]);
                }
            }
            return results;
        }


        #endregion

        #endregion

        #region д

        #region �ڲ�д���

        /// <summary>
        ///     ����
        /// </summary>
        private void SaveInner(TData entity)
        {
            if (entity.__EntityStatus.IsDelete)
            {
                DeleteInner(entity);
            }
            else if (entity.__EntityStatus.IsNew || !ExistPrimaryKey(entity.GetValue(KeyField)))
            {
                InsertInner(entity);
            }
            else
            {
                UpdateInner(entity);
            }
        }

        /// <summary>
        ///     ����ֵ
        /// </summary>
        protected void SaveValueInner(string field, object value, string[] conditionFiles, object[] values)
        {
            var args = CreateFieldsParameters(conditionFiles, values);
            var condition = FieldConditionSQL(true, conditionFiles);
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                if (Exist(condition, args))
                {
                    var sql = string.Format(@"UPDATE [{0}] SET [{1}] = @{1}_n WHERE {2};{3}"
                        , WriteTableName
                        , field
                        , condition
                        , AfterUpdateSql(condition));
                    using (var cmd = DataBase.CreateCommand(sql, args))
                    {
                        cmd.Parameters.Add(new SqlParameter(field + "_n", GetDbType(field))
                        {
                            Value = value
                        });
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    var entity = new TData();

                    for (var idx = 0; idx < values.Length; idx++)
                    {
                        entity.SetValue(conditionFiles[idx], values[idx]);
                    }
                    entity.SetValue(field, value);
                    InsertInner(entity);
                }
            }
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="entity">�������ݵ�ʵ��</param>
        protected bool InsertInner(TData entity)
        {
            PrepareSave(entity, EntitySubsist.Adding);
            using (var cmd = DataBase.CreateCommand())
            {
                bool isIdentitySql = SetInsertCommand(entity, cmd);
                foreach (var handler in _updateHandlers.Where(p => p.AfterInner))
                {
                    handler.PrepareExecSql(this, cmd, entity, EntitySubsist.Adding);
                }
                if (isIdentitySql)
                {
                    SqlServerDataBase.TraceSql(cmd);
                    var key = cmd.ExecuteScalar();
                    if (key == DBNull.Value)
                    {
                        return false;
                    }
                    entity.SetValue(KeyField, key);
                }
                else
                {
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        return false;
                    }
                }
            }
            EndSaved(entity, EntitySubsist.Added);
            return true;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="entity">�������ݵ�ʵ��</param>
        private void UpdateInner(TData entity)
        {
            //if (!entity.__EntityStatus.IsModified)
            //    return;
            PrepareSave(entity, EntitySubsist.Modify);
            using (var cmd = DataBase.CreateCommand())
            {
                SetUpdateCommand(entity, cmd);
                SqlServerDataBase.TraceSql(cmd);

                foreach (var handler in _updateHandlers.Where(p => p.AfterInner))
                {
                    handler.PrepareExecSql(this, cmd, entity, EntitySubsist.Modify);
                }
                cmd.ExecuteNonQuery();
            }
            EndSaved(entity, EntitySubsist.Modified);
        }

        /// <summary>
        ///     ɾ��
        /// </summary>
        private int DeleteInner(TData entity)
        {
            if (entity == null)
            {
                return 0;
            }
            entity.__EntityStatus.IsDelete = true;
            PrepareSave(entity, EntitySubsist.Deleting);
            DeleteInner(PrimaryKeyConditionSQL, CreatePimaryKeyParameter(entity));
            EndSaved(entity, EntitySubsist.Deleted);
            return 1;
        }

        /// <summary>
        ///     ɾ��
        /// </summary>
        // ReSharper disable once UnusedMethodReturnValue.Local
        private int DeleteInner(string condition = null, SqlParameter args = null)
        {
            return DeleteInner(condition, args == null ? new SqlParameter[0] : new[] { args });
        }

        /// <summary>
        ///     ɾ��
        /// </summary>
        private int DeleteInner(string condition, SqlParameter[] args)
        {
            if (string.IsNullOrEmpty(DeleteSqlCode))
            {
                return 0;
            }
            if (!string.IsNullOrEmpty(condition))
            {
                return DataBase.Execute($@"{DeleteSqlCode} WHERE {condition};{AfterUpdateSql(condition)}", args);
            }
            DataBase.Clear(WriteTableName);
            return int.MaxValue;
        }

        #endregion

        #region �¼�

        /// <summary>
        ///     ����ǰ����(Insert��Update)
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="subsist">��ǰʵ������״̬</param>
        /// <remarks>
        ///     �Ե�ǰ��������Եĸ���,�����б���,���򽫶�ʧ
        /// </remarks>
        protected void PrepareSave(TData entity, EntitySubsist subsist)
        {
            foreach (var handler in _updateHandlers.Where(p => !p.AfterInner))
            {
                handler.PrepareSave(this, entity, subsist);
            }
            if (!IsBaseClass)
            {
                entity.LaterPeriodByModify(subsist);
            }
            OnPrepareSave(entity, subsist);
            foreach (var handler in _updateHandlers.Where(p => p.AfterInner))
            {
                handler.PrepareSave(this, entity, subsist);
            }
        }

        /// <summary>
        ///     ����ǰ����(Insert��Update)
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="subsist">��ǰʵ������״̬</param>
        protected virtual void OnPrepareSave(TData entity, EntitySubsist subsist)
        {
        }

        /// <summary>
        ///     ������ɺ��ڴ���(Insert��Update)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="subsist">��ǰʵ������״̬</param>
        private void EndSaved(TData entity, EntitySubsist subsist)
        {
            foreach (var handler in _updateHandlers.Where(p => !p.AfterInner))
            {
                handler.EndSaved(this, entity, subsist);
            }
            if (!IsBaseClass)
            {
                switch (subsist)
                {
                    case EntitySubsist.Added:
                        entity.OnStatusChanged(NotificationStatusType.Added);
                        break;
                    case EntitySubsist.Deleted:
                        entity.OnStatusChanged(NotificationStatusType.Deleted);
                        break;
                    case EntitySubsist.Modified:
                        entity.OnStatusChanged(NotificationStatusType.Modified);
                        break;
                }
                entity.LaterPeriodByModify(subsist);
                entity.AcceptChanged();
            }
            OnDataSaved(entity);
            foreach (var handler in _updateHandlers.Where(p => p.AfterInner))
            {
                handler.EndSaved(this, entity, subsist);
            }
        }

        /// <summary>
        ///     ������ɺ��ڴ���(Insert��Update)
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void OnDataSaved(TData entity)
        {
        }

        #endregion

        #region ���ݲ���

        /// <summary>
        ///     ��������
        /// </summary>
        public void Save(IEnumerable<TData> entities)
        {
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    foreach (var entity in entities)
                    {
                        SaveInner(entity);
                    }
                    scope.SetState(true);
                }
            }
        }

        /// <summary>
        ///     ��������
        /// </summary>
        public void Save(TData entity)
        {
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    SaveInner(entity);
                    scope.SetState(true);
                }
            }
        }

        /// <summary>
        ///     ��������
        /// </summary>
        public void Update(TData entity)
        {
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    UpdateInner(entity);
                    scope.SetState(true);
                }
                ReLoadInner(entity);
            }
        }

        /// <summary>
        ///     ��������
        /// </summary>
        public void Update(IEnumerable<TData> entities)
        {
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    foreach (var entity in entities)
                    {
                        UpdateInner(entity);
                    }
                    scope.SetState(true);
                }
            }
        }

        /// <summary>
        ///     ����������
        /// </summary>
        public bool Add(TData entity)
        {
            return Insert(entity);
        }

        /// <summary>
        ///     ����������
        /// </summary>
        public bool Insert(TData entity)
        {
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    if (!InsertInner(entity))
                    {
                        return false;
                    }
                    ReLoadInner(entity);
                    scope.SetState(true);
                }
            }
            return true;
        }

        /// <summary>
        ///     ����������
        /// </summary>
        public void Insert(IEnumerable<TData> entities)
        {
            var datas = entities as TData[] ?? entities.ToArray();
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    foreach (var entity in datas)
                    {
                        InsertInner(entity);
                    }
                    scope.SetState(true);
                }
                foreach (var entity in datas)
                {
                    ReLoadInner(entity);
                }
            }
        }

        /// <summary>
        ///     ɾ������
        /// </summary>
        public void Delete(IEnumerable<TData> entities)
        {
            var datas = entities as TData[] ?? entities.ToArray();
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    foreach (var entity in datas)
                    {
                        DeleteInner(entity);
                    }
                    scope.SetState(true);
                }
            }
        }

        /// <summary>
        ///     ɾ������
        /// </summary>
        public int Delete(TData entity)
        {
            int cnt;
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    cnt = DeleteInner(entity);
                    scope.SetState(true);
                }
            }
            return cnt;
        }

        /// <summary>
        ///     ɾ������
        /// </summary>
        public int DeletePrimaryKey(object key)
        {
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                return Delete(LoadByPrimaryKey(key));
            }
        }

        /// <summary>
        ///     �����������
        /// </summary>
        public void Clear()
        {
            //throw new Exception("����ɾ�����ܱ�����");
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                DataBase.Clear(WriteTableName);
            }
        }

        /// <summary>
        ///     ����ɾ��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public int Delete(Expression<Func<TData, bool>> lambda)
        {
            //throw new Exception("����ɾ�����ܱ�����");
            var convert = Compile(lambda);
            return Delete(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ǿ������ɾ��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public int Destroy(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            int cnt;
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    cnt = DataBase.Execute(
                        $@"DELETE FROM [{WriteTableName}] WHERE {convert.ConditionSql};{
                            AfterUpdateSql(convert.ConditionSql)
                        }"
                        , convert.Parameters);

                    scope.SetState(true);
                }
            }
            return cnt;
        }

        /// <summary>
        ///     ����ɾ��
        /// </summary>
        public int Delete(string condition, params SqlParameter[] args)
        {
            //throw new Exception("����ɾ�����ܱ�����");
            if (string.IsNullOrWhiteSpace(condition))
            {
                throw new ArgumentException(@"ɾ����������Ϊ��,��Ϊ������ִ��ȫ��ɾ��", "condition");
            }
            int cnt;
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    cnt = DeleteInner(condition, args);
                    scope.SetState(true);
                }
            }
            return cnt;
        }

        #endregion

        #region ��������

        /// <summary>
        ///     ��������
        /// </summary>
        public void SaveValue(string field, object value, string[] conditionFiles, object[] values)
        {
            SaveValueInner(field, value, conditionFiles, values);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        public int SetValue(string field, object value, string condition, params SqlParameter[] args)
        {
            if (!FieldDictionary.ContainsKey(field))
            {
                throw new ArgumentException(@"�����ֶβ���Ϊ�ջ򲻴���", "field");
            }
            field = FieldDictionary[field];
            string sql = string.IsNullOrWhiteSpace(condition)
                ? string.Format(@"UPDATE [{0}] SET [{1}]=@{1};{2}", WriteTableName, field, AfterUpdateSql(null))
                : string.Format(@"UPDATE [{0}] SET [{1}]=@{1} WHERE {2};{3}", WriteTableName, field, condition, AfterUpdateSql(null));
            var arg2 = new List<SqlParameter>();
            if (args != null)
            {
                arg2.AddRange(args);
            }
            arg2.Add(CreateFieldParameter(field, value));
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    var result = DataBase.Execute(sql, arg2.ToArray());
                    scope.SetState(true);
                    return result;
                }
            }
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value, string condition, params SqlParameter[] args)
        {
            return SetValue(GetPropertyName(field), value, condition, args);
        }

        /// <summary>
        ///     ȫ������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <returns>��������</returns>
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value)
        {
            return SetValue(GetPropertyName(field), value, null, null);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="lambda">����</param>
        /// <returns>��������</returns>
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value, Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return SetValue(GetPropertyName(field), value, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="key">����</param>
        /// <returns>��������</returns>
        public int SetValue<TKey>(string field, object value, TKey key)
        {
            if (!FieldDictionary.ContainsKey(field))
            {
                throw new ArgumentException(@"�����ֶβ���Ϊ�ջ򲻴���", "field");
            }
            field = FieldDictionary[field];
            string condition = string.Format("[{0}]=@{0}", KeyField);
            var sql = string.Format(@"UPDATE [{0}] SET [{1}]=@{1} WHERE {2};{3}", WriteTableName, field, condition, AfterUpdateSql(condition));
            var arg2 = new List<SqlParameter>
            {
                CreateFieldParameter(KeyField, key),
                CreateFieldParameter(field, value)
            };
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    var result = DataBase.Execute(sql, arg2.ToArray());
                    scope.SetState(true);
                    return result;
                }
            }
        }

        #endregion

        #region �򵥸���

        /// <summary>
        ///     �����ͬʱִ�е�SQL
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected virtual string AfterUpdateSql(string condition)
        {
            return null;
        }

        /// <summary>
        ///     �Զ�����£����±��ʽ��д��
        /// </summary>
        /// <param name="expression">����SQL���ʽ</param>
        /// <param name="condition">����</param>
        /// <param name="args">����</param>
        /// <returns>��������</returns>
        public int SetValue(string expression, Expression<Func<TData, bool>> condition, params SqlParameter[] args)
        {
            var convert = Compile(condition);
            var sql = $@"UPDATE [{WriteTableName}] SET {expression} WHERE {convert.ConditionSql};{
                    AfterUpdateSql(convert.ConditionSql)
                }";
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    if (args.Length == 0)
                    {
                        return DataBase.Execute(sql, convert.Parameters);
                    }
                    if (convert.Parameters.Length == 0)
                    {
                        return DataBase.Execute(sql, args);
                    }
                    var arg2 = convert.Parameters.ToList();
                    arg2.AddRange(args);
                    var result = DataBase.Execute(sql, arg2.ToArray());
                    scope.SetState(true);
                    return result;
                }
            }
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="key">����</param>
        /// <returns>��������</returns>
        public int SetValue<TField, TKey>(Expression<Func<TData, TField>> fieldExpression, TField value, TKey key)
        {
            var field = GetPropertyName(fieldExpression);
            string condition = string.Format("[{0}]=@{0}", KeyField);
            var sql = string.Format(@"UPDATE [{0}] SET [{1}]=@{1} WHERE {2};{3}"
                , WriteTableName
                , field
                , condition
                , AfterUpdateSql(condition));
            var arg2 = new List<SqlParameter>
            {
                CreateFieldParameter(KeyField, key),
                CreateFieldParameter(field, value)
            };
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    var result = DataBase.Execute(sql, arg2.ToArray());
                    scope.SetState(true);
                    return result;
                }
            }
        }

        /// <summary>
        ///     ����ֶΰ��Զ�����ʽ����ֵ
        /// </summary>
        /// <param name="valueExpression">ֵ��SQL��ʽ</param>
        /// <param name="key">����</param>
        /// <returns>��������</returns>
        public int SetCoustomValue<TKey>(string valueExpression, TKey key)
        {
            string condition = string.Format("[{0}]=@{0}", KeyField);
            var sql = $@"UPDATE [{WriteTableName}] SET {valueExpression} WHERE {condition};{AfterUpdateSql(condition)}";
            var arg2 = new List<SqlParameter>
            {
                CreateFieldParameter(KeyField, key)
            };
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    var result = DataBase.Execute(sql, arg2.ToArray());
                    scope.SetState(true);
                    return result;
                }
            }
        }

        #endregion

        #region ���ݲ����¼�

        /// <summary>
        ///     ����ע�봦����
        /// </summary>
        private readonly List<IDataUpdateHandler<TData>> _updateHandlers = new List<IDataUpdateHandler<TData>>();

        /// <summary>
        ///     ע�����ݸ���ע����
        /// </summary>
        public void RegisterUpdateHandler(IDataUpdateHandler<TData> handler)
        {
            if (_updateHandlers.Contains(handler))
            {
                _updateHandlers.Add(handler);
            }
        }

        /// <summary>
        ///     ��ע�����ݸ���ע����
        /// </summary>
        public void UnRegisterUpdateHandler(IDataUpdateHandler<TData> handler)
        {
            _updateHandlers.Remove(handler);
        }

        #endregion

        #endregion

        #region ���鷽��

        /// <summary>
        ///     ���ø������ݵ�����
        /// </summary>
        protected abstract void SetUpdateCommand(TData entity, SqlCommand cmd);

        /// <summary>
        ///     ���ò������ݵ�����
        /// </summary>
        /// <returns>������˵��Ҫȡ����</returns>
        protected abstract bool SetInsertCommand(TData entity, SqlCommand cmd);

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="reader">���ݶ�ȡ��</param>
        /// <param name="entity">��ȡ���ݵ�ʵ��</param>
        protected abstract void LoadEntity(SqlDataReader reader, TData entity);

        #endregion

        #region �����������

        /// <summary>
        ///     ��������
        /// </summary>
        protected SqlCommand CreateLoadCommand(string condition, params SqlParameter[] args)
        {
            return CreateLoadCommand(condition, null, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        protected SqlCommand CreateLoadCommand(string condition, string orderby, params SqlParameter[] args)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"SELECT");
            sql.AppendLine(ContextLoadFields);
            sql.AppendFormat(@"FROM {0}", ContextReadTable);
            sql.AppendLine(ContitionSqlCode(condition));
            if (!string.IsNullOrWhiteSpace(orderby))
            {
                sql.AppendLine();
                sql.Append("ORDER BY ");
                sql.Append(orderby);
            }
            sql.Append(";");
            return DataBase.CreateCommand(sql.ToString(), args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        protected SqlCommand CreateLoadCommand(string orderby, bool desc, string condition, params SqlParameter[] args)
        {
            var orderSql =
                $"[{FieldDictionary[!string.IsNullOrEmpty(orderby) ? orderby : KeyField]}] {(desc ? "DESC" : "")}";
            return CreateLoadCommand(condition, orderSql, args);
        }

        #endregion

        #region �ֶεĲ�������

        /// <summary>
        ///     �õ��ֶε�SqlDbType����
        /// </summary>
        /// <param name="field">�ֶ�����</param>
        /// <returns>����</returns>
        protected virtual SqlDbType GetDbType(string field)
        {
            return SqlDbType.NVarChar;
        }


        private string _primaryConditionSQL;

        /// <summary>
        ///     ��������������SQL
        /// </summary>
        public string PrimaryKeyConditionSQL
        {
            get { return _primaryConditionSQL ?? (_primaryConditionSQL = FieldConditionSQL(KeyField)); }
        }

        IDataBase IDataTable<TData>.DataBase => DataBase;


        Dictionary<string, string> IDataTable<TData>.FieldDictionary => FieldDictionary;


        int IDataTable<TData>.TableId => TableId;
        
        /// <summary>
        ///     ���ɶ���ֶεĲ���
        /// </summary>
        /// <param name="fields">���ɲ������ֶ�</param>
        public SqlParameter[] CreateFieldsParameters(params string[] fields)
        {
            if (fields == null || fields.Length == 0)
            {
                throw new ArgumentException(@"û���ֶ��������ɲ���", "fields");
            }
            return fields.Select(field => new SqlParameter(field, GetDbType(field))).ToArray();
        }

        /// <summary>
        ///     ���ɶ���ֶεĲ���
        /// </summary>
        /// <param name="fields">���ɲ������ֶ�</param>
        /// <param name="values">���ɲ�����ֵ(���Ⱥ��ֶγ��ȱ���һ��)</param>
        public SqlParameter[] CreateFieldsParameters(string[] fields, object[] values)
        {
            if (fields == null || fields.Length == 0)
            {
                throw new ArgumentException(@"û���ֶ��������ɲ���", "fields");
            }
            if (values == null || values.Length == 0)
            {
                throw new ArgumentException(@"û��ֵ�������ɲ���", "values");
            }
            if (values.Length != fields.Length)
            {
                throw new ArgumentException(@"ֵ�ĳ��Ⱥ��ֶγ��ȱ���һ��", "values");
            }
            var res = fields.Select(field => new SqlParameter(field, GetDbType(field))).ToArray();
            for (var i = 0; i < fields.Length; i++)
            {
                res[i].Value = values[i];
            }
            return res;
        }

        /// <summary>
        ///     �����ֶεĲ���
        /// </summary>
        /// <param name="field">���ɲ������ֶ�</param>
        public SqlParameter CreateFieldParameter(string field)
        {
            return new SqlParameter(field, GetDbType(field));
        }

        /// <summary>
        ///     �����ֶεĲ���
        /// </summary>
        /// <param name="field">���ɲ������ֶ�</param>
        /// <param name="value">ֵ</param>
        public SqlParameter CreateFieldParameter(string field, object value)
        {
            if (value is Enum)
            {
                return new SqlParameter(field, SqlDbType.Int)
                {
                    Value = Convert.ToInt32(value)
                };
            }
            return new SqlParameter(field, GetDbType(field))
            {
                Value = value ?? DBNull.Value
            };
        }

        /// <summary>
        ///     �����ֶεĲ���
        /// </summary>
        /// <param name="field">���ɲ������ֶ�</param>
        /// <param name="entity">ȡֵ��ʵ��</param>
        public SqlParameter CreateFieldParameter(string field, TData entity)
        {
            return CreateFieldParameter(field, entity.GetValue(field));
        }

        /// <summary>
        ///     �����ֶεĲ���
        /// </summary>
        /// <param name="field">���ɲ������ֶ�</param>
        /// <param name="entity">ȡֵ��ʵ��</param>
        /// <param name="entityField">ȡֵ���ֶ�</param>
        public SqlParameter CreateFieldParameter(string field, DataObjectBase entity, string entityField)
        {
            return CreateFieldParameter(field, entity.GetValue(entityField));
        }

        /// <summary>
        ///     ���������ֶεĲ���
        /// </summary>
        public SqlParameter CreatePimaryKeyParameter()
        {
            return new SqlParameter(KeyField, GetDbType(KeyField));
        }

        /// <summary>
        ///     ���������ֶεĲ���
        /// </summary>
        /// <param name="value">����ֵ</param>
        public SqlParameter CreatePimaryKeyParameter(object value)
        {
            return new SqlParameter(KeyField, GetDbType(KeyField))
            {
                Value = value
            };
        }

        /// <summary>
        ///     ���������ֶεĲ���
        /// </summary>
        /// <param name="entity">ȡֵ��ʵ��</param>
        public SqlParameter CreatePimaryKeyParameter(TData entity)
        {
            return new SqlParameter("@"+KeyField, GetDbType(KeyField))
            {
                Value = entity.GetValue(KeyField)
            };
        }

        /// <summary>
        ///     ��������������SQL
        /// </summary>
        public string FieldConditionSQL(string field)
        {
            return $@"[{FieldDictionary[field]}] = @{field}";
        }

        /// <summary>
        ///     ��������SQL
        /// </summary>
        /// <param name="isAnd">�Ƿ���AND���</param>
        /// <param name="conditions">����</param>
        public string JoinConditionSQL(bool isAnd, params string[] conditions)
        {
            if (conditions == null || conditions.Length == 0)
            {
                throw new ArgumentException(@"û�������������", "conditions");
            }
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", conditions[0]);
            for (var idx = 1; idx < conditions.Length; idx++)
            {
                sql.AppendFormat(@" {0} ({1}) ", isAnd ? "AND" : "OR", conditions[idx]);
            }
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
            {
                throw new ArgumentException(@"û���ֶ����������������", "fields");
            }
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", FieldConditionSQL(fields[0]));
            for (var idx = 1; idx < fields.Length; idx++)
            {
                sql.AppendFormat(@" {0} ({1}) ", isAnd ? "AND" : "OR", FieldConditionSQL(fields[idx]));
            }
            return sql.ToString();
        }

        /// <summary>
        ///     �����ֶ�����
        /// </summary>
        /// <param name="isAnd">�Ƿ���AND���</param>
        /// <param name="fields">���ɲ������ֶ�</param>
        /// <returns>ConditionItem</returns>
        public ConditionItem CreateConditionItem(bool isAnd, params string[] fields)
        {
            if (fields == null || fields.Length == 0)
            {
                throw new ArgumentException(@"û���ֶ����������������", "fields");
            }
            return new ConditionItem
            {
                ConditionSql = FieldConditionSQL(isAnd, fields),
                Parameters = CreateFieldsParameters(fields)
            };
        }

        /// <summary>
        ///     �����ֶ�����
        /// </summary>
        /// <param name="isAnd">�Ƿ���AND���</param>
        /// <param name="fields">���ɲ������ֶ�</param>
        /// <param name="values">���ɲ�����ֵ(���Ⱥ��ֶγ��ȱ���һ��)</param>
        /// <returns>ConditionItem</returns>
        public ConditionItem CreateConditionItem(bool isAnd, string[] fields, object[] values)
        {
            if (fields == null || fields.Length == 0)
            {
                throw new ArgumentException(@"û���ֶ����������������", "fields");
            }
            return new ConditionItem
            {
                ConditionSql = FieldConditionSQL(isAnd, fields),
                Parameters = CreateFieldsParameters(fields, values)
            };
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
            {
                return false;
            }
            var fieldName = GetPropertyName(field);
            var convert = Compile(condition);
            convert.AddAndCondition($"([{FieldDictionary[fieldName]}] = @c_vl_)", new SqlParameter
            {
                ParameterName = "c_vl_",
                SqlDbType = GetDbType(fieldName),
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
            {
                return false;
            }
            var fieldName = GetPropertyName(field);
            return Exist($"([{FieldDictionary[fieldName]}] = @c_vl_ AND {PrimaryKeyConditionSQL}"
                , new SqlParameter
                {
                    ParameterName = "c_vl_",
                    SqlDbType = GetDbType(fieldName),
                    Value = val
                }
                , CreatePimaryKeyParameter(key));
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
            {
                return false;
            }
            var fieldName = GetPropertyName(field);
            return Exist($"([{FieldDictionary[fieldName]}] = @c_vl_)", new SqlParameter
            {
                ParameterName = "c_vl_",
                SqlDbType = GetDbType(fieldName),
                Value = val
            });
        }

        public TData FirstOrDefault(object id)
        {
            return LoadByPrimaryKey(id);
        }

        public TData First(object id)
        {
            return LoadByPrimaryKey(id);
        }
        
        
        public List<TData> All<TField>(Expression<Func<TData, bool>> lambda, Expression<Func<TData, TField>> orderBy, bool desc)
        {
            var convert = Compile(lambda);
            return PageData(0, -1, GetPropertyName(orderBy), desc, convert.ConditionSql, convert.Parameters);
        }
        
        public bool ExistPrimaryKey<T>(T id)
        {
            return ExistInner(PrimaryKeyConditionSQL, CreatePimaryKeyParameter(id));
        }
        
        public List<TField> LoadValues<TField>(Expression<Func<TData, TField>> fieldExpression, string condition)
        {
            var field = GetPropertyName(fieldExpression);

            var result = LoadValuesInner(field, condition);
            return result.Count == 0 ? new List<TField>() : result.Select(p => (TField)p).ToList();
        }

        public TData LoadData(object id)
        {
            return LoadByPrimaryKey(id);
        }

        public List<TData> LoadByPrimaryKeies(IEnumerable keies)
        {
            var list = new List<TData>();
            var par = CreatePimaryKeyParameter();
            using (SqlServerDataBaseScope.CreateScope(DataBase))
            {
                foreach (var key in keies)
                {
                    par.Value = key;
                    list.Add(LoadFirstInner(PrimaryKeyConditionSQL, par));
                }
            }
            return list;
        }

        public int Delete(object id)
        {
           return Delete(PrimaryKeyConditionSQL, CreatePimaryKeyParameter(id));
        }
        
        public int SetValue(string field, object value, object key)
        {
            return SetValue(field, value, PrimaryKeyConditionSQL, CreatePimaryKeyParameter(key));
        }
        
        #endregion
    }
}