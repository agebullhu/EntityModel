namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 编辑状态
    /// </summary>
    public interface IEditStatus
    {
        /// <summary>
        /// 修改状态记录器
        /// </summary>
        EntityEditStatus EditStatusRecorder { get; set; }

        /// <summary>
        /// 设置属性修改
        /// </summary>
        void OnSeted(string name) => EditStatusRecorder.SetModified(name);

        /// <summary>
        /// 重置为未修改
        /// </summary>
        void SetUnModify() => EditStatusRecorder.SetUnModify();

        /// <summary>
        /// 重置为未修改
        /// </summary>
        void SetUnModify(string name) => EditStatusRecorder.SetUnModify(name);
    }
}