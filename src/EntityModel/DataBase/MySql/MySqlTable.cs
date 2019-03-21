// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Agebull.Common.Logging;
using MySql.Data.MySqlClient;

#endregion

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    ///     Sqlʵ�������
    /// </summary>
    /// <typeparam name="TData">ʵ��</typeparam>
    public abstract partial class MySqlTable<TData>: IDataTable<TData>
        where TData : EditDataObject, new()
    {
        #region ���ݿ�

        /// <summary>
        ///     �Զ��������Ӷ���
        /// </summary>
        private MySqlDataBase _dataBase;

        /// <summary>
        ///     �Զ��������Ӷ���
        /// </summary>
        public MySqlDataBase DataBase
        {
            get => _dataBase ?? (_dataBase = MySqlDataBase.DefaultDataBase ??
                                             (_dataBase = MySqlDataBase.DefaultDataBase = CreateDefaultDataBase()));
            set => _dataBase = value;
        }

        /// <summary>
        ///     �Զ��������Ӷ���
        /// </summary>
        IDataBase IDataTable<TData>.DataBase => DataBase;

        /// <summary>
        ///     ���ʱ�������ֶ�
        /// </summary>
        string IDataTable<TData>.PrimaryKey => PrimaryKey;

        #endregion

        #region ���ݽṹ

        /// <summary>
        ///     �Ƿ���Ϊ������ڵ�
        /// </summary>
        public bool IsBaseClass { get; set; }

        /// <summary>
        ///     ����
        /// </summary>
        public string TableName => ReadTableName;

        /// <summary>
        ///     �ֶ��ֵ�(����ʱ)
        /// </summary>
        public Dictionary<string, string> FieldDictionary => OverrideFieldMap ?? FieldMap;

        /// <summary>
        ///     �����ֶ�(�ɶ�̬����PrimaryKey)
        /// </summary>
        private string _keyField;

        /// <summary>
        ///     �����ֶ�(�ɶ�̬����PrimaryKey)
        /// </summary>
        public string KeyField
        {
            get
            {
                if (_keyField != null)
                    return _keyField;
                return _keyField = PrimaryKey;
            }
            set => _keyField = value;
        }

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
                return;
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
            var expression = action.Body as MemberExpression;
            if (expression != null)
                return expression.Member.Name;
            var body = action.Body as UnaryExpression;
            if (body == null)
                throw new Exception("���ʽ̫����");

            expression = (MemberExpression)body.Operand;
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
        /// <param name="id">����</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public TData FirstOrDefault(object id)
        {
            return LoadByPrimaryKey(id);
        }


        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="id">����</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public TData First(object id)
        {
            return LoadByPrimaryKey(id);
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
        public TData First(string condition, MySqlParameter[] args)
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
        public TData FirstOrDefault(string condition, MySqlParameter[] args)
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
        public TData Last(string condition, MySqlParameter[] args)
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
        public TData LastOrDefault(string condition, MySqlParameter[] args)
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
        public List<TData> All(string condition, MySqlParameter[] args)
        {
            return LoadDataInner(condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> All<TField>(Expression<Func<TData, bool>> lambda, Expression<Func<TData, TField>> orderBy,
            bool desc)
        {
            var convert = Compile(lambda);
            return LoadPageData(1, -1, GetPropertyName(orderBy), desc, convert.ConditionSql, convert.Parameters);
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
            return LoadDataInner(convert.ConditionSql, convert.Parameters,
                orderBys.Length == 0 ? null : string.Join(",", orderBys));
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
            return LoadDataInner(convert.ConditionSql, convert.Parameters,
                orderBys.Length == 0 ? null : string.Join(",", orderBys));
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
        public object Collect(string fun, string field, string condition, params MySqlParameter[] args)
        {
            return CollectInner(fun, FieldMap[field], condition, args);
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
        public bool Exist(string condition, params MySqlParameter[] args)
        {
            return Count(condition, args) > 0;
        }

        /// <summary>
        ///     �Ƿ���ڴ�����������
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>�Ƿ��������</returns>
        public bool ExistPrimaryKey<T>(T id)
        {
            return ExistInner(PrimaryKeyConditionSQL, CreatePimaryKeyParameter(id));
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
        public long Count(string condition, params DbParameter[] args)
        {
            var obj = CollectInner("Count", "*", condition, args.Cast< MySqlParameter>().ToArray());
            return obj == DBNull.Value || obj == null ? 0L : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     ����
        /// </summary>
        public long Count(string condition, params MySqlParameter[] args)
        {
            var obj = CollectInner("Count", "*", condition, args);
            return obj == DBNull.Value || obj == null ? 0L : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ��������</returns>
        public long Count(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            var obj = CollectInner("Count", "*", convert.ConditionSql, convert.Parameters);
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
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
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
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
        ///     ����
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">��ѯ���ʽ</param>
        /// <param name="args"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public long Count<TValue>(Expression<Func<TData, TValue>> field, string condition, params MySqlParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = CollectInner("Count", FieldMap[expression.Member.Name], condition, args);
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }
        #endregion

        #region SUM

        /// <summary>
        ///     ����
        /// </summary>
        public decimal Sum(string field)
        {
            var obj = CollectInner("Sum", FieldMap[field], null, null);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     ����
        /// </summary>
        public decimal Sum(string field, string condition, params MySqlParameter[] args)
        {
            var obj = CollectInner("Sum", FieldMap[field], condition, args);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
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
        public bool Any(string condition, MySqlParameter[] args)
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


        #endregion

        #region Sum

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

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">��ѯ���ʽ</param>
        /// <param name="args"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public decimal Sum<TValue>(Expression<Func<TData, TValue>> field, string condition,
            params MySqlParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = CollectInner("Sum", FieldMap[expression.Member.Name], condition, args);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }

        #endregion

        #region ʵ��

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        protected bool ExistInner(string condition = null, MySqlParameter args = null)
        {
            return ExistInner(condition, args == null ? new MySqlParameter[0] : new[] { args });
        }

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        protected bool ExistInner(string condition, MySqlParameter[] args)
        {
            var obj = CollectInner("Count", "*", condition, args);
            return obj != DBNull.Value && obj != null && Convert.ToInt64(obj) > 0;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        protected long CountInner(string condition = null, MySqlParameter args = null)
        {
            var obj = CollectInner("Count", "*", condition, args);
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        protected object CollectInner(string fun, string field, string condition, params MySqlParameter[] args)
        {
            var sql = CreateCollectSql(fun, field, condition);
            using (MySqlDataBaseScope.CreateScope(DataBase))
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
        public List<TData> PageData(int page, int limit)
        {
            return PageData(page, limit, KeyField, false, null, null);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> PageData(int page, int limit, string condition, params MySqlParameter[] args)
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
        public List<TData> LoadData(int page, int limit, string order, bool desc, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, order, desc, condition, args.Cast<MySqlParameter>().ToArray());
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> PageData(int page, int limit, string order, bool desc, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, order, desc, condition, args.Cast<MySqlParameter>().ToArray());
        }
        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> LoadData(int page, int limit, string order, string condition, params MySqlParameter[] args)
        {
            return PageData(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> PageData(int page, int limit, string order, string condition, params MySqlParameter[] args)
        {
            return PageData(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> PageData<TField>(int page, int limit, Expression<Func<TData, TField>> field,
            Expression<Func<TData, bool>> lambda)
        {
            return PageData(page, limit, field, false, lambda);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> PageData<TField>(int page, int limit, Expression<Func<TData, TField>> field, bool desc,
            Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return PageData(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public List<TData> PageData<TField>(int page, int limit, Expression<Func<TData, TField>> field, bool desc,
            LambdaItem<TData> lambda)
        {
            var convert = Compile(lambda);
            return PageData(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        /// <param name="page">ҳ��(��1��ʼ)</param>
        /// <param name="limit">ÿҳ����(С�ڲ���ҳ��</param>
        /// <param name="order">�����ֶ�</param>
        /// <param name="desc">�Ƿ���</param>
        /// <param name="condition">��ѯ����</param>
        /// <param name="args">��ѯ����</param>
        /// <returns></returns>
        public List<TData> PageData(int page, int limit, string order, bool desc, string condition,
            params MySqlParameter[] args)
        {
            if (page <= 0)
                page = 1;
            if (limit == 0)
                limit = 20;
            else if (limit == 9999)
                limit = -1;
            else if (limit > 500)
                limit = 500;
            return LoadPageData(page, limit, order, desc, condition, args);
        }

        private List<TData> LoadPageData(int page, int limit, string order, bool desc, string condition,
            MySqlParameter[] args)
        {
            var results = new List<TData>();
            var sql = CreatePageSql(page, limit, order, desc, condition);
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var cmd = DataBase.CreateCommand(sql, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            results.Add(LoadEntity(reader));
                    }
                }
                for (var index = 0; index < results.Count; index++)
                    results[index] = EntityLoaded(results[index]);
            }
            return results;
        }

        #endregion

        #region ���ж�ȡ
        
        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="lambda">����</param>
        /// <returns>����</returns>
        public TField LoadValue<TField>(Expression<Func<TData, TField>> field, Expression<Func<TData, bool>> lambda)
        {
            var fn = GetPropertyName(field);
            var convert = Compile(lambda);
            var val = LoadValueInner(fn, convert.ConditionSql, convert.Parameters);
            return val == null || val == DBNull.Value ? default(TField) : (TField)val;
        }

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="key">����</param>
        /// <returns>����</returns>
        public object Read<TField, TKey>(Expression<Func<TData, TField>> field, TKey key)
        {
            var fn = GetPropertyName(field);
            var vl = LoadValueInner(fn, FieldConditionSQL(KeyField), CreateFieldParameter(KeyField, key));
            return vl == DBNull.Value || vl == null ? default(TField) : vl;
        }

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="key">����</param>
        /// <returns>����</returns>
        public TField LoadValue<TField, TKey>(Expression<Func<TData, TField>> field, TKey key)
        {
            var fn = GetPropertyName(field);
            var vl = LoadValueInner(fn, FieldConditionSQL(KeyField), CreateFieldParameter(KeyField, key));
            return vl == DBNull.Value || vl == null ? default(TField) : (TField)vl;
        }
        
        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="lambda">����</param>
        /// <returns>����</returns>
        public List<TField> LoadValues<TField>(Expression<Func<TData, TField>> fieldExpression,
            Expression<Func<TData, bool>> lambda)
        {
            var field = GetPropertyName(fieldExpression);
            var convert = Compile(lambda);
            Debug.Assert(FieldDictionary.ContainsKey(field));
            var sql = CreateLoadValuesSql(field, convert);
            var values = new List<TField>();
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var cmd = DataBase.CreateCommand(sql, convert.Parameters))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var vl = reader.GetValue(0);
                            if (vl != DBNull.Value && vl != null)
                                values.Add((TField)vl);
                        }
                    }
                }
            }
            return values;
        }

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="condition">����</param>
        /// <returns>����</returns>
        public List<TField> LoadValues<TField>(Expression<Func<TData, TField>> fieldExpression, string condition)
        {
            var field = GetPropertyName(fieldExpression);

            var result = LoadValuesInner(field, condition);
            return result.Count == 0 ? new List<TField>() : result.Select(p => (TField)p).ToList();
        }

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="parse">ת���������ͷ���</param>
        /// <param name="lambda">����</param>
        /// <returns>����</returns>
        public List<TField> LoadValues<TField>(Expression<Func<TData, TField>> fieldExpression,
            Func<object, TField> parse, Expression<Func<TData, bool>> lambda)
        {
            var field = GetPropertyName(fieldExpression);
            var convert = Compile(lambda);
            var result = LoadValuesInner(field, convert.ConditionSql, convert.Parameters);
            return result.Count == 0 ? new List<TField>() : result.Select(parse).ToList();
        }

        /// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        public object LoadValue(string field, string condition, params MySqlParameter[] args)
        {
            return LoadValueInner(field, condition, args);
        }

        /// <summary>
        ///     ��ȡֵ
        /// </summary>
        protected object LoadValueInner(string field, string condition, params MySqlParameter[] args)
        {
            var sql = CreateLoadValueSql(field, condition);

            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                return DataBase.ExecuteScalar(sql, args);
            }
        }


        /// <summary>
        ///     ��ȡ���ֵ
        /// </summary>
        protected List<object> LoadValuesInner(string field, string condition, params MySqlParameter[] args)
        {
            var sql = CreateLoadValueSql(field, condition);
            var values = new List<object>();
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var cmd = DataBase.CreateCommand(sql, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var vl = reader.GetValue(0);
                            if (vl != DBNull.Value && vl != null)
                                values.Add(vl);
                        }
                    }
                }
            }
            return values;
        }

        #endregion

        #region ���ݶ�ȡ

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="id">����</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public TData LoadData(object id)
        {
            return LoadByPrimaryKey(id);
        }


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
        public List<TData> LoadData(string condition, params MySqlParameter[] args)
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
        public List<TData> LoadByPrimaryKeies(IEnumerable keies)
        {
            var list = new List<TData>();
            var par = CreatePimaryKeyParameter();
            using (MySqlDataBaseScope.CreateScope(DataBase))
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
        public TData LoadFirst(string condition, params MySqlParameter[] args)
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
        public TData LoadLast(string condition, params MySqlParameter[] args)
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
            using (MySqlDataBaseScope.CreateScope(DataBase))
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
            using (MySqlDataBaseScope.CreateScope(DataBase))
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
        private TData LoadEntity(MySqlDataReader reader)
        {
            var entity = new TData();
            using (new EntityLoadScope(entity))
            {
                if (ContentLoadAction != null)
                    ContentLoadAction(reader, entity);
                else
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
                        return;
                    using (new EntityLoadScope(entity))
                    {
                        if (ContentLoadAction != null)
                            ContentLoadAction(reader, entity);
                        else
                            LoadEntity(reader, entity);
                    }
                }
            }
            var entity2 = EntityLoaded(entity);
            if (entity != entity2)
                entity.CopyValue(entity2);
        }


        /// <summary>
        ///     ��ȡ����
        /// </summary>
        protected TData LoadFirstInner(string condition = null, MySqlParameter args = null)
        {
            return LoadFirstInner(condition, args == null ? new MySqlParameter[0] : new[] { args });
        }

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        protected TData LoadFirstInner(string condition, MySqlParameter[] args)
        {
            TData entity = null;
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var cmd = CreateLoadCommand(condition, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            entity = LoadEntity(reader);
                    }
                }
                if (entity != null)
                    entity = EntityLoaded(entity);
            }
            return entity;
        }


        /// <summary>
        ///     ��ȡβ��
        /// </summary>
        protected TData LoadLastInner(string condition = null, MySqlParameter args = null)
        {
            return LoadLastInner(condition, args == null ? new MySqlParameter[0] : new[] { args });
        }

        /// <summary>
        ///     ��ȡβ��
        /// </summary>
        protected TData LoadLastInner(string condition, MySqlParameter[] args)
        {
            TData entity = null;
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var cmd = CreateLoadCommand(KeyField, true, condition, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            entity = LoadEntity(reader);
                    }
                }
                if (entity != null)
                    entity = EntityLoaded(entity);
            }
            return entity;
        }

        /// <summary>
        ///     ��ȡȫ��
        /// </summary>
        protected List<TData> LoadDataInner(string condition = null, MySqlParameter args = null)
        {
            return LoadDataInner(condition, args == null ? new MySqlParameter[0] : new[] { args }, null);
        }

        /// <summary>
        ///     ��ȡȫ��
        /// </summary>
        protected List<TData> LoadDataInner(string condition, MySqlParameter[] args)
        {
            return LoadDataInner(condition, args, null);
        }

        /// <summary>
        ///     ��ȡȫ��
        /// </summary>
        protected List<TData> LoadDataInner(string condition, MySqlParameter[] args, string orderBy)
        {
            var results = new List<TData>();
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var cmd = CreateLoadCommand(condition, orderBy, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            results.Add(LoadEntity(reader));
                    }
                }
                for (var index = 0; index < results.Count; index++)
                    results[index] = EntityLoaded(results[index]);
            }
            return results;
        }

        /// <summary>
        ///     ��ȡȫ��(SQL�������д��,�ֶ����ƺ�˳�������ʱ��ͬ)
        /// </summary>
        protected List<TData> LoadDataBySql(string sql, MySqlParameter[] args)
        {
            var results = new List<TData>();
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var cmd = DataBase.CreateCommand(sql, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            results.Add(LoadEntity(reader));
                    }
                }
                for (var index = 0; index < results.Count; index++)
                    results[index] = EntityLoaded(results[index]);
            }
            return results;
        }

        /// <summary>
        ///     ��ȡ�洢����
        /// </summary>
        public List<TData> LoadDataByProcedure(string procedure, MySqlParameter[] args)
        {
            var results = new List<TData>();
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var cmd = DataBase.CreateCommand(procedure, args))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            results.Add(LoadEntity(reader));
                    }
                }
                for (var index = 0; index < results.Count; index++)
                    results[index] = EntityLoaded(results[index]);
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
                DeleteInner(entity);
            else if (entity.__EntityStatus.IsNew || !ExistPrimaryKey(entity.GetValue(KeyField)))
                InsertInner(entity);
            else
                UpdateInner(entity);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="entity">�������ݵ�ʵ��</param>
        protected bool InsertInner(TData entity)
        {
            PrepareSave(entity, DataOperatorType.Insert);
            using (var cmd = DataBase.CreateCommand())
            {
                var isIdentitySql = SetInsertCommand(entity, cmd);
                MySqlDataBase.TraceSql(cmd);
                if (isIdentitySql)
                {
                    var key = cmd.ExecuteScalar();
                    if (key == DBNull.Value || key == null)
                        return false;
                    entity.SetValue(KeyField, key);
                }
                else
                {
                    if (cmd.ExecuteNonQuery() == 0)
                        return false;
                }
            }
            EndSaved(entity, DataOperatorType.Insert);
            return true;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="entity">�������ݵ�ʵ��</param>
        private void UpdateInner(TData entity)
        {
            if (UpdateByMidified && !entity.__EntityStatus.IsModified)
                return;
            int result;
            PrepareSave(entity, DataOperatorType.Update);
            using (var cmd = DataBase.CreateCommand())
            {
                SetUpdateCommand(entity, cmd);
                MySqlDataBase.TraceSql(cmd);
                cmd.CommandText = $@"{BeforeUpdateSql(PrimaryKeyConditionSQL)}
{UpdateSqlCode}
{AfterUpdateSql(PrimaryKeyConditionSQL)}";

                result = cmd.ExecuteNonQuery();
            }
            if (result > 0)
                EndSaved(entity, DataOperatorType.Update);
        }

        /// <summary>
        ///     ɾ��
        /// </summary>
        private int DeleteInner(TData entity)
        {
            if (entity == null)
                return 0;
            entity.__EntityStatus.IsDelete = true;
            PrepareSave(entity, DataOperatorType.Delete);
            var result = DeleteInner(PrimaryKeyConditionSQL, CreatePimaryKeyParameter(entity));
            EndSaved(entity, DataOperatorType.Delete);
            return result;
        }

        /// <summary>
        ///     ɾ��
        /// </summary>
        private int DeleteInner(string condition, params MySqlParameter[] args)
        {
            if (string.IsNullOrEmpty(DeleteSqlCode))
                return 0;
            if (!string.IsNullOrEmpty(condition))
                return DataBase.Execute(CreateDeleteSql(condition), args);
            throw new ArgumentException(@"ɾ����������Ϊ��,��Ϊ������ִ��ȫ��ɾ��", GetType().FullName);
        }

        #endregion

        #region �¼�

        /// <summary>
        ///     ����ǰ����(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="operatorType">��������</param>
        protected void PrepareSave(TData entity, DataOperatorType operatorType)
        {
            if (!IsBaseClass)
                switch (operatorType)
                {
                    case DataOperatorType.Insert:
                        entity.LaterPeriodByModify(EntitySubsist.Adding);
                        break;
                    case DataOperatorType.Delete:
                        entity.LaterPeriodByModify(EntitySubsist.Deleting);
                        break;
                    case DataOperatorType.Update:
                        entity.LaterPeriodByModify(EntitySubsist.Modify);
                        break;
                }
            OnPrepareSave(entity, operatorType);
        }

        /// <summary>
        ///     ������ɺ��ڴ���(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="operatorType">��������</param>
        /// <remarks>
        ///     �Ե�ǰ��������Եĸ���,�����б���,���򽫶�ʧ
        /// </remarks>
        private void EndSaved(TData entity, DataOperatorType operatorType)
        {
            if (!IsBaseClass)
            {
                switch (operatorType)
                {
                    case DataOperatorType.Insert:
                        entity.OnStatusChanged(NotificationStatusType.Added);
                        break;
                    case DataOperatorType.Delete:
                        entity.OnStatusChanged(NotificationStatusType.Deleted);
                        break;
                    case DataOperatorType.Update:
                        entity.OnStatusChanged(NotificationStatusType.Modified);
                        break;
                }
                entity.AcceptChanged();
            }
            OnDataSaved(entity, operatorType);
        }

        #endregion

        #region ���ݲ���

        /// <summary>
        ///     ��������
        /// </summary>
        public void Save(IEnumerable<TData> entities)
        {
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    foreach (var entity in entities)
                        SaveInner(entity);
                    scope.SetState(true);
                }
            }
        }

        /// <summary>
        ///     ��������
        /// </summary>
        public void Save(TData entity)
        {
            using (MySqlDataBaseScope.CreateScope(DataBase))
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
            using (MySqlDataBaseScope.CreateScope(DataBase))
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
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    foreach (var entity in entities)
                        UpdateInner(entity);
                    scope.SetState(true);
                }
            }
        }

        /// <summary>
        ///     ����������
        /// </summary>
        public bool Insert(TData entity)
        {
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    if (!InsertInner(entity))
                        return false;
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
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    foreach (var entity in datas)
                        InsertInner(entity);
                    scope.SetState(true);
                }
                foreach (var entity in datas)
                    ReLoadInner(entity);
            }
        }

        /// <summary>
        ///     ɾ������
        /// </summary>
        public void Delete(IEnumerable<TData> entities)
        {
            var datas = entities as TData[] ?? entities.ToArray();
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    foreach (var entity in datas)
                        DeleteInner(entity);
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
            using (MySqlDataBaseScope.CreateScope(DataBase))
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
        public int Delete(object id)
        {
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                return Delete(LoadByPrimaryKey(id));
            }
        }

        /// <summary>
        ///     ����ɾ������
        /// </summary>
        public bool PhysicalDelete(object id)
        {
            var condition = PrimaryKeyConditionSQL;
            var para = CreatePimaryKeyParameter(id);
            OnOperatorExecuting(condition, new[] { para }, DataOperatorType.Delete);

            bool result;
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    result = DataBase.Execute($@"DELETE FROM `{WriteTableName}` 
WHERE {condition}", CreatePimaryKeyParameter(id)) == 1;
                    scope.SetState(result);
                }
            }
            if (result)
                OnOperatorExecutd(condition, new[] { para }, DataOperatorType.Delete);
            return result;
        }


        /// <summary>
        ///     ɾ������
        /// </summary>
        public int DeletePrimaryKey(object key)
        {
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                return Delete(LoadByPrimaryKey(key));
            }
        }

        /// <summary>
        ///     �����������
        /// </summary>
        public void Clear()
        {
            throw new Exception("����ɾ�����ܱ�����");
            //using (MySqlDataBaseScope.CreateScope(DataBase))
            //{
            //    DataBase.Clear(WriteTableName);
            //}
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
        /// <returns>�Ƿ�ɾ���ɹ�</returns>
        public int Destroy(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            int cnt;
            OnOperatorExecuting(convert.ConditionSql, convert.Parameters, DataOperatorType.Delete);
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    cnt = DataBase.Execute(CreateDeleteSql(convert), convert.Parameters);
                    scope.SetState(cnt > 0);
                }
            }
            if (cnt > 0)
                OnOperatorExecutd(convert.ConditionSql, convert.Parameters, DataOperatorType.Delete);
            return cnt;
        }

        /// <summary>
        ///     ����ɾ��
        /// </summary>
        public int Delete(string condition, params MySqlParameter[] args)
        {
            //throw new Exception("����ɾ�����ܱ�����");
            if (string.IsNullOrWhiteSpace(condition))
                throw new ArgumentException(@"ɾ����������Ϊ��,��Ϊ������ִ��ȫ��ɾ��", GetType().FullName);
            int cnt;
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    OnOperatorExecuting(condition, args, DataOperatorType.Delete);
                    cnt = DeleteInner(condition, args);
                    OnOperatorExecutd(condition, args, DataOperatorType.Delete);
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
            var args = CreateFieldsParameters(conditionFiles, values);
            var condition = FieldConditionSQL(true, conditionFiles);
            SetValueInner(field, value, condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="key">����</param>
        /// <returns>��������</returns>
        public int SetValue(string field, object value, object key)
        {
            return SetValueInner(field, value, $"`{PrimaryKey}`='{key}'", CreateFieldParameter(KeyField, key));
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        public int SetValue(string field, object value, string condition, params MySqlParameter[] args)
        {
            return SetValueInner(field, value, condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        public int SetValue(Expression<Func<TData, bool>> field, bool value, string condition,
            params MySqlParameter[] args)
        {
            return SetValueInner(GetPropertyName(field), value ? 0 : 1, condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        public int SetValue(Expression<Func<TData, Enum>> field, Enum value, string condition,
            params MySqlParameter[] args)
        {
            return SetValueInner(GetPropertyName(field), Convert.ToInt32(value), condition, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value, string condition,
            params MySqlParameter[] args)
        {
            return SetValueInner(GetPropertyName(field), value, condition, args);
        }

        /// <summary>
        ///     ȫ������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <returns>��������</returns>
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value)
        {
            return SetValueInner(GetPropertyName(field), value, null, null);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="lambda">����</param>
        /// <returns>��������</returns>
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value,
            Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return SetValueInner(GetPropertyName(field), value, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="condition">��������</param>
        /// <param name="args">��������</param>
        /// <returns>��������</returns>
        private int SetValueInner(string field, object value, string condition, params MySqlParameter[] args)
        {
            Debug.Assert(FieldDictionary.ContainsKey(field));
            field = FieldDictionary[field];

            var arg2 = new List<MySqlParameter>();
            if (args != null)
                arg2.AddRange(args);
            var sql = CreateUpdateSql(field, value, condition, arg2);

            int result;
            OnOperatorExecuting(condition, args, DataOperatorType.Update);
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    result = DataBase.Execute(sql, arg2.ToArray());
                    scope.SetState(result > 0);
                }
            }
            if (result > 0)
                OnOperatorExecutd(condition, args, DataOperatorType.Update);
            return result;
        }

        #endregion

        #region �򵥸���

        /// <summary>
        ///     �����ͬʱִ�е�SQL(����֮ǰ����ִ��)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected virtual string BeforeUpdateSql(string condition)
        {
            return null;
        }

        /// <summary>
        ///     �����ͬʱִ�е�SQL(����֮������ִ��)
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
        /// <returns>��������</returns>
        public int SetValue(string expression, Expression<Func<TData, bool>> condition)
        {
            var convert = Compile(condition);
            var sql = CreateUpdateSql(expression, convert);
            int result;
            OnOperatorExecuting(convert.ConditionSql, convert.Parameters, DataOperatorType.Update);
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    var arg2 = convert.Parameters.ToList();
                    result = DataBase.Execute(sql, arg2.ToArray());
                    scope.SetState(true);
                }
            }
            if (result > 0)
                OnOperatorExecutd(convert.ConditionSql, convert.Parameters, DataOperatorType.Update);
            return result;
        }

        /// <summary>
        ///     �Զ�����£����±��ʽ��д��
        /// </summary>
        /// <param name="expression">����SQL���ʽ</param>
        /// <param name="condition">����</param>
        /// <param name="args">����</param>
        /// <returns>��������</returns>
        public int SetValue(string expression, Expression<Func<TData, bool>> condition, params MySqlParameter[] args)
        {
            var convert = Compile(condition);
            var sql = CreateUpdateSql(expression, convert);

            int result;
            OnOperatorExecuting(convert.ConditionSql, convert.Parameters, DataOperatorType.Update);
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    if (args.Length == 0)
                    {
                        result = DataBase.Execute(sql, convert.Parameters);
                    }
                    else if (convert.Parameters.Length == 0)
                    {
                        result = DataBase.Execute(sql, args);
                    }
                    else
                    {
                        var arg2 = convert.Parameters.ToList();
                        arg2.AddRange(args);
                        result = DataBase.Execute(sql, arg2.ToArray());
                    }
                    scope.SetState(true);
                }
            }
            if (result > 0)
                OnOperatorExecutd(convert.ConditionSql, convert.Parameters, DataOperatorType.Update);
            return result;
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
            var parameters = new List<MySqlParameter>
            {
                CreateFieldParameter(KeyField, key)
            };
            var sql = CreateUpdateSql(GetPropertyName(fieldExpression), value, PrimaryKeyConditionSQL, parameters);
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    var result = DataBase.Execute(sql, parameters);
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
            var condition = PrimaryKeyConditionSQL;
            var sql = CreateUpdateSql(valueExpression, condition);
            var arg2 = new List<MySqlParameter>
            {
                CreateFieldParameter(KeyField, key)
            };
            int result;
            OnOperatorExecuting(condition, arg2, DataOperatorType.Update);
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                using (var scope = TransactionScope.CreateScope(DataBase))
                {
                    result = DataBase.Execute(sql, arg2);
                    scope.SetState(result == 1);
                }
            }
            OnOperatorExecutd(condition, arg2, DataOperatorType.Update);
            return result;
        }

        #endregion

        #region ������֧��

        /// <summary>
        ///     ����ǰ����
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="operatorType">��������</param>
        private void OnPrepareSave(TData entity, DataOperatorType operatorType)
        {
            OnPrepareSave(operatorType, entity);
            DataUpdateHandler.OnPrepareSave(entity, operatorType);
        }

        /// <summary>
        ///     ������ɺ��ڴ���
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="operatorType">��������</param>
        private void OnDataSaved(TData entity, DataOperatorType operatorType)
        {
            OnDataSaved(operatorType, entity);
            DataUpdateHandler.OnDataSaved(entity, operatorType);
        }

        /// <summary>
        ///     �������ǰ����(����ʵ�����������)
        /// </summary>
        /// <param name="condition">ִ������</param>
        /// <param name="args">����ֵ</param>
        /// <param name="operatorType">��������</param>
        private void OnOperatorExecuting(string condition, IEnumerable<MySqlParameter> args, DataOperatorType operatorType)
        {
            var sqlParameters = args as MySqlParameter[] ?? args.ToArray();
            OnOperatorExecuting(operatorType, condition, sqlParameters);
            DataUpdateHandler.OnOperatorExecuting(TableId, condition, sqlParameters, operatorType);
        }

        /// <summary>
        ///     ����������(����ʵ�����������)
        /// </summary>
        /// <param name="condition">ִ������</param>
        /// <param name="args">����ֵ</param>
        /// <param name="operatorType">��������</param>
        private void OnOperatorExecutd(string condition, IEnumerable<MySqlParameter> args, DataOperatorType operatorType)
        {
            var mySqlParameters = args as MySqlParameter[] ?? args.ToArray();
            OnOperatorExecutd(operatorType, condition, mySqlParameters);
            DataUpdateHandler.OnOperatorExecutd(TableId, condition, mySqlParameters, operatorType);
        }

        #endregion

        #endregion


        #region �����������

        /// <summary>
        ///     ��������
        /// </summary>
        protected MySqlCommand CreateLoadCommand(string condition, params MySqlParameter[] args)
        {
            return CreateLoadCommand(condition, null, args);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        protected MySqlCommand CreateLoadCommand(string condition, string order, params MySqlParameter[] args)
        {
            var sql = CreateLoadSql(condition, order);
            return DataBase.CreateCommand(sql.ToString(), args);
        }

        /// <summary>
        ///     ������������
        /// </summary>
        /// <param name="order">�����ֶ�</param>
        /// <param name="desc">�Ƿ���</param>
        /// <param name="condition">��������</param>
        /// <param name="args">�����еĲ���</param>
        /// <returns>��������</returns>
        protected MySqlCommand CreateLoadCommand(string order, bool desc, string condition,
            params MySqlParameter[] args)
        {
            var field = !string.IsNullOrEmpty(order) ? order : KeyField;
            Debug.Assert(FieldDictionary.ContainsKey(field));
            var orderSql = $"`{FieldDictionary[field]}` {(desc ? "DESC" : "")}";
            return CreateLoadCommand(condition, orderSql, args);
        }

        #endregion

        #region �ֶεĲ�������

        /// <summary>
        ///     �õ��ֶε�MySqlDbType����
        /// </summary>
        /// <param name="field">�ֶ�����</param>
        /// <returns>����</returns>
        protected virtual MySqlDbType GetDbType(string field)
        {
            return MySqlDbType.VarString;
        }


        private string _primaryConditionSQL;

        /// <summary>
        ///     ��������������SQL
        /// </summary>
        public string PrimaryKeyConditionSQL => _primaryConditionSQL ??
                                                (_primaryConditionSQL = FieldConditionSQL(PrimaryKey));

        /// <summary>
        ///     ���ɶ���ֶεĲ���
        /// </summary>
        /// <param name="fields">���ɲ������ֶ�</param>
        public MySqlParameter[] CreateFieldsParameters(params string[] fields)
        {
            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"û���ֶ��������ɲ���", nameof(fields));
            return fields.Select(field => new MySqlParameter(field, GetDbType(field))).ToArray();
        }

        /// <summary>
        ///     ���ɶ���ֶεĲ���
        /// </summary>
        /// <param name="fields">���ɲ������ֶ�</param>
        /// <param name="values">���ɲ�����ֵ(���Ⱥ��ֶγ��ȱ���һ��)</param>
        public MySqlParameter[] CreateFieldsParameters(string[] fields, object[] values)
        {
            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"û���ֶ��������ɲ���", nameof(fields));
            if (values == null || values.Length == 0)
                throw new ArgumentException(@"û��ֵ�������ɲ���", nameof(values));
            if (values.Length != fields.Length)
                throw new ArgumentException(@"ֵ�ĳ��Ⱥ��ֶγ��ȱ���һ��", nameof(values));
            var res = new MySqlParameter[fields.Length];
            for (var i = 0; i < fields.Length; i++)
                res[i] = CreateFieldParameter(fields[i], values[i]);
            return res;
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
        /// <param name="value">ֵ</param>
        public MySqlParameter CreateFieldParameter(string field, object value)
        {
            return MySqlDataBase.CreateParameter(field, value, GetDbType(field));
        }

        /// <summary>
        ///     �����ֶεĲ���
        /// </summary>
        /// <param name="field">���ɲ������ֶ�</param>
        /// <param name="entity">ȡֵ��ʵ��</param>
        public MySqlParameter CreateFieldParameter(string field, TData entity)
        {
            return CreateFieldParameter(field, entity.GetValue(field));
        }

        /// <summary>
        ///     �����ֶεĲ���
        /// </summary>
        /// <param name="field">���ɲ������ֶ�</param>
        /// <param name="entity">ȡֵ��ʵ��</param>
        /// <param name="entityField">ȡֵ���ֶ�</param>
        public MySqlParameter CreateFieldParameter(string field, DataObjectBase entity, string entityField)
        {
            return CreateFieldParameter(field, entity.GetValue(entityField));
        }

        /// <summary>
        ///     ���������ֶεĲ���
        /// </summary>
        public MySqlParameter CreatePimaryKeyParameter()
        {
            return new MySqlParameter(KeyField, GetDbType(KeyField));
        }

        /// <summary>
        ///     ���������ֶεĲ���
        /// </summary>
        /// <param name="value">����ֵ</param>
        public MySqlParameter CreatePimaryKeyParameter(object value)
        {
            return MySqlDataBase.CreateParameter(KeyField, value, GetDbType(KeyField));
        }

        /// <summary>
        ///     ���������ֶεĲ���
        /// </summary>
        /// <param name="entity">ȡֵ��ʵ��</param>
        public MySqlParameter CreatePimaryKeyParameter(TData entity)
        {
            return MySqlDataBase.CreateParameter(KeyField, entity.GetValue(KeyField),
                GetDbType(KeyField));
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
                throw new ArgumentException(@"û���ֶ����������������", nameof(fields));
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
                throw new ArgumentException(@"û���ֶ����������������", nameof(fields));
            return new ConditionItem
            {
                ConditionSql = FieldConditionSQL(isAnd, fields),
                Parameters = CreateFieldsParameters(fields, values)
            };
        }

        #endregion

        #region SQL

        #region ����

        /// <summary>
        ///     ���ɸ��µ�SQL���
        /// </summary>
        /// <param name="expression">�ֶθ������</param>
        /// <param name="convert">��������</param>
        /// <returns>���µ�SQL���</returns>
        private string CreateUpdateSql(string expression, ConditionItem convert)
        {
            return CreateUpdateSql(expression, convert.ConditionSql);
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
UPDATE `{WriteTableName}` 
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
        private string CreateUpdateSql(string field, object value, string condition, IList<MySqlParameter> parameters)
        {
            return CreateUpdateSql(FileUpdateSql(field, value, parameters), condition);
        }

        /// <summary>
        ///     ���ɵ����ֶθ��µ�SQL
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="parameters">�����б�</param>
        /// <returns>�����ֶθ��µ�SQL</returns>
        private string FileUpdateSql(string field, object value, IList<MySqlParameter> parameters)
        {
            field = FieldDictionary[field];
            if (value == null)
                return $"`{field}` = NULL";
            if (value is string || value is DateTime || value is byte[])
            {
                var name = "v_" + field;
                parameters.Add(CreateFieldParameter(name, value));
                return $"`{field}` = ?{name}";
            }
            if (value is bool)
                value = (bool)value ? 1 : 0;
            else if (value is Enum)
                value = Convert.ToInt32(value);
            return $"`{field}` = {value}";
        }

        #endregion

        #region ����

        /// <summary>
        /// ����������ʼ����ɵı�ʶ
        /// </summary>
        private bool BaseConditionInited = false;

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
        protected string ContitionSqlCode(string condition)
        {
            if (!BaseConditionInited)
            {
                InitBaseCondition();
                BaseConditionInited = true;
            }

            if (string.IsNullOrWhiteSpace(BaseCondition))
                return string.IsNullOrWhiteSpace(condition)
                    ? null
                    : $@"
WHERE {condition}";
            return string.IsNullOrWhiteSpace(condition)
                ? $@"
WHERE {BaseCondition}"
                : $@"
WHERE ({BaseCondition}) AND ({condition})";
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
                field = $"`{FieldMap[field]}`";
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
            return $@"SELECT `{FieldDictionary[field]}` FROM {ContextReadTable}{ContitionSqlCode(condition)};";
        }

        /// <summary>
        ///     ��������ֵ��SQL
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="convert">����</param>
        /// <returns>�����ֶ�ֵ��SQL���</returns>
        private string CreateLoadValuesSql(string field, ConditionItem convert)
        {
            return $@"SELECT `{FieldDictionary[field]}` 
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
        private string CreateDeleteSql(string condition)
        {
            return $@"{DeleteSqlCode} WHERE {condition};{AfterUpdateSql(condition)}";
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
            return $@"`{FieldDictionary[field]}` {expression} ?{field}";
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

        #endregion

        #region ����У��֧��

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="condition"></param>
        public bool IsUnique<TValue>(Expression<Func<TData, TValue>> field, object val,
            Expression<Func<TData, bool>> condition)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            var convert = Compile(condition);
            Debug.Assert(FieldDictionary.ContainsKey(fieldName));
            convert.AddAndCondition($"(`{FieldDictionary[fieldName]}` = ?c_vl_)", new MySqlParameter
            {
                ParameterName = "c_vl_",
                MySqlDbType = GetDbType(fieldName),
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
            return Exist($"(`{FieldDictionary[fieldName]}` = ?c_vl_ AND {PrimaryKeyConditionSQL}"
                , new MySqlParameter
                {
                    ParameterName = "c_vl_",
                    MySqlDbType = GetDbType(fieldName),
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
                return false;
            var fieldName = GetPropertyName(field);
            Debug.Assert(FieldDictionary.ContainsKey(fieldName));
            return Exist($"(`{FieldDictionary[fieldName]}` = ?c_vl_)"
                , new MySqlParameter
                {
                    ParameterName = "c_vl_",
                    MySqlDbType = GetDbType(fieldName),
                    Value = val
                });
        }

        #endregion
    }
}