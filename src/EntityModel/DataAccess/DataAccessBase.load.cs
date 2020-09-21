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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace Agebull.EntityModel.Common
{
    partial class DataAccess<TEntity>
    {
        #region 遍历所有

        /// <summary>
        ///     遍历所有
        /// </summary>
        public void FeachAll(Action<TEntity> action, Action<List<TEntity>> end)
        {
            //Debug.WriteLine(typeof(TEntity).Name, "对象");
            var list = All();
            //Debug.WriteLine(list.Count, "数量");
            if (list.Count == 0)
                return;
            //DateTime s = DateTime.Now;
            list.ForEach(p => p.DoLaterPeriodByAllModified());
            list.ForEach(action);
            end(list);
            //Debug.WriteLine((DateTime.Now - s).TotalSeconds, "用时");
            //Debug.WriteLine((DateTime.Now - s).TotalSeconds / list.Count, "均时");
        }

        #endregion

        #region 查询条件相关(包含lambda编译)

        /// <summary>
        ///     编译查询条件
        /// </summary>
        /// <param name="lambda">条件</param>
        public ConditionItem Compile(Expression<Func<TEntity, bool>> lambda)
        {
            return SqlBuilder.Compile(SqlBuilder.SqlOption.FieldMap, lambda);
        }

        /// <summary>
        ///     编译查询条件
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        public ConditionItem Compile(LambdaItem<TEntity> lambda)
        {
            return SqlBuilder.Compile(SqlBuilder.SqlOption.FieldMap, lambda);
        }

        /// <summary>
        ///     取属性名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<TEntity, T>> action)
        {
            if (action.Body is MemberExpression expression)
                return expression.Member.Name;
            if (!(action.Body is UnaryExpression body))
                throw new EntityModelDbException("表达式太复杂");

            expression = (MemberExpression)body.Operand;
            return expression.Member.Name;
        }

        #endregion

        #region 首行

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TEntity First()
        {
            return LoadFirst();
        }


        /// <summary>
        ///     分页读取
        /// </summary>
        public TEntity First(LambdaItem<TEntity> lambda)
        {
            var convert = Compile(lambda);
            return LoadFirst(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     分页读取
        /// </summary>
        public TEntity FirstOrDefault(LambdaItem<TEntity> lambda)
        {
            var convert = Compile(lambda);

            return LoadFirst(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TEntity FirstOrDefault()
        {
            return LoadFirst();
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TEntity First(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);

            return LoadFirst(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TEntity First(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return LoadFirst($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters));
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public TEntity First(string condition, DbParameter[] args)
        {
            return LoadFirst(condition, args);
        }


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);

            return LoadFirst(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return LoadFirst($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters));
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="args">参数</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TEntity FirstOrDefault(string condition, DbParameter[] args)
        {
            return LoadFirst(condition, args);
        }

        #endregion

        #region 尾行

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TEntity Last()
        {
            return LoadLast();
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TEntity LastOrDefault()
        {
            return LoadLast();
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TEntity Last(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);

            return LoadLast(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TEntity Last(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return LoadLast($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters));
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public TEntity Last(string condition, DbParameter[] args)
        {
            return LoadLast(condition, args);
        }


        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TEntity LastOrDefault(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);

            return LoadLast(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TEntity LastOrDefault(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return LoadLast($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters));
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="args">参数</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TEntity LastOrDefault(string condition, DbParameter[] args)
        {
            return LoadLast(condition, args);
        }

        #endregion

        #region Select

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        [Obsolete]
        public List<TEntity> Select()
        {
            return LoadDataInner();
        }

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>数据</returns>
        [Obsolete]
        public List<TEntity> Select(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            return LoadDataInner(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>数据</returns>
        [Obsolete]
        public List<TEntity> Select(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return LoadDataInner($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }

        #endregion

        #region All

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>数据</returns>
        public List<TEntity> All()
        {
            return LoadDataInner();
        }


        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>数据</returns>
        public List<TEntity> All(string condition, DbParameter[] args)
        {
            return LoadDataInner(condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TEntity> All(LambdaItem<TEntity> lambda)
        {
            var convert = Compile(lambda);
            return LoadPageData(1, -1, SqlBuilder.SqlOption.KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TEntity> All<TField>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TField>> orderBy,
            bool desc)
        {
            var convert = Compile(lambda);
            return LoadPageData(1, -1, GetPropertyName(orderBy), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>数据</returns>
        public List<TEntity> All(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return LoadDataInner($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }


        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <param name="orderBys">排序</param>
        /// <returns>数据</returns>
        public List<TEntity> All(LambdaItem<TEntity> lambda, params string[] orderBys)
        {
            var convert = Compile(lambda);
            return LoadDataInner(convert.ConditionSql, convert.Parameters,
                orderBys.Length == 0 ? null : string.Join(",", orderBys));
        }

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <param name="orderBys">排序</param>
        /// <returns>数据</returns>
        public List<TEntity> All(Expression<Func<TEntity, bool>> lambda, params string[] orderBys)
        {
            var convert = Compile(lambda);
            return LoadDataInner(convert.ConditionSql, convert.Parameters,
                orderBys.Length == 0 ? null : string.Join(",", orderBys));
        }

        #endregion

        #region Where

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        [Obsolete]
        public List<TEntity> Where(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            return LoadDataInner(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>是否存在数据</returns>
        [Obsolete]
        public List<TEntity> Where(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return LoadDataInner($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }

        #endregion

        #region 聚合函数支持

        #region Collect

        /// <summary>
        ///     汇总方法
        /// </summary>
        public object Collect(string fun, string field)
        {
            return CollectInner(fun, SqlBuilder.SqlOption.FieldMap[field], null, null);
        }

        /// <summary>
        ///     汇总方法
        /// </summary>
        public object Collect<TValue>(string fun, Expression<Func<TEntity, TValue>> field)
        {
            var expression = (MemberExpression)field.Body;
            return CollectInner(fun, SqlBuilder.SqlOption.FieldMap[expression.Member.Name], null, null);
        }

        /// <summary>
        ///     汇总方法
        /// </summary>
        public object Collect(string fun, string field, string condition, params DbParameter[] args)
        {
            return CollectInner(fun, SqlBuilder.SqlOption.FieldMap[field], condition, args);
        }


        /// <summary>
        ///     汇总方法
        /// </summary>
        public object Collect(string fun, string field, Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            return CollectInner(fun, field, convert.ConditionSql, convert.Parameters);
        }

        #endregion

        #region Exist

        /// <summary>
        ///     是否存在数据
        /// </summary>
        public bool Exist()
        {
            return Count() > 0;
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        public bool Exist(string condition, params DbParameter[] args)
        {
            return Count(condition, args) > 0;
        }

        /// <summary>
        ///     是否存在此主键的数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>是否存在数据</returns>
        public bool ExistPrimaryKey<T>(T id)
        {
            return ExistInner(SqlBuilder.PrimaryKeyConditionSQL, ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, id, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
        }

        #endregion

        #region Count

        /// <summary>
        ///     总数
        /// </summary>
        public long Count()
        {
            return CountInner();
        }

        /// <summary>
        ///     总数
        /// </summary>
        public long Count(string condition, params DbParameter[] args)
        {
            var obj = CollectInner("Count", "*", condition, args);
            return obj == DBNull.Value || obj == null ? 0L : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public long Count(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            var obj = CollectInner("Count", "*", convert.ConditionSql, convert.Parameters);
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public long Count(LambdaItem<TEntity> lambda)
        {
            var convert = Compile(lambda);
            var obj = CollectInner("Count", "*", convert.ConditionSql, convert.Parameters);
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public long Count(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            var obj = CollectInner("Count", "*",
                $"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }


        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">查询表达式</param>
        /// <param name="args"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public long Count<TValue>(Expression<Func<TEntity, TValue>> field, string condition, params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = CollectInner("Count", SqlBuilder.SqlOption.FieldMap[expression.Member.Name], condition, args);
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }
        #endregion


        #region Any

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public bool Any()
        {
            return ExistInner();
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public bool Any(string condition, DbParameter[] args)
        {
            return ExistInner(condition, args);
        }


        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public bool Any(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            return ExistInner(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public bool Any(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return ExistInner($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }

        #endregion

        #region Min

        /// <summary>
        ///     汇总
        /// </summary>
        public decimal? Min(string field)
        {
            var obj = CollectInner("Min", SqlBuilder.SqlOption.FieldMap[field], null, null);
            if (obj == DBNull.Value || obj == null)
                return null;
            return Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     汇总
        /// </summary>
        public decimal? Min(string field, string condition, params DbParameter[] args)
        {
            var obj = CollectInner("Min", SqlBuilder.SqlOption.FieldMap[field], condition, args);
            if (obj == DBNull.Value || obj == null)
                return null;
            return Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">查询表达式</param>
        /// <param name="condition2">条件2，默认为空</param>
        public decimal? Min<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> lambda,
            string condition2 = null)
        {
            var expression = (MemberExpression)field.Body;
            var convert = Compile(lambda);
            var condition = condition2 == null
                ? convert.ConditionSql
                : convert.ConditionSql == null
                    ? condition2
                    : $"({convert.ConditionSql}) AND ({condition2})";

            var obj = CollectInner("Min", SqlBuilder.SqlOption.FieldMap[expression.Member.Name], condition, convert.Parameters);
            if (obj == DBNull.Value || obj == null)
                return null;
            return Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public decimal? Min<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> a,
            Expression<Func<TEntity, bool>> b)
        {
            var expression = (MemberExpression)field.Body;
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            var obj = CollectInner("Min", SqlBuilder.SqlOption.FieldMap[expression.Member.Name],
                $"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
            if (obj == DBNull.Value || obj == null)
                return null;
            return Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">查询表达式</param>
        /// <param name="args"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public decimal? Min<TValue>(Expression<Func<TEntity, TValue>> field, string condition,
            params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = CollectInner("Min", SqlBuilder.SqlOption.FieldMap[expression.Member.Name], condition, args);
            if (obj == DBNull.Value || obj == null)
                return null;
            return Convert.ToDecimal(obj);
        }

        #endregion

        #region Max

        /// <summary>
        ///     汇总
        /// </summary>
        public decimal? Max(string field)
        {
            var obj = CollectInner("Max", SqlBuilder.SqlOption.FieldMap[field], null, null);
            if (obj == DBNull.Value || obj == null)
                return null;
            return Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     汇总
        /// </summary>
        public decimal? Max(string field, string condition, params DbParameter[] args)
        {
            var obj = CollectInner("Max", SqlBuilder.SqlOption.FieldMap[field], condition, args);
            if (obj == DBNull.Value || obj == null)
                return null;
            return Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">查询表达式</param>
        /// <param name="condition2">条件2，默认为空</param>
        public decimal? Max<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> lambda,
            string condition2 = null)
        {
            var expression = (MemberExpression)field.Body;
            var convert = Compile(lambda);
            var condition = condition2 == null
                ? convert.ConditionSql
                : convert.ConditionSql == null
                    ? condition2
                    : $"({convert.ConditionSql}) AND ({condition2})";

            var obj = CollectInner("Max", SqlBuilder.SqlOption.FieldMap[expression.Member.Name], condition, convert.Parameters);
            if (obj == DBNull.Value || obj == null)
                return null;
            return Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public decimal? Max<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> a,
            Expression<Func<TEntity, bool>> b)
        {
            var expression = (MemberExpression)field.Body;
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            var obj = CollectInner("Max", SqlBuilder.SqlOption.FieldMap[expression.Member.Name],
                $"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
            if (obj == DBNull.Value || obj == null)
                return null;
            return Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">查询表达式</param>
        /// <param name="args"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public decimal? Max<TValue>(Expression<Func<TEntity, TValue>> field, string condition,
            params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = CollectInner("Max", SqlBuilder.SqlOption.FieldMap[expression.Member.Name], condition, args);
            if (obj == DBNull.Value || obj == null)
                return null;
            return Convert.ToDecimal(obj);
        }

        #endregion

        #region Sum

        /// <summary>
        ///     汇总
        /// </summary>
        public decimal Sum(string field)
        {
            var obj = CollectInner("Sum", SqlBuilder.SqlOption.FieldMap[field], null, null);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     汇总
        /// </summary>
        public decimal Sum(string field, string condition, params DbParameter[] args)
        {
            var obj = CollectInner("Sum", SqlBuilder.SqlOption.FieldMap[field], condition, args);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">查询表达式</param>
        /// <param name="condition2">条件2，默认为空</param>
        public decimal Sum<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> lambda,
            string condition2 = null)
        {
            var expression = (MemberExpression)field.Body;
            var convert = Compile(lambda);
            var condition = condition2 == null
                ? convert.ConditionSql
                : convert.ConditionSql == null
                    ? condition2
                    : $"({convert.ConditionSql}) AND ({condition2})";
            var obj = CollectInner("Sum", SqlBuilder.SqlOption.FieldMap[expression.Member.Name], condition, convert.Parameters);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public decimal Sum<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> a,
            Expression<Func<TEntity, bool>> b)
        {
            var expression = (MemberExpression)field.Body;
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            var obj = CollectInner("Sum", SqlBuilder.SqlOption.FieldMap[expression.Member.Name],
                $"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">查询表达式</param>
        /// <param name="args"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public decimal Sum<TValue>(Expression<Func<TEntity, TValue>> field, string condition,
            params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = CollectInner("Sum", SqlBuilder.SqlOption.FieldMap[expression.Member.Name], condition, args);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }

        #endregion

        #region 实现

        /// <summary>
        ///     是否存在数据
        /// </summary>
        protected bool ExistInner(string condition = null, DbParameter args = null)
        {
            return ExistInner(condition, args == null ? new DbParameter[0] : new[] { args });
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        protected bool ExistInner(string condition, DbParameter[] args)
        {
            var obj = CollectInner("Count", "*", condition, args);
            return obj != DBNull.Value && obj != null && Convert.ToInt64(obj) > 0;
        }

        /// <summary>
        ///     总数据量
        /// </summary>
        protected long CountInner(string condition = null, DbParameter args = null)
        {
            var obj = CollectInner("Count", "*", condition, args);
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     总数据量
        /// </summary>
        protected object CollectInner(string fun, string field, string condition, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateCollectSql(fun, field, condition);
            {
                return DataBase.ExecuteScalar(sql, args);
            }
        }

        #endregion

        #endregion

        #region 分页读取

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TEntity> PageData(int page, int limit)
        {
            return PageData(page, limit, SqlBuilder.SqlOption.KeyField, false, null, null);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TEntity> PageData(int page, int limit, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, SqlBuilder.SqlOption.KeyField, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TEntity> PageData(int page, int limit, Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            return PageData(page, limit, SqlBuilder.SqlOption.KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TEntity> PageData(int page, int limit, LambdaItem<TEntity> lambda)
        {
            var convert = Compile(lambda);
            return PageData(page, limit, SqlBuilder.SqlOption.KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TEntity> LoadData(int page, int limit, string order, bool desc, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, order, desc, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        /// <param name="page">页号(从1开始)</param>
        /// <param name="limit">每页行数(小于不分页）</param>
        /// <param name="order">排序字段</param>
        /// <param name="desc">是否反序</param>
        /// <param name="condition">查询条件</param>
        /// <param name="args">查询参数</param>
        /// <returns></returns>
        public List<TEntity> PageData(int page, int limit, string order, bool desc, string condition, params DbParameter[] args)
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
        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TEntity> LoadData(int page, int limit, string order, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TEntity> PageData(int page, int limit, string order, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TEntity> PageData<TField>(int page, int limit, Expression<Func<TEntity, TField>> field,
            Expression<Func<TEntity, bool>> lambda)
        {
            return PageData(page, limit, field, false, lambda);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TEntity> PageData<TField>(int page, int limit, Expression<Func<TEntity, TField>> field, bool desc,
            Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            return PageData(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TEntity> PageData<TField>(int page, int limit, Expression<Func<TEntity, TField>> field, bool desc,
            LambdaItem<TEntity> lambda)
        {
            var convert = Compile(lambda);
            return PageData(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }



        private List<TEntity> LoadPageData(int page, int limit, string order, bool desc, string condition, DbParameter[] args)
        {
            var results = new List<TEntity>();
            var sql = SqlBuilder.CreatePageSql(page, limit, order, desc, condition);
            using var connectionScope = DataBase.CreateConnectionScope();
            {
                using var cmd = CommandCreater.CreateCommand(connectionScope, sql, args);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(LoadEntity(reader));
                }
            }

            for (var index = 0; index < results.Count; index++)
                results[index] = EntityLoaded(results[index]);
            return results;
        }

        #endregion

        #region 分页读取

        /// <summary>
        ///     分页读取
        /// </summary>
        public ApiPageData<TEntity> Page(int page, int limit)
        {
            return Page(page, limit, SqlBuilder.SqlOption.KeyField, false, null, null);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public ApiPageData<TEntity> Page(int page, int limit, Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            return Page(page, limit, SqlBuilder.SqlOption.KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public ApiPageData<TEntity> Page(int page, int limit, LambdaItem<TEntity> lambda)
        {
            var convert = Compile(lambda);
            return Page(page, limit, SqlBuilder.SqlOption.KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public ApiPageData<TEntity> Page(int page, int limit, string condition, params DbParameter[] args)
        {
            return Page(page, limit, SqlBuilder.SqlOption.KeyField, false, condition, args);
        }


        /// <summary>
        ///     分页读取
        /// </summary>
        public ApiPageData<TEntity> Page(int page, int limit, string order, string condition, params DbParameter[] args)
        {
            return Page(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public ApiPageData<TEntity> Page<TField>(int page, int limit, Expression<Func<TEntity, TField>> field,
            Expression<Func<TEntity, bool>> lambda)
        {
            return Page(page, limit, field, false, lambda);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public ApiPageData<TEntity> Page<TField>(int page, int limit, Expression<Func<TEntity, TField>> field, bool desc,
            Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            return Page(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public ApiPageData<TEntity> Page<TField>(int page, int limit, Expression<Func<TEntity, TField>> field, bool desc,
            LambdaItem<TEntity> lambda)
        {
            var convert = Compile(lambda);
            return Page(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        /// <param name="page">页号(从1开始)</param>
        /// <param name="limit">每页行数(小于不分页）</param>
        /// <param name="order">排序字段</param>
        /// <param name="desc">是否反序</param>
        /// <param name="condition">查询条件</param>
        /// <param name="args">查询参数</param>
        /// <returns></returns>
        public ApiPageData<TEntity> Page(int page, int limit, string order, bool desc, string condition,
            params DbParameter[] args)
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

        private ApiPageData<TEntity> LoadPage(int page, int limit, string order, bool desc, string condition,
            DbParameter[] args)
        {
            var data = PageData(page, limit, order, desc, condition, args);
            var count = (int)Count(condition, args);
            return new ApiPageData<TEntity>
            {
                RowCount = count,
                Rows = data,
                PageIndex = page,
                PageSize = limit,
                PageCount = count / limit + ((count % limit) > 0 ? 1 : 0)
            };
        }

        #endregion

        #region 单列读取

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="lambda">条件</param>
        /// <returns>内容</returns>
        public TField LoadValue<TField>(Expression<Func<TEntity, TField>> field, Expression<Func<TEntity, bool>> lambda)
        {
            var fn = GetPropertyName(field);
            var convert = Compile(lambda);
            var val = LoadValueInner(fn, convert.ConditionSql, convert.Parameters);
            return val == null || val == DBNull.Value ? default(TField) : (TField)val;
        }

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="key">主键</param>
        /// <returns>内容</returns>
        public object Read<TField, TKey>(Expression<Func<TEntity, TField>> field, TKey key)
        {
            var fn = GetPropertyName(field);
            var vl = LoadValueInner(fn, SqlBuilder.FieldConditionSQL(SqlBuilder.SqlOption.KeyField), ParameterCreater.CreateFieldParameter(SqlBuilder.SqlOption.KeyField, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField), key));
            return vl == DBNull.Value || vl == null ? default(TField) : vl;
        }

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="key">主键</param>
        /// <returns>内容</returns>
        public TField LoadValue<TField, TKey>(Expression<Func<TEntity, TField>> field, TKey key)
        {
            var fn = GetPropertyName(field);
            var vl = LoadValueInner(fn, SqlBuilder.FieldConditionSQL(SqlBuilder.SqlOption.KeyField), ParameterCreater.CreateFieldParameter(SqlBuilder.SqlOption.KeyField, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField), key));
            return vl == DBNull.Value || vl == null ? default(TField) : (TField)vl;
        }

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="lambda">条件</param>
        /// <returns>内容</returns>
        public List<TField> LoadValues<TField>(Expression<Func<TEntity, TField>> fieldExpression,
            Expression<Func<TEntity, bool>> lambda)
        {
            var field = GetPropertyName(fieldExpression);
            var convert = Compile(lambda);
            Debug.Assert(SqlBuilder.SqlOption.FieldMap.ContainsKey(field));
            var sql = SqlBuilder.CreateLoadValuesSql(field, convert);
            var values = new List<TField>();
            using var connectionScope = DataBase.CreateConnectionScope();
            {
                using var cmd = CommandCreater.CreateCommand(connectionScope, sql, convert.Parameters);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var vl = reader.GetValue(0);
                    if (vl != DBNull.Value && vl != null)
                        values.Add((TField)vl);
                }
            }

            return values;
        }

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <returns>数据</returns>
        public List<TField> LoadValues<TField>(Expression<Func<TEntity, TField>> fieldExpression, string condition, DbParameter[] args)
        {
            var field = GetPropertyName(fieldExpression);

            var result = LoadValuesInner(field, condition, args);
            return result.Count == 0 ? new List<TField>() : result.Select(p => (TField)p).ToList();
        }

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="condition">条件</param>
        /// <returns>内容</returns>
        public List<TField> LoadValues<TField>(Expression<Func<TEntity, TField>> fieldExpression, string condition)
        {
            var field = GetPropertyName(fieldExpression);

            var result = LoadValuesInner(field, condition);
            return result.Count == 0 ? new List<TField>() : result.Select(p => (TField)p).ToList();
        }

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="parse">转换数据类型方法</param>
        /// <param name="lambda">条件</param>
        /// <returns>内容</returns>
        public List<TField> LoadValues<TField>(Expression<Func<TEntity, TField>> fieldExpression,
            Func<object, TField> parse, Expression<Func<TEntity, bool>> lambda)
        {
            var field = GetPropertyName(fieldExpression);
            var convert = Compile(lambda);
            var result = LoadValuesInner(field, convert.ConditionSql, convert.Parameters);
            return result.Count == 0 ? new List<TField>() : result.Select(parse).ToList();
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public object LoadValue(string field, string condition, params DbParameter[] args)
        {
            return LoadValueInner(field, condition, args);
        }

        /// <summary>
        ///     读取值
        /// </summary>
        protected object LoadValueInner(string field, string condition, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateLoadValueSql(field, condition);
            using var connectionScope = DataBase.CreateConnectionScope();
            {
                return DataBase.ExecuteScalar(sql, args);
            }
        }


        /// <summary>
        ///     读取多个值
        /// </summary>
        protected List<object> LoadValuesInner(string field, string condition, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateLoadValueSql(field, condition);
            var values = new List<object>();
            using var connectionScope = DataBase.CreateConnectionScope();
            {
                using var cmd = CommandCreater.CreateCommand(connectionScope, sql, args);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var vl = reader.GetValue(0);
                    if (vl != DBNull.Value && vl != null)
                        values.Add(vl);
                }
            }

            return values;
        }

        #endregion

        #region 数据读取

        /// <summary>
        ///     载入条件数据
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public List<TEntity> LoadData(MulitCondition condition)
        {
            if (condition == null || string.IsNullOrEmpty(condition.Condition))
                return new List<TEntity>();
            if (condition.Parameters == null)
                return LoadDataInner(condition.Condition);
            List<DbParameter> args = new List<DbParameter>();
            foreach (var item in condition.Parameters)
            {
                var pa = ParameterCreater.CreateParameter(item.Name, item.Type);
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
                                if (byte.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = (byte)0;
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
                                if (int.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Int64:
                            {
                                if (long.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.SByte:
                            break;
                        case DbType.Single:
                            {
                                if (float.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.UInt16:
                            {
                                if (ushort.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.UInt32:
                            {
                                if (uint.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.UInt64:
                            {
                                if (ulong.TryParse(item.Value, out var vl))
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
        ///     载入首行
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TEntity LoadData(object id)
        {
            return LoadByPrimaryKey(id);
        }


        /// <summary>
        ///     全表读取
        /// </summary>
        public List<TEntity> LoadData()
        {
            return LoadDataInner();
        }


        /// <summary>
        ///     条件读取
        /// </summary>
        public List<TEntity> LoadData(string condition, params DbParameter[] args)
        {
            return LoadDataInner(condition, args);
        }

        /// <summary>
        ///     主键读取
        /// </summary>
        public virtual TEntity LoadByPrimaryKey(object key)
        {
            return LoadFirstInner(SqlBuilder.PrimaryKeyConditionSQL, ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, key, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
        }

        /// <summary>
        ///     主键读取
        /// </summary>
        public List<TEntity> LoadByPrimaryKeies(IEnumerable keies)
        {
            var list = new List<TEntity>();
            var par = ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField));
            foreach (var key in keies)
            {
                par.Value = key;
                list.Add(LoadFirstInner(SqlBuilder.PrimaryKeyConditionSQL, par));
            }

            return list;
        }


        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public TEntity LoadFirst(string condition = null)
        {
            return LoadFirstInner(condition);
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public TEntity LoadFirst(string condition, params DbParameter[] args)
        {
            return LoadFirstInner(condition, args);
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public TEntity LoadFirst(string foreignKey, object key)
        {
            return LoadFirstInner(SqlBuilder.FieldConditionSQL(foreignKey), ParameterCreater.CreateFieldParameter(foreignKey, SqlBuilder.GetDbType(foreignKey), key));
        }

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        public TEntity LoadLast(string condition = null)
        {
            return LoadLastInner(condition);
        }

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        public TEntity LoadLast(string condition, params DbParameter[] args)
        {
            return LoadLastInner(condition, args);
        }

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        public TEntity LoadLast(string foreignKey, object key)
        {
            return LoadLastInner(SqlBuilder.FieldConditionSQL(foreignKey), ParameterCreater.CreateFieldParameter(foreignKey, SqlBuilder.GetDbType(foreignKey), key));
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public List<TEntity> LoadByForeignKey(string foreignKey, object key)
        {
            return LoadDataInner(SqlBuilder.FieldConditionSQL(foreignKey), ParameterCreater.CreateFieldParameter(foreignKey, SqlBuilder.GetDbType(foreignKey), key));
        }

        /// <summary>
        ///     重新读取
        /// </summary>
        public void ReLoad(TEntity entity)
        {
            ReLoadInner(entity);
            entity.OnStatusChanged(NotificationStatusType.Refresh);
        }

        /// <summary>
        ///     重新读取
        /// </summary>
        public void ReLoad(ref TEntity entity)
        {
            ReLoadInner(entity);
            entity.OnStatusChanged(NotificationStatusType.Refresh);
        }

        #endregion

        #region 载入事件

        /// <summary>
        ///     载入数据
        /// </summary>
        /// <param name="entity">读取数据的实体</param>
        private TEntity EntityLoaded(TEntity entity)
        {
            entity = OnEntityLoaded(entity);
            OnLoadAction?.Invoke(entity);
            return entity;
        }

        /// <summary>
        ///     载入后的同步处理
        /// </summary>
        /// <param name="entity"></param>
        protected virtual TEntity OnEntityLoaded(TEntity entity)
        {
            return entity;
        }

        /// <summary>
        ///     数据载入时给外部的处理方法
        /// </summary>
        public Action<TEntity> OnLoadAction;

        /// <summary>
        ///     载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <returns>读取数据的实体</returns>
        private TEntity LoadEntity(DbDataReader reader)
        {
            var entity = new TEntity();
            using (new EntityLoadScope(entity))
            {
                if (DynamicLoadAction != null)
                    DynamicLoadAction(reader, entity);
                else
                    SqlBuilder.SqlOption.LoadEntity(reader, entity);
            }
            return entity;
        }

        /// <summary>
        ///     重新载入
        /// </summary>
        private void ReLoadInner(TEntity entity)
        {
            entity.__status.RejectChanged();
            using var connectionScope = DataBase.CreateConnectionScope();
            {
                var para = ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, entity.GetValue(SqlBuilder.SqlOption.KeyField), SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField));
                using var cmd = CreateLoadCommand(connectionScope, SqlBuilder.PrimaryKeyConditionSQL, para);
                using var reader = cmd.ExecuteReader();
                if (!reader.Read())
                    return;
                using (new EntityLoadScope(entity))
                {
                    if (DynamicLoadAction != null)
                        DynamicLoadAction(reader, entity);
                    else
                        SqlBuilder.SqlOption.LoadEntity(reader, entity);
                }
            }

            var entity2 = EntityLoaded(entity);
            if (entity != entity2)
                entity.CopyValue(entity2);
        }


        /// <summary>
        ///     读取首行
        /// </summary>
        protected TEntity LoadFirstInner(string condition = null, DbParameter args = null)
        {
            return LoadFirstInner(condition, args == null ? new DbParameter[0] : new[] { args });
        }

        /// <summary>
        ///     读取首行
        /// </summary>
        protected TEntity LoadFirstInner(string condition, DbParameter[] args)
        {
            TEntity entity = null;
            using var connectionScope = DataBase.CreateConnectionScope();
            {
                using var cmd = CreateLoadCommand(connectionScope, condition, args);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                    entity = LoadEntity(reader);

                if (entity != null)
                    entity = EntityLoaded(entity);
            }
            return entity;
        }


        /// <summary>
        ///     读取尾行
        /// </summary>
        protected TEntity LoadLastInner(string condition = null, DbParameter args = null)
        {
            return LoadLastInner(condition, args == null ? new DbParameter[0] : new[] { args });
        }

        /// <summary>
        ///     读取尾行
        /// </summary>
        protected TEntity LoadLastInner(string condition, DbParameter[] args)
        {
            TEntity entity = null;
            using var connectionScope = DataBase.CreateConnectionScope();
            {
                using (var cmd = CreateLoadCommand(connectionScope, SqlBuilder.SqlOption.KeyField, true, condition, args))
                {
                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                        entity = LoadEntity(reader);
                }
                if (entity != null)
                    entity = EntityLoaded(entity);
            }
            return entity;
        }

        /// <summary>
        ///     读取全部
        /// </summary>
        protected List<TEntity> LoadDataInner(string condition = null, DbParameter args = null)
        {
            return LoadDataInner(condition, args == null ? new DbParameter[0] : new[] { args }, null);
        }

        /// <summary>
        ///     读取全部
        /// </summary>
        protected List<TEntity> LoadDataInner(string condition, DbParameter[] args)
        {
            return LoadDataInner(condition, args, null);
        }

        /// <summary>
        ///     读取全部
        /// </summary>
        protected List<TEntity> LoadDataInner(string condition, DbParameter[] args, string orderBy)
        {
            var results = new List<TEntity>();
            using var connectionScope = DataBase.CreateConnectionScope();
            {
                using (var cmd = CreateLoadCommand(connectionScope, condition, orderBy, args))
                {
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                        results.Add(LoadEntity(reader));
                }
                for (var index = 0; index < results.Count; index++)
                    results[index] = EntityLoaded(results[index]);
            }
            return results;
        }

        /// <summary>
        ///     读取全部(SQL语句是重写的,字段名称和顺序与设计时相同)
        /// </summary>
        protected List<TEntity> LoadDataBySql(string sql, DbParameter[] args)
        {
            var results = new List<TEntity>();
            using var connectionScope = DataBase.CreateConnectionScope();
            {
                using var cmd = CommandCreater.CreateCommand(connectionScope, sql, args);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(LoadEntity(reader));
                }
            }

            for (var index = 0; index < results.Count; index++)
                results[index] = EntityLoaded(results[index]);
            return results;
        }

        /// <summary>
        ///     读取存储过程
        /// </summary>
        public List<TEntity> LoadDataByProcedure(string procedure, DbParameter[] args)
        {
            var results = new List<TEntity>();

            using var connectionScope = DataBase.CreateConnectionScope();
            {
                using var cmd = CommandCreater.CreateCommand(connectionScope, procedure, args);
                cmd.CommandType = CommandType.StoredProcedure;
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(LoadEntity(reader));
                }
            }

            for (var index = 0; index < results.Count; index++)
                results[index] = EntityLoaded(results[index]);
            return results;
        }

        #endregion

        #region 数据校验支持

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="condition"></param>
        public bool IsUnique<TValue>(Expression<Func<TEntity, TValue>> field, object val, Expression<Func<TEntity, bool>> condition)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            var convert = Compile(condition);
            convert.AddAndCondition(SqlBuilder.Condition(fieldName, "c_vl_"), ParameterCreater.CreateDbParameter("c_vl_", fieldName, val));
            return !Exist(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="key"></param>
        public bool IsUnique<TValue>(Expression<Func<TEntity, TValue>> field, object val, object key)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            Debug.Assert(SqlBuilder.SqlOption.FieldMap.ContainsKey(fieldName));
            return !Exist($"({SqlBuilder.Condition(fieldName, "c_vl_")} AND {SqlBuilder.FieldConditionSQL(SqlBuilder.SqlOption.KeyField, "<>")}"
                , ParameterCreater.CreateDbParameter("c_vl_", fieldName, val)
                , ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, key, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
        }

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        public bool IsUnique<TValue>(Expression<Func<TEntity, TValue>> field, object val)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            return !Exist(SqlBuilder.Condition(fieldName, "c_vl_"), ParameterCreater.CreateDbParameter("c_vl_", fieldName, val));
        }

        #endregion
    }
}