// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/


namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     表明是一个数据操作对象
    /// </summary>
    public class DataOperator<TEntity> : IDataOperator<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 驱动提供者信息
        /// </summary>
        public DataAccessProvider<TEntity> Provider { get; set; }
    }
}