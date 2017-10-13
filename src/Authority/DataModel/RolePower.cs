
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gboxt.Common.DataModel;
using Gboxt.Common.SystemModel;

namespace Agebull.SystemAuthority.Organizations
{
    /// <summary>
    /// 角色权限
    /// </summary>
    [DataContract]
    sealed partial class RolePowerData : EditDataObject, IRolePower
    {
    }
}