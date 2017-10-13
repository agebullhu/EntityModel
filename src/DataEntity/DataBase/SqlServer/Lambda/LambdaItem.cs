// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:YhxBank.FundsManagement
// // 建立:2016-06-16
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace Gboxt.Common.DataModel.SqlServer
{
    /// <summary>
    ///     烂姆达条件节点
    /// </summary>
    /// <typeparam name="TData">数据类型</typeparam>
    public sealed class LambdaItem<TData>
    {
        /// <summary>
        ///     与关联的其它表达式
        /// </summary>
        public List<LambdaItem<TData>> Ands = new List<LambdaItem<TData>>();

        /// <summary>
        ///     或关联的其它表达式
        /// </summary>
        public List<LambdaItem<TData>> Ors = new List<LambdaItem<TData>>();

        /// <summary>
        ///     主条件
        /// </summary>
        public Expression<Func<TData, bool>> Expression { get; set; }

        /// <summary>
        ///     加入与关系的条件
        /// </summary>
        /// <param name="lambda"></param>
        public void AddAnd(Expression<Func<TData, bool>> lambda)
        {
            Ands.Add(new LambdaItem<TData>
            {
                Expression = lambda
            });
        }

        /// <summary>
        ///     加入或关系的条件
        /// </summary>
        /// <param name="lambda"></param>
        public void AddOr(Expression<Func<TData, bool>> lambda)
        {
            Ors.Add(new LambdaItem<TData>
            {
                Expression = lambda
            });
        }
    }
}