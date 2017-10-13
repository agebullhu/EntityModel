/*design by:agebull designer date:2017/6/9 11:34:09*/

using System.Runtime.Serialization;
using Gboxt.Common.DataModel;
using Newtonsoft.Json;

namespace Gboxt.Common.Workflow
{
    /// <summary>
    /// 用户工作列表
    /// </summary>
    [DataContract]
    sealed partial class UserJobData : EditDataObject
    {

        /// <summary>
        /// 实际数据的编辑页面的脚本地址
        /// </summary>
        [IgnoreDataMember, JsonProperty("script_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ScriptUrl { get; set; }
        /// <summary>
        /// 实际数据的编辑页面的地址
        /// </summary>
        [IgnoreDataMember, JsonProperty("form_url", NullValueHandling = NullValueHandling.Ignore)]
        public string FormUrl { get; set; }
        /// <summary>
        /// 实际数据的阅读页面的地址
        /// </summary>
        [IgnoreDataMember, JsonProperty("read_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ReadUrl { get; set; }

        /// <summary>
        /// 实际数据的动作页面的地址
        /// </summary>
        [IgnoreDataMember, JsonProperty("action_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ActionUrl { get; set; }

        /// <summary>
        /// 实际数据的ID,用在页面操作中
        /// </summary>
        [IgnoreDataMember, JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int MyId => LinkId;

        /// <summary>
        /// 实际数据的动作(edit,audit),用在页面操作中
        /// </summary>
        [IgnoreDataMember, JsonProperty("job", NullValueHandling = NullValueHandling.Ignore)]
        public string UserJob { get; set; }
    }
}