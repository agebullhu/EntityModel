// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
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
    public interface IOperatorInjection<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 依赖对象
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 配置对象
        /// </summary>
        DataAccessOption<TEntity> Option { get; set; }

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        void InjectionCondition(List<string> conditions);

        /// <summary>
        ///     得到可正确更新的条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        void CheckUpdateContition(ref string condition);

        /// <summary>
        ///     与更新同时执行的SQL(更新之前立即执行)
        /// </summary>
        /// <param name="condition">当前场景的执行条件</param>
        /// <returns></returns>
        string BeforeUpdateSql(string condition);

        /// <summary>
        ///     与更新同时执行的SQL(更新之后立即执行)
        /// </summary>
        /// <param name="condition">当前场景的执行条件</param>
        /// <returns></returns>
        string AfterUpdateSql(string condition);

        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        void PrepareSave(TEntity entity, DataOperatorType operatorType);

        /// <summary>
        ///     保存完成后期处理(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="operatorType">操作类型</param>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        void EndSaved(TEntity entity, DataOperatorType operatorType);

        /// <summary>
        ///     更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="parameter">参数值</param>
        /// <param name="operatorType">操作类型</param>
        void OnOperatorExecuting(string condition, DbParameter[] parameter, DataOperatorType operatorType);

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="parameter">参数值</param>
        /// <param name="operatorType">操作类型</param>
        void OnOperatorExecuted(string condition, DbParameter[] parameter, DataOperatorType operatorType);

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="operatorType">操作类型</param>
        /// <param name="key">其它参数</param>
        Task OnKeyEvent(DataOperatorType operatorType, object key);

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        Task OnMulitUpdateEvent(DataOperatorType operatorType, string condition, DbParameter[] parameter);

        /// <summary>
        ///     保存完成后期处理(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="operatorType">操作类型</param>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        Task OnEndSavedEvent(TEntity entity, DataOperatorType operatorType);
    }

}