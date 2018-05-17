using System.Collections.Generic;
using System.Data.Common;

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     数据更新触发器
    /// </summary>
    public interface IDataUpdateTrigger
    {
        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        void OnPrepareSave(EditDataObject entity, DataOperatorType operatorType);

        /// <summary>
        ///     保存完成后期处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        void OnDataSaved(EditDataObject entity, DataOperatorType operatorType);

        /// <summary>
        ///     更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="entityId">实体类型ID</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        void OnOperatorExecuting(int entityId, string condition, IEnumerable<DbParameter> args,
            DataOperatorType operatorType);

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="entityId">实体类型ID</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        void OnOperatorExecutd(int entityId, string condition, IEnumerable<DbParameter> args,
            DataOperatorType operatorType);
    }
}