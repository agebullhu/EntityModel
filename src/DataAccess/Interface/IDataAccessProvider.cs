using System;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 数据访问提供器
    /// </summary>
    public interface IDataAccessProvider<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 依赖对象
        /// </summary>
        IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 配置
        /// </summary>
        DataAccessOption Option { get; set; }

        /// <summary>
        /// 参数构造
        /// </summary>
        IParameterCreater ParameterCreater => SqlBuilder;

        /// <summary>
        /// 数据处理
        /// </summary>
        IDataOperator<TEntity> DataOperator { get; set; }

        /// <summary>
        /// 实体操作
        /// </summary>
        IEntityOperator<TEntity> EntityOperator { get; set; }

        /// <summary>
        /// 自动构建数据库对象
        /// </summary>
        Func<IDataBase> CreateDataBase { get; set; }

        /// <summary>
        ///     Sql构造器
        /// </summary>
        ISqlBuilder<TEntity> SqlBuilder { get; set; }

        /// <summary>
        /// 操作注入器
        /// </summary>
        IOperatorInjection<TEntity> Injection { get; set; }
    }
}