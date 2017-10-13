/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/6/8 17:32:24*/
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
	partial class AreaData
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
            if(Code != null)
            {
                if(Code.Length > 50)
                    result.Add("编码",nameof(Code),$"不能多于50个字");
            }
            if(FullName != null)
            {
                if(FullName.Length > 200)
                    result.Add("全称",nameof(FullName),$"不能多于200个字");
            }
            if(string.IsNullOrWhiteSpace(ShortName))
                 result.AddNoEmpty("简称",nameof(ShortName));
            else 
            {
                if(ShortName.Length > 50)
                    result.Add("简称",nameof(ShortName),$"不能多于50个字");
            }
            if(TreeName != null)
            {
                if(TreeName.Length > 50)
                    result.Add("树形名称",nameof(TreeName),$"不能多于50个字");
            }
            ValidateEx(result);
        }
    }
}