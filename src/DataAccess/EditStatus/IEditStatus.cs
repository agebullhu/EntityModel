namespace Agebull.EntityModel.Common
{
    public interface IEditStatus
    {
        /// <summary>
        /// 修改状态
        /// </summary>
        EntityEditStatus EditStatusRedorder { get; set; }

        /// <summary>
        /// 设置属性修改
        /// </summary>
        void OnSeted(string name) => EditStatusRedorder.SetModified(name);

        /// <summary>
        /// 重置为未修改
        /// </summary>
        void SetUnModify() => EditStatusRedorder.SetUnModify();

        /// <summary>
        /// 重置为未修改
        /// </summary>
        void SetUnModify(string name) => EditStatusRedorder.SetUnModify(name);
    }
}