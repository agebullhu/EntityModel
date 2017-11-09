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
    /// 数据字典
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class DataDictionaryData : IIdentityData
    {
        #region 构造
        
        /// <summary>
        /// 构造
        /// </summary>
        public DataDictionaryData()
        {
            Initialize();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        partial void Initialize();


        /// <summary>
        /// Id键
        /// </summary>
        long IIdentityData.Id
        {
            get
            {
                return this.Id;
            }
            set
            {
                this.Id = value;
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
        internal const int Real_Id = 0;

        /// <summary>
        /// 标识:标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal long _id;

        partial void OnIdGet();

        partial void OnIdSet(ref long value);

        partial void OnIdLoad(ref long value);

        partial void OnIdSeted();

        /// <summary>
        /// 标识:标识
        /// </summary>
        /// <remarks>
        /// 标识
        /// </remarks>
        [IgnoreDataMember , ReadOnly(true) , DisplayName(@"标识")]
        [JsonProperty("Id", NullValueHandling = NullValueHandling.Ignore)]
        public long Id
        {
            get
            {
                OnIdGet();
                return this._id;
            }
            set
            {
                if(this._id == value)
                    return;
                //if(this._id > 0)
                //    throw new Exception("主键一旦设置就不可以修改");
                OnIdSet(ref value);
                this._id = value;
                this.OnPropertyChanged(Real_Id);
                OnIdSeted();
            }
        }
        /// <summary>
        /// 名称:名称的实时记录顺序
        /// </summary>
        internal const int Real_Name = 1;

        /// <summary>
        /// 名称:名称
        /// </summary>
        [DataMember,JsonIgnore]
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
        [IgnoreDataMember , DisplayName(@"名称")]
        [JsonProperty("Name", NullValueHandling = NullValueHandling.Ignore)]
        public  string Name
        {
            get
            {
                OnNameGet();
                return this._name;
            }
            set
            {
                if(this._name == value)
                    return;
                OnNameSet(ref value);
                this._name = value;
                OnNameSeted();
                this.OnPropertyChanged(Real_Name);
            }
        }
        /// <summary>
        /// 状态:状态的实时记录顺序
        /// </summary>
        internal const int Real_State = 2;

        /// <summary>
        /// 状态:状态
        /// </summary>
        [DataMember,JsonIgnore]
        internal long _state;

        partial void OnStateGet();

        partial void OnStateSet(ref long value);

        partial void OnStateSeted();

        /// <summary>
        /// 状态:状态
        /// </summary>
        /// <remarks>
        /// 状态
        /// </remarks>
        [IgnoreDataMember , DisplayName(@"状态")]
        [JsonProperty("State", NullValueHandling = NullValueHandling.Ignore)]
        public  long State
        {
            get
            {
                OnStateGet();
                return this._state;
            }
            set
            {
                if(this._state == value)
                    return;
                OnStateSet(ref value);
                this._state = value;
                OnStateSeted();
                this.OnPropertyChanged(Real_State);
            }
        }
        /// <summary>
        /// 特性:特性的实时记录顺序
        /// </summary>
        internal const int Real_Feature = 3;

        /// <summary>
        /// 特性:特性
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _feature;

        partial void OnFeatureGet();

        partial void OnFeatureSet(ref string value);

        partial void OnFeatureSeted();

        /// <summary>
        /// 特性:特性
        /// </summary>
        /// <remarks>
        /// 特性
        /// </remarks>
        [IgnoreDataMember , DisplayName(@"特性")]
        [JsonProperty("Feature", NullValueHandling = NullValueHandling.Ignore)]
        public  string Feature
        {
            get
            {
                OnFeatureGet();
                return this._feature;
            }
            set
            {
                if(this._feature == value)
                    return;
                OnFeatureSet(ref value);
                this._feature = value;
                OnFeatureSeted();
                this.OnPropertyChanged(Real_Feature);
            }
        }
        /// <summary>
        /// 备注:备注的实时记录顺序
        /// </summary>
        internal const int Real_Memo = 4;

        /// <summary>
        /// 备注:备注
        /// </summary>
        [DataMember,JsonIgnore]
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
        [IgnoreDataMember , DisplayName(@"备注")]
        [JsonProperty("Memo", NullValueHandling = NullValueHandling.Ignore)]
        public  string Memo
        {
            get
            {
                OnMemoGet();
                return this._memo;
            }
            set
            {
                if(this._memo == value)
                    return;
                OnMemoSet(ref value);
                this._memo = value;
                OnMemoSeted();
                this.OnPropertyChanged(Real_Memo);
            }
        }
        /// <summary>
        /// 值:值的实时记录顺序
        /// </summary>
        internal const int Real_Value = 5;

        /// <summary>
        /// 值:值
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _value;

        partial void OnValueGet();

        partial void OnValueSet(ref string value);

        partial void OnValueSeted();

        /// <summary>
        /// 值:值
        /// </summary>
        /// <remarks>
        /// 值
        /// </remarks>
        [IgnoreDataMember , DisplayName(@"值")]
        [JsonProperty("Value", NullValueHandling = NullValueHandling.Ignore)]
        public  string Value
        {
            get
            {
                OnValueGet();
                return this._value;
            }
            set
            {
                if(this._value == value)
                    return;
                OnValueSet(ref value);
                this._value = value;
                OnValueSeted();
                this.OnPropertyChanged(Real_Value);
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
                this.Id = (int)Convert.ToDecimal(value);
                return;
            case "name":
                this.Name = value == null ? null : value.ToString();
                return;
            case "state":
                this.State = (long)Convert.ToDecimal(value);
                return;
            case "feature":
                this.Feature = value == null ? null : value.ToString();
                return;
            case "memo":
                this.Memo = value == null ? null : value.ToString();
                return;
            case "value":
                this.Value = value == null ? null : value.ToString();
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
            case Index_Id:
                this.Id = Convert.ToInt32(value);
                return;
            case Index_Name:
                this.Name = value == null ? null : value.ToString();
                return;
            case Index_State:
                this.State = Convert.ToInt64(value);
                return;
            case Index_Feature:
                this.Feature = value == null ? null : value.ToString();
                return;
            case Index_Memo:
                this.Memo = value == null ? null : value.ToString();
                return;
            case Index_Value:
                this.Value = value == null ? null : value.ToString();
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
            case "Id":
                return this.Id;
            case "Name":
                return this.Name;
            case "State":
                return this.State;
            case "Feature":
                return this.Feature;
            case "Memo":
                return this.Memo;
            case "Value":
                return this.Value;
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
                case Index_Id:
                    return this.Id;
                case Index_Name:
                    return this.Name;
                case Index_State:
                    return this.State;
                case Index_Feature:
                    return this.Feature;
                case Index_Memo:
                    return this.Memo;
                case Index_Value:
                    return this.Value;
            }*/

            return null;
        }

        #endregion

        #region 关联

        #endregion

        #region 复制


        partial void CopyExtendValue(DataDictionaryData source);

        /// <summary>
        /// 复制值
        /// </summary>
        /// <param name="source">复制的源字段</param>
        protected override void CopyValueInner(DataObjectBase source)
        {
            var sourceEntity = source as DataDictionaryData;
            if(sourceEntity == null)
                return;
            using (new EditScope(__EntityStatus, EditArrestMode.All, true))
            {
                this._name = sourceEntity._name;
                this._state = sourceEntity._state;
                this._feature = sourceEntity._feature;
                this._memo = sourceEntity._memo;
                this._value = sourceEntity._value;
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
            var sourceEntity = source as DataDictionaryEntity;
            if(sourceEntity == null)
                return;
            using (new EditScope(__EntityStatus, EditArrestMode.All, true))
            {
                this.Id = sourceEntity.Id;
                this.Name = sourceEntity.Name;
                this.State = sourceEntity.State;
                this.Feature = sourceEntity.Feature;
                this.Memo = sourceEntity.Memo;
                this.Value = sourceEntity.Value;
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
                OnIdModified(subsist,false);
                OnNameModified(subsist,false);
                OnStateModified(subsist,false);
                OnFeatureModified(subsist,false);
                OnMemoModified(subsist,false);
                OnValueModified(subsist,false);
                return;
            }
            else if (subsist == EntitySubsist.Adding || subsist == EntitySubsist.Added)
            {
                OnIdModified(subsist,true);
                OnNameModified(subsist,true);
                OnStateModified(subsist,true);
                OnFeatureModified(subsist,true);
                OnMemoModified(subsist,true);
                OnValueModified(subsist,true);
                return;
            }
            else if(modifieds != null && modifieds[6] > 0)
            {
                OnIdModified(subsist,modifieds[Real_Id] == 1);
                OnNameModified(subsist,modifieds[Real_Name] == 1);
                OnStateModified(subsist,modifieds[Real_State] == 1);
                OnFeatureModified(subsist,modifieds[Real_Feature] == 1);
                OnMemoModified(subsist,modifieds[Real_Memo] == 1);
                OnValueModified(subsist,modifieds[Real_Value] == 1);
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
        partial void OnIdModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 名称修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnNameModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 状态修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnStateModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 特性修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnFeatureModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 备注修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnMemoModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 值修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnValueModified(EntitySubsist subsist,bool isModified);

        #endregion

        #endregion


        #region 实体结构

        
        public const byte Index_Id = 0;
        public const byte Index_Name = 1;
        public const byte Index_State = 2;
        public const byte Index_Feature = 3;
        public const byte Index_Memo = 4;
        public const byte Index_Value = 5;

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
            EntityName = "DataDictionary",
            PrimaryKey = "Id",
            Properties = new Dictionary<int, PropertySturct>
            {
                {
                    Real_Id,
                    new PropertySturct
                    {
                        Index = Index_Id,
                        PropertyName = "Id",
                        Title = "标识",
                        ColumnName = "Id",
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
                    Real_State,
                    new PropertySturct
                    {
                        Index = Index_State,
                        PropertyName = "State",
                        Title = "状态",
                        ColumnName = "State",
                        PropertyType = typeof(long),
                        CanNull = false,
                        ValueType = PropertyValueType.Value
                    }
                },
                {
                    Real_Feature,
                    new PropertySturct
                    {
                        Index = Index_Feature,
                        PropertyName = "Feature",
                        Title = "特性",
                        ColumnName = "Feature",
                        PropertyType = typeof(string),
                        CanNull = true,
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
                    Real_Value,
                    new PropertySturct
                    {
                        Index = Index_Value,
                        PropertyName = "Value",
                        Title = "值",
                        ColumnName = "Value",
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