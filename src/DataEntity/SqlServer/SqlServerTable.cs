// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:YhxBank.FundsManagement
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

#endregion

namespace Gboxt.Common.DataModel.SqlServer
{
    /// <summary>
    ///     Sql实体访问类
    /// </summary>
    /// <typeparam name="TData">实体</typeparam>
    public abstract class SqlServerTable<TData>
        where TData : EditDataObject, new()
    {
        #region 数据库

        /// <summary>
        ///     表的唯一标识
        /// </summary>
        public abstract int TableId { get; }

        /// <summary>
        ///     自动数据连接对象
        /// </summary>
        private SqlServerDataBase _dataBase;

        /// <summary>
        ///     自动数据连接对象
        /// </summary>
        public SqlServerDataBase DataBase
        {
            get { return this._dataBase ?? (this._dataBase = SqlServerDataBase.DefaultDataBase ?? (this._dataBase = SqlServerDataBase.DefaultDataBase = this.CreateDefaultDataBase())); }
            set { this._dataBase = value; }
        }

        /// <summary>
        ///     初始化
        /// </summary>
        public void Initialize()
        {
            this.DataBase = SqlServerDataBase.DefaultDataBase;
        }

        /// <summary>
        ///     构造一个缺省可用的数据库对象
        /// </summary>
        /// <returns></returns>
        protected abstract SqlServerDataBase CreateDefaultDataBase();

        #endregion

        #region 数据结构

        /// <summary>
        ///     是否作为基类存在的
        /// </summary>
        public bool IsBaseClass { get; set; }


        /// <summary>
        ///     全表读取的SQL语句
        /// </summary>
        protected abstract string FullLoadFields { get; }


        /// <summary>
        ///     动态读取的字段
        /// </summary>
        private string _contextReadFields;

        /// <summary>
        ///     动态读取的字段
        /// </summary>
        protected string ContextLoadFields
        {
            get { return this._contextReadFields ?? this.FullLoadFields; }
            set { this._contextReadFields = string.IsNullOrWhiteSpace(value) ? null : value; }
        }

        /// <summary>
        ///     表名
        /// </summary>
        protected abstract string ReadTableName { get; }

        /// <summary>
        ///     动态读取的表
        /// </summary>
        protected string _dynamicReadTable;

        /// <summary>
        ///     取得动态读取的表名
        /// </summary>
        /// <returns>动态读取的表名</returns>
        internal string GetDynamicReadTable()
        {
            return this._dynamicReadTable;
        }

        /// <summary>
        ///     取得实际设置的ContextReadTable动态读取的表
        /// </summary>
        /// <returns>之前的动态读取的表名</returns>
        internal string SetDynamicReadTable(string table)
        {
            var old = this._dynamicReadTable;
            this._dynamicReadTable = string.IsNullOrWhiteSpace(table) ? null : table;
            return old;
        }

        /// <summary>
        ///     当前上下文读取的表名
        /// </summary>
        protected virtual string ContextReadTable
        {
            get { return this._dynamicReadTable ?? this.ReadTableName; }
        }

        /// <summary>
        ///     基本查询条件
        /// </summary>
        protected string BaseCondition { get; set; }

        /// <summary>
        ///     表名
        /// </summary>
        protected abstract string WriteTableName { get; }

        /// <summary>
        ///     设计时的主键字段
        /// </summary>
        protected abstract string PrimaryKey { get; }

        /// <summary>
        ///     主键字段(可动态覆盖PrimaryKey)
        /// </summary>
        private string _keyField;

        /// <summary>
        ///     主键字段(可动态覆盖PrimaryKey)
        /// </summary>
        public string KeyField
        {
            get { return this._keyField ?? this.PrimaryKey; }
            set { this._keyField = value; }
        }

        /// <summary>
        ///     删除的SQL语句
        /// </summary>
        protected virtual string DeleteSqlCode
        {
            get { return string.Format(@"DELETE FROM [{0}]", this.WriteTableName); }
        }

        /// <summary>
        ///     插入的SQL语句
        /// </summary>
        protected abstract string InsertSqlCode { get; }

        /// <summary>
        ///     全部更新的SQL语句
        /// </summary>
        protected abstract string UpdateSqlCode { get; }

        /// <summary>
        ///     所有字段
        /// </summary>
        public abstract string[] Fields { get; }

        /// <summary>
        ///     字段字典
        /// </summary>
        private Dictionary<string, string> _fieldMap;

        /// <summary>
        ///     字段字典(设计时)
        /// </summary>
        public virtual Dictionary<string, string> FieldMap
        {
            get { return this._fieldMap ?? (this._fieldMap = this.Fields.ToDictionary(p => p, p => p)); }
        }

        /// <summary>
        ///     字段字典(运行时)
        /// </summary>
        public Dictionary<string, string> FieldDictionary
        {
            get { return this.OverrideFieldMap ?? this.FieldMap; }
        }

        /// <summary>
        ///     字段字典(动态覆盖)
        /// </summary>
        public Dictionary<string, string> OverrideFieldMap { get; set; }

        #endregion

        #region 读

        #region 遍历所有

        /// <summary>
        ///     遍历所有
        /// </summary>
        public void FeachAll(Action<TData> action, Action<List<TData>> end)
        {
            //Debug.WriteLine(typeof(TData).Name, "对象");
            var list = this.All();
            //Debug.WriteLine(list.Count, "数量");
            if (list.Count == 0)
            {
                return;
            }
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
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected string ContitionSqlCode(string condition)
        {
            if (string.IsNullOrWhiteSpace(this.BaseCondition) && string.IsNullOrWhiteSpace(condition))
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.Append("WHERE ");
            if (!string.IsNullOrWhiteSpace(this.BaseCondition))
            {
                sb.AppendLine($"({this.BaseCondition})");
                if (!string.IsNullOrWhiteSpace(condition))
                {
                    sb.Append(" AND ");
                }
            }
            if (!string.IsNullOrWhiteSpace(condition))
            {
                sb.AppendLine($"({condition})");
            }
            return sb.ToString();
        }

        /// <summary>
        ///     编译查询条件
        /// </summary>
        /// <param name="lambda">条件</param>
        public ConditionItem Compile(Expression<Func<TData, bool>> lambda)
        {
            return PredicateConvert.Convert(this.FieldDictionary, lambda);
        }

        /// <summary>
        ///     编译查询条件
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        public ConditionItem Compile(LambdaItem<TData> lambda)
        {
            return PredicateConvert.Convert(this.FieldDictionary, lambda);
        }

        /// <summary>
        ///     取属性名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<TData, T>> action)
        {
            var expression = (MemberExpression)action.Body;
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
            return this.LoadFirst();
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TData FirstOrDefault()
        {
            return this.LoadFirst();
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TData First(Expression<Func<TData, bool>> lambda)
        {
            var convert = this.Compile(lambda);

            return this.LoadFirst(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TData First(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = this.Compile(a);
            var convert2 = this.Compile(b);
            return this.LoadFirst(string.Format("({0}) AND ({1})", convert1.ConditionSql, convert1.ConditionSql)
                , convert1.Parameters.Union(convert2.Parameters));
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public TData First(string condition, SqlParameter[] args)
        {
            return this.LoadFirst(condition, args);
        }


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TData FirstOrDefault(Expression<Func<TData, bool>> lambda)
        {
            var convert = this.Compile(lambda);

            return this.LoadFirst(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TData FirstOrDefault(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = this.Compile(a);
            var convert2 = this.Compile(b);
            return this.LoadFirst(string.Format("({0}) AND ({1})", convert1.ConditionSql, convert1.ConditionSql)
                , convert1.Parameters.Union(convert2.Parameters));
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="args">参数</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TData FirstOrDefault(string condition, SqlParameter[] args)
        {
            return this.LoadFirst(condition, args);
        }

        #endregion

        #region 尾行

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TData Last()
        {
            return this.LoadLast();
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TData LastOrDefault()
        {
            return this.LoadLast();
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TData Last(Expression<Func<TData, bool>> lambda)
        {
            var convert = this.Compile(lambda);

            return this.LoadLast(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TData Last(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = this.Compile(a);
            var convert2 = this.Compile(b);
            return this.LoadLast(string.Format("({0}) AND ({1})", convert1.ConditionSql, convert1.ConditionSql)
                , convert1.Parameters.Union(convert2.Parameters));
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public TData Last(string condition, SqlParameter[] args)
        {
            return this.LoadLast(condition, args);
        }


        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TData LastOrDefault(Expression<Func<TData, bool>> lambda)
        {
            var convert = this.Compile(lambda);

            return this.LoadLast(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TData LastOrDefault(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = this.Compile(a);
            var convert2 = this.Compile(b);
            return this.LoadLast(string.Format("({0}) AND ({1})", convert1.ConditionSql, convert1.ConditionSql)
                , convert1.Parameters.Union(convert2.Parameters));
        }

        /// <summary>
        ///     载入尾行
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="args">参数</param>
        /// <returns>如果有载入尾行,否则返回空</returns>
        public TData LastOrDefault(string condition, SqlParameter[] args)
        {
            return this.LoadLast(condition, args);
        }

        #endregion

        #region Select

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public List<TData> Select()
        {
            return this.LoadDataInner();
        }

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>数据</returns>
        public List<TData> Select(Expression<Func<TData, bool>> lambda)
        {
            var convert = this.Compile(lambda);
            return this.LoadDataInner(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>数据</returns>
        public List<TData> Select(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = this.Compile(a);
            var convert2 = this.Compile(b);
            return this.LoadDataInner(string.Format("({0}) AND ({1})", convert1.ConditionSql, convert1.ConditionSql)
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
            return this.LoadDataInner();
        }


        /// <summary>
        ///     读取数据
        /// </summary>
        /// <returns>数据</returns>
        public List<TData> All(string condition, SqlParameter[] args)
        {
            return this.LoadDataInner(condition, args);
        }

        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>数据</returns>
        public List<TData> All(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = this.Compile(a);
            var convert2 = this.Compile(b);
            return this.LoadDataInner(string.Format("({0}) AND ({1})", convert1.ConditionSql, convert1.ConditionSql)
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
            var convert = this.Compile(lambda);
            return this.LoadDataInner(convert.ConditionSql, convert.Parameters, orderBys.Length == 0 ? null : string.Join(",", orderBys));
        }

        /// <summary>
        ///     读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <param name="orderBys">排序</param>
        /// <returns>数据</returns>
        public List<TData> All(Expression<Func<TData, bool>> lambda, params string[] orderBys)
        {
            var convert = this.Compile(lambda);
            return this.LoadDataInner(convert.ConditionSql, convert.Parameters, orderBys.Length == 0 ? null : string.Join(",", orderBys));
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
            var convert = this.Compile(lambda);
            return this.LoadDataInner(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>是否存在数据</returns>
        public List<TData> Where(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = this.Compile(a);
            var convert2 = this.Compile(b);
            return this.LoadDataInner(string.Format("({0}) AND ({1})", convert1.ConditionSql, convert1.ConditionSql)
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }

        #endregion


        #region 聚合函数支持


        #region Collect

        /// <summary>
        ///     汇总方法
        /// </summary>
        public object Collect(string fun, string field, string condition, params SqlParameter[] args)
        {
            return this.CollectInner(fun, field, condition, args);
        }


        /// <summary>
        ///     汇总方法
        /// </summary>
        public object Collect(string fun, string field, Expression<Func<TData, bool>> lambda)
        {
            var convert = this.Compile(lambda);
            return this.CollectInner(fun, field, convert.ConditionSql, convert.Parameters);
        }
        #endregion

        #region Exist

        /// <summary>
        ///     是否存在数据
        /// </summary>
        public bool Exist()
        {
            return this.Count() > 0;
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        public bool Exist(string condition, params SqlParameter[] args)
        {
            return this.Count(condition, args) > 0;
        }

        /// <summary>
        ///     主键数据是否存在
        /// </summary>
        public bool ExistPrimaryKey(object key)
        {
            return this.ExistInner(this.PrimaryKeyConditionSQL, this.CreatePimaryKeyParameter(key));
        }
        #endregion

        #region Count

        /// <summary>
        ///     总数
        /// </summary>
        public long Count()
        {
            return this.CountInner();
        }

        /// <summary>
        ///     总数
        /// </summary>
        public long Count(string condition, params SqlParameter[] args)
        {
            var obj = this.CollectInner("Count", "*", condition, args);
            return obj == DBNull.Value ? 0L : Convert.ToInt64(obj);
        }

        #endregion

        #region SUM

        /// <summary>
        ///     汇总
        /// </summary>
        public decimal Sum(string field)
        {
            var obj = this.CollectInner("Sum", field, null, null);
            return obj == DBNull.Value ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     汇总
        /// </summary>
        public decimal Sum(string field, string condition, params SqlParameter[] args)
        {
            var obj = this.CollectInner("Sum", field, condition, args);
            return obj == DBNull.Value ? 0M : Convert.ToDecimal(obj);
        }

        #endregion


        #region Any

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public bool Any()
        {
            return this.ExistInner();
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public bool Any(string condition, SqlParameter[] args)
        {
            return this.ExistInner(condition, args);
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public bool Any(Expression<Func<TData, bool>> lambda)
        {
            var convert = this.Compile(lambda);
            return this.ExistInner(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     载入首行
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public bool Any(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = this.Compile(a);
            var convert2 = this.Compile(b);
            return this.ExistInner(string.Format("({0}) AND ({1})", convert1.ConditionSql, convert1.ConditionSql)
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }

        #endregion

        #region Count

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public long Count(Expression<Func<TData, bool>> lambda)
        {
            var convert = this.Compile(lambda);
            var obj = this.CollectInner("Count", "*", convert.ConditionSql, convert.Parameters);
            return obj == DBNull.Value ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public long Count(LambdaItem<TData> lambda)
        {
            var convert = this.Compile(lambda);
            var obj = this.CollectInner("Count", "*", convert.ConditionSql, convert.Parameters);
            return obj == DBNull.Value ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public long Count(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = this.Compile(a);
            var convert2 = this.Compile(b);
            var obj = this.CollectInner("Count", "*",
                string.Format("({0}) AND ({1})", convert1.ConditionSql, convert1.ConditionSql)
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
            return obj == DBNull.Value ? 0 : Convert.ToInt64(obj);
        }


        /// <summary>
        ///     计数
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">查询表达式</param>
        /// <param name="args"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public long Count<TValue>(Expression<Func<TData, TValue>> field, string condition, params SqlParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = this.CollectInner("Count", expression.Member.Name, condition, args);
            return obj == DBNull.Value ? 0 : Convert.ToInt64(obj);
        }

        #endregion

        #region Sum

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">查询表达式</param>
        /// <param name="condition2">条件2，默认为空</param>
        public decimal Sum<TValue>(Expression<Func<TData, TValue>> field, Expression<Func<TData, bool>> lambda, string condition2 = null)
        {
            var expression = (MemberExpression)field.Body;
            var convert = this.Compile(lambda);
            var condition = condition2 == null
                ? convert.ConditionSql
                : convert.ConditionSql == null
                    ? condition2
                    : string.Format("({0}) AND ({1})", convert.ConditionSql, condition2);
            var obj = this.CollectInner("Sum", expression.Member.Name, condition, convert.Parameters);
            return obj == DBNull.Value ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">查询表达式</param>
        /// <param name="b"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public decimal Sum<TValue>(Expression<Func<TData, TValue>> field, Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var expression = (MemberExpression)field.Body;
            var convert1 = this.Compile(a);
            var convert2 = this.Compile(b);
            var obj = this.CollectInner("Sum", expression.Member.Name,
                string.Format("({0}) AND ({1})", convert1.ConditionSql, convert1.ConditionSql)
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
            return obj == DBNull.Value ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">查询表达式</param>
        /// <param name="args"></param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public decimal Sum<TValue>(Expression<Func<TData, TValue>> field, string condition, params SqlParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = this.CollectInner("Sum", expression.Member.Name, condition, args);
            return obj == DBNull.Value ? 0M : Convert.ToDecimal(obj);
        }

        #endregion

        #region 实现

        /// <summary>
        ///     是否存在数据
        /// </summary>
        protected bool ExistInner(string condition = null, SqlParameter args = null)
        {
            return this.ExistInner(condition, args == null ? new SqlParameter[0] : new[] { args });
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        protected bool ExistInner(string condition, SqlParameter[] args)
        {
            var obj = this.CollectInner("Count", "*", condition, args);
            return obj != DBNull.Value && Convert.ToInt64(obj) > 0;
        }

        /// <summary>
        ///     总数据量
        /// </summary>
        protected long CountInner(string condition = null, SqlParameter args = null)
        {
            var obj = this.CollectInner("Count", "*", condition, args);
            return obj == DBNull.Value ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     总数据量
        /// </summary>
        protected object CollectInner(string fun, string field, string condition, params SqlParameter[] args)
        {
            var sql = string.Format(@"SELECT {0}({1}) FROM {2}{3};", fun, field, this.ContextReadTable, this.ContitionSqlCode(condition));
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                return this.DataBase.ExecuteScalar(sql, args);
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
            return this.PageData(page, limit, this.KeyField, false, null, null);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> PageData(int page, int limit, string condition, params SqlParameter[] args)
        {
            return this.PageData(page, limit, this.KeyField, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> PageData(int page, int limit, Expression<Func<TData, bool>> lambda)
        {
            var convert = this.Compile(lambda);
            return this.PageData(page, limit, this.KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> PageData(int page, int limit, LambdaItem<TData> lambda)
        {
            var convert = this.Compile(lambda);
            return this.PageData(page, limit, this.KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> LoadData(int page, int limit, string order, string condition, params SqlParameter[] args)
        {
            return this.PageData(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> PageData(int page, int limit, string order, string condition, params SqlParameter[] args)
        {
            return this.PageData(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> PageData<TField>(int page, int limit, Expression<Func<TData, TField>> field, Expression<Func<TData, bool>> lambda)
        {
            return this.PageData(page, limit, field, false, lambda);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        public List<TData> PageData<TField>(int page, int limit, Expression<Func<TData, TField>> field, bool desc, Expression<Func<TData, bool>> lambda)
        {
            var convert = this.Compile(lambda);
            return this.PageData(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     分页读取
        /// </summary>
        /// <param name="page">页号(从1开始)</param>
        /// <param name="limit">每页行数</param>
        /// <param name="order">排序字段</param>
        /// <param name="desc">是否反序</param>
        /// <param name="condition">查询条件</param>
        /// <param name="args">查询参数</param>
        /// <returns></returns>
        public List<TData> PageData(int page, int limit, string order, bool desc, string condition, params SqlParameter[] args)
        {
            var results = new List<TData>();
#if DEBUG
            var orderField = string.IsNullOrWhiteSpace(order) || !this.FieldDictionary.ContainsKey(order) ? this.KeyField : this.FieldDictionary[order];
#else
            var orderField = order == null ? this.KeyField : this.FieldDictionary[order];
#endif
            var sql =
                $@"SELECT{this.ContextLoadFields}
FROM (
    SELECT ROW_NUMBER() OVER (ORDER BY [{orderField}] {(desc ? "DESC" : "ASC")}) AS __rs,{this.ContextLoadFields}
FROM {
                    this.ContextReadTable}{this.ContitionSqlCode(condition)}
) t WHERE __rs > {((page - 1) * limit)} AND __rs <= {page * limit};";

            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var cmd = this.DataBase.CreateCommand(sql, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(this.LoadEntity(reader));
                        }
                    }
                }
                for (var index = 0; index < results.Count; index++)
                {
                    results[index] = this.EntityLoaded(results[index]);
                }
            }
            return results;
        }

        #endregion
        #region 单列读取

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        public TField LoadValue<TField>(Expression<Func<TData, TField>> field, Expression<Func<TData, bool>> lambda)
        {
            var fn = GetPropertyName(field);
            var convert = this.Compile(lambda);
            var val = this.LoadValueInner(fn, convert.ConditionSql, convert.Parameters);
            return val == null || val == DBNull.Value ? default(TField) : (TField)val;
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        public TField LoadValue<TField, TKey>(Expression<Func<TData, TField>> field, TKey key)
        {
            var fn = GetPropertyName(field);
            var condition = string.Format(@"[{0}]=@{0}", this.KeyField);
            var args = new[]
            {
                this.CreateFieldParameter(this.KeyField, key)
            };
            var vl = this.LoadValueInner(fn, condition, args);
            return vl == DBNull.Value ? default(TField) : (TField)vl;
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        public List<TField> LoadValues<TField>(Expression<Func<TData, TField>> fieldExpression, Expression<Func<TData, bool>> lambda)
        {
            var field = GetPropertyName(fieldExpression);
            var convert = this.Compile(lambda);
            var result = this.LoadValuesInner(field, convert.ConditionSql, convert.Parameters);
            return result.Count == 0 ? new List<TField>() : result.Select(p => (TField)p).ToList();
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="fieldExpression">字段</param>
        /// <param name="parse">转换数据类型方法</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        public List<TField> LoadValues<TField>(Expression<Func<TData, TField>> fieldExpression, Func<object, TField> parse, Expression<Func<TData, bool>> lambda)
        {
            var field = GetPropertyName(fieldExpression);
            var convert = this.Compile(lambda);
            var result = this.LoadValuesInner(field, convert.ConditionSql, convert.Parameters);
            return result.Count == 0 ? new List<TField>() : result.Select(parse).ToList();
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public object LoadValue(string field, string condition, params SqlParameter[] args)
        {
            return this.LoadValueInner(field, condition, args);
        }
        /// <summary>
        ///     读取值
        /// </summary>
        protected object LoadValueInner(string field, string condition, params SqlParameter[] args)
        {
            var sql = string.Format(@"SELECT [{0}] FROM {1}{2};", field, this.ContextReadTable, this.ContitionSqlCode(condition));

            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                return this.DataBase.ExecuteScalar(sql, args);
            }
        }

        /// <summary>
        ///     读取多个值(SQL中的第一个字段)
        /// </summary>
        protected List<object> LoadValuesInner(string field, string condition, params SqlParameter[] args)
        {
            var sql = string.Format(@"SELECT [{0}] FROM {1}{2};", field, this.ContextReadTable, this.ContitionSqlCode(condition));
            var values = new List<object>();
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var cmd = this.DataBase.CreateCommand(sql, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            values.Add(reader.GetValue(0));
                        }
                    }
                }
            }
            return values;
        }
        #endregion
        #region 数据读取

        /// <summary>
        ///     全表读取
        /// </summary>
        public List<TData> LoadData()
        {
            return this.LoadDataInner();
        }


        /// <summary>
        ///     条件读取
        /// </summary>
        public List<TData> LoadData(string condition, params SqlParameter[] args)
        {
            return this.LoadDataInner(condition, args);
        }

        /// <summary>
        ///     主键读取
        /// </summary>
        public virtual TData LoadByPrimaryKey(object key)
        {
            return this.LoadFirstInner(this.PrimaryKeyConditionSQL, this.CreatePimaryKeyParameter(key));
        }

        /// <summary>
        ///     主键读取
        /// </summary>
        public List<TData> LoadByPrimaryKeies(IEnumerable<object> keies)
        {
            var list = new List<TData>();
            var par = this.CreatePimaryKeyParameter();
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                foreach (var key in keies)
                {
                    par.Value = key;
                    list.Add(this.LoadFirstInner(this.PrimaryKeyConditionSQL, par));
                }
            }
            return list;
        }



        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public TData LoadFirst(string condition = null)
        {
            return this.LoadFirstInner(condition);
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public TData LoadFirst(string condition, params SqlParameter[] args)
        {
            return this.LoadFirstInner(condition, args);
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public TData LoadFirst(string foreignKey, object key)
        {
            return this.LoadFirstInner(this.FieldConditionSQL(foreignKey), this.CreateFieldParameter(foreignKey, key));
        }

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        public TData LoadLast(string condition = null)
        {
            return this.LoadLastInner(condition);
        }

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        public TData LoadLast(string condition, params SqlParameter[] args)
        {
            return this.LoadLastInner(condition, args);
        }

        /// <summary>
        ///     如果存在的话读取尾行
        /// </summary>
        public TData LoadLast(string foreignKey, object key)
        {
            return this.LoadLastInner(this.FieldConditionSQL(foreignKey), this.CreateFieldParameter(foreignKey, key));
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public List<TData> LoadByForeignKey(string foreignKey, object key)
        {
            return this.LoadDataInner(this.FieldConditionSQL(foreignKey), this.CreateFieldParameter(foreignKey, key));
        }

        /// <summary>
        ///     重新读取
        /// </summary>
        public void ReLoad(TData entity)
        {
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                this.ReLoadInner(entity);
            }
            entity.OnStatusChanged(NotificationStatusType.Refresh);
        }

        /// <summary>
        ///     重新读取
        /// </summary>
        public void ReLoad(ref TData entity)
        {
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                this.ReLoadInner(entity);
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
            entity = this.OnEntityLoaded(entity);
            if (this.OnLoadAction != null)
            {
                this.OnLoadAction(entity);
            }
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
        private TData LoadEntity(SqlDataReader reader)
        {
            var entity = new TData();
            using (new EntityLoadScope(entity))
            {
                this.LoadEntity(reader, entity);
            }
            return entity;
        }

        /// <summary>
        ///     重新载入
        /// </summary>
        private void ReLoadInner(TData entity)
        {
            entity.RejectChanged();
            var cmd = this.CreateLoadCommand(this.PrimaryKeyConditionSQL, this.CreatePimaryKeyParameter(entity));
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return;
                }
                using (new EntityLoadScope(entity))
                {
                    this.LoadEntity(reader, entity);
                }
            }
            var entity2 = this.EntityLoaded(entity);
            if (entity != entity2)
            {
                entity.CopyValue(entity2);
            }
        }


        /// <summary>
        ///     读取首行
        /// </summary>
        protected TData LoadFirstInner(string condition = null, SqlParameter args = null)
        {
            return this.LoadFirstInner(condition, args == null ? new SqlParameter[0] : new[] { args });
        }

        /// <summary>
        ///     读取首行
        /// </summary>
        protected TData LoadFirstInner(string condition, SqlParameter[] args)
        {
            TData entity = null;
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var cmd = this.CreateLoadCommand(condition, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            entity = this.LoadEntity(reader);
                        }
                    }
                }
                if (entity != null)
                {
                    entity = this.EntityLoaded(entity);
                }
            }
            return entity;
        }


        /// <summary>
        ///     读取尾行
        /// </summary>
        protected TData LoadLastInner(string condition = null, SqlParameter args = null)
        {
            return this.LoadLastInner(condition, args == null ? new SqlParameter[0] : new[] { args });
        }

        /// <summary>
        ///     读取尾行
        /// </summary>
        protected TData LoadLastInner(string condition, SqlParameter[] args)
        {
            TData entity = null;
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var cmd = this.CreateLoadCommand(this.KeyField, true, condition, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            entity = this.LoadEntity(reader);
                        }
                    }
                }
                if (entity != null)
                {
                    entity = this.EntityLoaded(entity);
                }
            }
            return entity;
        }

        /// <summary>
        ///     读取全部
        /// </summary>
        protected List<TData> LoadDataInner(string condition = null, SqlParameter args = null)
        {
            return this.LoadDataInner(condition, args == null ? new SqlParameter[0] : new[] { args }, null);
        }

        /// <summary>
        ///     读取全部
        /// </summary>
        protected List<TData> LoadDataInner(string condition, SqlParameter[] args)
        {
            return this.LoadDataInner(condition, args, null);
        }

        /// <summary>
        ///     读取全部
        /// </summary>
        protected List<TData> LoadDataInner(string condition, SqlParameter[] args, string orderBy)
        {
            var results = new List<TData>();
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var cmd = this.CreateLoadCommand(condition, orderBy, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(this.LoadEntity(reader));
                        }
                    }
                }
                for (var index = 0; index < results.Count; index++)
                {
                    results[index] = this.EntityLoaded(results[index]);
                }
            }
            return results;
        }

        /// <summary>
        ///     读取全部(SQL语句是重写的,字段名称和顺序与设计时相同)
        /// </summary>
        protected List<TData> LoadDataBySql(string sql, SqlParameter[] args)
        {
            var results = new List<TData>();
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var cmd = this.DataBase.CreateCommand(sql, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(this.LoadEntity(reader));
                        }
                    }
                }
                for (var index = 0; index < results.Count; index++)
                {
                    results[index] = this.EntityLoaded(results[index]);
                }
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
            {
                this.DeleteInner(entity);
            }
            else if (entity.__EntityStatus.IsNew || !this.ExistPrimaryKey(entity.GetValue(this.KeyField)))
            {
                this.InsertInner(entity);
            }
            else
            {
                this.UpdateInner(entity);
            }
        }

        /// <summary>
        ///     保存值
        /// </summary>
        protected void SaveValueInner(string field, object value, string[] conditionFiles, object[] values)
        {
            var args = this.CreateFieldsParameters(conditionFiles, values);
            var condition = this.FieldConditionSQL(true, conditionFiles);
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                if (this.Exist(condition, args))
                {
                    var sql = string.Format(@"UPDATE [{0}] SET [{1}] = @{1}_n WHERE {2};{3}"
                        , this.WriteTableName
                        , field
                        , condition
                        , this.AfterUpdateSql(condition));
                    using (var cmd = this.DataBase.CreateCommand(sql, args))
                    {
                        cmd.Parameters.Add(new SqlParameter(field + "_n", this.GetDbType(field))
                        {
                            Value = value
                        });
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    var entity = new TData();

                    for (var idx = 0; idx < values.Length; idx++)
                    {
                        entity.SetValue(conditionFiles[idx], values[idx]);
                    }
                    entity.SetValue(field, value);
                    this.InsertInner(entity);
                }
            }
        }

        /// <summary>
        ///     更新数据
        /// </summary>
        /// <param name="entity">更新数据的实体</param>
        protected bool InsertInner(TData entity)
        {
            this.PrepareSave(entity, EntitySubsist.Adding);
            using (var cmd = this.DataBase.CreateCommand())
            {
                bool isIdentitySql = this.SetInsertCommand(entity, cmd);
                foreach (var handler in this._updateHandlers.Where(p => p.AfterInner))
                {
                    handler.PrepareExecSql(this, cmd, entity, EntitySubsist.Adding);
                }
                if (isIdentitySql)
                {
                    SqlServerDataBase.TraceSql(cmd);
                    var key = cmd.ExecuteScalar();
                    if (key == DBNull.Value)
                    {
                        return false;
                    }
                    entity.SetValue(this.KeyField, key);
                }
                else
                {
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        return false;
                    }
                }
            }
            this.EndSaved(entity, EntitySubsist.Added);
            return true;
        }

        /// <summary>
        ///     插入数据
        /// </summary>
        /// <param name="entity">插入数据的实体</param>
        private void UpdateInner(TData entity)
        {
            //if (!entity.__EntityStatus.IsModified)
            //    return;
            this.PrepareSave(entity, EntitySubsist.Modify);
            using (var cmd = this.DataBase.CreateCommand())
            {
                this.SetUpdateCommand(entity, cmd);
                SqlServerDataBase.TraceSql(cmd);

                foreach (var handler in this._updateHandlers.Where(p => p.AfterInner))
                {
                    handler.PrepareExecSql(this, cmd, entity, EntitySubsist.Modify);
                }
                cmd.ExecuteNonQuery();
            }
            this.EndSaved(entity, EntitySubsist.Modified);
        }

        /// <summary>
        ///     删除
        /// </summary>
        private int DeleteInner(TData entity)
        {
            if (entity == null)
            {
                return 0;
            }
            entity.__EntityStatus.IsDelete = true;
            this.PrepareSave(entity, EntitySubsist.Deleting);
            this.DeleteInner(this.PrimaryKeyConditionSQL, this.CreatePimaryKeyParameter(entity));
            this.EndSaved(entity, EntitySubsist.Deleted);
            return 1;
        }

        /// <summary>
        ///     删除
        /// </summary>
        private int DeleteInner(string condition = null, SqlParameter args = null)
        {
            return this.DeleteInner(condition, args == null ? new SqlParameter[0] : new[] { args });
        }

        /// <summary>
        ///     删除
        /// </summary>
        private int DeleteInner(string condition, SqlParameter[] args)
        {
            if (string.IsNullOrEmpty(this.DeleteSqlCode))
            {
                return 0;
            }
            if (!string.IsNullOrEmpty(condition))
            {
                return this.DataBase.Execute(string.Format(@"{0} WHERE {1};{2}", this.DeleteSqlCode, condition, this.AfterUpdateSql(condition)), args);
            }
            this.DataBase.Clear(this.WriteTableName);
            return int.MaxValue;
        }

        #endregion

        #region 事件

        /// <summary>
        ///     保存前处理(Insert或Update)
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="subsist">当前实体生存状态</param>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        protected void PrepareSave(TData entity, EntitySubsist subsist)
        {
            foreach (var handler in this._updateHandlers.Where(p => !p.AfterInner))
            {
                handler.PrepareSave(this, entity, subsist);
            }
            if (!this.IsBaseClass)
            {
                entity.LaterPeriodByModify(subsist);
            }
            this.OnPrepareSave(entity, subsist);
            foreach (var handler in this._updateHandlers.Where(p => p.AfterInner))
            {
                handler.PrepareSave(this, entity, subsist);
            }
        }

        /// <summary>
        ///     保存前处理(Insert或Update)
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="subsist">当前实体生存状态</param>
        protected virtual void OnPrepareSave(TData entity, EntitySubsist subsist)
        {
        }

        /// <summary>
        ///     保存完成后期处理(Insert或Update)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="subsist">当前实体生存状态</param>
        private void EndSaved(TData entity, EntitySubsist subsist)
        {
            foreach (var handler in this._updateHandlers.Where(p => !p.AfterInner))
            {
                handler.EndSaved(this, entity, subsist);
            }
            if (!this.IsBaseClass)
            {
                switch (subsist)
                {
                    case EntitySubsist.Added:
                        entity.OnStatusChanged(NotificationStatusType.Added);
                        break;
                    case EntitySubsist.Deleted:
                        entity.OnStatusChanged(NotificationStatusType.Deleted);
                        break;
                    case EntitySubsist.Modified:
                        entity.OnStatusChanged(NotificationStatusType.Modified);
                        break;
                }
                entity.LaterPeriodByModify(subsist);
                entity.AcceptChanged();
            }
            this.OnDataSaved(entity);
            foreach (var handler in this._updateHandlers.Where(p => p.AfterInner))
            {
                handler.EndSaved(this, entity, subsist);
            }
        }

        /// <summary>
        ///     保存完成后期处理(Insert或Update)
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void OnDataSaved(TData entity)
        {
        }

        #endregion

        #region 数据操作

        /// <summary>
        ///     保存数据
        /// </summary>
        public void Save(IEnumerable<TData> entities)
        {
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(this.DataBase))
                {
                    foreach (var entity in entities)
                    {
                        this.SaveInner(entity);
                    }
                    scope.SetState(true);
                }
            }
        }

        /// <summary>
        ///     保存数据
        /// </summary>
        public void Save(TData entity)
        {
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(this.DataBase))
                {
                    this.SaveInner(entity);
                    scope.SetState(true);
                }
            }
        }

        /// <summary>
        ///     更新数据
        /// </summary>
        public void Update(TData entity)
        {
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(this.DataBase))
                {
                    this.UpdateInner(entity);
                    scope.SetState(true);
                }
                this.ReLoadInner(entity);
            }
        }

        /// <summary>
        ///     更新数据
        /// </summary>
        public void Update(IEnumerable<TData> entities)
        {
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(this.DataBase))
                {
                    foreach (var entity in entities)
                    {
                        this.UpdateInner(entity);
                    }
                    scope.SetState(true);
                }
            }
        }

        /// <summary>
        ///     插入新数据
        /// </summary>
        public bool Insert(TData entity)
        {
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(this.DataBase))
                {
                    if (!this.InsertInner(entity))
                    {
                        return false;
                    }
                    this.ReLoadInner(entity);
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
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(this.DataBase))
                {
                    foreach (var entity in datas)
                    {
                        this.InsertInner(entity);
                    }
                    scope.SetState(true);
                }
                foreach (var entity in datas)
                {
                    this.ReLoadInner(entity);
                }
            }
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        public void Delete(IEnumerable<TData> entities)
        {
            var datas = entities as TData[] ?? entities.ToArray();
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(this.DataBase))
                {
                    foreach (var entity in datas)
                    {
                        this.DeleteInner(entity);
                    }
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
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(this.DataBase))
                {
                    cnt = this.DeleteInner(entity);
                    scope.SetState(true);
                }
            }
            return cnt;
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        public int DeletePrimaryKey(object key)
        {
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                return this.Delete(this.LoadByPrimaryKey(key));
            }
        }

        /// <summary>
        ///     清除所有数据
        /// </summary>
        public void Clear()
        {
            //throw new Exception("批量删除功能被禁用");
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                this.DataBase.Clear(this.WriteTableName);
            }
        }

        /// <summary>
        ///     条件删除
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public int Delete(Expression<Func<TData, bool>> lambda)
        {
            //throw new Exception("批量删除功能被禁用");
            var convert = this.Compile(lambda);
            return this.Delete(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     强制物理删除
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public int Destroy(Expression<Func<TData, bool>> lambda)
        {
            var convert = this.Compile(lambda);
            int cnt;
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(this.DataBase))
                {
                    cnt = this.DataBase.Execute(string.Format(@"DELETE FROM [{0}] WHERE {1};{2}"
                            , this.WriteTableName
                            , convert.ConditionSql
                            , this.AfterUpdateSql(convert.ConditionSql))
                            , convert.Parameters);

                    scope.SetState(true);
                }
            }
            return cnt;
        }

        /// <summary>
        ///     条件删除
        /// </summary>
        public int Delete(string condition, params SqlParameter[] args)
        {
            //throw new Exception("批量删除功能被禁用");
            if (string.IsNullOrWhiteSpace(condition))
            {
                throw new ArgumentException(@"删除条件不能为空,因为不允许执行全表删除", "condition");
            }
            int cnt;
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(this.DataBase))
                {
                    cnt = this.DeleteInner(condition, args);
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
            this.SaveValueInner(field, value, conditionFiles, values);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        public int SetValue(string field, object value, string condition, params SqlParameter[] args)
        {
            if (!this.FieldDictionary.ContainsKey(field))
            {
                throw new ArgumentException(@"更新字段不能为空或不存在", "field");
            }
            field = this.FieldDictionary[field];
            string sql = string.IsNullOrWhiteSpace(condition)
                ? string.Format(@"UPDATE [{0}] SET [{1}]=@{1};{2}", this.WriteTableName, field, this.AfterUpdateSql(null))
                : string.Format(@"UPDATE [{0}] SET [{1}]=@{1} WHERE {2};{3}", this.WriteTableName, field, condition, this.AfterUpdateSql(null));
            var arg2 = new List<SqlParameter>();
            if (args != null)
            {
                arg2.AddRange(args);
            }
            arg2.Add(this.CreateFieldParameter(field, value));
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(this.DataBase))
                {
                    var result = this.DataBase.Execute(sql, arg2.ToArray());
                    scope.SetState(true);
                    return result;
                }
            }
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="condition">更新条件</param>
        /// <param name="args">条件参数</param>
        /// <returns>更新行数</returns>
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value, string condition, params SqlParameter[] args)
        {
            return this.SetValue(GetPropertyName(field), value, condition, args);
        }

        /// <summary>
        ///     全量更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>更新行数</returns>
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value)
        {
            return this.SetValue(GetPropertyName(field), value, null, null);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="lambda">条件</param>
        /// <returns>更新行数</returns>
        public int SetValue<TField>(Expression<Func<TData, TField>> field, TField value, Expression<Func<TData, bool>> lambda)
        {
            var convert = this.Compile(lambda);
            return this.SetValue(GetPropertyName(field), value, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     条件更新
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="key">主键</param>
        /// <returns>更新行数</returns>
        public int SetValue<TKey>(string field, object value, TKey key)
        {
            if (!this.FieldDictionary.ContainsKey(field))
            {
                throw new ArgumentException(@"更新字段不能为空或不存在", "field");
            }
            field = this.FieldDictionary[field];
            string condition = string.Format("[{0}]=@{0}", this.KeyField);
            var sql = string.Format(@"UPDATE [{0}] SET [{1}]=@{1} WHERE {2};{3}", this.WriteTableName, field, condition, this.AfterUpdateSql(condition));
            var arg2 = new List<SqlParameter>
            {
                this.CreateFieldParameter(this.KeyField, key),
                this.CreateFieldParameter(field, value)
            };
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(this.DataBase))
                {
                    var result = this.DataBase.Execute(sql, arg2.ToArray());
                    scope.SetState(true);
                    return result;
                }
            }
        }

        #endregion

        #region 简单更新

        /// <summary>
        ///     与更新同时执行的SQL
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
        /// <param name="args">参数</param>
        /// <returns>更新行数</returns>
        public int SetValue(string expression, Expression<Func<TData, bool>> condition, params SqlParameter[] args)
        {
            var convert = this.Compile(condition);
            var sql = string.Format(@"UPDATE [{0}] SET {1} WHERE {2};{3}"
                , this.WriteTableName
                , expression
                , convert.ConditionSql
                , this.AfterUpdateSql(convert.ConditionSql));
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(this.DataBase))
                {
                    if (args.Length == 0)
                    {
                        return this.DataBase.Execute(sql, convert.Parameters);
                    }
                    if (convert.Parameters.Length == 0)
                    {
                        return this.DataBase.Execute(sql, args);
                    }
                    var arg2 = convert.Parameters.ToList();
                    arg2.AddRange(args);
                    var result = this.DataBase.Execute(sql, arg2.ToArray());
                    scope.SetState(true);
                    return result;
                }
            }
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
            var field = GetPropertyName(fieldExpression);
            string condition = string.Format("[{0}]=@{0}", this.KeyField);
            var sql = string.Format(@"UPDATE [{0}] SET [{1}]=@{1} WHERE {2};{3}"
                , this.WriteTableName
                , field
                , condition
                , this.AfterUpdateSql(condition));
            var arg2 = new List<SqlParameter>
            {
                this.CreateFieldParameter(this.KeyField, key),
                this.CreateFieldParameter(field, value)
            };
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(this.DataBase))
                {
                    var result = this.DataBase.Execute(sql, arg2.ToArray());
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
            string condition = string.Format("[{0}]=@{0}", this.KeyField);
            var sql = string.Format(@"UPDATE [{0}] SET {1} WHERE {2};{3}"
                , this.WriteTableName
                , valueExpression
                , condition
                , this.AfterUpdateSql(condition));
            var arg2 = new List<SqlParameter>
            {
                this.CreateFieldParameter(this.KeyField, key)
            };
            using (SqlServerDataBaseScope.CreateScope(this.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(this.DataBase))
                {
                    var result = this.DataBase.Execute(sql, arg2.ToArray());
                    scope.SetState(true);
                    return result;
                }
            }
        }

        #endregion

        #region 数据操作事件

        /// <summary>
        ///     更新注入处理器
        /// </summary>
        private readonly List<IDataUpdateHandler<TData>> _updateHandlers = new List<IDataUpdateHandler<TData>>();

        /// <summary>
        ///     注册数据更新注入器
        /// </summary>
        public void RegisterUpdateHandler(IDataUpdateHandler<TData> handler)
        {
            if (this._updateHandlers.Contains(handler))
            {
                this._updateHandlers.Add(handler);
            }
        }

        /// <summary>
        ///     反注册数据更新注入器
        /// </summary>
        public void UnRegisterUpdateHandler(IDataUpdateHandler<TData> handler)
        {
            this._updateHandlers.Remove(handler);
        }

        #endregion

        #endregion

        #region 纯虚方法

        /// <summary>
        ///     设置更新数据的命令
        /// </summary>
        protected abstract void SetUpdateCommand(TData entity, SqlCommand cmd);

        /// <summary>
        ///     设置插入数据的命令
        /// </summary>
        /// <returns>返回真说明要取主键</returns>
        protected abstract bool SetInsertCommand(TData entity, SqlCommand cmd);

        /// <summary>
        ///     载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        protected abstract void LoadEntity(SqlDataReader reader, TData entity);

        #endregion

        #region 生成命令对象

        /// <summary>
        ///     生成命令
        /// </summary>
        protected SqlCommand CreateLoadCommand(string condition, params SqlParameter[] args)
        {
            return this.CreateLoadCommand(condition, null, args);
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        protected SqlCommand CreateLoadCommand(string condition, string @orderby, params SqlParameter[] args)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"SELECT");
            sql.AppendLine(this.ContextLoadFields);
            sql.AppendFormat(@"FROM {0}", this.ContextReadTable);
            sql.AppendLine(this.ContitionSqlCode(condition));
            if (!string.IsNullOrWhiteSpace(@orderby))
            {
                sql.AppendLine();
                sql.Append("ORDER BY ");
                sql.Append(@orderby);
            }
            sql.Append(";");
            return this.DataBase.CreateCommand(sql.ToString(), args);
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        protected SqlCommand CreateLoadCommand(string orderby, bool desc, string condition, params SqlParameter[] args)
        {
            var orderSql = string.Format("[{0}] {1}", this.FieldDictionary[!string.IsNullOrEmpty(@orderby) ? @orderby : this.KeyField], desc ? "DESC" : "");
            return this.CreateLoadCommand(condition, orderSql, args);
        }

        #endregion

        #region 字段的参数帮助

        /// <summary>
        ///     得到字段的SqlDbType类型
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>参数</returns>
        protected virtual SqlDbType GetDbType(string field)
        {
            return SqlDbType.NVarChar;
        }


        private string _primaryConditionSQL;

        /// <summary>
        ///     主键的条件部分SQL
        /// </summary>
        public string PrimaryKeyConditionSQL
        {
            get { return this._primaryConditionSQL ?? (this._primaryConditionSQL = this.FieldConditionSQL(this.KeyField)); }
        }

        /// <summary>
        ///     生成多个字段的参数
        /// </summary>
        /// <param name="fields">生成参数的字段</param>
        public SqlParameter[] CreateFieldsParameters(params string[] fields)
        {
            if (fields == null || fields.Length == 0)
            {
                throw new ArgumentException(@"没有字段用于生成参数", "fields");
            }
            return fields.Select(field => new SqlParameter(field, this.GetDbType(field))).ToArray();
        }

        /// <summary>
        ///     生成多个字段的参数
        /// </summary>
        /// <param name="fields">生成参数的字段</param>
        /// <param name="values">生成参数的值(长度和字段长度必须一致)</param>
        public SqlParameter[] CreateFieldsParameters(string[] fields, object[] values)
        {
            if (fields == null || fields.Length == 0)
            {
                throw new ArgumentException(@"没有字段用于生成参数", "fields");
            }
            if (values == null || values.Length == 0)
            {
                throw new ArgumentException(@"没有值用于生成参数", "values");
            }
            if (values.Length != fields.Length)
            {
                throw new ArgumentException(@"值的长度和字段长度必须一致", "values");
            }
            var res = fields.Select(field => new SqlParameter(field, this.GetDbType(field))).ToArray();
            for (var i = 0; i < fields.Length; i++)
            {
                res[i].Value = values[i];
            }
            return res;
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        public SqlParameter CreateFieldParameter(string field)
        {
            return new SqlParameter(field, this.GetDbType(field));
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        /// <param name="value">值</param>
        public SqlParameter CreateFieldParameter(string field, object value)
        {
            if (value is Enum)
            {
                return new SqlParameter(field, SqlDbType.Int)
                {
                    Value = Convert.ToInt32(value)
                };
            }
            return new SqlParameter(field, this.GetDbType(field))
            {
                Value = value ?? DBNull.Value
            };
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        /// <param name="entity">取值的实体</param>
        public SqlParameter CreateFieldParameter(string field, TData entity)
        {
            return this.CreateFieldParameter(field, entity.GetValue(field));
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        /// <param name="entity">取值的实体</param>
        /// <param name="entityField">取值的字段</param>
        public SqlParameter CreateFieldParameter(string field, DataObjectBase entity, string entityField)
        {
            return this.CreateFieldParameter(field, entity.GetValue(entityField));
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        public SqlParameter CreatePimaryKeyParameter()
        {
            return new SqlParameter(this.KeyField, this.GetDbType(this.KeyField));
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        /// <param name="value">主键值</param>
        public SqlParameter CreatePimaryKeyParameter(object value)
        {
            return new SqlParameter(this.KeyField, this.GetDbType(this.KeyField))
            {
                Value = value
            };
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        /// <param name="entity">取值的实体</param>
        public SqlParameter CreatePimaryKeyParameter(TData entity)
        {
            return new SqlParameter(this.KeyField, this.GetDbType(this.KeyField))
            {
                Value = entity.GetValue(this.KeyField)
            };
        }

        /// <summary>
        ///     主键的条件部分SQL
        /// </summary>
        public string FieldConditionSQL(string field)
        {
            return string.Format(@"[{0}] = @{1}", this.FieldDictionary[field], field);
        }

        /// <summary>
        ///     连接条件SQL
        /// </summary>
        /// <param name="isAnd">是否用AND组合</param>
        /// <param name="conditions">条件</param>
        public string JoinConditionSQL(bool isAnd, params string[] conditions)
        {
            if (conditions == null || conditions.Length == 0)
            {
                throw new ArgumentException(@"没有条件用于组合", "conditions");
            }
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", conditions[0]);
            for (var idx = 1; idx < conditions.Length; idx++)
            {
                sql.AppendFormat(@" {0} ({1}) ", isAnd ? "AND" : "OR", conditions[idx]);
            }
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
            {
                throw new ArgumentException(@"没有字段用于生成组合条件", "fields");
            }
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", this.FieldConditionSQL(fields[0]));
            for (var idx = 1; idx < fields.Length; idx++)
            {
                sql.AppendFormat(@" {0} ({1}) ", isAnd ? "AND" : "OR", this.FieldConditionSQL(fields[idx]));
            }
            return sql.ToString();
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
            {
                throw new ArgumentException(@"没有字段用于生成组合条件", "fields");
            }
            return new ConditionItem
            {
                ConditionSql = this.FieldConditionSQL(isAnd, fields),
                Parameters = this.CreateFieldsParameters(fields)
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
            {
                throw new ArgumentException(@"没有字段用于生成组合条件", "fields");
            }
            return new ConditionItem
            {
                ConditionSql = this.FieldConditionSQL(isAnd, fields),
                Parameters = this.CreateFieldsParameters(fields, values)
            };
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
        public bool IsUnique<TValue>(Expression<Func<TData, TValue>> field, object val, Expression<Func<TData, bool>> condition)
        {
            if (Equals(val, default(TValue)))
            {
                return false;
            }
            var fieldName = GetPropertyName(field);
            var convert = this.Compile(condition);
            convert.AddAndCondition(string.Format("([{0}] = @c_vl_)", this.FieldDictionary[fieldName]), new SqlParameter
            {
                ParameterName = "c_vl_",
                SqlDbType = this.GetDbType(fieldName),
                Value = val
            });
            return this.Exist(convert.ConditionSql, convert.Parameters);
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
            {
                return false;
            }
            var fieldName = GetPropertyName(field);
            return this.Exist(string.Format("([{0}] = @c_vl_ AND {1}", this.FieldDictionary[fieldName], this.PrimaryKeyConditionSQL)
                , new SqlParameter
                {
                    ParameterName = "c_vl_",
                    SqlDbType = this.GetDbType(fieldName),
                    Value = val
                }
                , this.CreatePimaryKeyParameter(key));
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
            {
                return false;
            }
            var fieldName = GetPropertyName(field);
            return this.Exist(string.Format("([{0}] = @c_vl_)", this.FieldDictionary[fieldName]), new SqlParameter
            {
                ParameterName = "c_vl_",
                SqlDbType = this.GetDbType(fieldName),
                Value = val
            });
        }

        #endregion
    }
}