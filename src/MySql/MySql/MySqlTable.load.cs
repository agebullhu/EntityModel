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
using MySql.Data.MySqlClient;
using System.Linq;
using System.Linq.Expressions;
using Agebull.EntityModel.Events;

using Agebull.EntityModel.Common;
using Agebull.MicroZero.ZeroApis;

#endregion

namespace Agebull.EntityModel.MySql
{
    partial class MySqlTable<TData, TMySqlDataBase>
    {
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
            if (action.Body is MemberExpression expression)
                return expression.Member.Name;
            if (!(action.Body is UnaryExpression body))
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
        public List<TData> All(LambdaItem<TData> lambda)
        {
            var convert = Compile(lambda);
            return LoadPageData(1, -1, PrimaryKey, false, convert.ConditionSql, convert.Parameters);
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
            var obj = CollectInner("Count", "*", condition, args.Cast<MySqlParameter>().ToArray());
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


        #region Sum

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

        #region ��ҳ��ȡ

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public ApiPageData<TData> Page(int page, int limit)
        {
            return Page(page, limit, KeyField, false, null, null);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public ApiPageData<TData> Page(int page, int limit, string condition, params MySqlParameter[] args)
        {
            return Page(page, limit, KeyField, false, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public ApiPageData<TData>  Page(int page, int limit, Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return Page(page, limit, KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public ApiPageData<TData> Page(int page, int limit, LambdaItem<TData> lambda)
        {
            var convert = Compile(lambda);
            return Page(page, limit, KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public ApiPageData<TData> Page(int page, int limit, string order, bool desc, string condition, params DbParameter[] args)
        {
            return Page(page, limit, order, desc, condition, args.Cast<MySqlParameter>().ToArray());
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public ApiPageData<TData> Page(int page, int limit, string order, string condition, params MySqlParameter[] args)
        {
            return Page(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public ApiPageData<TData> Page<TField>(int page, int limit, Expression<Func<TData, TField>> field,
            Expression<Func<TData, bool>> lambda)
        {
            return Page(page, limit, field, false, lambda);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public ApiPageData<TData> Page<TField>(int page, int limit, Expression<Func<TData, TField>> field, bool desc,
            Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return Page(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public ApiPageData<TData> Page<TField>(int page, int limit, Expression<Func<TData, TField>> field, bool desc,
            LambdaItem<TData> lambda)
        {
            var convert = Compile(lambda);
            return Page(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
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
        public ApiPageData<TData> Page(int page, int limit, string order, bool desc, string condition,
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
            return LoadPage(page, limit, order, desc, condition, args);
        }

        private ApiPageData<TData> LoadPage(int page, int limit, string order, bool desc, string condition,
            MySqlParameter[] args)
        {
            var data = PageData(page, limit, order, desc, condition, args);
            var count = (int)Count(condition, args);
            return new ApiPageData<TData>
            {
                RowCount = count,
                Rows = data,
                PageIndex = page,
                PageSize = limit,
                PageCount = count / limit + (((count % limit) > 0 ? 1 : 0))
            };
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
            var vl = LoadValueInner(fn, FieldConditionSQL(KeyField), CreateFieldParameter(KeyField, GetDbType(KeyField), key));
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
            var vl = LoadValueInner(fn, FieldConditionSQL(KeyField), CreateFieldParameter(KeyField, GetDbType(KeyField), key));
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
        ///     ������������
        /// </summary>
        /// <param name="condition">����</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public List<TData> LoadData(MulitCondition condition)
        {
            if (condition == null || string.IsNullOrEmpty(condition.Condition))
                return new List<TData>();
            if (condition.Parameters == null)
                return LoadDataInner(condition.Condition);
            List<MySqlParameter> args = new List<MySqlParameter>();
            foreach (var item in condition.Parameters)
            {
                var pa = new MySqlParameter(item.Name, item.Type);
                if (item.Value == null)
                    pa.Value = DBNull.Value;
                else
                    switch (item.Type)
                    {
                        default:
                            //case DbType.Xml:
                            //case DbType.String:
                            //case DbType.StringFixedLength:
                            //case DbType.AnsiStringFixedLength:
                            //case DbType.AnsiString:
                            pa.Size = item.Value.Length * 2;
                            pa.Value = item.Value;
                            break;
                        case DbType.Boolean:
                        {
                            pa.Value = bool.TryParse(item.Value, out var vl) && vl;
                        }
                            break;
                        case DbType.Byte:
                        {
                            if (Byte.TryParse(item.Value, out var vl))
                                pa.Value = vl;
                            else pa.Value = (Byte)0;
                        }
                            break;
                        case DbType.VarNumeric:
                        case DbType.Decimal:
                        case DbType.Currency:
                        {
                            if (decimal.TryParse(item.Value, out var vl))
                                pa.Value = vl;
                            else pa.Value = (decimal)0;
                        }
                            break;
                        case DbType.Time:
                        case DbType.DateTime2:
                        case DbType.DateTime:
                        case DbType.Date:
                        {
                            if (DateTime.TryParse(item.Value, out var vl))
                                pa.Value = vl;
                            else pa.Value = DBNull.Value;
                        }
                            break;
                        case DbType.DateTimeOffset:
                        {
                            if (TimeSpan.TryParse(item.Value, out var vl))
                                pa.Value = vl;
                            else pa.Value = DBNull.Value;
                        }
                            break;
                        case DbType.Double:
                        {
                            if (double.TryParse(item.Value, out var vl))
                                pa.Value = vl;
                            else pa.Value = DBNull.Value;
                        }
                            break;
                        case DbType.Guid:
                        {
                            if (Guid.TryParse(item.Value, out var vl))
                                pa.Value = vl;
                            else pa.Value = DBNull.Value;
                        }
                            break;
                        case DbType.Int16:
                        {
                            if (short.TryParse(item.Value, out var vl))
                                pa.Value = vl;
                            else pa.Value = DBNull.Value;
                        }
                            break;
                        case DbType.Int32:
                        {
                            if (Int32.TryParse(item.Value, out var vl))
                                pa.Value = vl;
                            else pa.Value = DBNull.Value;
                        }
                            break;
                        case DbType.Int64:
                        {
                            if (Int64.TryParse(item.Value, out var vl))
                                pa.Value = vl;
                            else pa.Value = DBNull.Value;
                        }
                            break;
                        case DbType.SByte:
                            break;
                        case DbType.Single:
                        {
                            if (Single.TryParse(item.Value, out var vl))
                                pa.Value = vl;
                            else pa.Value = DBNull.Value;
                        }
                            break;
                        case DbType.UInt16:
                        {
                            if (UInt16.TryParse(item.Value, out var vl))
                                pa.Value = vl;
                            else pa.Value = DBNull.Value;
                        }
                            break;
                        case DbType.UInt32:
                        {
                            if (UInt32.TryParse(item.Value, out var vl))
                                pa.Value = vl;
                            else pa.Value = DBNull.Value;
                        }
                            break;
                        case DbType.UInt64:
                        {
                            if (UInt64.TryParse(item.Value, out var vl))
                                pa.Value = vl;
                            else pa.Value = DBNull.Value;
                        }
                            break;
                    }
                args.Add(pa);
            }
            return LoadDataInner(condition.Condition, args.ToArray());
        }


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
            return LoadFirstInner(FieldConditionSQL(foreignKey), CreateFieldParameter(foreignKey, GetDbType(foreignKey), key));
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
            return LoadLastInner(FieldConditionSQL(foreignKey), CreateFieldParameter(foreignKey, GetDbType(foreignKey), key));
        }

        /// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        public List<TData> LoadByForeignKey(string foreignKey, object key)
        {
            return LoadDataInner(FieldConditionSQL(foreignKey), CreateFieldParameter(foreignKey, GetDbType(foreignKey), key));
        }

        /// <summary>
        ///     ���¶�ȡ
        /// </summary>
        public void ReLoad(TData entity)
        {
            ReLoadInner(entity);
            entity.OnStatusChanged(NotificationStatusType.Refresh);
        }

        /// <summary>
        ///     ���¶�ȡ
        /// </summary>
        public void ReLoad(ref TData entity)
        {
            ReLoadInner(entity);
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
                if (DynamicLoadAction != null)
                    DynamicLoadAction(reader, entity);
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
                        if (DynamicLoadAction != null)
                            DynamicLoadAction(reader, entity);
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