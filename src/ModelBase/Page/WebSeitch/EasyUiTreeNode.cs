// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

#endregion

namespace Agebull.ProjectDeveloper.WebDomain.Models
{
    /// <summary>
    ///     EasyUi树数据的格式
    /// </summary>
    /// <remarks>仅调用ToJson时符合格式</remarks>
    [DataContract]
    public class EasyUiTreeNodeBase
    {
        #region 状态相关

        /// <summary>
        ///     节点的 id，它对于加载远程数据很重要。
        /// </summary>
        [DataMember, JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        ///     是否已选择
        /// </summary>
        [DataMember, JsonProperty("checked")]
        public bool IsChecked { get; set; }

        /// <summary>
        ///     是否已选择
        /// </summary>
        [DataMember, JsonProperty("selected")]
        public bool IsSelect { get; set; }

        /// <summary>
        ///     表格树关闭状态
        /// </summary>
        [IgnoreDataMember, JsonProperty("state")]
        public virtual string TreeState => IsOpen != null && IsOpen.Value ? "open" : "closed";

        /// <summary>
        ///     图标
        /// </summary>
        [DataMember, JsonProperty("iconCls")]
        public string Icon { get; set; }

        /// <summary>
        ///     是否展开
        /// </summary>
        [DataMember, JsonProperty("IsOpen")]
        public bool? IsOpen { get; set; }

        #endregion

        #region 内容相关

        /// <summary>
        ///     显示的节点文字。
        /// </summary>
        [DataMember, JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        ///     对象的原始标题
        /// </summary>
        [DataMember, JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        ///     给一个节点追加的自定义属性。
        /// </summary>
        [DataMember, JsonProperty("attributes", NullValueHandling = NullValueHandling.Ignore)]
        public string Attributes { get; set; }

        /// <summary>
        ///     标签文本
        /// </summary>
        [DataMember, JsonProperty("tag", NullValueHandling = NullValueHandling.Ignore)]
        public string Tag { get; set; }

        /// <summary>
        ///     备注文本
        /// </summary>
        [DataMember, JsonProperty("memo", NullValueHandling = NullValueHandling.Ignore)]
        public string Memo { get; set; }

        /// <summary>
        ///     还有其它的
        /// </summary>
        [DataMember, JsonProperty("json", NullValueHandling = NullValueHandling.Ignore)]
        public string Json { get; set; }

        /// <summary>
        ///     其它文本
        /// </summary>
        [DataMember, JsonProperty("extend", NullValueHandling = NullValueHandling.Ignore)]
        public string Extend { get; set; }

        #endregion

        public static EasyUiTreeNodeBase Empty = new EasyUiTreeNodeBase { ID = 0, Text = "-", Title = "-" };
    }

    /// <summary>
    ///     EasyUi树数据的格式
    /// </summary>
    /// <remarks>仅调用ToJson时符合格式</remarks>
    [DataContract]
    public class EasyUiTreeNode : EasyUiTreeNodeBase
    {
        /// <summary>
        ///     表格树的下级
        /// </summary>
        [IgnoreDataMember, JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public List<EasyUiTreeNode> Lists
        {
            get
            {
                return Children == null || Children.Count == 0
                    ? IsOpen == true
                        ? new List<EasyUiTreeNode>()
                        : null
                    : Children;
            }
        }

        /// <summary>
        ///     表格树的下级
        /// </summary>
        [DataMember, JsonIgnore]
        public List<EasyUiTreeNode> Children { get; set; }

        /// <summary>
        ///     表格树关闭状态
        /// </summary>
        [IgnoreDataMember, JsonProperty("state")]
        public override string TreeState
        {
            get
            {
                return IsOpen == null
                    ? (Children != null && Children.Count > 0 ? "open" : "closed")
                    : IsOpen.Value ? "open" : "closed";
            }
        }
        public static EasyUiTreeNode EmptyNode = new EasyUiTreeNode { ID = 0, Text = "-", Title = "-", IsOpen = true };
    }

    /// <summary>
    ///     EasyUi树数据的格式
    /// </summary>
    /// <remarks>仅调用ToJson时符合格式</remarks>
    public class EasyUiTreeNode<TData> : EasyUiTreeNodeBase
    {
        /// <summary>
        ///     数据
        /// </summary>
        [DataMember, JsonProperty("data")]
        public TData Data { get; set; }

        /// <summary>
        ///     表格树的下级
        /// </summary>
        [IgnoreDataMember, JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public List<EasyUiTreeNode<TData>> Lists
        {
            get { return Children == null || Children.Count == 0 ? null : Children; }
        }

        /// <summary>
        ///     表格树的下级
        /// </summary>
        [DataMember, JsonIgnore]
        public List<EasyUiTreeNode<TData>> Children { get; set; }

        /// <summary>
        ///     表格树关闭状态
        /// </summary>
        [IgnoreDataMember, JsonProperty("state")]
        public override string TreeState
        {
            get
            {
                return IsOpen == null
                    ? (Children != null && Children.Count > 0 ? "open" : "closed")
                    : IsOpen.Value ? "open" : "closed";
            }
        }
    }
}