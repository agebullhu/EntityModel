// // /*****************************************************
// // 作者:agebull
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2018.10.07
// // *****************************************************/

#region 引用

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     Sql实体访问类
    /// </summary>
    public interface IDataAccess : IConfig
    {
        /// <summary>
        ///     自动数据连接对象
        /// </summary>
        IDataBase DataBase
        {
            get;
            set;
        }

        /// <summary>
        ///     无懒构造数据库对象
        /// </summary>
        IDataBase OriDataBase { get; }

        /// <summary>
        /// 数据结构
        /// </summary>
        EntitySturct DataSturct { get; }

        /// <summary>
        /// Sql配置信息
        /// </summary>
        DataAccessOption Option { get; }


        /// <summary>
        /// Sql语句构造器
        /// </summary>
        ISqlBuilder SqlBuilder { get; }

    }

    /// <summary>
    ///     Sql实体访问类
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    public interface IDataAccess<TEntity> : IDataAccess
    {
        #region 聚合函数支持

        /// <summary>
        ///     是否存在数据
        /// </summary>
        Task<bool> ExistAsync();

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        Task<bool> AnyAsync();

        /// <summary>
        ///     是否存在此主键的数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>是否存在数据</returns>
        Task<bool> ExistPrimaryKeyAsync<T>(T id);

        /// <summary>
        ///     汇总
        /// </summary>
        Task<decimal> SumAsync(string field);

        /// <summary>
        ///     总数
        /// </summary>
        Task<long> CountAsync();

        /// <summary>
        ///     总数
        /// </summary>
        Task<long> CountAsync(string condition, params DbParameter[] args);


        #endregion

        #region 条件更新

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">更新字段</param>
        /// <param name="value">更新值</param>
        /// <param name="conditionFiles">条件包含的字段</param>
        /// <param name="conditionValues">条件包含的值</param>
        /// <returns>更新行数</returns>
        /// <remarks>
        /// 条件中使用AND组合,均为等于
        /// </remarks>
        Task SaveValueAsync(string field, object value, string[] conditionFiles, object[] conditionValues);

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="key">条件主键</param>
        /// <returns>更新行数</returns>
        Task<int> SetValueAsync(string field, object value, object key);

        /// <summary>
        ///     设计字段按自定义表达式更新值
        /// </summary>
        /// <param name="valueExpression">值的SQL方式</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        Task<int> SetCoustomValueAsync<TKey>(string valueExpression, TKey key);

        #endregion
        #region 读

        #region 首行


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <returns>如果有载入首行,否则返回空</returns>
        Task<TEntity> FirstOrDefaultAsync();

        /// <summary>
        ///     载入首行
        /// </summary>
        Task<TEntity> FirstOrDefaultAsync(LambdaItem<TEntity> lambda);


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> lambda);


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b);

        #endregion

        #region 尾行

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <returns>如果有载入尾行,否则返回空</returns>
        Task<TEntity> LastAsync();

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <returns>如果有载入尾行,否则返回空</returns>
        Task<TEntity> LastOrDefaultAsync();

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        Task<TEntity> LastAsync(Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        Task<TEntity> LastAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b);


        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b);

        #endregion

        #region All

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>数据</returns>
        Task<List<TEntity>> AllAsync();

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>数据</returns>
        Task<List<TEntity>> AllAsync(LambdaItem<TEntity> lambda);

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>数据</returns>
        Task<List<TEntity>> AllAsync<TField>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TField>> orderBy, bool desc);

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>数据</returns>
        Task<List<TEntity>> AllAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b);

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <param name="orderBys">排序</param>
        /// <returns>数据</returns>
        Task<List<TEntity>> AllAsync(Expression<Func<TEntity, bool>> lambda, params string[] orderBys);

        /// <summary>
        ///     分页读取
        /// </summary>
        Task<List<TEntity>> LoadDataAsync(int page, int limit, string order, bool desc, string condition, params DbParameter[] args);

        #endregion

        #region 聚合函数支持

        #region Collect

        /// <summary>
        ///     汇总方法
        /// </summary>
        Task<object> CollectAsync(string fun, string field, Expression<Func<TEntity, bool>> lambda);

        #endregion


        #region Any

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> lambda);


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b);

        #endregion

        #region Count

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        Task<long> CountAsync(Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        Task<long> CountAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b);

        #endregion

        #region Sum

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">查询表达式</param>
        /// <param name="condition2">条件2，默认为空</param>
        Task<decimal> SumAsync<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> lambda,
            string condition2 = null);

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        Task<decimal> SumAsync<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> a,
            Expression<Func<TEntity, bool>> b);


        #endregion


        #endregion

        #region 分页读取

        /// <summary>
        ///     分页读取
        /// </summary>
        Task<List<TEntity>> PageDataAsync(int page, int limit);

        /// <summary>
        ///     分页读取
        /// </summary>
        Task<List<TEntity>> PageDataAsync(int page, int limit, Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     分页读取
        /// </summary>
        Task<List<TEntity>> PageDataAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> field,
            Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     分页读取
        /// </summary>
        Task<List<TEntity>> PageDataAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> field, bool desc,
            Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     分页读取
        /// </summary>
        Task<List<TEntity>> PageDataAsync(int page, int limit, string order, bool desc, string condition, params DbParameter[] args);

        /// <summary>
        ///     分页读取
        /// </summary>
        Task<ApiPageData<TEntity>> PageAsync(int page, int limit, string order, bool desc, string condition, params DbParameter[] args);

        #endregion

        #region 单列读取


        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="lambda">条件</param>
        /// <returns>内容</returns>
        Task<TField> LoadValueAsync<TField>(Expression<Func<TEntity, TField>> field, Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="key">主键</param>
        /// <returns>内容</returns>
        Task<TField> LoadValueAsync<TField, TKey>(Expression<Func<TEntity, TField>> field, TKey key);

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="lambda">条件</param>
        /// <returns>内容</returns>
        Task<List<TField>> LoadValuesAsync<TField>(Expression<Func<TEntity, TField>> fieldExpression,
            Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="condition">条件</param>
        /// <returns>内容</returns>
        Task<List<TField>> LoadValuesAsync<TField>(Expression<Func<TEntity, TField>> fieldExpression, string condition);

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="parse">转换数据类型方法</param>
        /// <param name="lambda">条件</param>
        /// <returns>内容</returns>
        Task<List<TField>> LoadValuesAsync<TField>(Expression<Func<TEntity, TField>> fieldExpression,
            Func<object, TField> parse, Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="condition">条件</param>
        /// <param name="args">参数</param>
        /// <returns>内容</returns>
        Task<List<TField>> LoadValuesAsync<TField>(Expression<Func<TEntity, TField>> fieldExpression, string condition, DbParameter[] args);
        #endregion

        #region 数据读取

        /// <summary>
        ///     主键读取
        /// </summary>
        Task<TEntity> LoadByPrimaryKeyAsync(object key);

        /// <summary>
        ///     主键读取
        /// </summary>
        Task<List<TEntity>> LoadByPrimaryKeiesAsync(IEnumerable keies);


        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        Task<TEntity> LoadFirstAsync(string condition = null);

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        Task<TEntity> LoadFirstAsync(string foreignKey, object key);

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        Task<TEntity> LoadLastAsync(string condition = null);

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        Task<TEntity> LoadLastAsync(string foreignKey, object key);

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        Task<List<TEntity>> LoadByForeignKeyAsync(string foreignKey, object key);


        #endregion

        #endregion

        #region 写

        #region 数据操作


        /// <summary>
        ///     保存数据
        /// </summary>
        Task<bool> SaveAsync(TEntity entity);

        /// <summary>
        ///     更新数据
        /// </summary>
        Task<bool> UpdateAsync(TEntity entity);

        /// <summary>
        ///     插入新数据
        /// </summary>
        Task<bool> InsertAsync(TEntity entity);

        /// <summary>
        ///     删除数据
        /// </summary>
        Task<bool> DeleteAsync(TEntity entity);

        /// <summary>
        ///     删除数据
        /// </summary>
        Task<bool> DeletePrimaryKeyAsync(object key);

        /// <summary>
        ///     条件删除
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     物理删除数据
        /// </summary>
        Task<bool> PhysicalDeleteAsync(object key);

        /// <summary>
        ///     强制物理删除
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否删除成功</returns>
        Task<int> PhysicalDeleteAsync(Expression<Func<TEntity, bool>> lambda);
        /// <summary>
        ///     保存数据
        /// </summary>
        Task<int> SaveAsync(IEnumerable<TEntity> entities);

        /// <summary>
        ///     更新数据
        /// </summary>
        Task<int> UpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        ///     插入新数据
        /// </summary>
        Task<int> InsertAsync(IEnumerable<TEntity> entities);

        /// <summary>
        ///     删除数据
        /// </summary>
        Task<int> DeleteAsync(IEnumerable<TEntity> entities);

        #endregion

        #region 条件更新

        /// <summary>
        ///     全量更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>更新行数</returns>
        Task<int> SetValueAsync<TField>(Expression<Func<TEntity, TField>> field, TField value);

        /// <summary>
        ///     条件更新实体中已记录更新部分
        /// </summary>
        /// <param name="data">实体</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        Task<int> SetValueAsync(TEntity data, Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        Task<int> SetValueAsync<TField>(Expression<Func<TEntity, TField>> field, TField value, Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="value">值</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        Task<int> SetValueAsync<TField, TKey>(Expression<Func<TEntity, TField>> fieldExpression, TField value, TKey key);

        #endregion


        #endregion

        #region 数据校验支持

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="condition"></param>
        Task<bool> IsUniqueAsync<TValue>(Expression<Func<TEntity, TValue>> field, object val, Expression<Func<TEntity, bool>> condition);

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="key"></param>
        Task<bool> IsUniqueAsync<TValue>(Expression<Func<TEntity, TValue>> field, object val, object key);

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        Task<bool> IsUniqueAsync<TValue>(Expression<Func<TEntity, TValue>> field, object val);

        #endregion
    }
}