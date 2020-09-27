using System;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 数据访问提供器
    /// </summary>
    public class DataAccessProvider<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 依赖对象
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 配置
        /// </summary>
        public DataAccessOption<TEntity> Option { get; set; }

        /// <summary>
        /// 参数构造
        /// </summary>
        public IParameterCreater ParameterCreater => SqlBuilder;

        /// <summary>
        /// 自动构建数据库对象
        /// </summary>
        public IDataOperator<TEntity> DataOperator { get; set; }

        /// <summary>
        /// 自动构建数据库对象
        /// </summary>
        public Func<IDataBase> CreateDataBase { get; set; }

        /// <summary>
        ///     Sql构造器
        /// </summary>
        public ISqlBuilder<TEntity> SqlBuilder { get; set; }

        /// <summary>
        /// 操作注入器
        /// </summary>
        public IOperatorInjection<TEntity> Injection { get; set; }

    }
}