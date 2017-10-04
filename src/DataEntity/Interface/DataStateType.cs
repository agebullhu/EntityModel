namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     数据状态
    /// </summary>
    public enum DataStateType
    {
        /// <summary>
        ///     草稿
        /// </summary>
        None = 0,

        /// <summary>
        ///     启用
        /// </summary>
        Enable = 1,

        /// <summary>
        ///     禁用
        /// </summary>
        Disable = 2,

        /// <summary>
        ///     锁定(不可用,仅表明状态,应使用)
        /// </summary>
        Lock = 0xF,

        /// <summary>
        ///     废弃
        /// </summary>
        Discard = 0x10,

        /// <summary>
        ///     删除
        /// </summary>
        Delete = 0xFF,

        /// <summary>
        ///     不正确的状态
        /// </summary>
        Error = 0x101
    }
}