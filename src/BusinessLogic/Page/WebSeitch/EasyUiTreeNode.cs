// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

#endregion

namespace Agebull.ProjectDeveloper.WebDomain.Models
{
    /// <summary>
    ///     EasyUi�����ݵĸ�ʽ
    /// </summary>
    /// <remarks>������ToJsonʱ���ϸ�ʽ</remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class EasyUiTreeNodeBase
    {
        #region ״̬���

        /// <summary>
        ///     �ڵ�� id�������ڼ���Զ�����ݺ���Ҫ��
        /// </summary>
        [ JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        ///     �Ƿ���ѡ��
        /// </summary>
        [ JsonProperty("checked")]
        public bool IsChecked { get; set; }

        /// <summary>
        ///     �Ƿ���ѡ��
        /// </summary>
        [ JsonProperty("selected")]
        public bool IsSelect { get; set; }

        /// <summary>
        ///     ������ر�״̬
        /// </summary>
        [ JsonProperty("state")]
        public virtual string TreeState => IsOpen != null && IsOpen.Value ? "open" : "closed";

        /// <summary>
        ///     ͼ��
        /// </summary>
        [ JsonProperty("iconCls")]
        public string Icon { get; set; }

        /// <summary>
        ///     �Ƿ�չ��
        /// </summary>
        [ JsonProperty("IsOpen")]
        public bool? IsOpen { get; set; }

        #endregion

        #region �������

        /// <summary>
        /// �Ƿ��ļ���
        /// </summary>
        [ JsonProperty("IsFolder")]
        public bool IsFolder
        {
            get;
            set;
        }

        /// <summary>
        ///     ��ʾ�Ľڵ����֡�
        /// </summary>
        [ JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        ///     �����ԭʼ����
        /// </summary>
        [ JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        ///     ��һ���ڵ�׷�ӵ��Զ������ԡ�
        /// </summary>
        [ JsonProperty("attributes", NullValueHandling = NullValueHandling.Ignore)]
        public string Attributes { get; set; }

        /// <summary>
        ///     ��ǩ�ı�
        /// </summary>
        [ JsonProperty("tag", NullValueHandling = NullValueHandling.Ignore)]
        public string Tag { get; set; }

        /// <summary>
        ///     ��ע�ı�
        /// </summary>
        [ JsonProperty("memo", NullValueHandling = NullValueHandling.Ignore)]
        public string Memo { get; set; }

        /// <summary>
        ///     ����������
        /// </summary>
        [ JsonProperty("json", NullValueHandling = NullValueHandling.Ignore)]
        public string Json { get; set; }

        /// <summary>
        ///     �����ı�
        /// </summary>
        [ JsonProperty("extend", NullValueHandling = NullValueHandling.Ignore)]
        public string Extend { get; set; }

        #endregion

        public override string ToString()
        {
            return $"{Text}(IsOpen:{IsOpen},IsFolder:{IsFolder})";
        }

        public static EasyUiTreeNodeBase Empty = new EasyUiTreeNodeBase { ID = 0, Text = "-", Title = "-" };
    }

    /// <summary>
    ///     EasyUi�����ݵĸ�ʽ
    /// </summary>
    /// <remarks>������ToJsonʱ���ϸ�ʽ</remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class EasyUiTreeNode : EasyUiTreeNodeBase
    {
        public EasyUiTreeNode()
        {

        }
        public EasyUiTreeNode(List<EasyUiTreeNode> children)
        {
            IsFolder = children != null && children.Count > 0;
            _children = children;
        }
        /// <summary>
        /// �з��Ӽ�
        /// </summary>
        public bool HaseChildren => IsFolder && _children != null && _children.Count > 0;


        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        private List<EasyUiTreeNode> _children;

        /// <summary>
        ///     ��������¼�
        /// </summary>
        [JsonIgnore]
        public List<EasyUiTreeNode> Children => !IsFolder
            ? null
            : (_children ?? (_children = new List<EasyUiTreeNode>()));

        /// <summary>
        ///     ������ر�״̬
        /// </summary>
        [ JsonProperty("state")]
        public override string TreeState => !IsFolder
            ? "open"
            : (IsOpen == null
                ? (Children.Count > 0 ? "open" : "closed")
                : IsOpen.Value ? "open" : "closed");

        public static EasyUiTreeNode EmptyNode = new EasyUiTreeNode { ID = 0, Text = "-", Title = "-", IsOpen = true };

        public override string ToString()
        {
            return $"{Text}(IsOpen:{IsOpen},IsFolder:{IsFolder},HaseChildren:{HaseChildren})";
        }
    }

    /// <summary>
    ///     EasyUi�����ݵĸ�ʽ
    /// </summary>
    /// <remarks>������ToJsonʱ���ϸ�ʽ</remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class EasyUiTreeNode<TData> : EasyUiTreeNodeBase
    {
        /// <summary>
        /// �з��Ӽ�
        /// </summary>
        public bool HaseChildren => IsFolder && _children != null && _children.Count > 0;

        /// <summary>
        ///     ����
        /// </summary>
        [ JsonProperty("data")]
        public TData Data { get; set; }
        
        [ JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        private List<EasyUiTreeNode<TData>> _children;

        /// <summary>
        ///     ��������¼�
        /// </summary>
        [ JsonIgnore]
        public List<EasyUiTreeNode<TData>> Children => !IsFolder
            ? null
            : (_children ?? (_children = new List<EasyUiTreeNode<TData>>()));

        /// <summary>
        ///     ������ر�״̬
        /// </summary>
        [ JsonProperty("state")]
        public override string TreeState => !IsFolder
            ? "open"
            : IsOpen == null
                ? (Children.Count > 0 ? "open" : "closed")
                : IsOpen.Value ? "open" : "closed";

        public override string ToString()
        {
            return $"{Text}(IsOpen:{IsOpen},IsFolder:{IsFolder},HaseChildren:{HaseChildren})";
        }
    }
}