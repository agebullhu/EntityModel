// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     Sql实体访问类
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    public partial class DataQuery<TEntity> : DataAccessBase<TEntity>
         where TEntity : class, new()
    {

        #region 构造

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="provider"></param>
        public DataQuery(DataAccessProvider<TEntity> provider)
            :base(provider)
        {
        }

        #endregion

        #region 首行

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <returns>如果有载入首行,否则返回空</returns>
        public Task<TEntity> FirstAsync()
        {
            return LoadFirstInnerAsync(null);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public Task<TEntity> FirstAsync(LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return LoadFirstInnerAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return LoadFirstInnerAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            return LoadFirstInnerAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());//SQL
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public Task<TEntity> FirstAsync(string condition, DbParameter[] args)
        {
            return LoadFirstInnerAsync(condition, args);
        }


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public Task<TEntity> FirstOrDefaultAsync(LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return LoadFirstInnerAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);

            return LoadFirstInnerAsync(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            return LoadFirstInnerAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());//SQL
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="args">参数</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public Task<TEntity> FirstOrDefaultAsync(string condition, DbParameter[] args)
        {
            return LoadFirstInnerAsync(condition, args);
        }

        #endregion

        #region 尾行

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public Task<TEntity> LastAsync()
        {
            return LoadLastInnerAsync(null);
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public Task<TEntity> LastAsync(LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);

            return LoadLastInnerAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public Task<TEntity> LastAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);

            return LoadLastInnerAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public Task<TEntity> LastAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            return LoadLastInnerAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());//SQL
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public Task<TEntity> LastAsync(string condition, DbParameter[] args)
        {
            return LoadLastInnerAsync(condition, args);
        }


        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public Task<TEntity> LastOrDefaultAsync(LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);

            return LoadLastInnerAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);

            return LoadLastInnerAsync(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            return LoadLastInnerAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());//SQL
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="args">参数</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public Task<TEntity> LastOrDefaultAsync(string condition, DbParameter[] args)
        {
            return LoadLastInnerAsync(condition, args);
        }

        #endregion

        #region All

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>数据</returns>
        public Task<List<TEntity>> AllAsync()
        {
            return LoadDataInnerAsync(null, null, null);
        }


        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>数据</returns>
        public Task<List<TEntity>> AllAsync(string condition, params DbParameter[] args)
        {
            return LoadDataInnerAsync(condition, null, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public Task<List<TEntity>> AllAsync(LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return LoadPageDataAsync(1, -1, Option.PrimaryKey, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public Task<List<TEntity>> AllAsync<TField>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TField>> orderBy,
            bool desc)
        {
            var convert = SqlBuilder.Compile(lambda);
            return LoadPageDataAsync(1, -1, GetPropertyName(orderBy), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>数据</returns>
        public Task<List<TEntity>> AllAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            return LoadDataInnerAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , null
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }


        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <param name="orderBys">排序</param>
        /// <returns>数据</returns>
        public Task<List<TEntity>> AllAsync(LambdaItem<TEntity> lambda, params string[] orderBys)
        {
            var convert = SqlBuilder.Compile(lambda);
            return LoadDataInnerAsync(convert.ConditionSql,
                orderBys.Length == 0 ? null : string.Join(",", orderBys),
                convert.Parameters);
        }

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <param name="orderBys">排序</param>
        /// <returns>数据</returns>
        public Task<List<TEntity>> AllAsync(Expression<Func<TEntity, bool>> lambda, params string[] orderBys)
        {
            var convert = SqlBuilder.Compile(lambda);
            return LoadDataInnerAsync(convert.ConditionSql,
                orderBys.Length == 0 ? null : string.Join(",", orderBys),
                convert.Parameters);
        }

        #endregion

        #region 聚合函数

        #region Collect

        /// <summary>
        ///     汇总方法
        /// </summary>
        public Task<(bool hase, object value)> CollectAsync(string fun, string field)
        {
            return CollectInnerAsync(fun, Option.FieldMap[field], null, null);
        }

        /// <summary>
        ///     汇总方法
        /// </summary>
        public Task<(bool hase, object value)> CollectAsync<TValue>(string fun, Expression<Func<TEntity, TValue>> field)
        {
            var expression = (MemberExpression)field.Body;
            return CollectInnerAsync(fun, Option.FieldMap[expression.Member.Name], null, null);
        }

        /// <summary>
        ///     汇总方法
        /// </summary>
        public Task<(bool hase, object value)> CollectAsync(string fun, string field, string condition, params DbParameter[] args)
        {
            return CollectInnerAsync(fun, Option.FieldMap[field], condition, args);
        }


        /// <summary>
        ///     汇总方法
        /// </summary>
        public Task<(bool hase, object value)> CollectAsync(string fun, string field, Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return CollectInnerAsync(fun, field, convert.ConditionSql, convert.Parameters);
        }

        #endregion

        #region Exist

        /// <summary>
        ///     是否存在数据
        /// </summary>
        public Task<bool> ExistAsync()
        {
            return ExistInnerAsync();
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        public Task<bool> ExistAsync(string condition, params DbParameter[] args)
        {
            return ExistInnerAsync(condition, args);
        }

        /// <summary>
        ///     是否存在此主键的数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>是否存在数据</returns>
        public Task<bool> ExistPrimaryKeyAsync<T>(T id)
        {
            return ExistInnerAsync(SqlBuilder.PrimaryKeyCondition, ParameterCreater.CreateParameter(Option.PrimaryKey, id, SqlBuilder.GetDbType(Option.PrimaryKey)));
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public async Task<bool> AnyAsync()
        {
            return await ExistInnerAsync();
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public async Task<bool> AnyAsync(string condition, DbParameter[] args)
        {
            return await ExistInnerAsync(condition, args);
        }


        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return await ExistInnerAsync(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            return await ExistInnerAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }
        /// <summary>
        ///     是否存在数据
        /// </summary>
        protected async Task<bool> ExistInnerAsync(string condition = null, DbParameter args = null)
        {
            return await ExistInnerAsync(condition, args == null ? new DbParameter[0] : new[] { args });
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        protected async Task<bool> ExistInnerAsync(string condition, DbParameter[] args)
        {
            var (hase, _) = await LoadValueAsync(Option.PrimaryKey, condition, args);
            return hase;
        }

        #endregion

        #region Count

        /// <summary>
        ///     总数
        /// </summary>
        public async Task<long> CountAsync()
        {
            return await CountInnerAsync();
        }

        /// <summary>
        ///     总数
        /// </summary>
        public async Task<long> CountAsync(string condition, params DbParameter[] args)
        {
            var (_, value) = await CollectInnerAsync<long>("Count", Option.PrimaryKey, condition, args);
            return value;
        }

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            var (_, value) = await CollectInnerAsync<long>("Count", Option.PrimaryKey, convert.ConditionSql, convert.Parameters);
            return value;
        }

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public async Task<long> CountAsync(LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            var (_, value) = await CollectInnerAsync<long>("Count", Option.PrimaryKey, convert.ConditionSql, convert.Parameters);
            return value;
        }

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            var (_, value) = await CollectInnerAsync<long>("Count", Option.PrimaryKey,
                $"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
            return value;
        }


        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">查询表达式</param>
        /// <param name="args"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public async Task<long> CountAsync<TValue>(Expression<Func<TEntity, TValue>> field, string condition, params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var (_, value) = await CollectInnerAsync<long>("Count", expression.Member.Name, condition, args);
            return value;
        }

        /// <summary>
        ///     总数据量
        /// </summary>
        protected async Task<long> CountInnerAsync(string condition = null, DbParameter args = null)
        {
            var (_, value) = await CollectInnerAsync<long>("Count", Option.PrimaryKey, condition, args);
            return value;
        }

        #endregion

        #region Min

        /// <summary>
        ///     汇总
        /// </summary>
        public Task<(bool hase, object max)> MinAsyn(string field)
        {
            return CollectInnerAsync("Min", Option.FieldMap[field], null, null);
        }

        /// <summary>
        ///     汇总
        /// </summary>
        public Task<(bool hase, TValue max)> MinAsyn<TValue>(string field)
        {
            return CollectInnerAsync<TValue>("Min", Option.FieldMap[field], null, null);
        }
        /// <summary>
        ///     汇总
        /// </summary>
        public Task<(bool hase, object max)> MinAsyn(string field, string condition, params DbParameter[] args)
        {
            return CollectInnerAsync("Min", Option.FieldMap[field], condition, args);
        }

        /// <summary>
        ///     汇总
        /// </summary>
        public Task<(bool hase, TValue max)> MinAsyn<TValue>(string field, string condition, params DbParameter[] args)
        {
            return CollectInnerAsync<TValue>("Min", Option.FieldMap[field], condition, args);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">查询表达式</param>
        /// <param name="condition2">条件2，默认为空</param>
        public Task<(bool hase, TValue max)> MinAsyn<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> lambda,
            string condition2 = null)
        {
            var expression = (MemberExpression)field.Body;
            var convert = SqlBuilder.Compile(lambda);
            var condition = condition2 == null
                ? convert.ConditionSql
                : convert.ConditionSql == null
                    ? condition2
                    : $"({convert.ConditionSql}) AND ({condition2})";

            return CollectInnerAsync<TValue>("Min", Option.FieldMap[expression.Member.Name], condition, convert.Parameters);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public Task<(bool hase, TValue max)> MinAsyn<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> a,
            Expression<Func<TEntity, bool>> b)
        {
            var expression = (MemberExpression)field.Body;
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            return CollectInnerAsync<TValue>("Min", Option.FieldMap[expression.Member.Name],
                $"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());

        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">查询表达式</param>
        /// <param name="args"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public Task<(bool hase, TValue max)> MinAsyn<TValue>(Expression<Func<TEntity, TValue>> field, string condition,
            params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            return CollectInnerAsync<TValue>("Min", Option.FieldMap[expression.Member.Name], condition, args);
        }

        #endregion

        #region Max

        /// <summary>
        ///     汇总
        /// </summary>
        public Task<(bool hase, object max)> MaxAsyn(string field)
        {
            return CollectInnerAsync("Max", Option.FieldMap[field], null, null);
        }

        /// <summary>
        ///     汇总
        /// </summary>
        public Task<(bool hase, TValue max)> MaxAsyn<TValue>(string field)
        {
            return CollectInnerAsync<TValue>("Max", Option.FieldMap[field], null, null);
        }
        /// <summary>
        ///     汇总
        /// </summary>
        public Task<(bool hase, object max)> MaxAsyn(string field, string condition, params DbParameter[] args)
        {
            return CollectInnerAsync("Max", Option.FieldMap[field], condition, args);
        }

        /// <summary>
        ///     汇总
        /// </summary>
        public Task<(bool hase, TValue max)> MaxAsyn<TValue>(string field, string condition, params DbParameter[] args)
        {
            return CollectInnerAsync<TValue>("Max", Option.FieldMap[field], condition, args);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">查询表达式</param>
        /// <param name="condition2">条件2，默认为空</param>
        public Task<(bool hase, TValue max)> MaxAsyn<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> lambda,
            string condition2 = null)
        {
            var expression = (MemberExpression)field.Body;
            var convert = SqlBuilder.Compile(lambda);
            var condition = condition2 == null
                ? convert.ConditionSql
                : convert.ConditionSql == null
                    ? condition2
                    : $"({convert.ConditionSql}) AND ({condition2})";

            return CollectInnerAsync<TValue>("Max", Option.FieldMap[expression.Member.Name], condition, convert.Parameters);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public Task<(bool hase, TValue max)> MaxAsyn<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> a,
            Expression<Func<TEntity, bool>> b)
        {
            var expression = (MemberExpression)field.Body;
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            return CollectInnerAsync<TValue>("Max", Option.FieldMap[expression.Member.Name],
                $"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());

        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">查询表达式</param>
        /// <param name="args"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public Task<(bool hase, TValue max)> MaxAsyn<TValue>(Expression<Func<TEntity, TValue>> field, string condition,
            params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            return CollectInnerAsync<TValue>("Max", Option.FieldMap[expression.Member.Name], condition, args);
        }

        #endregion


        #region Sum

        /// <summary>
        ///     汇总
        /// </summary>
        public async Task<decimal> SumAsync(string field)
        {
            var (_, value) = await CollectInnerAsync("Sum", Option.FieldMap[field], null, null);
            return Convert.ToDecimal(value);
        }

        /// <summary>
        ///     汇总
        /// </summary>
        public async Task<decimal> SumAsync(string field, string condition, params DbParameter[] args)
        {
            var (_, value) = await CollectInnerAsync("Sum", Option.FieldMap[field], condition, args);
            return Convert.ToDecimal(value);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">查询表达式</param>
        /// <param name="condition2">条件2，默认为空</param>
        public async Task<decimal> SumAsync<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> lambda,
            string condition2 = null)
        {
            var expression = (MemberExpression)field.Body;
            var convert = SqlBuilder.Compile(lambda);
            var condition = condition2 == null
                ? convert.ConditionSql
                : convert.ConditionSql == null
                    ? condition2
                    : $"({convert.ConditionSql}) AND ({condition2})";
            var (_, value) = await CollectInnerAsync("Sum", Option.FieldMap[expression.Member.Name], condition, convert.Parameters);
            return Convert.ToDecimal(value);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public async Task<decimal> SumAsync<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> a,
            Expression<Func<TEntity, bool>> b)
        {
            var expression = (MemberExpression)field.Body;
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            var (_, value) = await CollectInnerAsync("Sum", Option.FieldMap[expression.Member.Name],
                $"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
            return Convert.ToDecimal(value);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">查询表达式</param>
        /// <param name="args"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public async Task<decimal> SumAsync<TValue>(Expression<Func<TEntity, TValue>> field, string condition,
            params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var (_, value) = await CollectInnerAsync("Sum", Option.FieldMap[expression.Member.Name], condition, args);
            return Convert.ToDecimal(value);
        }

        #endregion

        #region 实现

        /// <summary>
        ///     汇总
        /// </summary>
        protected async Task<(bool hase, object value)> CollectInnerAsync(string fun, string field, string condition, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateCollectSql(fun, field, condition);
            await using var connectionScope = await DataBase.CreateConnectionScope();
            return await DataBase.ExecuteScalarAsync(sql, args);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">查询表达式</param>
        /// <param name="args"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        protected async Task<(bool hase, TValue value)> CollectInnerAsync<TValue>(string fun, string field, string condition, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateCollectSql(fun, field, condition);
            await using var connectionScope = await DataBase.CreateConnectionScope();
            var (hase, value) = await DataBase.ExecuteScalarAsync(sql, args);
            return hase ? (true, (TValue)value) : ((bool hase, TValue max))(false, default);
        }

        #endregion

        #endregion

        #region 分页读取

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<List<TEntity>> PageDataAsync(int page, int limit)
        {
            return await PageDataAsync(page, limit, Option.PrimaryKey, false, null, null);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<List<TEntity>> PageDataAsync(int page, int limit, string condition, params DbParameter[] args)
        {
            return await PageDataAsync(page, limit, Option.PrimaryKey, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<List<TEntity>> PageDataAsync(int page, int limit, Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return await PageDataAsync(page, limit, Option.PrimaryKey, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<List<TEntity>> PageDataAsync(int page, int limit, LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return await PageDataAsync(page, limit, Option.PrimaryKey, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<List<TEntity>> LoadDataAsync(int page, int limit, string order, bool desc, string condition, params DbParameter[] args)
        {
            return await PageDataAsync(page, limit, order, desc, condition, args);
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
        public Task<List<TEntity>> PageDataAsync(int page, int limit, string order, bool desc, string condition, params DbParameter[] args)
        {
            if (page <= 0)
                page = 1;
            if (limit == 0)
                limit = 20;
            else if (limit == 9999)
                limit = -1;
            else if (limit > 500)
                limit = 500;
            return LoadPageDataAsync(page, limit, order, desc, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public Task<List<TEntity>> LoadDataAsync(int page, int limit, string order, string condition, params DbParameter[] args)
        {
            return PageDataAsync(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public Task<List<TEntity>> PageDataAsync(int page, int limit, string order, string condition, params DbParameter[] args)
        {
            return PageDataAsync(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public Task<List<TEntity>> PageDataAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> field,
            Expression<Func<TEntity, bool>> lambda)
        {
            return PageDataAsync(page, limit, field, false, lambda);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public Task<List<TEntity>> PageDataAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> field, bool desc,
            Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return PageDataAsync(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public Task<List<TEntity>> PageDataAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> field, bool desc,
            LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return PageDataAsync(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }



        protected async Task<List<TEntity>> LoadPageDataAsync(int page, int limit, string order, bool desc, string condition, DbParameter[] args)
        {
            var results = new List<TEntity>();
            var sql = SqlBuilder.CreatePageSql(page, limit, order, desc, condition);
            await using var connectionScope = await DataBase.CreateConnectionScope();
            await using var cmd = connectionScope.CreateCommand(sql, args);
            DataBase.TraceSql(cmd);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(await LoadEntityAsync(reader));
            }
            return results;
        }

        #endregion

        #region 分页读取

        /// <summary>
        ///     分页读取
        /// </summary>
        public Task<ApiPageData<TEntity>> PageAsync(int page, int limit)
        {
            return PageAsync(page, limit, Option.PrimaryKey, false, null, null);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public Task<ApiPageData<TEntity>> PageAsync(int page, int limit, Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return PageAsync(page, limit, Option.PrimaryKey, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public Task<ApiPageData<TEntity>> PageAsync(int page, int limit, LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return PageAsync(page, limit, Option.PrimaryKey, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public Task<ApiPageData<TEntity>> PageAsync(int page, int limit, string condition, params DbParameter[] args)
        {
            return PageAsync(page, limit, Option.PrimaryKey, false, condition, args);
        }


        /// <summary>
        ///     分页读取
        /// </summary>
        public Task<ApiPageData<TEntity>> PageAsync(int page, int limit, string order, string condition, params DbParameter[] args)
        {
            return PageAsync(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public Task<ApiPageData<TEntity>> PageAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> field,
            Expression<Func<TEntity, bool>> lambda)
        {
            return PageAsync(page, limit, field, false, lambda);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public Task<ApiPageData<TEntity>> PageAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> field, bool desc,
            Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return PageAsync(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public Task<ApiPageData<TEntity>> PageAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> field, bool desc,
            LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return PageAsync(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
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
        public Task<ApiPageData<TEntity>> PageAsync(int page, int limit, string order, bool desc, LambdaItem<TEntity> lambda)
        {
            var item = SqlBuilder.Compile(lambda);
            return LoadPageAsync(page, limit, order, desc, item.ConditionSql, item.Parameters);
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
        public Task<ApiPageData<TEntity>> PageAsync(int page, int limit, string order, bool desc, string condition,
            params DbParameter[] args)
        {
            return LoadPageAsync(page, limit, order, desc, condition, args);
        }


        protected async Task<ApiPageData<TEntity>> LoadPageAsync(int page, int limit, string order, bool desc, string condition,
            DbParameter[] args)
        {
            if (page <= 0)
                page = 1;
            if (limit == 0)
                limit = 20;
            else if (limit == 9999)
                limit = -1;
            else if (limit > 500)
                limit = 500;

            var count = await CountAsync(condition, args);
            var data = await PageDataAsync(page, limit, order, desc, condition, args);
            return new ApiPageData<TEntity>
            {
                Total = count,
                Rows = data,
                Page = page,
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
        public async Task<(bool hase, TField value)> LoadValueAsync<TField>(Expression<Func<TEntity, TField>> field, Expression<Func<TEntity, bool>> lambda)
        {
            var fn = GetPropertyName(field);
            var convert = SqlBuilder.Compile(lambda);
            var (hase, value) = await LoadValueAsync(fn, convert.ConditionSql, convert.Parameters);
            return !hase
                ? (false, default)
                : (true, value == null ? default : (TField)value);
        }

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="key">主键</param>
        /// <returns>内容</returns>
        public Task<(bool hase, TField value)> LoadValueAsync<TField, TKey>(Expression<Func<TEntity, TField>> field, TKey key)
        {
            return LoadValueAsync<TField>(GetPropertyName(field),
                SqlBuilder.FieldCondition(Option.PrimaryKey),
                ParameterCreater.CreateParameter(Option.PrimaryKey, key, SqlBuilder.GetDbType(Option.PrimaryKey)));
        }

        /// <summary>
        ///     读取多个值
        /// </summary>
        public async Task<(bool hase, TField value)> LoadValueAsync<TField>(string field, string condition, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateLoadValueSql(field, condition);
            await using var connectionScope = await DataBase.CreateConnectionScope();
            var (hase, value) = await DataBase.ExecuteScalarAsync(sql, args);
            return !hase
                ? (false, default)
                : (true, value == null ? default : (TField)value);
        }

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="lambda">条件</param>
        /// <returns>内容</returns>
        public async Task<List<TField>> LoadValuesAsync<TField>(Expression<Func<TEntity, TField>> fieldExpression,
            Expression<Func<TEntity, bool>> lambda)
        {
            var field = GetPropertyName(fieldExpression);
            var convert = SqlBuilder.Compile(lambda);

            var sql = SqlBuilder.CreateLoadValuesSql(field, convert);
            var values = new List<TField>();
            await using var connectionScope = await DataBase.CreateConnectionScope();
            {
                await using var cmd = connectionScope.CreateCommand(sql, convert.Parameters);
                DataBase.TraceSql(cmd);
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    if (await reader.IsDBNullAsync(0))
                        values.Add(await reader.GetFieldValueAsync<TField>(0));
                }
            }

            return values;
        }

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="condition">条件</param>
        /// <returns>内容</returns>
        public Task<List<TField>> LoadValuesAsync<TField>(Expression<Func<TEntity, TField>> fieldExpression, string condition)
        {
            return LoadValuesInnerAsync<TField>(GetPropertyName(fieldExpression), condition);
        }

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <returns>数据</returns>
        public Task<List<TField>> LoadValuesAsync<TField>(Expression<Func<TEntity, TField>> fieldExpression, string condition, DbParameter[] args)
        {
            return LoadValuesInnerAsync<TField>(GetPropertyName(fieldExpression), condition, args);
        }

        /// <summary>
        ///     读取值
        /// </summary>
        public async Task<(bool hase, object value)> LoadValueAsync(string field, string condition, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateLoadValueSql(field, condition);
            await using var connectionScope = await DataBase.CreateConnectionScope();
            return await DataBase.ExecuteScalarAsync(sql, args);
        }

        /// <summary>
        ///     读取多个值
        /// </summary>
        protected async Task<List<T>> LoadValuesInnerAsync<T>(string field, string condition, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateLoadValueSql(field, condition);
            var values = new List<T>();
            await using var connectionScope = await DataBase.CreateConnectionScope();
            await using var cmd = connectionScope.CreateCommand(sql, args);
            DataBase.TraceSql(cmd);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                if (await reader.IsDBNullAsync(0))
                    values.Add(reader.GetFieldValue<T>(0));
                else
                    values.Add(default);
            }
            return values;
        }
        #endregion

        #region 数据读取

        /*// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public Task<TEntity> LoadDataAsync(object id)
        {
            return LoadByPrimaryKeyAsync(id);
        }


        /// <summary>
        ///     全表读取
        /// </summary>
        public Task<List<TEntity>> LoadDataAsync()
        {
            return LoadDataInnerAsync(null, null, null);
        }


        /// <summary>
        ///     条件读取
        /// </summary>
        public Task<List<TEntity>> LoadDataAsync(string condition, params DbParameter[] args)
        {
            return LoadDataInnerAsync(condition, null, args);
        }*/

        /// <summary>
        ///     主键读取
        /// </summary>
        public Task<TEntity> LoadByPrimaryKeyAsync(object key)
        {
            return LoadFirstInnerAsync(SqlBuilder.PrimaryKeyCondition, ParameterCreater.CreateParameter(Option.PrimaryKey, key, SqlBuilder.GetDbType(Option.PrimaryKey)));
        }

        /// <summary>
        ///     主键读取
        /// </summary>
        public async Task<List<TEntity>> LoadByPrimaryKeiesAsync(IEnumerable keies)
        {
            var list = new List<TEntity>();
            var par = ParameterCreater.CreateParameter(Option.PrimaryKey, SqlBuilder.GetDbType(Option.PrimaryKey));
            foreach (var key in keies)
            {
                par.Value = key;
                list.Add(await LoadFirstInnerAsync(SqlBuilder.PrimaryKeyCondition, par));
            }

            return list;
        }


        /*// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public Task<TEntity> LoadFirstAsync(string condition = null)
        {
            return LoadFirstInnerAsync(condition);
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public Task<TEntity> LoadFirstAsync(string condition, params DbParameter[] args)
        {
            return LoadFirstInnerAsync(condition, args);
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public Task<TEntity> LoadFirstAsync(string foreignKey, object key)
        {
            return LoadFirstInnerAsync(SqlBuilder.FieldConditionSQL(foreignKey),
                ParameterCreater.CreateParameter(foreignKey, key, SqlBuilder.GetDbType(foreignKey)));
        }

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        public Task<TEntity> LoadLastAsync(string condition = null)
        {
            return LoadLastAsync(condition);
        }

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        public Task<TEntity> LoadLastAsync(string foreignKey, object key)
        {
            return LoadLastAsync(
                SqlBuilder.FieldConditionSQL(foreignKey),
                ParameterCreater.CreateParameter(foreignKey, key, SqlBuilder.GetDbType(foreignKey)));
        }*/

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public Task<List<TEntity>> LoadByForeignKeyAsync(string foreignKey, object key)
        {
            return LoadDataInnerAsync(SqlBuilder.FieldCondition(foreignKey), null,
                ParameterCreater.CreateParameter(foreignKey, key, SqlBuilder.GetDbType(foreignKey)));

        }

        /// <summary>
        ///     重新读取
        /// </summary>
        public async Task<bool> ReLoadAsync(TEntity entity)
        {
            return await ReLoadInnerAsync(entity);
        }

        /// <summary>
        ///     重新载入
        /// </summary>
        protected async Task<bool> ReLoadInnerAsync(TEntity entity)
        {
            await using var connectionScope = await DataBase.CreateConnectionScope();
            var para = ParameterCreater.CreateParameter(Option.PrimaryKey, Provider.EntityOperator.GetValue(entity, Option.PrimaryKey), SqlBuilder.GetDbType(Option.PrimaryKey));
            var sql = SqlBuilder.CreateLoadSql(SqlBuilder.PrimaryKeyCondition, null, null);
            await using var cmd = connectionScope.CreateCommand(sql, para);
            DataBase.TraceSql(cmd);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return false;
            await DataOperator.LoadEntity(reader, entity);
            return true;
        }

        #endregion

        #region 载入数据


        /// <summary>
        ///     读取首行
        /// </summary>
        protected async Task<TEntity> LoadFirstInnerAsync(string condition, params DbParameter[] args)
        {
            await using var connectionScope = await DataBase.CreateConnectionScope();
            var sql = SqlBuilder.CreateLoadSql(condition, null, "1");
            await using var cmd = connectionScope.CreateCommand(sql, args);
            DataBase.TraceSql(cmd);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;
            return await LoadEntityAsync(reader);
        }


        /// <summary>
        ///     读取尾行
        /// </summary>
        protected async Task<TEntity> LoadLastInnerAsync(string condition, params DbParameter[] args)
        {
            await using var connectionScope = await DataBase.CreateConnectionScope();
            var sql = SqlBuilder.CreateLoadSql(condition, SqlBuilder.OrderCode(true, Option.PrimaryKey), "1");
            await using var cmd = connectionScope.CreateCommand(sql, args);
            DataBase.TraceSql(cmd);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;
            return await LoadEntityAsync(reader);
        }

        /// <summary>
        ///     读取全部
        /// </summary>
        protected async Task<List<TEntity>> LoadDataInnerAsync(string condition, string orderBy, params DbParameter[] args)
        {
            var results = new List<TEntity>();
            await using var connectionScope = await DataBase.CreateConnectionScope();
            var sql = SqlBuilder.CreateLoadSql(condition, orderBy, null);
            await using var cmd = connectionScope.CreateCommand(sql, args);
            DataBase.TraceSql(cmd);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                results.Add(await LoadEntityAsync(reader));
            return results;
        }

        /// <summary>
        ///     读取全部(SQL语句是重写的,字段名称和顺序与设计时相同)
        /// </summary>
        public async Task<List<TEntity>> LoadDataBySqlAsync(string sql, DbParameter[] args)
        {
            var results = new List<TEntity>();
            await using var connectionScope = await DataBase.CreateConnectionScope();
            await using var cmd = connectionScope.CreateCommand(sql, args);
            DataBase.TraceSql(cmd);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(await LoadEntityAsync(reader));
            }
            return results;
        }

        /// <summary>
        ///     读取存储过程
        /// </summary>
        public async Task<List<TEntity>> LoadDataByProcedureAsync(string procedure, DbParameter[] args)
        {
            var results = new List<TEntity>();
            await using var connectionScope = await DataBase.CreateConnectionScope();
            await using var cmd = connectionScope.CreateCommand(procedure, args);
            cmd.CommandType = CommandType.StoredProcedure;
            DataBase.TraceSql(cmd);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(await LoadEntityAsync(reader));
            }
            return results;
        }

        /// <summary>
        ///     载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <returns>读取数据的实体</returns>
        protected async Task<TEntity> LoadEntityAsync(DbDataReader reader)
        {
            var entity = new TEntity();
            await DataOperator.LoadEntity(reader, entity);
            return entity;
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
        public async Task<bool> IsUniqueAsync<TValue>(Expression<Func<TEntity, TValue>> field, TValue val, Expression<Func<TEntity, bool>> condition)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            var convert = SqlBuilder.Compile(condition);

            convert.AddAndCondition(SqlBuilder.Condition(fieldName, "c_vl_"),
                ParameterCreater.CreateParameter("c_vl_", val, SqlBuilder.GetDbType(fieldName)));
            return !await ExistAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="key"></param>
        public async Task<bool> IsUniqueAsync<TValue>(Expression<Func<TEntity, TValue>> field, TValue val, object key)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            Debug.Assert(Option.FieldMap.ContainsKey(fieldName));
            return !await ExistAsync($"({SqlBuilder.Condition(fieldName, "c_vl_")} AND {SqlBuilder.FieldCondition(Option.PrimaryKey, "<>")}"
                , ParameterCreater.CreateParameter("c_vl_", val)
                , ParameterCreater.CreateParameter(Option.PrimaryKey, key, SqlBuilder.GetDbType(Option.PrimaryKey)));
        }

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        public async Task<bool> IsUniqueAsync<TValue>(Expression<Func<TEntity, TValue>> field, TValue val)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            return !await ExistAsync(SqlBuilder.Condition(fieldName, "c_vl_"),
                ParameterCreater.CreateParameter("c_vl_", val, SqlBuilder.GetDbType(fieldName)));
        }

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="condition"></param>
        public async Task<bool> IsUniqueAsync<TValue>(string field, TValue val, Expression<Func<TEntity, bool>> condition)
        {
            var convert = SqlBuilder.Compile(condition);
            convert.AddAndCondition(SqlBuilder.Condition(field, "c_vl_"),
                ParameterCreater.CreateParameter("c_vl_", val, SqlBuilder.GetDbType(field)));
            return !await ExistAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="key"></param>
        public async Task<bool> IsUniqueAsync(string field, string val, object key)
        {
            return !await ExistAsync($"({SqlBuilder.Condition(field, "c_vl_")} AND {SqlBuilder.FieldCondition(Option.PrimaryKey, "<>")}"
                , ParameterCreater.CreateParameter("c_vl_", val)
                , ParameterCreater.CreateParameter(Option.PrimaryKey, key, SqlBuilder.GetDbType(Option.PrimaryKey)));
        }

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        public async Task<bool> IsUniqueAsync(string field, string val)
        {
            return !await ExistAsync(SqlBuilder.Condition(field, "c_vl_"), ParameterCreater.CreateParameter("c_vl_", val));
        }

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="key"></param>
        public async Task<bool> IsUniqueAsync<TValue>(string fieldName, object val, string key)
        {
            return !await ExistAsync($"({SqlBuilder.Condition(fieldName, "c_vl_")} AND {SqlBuilder.FieldCondition(Option.PrimaryKey, "<>")}"
                , ParameterCreater.CreateParameter("c_vl_", val, SqlBuilder.GetDbType(fieldName))
                , ParameterCreater.CreateParameter(Option.PrimaryKey, key));
        }

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="key"></param>
        public async Task<bool> IsUniqueAsync<TValue>(string fieldName, string val)
        {
            return !await ExistAsync(SqlBuilder.Condition(fieldName, "c_vl_"), ParameterCreater.CreateParameter("c_vl_", val));
        }
        #endregion
    }
}