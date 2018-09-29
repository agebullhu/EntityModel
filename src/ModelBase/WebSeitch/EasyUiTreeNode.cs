// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System.Collections.Generic;
using Newtonsoft.Json;

#endregion

namespace Agebull.Common.DataModel.WebUI
{
    /// <summary>
    ///     EasyUi树数据的格式
    /// </summary>
    /// <remarks>仅调用ToJson时符合格式</remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class EasyUiTreeNodeBase 
    {
        #region 状态相关

        /// <summary>
        ///     节点的 id，它对于加载远程数据很重要。
        /// </summary>
        [ JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        ///     是否已选择
        /// </summary>
        [ JsonProperty("checked")]
        public bool IsChecked { get; set; }

        /// <summary>
        ///     是否已选择
        /// </summary>
        [ JsonProperty("selected")]
        public bool IsSelect { get; set; }

        /// <summary>
        ///     表格树关闭状态
        /// </summary>
        [ JsonProperty("state")]
        public virtual string TreeState => IsOpen != null && IsOpen.Value ? "open" : "closed";

        /// <summary>
        ///     图标
        /// </summary>
        [ JsonProperty("iconCls")]
        public string Icon { get; set; }

        /// <summary>
        ///     是否展开
        /// </summary>
        [ JsonProperty("IsOpen")]
        public bool? IsOpen { get; set; }

        #endregion

        #region 内容相关

        /// <summary>
        /// 是否文件夹
        /// </summary>
        [ JsonProperty("IsFolder")]
        public bool IsFolder
        {
            get;
            set;
        }

        /// <summary>
        ///     显示的节点文字。
        /// </summary>
        [ JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        ///     对象的原始标题
        /// </summary>
        [ JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        ///     给一个节点追加的自定义属性。
        /// </summary>
        [ JsonProperty("attributes", NullValueHandling = NullValueHandling.Ignore)]
        public string Attributes { get; set; }

        /// <summary>
        ///     标签文本
        /// </summary>
        [ JsonProperty("tag", NullValueHandling = NullValueHandling.Ignore)]
        public string Tag { get; set; }

        /// <summary>
        ///     备注文本
        /// </summary>
        [ JsonProperty("memo", NullValueHandling = NullValueHandling.Ignore)]
        public string Memo { get; set; }

        /// <summary>
        ///     还有其它的
        /// </summary>
        [ JsonProperty("json", NullValueHandling = NullValueHandling.Ignore)]
        public string Json { get; set; }

        /// <summary>
        ///     其它文本
        /// </summary>
        [ JsonProperty("extend", NullValueHandling = NullValueHandling.Ignore)]
        public string Extend { get; set; }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Text}(IsOpen:{IsOpen},IsFolder:{IsFolder})";
        }
        /// <summary>
        /// 空对象
        /// </summary>
        public static EasyUiTreeNodeBase Empty = new EasyUiTreeNodeBase { ID = 0, Text = "-", Title = "-" };
    }

    /// <summary>
    ///     EasyUi树数据的格式
    /// </summary>
    /// <remarks>仅调用ToJson时符合格式</remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class EasyUiTreeNode : EasyUiTreeNodeBase
    {
        /// <summary>
        /// 构造
        /// </summary>
        public EasyUiTreeNode()
        {

        }
        /// <summary>
        /// 构造
        /// </summary>
        public EasyUiTreeNode(List<EasyUiTreeNode> children)
        {
            IsFolder = children != null && children.Count > 0;
            _children = children;
        }
        /// <summary>
        /// 有否子级
        /// </summary>
        public bool HaseChildren => IsFolder && _children != null && _children.Count > 0;


        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        private List<EasyUiTreeNode> _children;

        /// <summary>
        ///     表格树的下级
        /// </summary>
        [JsonIgnore]
        public List<EasyUiTreeNode> Children => !IsFolder
            ? null
            : (_children ?? (_children = new List<EasyUiTreeNode>()));

        /// <summary>
        ///     表格树关闭状态
        /// </summary>
        [ JsonProperty("state")]
        public override string TreeState => !IsFolder
            ? "open"
            : (IsOpen == null
                ? (Children.Count > 0 ? "open" : "closed")
                : IsOpen.Value ? "open" : "closed");

        /// <summary>
        /// 空内容
        /// </summary>
        public static EasyUiTreeNode EmptyNode = new EasyUiTreeNode { ID = 0, Text = "-", Title = "-", IsOpen = true };

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Text}(IsOpen:{IsOpen},IsFolder:{IsFolder},HaseChildren:{HaseChildren})";
        }
    }

    /// <summary>
    ///     EasyUi树数据的格式
    /// </summary>
    /// <remarks>仅调用ToJson时符合格式</remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class EasyUiTreeNode<TData> : EasyUiTreeNodeBase
    {
        /// <summary>
        /// 有否子级
        /// </summary>
        public bool HaseChildren => IsFolder && _children != null && _children.Count > 0;

        /// <summary>
        ///     数据
        /// </summary>
        [ JsonProperty("data")]
        public TData Data { get; set; }
        
        [ JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        private List<EasyUiTreeNode<TData>> _children;

        /// <summary>
        ///     表格树的下级
        /// </summary>
        [ JsonIgnore]
        public List<EasyUiTreeNode<TData>> Children => !IsFolder
            ? null
            : (_children ?? (_children = new List<EasyUiTreeNode<TData>>()));

        /// <summary>
        ///     表格树关闭状态
        /// </summary>
        [ JsonProperty("state")]
        public override string TreeState => !IsFolder
            ? "open"
            : IsOpen == null
                ? (Children.Count > 0 ? "open" : "closed")
                : IsOpen.Value ? "open" : "closed";

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Text}(IsOpen:{IsOpen},IsFolder:{IsFolder},HaseChildren:{HaseChildren})";
        }
    }
}