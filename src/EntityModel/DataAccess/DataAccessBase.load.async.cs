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
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace Agebull.EntityModel.Common
{
    partial class DataAccess<TEntity>
    {
        #region 首行

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <returns>如果有载入首行,否则返回空</returns>
        public async Task<TEntity> FirstAsync()
        {
            return await LoadFirstAsync();
        }


        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<TEntity> FirstAsync(LambdaItem<TEntity> lambda)
        {
            var convert = Compile(lambda);
            return await LoadFirstAsync(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<TEntity> FirstOrDefaultAsync(LambdaItem<TEntity> lambda)
        {
            var convert = Compile(lambda);

            return await LoadFirstAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <returns>如果有载入首行,否则返回空</returns>
        public async Task<TEntity> FirstOrDefaultAsync()
        {
            return await LoadFirstAsync();
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);

            return await LoadFirstAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return await LoadFirstAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters));//SQL
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public async Task<TEntity> FirstAsync(string condition, DbParameter[] args)
        {
            return await LoadFirstAsync(condition, args);
        }


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);

            return await LoadFirstAsync(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return await LoadFirstAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters));//SQL
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="args">参数</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public async Task<TEntity> FirstOrDefaultAsync(string condition, DbParameter[] args)
        {
            return await LoadFirstAsync(condition, args);
        }

        #endregion

        #region 尾行

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public async Task<TEntity> LastAsync()
        {
            return await LoadLastAsync();
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public async Task<TEntity> LastOrDefaultAsync()
        {
            return await LoadLastAsync();
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public async Task<TEntity> LastAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);

            return await LoadLastAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public async Task<TEntity> LastAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return await LoadLastAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters));//SQL
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public async Task<TEntity> LastAsync(string condition, DbParameter[] args)
        {
            return await LoadLastAsync(condition, args);
        }


        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public async Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);

            return await LoadLastAsync(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public async Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return await LoadLastAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters));//SQL
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="args">参数</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public async Task<TEntity> LastOrDefaultAsync(string condition, DbParameter[] args)
        {
            return await LoadLastAsync(condition, args);
        }

        #endregion

        #region All

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>数据</returns>
        public async Task<List<TEntity>> AllAsync()
        {
            return await LoadDataInnerAsync();
        }


        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>数据</returns>
        public async Task<List<TEntity>> AllAsync(string condition, DbParameter[] args)
        {
            return await LoadDataInnerAsync(condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<List<TEntity>> AllAsync(LambdaItem<TEntity> lambda)
        {
            var convert = Compile(lambda);
            return await LoadPageDataAsync(1, -1, SqlBuilder.SqlOption.KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<List<TEntity>> AllAsync<TField>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TField>> orderBy,
            bool desc)
        {
            var convert = Compile(lambda);
            return await LoadPageDataAsync(1, -1, GetPropertyName(orderBy), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>数据</returns>
        public async Task<List<TEntity>> AllAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return await LoadDataInnerAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }


        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <param name="orderBys">排序</param>
        /// <returns>数据</returns>
        public async Task<List<TEntity>> AllAsync(LambdaItem<TEntity> lambda, params string[] orderBys)
        {
            var convert = Compile(lambda);
            return await LoadDataInnerAsync(convert.ConditionSql, convert.Parameters,
                orderBys.Length == 0 ? null : string.Join(",", orderBys));
        }

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <param name="orderBys">排序</param>
        /// <returns>数据</returns>
        public async Task<List<TEntity>> AllAsync(Expression<Func<TEntity, bool>> lambda, params string[] orderBys)
        {
            var convert = Compile(lambda);
            return await LoadDataInnerAsync(convert.ConditionSql, convert.Parameters,
                orderBys.Length == 0 ? null : string.Join(",", orderBys));
        }

        #endregion

        #region 聚合函数支持

        #region Collect

        /// <summary>
        ///     汇总方法
        /// </summary>
        public Task<object> CollectAsync(string fun, string field)
        {
            return CollectInnerAsync(fun, SqlBuilder.SqlOption.FieldMap[field], null, null);
        }

        /// <summary>
        ///     汇总方法
        /// </summary>
        public Task<object> CollectAsync<TValue>(string fun, Expression<Func<TEntity, TValue>> field)
        {
            var expression = (MemberExpression)field.Body;
            return CollectInnerAsync(fun, SqlBuilder.SqlOption.FieldMap[expression.Member.Name], null, null);
        }

        /// <summary>
        ///     汇总方法
        /// </summary>
        public Task<object> CollectAsync(string fun, string field, string condition, params DbParameter[] args)
        {
            return CollectInnerAsync(fun, SqlBuilder.SqlOption.FieldMap[field], condition, args);
        }


        /// <summary>
        ///     汇总方法
        /// </summary>
        public Task<object> CollectAsync(string fun, string field, Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            return CollectInnerAsync(fun, field, convert.ConditionSql, convert.Parameters);
        }

        #endregion

        #region Exist

        /// <summary>
        ///     是否存在数据
        /// </summary>
        public Task<bool> ExistAsync()
        {
            return AnyAsync();
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        public Task<bool> ExistAsync(string condition, params DbParameter[] args)
        {
            return AnyAsync(condition, args);
        }

        /// <summary>
        ///     是否存在此主键的数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>是否存在数据</returns>
        public Task<bool> ExistPrimaryKeyAsync<T>(T id)
        {
            return ExistInnerAsync(SqlBuilder.PrimaryKeyConditionSQL, ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, id, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
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
            var obj = await CollectInnerAsync("Count", "*", condition, args);
            return obj == DBNull.Value || obj == null ? 0L : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            var obj = await CollectInnerAsync("Count", "*", convert.ConditionSql, convert.Parameters);
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public async Task<long> CountAsync(LambdaItem<TEntity> lambda)
        {
            var convert = Compile(lambda);
            var obj = await CollectInnerAsync("Count", "*", convert.ConditionSql, convert.Parameters);
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            var obj = await CollectInnerAsync("Count", "*",
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
        public async Task<long> CountAsync<TValue>(Expression<Func<TEntity, TValue>> field, string condition, params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = await CollectInnerAsync("Count", SqlBuilder.SqlOption.FieldMap[expression.Member.Name], condition, args);
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }
        #endregion


        #region Any

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
            var convert = Compile(lambda);
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
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return await ExistInnerAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }

        #endregion

        #region Min

        /// <summary>
        ///     汇总
        /// </summary>
        public async Task<decimal?> MinAsyn(string field)
        {
            var obj = await CollectInnerAsync("Min", SqlBuilder.SqlOption.FieldMap[field], null, null);
            if (obj == DBNull.Value || obj == null)
                return null;
            return Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     汇总
        /// </summary>
        public async Task<decimal?> MinAsyn(string field, string condition, params DbParameter[] args)
        {
            var obj = await CollectInnerAsync("Min", SqlBuilder.SqlOption.FieldMap[field], condition, args);
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
        public async Task<decimal?> MinAsyn<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> lambda,
            string condition2 = null)
        {
            var expression = (MemberExpression)field.Body;
            var convert = Compile(lambda);
            var condition = condition2 == null
                ? convert.ConditionSql
                : convert.ConditionSql == null
                    ? condition2
                    : $"({convert.ConditionSql}) AND ({condition2})";

            var obj = await CollectInnerAsync("Min", SqlBuilder.SqlOption.FieldMap[expression.Member.Name], condition, convert.Parameters);
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
        public async Task<decimal?> MinAsyn<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> a,
            Expression<Func<TEntity, bool>> b)
        {
            var expression = (MemberExpression)field.Body;
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            var obj = await CollectInnerAsync("Min", SqlBuilder.SqlOption.FieldMap[expression.Member.Name],
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
        public async Task<decimal?> MinAsyn<TValue>(Expression<Func<TEntity, TValue>> field, string condition,
            params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = await CollectInnerAsync("Min", SqlBuilder.SqlOption.FieldMap[expression.Member.Name], condition, args);
            if (obj == DBNull.Value || obj == null)
                return null;
            return Convert.ToDecimal(obj);
        }

        #endregion

        #region Max

        /// <summary>
        ///     汇总
        /// </summary>
        public async Task<decimal?> MaxAsyn(string field)
        {
            var obj = await CollectInnerAsync("Max", SqlBuilder.SqlOption.FieldMap[field], null, null);
            if (obj == DBNull.Value || obj == null)
                return null;
            return Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     汇总
        /// </summary>
        public async Task<decimal?> MaxAsyn(string field, string condition, params DbParameter[] args)
        {
            var obj = await CollectInnerAsync("Max", SqlBuilder.SqlOption.FieldMap[field], condition, args);
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
        public async Task<decimal?> MaxAsyn<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> lambda,
            string condition2 = null)
        {
            var expression = (MemberExpression)field.Body;
            var convert = Compile(lambda);
            var condition = condition2 == null
                ? convert.ConditionSql
                : convert.ConditionSql == null
                    ? condition2
                    : $"({convert.ConditionSql}) AND ({condition2})";

            var obj = await CollectInnerAsync("Max", SqlBuilder.SqlOption.FieldMap[expression.Member.Name], condition, convert.Parameters);
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
        public async Task<decimal?> MaxAsyn<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> a,
            Expression<Func<TEntity, bool>> b)
        {
            var expression = (MemberExpression)field.Body;
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            var obj = await CollectInnerAsync("Max", SqlBuilder.SqlOption.FieldMap[expression.Member.Name],
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
        public async Task<decimal?> MaxAsyn<TValue>(Expression<Func<TEntity, TValue>> field, string condition,
            params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = await CollectInnerAsync("Max", SqlBuilder.SqlOption.FieldMap[expression.Member.Name], condition, args);
            if (obj == DBNull.Value || obj == null)
                return null;
            return Convert.ToDecimal(obj);
        }

        #endregion


        #region Sum

        /// <summary>
        ///     汇总
        /// </summary>
        public async Task<decimal> SumAsync(string field)
        {
            var obj = await CollectInnerAsync("Sum", SqlBuilder.SqlOption.FieldMap[field], null, null);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     汇总
        /// </summary>
        public async Task<decimal> SumAsync(string field, string condition, params DbParameter[] args)
        {
            var obj = await CollectInnerAsync("Sum", SqlBuilder.SqlOption.FieldMap[field], condition, args);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
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
            var convert = Compile(lambda);
            var condition = condition2 == null
                ? convert.ConditionSql
                : convert.ConditionSql == null
                    ? condition2
                    : $"({convert.ConditionSql}) AND ({condition2})";
            var obj = await CollectInnerAsync("Sum", SqlBuilder.SqlOption.FieldMap[expression.Member.Name], condition, convert.Parameters);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
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
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            var obj = await CollectInnerAsync("Sum", SqlBuilder.SqlOption.FieldMap[expression.Member.Name],
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
        public async Task<decimal> SumAsync<TValue>(Expression<Func<TEntity, TValue>> field, string condition,
            params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = await CollectInnerAsync("Sum", SqlBuilder.SqlOption.FieldMap[expression.Member.Name], condition, args);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }

        #endregion

        #region 实现

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
            var obj = await CollectInnerAsync("Count", "*", condition, args);
            return obj != DBNull.Value && obj != null && Convert.ToInt64(obj) > 0;
        }

        /// <summary>
        ///     总数据量
        /// </summary>
        protected async Task<long> CountInnerAsync(string condition = null, DbParameter args = null)
        {
            var obj = await CollectInnerAsync("Count", "*", condition, args);
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     总数据量
        /// </summary>
        protected async Task<object> CollectInnerAsync(string fun, string field, string condition, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateCollectSql(fun, field, condition);
            using var connectionScope = DataBase.CreateConnectionScope();
            {
                return await DataBase.ExecuteScalarAsync(sql, args);
            }
        }

        #endregion

        #endregion

        #region 分页读取

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<List<TEntity>> PageDataAsync(int page, int limit)
        {
            return await PageDataAsync(page, limit, SqlBuilder.SqlOption.KeyField, false, null, null);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<List<TEntity>> PageDataAsync(int page, int limit, string condition, params DbParameter[] args)
        {
            return await PageDataAsync(page, limit, SqlBuilder.SqlOption.KeyField, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<List<TEntity>> PageDataAsync(int page, int limit, Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            return await PageDataAsync(page, limit, SqlBuilder.SqlOption.KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<List<TEntity>> PageDataAsync(int page, int limit, LambdaItem<TEntity> lambda)
        {
            var convert = Compile(lambda);
            return await PageDataAsync(page, limit, SqlBuilder.SqlOption.KeyField, false, convert.ConditionSql, convert.Parameters);
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
        public async Task<List<TEntity>> PageDataAsync(int page, int limit, string order, bool desc, string condition, params DbParameter[] args)
        {
            if (page <= 0)
                page = 1;
            if (limit == 0)
                limit = 20;
            else if (limit == 9999)
                limit = -1;
            else if (limit > 500)
                limit = 500;
            return await LoadPageDataAsync(page, limit, order, desc, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<List<TEntity>> LoadDataAsync(int page, int limit, string order, string condition, params DbParameter[] args)
        {
            return await PageDataAsync(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<List<TEntity>> PageDataAsync(int page, int limit, string order, string condition, params DbParameter[] args)
        {
            return await PageDataAsync(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<List<TEntity>> PageDataAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> field,
            Expression<Func<TEntity, bool>> lambda)
        {
            return await PageDataAsync(page, limit, field, false, lambda);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<List<TEntity>> PageDataAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> field, bool desc,
            Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            return await PageDataAsync(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<List<TEntity>> PageDataAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> field, bool desc,
            LambdaItem<TEntity> lambda)
        {
            var convert = Compile(lambda);
            return await PageDataAsync(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }



        private async Task<List<TEntity>> LoadPageDataAsync(int page, int limit, string order, bool desc, string condition, DbParameter[] args)
        {
            var results = new List<TEntity>();
            var sql = SqlBuilder.CreatePageSql(page, limit, order, desc, condition);
            //await using (TransactionScope.CreateScope(DataBase))
            {
                using var connectionScope = DataBase.CreateConnectionScope();
                await using var cmd = CommandCreater.CreateCommand(connectionScope, sql, args);
                await using var reader = (DbDataReader)(await cmd.ExecuteReaderAsync());
                while (await reader.ReadAsync())
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
        public async Task<ApiPageData<TEntity>> PageAsync(int page, int limit)
        {
            return await PageAsync(page, limit, SqlBuilder.SqlOption.KeyField, false, null, null);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<ApiPageData<TEntity>> PageAsync(int page, int limit, Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            return await PageAsync(page, limit, SqlBuilder.SqlOption.KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<ApiPageData<TEntity>> PageAsync(int page, int limit, LambdaItem<TEntity> lambda)
        {
            var convert = Compile(lambda);
            return await PageAsync(page, limit, SqlBuilder.SqlOption.KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<ApiPageData<TEntity>> PageAsync(int page, int limit, string condition, params DbParameter[] args)
        {
            return await PageAsync(page, limit, SqlBuilder.SqlOption.KeyField, false, condition, args);
        }


        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<ApiPageData<TEntity>> PageAsync(int page, int limit, string order, string condition, params DbParameter[] args)
        {
            return await PageAsync(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<ApiPageData<TEntity>> PageAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> field,
            Expression<Func<TEntity, bool>> lambda)
        {
            return await PageAsync(page, limit, field, false, lambda);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<ApiPageData<TEntity>> PageAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> field, bool desc,
            Expression<Func<TEntity, bool>> lambda)
        {
            var convert = Compile(lambda);
            return await PageAsync(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public async Task<ApiPageData<TEntity>> PageAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> field, bool desc,
            LambdaItem<TEntity> lambda)
        {
            var convert = Compile(lambda);
            return await PageAsync(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
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
        public async Task<ApiPageData<TEntity>> PageAsync(int page, int limit, string order, bool desc, string condition,
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
            return await LoadPageAsync(page, limit, order, desc, condition, args);
        }

        private async Task<ApiPageData<TEntity>> LoadPageAsync(int page, int limit, string order, bool desc, string condition,
            DbParameter[] args)
        {
            var data = await PageDataAsync(page, limit, order, desc, condition, args);
            var count = await CountAsync(condition, args);
            return new ApiPageData<TEntity>
            {
                RowCount = (int)count,
                Rows = data,
                PageIndex = page,
                PageSize = limit,
                PageCount = (int)(count / limit + ((count % limit) > 0 ? 1 : 0))
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
        public async Task<TField> LoadValueAsync<TField>(Expression<Func<TEntity, TField>> field, Expression<Func<TEntity, bool>> lambda)
        {
            var fn = GetPropertyName(field);
            var convert = Compile(lambda);
            var val = await LoadValueInnerAsync(fn, convert.ConditionSql, convert.Parameters);
            return val == null || val == DBNull.Value ? default : (TField)val;
        }

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="key">主键</param>
        /// <returns>内容</returns>
        public async Task<object> ReadAsync<TField, TKey>(Expression<Func<TEntity, TField>> field, TKey key)
        {
            var fn = GetPropertyName(field);
            var vl = await LoadValueInnerAsync(fn, SqlBuilder.FieldConditionSQL(SqlBuilder.SqlOption.KeyField), ParameterCreater.CreateFieldParameter(SqlBuilder.SqlOption.KeyField, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField), key));
            return vl == DBNull.Value || vl == null ? default(TField) : vl;
        }

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="key">主键</param>
        /// <returns>内容</returns>
        public async Task<TField> LoadValueAsync<TField, TKey>(Expression<Func<TEntity, TField>> field, TKey key)
        {
            var fn = GetPropertyName(field);
            var vl = await LoadValueInnerAsync(fn, SqlBuilder.FieldConditionSQL(SqlBuilder.SqlOption.KeyField), ParameterCreater.CreateFieldParameter(SqlBuilder.SqlOption.KeyField, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField), key));
            return vl == DBNull.Value || vl == null ? default : (TField)vl;
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
            var convert = Compile(lambda);
            Debug.Assert(SqlBuilder.SqlOption.FieldMap.ContainsKey(field));
            var sql = SqlBuilder.CreateLoadValuesSql(field, convert);
            var values = new List<TField>();
            using var connectionScope = DataBase.CreateConnectionScope();
            {
                await using var cmd = CommandCreater.CreateCommand(connectionScope, sql, convert.Parameters);
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
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
        /// <param name="fieldExpression">字段</param>
        /// <param name="condition">条件</param>
        /// <returns>内容</returns>
        public async Task<List<TField>> LoadValuesAsync<TField>(Expression<Func<TEntity, TField>> fieldExpression, string condition)
        {
            var field = GetPropertyName(fieldExpression);

            var result = await LoadValuesInnerAsync(field, condition);
            return result.Count == 0 ? new List<TField>() : result.Select(p => (TField)p).ToList();
        }

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <returns>数据</returns>
        public async Task<List<TField>> LoadValuesAsync<TField>(Expression<Func<TEntity, TField>> fieldExpression, string condition, DbParameter[] args)
        {
            var field = GetPropertyName(fieldExpression);

            var result = await LoadValuesInnerAsync(field, condition, args);
            return result.Count == 0 ? new List<TField>() : result.Select(p => (TField)p).ToList();
        }

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="parse">转换数据类型方法</param>
        /// <param name="lambda">条件</param>
        /// <returns>内容</returns>
        public async Task<List<TField>> LoadValuesAsync<TField>(Expression<Func<TEntity, TField>> fieldExpression,
            Func<object, TField> parse, Expression<Func<TEntity, bool>> lambda)
        {
            var field = GetPropertyName(fieldExpression);
            var convert = Compile(lambda);
            var result = await LoadValuesInnerAsync(field, convert.ConditionSql, convert.Parameters);
            return result.Count == 0 ? new List<TField>() : result.Select(parse).ToList();
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public Task<object> LoadValueAsync(string field, string condition, params DbParameter[] args)
        {
            return LoadValueInnerAsync(field, condition, args);
        }

        /// <summary>
        ///     读取值
        /// </summary>
        protected async Task<object> LoadValueInnerAsync(string field, string condition, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateLoadValueSql(field, condition);
            using var connectionScope = DataBase.CreateConnectionScope();
            return await DataBase.ExecuteScalarAsync(sql, args);
        }


        /// <summary>
        ///     读取多个值
        /// </summary>
        protected async Task<List<object>> LoadValuesInnerAsync(string field, string condition, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateLoadValueSql(field, condition);
            var values = new List<object>();
            using var connectionScope = DataBase.CreateConnectionScope();
            {
                await using var cmd = CommandCreater.CreateCommand(connectionScope, sql, args);
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
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
        public Task<List<TEntity>> LoadDataAsync(MulitCondition condition)
        {
            if (condition == null || string.IsNullOrEmpty(condition.Condition))
                return Task.FromResult(new List<TEntity>());
            if (condition.Parameters == null)
                return LoadDataInnerAsync(condition.Condition);
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
            return LoadDataInnerAsync(condition.Condition, args.ToArray());
        }


        /// <summary>
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
            return LoadDataInnerAsync();
        }


        /// <summary>
        ///     条件读取
        /// </summary>
        public Task<List<TEntity>> LoadDataAsync(string condition, params DbParameter[] args)
        {
            return LoadDataInnerAsync(condition, args);
        }

        /// <summary>
        ///     主键读取
        /// </summary>
        public virtual Task<TEntity> LoadByPrimaryKeyAsync(object key)
        {
            return LoadFirstInnerAsync(SqlBuilder.PrimaryKeyConditionSQL, ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, key, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
        }

        /// <summary>
        ///     主键读取
        /// </summary>
        public async Task<List<TEntity>> LoadByPrimaryKeiesAsync(IEnumerable keies)
        {
            var list = new List<TEntity>();
            var par = ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField));
            foreach (var key in keies)
            {
                par.Value = key;
                list.Add(await LoadFirstInnerAsync(SqlBuilder.PrimaryKeyConditionSQL, par));
            }

            return list;
        }


        /// <summary>
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
            return LoadFirstInnerAsync(SqlBuilder.FieldConditionSQL(foreignKey), ParameterCreater.CreateFieldParameter(foreignKey, SqlBuilder.GetDbType(foreignKey), key));
        }

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        public Task<TEntity> LoadLastAsync(string condition = null)
        {
            return LoadLastInnerAsync(condition);
        }

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        public Task<TEntity> LoadLastAsync(string condition, params DbParameter[] args)
        {
            return LoadLastInnerAsync(condition, args);
        }

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        public Task<TEntity> LoadLastAsync(string foreignKey, object key)
        {
            return LoadLastInnerAsync(SqlBuilder.FieldConditionSQL(foreignKey), ParameterCreater.CreateFieldParameter(foreignKey, SqlBuilder.GetDbType(foreignKey), key));
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public Task<List<TEntity>> LoadByForeignKeyAsync(string foreignKey, object key)
        {
            return LoadDataInnerAsync(SqlBuilder.FieldConditionSQL(foreignKey), ParameterCreater.CreateFieldParameter(foreignKey, SqlBuilder.GetDbType(foreignKey), key));
        }

        /// <summary>
        ///     重新读取
        /// </summary>
        public async Task<bool> ReLoadAsync(TEntity entity)
        {
            bool res;
            using var connectionScope = DataBase.CreateConnectionScope();
            res = await ReLoadInnerAsync(entity);
            entity.OnStatusChanged(NotificationStatusType.Refresh);
            return res;
        }

        #endregion

        #region 载入数据


        /// <summary>
        ///     读取首行
        /// </summary>
        protected Task<TEntity> LoadFirstInnerAsync(string condition = null, DbParameter args = null)
        {
            return LoadFirstInnerAsync(condition, args == null ? new DbParameter[0] : new[] { args });
        }

        /// <summary>
        ///     读取首行
        /// </summary>
        protected async Task<TEntity> LoadFirstInnerAsync(string condition, DbParameter[] args)
        {
            TEntity entity = null;
            using var connectionScope = DataBase.CreateConnectionScope();
            {
                using var cmd = CreateLoadCommand(connectionScope, condition, args);
                await using var reader = cmd.ExecuteReader();
                if (await reader.ReadAsync())
                    entity = LoadEntity(reader);
                if (entity != null)
                    entity = EntityLoaded(entity);
            }

            return entity;
        }


        /// <summary>
        ///     读取尾行
        /// </summary>
        protected Task<TEntity> LoadLastInnerAsync(string condition = null, DbParameter args = null)
        {
            return LoadLastInnerAsync(condition, args == null ? new DbParameter[0] : new[] { args });
        }

        /// <summary>
        ///     读取尾行
        /// </summary>
        protected async Task<TEntity> LoadLastInnerAsync(string condition, DbParameter[] args)
        {
            TEntity entity = null;
            using var connectionScope = DataBase.CreateConnectionScope();
            {
                using var cmd = CreateLoadCommand(connectionScope, SqlBuilder.SqlOption.KeyField, true, condition, args);
                await using var reader = cmd.ExecuteReader();
                while (await reader.ReadAsync())
                    entity = LoadEntity(reader);

                if (entity != null)
                    entity = EntityLoaded(entity);
            }
            return entity;
        }

        /// <summary>
        ///     读取全部
        /// </summary>
        protected Task<List<TEntity>> LoadDataInnerAsync(string condition = null, DbParameter args = null)
        {
            return LoadDataInnerAsync(condition, args == null ? new DbParameter[0] : new[] { args }, null);
        }

        /// <summary>
        ///     读取全部
        /// </summary>
        protected Task<List<TEntity>> LoadDataInnerAsync(string condition, DbParameter[] args)
        {
            return LoadDataInnerAsync(condition, args, null);
        }

        /// <summary>
        ///     读取全部
        /// </summary>
        protected async Task<List<TEntity>> LoadDataInnerAsync(string condition, DbParameter[] args, string orderBy)
        {
            var results = new List<TEntity>();
            using var connectionScope = DataBase.CreateConnectionScope();
            {
                await using (var cmd = CreateLoadCommand(connectionScope, condition, orderBy, args))
                {
                    await using var reader = cmd.ExecuteReader();
                    while (await reader.ReadAsync())
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
        protected async Task<List<TEntity>> LoadDataBySqlAsync(string sql, DbParameter[] args)
        {
            var results = new List<TEntity>();
            //await using (TransactionScope.CreateScope(DataBase))
            {
                using var connectionScope = DataBase.CreateConnectionScope();
                await using var cmd = CommandCreater.CreateCommand(connectionScope, sql, args);
                var task = cmd.ExecuteReaderAsync();
                task.Wait();
                await using var reader = (DbDataReader)task.Result;
                while (await reader.ReadAsync())
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
        public async Task<List<TEntity>> LoadDataByProcedureAsync(string procedure, DbParameter[] args)
        {
            var results = new List<TEntity>();
            //await using (TransactionScope.CreateScope(DataBase))
            {
                using var connectionScope = DataBase.CreateConnectionScope();
                await using var cmd = CommandCreater.CreateCommand(connectionScope, procedure, args);
                cmd.CommandType = CommandType.StoredProcedure;
                var res = await cmd.ExecuteReaderAsync();
                await using var reader = (DbDataReader)res;
                while (await reader.ReadAsync())
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
        public async Task<bool> IsUniqueAsync<TValue>(Expression<Func<TEntity, TValue>> field, object val, Expression<Func<TEntity, bool>> condition)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            var convert = Compile(condition);

            convert.AddAndCondition(SqlBuilder.Condition(fieldName, "c_vl_"), ParameterCreater.CreateDbParameter("c_vl_", fieldName, val));
            return !await ExistAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="key"></param>
        public async Task<bool> IsUniqueAsync<TValue>(Expression<Func<TEntity, TValue>> field, object val, object key)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            Debug.Assert(SqlBuilder.SqlOption.FieldMap.ContainsKey(fieldName));
            return !await ExistAsync($"({SqlBuilder.Condition(fieldName, "c_vl_")} AND {SqlBuilder.FieldConditionSQL(SqlBuilder.SqlOption.KeyField, "<>")}"
                , ParameterCreater.CreateDbParameter("c_vl_", fieldName, val)
                , ParameterCreater.CreateParameter(SqlBuilder.SqlOption.KeyField, key, SqlBuilder.GetDbType(SqlBuilder.SqlOption.KeyField)));
        }

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        public async Task<bool> IsUniqueAsync<TValue>(Expression<Func<TEntity, TValue>> field, object val)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            Debug.Assert(SqlBuilder.SqlOption.FieldMap.ContainsKey(fieldName));
            return !await ExistAsync(SqlBuilder.Condition(fieldName, "c_vl_"),ParameterCreater.CreateDbParameter("c_vl_", fieldName, val));
        }

        #endregion
    }
}