using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Gboxt.Common.DataModel;

namespace Gboxt.Common.SystemModel
{
	partial class DataDictionaryData
	{
	    /// <summary>
	    ///     数据校验
	    /// </summary>
	    public override void Validate(ValidateResult result)
        {
            if(string.IsNullOrWhiteSpace(Name))
                result.AddNoEmpty("名称","Name");
        }
        
    }
}