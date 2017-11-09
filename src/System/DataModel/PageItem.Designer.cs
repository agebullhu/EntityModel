using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Runtime.Serialization;
using Agebull.Common;
using Gboxt.Common.DataModel;

using Gboxt.Common.SystemModel.DataAccess;
using Gboxt.Common.DataModel.MySql;
using Newtonsoft.Json;

namespace Gboxt.Common.SystemModel
{
    /// <summary>
    /// 页面节点
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class PageItemData : IIdentityData
    {
        #region 构造

        /// <summary>
        /// 构造
        /// </summary>
        public PageItemData()
        {
            Initialize();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        partial void Initialize();


        /// <summary>
        /// 对象标识
        /// </summary>
        [IgnoreDataMember, Browsable(false)]
        public long Id
        {
            get
            {
                return this.ID;
            }
            set
            {
                this.ID = value;
            }
        }

        /// <summary>
        /// Id键
        /// </summary>
        long IIdentityData.Id
        {
            get
            {
                return this.ID;
            }
            set
            {
                this.ID = value;
            }
        }
        #endregion


        #region 属性字义


        /// <summary>
        /// 修改主键
        /// </summary>
        public void ChangePrimaryKey(int id)
        {
            _id = id;
        }

        /// <summary>
        /// 标识:标识的实时记录顺序
        /// </summary>
        internal const int Real_ID = 0;

        /// <summary>
        /// 标识:标识
        /// </summary>
        [DataMember, JsonIgnore]
        internal long _id;

        partial void OnIDGet();

        partial void OnIDSet(ref long value);

        partial void OnIDLoad(ref long value);

        partial void OnIDSeted();

        /// <summary>
        /// 标识:标识
        /// </summary>
        /// <remarks>
        /// 标识
        /// </remarks>
        [IgnoreDataMember, ReadOnly(true), DisplayName(@"标识")]
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        public long ID
        {
            get
            {
                OnIDGet();
                return this._id;
            }
            set
            {
                if (this._id == value)
                    return;
                //if(this._id > 0)
                //    throw new Exception("主键一旦设置就不可以修改");
                OnIDSet(ref value);
                this._id = value;
                this.OnPropertyChanged(Real_ID);
                OnIDSeted();
            }
        }
        /// <summary>
        /// 节点类型:节点类型的实时记录顺序
        /// </summary>
        internal const int Real_ItemType = 1;

        /// <summary>
        /// 节点类型:节点类型
        /// </summary>
        [DataMember, JsonIgnore]
        internal PageItemType _itemtype;

        partial void OnItemTypeGet();

        partial void OnItemTypeSet(ref PageItemType value);

        partial void OnItemTypeSeted();

        /// <summary>
        /// 节点类型:节点类型
        /// </summary>
        /// <remarks>
        /// 节点类型
        /// </remarks>
        [IgnoreDataMember, DisplayName(@"节点类型")]
        [JsonProperty("ItemType", NullValueHandling = NullValueHandling.Ignore)]
        public PageItemType ItemType
        {
            get
            {
                OnItemTypeGet();
                return this._itemtype;
            }
            set
            {
                if (this._itemtype == value)
                    return;
                OnItemTypeSet(ref value);
                this._itemtype = value;
                OnItemTypeSeted();
                this.OnPropertyChanged(Real_ItemType);
            }
        }

        /// <summary>
        /// 名称:名称的实时记录顺序
        /// </summary>
        internal const int Real_Name = 2;

        /// <summary>
        /// 名称:名称
        /// </summary>
        [DataMember, JsonIgnore]
        internal string _name;

        partial void OnNameGet();

        partial void OnNameSet(ref string value);

        partial void OnNameSeted();

        /// <summary>
        /// 名称:名称
        /// </summary>
        /// <remarks>
        /// 名称
        /// </remarks>
        [IgnoreDataMember, DisplayName(@"名称")]
        [JsonProperty("Name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name
        {
            get
            {
                OnNameGet();
                return this._name;
            }
            set
            {
                if (this._name == value)
                    return;
                OnNameSet(ref value);
                this._name = value;
                OnNameSeted();
                this.OnPropertyChanged(Real_Name);
            }
        }
        /// <summary>
        /// 标题:标题的实时记录顺序
        /// </summary>
        internal const int Real_Caption = 3;

        /// <summary>
        /// 标题:标题
        /// </summary>
        [DataMember, JsonIgnore]
        internal string _caption;

        partial void OnCaptionGet();

        partial void OnCaptionSet(ref string value);

        partial void OnCaptionSeted();

        /// <summary>
        /// 标题:标题
        /// </summary>
        /// <remarks>
        /// 标题
        /// </remarks>
        [IgnoreDataMember, DisplayName(@"标题")]
        [JsonProperty("Caption", NullValueHandling = NullValueHandling.Ignore)]
        public string Caption
        {
            get
            {
                OnCaptionGet();
                return this._caption;
            }
            set
            {
                if (this._caption == value)
                    return;
                OnCaptionSet(ref value);
                this._caption = value;
                OnCaptionSeted();
                this.OnPropertyChanged(Real_Caption);
            }
        }
        /// <summary>
        /// 图标:图标的实时记录顺序
        /// </summary>
        internal const int Real_Icon = 4;

        /// <summary>
        /// 图标:图标
        /// </summary>
        [DataMember, JsonIgnore]
        internal string _icon;

        partial void OnIconGet();

        partial void OnIconSet(ref string value);

        partial void OnIconSeted();

        /// <summary>
        /// 图标:图标
        /// </summary>
        /// <remarks>
        /// 图标
        /// </remarks>
        [IgnoreDataMember, DisplayName(@"图标")]
        [JsonProperty("Icon", NullValueHandling = NullValueHandling.Ignore)]
        public string Icon
        {
            get
            {
                OnIconGet();
                return this._icon;
            }
            set
            {
                if (this._icon == value)
                    return;
                OnIconSet(ref value);
                this._icon = value;
                OnIconSeted();
                this.OnPropertyChanged(Real_Icon);
            }
        }
        /// <summary>
        /// Json内容:Json内容的实时记录顺序
        /// </summary>
        internal const int Real_Json = 5;

        /// <summary>
        /// Json内容:Json内容
        /// </summary>
        [DataMember, JsonIgnore]
        internal string _json;

        partial void OnJsonGet();

        partial void OnJsonSet(ref string value);

        partial void OnJsonSeted();

        /// <summary>
        /// Json内容
        /// </summary>
        /// <remarks>
        /// Json内容
        /// </remarks>
        [IgnoreDataMember, DisplayName(@"Json内容")]
        [JsonProperty("Json", NullValueHandling = NullValueHandling.Ignore)]
        public string Json
        {
            get
            {
                OnJsonGet();
                return this._json;
            }
            set
            {
                if (this._json == value)
                    return;
                OnJsonSet(ref value);
                this._json = value;
                OnJsonSeted();
                this.OnPropertyChanged(Real_Json);
            }
        }
        /// <summary>
        /// 页面连接:页面连接的实时记录顺序
        /// </summary>
        internal const int Real_Url = 6;

        /// <summary>
        /// 页面连接:页面连接
        /// </summary>
        [DataMember, JsonIgnore]
        internal string _url;

        partial void OnUrlGet();

        partial void OnUrlSet(ref string value);

        partial void OnUrlSeted();

        /// <summary>
        /// 页面连接:页面连接
        /// </summary>
        /// <remarks>
        /// 页面连接
        /// </remarks>
        [IgnoreDataMember, DisplayName(@"页面连接")]
        [JsonProperty("Url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url
        {
            get
            {
                OnUrlGet();
                return this._url;
            }
            set
            {
                if (this._url == value)
                    return;
                OnUrlSet(ref value);
                this._url = value;
                OnUrlSeted();
                this.OnPropertyChanged(Real_Url);
            }
        }
        /// <summary>
        /// 备注:备注的实时记录顺序
        /// </summary>
        internal const int Real_Memo = 7;

        /// <summary>
        /// 备注:备注
        /// </summary>
        [DataMember, JsonIgnore]
        internal string _memo;

        partial void OnMemoGet();

        partial void OnMemoSet(ref string value);

        partial void OnMemoSeted();

        /// <summary>
        /// 备注:备注
        /// </summary>
        /// <remarks>
        /// 备注
        /// </remarks>
        [IgnoreDataMember, DisplayName(@"备注")]
        [JsonProperty("Memo", NullValueHandling = NullValueHandling.Ignore)]
        public string Memo
        {
            get
            {
                OnMemoGet();
                return this._memo;
            }
            set
            {
                if (this._memo == value)
                    return;
                OnMemoSet(ref value);
                this._memo = value;
                OnMemoSeted();
                this.OnPropertyChanged(Real_Memo);
            }
        }
        /// <summary>
        /// 上级节点:上级节点的实时记录顺序
        /// </summary>
        internal const int Real_ParentId = 8;

        /// <summary>
        /// 上级节点:上级节点
        /// </summary>
        [DataMember, JsonIgnore]
        internal long _parentid;

        partial void OnParentIdGet();

        partial void OnParentIdSet(ref long value);

        partial void OnParentIdSeted();

        /// <summary>
        /// 上级节点:上级节点
        /// </summary>
        /// <remarks>
        /// 上级节点
        /// </remarks>
        [IgnoreDataMember, DisplayName(@"上级节点")]
        [JsonProperty("ParentId", NullValueHandling = NullValueHandling.Ignore)]
        public long ParentId
        {
            get
            {
                OnParentIdGet();
                return this._parentid;
            }
            set
            {
                if (this._parentid == value)
                    return;
                OnParentIdSet(ref value);
                this._parentid = value;
                OnParentIdSeted();
                this.OnPropertyChanged(Real_ParentId);
            }
        }
        /// <summary>
        /// 扩展内容:扩展内容的实时记录顺序
        /// </summary>
        internal const int Real_ExtendValue = 9;

        /// <summary>
        /// 扩展内容:扩展内容
        /// </summary>
        [DataMember, JsonIgnore]
        internal string _extendvalue;

        partial void OnExtendValueGet();

        partial void OnExtendValueSet(ref string value);

        partial void OnExtendValueSeted();

        /// <summary>
        /// 扩展内容:扩展内容
        /// </summary>
        /// <remarks>
        /// 扩展内容
        /// </remarks>
        [IgnoreDataMember, DisplayName(@"扩展内容")]
        [JsonProperty("ExtendValue", NullValueHandling = NullValueHandling.Ignore)]
        public string ExtendValue
        {
            get
            {
                OnExtendValueGet();
                return this._extendvalue;
            }
            set
            {
                if (this._extendvalue == value)
                    return;
                OnExtendValueSet(ref value);
                this._extendvalue = value;
                OnExtendValueSeted();
                this.OnPropertyChanged(Real_ExtendValue);
            }
        }

        /// <summary>
        /// 扩展内容:扩展内容的实时记录顺序
        /// </summary>
        internal const int Real_Index = 10;

        /// <summary>
        /// 扩展内容:扩展内容
        /// </summary>
        [DataMember, JsonIgnore]
        internal int _index;

        partial void OnIndexGet();

        partial void OnIndexSet(ref int value);

        partial void OnIndexSeted();

        /// <summary>
        /// 序号
        /// </summary>
        /// <remarks>
        /// 用于排序
        /// </remarks>
        [IgnoreDataMember, DisplayName(@"序号")]
        [JsonProperty("Index", NullValueHandling = NullValueHandling.Ignore)]
        public int Index
        {
            get
            {
                OnIndexGet();
                return this._index;
            }
            set
            {
                if (this._index == value)
                    return;
                OnIndexSet(ref value);
                this._index = value;
                OnIndexSeted();
                this.OnPropertyChanged(Real_Index);
            }
        }

        #endregion

        #region 属性扩展




        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        protected override void SetValueInner(string property, object value)
        {
            switch (property.Trim().ToLower())
            {
                case "id":
                    this.ID = (int)Convert.ToDecimal(value);
                    return;
                case "itemtype":
                    this.ItemType = (PageItemType)Convert.ToDecimal(value);
                    return;
                case "name":
                    this.Name = value == null ? null : value.ToString();
                    return;
                case "caption":
                    this.Caption = value == null ? null : value.ToString();
                    return;
                case "icon":
                    this.Icon = value == null ? null : value.ToString();
                    return;
                case "Json":
                    this.Json = value == null ? null : value.ToString();
                    return;
                case "url":
                    this.Url = value == null ? null : value.ToString();
                    return;
                case "memo":
                    this.Memo = value == null ? null : value.ToString();
                    return;
                case "parentid":
                    this.ParentId = (int)Convert.ToDecimal(value);
                    return;
                case "extendvalue":
                    this.ExtendValue = value == null ? null : value.ToString();
                    return;
                case "index":
                    this.Index = (int)Convert.ToDecimal(value);
                    return ;
            }

            System.Diagnostics.Trace.WriteLine(property + @"=>" + value);

        }



        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected override void SetValueInner(int index, object value)
        {
            /*switch(index)
            {
            case Index_ID:
                this.ID = Convert.ToInt32(value);
                return;
            case Index_ItemType:
                this.ItemType = Convert.ToInt32(value);
                return;
            case Index_Name:
                this.Name = value == null ? null : value.ToString();
                return;
            case Index_Caption:
                this.Caption = value == null ? null : value.ToString();
                return;
            case Index_Icon:
                this.Icon = value == null ? null : value.ToString();
                return;
            case Index_json:
                this.Json = value == null ? null : value.ToString();
                return;
            case Index_Url:
                this.Url = value == null ? null : value.ToString();
                return;
            case Index_Memo:
                this.Memo = value == null ? null : value.ToString();
                return;
            case Index_ParentId:
                this.ParentId = Convert.ToInt32(value);
                return;
            case Index_ExtendValue:
                this.ExtendValue = value == null ? null : value.ToString();
                return;
            }*/
        }


        /// <summary>
        ///     读取属性值
        /// </summary>
        /// <param name="property"></param>
        protected override object GetValueInner(string property)
        {
            switch (property)
            {
                case "ID":
                    return this.ID;
                case "ItemType":
                    return this.ItemType;
                case "Name":
                    return this.Name;
                case "Caption":
                    return this.Caption;
                case "Icon":
                    return this.Icon;
                case "Json":
                    return this.Json;
                case "Url":
                    return this.Url;
                case "Memo":
                    return this.Memo;
                case "ParentId":
                    return this.ParentId;
                case "ExtendValue":
                    return this.ExtendValue;
                case "Index":
                    return this.Index;
            }

            return null;
        }


        /// <summary>
        ///     读取属性值
        /// </summary>
        /// <param name="index"></param>
        protected override object GetValueInner(int index)
        {
            /*switch(index)
            {
                case Index_ID:
                    return this.ID;
                case Index_ItemType:
                    return this.ItemType;
                case Index_Name:
                    return this.Name;
                case Index_Caption:
                    return this.Caption;
                case Index_Icon:
                    return this.Icon;
                case Index_json:
                    return this.Json;
                case Index_Url:
                    return this.Url;
                case Index_Memo:
                    return this.Memo;
                case Index_ParentId:
                    return this.ParentId;
                case Index_ExtendValue:
                    return this.ExtendValue;
            }*/

            return null;
        }

        #endregion

        #region 关联

        #endregion

        #region 复制


        partial void CopyExtendValue(PageItemData source);

        /// <summary>
        /// 复制值
        /// </summary>
        /// <param name="source">复制的源字段</param>
        protected override void CopyValueInner(DataObjectBase source)
        {
            var sourceEntity = source as PageItemData;
            if (sourceEntity == null)
                return;
            using (new EditScope(__EntityStatus, EditArrestMode.All, true))
            {
                this._itemtype = sourceEntity._itemtype;
                this._name = sourceEntity._name;
                this._caption = sourceEntity._caption;
                this._icon = sourceEntity._icon;
                this._json = sourceEntity._json;
                this._url = sourceEntity._url;
                this._memo = sourceEntity._memo;
                this._parentid = sourceEntity._parentid;
                this._extendvalue = sourceEntity._extendvalue;
            }
            CopyExtendValue(sourceEntity);
            this.__EntityStatus.SetModified();
        }

        /*// <summary>
        /// 复制值
        /// </summary>
        /// <param name="source">复制的源字段</param>
        protected override void CopyValueInner(EntityObjectBase source)
        {{
            var sourceEntity = source as PageItemEntity;
            if(sourceEntity == null)
                return;
            using (new EditScope(__EntityStatus, EditArrestMode.All, true))
            {
                this.ID = sourceEntity.ID;
                this.ItemType = sourceEntity.ItemType;
                this.Name = sourceEntity.Name;
                this.Caption = sourceEntity.Caption;
                this.Icon = sourceEntity.Icon;
                this.Json = sourceEntity.Json;
                this.Url = sourceEntity.Url;
                this.Memo = sourceEntity.Memo;
                this.ParentId = sourceEntity.ParentId;
                this.ExtendValue = sourceEntity.ExtendValue;
            }
            CopyExtendValue(sourceEntity);
            this.__EntityStatus.SetModified();
        }*/
        #endregion


        #region 后期处理

        /// <summary>
        /// 单个属性修改的后期处理(保存后)
        /// </summary>
        /// <param name="subsist">当前实体生存状态</param>
        /// <param name="modifieds">修改列表</param>
        /// <remarks>
        /// 对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        protected override void OnLaterPeriodBySignleModified(EntitySubsist subsist, byte[] modifieds)
        {
            if (subsist == EntitySubsist.Deleting)
            {
                OnIDModified(subsist, false);
                OnItemTypeModified(subsist, false);
                OnNameModified(subsist, false);
                OnCaptionModified(subsist, false);
                OnIconModified(subsist, false);
                OnJsonModified(subsist, false);
                OnUrlModified(subsist, false);
                OnMemoModified(subsist, false);
                OnParentIdModified(subsist, false);
                OnExtendValueModified(subsist, false);
                return;
            }
            else if (subsist == EntitySubsist.Adding || subsist == EntitySubsist.Added)
            {
                OnIDModified(subsist, true);
                OnItemTypeModified(subsist, true);
                OnNameModified(subsist, true);
                OnCaptionModified(subsist, true);
                OnIconModified(subsist, true);
                OnJsonModified(subsist, true);
                OnUrlModified(subsist, true);
                OnMemoModified(subsist, true);
                OnParentIdModified(subsist, true);
                OnExtendValueModified(subsist, true);
                return;
            }
            else if (modifieds != null && modifieds[10] > 0)
            {
                OnIDModified(subsist, modifieds[Real_ID] == 1);
                OnItemTypeModified(subsist, modifieds[Real_ItemType] == 1);
                OnNameModified(subsist, modifieds[Real_Name] == 1);
                OnCaptionModified(subsist, modifieds[Real_Caption] == 1);
                OnIconModified(subsist, modifieds[Real_Icon] == 1);
                OnJsonModified(subsist, modifieds[Real_Json] == 1);
                OnUrlModified(subsist, modifieds[Real_Url] == 1);
                OnMemoModified(subsist, modifieds[Real_Memo] == 1);
                OnParentIdModified(subsist, modifieds[Real_ParentId] == 1);
                OnExtendValueModified(subsist, modifieds[Real_ExtendValue] == 1);
            }
        }

        #region 属性后期修改的分部方法

        /// <summary>
        /// 标识修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnIDModified(EntitySubsist subsist, bool isModified);

        /// <summary>
        /// 节点类型修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnItemTypeModified(EntitySubsist subsist, bool isModified);

        /// <summary>
        /// 名称修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnNameModified(EntitySubsist subsist, bool isModified);

        /// <summary>
        /// 标题修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnCaptionModified(EntitySubsist subsist, bool isModified);

        /// <summary>
        /// 图标修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnIconModified(EntitySubsist subsist, bool isModified);

        /// <summary>
        /// Json内容修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnJsonModified(EntitySubsist subsist, bool isModified);

        /// <summary>
        /// 页面连接修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnUrlModified(EntitySubsist subsist, bool isModified);

        /// <summary>
        /// 备注修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnMemoModified(EntitySubsist subsist, bool isModified);

        /// <summary>
        /// 上级节点修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnParentIdModified(EntitySubsist subsist, bool isModified);

        /// <summary>
        /// 扩展内容修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnExtendValueModified(EntitySubsist subsist, bool isModified);

        #endregion

        #endregion


        #region 实体结构


        public const byte Index_ID = 0;
        public const byte Index_ItemType = 1;
        public const byte Index_Name = 2;
        public const byte Index_Caption = 3;
        public const byte Index_Icon = 4;
        public const byte Index_json = 5;
        public const byte Index_Url = 6;
        public const byte Index_Memo = 7;
        public const byte Index_ParentId = 8;
        public const byte Index_ExtendValue = 9;
        public const byte Index_Index = 10;

        /// <summary>
        /// 实体结构
        /// </summary>
        [IgnoreDataMember, Browsable(false)]
        public override EntitySturct __Struct
        {
            get
            {
                return __struct;
            }
        }

        /// <summary>
        /// 实体结构
        /// </summary>
        [IgnoreDataMember]
        static readonly EntitySturct __struct = new EntitySturct
        {
            EntityName = "PageItem",
            PrimaryKey = "ID",
            Properties = new Dictionary<int, PropertySturct>
            {
                {
                    Real_ID,
                    new PropertySturct
                    {
                        Index = Index_ID,
                        PropertyName = "ID",
                        Title = "标识",
                        ColumnName = "ID",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value
                    }
                },
                {
                    Real_ItemType,
                    new PropertySturct
                    {
                        Index = Index_ItemType,
                        PropertyName = "ItemType",
                        Title = "节点类型",
                        ColumnName = "ItemType",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value
                    }
                },
                {
                    Real_Name,
                    new PropertySturct
                    {
                        Index = Index_Name,
                        PropertyName = "Name",
                        Title = "名称",
                        ColumnName = "Name",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String
                    }
                },
                {
                    Real_Caption,
                    new PropertySturct
                    {
                        Index = Index_Caption,
                        PropertyName = "Caption",
                        Title = "标题",
                        ColumnName = "Caption",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String
                    }
                },
                {
                    Real_Icon,
                    new PropertySturct
                    {
                        Index = Index_Icon,
                        PropertyName = "Icon",
                        Title = "图标",
                        ColumnName = "Icon",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String
                    }
                },
                {
                    Real_Json,
                    new PropertySturct
                    {
                        Index = Index_json,
                        PropertyName = "Json",
                        Title = "Json内容",
                        ColumnName = "Json",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String
                    }
                },
                {
                    Real_Url,
                    new PropertySturct
                    {
                        Index = Index_Url,
                        PropertyName = "Url",
                        Title = "页面连接",
                        ColumnName = "Url",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String
                    }
                },
                {
                    Real_Memo,
                    new PropertySturct
                    {
                        Index = Index_Memo,
                        PropertyName = "Memo",
                        Title = "备注",
                        ColumnName = "Memo",
                        PropertyType = typeof(string),
                        CanNull = true,
                        ValueType = PropertyValueType.String
                    }
                },
                {
                    Real_ParentId,
                    new PropertySturct
                    {
                        Index = Index_ParentId,
                        PropertyName = "ParentId",
                        Title = "上级节点",
                        ColumnName = "ParentId",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value
                    }
                },
                {
                    Real_ExtendValue,
                    new PropertySturct
                    {
                        Index = Index_ExtendValue,
                        PropertyName = "ExtendValue",
                        Title = "扩展内容",
                        ColumnName = "ExtendValue",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String
                    }
                },
                {
                    Real_Index,
                    new PropertySturct
                    {
                        Index = Index_Index,
                        PropertyName = "Index",
                        Title = "序号",
                        ColumnName = "Index",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String
                    }
                }
            }
        };

        #endregion
    }
}