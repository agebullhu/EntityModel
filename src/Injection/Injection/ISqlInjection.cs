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
        /// <param name="option">当前数据操作配置</param>
        /// <param name="conditions">附加的条件集合</param>
        /// <returns></returns>
        void InjectionQueryCondition<TEntity>(DataAccessOption option, List<string> conditions)
            where TEntity : class, new()
        { }

        /// <summary>
        ///     注入数据更新代码
        /// </summary>
        /// <param name="option">当前数据操作配置</param>
        /// <param name="condition"></param>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        void InjectionUpdateCode<TEntity>(DataAccessOption option, StringBuilder valueExpression, StringBuilder condition)
            where TEntity : class, new()
        { }

        /// <summary>
        ///     注入数据更新代码
        /// </summary>
        /// <param name="option">当前数据操作配置</param>
        /// <param name="condition"></param>
        /// <returns></returns>
        void InjectionDeleteCondition<TEntity>(DataAccessOption option, StringBuilder condition)
            where TEntity : class, new()
        { }

    }

}