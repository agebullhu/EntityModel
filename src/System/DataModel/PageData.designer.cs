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
    /// 用户的页面数据
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class PageDataData : IIdentityData
    {
        #region 构造
        
        /// <summary>
        /// 构造
        /// </summary>
        public PageDataData()
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
        [IgnoreDataMember,Browsable(false)]
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
        [DataMember,JsonIgnore]
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
        [IgnoreDataMember , ReadOnly(true) , DisplayName(@"标识")]
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
                if(this._id == value)
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
        /// 用户标识:用户标识的实时记录顺序
        /// </summary>
        internal const int Real_UserId = 1;

        /// <summary>
        /// 用户标识:用户标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _userid;

        partial void OnUserIdGet();

        partial void OnUserIdSet(ref int value);

        partial void OnUserIdSeted();

        /// <summary>
        /// 用户标识:用户标识
        /// </summary>
        /// <remarks>
        /// 用户标识
        /// </remarks>
        [IgnoreDataMember , DisplayName(@"用户标识")]
        [JsonProperty("UserId", NullValueHandling = NullValueHandling.Ignore)]
        public  int UserId
        {
            get
            {
                OnUserIdGet();
                return this._userid;
            }
            set
            {
                if(this._userid == value)
                    return;
                OnUserIdSet(ref value);
                this._userid = value;
                OnUserIdSeted();
                this.OnPropertyChanged(Real_UserId);
            }
        }
        /// <summary>
        /// 页面标识:页面标识的实时记录顺序
        /// </summary>
        internal const int Real_PageId = 2;

        /// <summary>
        /// 页面标识:页面标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _pageid;

        partial void OnPageIdGet();

        partial void OnPageIdSet(ref int value);

        partial void OnPageIdSeted();

        /// <summary>
        /// 页面标识:页面标识
        /// </summary>
        /// <remarks>
        /// 页面标识
        /// </remarks>
        [IgnoreDataMember , DisplayName(@"页面标识")]
        [JsonProperty("PageId", NullValueHandling = NullValueHandling.Ignore)]
        public  int PageId
        {
            get
            {
                OnPageIdGet();
                return this._pageid;
            }
            set
            {
                if(this._pageid == value)
                    return;
                OnPageIdSet(ref value);
                this._pageid = value;
                OnPageIdSeted();
                this.OnPropertyChanged(Real_PageId);
            }
        }
        /// <summary>
        /// 页面数据:页面数据的实时记录顺序
        /// </summary>
        internal const int Real_PageData = 3;

        /// <summary>
        /// 页面数据:页面数据
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _pagedata;

        partial void OnPageDataGet();

        partial void OnPageDataSet(ref string value);

        partial void OnPageDataSeted();

        /// <summary>
        /// 页面数据:页面数据
        /// </summary>
        /// <remarks>
        /// 页面数据
        /// </remarks>
        [IgnoreDataMember , DisplayName(@"页面数据")]
        [JsonProperty("PageData", NullValueHandling = NullValueHandling.Ignore)]
        public  string PageData
        {
            get
            {
                OnPageDataGet();
                return this._pagedata;
            }
            set
            {
                if(this._pagedata == value)
                    return;
                OnPageDataSet(ref value);
                this._pagedata = value;
                OnPageDataSeted();
                this.OnPropertyChanged(Real_PageData);
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
            switch(property.Trim().ToLower())
            {
            case "id":
                this.ID = (int)Convert.ToDecimal(value);
                return;
            case "userid":
                this.UserId = (int)Convert.ToDecimal(value);
                return;
            case "pageid":
                this.PageId = (int)Convert.ToDecimal(value);
                return;
            case "pagedata":
                this.PageData = value == null ? null : value.ToString();
                return;
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
            case Index_UserId:
                this.UserId = Convert.ToInt32(value);
                return;
            case Index_PageId:
                this.PageId = Convert.ToInt32(value);
                return;
            case Index_PageData:
                this.PageData = value == null ? null : value.ToString();
                return;
            }*/
        }


        /// <summary>
        ///     读取属性值
        /// </summary>
        /// <param name="property"></param>
        protected override object GetValueInner(string property)
        {
            switch(property)
            {
            case "ID":
                return this.ID;
            case "UserId":
                return this.UserId;
            case "PageId":
                return this.PageId;
            case "PageData":
                return this.PageData;
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
                case Index_UserId:
                    return this.UserId;
                case Index_PageId:
                    return this.PageId;
                case Index_PageData:
                    return this.PageData;
            }*/

            return null;
        }

        #endregion

        #region 关联

        #endregion

        #region 复制


        partial void CopyExtendValue(PageDataData source);

        /// <summary>
        /// 复制值
        /// </summary>
        /// <param name="source">复制的源字段</param>
        protected override void CopyValueInner(DataObjectBase source)
        {
            var sourceEntity = source as PageDataData;
            if(sourceEntity == null)
                return;
            using (new EditScope(__EntityStatus, EditArrestMode.All, true))
            {
                this._userid = sourceEntity._userid;
                this._pageid = sourceEntity._pageid;
                this._pagedata = sourceEntity._pagedata;
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
            var sourceEntity = source as PageDataEntity;
            if(sourceEntity == null)
                return;
            using (new EditScope(__EntityStatus, EditArrestMode.All, true))
            {
                this.ID = sourceEntity.ID;
                this.UserId = sourceEntity.UserId;
                this.PageId = sourceEntity.PageId;
                this.PageData = sourceEntity.PageData;
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
        protected override void OnLaterPeriodBySignleModified(EntitySubsist subsist,byte[] modifieds)
        {
            if (subsist == EntitySubsist.Deleting)
            {
                OnIDModified(subsist,false);
                OnUserIdModified(subsist,false);
                OnPageIdModified(subsist,false);
                OnPageDataModified(subsist,false);
                return;
            }
            else if (subsist == EntitySubsist.Adding || subsist == EntitySubsist.Added)
            {
                OnIDModified(subsist,true);
                OnUserIdModified(subsist,true);
                OnPageIdModified(subsist,true);
                OnPageDataModified(subsist,true);
                return;
            }
            else if(modifieds != null && modifieds[4] > 0)
            {
                OnIDModified(subsist,modifieds[Real_ID] == 1);
                OnUserIdModified(subsist,modifieds[Real_UserId] == 1);
                OnPageIdModified(subsist,modifieds[Real_PageId] == 1);
                OnPageDataModified(subsist,modifieds[Real_PageData] == 1);
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
        partial void OnIDModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 用户标识修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnUserIdModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 页面标识修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnPageIdModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 页面数据修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnPageDataModified(EntitySubsist subsist,bool isModified);

        #endregion

        #endregion


        #region 实体结构

        
        public const byte Index_ID = 0;
        public const byte Index_UserId = 1;
        public const byte Index_PageId = 2;
        public const byte Index_PageData = 3;

        /// <summary>
        /// 实体结构
        /// </summary>
        [IgnoreDataMember,Browsable (false)]
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
            EntityName = "PageData",
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
                    Real_UserId,
                    new PropertySturct
                    {
                        Index = Index_UserId,
                        PropertyName = "UserId",
                        Title = "用户标识",
                        ColumnName = "UserId",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value
                    }
                },
                {
                    Real_PageId,
                    new PropertySturct
                    {
                        Index = Index_PageId,
                        PropertyName = "PageId",
                        Title = "页面标识",
                        ColumnName = "PageId",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value
                    }
                },
                {
                    Real_PageData,
                    new PropertySturct
                    {
                        Index = Index_PageData,
                        PropertyName = "PageData",
                        Title = "页面数据",
                        ColumnName = "PageData",
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