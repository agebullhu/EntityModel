using System.Collections.Generic;
using System.Text;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     Sql注入器
    /// </summary>
    public interface ISqlInjection
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        DataBaseType DataBaseType
        {
            get;
        }

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="provider">当前数据操作适配器</param>
        /// <param name="conditions">附加的条件集合</param>
        /// <returns></returns>
        void InjectionQueryCondition<TEntity>(IDataAccessProvider<TEntity> provider, List<string> conditions)
            where TEntity : class, new()
        { }

        /// <summary>
        ///     注入数据插入代码
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        void InjectionInsertCode<TEntity>(IDataAccessProvider<TEntity> provider, StringBuilder fields, StringBuilder values) where TEntity : class, new()
        { }

        /// <summary>
        ///     注入数据更新代码
        /// </summary>
        /// <param name="provider">当前数据操作适配器</param>
        /// <param name="condition"></param>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        void InjectionUpdateCode<TEntity>(IDataAccessProvider<TEntity> provider, StringBuilder valueExpression, List<string> condition)
            where TEntity : class, new()
        { }

        /// <summary>
        ///     注入数据更新代码
        /// </summary>
        /// <param name="provider">当前数据操作适配器</param>
        /// <param name="condition"></param>
        /// <returns></returns>
        void InjectionDeleteCondition<TEntity>(IDataAccessProvider<TEntity> provider, List<string> condition)
            where TEntity : class, new()
        { }

    }

}