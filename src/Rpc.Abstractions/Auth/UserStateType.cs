namespace Agebull.Common.OAuth
{
    /// <summary>
    ///     数据状态
    /// </summary>
    public enum UserStateType
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
        ///     废弃
        /// </summary>
        Discard = 0x10,

        /// <summary>
        ///     删除
        /// </summary>
        Delete = 0xFF
    }
}