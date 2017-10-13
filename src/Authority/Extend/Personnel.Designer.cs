using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Gboxt.Common.DataModel;

namespace Agebull.SystemAuthority.Organizations
{
	partial class PersonnelData
	{
	    /// <summary>
	    ///     数据校验
	    /// </summary>
	    public override void Validate(ValidateResult result)
	    {
            if(string.IsNullOrWhiteSpace(FullName))
                result.AddNoEmpty("姓名",nameof(FullName));
            if(Birthday != DateTime.MinValue && (Birthday > DateTime.Today.AddYears(-18) || Birthday < DateTime.Today.AddYears(-80)))
	            result.Add("生日", nameof(Birthday),"此人满十八周岁或已是八十老翁~_~");
            if(string.IsNullOrWhiteSpace(Mobile))
               result.Add("手机", nameof(Mobile),"手机号不能为空,因为此员工要登录系统时,手机为登录名");
        }
        partial void ValidateEx(List<string> msg);
        
    }
}