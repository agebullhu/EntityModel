/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/6/11 12:57:03*/
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
    /// 职位字典
    /// </summary>
    [DataContract]
    sealed partial class PositionData : EditDataObject
    {
        
        /// <summary>
        /// 初始化
        /// </summary>
        partial void Initialize()
        {

            _datastate = 0;
            _isfreeze = true;
            _authorid = 0;
            _lastreviserid = 0;
            _auditstate = 0;
            _auditorid = 0;
        }

    }
}