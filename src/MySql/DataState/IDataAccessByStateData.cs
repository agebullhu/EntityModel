// // /*****************************************************
// // 作者:agebull
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2018.10.07
// // *****************************************************/

#region 引用

using System;
using System.Linq.Expressions;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 数据状态表
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IDataAccessByStateData<TEntity> : IDataAccess<TEntity>
        where TEntity : EditDataObject, new()
    {
        /// <summary>
        /// 重置状态
        /// </summary>
        bool ResetState<TPrimaryKey>(TPrimaryKey id);

        /// <summary>
        /// 重置状态
        /// </summary>
        bool ResetState(Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        /// 修改状态
        /// </summary>
        bool SetState<TPrimaryKey>(DataStateType state, bool isFreeze, TPrimaryKey id);
    }
}