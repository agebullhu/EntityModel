
namespace Gboxt.Common.DataModel
{
    /// <summary>
    /// 表示为行级权限表
    /// </summary>
    public interface IRowScopeData : IHistoryData, IIdentityData, IStateData
    {
        /// <summary>
        /// 部门级别
        /// </summary>
        int DepartmentLevel { get; set; }

        /// <summary>
        /// 部门所有者
        /// </summary>
        int DepartmentId { get; set; }

        /*// <summary>
        /// 可视范围
        /// </summary>
        ViewScopeType ViewScope { get; }*/
    }

    /// <summary>
    /// 表示主办方关联数据(区别于行级权限的关联)
    /// </summary>
    public interface ISponsor
    {
        /// <summary>
        /// 主办方
        /// </summary>
        int SponsorId { get; }
    }

    /// <summary>
    /// 可视范围枚举类型
    /// </summary>
    /// <remark>
    /// 可视范围
    /// </remark>
    public enum ViewScopeType
    {
        /// <summary>
        /// 仅作者
        /// </summary>
        None = 0x0,
        /// <summary>
        /// 所有机构
        /// </summary>
        Protected = 0x1,
        /// <summary>
        /// 所有系统用户
        /// </summary>
        Intenal = 0x2,
        /// <summary>
        /// 所有人
        /// </summary>
        Public = 0x3,
    }

    public static class EnumViewScopeHelper
    {

        /// <summary>
        ///     可视范围枚举类型名称转换
        /// </summary>
        public static string ToCaption(this ViewScopeType value)
        {
            switch (value)
            {
                case ViewScopeType.None:
                    return "仅作者";
                case ViewScopeType.Protected:
                    return "所有机构";
                case ViewScopeType.Intenal:
                    return "所有系统用户";
                case ViewScopeType.Public:
                    return "所有人";
                default:
                    return "可视范围枚举类型(未知)";
            }
        }
    }
}