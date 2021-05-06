/*****************************************************
(c)2016-2021 by ZeroTeam
作者: 胡天水
工程: Agebull.EntityModel.CoreAgebull.DataModel
建立: 忘了日期
修改: -
*****************************************************/

#region 引用

using System;
using System.Data.Common;
using System.Linq.Expressions;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     Sql实体访问类
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    public class DataAccessBase<TEntity>
         where TEntity : class, new()
    {
        #region 构造

        /// <summary>
        /// 实体操作
        /// </summary>
        public IEntityOperator<TEntity> EntityOperator => Provider.EntityOperator;

        /// <summary>
        /// 自动构建数据库对象
        /// </summary>
        public IDataOperator<TEntity> DataOperator => Provider.DataOperator;

        /// <summary>
        /// 参数构造
        /// </summary>
        public IParameterCreater ParameterCreater => Provider.ParameterCreater;

        /// <summary>
        /// Sql语句构造器
        /// </summary>
        public DataAccessOption Option => Provider.Option;

        /// <summary>
        /// Sql语句构造器
        /// </summary>
        public ISqlBuilder<TEntity> SqlBuilder => Provider.SqlBuilder;

        /// <summary>
        /// 驱动提供者信息
        /// </summary>
        public IDataAccessProvider<TEntity> Provider { get; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="provider"></param>
        protected DataAccessBase(IDataAccessProvider<TEntity> provider)
        {
            Provider = provider;
        }

        #endregion

        #region 数据库

        /// <summary>
        ///     自动数据连接对象
        /// </summary>
        public IDataBase DataBase
        {
            get => OriDataBase ??= Provider.CreateDataBase();
            set => OriDataBase = value;
        }

        /// <summary>
        ///     无懒构造数据库对象
        /// </summary>
        public IDataBase OriDataBase { get; protected set; }

        #endregion

        #region 字段的参数帮助

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
                res[i] = ParameterCreater.CreateParameter(fields[i], values[i], DataOperator.GetDbType(fields[i]));
            return res;
        }

        #endregion

        #region 取属性名称

        /// <summary>
        ///     取属性名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        protected static string GetPropertyName<T>(Expression<Func<TEntity, T>> action)
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