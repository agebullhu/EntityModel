// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:YhxBank.FundsManagement
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

using System.Data.SqlClient;

namespace Gboxt.Common.DataModel.SqlServer
{
    /// <summary>
    ///     数据更新插入器
    /// </summary>
    public interface IDataUpdateHandler<TData>
        where TData : EditDataObject, new()
    {
        /// <summary>
        /// 是否在访问器操作之后触发，如是为是则在访问器重载方法之后引发，否则在访问器重载方法之前引发
        /// </summary>
        bool AfterInner { get; }

        /// <summary>
        ///     保存前处理(Insert或Update)
        /// </summary>
        /// <param name="access">引发事件的数据访问对象</param>
        /// <param name="entity">保存的对象</param>
        /// <param name="subsist">当前实体生存状态</param>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        void PrepareSave(SqlServerTable<TData> access,TData entity, EntitySubsist subsist);

        /// <summary>
        ///     保存完成后期处理(Insert或Update)
        /// </summary>
        /// <param name="access">引发事件的数据访问对象</param>
        /// <param name="entity"></param>
        /// <param name="subsist"></param>
        void EndSaved(SqlServerTable<TData> access, TData entity, EntitySubsist subsist);

        /// <summary>
        ///     执行SQL语句前处理
        /// </summary>
        /// <param name="access">引发事件的数据访问对象</param>
        /// <param name="cmd">执行的命令对象</param>
        /// <param name="entity">保存的对象</param>
        /// <param name="subsist">当前实体生存状态</param>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        void PrepareExecSql(SqlServerTable<TData> access, SqlCommand cmd, TData entity, EntitySubsist subsist);
    }
}