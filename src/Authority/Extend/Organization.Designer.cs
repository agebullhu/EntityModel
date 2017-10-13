using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace YhxBank.Common.Organization
{
	partial class OrganizationData
	{


        /// <summary>
        /// 数据校验
        /// </summary>
        /// <returns>校验消息,如果正确,返回空</returns>
        public override string Validate()
        {
            var msg =new List< string>();
            if(string.IsNullOrWhiteSpace(Code))
                msg.Add("[机构代码]不能为空");
            if(string.IsNullOrWhiteSpace(FullNane))
                msg.Add("[机构全称]不能为空");
            if(string.IsNullOrWhiteSpace(ShortName))
                msg.Add("[机构简称]不能为空");
            if(SetUpDate == DateTime.MinValue)
                msg.Add("[成立日期]不能为空");
            else 
            {
                if(SetUpDate > new DateTime(2099,12,31) ||SetUpDate < new DateTime(2010,1,1))
                    msg.Add("[成立日期]不能大于2099年12月31日或小于2010年1月1日");
            }
            if(string.IsNullOrWhiteSpace(TreeName))
                msg.Add("[组织树上的名称]不能为空");
            ValidateEx(msg);
            var bm = base.Validate();
            if(bm != null)
                msg.Add(bm);
            return msg.Count ==0 ? null : string.Join(",", msg);
        }
        partial void ValidateEx(List<string> msg);
        
    }
}