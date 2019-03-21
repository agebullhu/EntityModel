// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-16
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System.Collections.Generic;

#endregion

namespace Agebull.EntityModel.Common.WebUI
{
    /// <summary>
    ///     两层树形数据
    /// </summary>
    /// <typeparam name="TParent">上级类型</typeparam>
    /// <typeparam name="TChild">子级类型</typeparam>
    public class FamilyData<TParent, TChild>
    {
        private List<TChild> _children;

        /// <summary>
        ///     本级数据
        /// </summary>
        public TParent Data { get; set; }

        /// <summary>
        ///     子级
        /// </summary>
        public List<TChild> Children
        {
            get => _children ?? (_children = new List<TChild>());
            set => _children = value;
        }
    }

    /// <summary>
    ///     树形数据
    /// </summary>
    /// <typeparam name="TData">数据类型</typeparam>
    public class TreeData<TData>
    {
        /// <summary>
        ///     子级
        /// </summary>
        private List<TData> _children;

        /// <summary>
        ///     本级数据
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        ///     子级
        /// </summary>
        public List<TData> Children
        {
            get => _children ?? (_children = new List<TData>());
            set => _children = value;
        }
    }

    /// <summary>
    ///     树形数据(带关联节点)
    /// </summary>
    /// <typeparam name="TData">本级数据</typeparam>
    /// <typeparam name="TItem">其它关联节点</typeparam>
    public class TreeData<TData, TItem>
    {
        /// <summary>
        ///     子级
        /// </summary>
        private List<TData> _children;

        /// <summary>
        ///     子级
        /// </summary>
        private List<TItem> _items;

        /// <summary>
        ///     本级数据
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        ///     其它关联节点
        /// </summary>
        public List<TItem> Items
        {
            get => _items ?? (_items = new List<TItem>());
            set => _items = value;
        }

        /// <summary>
        ///     子级
        /// </summary>
        public List<TData> Children
        {
            get => _children ?? (_children = new List<TData>());
            set => _children = value;
        }
    }

    /// <summary>
    ///     树形数据(带关联节点)
    /// </summary>
    /// <typeparam name="TData">本级数据</typeparam>
    /// <typeparam name="TItem">其它关联节点</typeparam>
    /// <typeparam name="TFriend">本级关联数据</typeparam>
    public class TreeData<TData, TItem, TFriend>
    {
        /// <summary>
        ///     子级
        /// </summary>
        private List<TData> _children;

        /// <summary>
        ///     其它关联节点
        /// </summary>
        private List<TItem> _items;

        /// <summary>
        ///     本级数据
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        ///     本级关联数据
        /// </summary>
        public TFriend Friend { get; set; }

        /// <summary>
        ///     其它关联节点
        /// </summary>
        public List<TItem> Items
        {
            get => _items ?? (_items = new List<TItem>());
            set => _items = value;
        }

        /// <summary>
        ///     子级
        /// </summary>
        public List<TData> Children
        {
            get => _children ?? (_children = new List<TData>());
            set => _children = value;
        }
    }

    /// <summary>
    ///     分页类
    /// </summary>
    public class PageData<T>
    {
        /// <summary>
        ///     分页类
        /// </summary>
        public PageData()
        {
            PageSize = 10;
        }

        /// <summary>
        ///     查询条件
        /// </summary>
        public string WhereStr { set; get; }

        /// <summary>
        ///     当前页码
        /// </summary>
        public int PageIndex { set; get; }

        /// <summary>
        ///     每页显示条数
        /// </summary>
        public int PageSize { set; get; }

        /// <summary>
        ///     总记录数
        /// </summary>
        public int Count { set; get; }

        /// <summary>
        ///     总页码数
        /// </summary>
        public int PageCount { set; get; }

        /// <summary>
        ///     数据集
        /// </summary>
        public List<T> DataSoure { set; get; }
    }
}