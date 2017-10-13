
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gboxt.Common.DataModel;

namespace Agebull.SystemAuthority.Organizations
{
    /// <summary>
    /// 系统角色
    /// </summary>
    [DataContract]
    sealed partial class RoleData : EditDataObject, IRoleData
    {
    }

    public interface IRoleData
    {
        /// <summary>
        /// 角色标识
        /// </summary>
        /// <remarks>
        /// 角色标识
        /// </remarks>
        int Id { get; set; }

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