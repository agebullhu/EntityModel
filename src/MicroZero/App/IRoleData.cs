// /*****************************************************
// (c)2008-2013 Copy right www.Agebull.com
// 作者:bull2
// 工程:CodeRefactor-Agebull.Common.WpfMvvmBase
// 建立:2014-11-29
// 修改:2014-11-29
// *****************************************************/

namespace Agebull.Common.OAuth
{
    /// <summary>
    /// 表示一个角色数据
    /// </summary>
    public interface IRoleData
    {
        /// <summary>
        /// 角色标识
        /// </summary>
        /// <remarks>
        /// 角色标识
        /// </remarks>
        long Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        /// <remarks>
        /// 角色名称
        /// </remarks>
        string Role { get; set; }

        /// <summary>
        /// 角色显示的文本
        /// </summary>
        /// <remarks>
        /// 角色显示的文本
        /// </remarks>
        string Caption { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <remarks>
        /// 备注
        /// </remarks>
        string Memo { get; set; }
    }
}