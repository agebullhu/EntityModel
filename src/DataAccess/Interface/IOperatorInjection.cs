// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 表示操作注入器
    /// </summary>
    public interface IOperatorInjection<TEntity> : IOperatorExtension<TEntity>
        where TEntity : class, new()
    {

        /// <summary>
        ///     注入查询条件
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        void InjectionQueryCondition(List<string> conditions) { }

        /// <summary>
        ///     注入数据更新代码
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        void InjectionUpdateCode(ref string valueExpression, ref string condition) { }

        /// <summary>
        ///     注入数据插入代码
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        void InjectionInsertCode(StringBuilder fields, StringBuilder values) { }

        /// <summary>
        ///     删除条件注入
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        string InjectionDeleteCondition(string condition) => condition;

    }
}