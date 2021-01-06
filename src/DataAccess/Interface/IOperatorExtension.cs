// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用


#endregion

using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     表明是一个数据操作对象
    /// </summary>
    public interface IOperatorExtension<TEntity> : IDataAccessTool<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        ///     载入后
        /// </summary>
        /// <param name="entity">实体</param>
        Task AfterLoad(TEntity entity) => Task.CompletedTask;

        /// <summary>
        ///     实体保存前处理(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        Task BeforeSave(TEntity entity, DataOperatorType operatorType) => Task.CompletedTask;

        /// <summary>
        ///     实体保存完成后期处理(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="operatorType">操作类型</param>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        Task AfterSave(TEntity entity, DataOperatorType operatorType) => Task.CompletedTask;

        /// <summary>
        ///     更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="parameter">参数值</param>
        /// <param name="operatorType">操作类型</param>
        Task BeforeExecute(DataOperatorType operatorType, string condition, IEnumerable<DbParameter> parameter) => Task.CompletedTask;

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="condition">执行条件</param>
        /// <param name="parameter">参数值</param>
        /// <param name="operatorType">操作类型</param>
        Task AfterExecute(DataOperatorType operatorType, string sql, string condition, IEnumerable<DbParameter> parameter) => Task.CompletedTask;


        /// <summary>
        /// 业务操作命令事件
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="entity">实体</param>
        /// <param name="object">实体</param>
        /// <param name="id">主键</param>
        /// <param name="cmd">命令</param>
        Task AfterCommand(TEntity entity, string id, DataOperatorType cmd) => Task.CompletedTask;

    }
}