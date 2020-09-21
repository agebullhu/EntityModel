// // /*****************************************************
// // 作者:agebull
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2018.10.07
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Events;
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
    public interface IDataAccess
    {
        #region 数据库

        /// <summary>
        /// 数据库类型
        /// </summary>
        DataBaseType DataBaseType
        {
            get;
        }

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
        /// 不做代码注入
        /// </summary>
        bool NoInjection { get;}

        #endregion

        #region 聚合函数支持


        /// <summary>
        ///     是否存在数据
        /// </summary>
        bool Exist();

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        bool Any();

        /// <summary>
        ///     是否存在此主键的数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>是否存在数据</returns>
        bool ExistPrimaryKey<T>(T id);

        /// <summary>
        ///     汇总
        /// </summary>
        decimal Sum(string field);

        /// <summary>
        ///     总数
        /// </summary>
        long Count();

        /// <summary>
        ///     总数
        /// </summary>
        long Count(string condition, params DbParameter[] args);


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
        void SaveValue(string field, object value, string[] conditionFiles, object[] conditionValues);

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="key">条件主键</param>
        /// <returns>更新行数</returns>
        int SetValue(string field, object value, object key);

        /// <summary>
        ///     设计字段按自定义表达式更新值
        /// </summary>
        /// <param name="valueExpression">值的SQL方式</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        int SetCoustomValue<TKey>(string valueExpression, TKey key);

        #endregion
    }

    /// <summary>
    ///     Sql实体访问类
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    public interface IDataAccess<TEntity> : IDataAccess, IConfig
        where TEntity : EditDataObject, new()
    {

        /// <summary>
        /// Sql语句构造器
        /// </summary>
        ISqlBuilder<TEntity> SqlBuilder
        {
            get;
        }

        /// <summary>
        /// 当前上下文的读取器
        /// </summary>
        Action<DbDataReader, TEntity> DynamicLoadAction { get; set; }


        #region 查询条件相关(包含lambda编译)

        /// <summary>
        ///     编译查询条件
        /// </summary>
        /// <param name="lambda">条件</param>
        ConditionItem Compile(Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     编译查询条件
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        ConditionItem Compile(LambdaItem<TEntity> lambda);

        #endregion

        #region 读

        #region 遍历所有

        /// <summary>
        ///     遍历所有
        /// </summary>

        void FeachAll(Action<TEntity> action, Action<List<TEntity>> end);

        #endregion


        #region 首行

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <returns>如果有载入首行,否则返回空</returns>
        [Obsolete]
        TEntity First();

        /// <summary>
        ///     载入首行
        /// </summary>
        [Obsolete]
        TEntity First(LambdaItem<TEntity> lambda);

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        [Obsolete]
        TEntity First(Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        [Obsolete]
        TEntity First(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b);


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <returns>如果有载入首行,否则返回空</returns>
        TEntity FirstOrDefault();

        /// <summary>
        ///     载入首行
        /// </summary>
        TEntity FirstOrDefault(LambdaItem<TEntity> lambda);

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b);


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
        TEntity Last();

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <returns>如果有载入尾行,否则返回空</returns>
        TEntity LastOrDefault();

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        TEntity Last(Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        TEntity Last(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b);


        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        TEntity LastOrDefault(Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        TEntity LastOrDefault(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b);

        #endregion

        #region Select

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        [Obsolete]
        List<TEntity> Select();

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>数据</returns>
        [Obsolete]
        List<TEntity> Select(Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>数据</returns>
        [Obsolete]
        List<TEntity> Select(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b);

        #endregion

        #region All

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>数据</returns>
        List<TEntity> All();


        /// <summary>
        ///     读取数据
        /// </summary>
        List<TEntity> All(LambdaItem<TEntity> lambda);

        /// <summary>
        ///     读取数据
        /// </summary>
        List<TEntity> All<TField>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TField>> orderBy, bool desc);

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>数据</returns>
        List<TEntity> All(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b);

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <param name="orderBys">排序</param>
        /// <returns>数据</returns>
        List<TEntity> All(Expression<Func<TEntity, bool>> lambda, params string[] orderBys);

        /// <summary>
        ///     分页读取
        /// </summary>
        List<TEntity> LoadData(int page, int limit, string order, bool desc, string condition, params DbParameter[] args);

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

        #region Where

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        [Obsolete]
        List<TEntity> Where(Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>是否存在数据</returns>
        [Obsolete]
        List<TEntity> Where(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b);

        #endregion


        #region 聚合函数支持

        #region Collect

        /// <summary>
        ///     汇总方法
        /// </summary>
        object Collect(string fun, string field, Expression<Func<TEntity, bool>> lambda);

        #endregion


        #region Any

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        bool Any(Expression<Func<TEntity, bool>> lambda);


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        bool Any(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b);

        #endregion

        #region Count

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        long Count(Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        long Count(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b);

        #endregion

        #region Sum

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">查询表达式</param>
        /// <param name="condition2">条件2，默认为空</param>
        decimal Sum<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> lambda,
            string condition2 = null);

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        decimal Sum<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> a,
            Expression<Func<TEntity, bool>> b);


        #endregion


        #endregion

        #region 分页读取

        /// <summary>
        ///     分页读取
        /// </summary>
        List<TEntity> PageData(int page, int limit);

        /// <summary>
        ///     分页读取
        /// </summary>
        List<TEntity> PageData(int page, int limit, Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     分页读取
        /// </summary>
        List<TEntity> PageData<TField>(int page, int limit, Expression<Func<TEntity, TField>> field,
            Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     分页读取
        /// </summary>
        List<TEntity> PageData<TField>(int page, int limit, Expression<Func<TEntity, TField>> field, bool desc,
            Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     分页读取
        /// </summary>
        List<TEntity> PageData(int page, int limit, string order, bool desc, string condition, params DbParameter[] args);

        /// <summary>
        ///     分页读取
        /// </summary>
        ApiPageData<TEntity> Page(int page, int limit, string order, bool desc, string condition, params DbParameter[] args);

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
        TField LoadValue<TField>(Expression<Func<TEntity, TField>> field, Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="key">主键</param>
        /// <returns>内容</returns>
        TField LoadValue<TField, TKey>(Expression<Func<TEntity, TField>> field, TKey key);

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="lambda">条件</param>
        /// <returns>内容</returns>
        List<TField> LoadValues<TField>(Expression<Func<TEntity, TField>> fieldExpression,
            Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="condition">条件</param>
        /// <returns>内容</returns>
        List<TField> LoadValues<TField>(Expression<Func<TEntity, TField>> fieldExpression, string condition);

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="parse">转换数据类型方法</param>
        /// <param name="lambda">条件</param>
        /// <returns>内容</returns>
        List<TField> LoadValues<TField>(Expression<Func<TEntity, TField>> fieldExpression,
            Func<object, TField> parse, Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="condition">条件</param>
        /// <param name="args">参数</param>
        /// <returns>内容</returns>
        List<TField> LoadValues<TField>(Expression<Func<TEntity, TField>> fieldExpression, string condition, DbParameter[] args);
        #endregion

        #region 数据读取
        /// <summary>
        ///     载入条件数据
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        List<TEntity> LoadData(MulitCondition condition);

        /// <summary>
        ///     主键读取
        /// </summary>
        TEntity LoadByPrimaryKey(object key);

        /// <summary>
        ///     主键读取
        /// </summary>
        Task<TEntity> LoadByPrimaryKeyAsync(object key);

        /// <summary>
        ///     主键读取
        /// </summary>
        List<TEntity> LoadByPrimaryKeies(IEnumerable keies);


        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        TEntity LoadFirst(string condition = null);

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        TEntity LoadFirst(string foreignKey, object key);

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        TEntity LoadLast(string condition = null);

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        TEntity LoadLast(string foreignKey, object key);

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        List<TEntity> LoadByForeignKey(string foreignKey, object key);


        #endregion

        #endregion

        #region 写

        #region 数据操作


        /// <summary>
        ///     保存数据
        /// </summary>
        bool Save(TEntity entity);

        /// <summary>
        ///     更新数据
        /// </summary>
        bool Update(TEntity entity);

        /// <summary>
        ///     更新数据
        /// </summary>
        Task<bool> UpdateAsync(TEntity entity);

        /// <summary>
        ///     插入新数据
        /// </summary>
        bool Insert(TEntity entity);
        /// <summary>
        ///     插入新数据
        /// </summary>
        Task<bool> InsertAsync(TEntity entity);

        /// <summary>
        ///     删除数据
        /// </summary>
        bool Delete(TEntity entity);


        /// <summary>
        ///     删除数据
        /// </summary>
        bool DeletePrimaryKey(object key);


        /// <summary>
        ///     删除数据
        /// </summary>
        Task<bool> DeletePrimaryKeyAsync(object key);

        /// <summary>
        ///     条件删除
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        int Delete(Expression<Func<TEntity, bool>> lambda);
        /// <summary>
        ///     物理删除数据
        /// </summary>
        bool PhysicalDelete(object key);

        /// <summary>
        ///     强制物理删除
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否删除成功</returns>
        int PhysicalDelete(Expression<Func<TEntity, bool>> lambda);
        /// <summary>
        ///     保存数据
        /// </summary>
        int Save(IEnumerable<TEntity> entities);

        /// <summary>
        ///     更新数据
        /// </summary>
        int Update(IEnumerable<TEntity> entities);
        /// <summary>
        ///     插入新数据
        /// </summary>
        int Insert(IEnumerable<TEntity> entities);

        /// <summary>
        ///     删除数据
        /// </summary>
        int Delete(IEnumerable<TEntity> entities);
        #endregion

        #region 条件更新

        /// <summary>
        ///     全量更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>更新行数</returns>
        int SetValue<TField>(Expression<Func<TEntity, TField>> field, TField value);

        /// <summary>
        ///     条件更新实体中已记录更新部分
        /// </summary>
        /// <param name="data">实体</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        int SetValue(TEntity data, Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        int SetValue<TField>(Expression<Func<TEntity, TField>> field, TField value, Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="value">值</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        int SetValue<TField, TKey>(Expression<Func<TEntity, TField>> fieldExpression, TField value, TKey key);

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
        bool IsUnique<TValue>(Expression<Func<TEntity, TValue>> field, object val, Expression<Func<TEntity, bool>> condition);

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="key"></param>
        bool IsUnique<TValue>(Expression<Func<TEntity, TValue>> field, object val, object key);

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        bool IsUnique<TValue>(Expression<Func<TEntity, TValue>> field, object val);

        #endregion
    }

    /// <summary>
    /// 数据状态表
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IDataAccessByStateData<TEntity> : IDataAccess<TEntity>
        where TEntity : EditDataObject, new()
    {
        /// <summary>
        /// 重置状态
        /// </summary>
        bool ResetState<TPrimaryKey>(TPrimaryKey id);

        /// <summary>
        /// 重置状态
        /// </summary>
        bool ResetState(Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        /// 修改状态
        /// </summary>
        bool SetState<TPrimaryKey>(DataStateType state, bool isFreeze, TPrimaryKey id);
    }
}