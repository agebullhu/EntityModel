// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

using Agebull.EntityModel.Common;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Agebull.EntityModel.Events
{
    /// <summary>
    ///     数据更新处理器
    /// </summary>
    public interface IDataUpdateHandler
    {
        #region 对象类型检查

        /// <summary>
        /// 初始化类型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        void InitType<TEntity>() where TEntity : class, new();

        /// <summary>
        /// 是否指定类型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsType<TEntity>(int type);

        #endregion

        #region 数据事件

        /// <summary>
        /// 状态修改事件
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="entity">实体</param>
        /// <param name="oType">操作</param>
        /// <param name="valueType">值类型</param>
        /// <param name="value">内容</param>
        /// <remarks>
        /// 如果内容为实体,使用JSON序列化,
        /// 如果使用主键内容为#:[key](如:#:123)样式,
        /// 如果为批量操作,内容为对象，请进行JSON序列化后传播
        /// </remarks>
        Task OnStatusChanged(string database, string entity,
            DataOperatorType oType,
            EntityEventValueType valueType,
            object value);

        #endregion

        #region 扩展流程

        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="data">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        void OnPrepareSave<TEntity>(TEntity data, DataOperatorType operatorType)
             where TEntity : class, new();

        /// <summary>
        ///     保存完成后期处理
        /// </summary>
        /// <param name="data">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        void OnDataSaved<TEntity>(TEntity data, DataOperatorType operatorType)
             where TEntity : class, new();


        /// <summary>
        ///     更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="option">当前数据操作配置</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        void OnOperatorExecuting<TEntity>(DataAccessOption<TEntity> option, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
                      where TEntity : class, new();

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="option">当前数据操作配置</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        void OnOperatorExecuted<TEntity>(DataAccessOption<TEntity> option, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
              where TEntity : class, new();

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="option">当前数据操作配置</param>
        /// <param name="conditions">附加的条件集合</param>
        /// <returns></returns>
        void InjectionCondition<TEntity>(DataAccessOption<TEntity> option, List<string> conditions)
            where TEntity : class, new();

        /// <summary>
        ///     与更新同时执行的SQL(更新之前立即执行)
        /// </summary>
        /// <param name="option">当前数据操作配置</param>
        /// <param name="code">写入SQL的文本构造器</param>
        /// <param name="condition">当前场景的执行条件</param>
        /// <returns></returns>
        void BeforeUpdateSql<TEntity>(DataAccessOption<TEntity> option, StringBuilder code, string condition)
             where TEntity : class, new();

        /// <summary>
        ///     与更新同时执行的SQL(更新之后立即执行)
        /// </summary>
        /// <param name="option">当前数据操作配置</param>
        /// <param name="condition">当前场景的执行条件</param>
        /// <param name="code">写入SQL的文本构造器</param>
        /// <returns></returns>
        void AfterUpdateSql<TEntity>(DataAccessOption<TEntity> option, StringBuilder code, string condition)
             where TEntity : class, new();

        #endregion
    }
}