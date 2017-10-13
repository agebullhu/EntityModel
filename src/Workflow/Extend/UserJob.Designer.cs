/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/6/15 21:27:19*/
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Gboxt.Common.DataModel;

namespace Gboxt.Common.Workflow
{
	partial class UserJobData
	{



        /// <summary>
        /// 扩展校验
        /// </summary>
        /// <param name="result">结果存放处</param>
        partial void ValidateEx(ValidateResult result);

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="result">结果存放处</param>
        public override void Validate(ValidateResult result)
        {
            result.Id = Id; 
            base.Validate(result);
            if(string.IsNullOrWhiteSpace(Title))
                 result.AddNoEmpty("标题",nameof(Title));
            else 
            {
                if(Title.Length > 50)
                    result.Add("标题",nameof(Title),$"不能多于50个字");
            }
            if(Message != null)
            {
                if(Message.Length > 50)
                    result.Add("工作消息",nameof(Message),$"不能多于50个字");
            }
            if(ToUserName != null)
            {
                if(ToUserName.Length > 50)
                    result.Add("目标用户名字",nameof(ToUserName),$"不能多于50个字");
            }
            if(FromUserName != null)
            {
                if(FromUserName.Length > 50)
                    result.Add("来源用户名字",nameof(FromUserName),$"不能多于50个字");
            }
            ValidateEx(result);
        }
    }
}