/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/5/26 18:30:59*/
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Agebull.Project.Cbs.Organizations
{
	partial class PersonnelPositionData
	{


        /// <summary>
        /// 数据校验
        /// </summary>
        /// <returns>校验消息,如果正确,返回空</returns>
        public override string Validate()
        {
            var msg =new List< string>();
            if(string.IsNullOrWhiteSpace(Appellation))
                msg.Add("[称谓]不能为空");
            else 
            {
                if(Appellation.Length > 50)
                    msg.Add("[称谓]不能多于50个字");
            }
            ValidateEx(msg);
            var bm = base.Validate();
            if(bm != null)
                msg.Add(bm);
            return msg.Count ==0 ? null : string.Join(",", msg);
        }
        partial void ValidateEx(List<string> msg);
        
    }
}