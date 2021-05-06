/*****************************************************
(c)2016-2021 by ZeroTeam
作者: 胡天水
工程: Agebull.EntityModel.CoreAgebull.DataModel
建立: 忘了日期
修改: -
*****************************************************/

#region 引用


#endregion


namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     表明是一个数据操作对象
    /// </summary>
    public interface IDataAccessTool<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 驱动提供者信息
        /// </summary>
        IDataAccessProvider<TEntity> Provider { get; set; }

    }
}