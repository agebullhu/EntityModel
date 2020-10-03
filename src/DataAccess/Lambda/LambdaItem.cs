// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-16
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     烂姆达条件节点
    /// </summary>
    /// <typeparam name="TEntity">数据类型</typeparam>
    public sealed class LambdaItem<TEntity>
    {
        /// <summary>
        ///     与关联的其它表达式
        /// </summary>
        public List<LambdaItem<TEntity>> Ands = new List<LambdaItem<TEntity>>();

        /// <summary>
        ///     或关联的其它表达式
        /// </summary>
        public List<LambdaItem<TEntity>> Ors = new List<LambdaItem<TEntity>>();

        /// <summary>
        ///     主条件
        /// </summary>
        public Expression<Func<TEntity, bool>> Root { get; set; }


        /// <summary>
        ///     根条件
        /// </summary>
        public List<LambdaItem<TEntity>> Roots = new List<LambdaItem<TEntity>>();

        /// <summary>
        ///     加入与关系的条件
        /// </summary>
        /// <param name="lambda"></param>
        public void AddRoot(Expression<Func<TEntity, bool>> lambda)
        {
            Roots.Add(new LambdaItem<TEntity>
            {
                Root = lambda
            });
        }

        /// <summary>
        ///     加入与关系的条件
        /// </summary>
        /// <param name="lambda"></param>
        public void AddAnd(Expression<Func<TEntity, bool>> lambda)
        {
            Ands.Add(new LambdaItem<TEntity>
            {
                Root = lambda
            });
        }

        /// <summary>
        ///     加入或关系的条件
        /// </summary>
        /// <param name="lambda"></param>
        public void AddOr(Expression<Func<TEntity, bool>> lambda)
        {
            Ors.Add(new LambdaItem<TEntity>
            {
                Root = lambda
            });
        }
    }
}