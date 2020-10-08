/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/7 0:54:45*/
#region
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json;

using Agebull.Common;
using Agebull.Common.Ioc;

using Agebull.EntityModel.Common;
using ZeroTeam.MessageMVC.ModelApi;
using ZeroTeam.MessageMVC.ZeroApis;



using Zeroteam.MessageMVC.EventBus;

#endregion

namespace Zeroteam.MessageMVC.EventBus.WebApi
{
    /// <summary>
    ///  事件定义
    /// </summary>
    [Service("eventManagement")]
    [Route("eventDefault/v1")]
    [ApiPage("/wwwroot/Management/EventDefault/index.htm")]
    public sealed partial class EventDefaultApiController 
         : ApiControllerForDataState<EventDefaultEntity,long,EventDefaultEntityBusinessLogic>
    {
        #region 基本扩展

        /// <summary>
        /// 读取Form传过来的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="convert">转化器</param>
        protected override void ReadFormData(EventDefaultEntity data, FormConvert convert)
        {
            DefaultReadFormData(data,convert);
        }

        #endregion
    }
}