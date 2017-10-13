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
using System.Text;
using Gboxt.Common.DataModel.MySql;
using MySql.Data.MySqlClient;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     Sql实体访问类
    /// </summary>
    /// <typeparam name="TData">实体</typeparam>
    public interface IDataTable<TData>
        where TData : EditDataObject, new()
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
        ///     字段字典(运行时)
        /// </summary>
        Dictionary<string, string> FieldDictionary { get; }

        /// <summary>
        ///     表的唯一标识
        /// </summary>
        int TableId { get; }
        /// <summary>
        ///     设计时的主键字段
        /// </summary>
        string PrimaryKey { get; }

        #endregion

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
        TData FirstOrDefault(int id);


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        TData First(int id);

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
        List<TData> All<TField>(Expression<Func<TData, bool>> lambda, Expression<Func<TData, TField>> orderBy,
            bool desc);

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
        List<TData> PageData(int page, int limit, string order,bool desc, string condition, params DbParameter[] args);
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

        #region Exist

        /// <summary>
        ///     是否存在数据
        /// </summary>
        bool Exist();

        /// <summary>
        ///     是否存在此主键的数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>是否存在数据</returns>
        bool ExistPrimaryKey<T>(T id);

        #endregion

        #region Count

        /// <summary>
        ///     总数
        /// </summary>
        long Count();
        
        #endregion

        #region SUM

        /// <summary>
        ///     汇总
        /// </summary>
        decimal Sum(string field);
        
        #endregion


        #region Any

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        bool Any();

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


        /// <summary>
        ///     总数
        /// </summary>
        long Count(string condition, params DbParameter[] args);
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
        /// <param name="fieldExpression">字段</param>
        /// <param name="lambda">条件</param>
        /// <returns>内容</returns>
        int LoadValue(Expression<Func<TData, int>> fieldExpression, Expression<Func<TData, bool>> lambda);


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
        List<int> LoadValues(Expression<Func<TData, int>> fieldExpression,
            Expression<Func<TData, bool>> lambda);

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
        ///     载入首行
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        TData LoadData(int id);

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
        void Save(IEnumerable<TData> entities);

        /// <summary>
        ///     保存数据
        /// </summary>
        void Save(TData entity);

        /// <summary>
        ///     更新数据
        /// </summary>
        void Update(TData entity);

        /// <summary>
        ///     更新数据
        /// </summary>
        void Update(IEnumerable<TData> entities);

        /// <summary>
        ///     插入新数据
        /// </summary>
        bool Insert(TData entity);

        /// <summary>
        ///     插入新数据
        /// </summary>
        void Insert(IEnumerable<TData> entities);

        /// <summary>
        ///     删除数据
        /// </summary>
        void Delete(IEnumerable<TData> entities);

        /// <summary>
        ///     删除数据
        /// </summary>
        int Delete(TData entity);

        /// <summary>
        ///     删除数据
        /// </summary>
        int Delete(int id);
        

        /// <summary>
        ///     删除数据
        /// </summary>
        int DeletePrimaryKey(object key);

        /// <summary>
        ///     清除所有数据
        /// </summary>
        void Clear();

        /// <summary>
        ///     条件删除
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        int Delete(Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     强制物理删除
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否删除成功</returns>
        int Destroy(Expression<Func<TData, bool>> lambda);
        
        #endregion

        #region 条件更新

        /// <summary>
        ///     条件更新
        /// </summary>
        void SaveValue(string field, object value, string[] conditionFiles, object[] values);

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        int SetValue(string field, object value, int key);

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
        int SetValue<TField>(Expression<Func<TData, TField>> field, TField value,
            Expression<Func<TData, bool>> lambda);
        
        #endregion

        #region 简单更新
        
        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="value">值</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        int SetValue<TField, TKey>(Expression<Func<TData, TField>> fieldExpression, TField value, TKey key);

        /// <summary>
        ///     设计字段按自定义表达式更新值
        /// </summary>
        /// <param name="valueExpression">值的SQL方式</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        int SetCoustomValue<TKey>(string valueExpression, TKey key);

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
        bool IsUnique<TValue>(Expression<Func<TData, TValue>> field, object val,
            Expression<Func<TData, bool>> condition);

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