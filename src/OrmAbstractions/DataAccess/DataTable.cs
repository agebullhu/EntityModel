// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using Agebull.Common.Ioc;
using Gboxt.Common.DataModel;

#endregion

namespace Agebull.Orm.Abstractions
{
    /// <summary>
    ///     Sql实体访问类
    /// </summary>
    /// <typeparam name="TData">实体</typeparam>
    /// <typeparam name="TSqliteDataBase">所在的数据库对象,可通过Ioc自动构造</typeparam>
    public abstract partial class DataTable<TData, TSqliteDataBase> : SimpleConfig, IDataTable
        where TData : EditDataObject, new()
        where TSqliteDataBase : OrmDataBase
    {
        #region 数据库

        /// <summary>
        ///     自动数据连接对象
        /// </summary>
        private OrmDataBase _dataBase;

        /// <summary>
        ///     自动数据连接对象
        /// </summary>
        public OrmDataBase DataBase
        {
            get => _dataBase ?? (_dataBase = IocHelper.Create<TSqliteDataBase>());
            set => _dataBase = value;
        }

        /// <summary>
        ///     自动数据连接对象
        /// </summary>
        IDataBase IDataTable.DataBase => DataBase;

        #endregion

        #region 数据结构

        /// <summary>
        ///     是否作为基类存在的
        /// </summary>
        public bool IsBaseClass { get; set; }

        /// <summary>
        ///     设计时的主键字段
        /// </summary>
        string IDataTable.PrimaryKey => PrimaryKey;

        /// <summary>
        ///     表名
        /// </summary>
        string IDataTable.ReadTableName => ReadTableName;

        /// <summary>
        ///     写表名
        /// </summary>
        string IDataTable.WriteTableName => WriteTableName;

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

        #region 生成命令对象

        /// <summary>
        ///     生成命令
        /// </summary>
        protected DbCommand CreateLoadCommand(string condition, params DbParameter[] args)
        {
            return CreateLoadCommand(condition, null, args);
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        protected DbCommand CreateLoadCommand(string condition, string order, params DbParameter[] args)
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
        protected DbCommand CreateLoadCommand(string order, bool desc, string condition,
            params DbParameter[] args)
        {
            var field = !string.IsNullOrEmpty(order) ? order : KeyField;
            Debug.Assert(FieldDictionary.ContainsKey(field));
            var orderSql = $"[{FieldDictionary[field]}] {(desc ? "DESC" : "")}";
            return CreateLoadCommand(condition, orderSql, args);
        }

        #endregion

        #region 字段的参数帮助

        /// <summary>
        ///     得到字段的SqliteDbType类型
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>参数</returns>
        protected virtual DbType GetDbType(string field)
        {
            return DbType.String;
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
        public DbParameter[] CreateFieldsParameters(params string[] fields)
        {
            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"没有字段用于生成参数", nameof(fields));
            return fields.Select(field => new DbParameter(field, GetDbType(field))).ToArray();
        }

        /// <summary>
        ///     生成多个字段的参数
        /// </summary>
        /// <param name="fields">生成参数的字段</param>
        /// <param name="values">生成参数的值(长度和字段长度必须一致)</param>
        public DbParameter[] CreateFieldsParameters(string[] fields, object[] values)
        {
            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"没有字段用于生成参数", nameof(fields));
            if (values == null || values.Length == 0)
                throw new ArgumentException(@"没有值用于生成参数", nameof(values));
            if (values.Length != fields.Length)
                throw new ArgumentException(@"值的长度和字段长度必须一致", nameof(values));
            var res = new DbParameter[fields.Length];
            for (var i = 0; i < fields.Length; i++)
                res[i] = CreateFieldParameter(fields[i], values[i]);
            return res;
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        public DbParameter CreateFieldParameter(string field)
        {
            return new DbParameter(field, GetDbType(field));
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        /// <param name="value">值</param>
        public DbParameter CreateFieldParameter(string field, object value)
        {
            return OrmDataBase.CreateParameter(field, value, GetDbType(field));
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        /// <param name="entity">取值的实体</param>
        public DbParameter CreateFieldParameter(string field, TData entity)
        {
            return CreateFieldParameter(field, entity.GetValue(field));
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        /// <param name="entity">取值的实体</param>
        /// <param name="entityField">取值的字段</param>
        public DbParameter CreateFieldParameter(string field, DataObjectBase entity, string entityField)
        {
            return CreateFieldParameter(field, entity.GetValue(entityField));
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        public DbParameter CreatePimaryKeyParameter()
        {
            return new DbParameter(KeyField, GetDbType(KeyField));
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        /// <param name="value">主键值</param>
        public DbParameter CreatePimaryKeyParameter(object value)
        {
            return OrmDataBase.CreateParameter(KeyField, value, GetDbType(KeyField));
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        /// <param name="entity">取值的实体</param>
        public DbParameter CreatePimaryKeyParameter(TData entity)
        {
            return DataBase.CreateParameter(KeyField, entity.GetValue(KeyField),
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


        #region 数据库

        /// <summary>
        /// 按修改更新
        /// </summary>
        public bool UpdateByMidified { get; set; }

        /// <summary>
        ///     表的唯一标识
        /// </summary>
        public abstract int TableId { get; }

        #endregion

        #region 字段字典

        /// <summary>
        ///     设计时的主键字段
        /// </summary>
        protected abstract string PrimaryKey { get; }

        /// <summary>
        ///     字段字典
        /// </summary>
        private Dictionary<string, string> _fieldMap;

        /// <summary>
        ///     字段字典(设计时)
        /// </summary>
        public virtual Dictionary<string, string> FieldMap
        {
            get { return _fieldMap ?? (_fieldMap = Fields.ToDictionary(p => p, p => p)); }
        }

        /// <summary>
        ///     所有字段(设计时)
        /// </summary>
        public abstract string[] Fields { get; }

        /// <summary>
        ///     字段字典(动态覆盖)
        /// </summary>
        public Dictionary<string, string> OverrideFieldMap { get; set; }
        

        #endregion


        #region 动态上下文扩展


        /// <summary>
        ///     动态读取的字段
        /// </summary>
        internal string DynamicReadFields;

        /// <summary>
        ///     动态读取的表
        /// </summary>
        protected string DynamicReadTable;

        /// <summary>
        /// 当前上下文的读取器
        /// </summary>
        public Action<ReaderScope, TData> DynamicLoadAction { get; set; }

        /// <summary>
        ///     取得实际设置的ContextReadTable动态读取的表
        /// </summary>
        /// <returns>之前的动态读取的表名</returns>
        public string SetDynamicReadTable(string table)
        {
            var old = DynamicReadTable;
            DynamicReadTable = string.IsNullOrWhiteSpace(table) ? null : table;
            return old;
        }

        /// <summary>
        ///     动态读取的字段
        /// </summary>
        protected string ContextLoadFields
        {
            get => DynamicReadFields ?? FullLoadFields;
            set => DynamicReadFields = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        /// <summary>
        ///     当前上下文读取的表名
        /// </summary>
        protected string ContextReadTable => DynamicReadTable ?? ReadTableName;

        #endregion

        #region 简单读取

        /// <summary>
        /// 简单读取SQL语句
        /// </summary>
        public virtual string SimpleFields => FullLoadFields;


        /// <summary>
        /// 简单读取载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public virtual void SimpleLoad(ReaderScope reader, TData entity)
        {
            LoadEntity(reader, entity);
        }
        #endregion

        #region 纯虚方法

        /// <summary>
        ///     设置更新数据的命令
        /// </summary>
        protected virtual void SetUpdateCommand(TData entity, DbCommand cmd)
        {
        }

        /// <summary>
        ///     设置插入数据的命令
        /// </summary>
        /// <returns>返回真说明要取主键</returns>
        protected virtual bool SetInsertCommand(TData entity, DbCommand cmd)
        {
            return false;
        }

        /// <summary>
        ///     载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        protected virtual void LoadEntity(ReaderScope reader, TData entity)
        {
        }

        #endregion
    }
}