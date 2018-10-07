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
using Gboxt.Common.DataModel.ExtendEvents;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     Sql实体访问类
    /// </summary>
    public interface IDataTable
    {
        #region 数据库

        /// <summary>
        ///     自动数据连接对象
        /// </summary>
        IDataBase DataBase
        {
            get;
        }

        #endregion

        #region 数据结构

        /// <summary>
        ///     表的唯一标识
        /// </summary>
        int TableId { get; }

        /// <summary>
        ///     设计时的主键字段
        /// </summary>
        string PrimaryKey { get; }

        /// <summary>
        ///     字段字典(设计时)
        /// </summary>
        Dictionary<string, string> FieldMap { get; }

        /// <summary>
        ///     所有字段(设计时)
        /// </summary>
        string[] Fields { get; }

        /// <summary>
        ///     读表名
        /// </summary>
        string ReadTableName { get; }

        /// <summary>
        ///     写表名
        /// </summary>
        string WriteTableName { get; }


        /// <summary>
        ///     字段字典(运行时)
        /// </summary>
        Dictionary<string, string> FieldDictionary { get; }

        /// <summary>
        ///     主键字段(可动态覆盖PrimaryKey)
        /// </summary>
        string KeyField { get; }
        #endregion

        #region 动态虚化


        /// <summary>
        ///     切换读取的表
        /// </summary>
        /// <returns>之前的动态读取的表名</returns>
        string SetDynamicReadTable(string table);

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
    /// <typeparam name="TData">实体</typeparam>
    public interface IDataTable<TData> : IDataTable, IConfig
    where TData : EditDataObject, new()
    {
        #region 读

        #region 遍历所有

        /// <summary>
        ///     遍历所有
        /// </summary>

        void FeachAll(Action<TData> action, Action<List<TData>> end);

        #endregion


        #region 首行

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <returns>如果有载入首行,否则返回空</returns>
        TData First();

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <returns>如果有载入首行,否则返回空</returns>
        TData FirstOrDefault();

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        TData FirstOrDefault(object id);


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        TData First(object id);

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        TData First(Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        TData First(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        TData FirstOrDefault(Expression<Func<TData, bool>> lambda);


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        TData FirstOrDefault(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);

        #endregion

        #region 尾行

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <returns>如果有载入尾行,否则返回空</returns>
        TData Last();

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <returns>如果有载入尾行,否则返回空</returns>
        TData LastOrDefault();

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        TData Last(Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        TData Last(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);


        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        TData LastOrDefault(Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        TData LastOrDefault(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);

        #endregion

        #region Select

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        List<TData> Select();

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>数据</returns>
        List<TData> Select(Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>数据</returns>
        List<TData> Select(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);

        #endregion

        #region All

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>数据</returns>
        List<TData> All();

        /// <summary>
        ///     分页读取
        /// </summary>
        List<TData> All(LambdaItem<TData> lambda);

        /// <summary>
        ///     分页读取
        /// </summary>
        List<TData> All<TField>(Expression<Func<TData, bool>> lambda, Expression<Func<TData, TField>> orderBy, bool desc);

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>数据</returns>
        List<TData> All(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <param name="orderBys">排序</param>
        /// <returns>数据</returns>
        List<TData> All(Expression<Func<TData, bool>> lambda, params string[] orderBys);

        /// <summary>
        ///     分页读取
        /// </summary>
        List<TData> LoadData(int page, int limit, string order, bool desc, string condition, params DbParameter[] args);

        /// <summary>
        ///     分页读取
        /// </summary>
        List<TData> PageData(int page, int limit, string order, bool desc, string condition, params DbParameter[] args);
        #endregion

        #region Where

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        List<TData> Where(Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>是否存在数据</returns>
        List<TData> Where(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);

        #endregion


        #region 聚合函数支持

        #region Collect

        /// <summary>
        ///     汇总方法
        /// </summary>
        object Collect(string fun, string field, Expression<Func<TData, bool>> lambda);

        #endregion


        #region Any

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        bool Any(Expression<Func<TData, bool>> lambda);


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        bool Any(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);

        #endregion

        #region Count

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        long Count(Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        long Count(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);

        #endregion

        #region Sum

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">查询表达式</param>
        /// <param name="condition2">条件2，默认为空</param>
        decimal Sum<TValue>(Expression<Func<TData, TValue>> field, Expression<Func<TData, bool>> lambda,
            string condition2 = null);

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        decimal Sum<TValue>(Expression<Func<TData, TValue>> field, Expression<Func<TData, bool>> a,
            Expression<Func<TData, bool>> b);


        #endregion


        #endregion

        #region 分页读取

        /// <summary>
        ///     分页读取
        /// </summary>
        List<TData> PageData(int page, int limit);

        /// <summary>
        ///     分页读取
        /// </summary>
        List<TData> PageData(int page, int limit, Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     分页读取
        /// </summary>
        List<TData> PageData<TField>(int page, int limit, Expression<Func<TData, TField>> field,
            Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     分页读取
        /// </summary>
        List<TData> PageData<TField>(int page, int limit, Expression<Func<TData, TField>> field, bool desc,
            Expression<Func<TData, bool>> lambda);

        #endregion

        #region 单列读取


        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="lambda">条件</param>
        /// <returns>内容</returns>
        TField LoadValue<TField>(Expression<Func<TData, TField>> field, Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="key">主键</param>
        /// <returns>内容</returns>
        TField LoadValue<TField, TKey>(Expression<Func<TData, TField>> field, TKey key);

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="lambda">条件</param>
        /// <returns>内容</returns>
        List<TField> LoadValues<TField>(Expression<Func<TData, TField>> fieldExpression,
            Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="condition">条件</param>
        /// <returns>内容</returns>
        List<TField> LoadValues<TField>(Expression<Func<TData, TField>> fieldExpression, string condition);

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="parse">转换数据类型方法</param>
        /// <param name="lambda">条件</param>
        /// <returns>内容</returns>
        List<TField> LoadValues<TField>(Expression<Func<TData, TField>> fieldExpression,
            Func<object, TField> parse, Expression<Func<TData, bool>> lambda);

        #endregion

        #region 数据读取
        /// <summary>
        ///     载入条件数据
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        List<TData> LoadData(MulitCondition condition);

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        TData LoadData(object id);

        /// <summary>
        ///     全表读取
        /// </summary>
        List<TData> LoadData();

        /// <summary>
        ///     主键读取
        /// </summary>
        TData LoadByPrimaryKey(object key);

        /// <summary>
        ///     主键读取
        /// </summary>
        List<TData> LoadByPrimaryKeies(IEnumerable keies);


        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        TData LoadFirst(string condition = null);

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        TData LoadFirst(string foreignKey, object key);

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        TData LoadLast(string condition = null);

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        TData LoadLast(string foreignKey, object key);

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        List<TData> LoadByForeignKey(string foreignKey, object key);


        #endregion

        #endregion

        #region 写

        #region 数据操作


        /// <summary>
        ///     保存数据
        /// </summary>
        bool Save(TData entity);

        /// <summary>
        ///     更新数据
        /// </summary>
        bool Update(TData entity);

        /// <summary>
        ///     插入新数据
        /// </summary>
        bool Insert(TData entity);

        /// <summary>
        ///     删除数据
        /// </summary>
        bool Delete(TData entity);


        /// <summary>
        ///     删除数据
        /// </summary>
        bool DeletePrimaryKey(object key);

        /// <summary>
        ///     条件删除
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        int Delete(Expression<Func<TData, bool>> lambda);
        /// <summary>
        ///     物理删除数据
        /// </summary>
        bool PhysicalDelete(object key);

        /// <summary>
        ///     强制物理删除
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否删除成功</returns>
        int PhysicalDelete(Expression<Func<TData, bool>> lambda);
        /// <summary>
        ///     保存数据
        /// </summary>
        int Save(IEnumerable<TData> entities);

        /// <summary>
        ///     更新数据
        /// </summary>
        int Update(IEnumerable<TData> entities);
        /// <summary>
        ///     插入新数据
        /// </summary>
        int Insert(IEnumerable<TData> entities);

        /// <summary>
        ///     删除数据
        /// </summary>
        int Delete(IEnumerable<TData> entities);
        #endregion

        #region 条件更新

        /// <summary>
        ///     全量更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>更新行数</returns>
        int SetValue<TField>(Expression<Func<TData, TField>> field, TField value);

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        int SetValue<TField>(Expression<Func<TData, TField>> field, TField value, Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="value">值</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        int SetValue<TField, TKey>(Expression<Func<TData, TField>> fieldExpression, TField value, TKey key);

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
        bool IsUnique<TValue>(Expression<Func<TData, TValue>> field, object val, Expression<Func<TData, bool>> condition);

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="key"></param>
        bool IsUnique<TValue>(Expression<Func<TData, TValue>> field, object val, object key);

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        bool IsUnique<TValue>(Expression<Func<TData, TValue>> field, object val);

        #endregion
    }
}