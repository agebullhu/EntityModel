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
	partial class PositionPersonnelData
	{


        /// <summary>
        /// 数据校验
        /// </summary>
        /// <returns>校验消息,如果正确,返回空</returns>
        public override string Validate()
        {
            var msg =new List< string>();
            if(string.IsNullOrWhiteSpace(Appellative))
                msg.Add("[称呼]不能为空");
            if(CancelDate == DateTime.MinValue)
                msg.Add("[取消日期]不能为空");
            else 
            {
                if(CancelDate > new DateTime(2099,12,31) ||CancelDate < new DateTime(2010,1,1))
                    msg.Add("[取消日期]不能大于2099年12月31日或小于2010年1月1日");
            }
            if(AwardDate == DateTime.MinValue)
                msg.Add("[就职日期]不能为空");
            else 
            {
                if(AwardDate > new DateTime(2099,12,31) ||AwardDate < new DateTime(2010,1,1))
                    msg.Add("[就职日期]不能大于2099年12月31日或小于2010年1月1日");
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