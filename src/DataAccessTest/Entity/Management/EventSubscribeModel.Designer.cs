/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/3 11:04:54*/
#region
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;

using Agebull.EntityModel.Common;
using Agebull.EntityModel.Interfaces;


#endregion

namespace Zeroteam.MessageMVC.EventBus
{
    /// <summary>
    /// 事件订阅
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class EventSubscribeModel 
    {
        #region 构造
        
        /// <summary>
        /// 构造
        /// </summary>
        public EventSubscribeModel()
        {
            Initialize();
            InitEntityEditStatus();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        partial void Initialize();
        partial void InitEntityEditStatus();

        #endregion

        #region 基本属性


        
        /// <summary>
        ///  所属服务
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [JsonProperty("service",  NullValueHandling = NullValueHandling.Ignore)]
        public  long Service
        {
            get;
            set;
        }

        
        /// <summary>
        ///  事件标识
        /// </summary>
        /// <example>
        ///     0
        /// </example>
        [JsonProperty("eventId",  NullValueHandling = NullValueHandling.Ignore)]
        public  long EventId
        {
            get;
            set;
        }

        
        /// <summary>
        ///  是否查阅服务
        /// </summary>
        /// <remarks>
        ///     如为查阅服务，则发送后不处理与等待结果
        /// </remarks>
        /// <example>
        ///     true
        /// </example>
        [JsonProperty("isLookUp",  NullValueHandling = NullValueHandling.Ignore)]
        public  bool IsLookUp
        {
            get;
            set;
        }
        #endregion



    }
}