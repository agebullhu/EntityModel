// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     编辑对象
    /// </summary>
    public interface IEditObject : IDataObject
    {
        /// <summary>
        ///     接受修改
        /// </summary>
        void AcceptChanged();

        /// <summary>
        /// 设置为未修改
        /// </summary>
        void RejectChanged();

        /// <summary>
        ///     是否修改
        /// </summary>
        bool IsModified { get; }

        /// <summary>
        ///     是否已删除
        /// </summary>
        bool IsDelete { get; }

        /// <summary>
        ///     是否新增
        /// </summary>
        bool IsNew { get; }

        /// <summary>
        ///     是否修改
        /// </summary>
        /// <param name="propertyIndex"> 字段的序号 </param>
        bool FieldIsModified(int propertyIndex);

        /// <summary>
        ///     设置为非改变
        /// </summary>
        /// <param name="propertyIndex"> 字段的序号 </param>
        void SetUnModify(int propertyIndex);

        /// <summary>
        ///     设置为改变
        /// </summary>
        /// <param name="propertyIndex"> 字段的序号 </param>
        void SetModify(int propertyIndex);

        /// <summary>
        ///     属性修改的后期处理(保存后)
        /// </summary>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        void LaterPeriodByModify();
    }
}