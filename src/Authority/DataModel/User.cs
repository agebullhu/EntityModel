
using System;
using System.Runtime.Serialization;
using Gboxt.Common.DataModel;

namespace Agebull.SystemAuthority.Organizations
{
    /// <summary>
    /// 系统用户
    /// </summary>
    [DataContract, Serializable]
    sealed partial class UserData : EditDataObject, IHistoryData, IStateData
    {
    }
}