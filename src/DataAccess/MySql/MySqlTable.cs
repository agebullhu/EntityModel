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
using MySql.Data.MySqlClient;

#endregion

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    ///     Sql实体访问类
    /// </summary>
    /// <typeparam name="TData">实体</typeparam>
    public abstract partial class MySqlTable<TData> : SimpleConfig, IDataTable<TData>
        where TData : EditDataObject, new()
    {
        #region 数据库

        /// <summary>
        ///     自动数据连接对象
        /// </summary>
        private MySqlDataBase _dataBase;

        /// <summary>
        ///     自动数据连接对象
        /// </summary>
        public MySqlDataBase DataBase
        {
            get => _dataBase ?? (_dataBase = MySqlDataBase.DefaultDataBase ??
                                             (_dataBase = MySqlDataBase.DefaultDataBase = CreateDefaultDataBase()));
            set => _dataBase = value;
        }

        /// <summary>
        ///     自动数据连接对象
        /// </summary>
        IDataBase IDataTable<TData>.DataBase => DataBase;

        /// <summary>
        ///     设计时的主键字段
        /// </summary>
        string IDataTable<TData>.PrimaryKey => PrimaryKey;

        #endregion

        #region 数据结构

        /// <summary>
        ///     是否作为基类存在的
        /// </summary>
        public bool IsBaseClass { get; set; }

        /// <summary>
        ///     表名
        /// </summary>
        public string TableName => ReadTableName;

        /// <summary>
        ///     字段字典(运行时)
        /// </summary>
        public Dictionary<string, string> FieldDictionary => OverrideFieldMap ?? FieldMap;

        /// <summary>
        ///     主键字段(可动态覆盖PrimaryKey)
        /// </summary>
        private string _keyField;

        /// <summary>
        ///     主键字段(可动态覆盖PrimaryKey)
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

        #region 读

        #region 遍历所有

        /// <summary>
        ///     遍历所有
        /// </summary>
        public void FeachAll(Action<TData> action, Action<List<TData>> end)
        {
            //Debug.WriteLine(typeof(TData).Name, "对象");
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
        public ConditionItem Compile(Expression<Func<TData, bool>> lambda)
        {
            return PredicateConvert.Convert(FieldDictionary, lambda);
        }

        /// <summary>
        ///     编译查询条件
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        public ConditionItem Compile(LambdaItem<TData> lambda)
        {
            return PredicateConvert.Convert(FieldDictionary, lambda);
        }

        /// <summary>
        ///     取属性名称
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
                throw new Exception("表达式太复杂");

            expression = (MemberExpression)body.Operand;
            return expression.Member.Name;
        }

        #endregion

        #region 首行

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TData First()
        {
            return LoadFirst();
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TData FirstOrDefault()
        {
            return LoadFirst();
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TData FirstOrDefault(object id)
        {
            return LoadByPrimaryKey(id);
        }


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TData First(object id)
        {
            return LoadByPrimaryKey(id);
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TData First(Expression<Func<TData, bool>> lambda)
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
        public TData First(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
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
        public TData First(string condition, MySqlParameter[] args)
        {
            return LoadFirst(condition, args);
        }


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TData FirstOrDefault(Expression<Func<TData, bool>> lambda)
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
        public TData FirstOrDefault(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
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
        public TData FirstOrDefault(string condition, MySqlParameter[] args)
        {
            return LoadFirst(condition, args);
        }

        #endregion

        #region 尾行

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TData Last()
        {
            return LoadLast();
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TData LastOrDefault()
        {
            return LoadLast();
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TData Last(Expression<Func<TData, bool>> lambda)
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
        public TData Last(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
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
        public TData Last(string condition, MySqlParameter[] args)
        {
            return LoadLast(condition, args);
        }


        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TData LastOrDefault(Expression<Func<TData, bool>> lambda)
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
        public TData LastOrDefault(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
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
        public TData LastOrDefault(string condition, MySqlParameter[] args)
        {
            return LoadLast(condition, args);
        }

        #endregion

        #region Select

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public List<TData> Select()
        {
            return LoadDataInner();
        }

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>数据</returns>
        public List<TData> Select(Expression<Func<TData, bool>> lambda)
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
        ///     读取数据
        /// </summary>
        /// <returns>数据</returns>
        public List<TData> All()
        {
            return LoadDataInner();
        }


        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>数据</returns>
        public List<TData> All(string condition, MySqlParameter[] args)
        {
            return LoadDataInner(condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> All<TField>(Expression<Func<TData, bool>> lambda, Expression<Func<TData, TField>> orderBy,
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
        public List<TData> All(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
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
        public List<TData> All(LambdaItem<TData> lambda, params string[] orderBys)
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
        public List<TData> All(Expression<Func<TData, bool>> lambda, params string[] orderBys)
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
        public List<TData> Where(Expression<Func<TData, bool>> lambda)
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
        public List<TData> Where(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
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
        public object Collect(string fun, string field, string condition, params MySqlParameter[] args)
        {
            return CollectInner(fun, FieldMap[field], condition, args);
        }


        /// <summary>
        ///     汇总方法
        /// </summary>
        public object Collect(string fun, string field, Expression<Func<TData, bool>> lambda)
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
        public bool Exist(string condition, params MySqlParameter[] args)
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
            return ExistInner(PrimaryKeyConditionSQL, CreatePimaryKeyParameter(id));
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
            var obj = CollectInner("Count", "*", condition, args.Cast<MySqlParameter>().ToArray());
            return obj == DBNull.Value || obj == null ? 0L : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     总数
        /// </summary>
        public long Count(string condition, params MySqlParameter[] args)
        {
            var obj = CollectInner("Count", "*", condition, args);
            return obj == DBNull.Value || obj == null ? 0L : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public long Count(Expression<Func<TData, bool>> lambda)
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
        public long Count(LambdaItem<TData> lambda)
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
        ///     计数
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">查询表达式</param>
        /// <param name="args"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public long Count<TValue>(Expression<Func<TData, TValue>> field, string condition, params MySqlParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = CollectInner("Count", FieldMap[expression.Member.Name], condition, args);
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }
        #endregion

        #region SUM

        /// <summary>
        ///     汇总
        /// </summary>
        public decimal Sum(string field)
        {
            var obj = CollectInner("Sum", FieldMap[field], null, null);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     汇总
        /// </summary>
        public decimal Sum(string field, string condition, params MySqlParameter[] args)
        {
            var obj = CollectInner("Sum", FieldMap[field], condition, args);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
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
        public bool Any(string condition, MySqlParameter[] args)
        {
            return ExistInner(condition, args);
        }


        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public bool Any(Expression<Func<TData, bool>> lambda)
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
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">查询表达式</param>
        /// <param name="condition2">条件2，默认为空</param>
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
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
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
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">查询表达式</param>
        /// <param name="args"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public decimal Sum<TValue>(Expression<Func<TData, TValue>> field, string condition,
            params MySqlParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = CollectInner("Sum", FieldMap[expression.Member.Name], condition, args);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }

        #endregion

        #region 实现

        /// <summary>
        ///     是否存在数据
        /// </summary>
        protected bool ExistInner(string condition = null, MySqlParameter args = null)
        {
            return ExistInner(condition, args == null ? new MySqlParameter[0] : new[] { args });
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        protected bool ExistInner(string condition, MySqlParameter[] args)
        {
            var obj = CollectInner("Count", "*", condition, args);
            return obj != DBNull.Value && obj != null && Convert.ToInt64(obj) > 0;
        }

        /// <summary>
        ///     总数据量
        /// </summary>
        protected long CountInner(string condition = null, MySqlParameter args = null)
        {
            var obj = CollectInner("Count", "*", condition, args);
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     总数据量
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

        #region 分页读取

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> PageData(int page, int limit)
        {
            return PageData(page, limit, KeyField, false, null, null);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> PageData(int page, int limit, string condition, params MySqlParameter[] args)
        {
            return PageData(page, limit, KeyField, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> PageData(int page, int limit, Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return PageData(page, limit, KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> PageData(int page, int limit, LambdaItem<TData> lambda)
        {
            var convert = Compile(lambda);
            return PageData(page, limit, KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> LoadData(int page, int limit, string order, bool desc, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, order, desc, condition, args.Cast<MySqlParameter>().ToArray());
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> PageData(int page, int limit, string order, bool desc, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, order, desc, condition, args.Cast<MySqlParameter>().ToArray());
        }
        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> LoadData(int page, int limit, string order, string condition, params MySqlParameter[] args)
        {
            return PageData(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> PageData(int page, int limit, string order, string condition, params MySqlParameter[] args)
        {
            return PageData(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> PageData<TField>(int page, int limit, Expression<Func<TData, TField>> field,
            Expression<Func<TData, bool>> lambda)
        {
            return PageData(page, limit, field, false, lambda);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> PageData<TField>(int page, int limit, Expression<Func<TData, TField>> field, bool desc,
            Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return PageData(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> PageData<TField>(int page, int limit, Expression<Func<TData, TField>> field, bool desc,
            LambdaItem<TData> lambda)
        {
            var convert = Compile(lambda);
            return PageData(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
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

        #region 单列读取

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="lambda">条件</param>
        /// <returns>内容</returns>
        public TField LoadValue<TField>(Expression<Func<TData, TField>> field, Expression<Func<TData, bool>> lambda)
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
        public object Read<TField, TKey>(Expression<Func<TData, TField>> field, TKey key)
        {
            var fn = GetPropertyName(field);
            var vl = LoadValueInner(fn, FieldConditionSQL(KeyField), CreateFieldParameter(KeyField, key));
            return vl == DBNull.Value || vl == null ? default(TField) : vl;
        }

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="key">主键</param>
        /// <returns>内容</returns>
        public TField LoadValue<TField, TKey>(Expression<Func<TData, TField>> field, TKey key)
        {
            var fn = GetPropertyName(field);
            var vl = LoadValueInner(fn, FieldConditionSQL(KeyField), CreateFieldParameter(KeyField, key));
            return vl == DBNull.Value || vl == null ? default(TField) : (TField)vl;
        }

        /// <summary>
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="lambda">条件</param>
        /// <returns>内容</returns>
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
        ///     读取一个字段
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="condition">条件</param>
        /// <returns>内容</returns>
        public List<TField> LoadValues<TField>(Expression<Func<TData, TField>> fieldExpression, string condition)
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
        public List<TField> LoadValues<TField>(Expression<Func<TData, TField>> fieldExpression,
            Func<object, TField> parse, Expression<Func<TData, bool>> lambda)
        {
            var field = GetPropertyName(fieldExpression);
            var convert = Compile(lambda);
            var result = LoadValuesInner(field, convert.ConditionSql, convert.Parameters);
            return result.Count == 0 ? new List<TField>() : result.Select(parse).ToList();
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public object LoadValue(string field, string condition, params MySqlParameter[] args)
        {
            return LoadValueInner(field, condition, args);
        }

        /// <summary>
        ///     读取值
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
        ///     读取多个值
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

        #region 数据读取

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TData LoadData(object id)
        {
            return LoadByPrimaryKey(id);
        }


        /// <summary>
        ///     全表读取
        /// </summary>
        public List<TData> LoadData()
        {
            return LoadDataInner();
        }


        /// <summary>
        ///     条件读取
        /// </summary>
        public List<TData> LoadData(string condition, params MySqlParameter[] args)
        {
            return LoadDataInner(condition, args);
        }

        /// <summary>
        ///     主键读取
        /// </summary>
        public virtual TData LoadByPrimaryKey(object key)
        {
            return LoadFirstInner(PrimaryKeyConditionSQL, CreatePimaryKeyParameter(key));
        }

        /// <summary>
        ///     主键读取
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
        ///     如果存在的话读取首行
        /// </summary>
        public TData LoadFirst(string condition = null)
        {
            return LoadFirstInner(condition);
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public TData LoadFirst(string condition, params MySqlParameter[] args)
        {
            return LoadFirstInner(condition, args);
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public TData LoadFirst(string foreignKey, object key)
        {
            return LoadFirstInner(FieldConditionSQL(foreignKey), CreateFieldParameter(foreignKey, key));
        }

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        public TData LoadLast(string condition = null)
        {
            return LoadLastInner(condition);
        }

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        public TData LoadLast(string condition, params MySqlParameter[] args)
        {
            return LoadLastInner(condition, args);
        }

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        public TData LoadLast(string foreignKey, object key)
        {
            return LoadLastInner(FieldConditionSQL(foreignKey), CreateFieldParameter(foreignKey, key));
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public List<TData> LoadByForeignKey(string foreignKey, object key)
        {
            return LoadDataInner(FieldConditionSQL(foreignKey), CreateFieldParameter(foreignKey, key));
        }

        /// <summary>
        ///     重新读取
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
        ///     重新读取
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

        #region 载入事件

        /// <summary>
        ///     载入数据
        /// </summary>
        /// <param name="entity">读取数据的实体</param>
        private TData EntityLoaded(TData entity)
        {
            entity = OnEntityLoaded(entity);
            OnLoadAction?.Invoke(entity);
            return entity;
        }

        /// <summary>
        ///     载入后的同步处理
        /// </summary>
        /// <param name="entity"></param>
        protected virtual TData OnEntityLoaded(TData entity)
        {
            return entity;
        }

        /// <summary>
        ///     数据载入时给外部的处理方法
        /// </summary>
        public Action<TData> OnLoadAction;

        /// <summary>
        ///     载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <returns>读取数据的实体</returns>
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
        ///     重新载入
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
        ///     读取首行
        /// </summary>
        protected TData LoadFirstInner(string condition = null, MySqlParameter args = null)
        {
            return LoadFirstInner(condition, args == null ? new MySqlParameter[0] : new[] { args });
        }

        /// <summary>
        ///     读取首行
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
        ///     读取尾行
        /// </summary>
        protected TData LoadLastInner(string condition = null, MySqlParameter args = null)
        {
            return LoadLastInner(condition, args == null ? new MySqlParameter[0] : new[] { args });
        }

        /// <summary>
        ///     读取尾行
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
        ///     读取全部
        /// </summary>
        protected List<TData> LoadDataInner(string condition = null, MySqlParameter args = null)
        {
            return LoadDataInner(condition, args == null ? new MySqlParameter[0] : new[] { args }, null);
        }

        /// <summary>
        ///     读取全部
        /// </summary>
        protected List<TData> LoadDataInner(string condition, MySqlParameter[] args)
        {
            return LoadDataInner(condition, args, null);
        }

        /// <summary>
        ///     读取全部
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
        ///     读取全部(SQL语句是重写的,字段名称和顺序与设计时相同)
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
        ///     读取存储过程
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

        #region 写

        #region 内部写相关

        /// <summary>
        ///     保存
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
        ///     更新数据
        /// </summary>
        /// <param name="entity">更新数据的实体</param>
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
        ///     插入数据
        /// </summary>
        /// <param name="entity">插入数据的实体</param>
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
        ///     删除
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
        ///     删除
        /// </summary>
        private int DeleteInner(string condition, params MySqlParameter[] args)
        {
            if (string.IsNullOrEmpty(DeleteSqlCode))
                return 0;
            if (!string.IsNullOrEmpty(condition))
                return DataBase.Execute(CreateDeleteSql(condition), args);
            throw new ArgumentException(@"删除条件不能为空,因为不允许执行全表删除", GetType().FullName);
        }

        #endregion

        #region 事件

        /// <summary>
        ///     保存前处理(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="operatorType">操作类型</param>
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
        ///     保存完成后期处理(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="operatorType">操作类型</param>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
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

        #region 数据操作

        /// <summary>
        ///     保存数据
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
        ///     保存数据
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
        ///     更新数据
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
        ///     更新数据
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
        ///     插入新数据
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
        ///     插入新数据
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
        ///     删除数据
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
        ///     删除数据
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
        ///     删除数据
        /// </summary>
        public int Delete(object id)
        {
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                return Delete(LoadByPrimaryKey(id));
            }
        }

        /// <summary>
        ///     物理删除数据
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
        ///     删除数据
        /// </summary>
        public int DeletePrimaryKey(object key)
        {
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                return Delete(LoadByPrimaryKey(key));
            }
        }

        /// <summary>
        ///     清除所有数据
        /// </summary>
        public void Clear()
        {
            throw new Exception("批量删除功能被禁用");
            //using (MySqlDataBaseScope.CreateScope(DataBase))
            //{
            //    DataBase.Clear(WriteTableName);
            //}
        }

        /// <summary>
        ///     条件删除
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public int Delete(Expression<Func<TData, bool>> lambda)
        {
            //throw new Exception("批量删除功能被禁用");
            var convert = Compile(lambda);
            return Delete(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     强制物理删除
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否删除成功</returns>
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
        ///     条件删除
        /// </summary>
        public int Delete(string condition, params MySqlParameter[] args)
        {
            //throw new Exception("批量删除功能被禁用");
            if (string.IsNullOrWhiteSpace(condition))
                throw new ArgumentException(@"删除条件不能为空,因为不允许执行全表删除", GetType().FullName);
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

        #region 条件更新

        /// <summary>
        ///     条件更新
        /// </summary>
        public void SaveValue(string field, object value, string[] conditionFiles, object[] values)
        {
            var args = CreateFieldsParameters(conditionFiles, values);
            var condition = FieldConditionSQL(true, conditionFiles);
            SetValueInner(field, value, condition, args);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        public int SetValue(string field, object value, object key)
        {
            return SetValueInner(field, value, $"`{PrimaryKey}`='{key}'", CreateFieldParameter(KeyField, key));
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        public int SetValue(string field, object value, string condition, params MySqlParameter[] args)
        {
            return SetValueInner(field, value, condition, args);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        public int SetValue(Expression<Func<TData, bool>> field, bool value, string condition,
            params MySqlParameter[] args)
        {
            return SetValueInner(GetPropertyName(field), value ? 0 : 1, condition, args);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        public int SetValue(Expression<Func<TData, Enum>> field, Enum value, string condition,
            params MySqlParameter[] args)
        {
            return SetValueInner(GetPropertyName(field), Convert.ToInt32(value), condition, args);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value, string condition,
            params MySqlParameter[] args)
        {
            return SetValueInner(GetPropertyName(field), value, condition, args);
        }

        /// <summary>
        ///     全量更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>更新行数</returns>
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value)
        {
            return SetValueInner(GetPropertyName(field), value, null, null);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value,
            Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return SetValueInner(GetPropertyName(field), value, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
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

        #region 简单更新

        /// <summary>
        ///     与更新同时执行的SQL(更新之前立即执行)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected virtual string BeforeUpdateSql(string condition)
        {
            return null;
        }

        /// <summary>
        ///     与更新同时执行的SQL(更新之后立即执行)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected virtual string AfterUpdateSql(string condition)
        {
            return null;
        }

        /// <summary>
        ///     自定义更新（更新表达式自写）
        /// </summary>
        /// <param name="expression">更新SQL表达式</param>
        /// <param name="condition">条件</param>
        /// <returns>更新行数</returns>
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
        ///     自定义更新（更新表达式自写）
        /// </summary>
        /// <param name="expression">更新SQL表达式</param>
        /// <param name="condition">条件</param>
        /// <param name="args">参数</param>
        /// <returns>更新行数</returns>
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
        ///     条件更新
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="value">值</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
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
        ///     设计字段按自定义表达式更新值
        /// </summary>
        /// <param name="valueExpression">值的SQL方式</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
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

        #region 触发器支持

        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        private void OnPrepareSave(TData entity, DataOperatorType operatorType)
        {
            OnPrepareSave(operatorType, entity);
            DataUpdateHandler.OnPrepareSave(DataBase.Name, Name, entity, operatorType);
        }

        /// <summary>
        ///     保存完成后期处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        private void OnDataSaved(TData entity, DataOperatorType operatorType)
        {
            OnDataSaved(operatorType, entity);
            DataUpdateHandler.OnDataSaved(DataBase.Name,Name, entity, operatorType);
        }

        /// <summary>
        ///     更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        private void OnOperatorExecuting(string condition, IEnumerable<MySqlParameter> args, DataOperatorType operatorType)
        {
            var sqlParameters = args as MySqlParameter[] ?? args.ToArray();
            OnOperatorExecuting(operatorType, condition, sqlParameters);
            DataUpdateHandler.OnOperatorExecuting(DataBase.Name, Name, TableId, condition, sqlParameters, operatorType);
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        private void OnOperatorExecutd(string condition, IEnumerable<MySqlParameter> args, DataOperatorType operatorType)
        {
            var mySqlParameters = args as MySqlParameter[] ?? args.ToArray();
            OnOperatorExecutd(operatorType, condition, mySqlParameters);
            DataUpdateHandler.OnOperatorExecutd(DataBase.Name, Name, TableId, condition, mySqlParameters, operatorType);
        }

        #endregion

        #endregion


        #region 生成命令对象

        /// <summary>
        ///     生成命令
        /// </summary>
        protected MySqlCommand CreateLoadCommand(string condition, params MySqlParameter[] args)
        {
            return CreateLoadCommand(condition, null, args);
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        protected MySqlCommand CreateLoadCommand(string condition, string order, params MySqlParameter[] args)
        {
            var sql = CreateLoadSql(condition, order);
            return DataBase.CreateCommand(sql.ToString(), args);
        }

        /// <summary>
        ///     生成载入命令
        /// </summary>
        /// <param name="order">排序字段</param>
        /// <param name="desc">是否倒序</param>
        /// <param name="condition">数据条件</param>
        /// <param name="args">条件中的参数</param>
        /// <returns>载入命令</returns>
        protected MySqlCommand CreateLoadCommand(string order, bool desc, string condition,
            params MySqlParameter[] args)
        {
            var field = !string.IsNullOrEmpty(order) ? order : KeyField;
            Debug.Assert(FieldDictionary.ContainsKey(field));
            var orderSql = $"`{FieldDictionary[field]}` {(desc ? "DESC" : "")}";
            return CreateLoadCommand(condition, orderSql, args);
        }

        #endregion

        #region 字段的参数帮助

        /// <summary>
        ///     得到字段的MySqlDbType类型
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>参数</returns>
        protected virtual MySqlDbType GetDbType(string field)
        {
            return MySqlDbType.VarString;
        }


        private string _primaryConditionSQL;

        /// <summary>
        ///     主键的条件部分SQL
        /// </summary>
        public string PrimaryKeyConditionSQL => _primaryConditionSQL ??
                                                (_primaryConditionSQL = FieldConditionSQL(PrimaryKey));

        /// <summary>
        ///     生成多个字段的参数
        /// </summary>
        /// <param name="fields">生成参数的字段</param>
        public MySqlParameter[] CreateFieldsParameters(params string[] fields)
        {
            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"没有字段用于生成参数", nameof(fields));
            return fields.Select(field => new MySqlParameter(field, GetDbType(field))).ToArray();
        }

        /// <summary>
        ///     生成多个字段的参数
        /// </summary>
        /// <param name="fields">生成参数的字段</param>
        /// <param name="values">生成参数的值(长度和字段长度必须一致)</param>
        public MySqlParameter[] CreateFieldsParameters(string[] fields, object[] values)
        {
            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"没有字段用于生成参数", nameof(fields));
            if (values == null || values.Length == 0)
                throw new ArgumentException(@"没有值用于生成参数", nameof(values));
            if (values.Length != fields.Length)
                throw new ArgumentException(@"值的长度和字段长度必须一致", nameof(values));
            var res = new MySqlParameter[fields.Length];
            for (var i = 0; i < fields.Length; i++)
                res[i] = CreateFieldParameter(fields[i], values[i]);
            return res;
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        public MySqlParameter CreateFieldParameter(string field)
        {
            return new MySqlParameter(field, GetDbType(field));
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        /// <param name="value">值</param>
        public MySqlParameter CreateFieldParameter(string field, object value)
        {
            return MySqlDataBase.CreateParameter(field, value, GetDbType(field));
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        /// <param name="entity">取值的实体</param>
        public MySqlParameter CreateFieldParameter(string field, TData entity)
        {
            return CreateFieldParameter(field, entity.GetValue(field));
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        /// <param name="entity">取值的实体</param>
        /// <param name="entityField">取值的字段</param>
        public MySqlParameter CreateFieldParameter(string field, DataObjectBase entity, string entityField)
        {
            return CreateFieldParameter(field, entity.GetValue(entityField));
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        public MySqlParameter CreatePimaryKeyParameter()
        {
            return new MySqlParameter(KeyField, GetDbType(KeyField));
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        /// <param name="value">主键值</param>
        public MySqlParameter CreatePimaryKeyParameter(object value)
        {
            return MySqlDataBase.CreateParameter(KeyField, value, GetDbType(KeyField));
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        /// <param name="entity">取值的实体</param>
        public MySqlParameter CreatePimaryKeyParameter(TData entity)
        {
            return MySqlDataBase.CreateParameter(KeyField, entity.GetValue(KeyField),
                GetDbType(KeyField));
        }


        /// <summary>
        ///     连接字段条件
        /// </summary>
        /// <param name="isAnd">是否用AND组合</param>
        /// <param name="fields">生成参数的字段</param>
        /// <returns>ConditionItem</returns>
        public ConditionItem CreateConditionItem(bool isAnd, params string[] fields)
        {
            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"没有字段用于生成组合条件", nameof(fields));
            return new ConditionItem
            {
                ConditionSql = FieldConditionSQL(isAnd, fields),
                Parameters = CreateFieldsParameters(fields)
            };
        }

        /// <summary>
        ///     连接字段条件
        /// </summary>
        /// <param name="isAnd">是否用AND组合</param>
        /// <param name="fields">生成参数的字段</param>
        /// <param name="values">生成参数的值(长度和字段长度必须一致)</param>
        /// <returns>ConditionItem</returns>
        public ConditionItem CreateConditionItem(bool isAnd, string[] fields, object[] values)
        {
            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"没有字段用于生成组合条件", nameof(fields));
            return new ConditionItem
            {
                ConditionSql = FieldConditionSQL(isAnd, fields),
                Parameters = CreateFieldsParameters(fields, values)
            };
        }

        #endregion

        #region SQL

        #region 更新

        /// <summary>
        ///     生成更新的SQL语句
        /// </summary>
        /// <param name="expression">字段更新语句</param>
        /// <param name="convert">更新条件</param>
        /// <returns>更新的SQL语句</returns>
        private string CreateUpdateSql(string expression, ConditionItem convert)
        {
            return CreateUpdateSql(expression, convert.ConditionSql);
        }


        /// <summary>
        ///     生成更新的SQL
        /// </summary>
        /// <param name="valueExpression">更新表达式(SQL)</param>
        /// <param name="condition">更新条件</param>
        /// <returns>更新的SQL</returns>
        private string CreateUpdateSql(string valueExpression, string condition)
        {
            return $@"{BeforeUpdateSql(condition)}
UPDATE `{WriteTableName}` 
   SET {valueExpression} 
 WHERE {condition};
{AfterUpdateSql(condition)}";
        }


        /// <summary>
        ///     生成更新的SQL
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">条件</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>更新的SQL</returns>
        private string CreateUpdateSql(string field, object value, string condition, IList<MySqlParameter> parameters)
        {
            return CreateUpdateSql(FileUpdateSql(field, value, parameters), condition);
        }

        /// <summary>
        ///     生成单个字段更新的SQL
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>单个字段更新的SQL</returns>
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

        #region 载入

        /// <summary>
        /// 基本条件初始化完成的标识
        /// </summary>
        private bool BaseConditionInited = false;

        /// <summary>
        ///  初始化基本条件
        /// </summary>
        /// <returns></returns>
        protected virtual void InitBaseCondition()
        {
        }

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
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
        ///     生成汇总的SQL语句
        /// </summary>
        /// <param name="fun">汇总函数名称</param>
        /// <param name="field">汇总字段</param>
        /// <param name="condition">汇总条件</param>
        /// <returns>汇总的SQL语句</returns>
        private string CreateCollectSql(string fun, string field, string condition)
        {
            if (field != "*")
                field = $"`{FieldMap[field]}`";
            var sql = $@"SELECT {fun}({field}) FROM {ContextReadTable}{ContitionSqlCode(condition)};";
            return sql;
        }

        /// <summary>
        ///     生成载入字段值的SQL语句
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="condition">条件</param>
        /// <returns>载入字段值的SQL语句</returns>
        private string CreateLoadValueSql(string field, string condition)
        {
            Debug.Assert(FieldDictionary.ContainsKey(field));
            return $@"SELECT `{FieldDictionary[field]}` FROM {ContextReadTable}{ContitionSqlCode(condition)};";
        }

        /// <summary>
        ///     生成载入值的SQL
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="convert">条件</param>
        /// <returns>载入字段值的SQL语句</returns>
        private string CreateLoadValuesSql(string field, ConditionItem convert)
        {
            return $@"SELECT `{FieldDictionary[field]}` 
FROM {ContextReadTable}{ContitionSqlCode(convert.ConditionSql)};";
        }

        /// <summary>
        ///     生成载入的SQL语句
        /// </summary>
        /// <param name="condition">数据条件</param>
        /// <param name="order">排序字段</param>
        /// <returns>载入的SQL语句</returns>
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
        ///     生成分页的SQL
        /// </summary>
        /// <param name="page">页号</param>
        /// <param name="pageSize">每页几行(强制大于0,小于500行)</param>
        /// <param name="order">排序字段</param>
        /// <param name="desc">是否倒序</param>
        /// <param name="condition">数据条件</param>
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

        #region 删除

        /// <summary>
        ///     生成删除的SQL语句
        /// </summary>
        /// <param name="condition">删除条件</param>
        /// <returns>删除的SQL语句</returns>
        private string CreateDeleteSql(string condition)
        {
            return $@"{BeforeUpdateSql(condition)}
{DeleteSqlCode} WHERE {condition};
{AfterUpdateSql(condition)}";
        }

        /// <summary>
        ///     生成删除的SQL语句
        /// </summary>
        /// <param name="convert">删除条件</param>
        /// <returns>删除的SQL语句</returns>
        private string CreateDeleteSql(ConditionItem convert)
        {
            return CreateDeleteSql(convert.ConditionSql);
        }

        #endregion

        #region 字段条件

        /// <summary>
        ///     用在条件中的字段条件
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="expression">条件表达式</param>
        /// <returns>字段条件</returns>
        public string FieldConditionSQL(string field, string expression = "=")
        {
            Debug.Assert(FieldDictionary.ContainsKey(field));
            return $@"`{FieldDictionary[field]}` {expression} ?{field}";
        }

        /// <summary>
        ///     组合条件SQL
        /// </summary>
        /// <param name="isAnd">是否用AND组合</param>
        /// <param name="conditions">条件</param>
        public string JoinConditionSQL(bool isAnd, params string[] conditions)
        {
            if (conditions == null || conditions.Length == 0)
                throw new ArgumentException(@"没有条件用于组合", nameof(conditions));
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", conditions[0]);
            for (var idx = 1; idx < conditions.Length; idx++)
                sql.Append($@" {(isAnd ? "AND" : "OR")} ({conditions[idx]}) ");
            return sql.ToString();
        }

        /// <summary>
        ///     连接字段条件SQL
        /// </summary>
        /// <param name="isAnd">是否用AND组合</param>
        /// <param name="fields">生成参数的字段</param>
        public string FieldConditionSQL(bool isAnd, params string[] fields)
        {
            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"没有字段用于生成组合条件", nameof(fields));
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", FieldConditionSQL(fields[0]));
            for (var idx = 1; idx < fields.Length; idx++)
                sql.AppendFormat(@" {0} ({1}) ", isAnd ? "AND" : "OR", FieldConditionSQL(fields[idx]));
            return sql.ToString();
        }

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
        ///     检查值的唯一性
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
        ///     检查值的唯一性
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