﻿namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     数据状态
    /// </summary>
    public enum DataStateType
    {
        /// <summary>
        ///     草稿
        /// </summary>
        /// <remarks>
        ///  当使用启用禁用状态逻辑时,此数据不应使用。非严格定义时,作为启用状态数据使用
        /// </remarks>
        None = 0,

        /// <summary>
        ///     启用
        /// </summary>
        /// <remarks>
        ///  此数据任何时间均可使用
        /// </remarks>
        Enable = 1,

        /// <summary>
        ///     禁用
        /// </summary>
        /// <remarks>
        ///  此数据任何时间均不应使用
        /// </remarks>
        Disable = 2,

        /// <summary>
        ///     第三方锁定
        /// </summary>
        /// <remarks>
        ///  用于框架内部,调用者可忽略
        /// </remarks>
        Orther = 0xE,

        /// <summary>
        ///     锁定
        /// </summary>
        /// <remarks>
        ///  用于框架内部,调用者可忽略
        /// </remarks>
        Lock = 0xF,

        /// <summary>
        ///     废弃
        /// </summary>
        /// <remarks>
        ///  数据实际用途与删除状态相同,但用户可见
        /// </remarks>
        Discard = 0x10,

        /// <summary>
        ///     删除
        /// </summary>
        /// <remarks>
        ///  数据实际处于删除状态,用户不可见
        /// </remarks>
        Delete = 0xFF,

        /// <summary>
        ///     错误状态
        /// </summary>
        /// <remarks>
        ///  用于框架内部,调用者可忽略
        /// </remarks>
        State = 0x101
    }

    /// <summary>
    ///     数据状态扩展类
    /// </summary>
    public static class DataStateHelper
    {
        /// <summary>
        ///     是否为删除数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>是否为删除数据</returns>
        public static bool IsDeleted(this IStateData data)
        {
            return data.DataState == DataStateType.Delete;
        }

        /// <summary>
        ///     数据状态枚举类型
        /// </summary>
        public static string ToCaption(this DataStateType value)
        {
            switch (value)
            {
                case DataStateType.None:
                    return "草稿";
                case DataStateType.Discard:
                    return "废弃";
                case DataStateType.State:
                    return "不正确的状态";
                case DataStateType.Orther:
                    return "其它人编辑中";
                case DataStateType.Lock:
                    return "锁定";
                case DataStateType.Delete:
                    return "删除";
                case DataStateType.Enable:
                    return "启用";
                case DataStateType.Disable:
                    return "禁用";
                default:
                    return "未知数据状态";
            }
        }
    }
}