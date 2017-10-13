
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
    /// 隶属关系表,包括职位与其它职位的隶属关系、机构之间的隶属关系、机构与职位的隶属关系
    /// </summary>
    [DataContract]
    sealed partial class SubjectionData : EditDataObject
    {
    }
}