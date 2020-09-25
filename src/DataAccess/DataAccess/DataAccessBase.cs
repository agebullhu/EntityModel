// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     Sql实体访问类
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    public sealed partial class DataAccess<TEntity> : SimpleConfig, IDataAccess//<TEntity>
         where TEntity : class, new()
    {
        #region 构造

        /// <summary>
        /// 参数构造
        /// </summary>
        public IParameterCreater ParameterCreater => SqlBuilder;

        /// <summary>
        /// Sql语句构造器
        /// </summary>
        public ISqlBuilder<TEntity> SqlBuilder { get; set; }

        /// <summary>
        /// Sql语句构造器
        /// </summary>
        ISqlBuilder IDataAccess.SqlBuilder => SqlBuilder;

        /// <summary>
        /// 表配置
        /// </summary>
        public EntitySturct DataSturct { get; set; }

        /// <summary>
        /// Sql配置信息
        /// </summary>
        DataAccessOption IDataAccess.Option => Option;

        /// <summary>
        /// Sql配置信息
        /// </summary>
        public DataAccessOption<TEntity> Option { get; }

        public DataAccess(DataAccessOption<TEntity> option)
        {
            Option = option;
            Option.DataAccess = this;
            SqlBuilder = option.SqlBuilder;
            DataSturct = option.DataSturct;
            CreateDataBase = option.CreateDataBase;
        }

        #endregion

        #region 数据库


        /// <summary>
        /// 自动构建数据库对象
        /// </summary>
        public Func<IDataBase> CreateDataBase { get; set; }

        /// <summary>
        ///     自动数据连接对象
        /// </summary>
        private IDataBase _dataBase;

        /// <summary>
        ///     自动数据连接对象
        /// </summary>
        public IDataBase DataBase
        {
            get => _dataBase ??= CreateDataBase();
            set => _dataBase = value;
        }

        /// <summary>
        ///     无懒构造数据库对象
        /// </summary>
        public IDataBase OriDataBase => _dataBase;

        #endregion

        #region 生成命令对象

        /// <summary>
        ///     生成命令
        /// </summary>
        private DbCommand CreateLoadCommand(IConnectionScope scope, string condition, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateLoadSql(condition, null, null);
            return scope.CreateCommand(sql.ToString(), args);
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        private DbCommand CreateLoadCommand(IConnectionScope scope, string condition, string order, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateLoadSql(condition, order, null);
            return scope.CreateCommand(sql.ToString(), args);
        }

        /// <summary>
        ///     生成载入命令
        /// </summary>
        /// <param name="scope">连接使用范围</param>
        /// <param name="order">排序字段</param>
        /// <param name="desc">是否倒序</param>
        /// <param name="condition">数据条件</param>
        /// <param name="args">条件中的参数</param>
        /// <returns>载入命令</returns>
        private DbCommand CreateLoadCommand(IConnectionScope scope, string order, bool desc, string condition,
            params DbParameter[] args)
        {
            var field = !string.IsNullOrEmpty(order) ? order : Option.PrimaryKey;

            string orderSql = SqlBuilder.OrderSql(desc, field);
            return CreateLoadCommand(scope, condition: condition, orderSql, args);
        }

        #endregion

        #region 字段的参数帮助

        /// <summary>
        ///     生成多个字段的参数
        /// </summary>
        /// <param name="fields">生成参数的字段</param>
        public DbParameter[] CreateFieldsParameters(params string[] fields)
        {
            if (fields == null || fields.Length == 0)
                throw new ArgumentException(@"没有字段用于生成参数", nameof(fields));
            return fields.Select(field => ParameterCreater.CreateParameter(field, SqlBuilder.GetDbType(field))).ToArray();
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
                res[i] = ParameterCreater.CreateParameter(fields[i], values[i], SqlBuilder.GetDbType(fields[i]));
            return res;
        }


        /// <summary>
        ///     生成多个字段的参数
        /// </summary>
        /// <param name="parameters">生成参数的字段</param>
        public DbParameter[] CreateFieldsParameters(params (string field, object value)[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                throw new ArgumentException(@"没有字段用于生成参数", nameof(parameters));
            var res = new DbParameter[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
                res[i] = ParameterCreater.CreateParameter(parameters[i].field, parameters[i].value, SqlBuilder.GetDbType(parameters[i].field));
            return res;
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
                throw new ArgumentException(@"没有字段用于生成参数", nameof(fields));
            var paras = fields.Select(field => ParameterCreater.CreateParameter(field, SqlBuilder.GetDbType(field))).ToArray();

            return new ConditionItem
            {
                ParameterCreater = ParameterCreater,
                ConditionSql = SqlBuilder.FieldConditionSQL(isAnd, fields),
                Parameters = paras
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
                ParameterCreater = ParameterCreater,
                ConditionSql = SqlBuilder.FieldConditionSQL(isAnd, fields),
                Parameters = CreateFieldsParameters(fields, values)
            };
        }

        #endregion

        #region 取属性名称

        /// <summary>
        ///     取属性名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        private static string GetPropertyName<T>(Expression<Func<TEntity, T>> action)
        {
            if (action.Body is MemberExpression expression)
                return expression.Member.Name;
            if (!(action.Body is UnaryExpression body))
                throw new EntityModelDbException("表达式太复杂");

            expression = (MemberExpression)body.Operand;
            return expression.Member.Name;
        }

        #endregion

    }
}