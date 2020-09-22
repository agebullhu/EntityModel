/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/9/16 10:40:07*/
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
using Agebull.EntityModel.Common;
using Agebull.EntityModel.Interfaces;


#endregion

namespace Zeroteam.MessageMVC.EventBus
{

    /// <summary>
    /// 实体结构
    /// </summary>
    public sealed class EventSubscribeDataStruct
    {
        /// <summary>
        /// 实体名称
        /// </summary>
        public const string EntityName = @"EventSubscribe";
        /// <summary>
        /// 实体标题
        /// </summary>
        public const string EntityCaption = @"事件订阅";
        /// <summary>
        /// 实体说明
        /// </summary>
        public const string EntityDescription = @"事件订阅";
        /// <summary>
        /// 实体标识
        /// </summary>
        public const int EntityIdentity = 0x0;
        /// <summary>
        /// 实体说明
        /// </summary>
        public const string EntityPrimaryKey = "Id";



        /// <summary>
        /// 主键的数字标识
        /// </summary>
        public const int Id = 1;

        /// <summary>
        /// 主键的实时记录顺序
        /// </summary>
        public const int Real_Id = 0;

        /// <summary>
        /// 事件标识的数字标识
        /// </summary>
        public const int EventId = 2;

        /// <summary>
        /// 事件标识的实时记录顺序
        /// </summary>
        public const int Real_EventId = 1;

        /// <summary>
        /// 所属服务的数字标识
        /// </summary>
        public const int Service = 3;

        /// <summary>
        /// 所属服务的实时记录顺序
        /// </summary>
        public const int Real_Service = 2;

        /// <summary>
        /// 是否查阅服务的数字标识
        /// </summary>
        public const int IsLookUp = 4;

        /// <summary>
        /// 是否查阅服务的实时记录顺序
        /// </summary>
        public const int Real_IsLookUp = 3;

        /// <summary>
        /// 接口名称的数字标识
        /// </summary>
        public const int ApiName = 5;

        /// <summary>
        /// 接口名称的实时记录顺序
        /// </summary>
        public const int Real_ApiName = 4;

        /// <summary>
        /// 目标说明的数字标识
        /// </summary>
        public const int TargetDescription = 6;

        /// <summary>
        /// 目标说明的实时记录顺序
        /// </summary>
        public const int Real_TargetDescription = 5;

        /// <summary>
        /// 目标名称的数字标识
        /// </summary>
        public const int TargetName = 7;

        /// <summary>
        /// 目标名称的实时记录顺序
        /// </summary>
        public const int Real_TargetName = 6;

        /// <summary>
        /// 目标类型的数字标识
        /// </summary>
        public const int TargetType = 8;

        /// <summary>
        /// 目标类型的实时记录顺序
        /// </summary>
        public const int Real_TargetType = 7;

        /// <summary>
        /// 备注的数字标识
        /// </summary>
        public const int Memo = 9;

        /// <summary>
        /// 备注的实时记录顺序
        /// </summary>
        public const int Real_Memo = 8;

        /// <summary>
        /// 冻结更新的数字标识
        /// </summary>
        public const int IsFreeze = 256;

        /// <summary>
        /// 冻结更新的实时记录顺序
        /// </summary>
        public const int Real_IsFreeze = 9;

        /// <summary>
        /// 数据状态的数字标识
        /// </summary>
        public const int DataState = 257;

        /// <summary>
        /// 数据状态的实时记录顺序
        /// </summary>
        public const int Real_DataState = 10;

        /// <summary>
        /// 制作时间的数字标识
        /// </summary>
        public const int AddDate = 267;

        /// <summary>
        /// 制作时间的实时记录顺序
        /// </summary>
        public const int Real_AddDate = 11;

        /// <summary>
        /// 制作人标识的数字标识
        /// </summary>
        public const int AuthorId = 268;

        /// <summary>
        /// 制作人标识的实时记录顺序
        /// </summary>
        public const int Real_AuthorId = 12;

        /// <summary>
        /// 制作人的数字标识
        /// </summary>
        public const int Author = 269;

        /// <summary>
        /// 制作人的实时记录顺序
        /// </summary>
        public const int Real_Author = 13;

        /// <summary>
        /// 最后修改日期的数字标识
        /// </summary>
        public const int LastModifyDate = 270;

        /// <summary>
        /// 最后修改日期的实时记录顺序
        /// </summary>
        public const int Real_LastModifyDate = 14;

        /// <summary>
        /// 最后修改者标识的数字标识
        /// </summary>
        public const int LastReviserId = 271;

        /// <summary>
        /// 最后修改者标识的实时记录顺序
        /// </summary>
        public const int Real_LastReviserId = 15;

        /// <summary>
        /// 最后修改者的数字标识
        /// </summary>
        public const int LastReviser = 272;

        /// <summary>
        /// 最后修改者的实时记录顺序
        /// </summary>
        public const int Real_LastReviser = 16;

        /// <summary>
        /// 实体结构
        /// </summary>
        public static readonly DataTableSturct Struct = new DataTableSturct
        {
            EntityName = EntityName,
            Caption = EntityCaption,
            Description = EntityDescription,
            PrimaryKey = EntityPrimaryKey,
            EntityType = EntityIdentity,
            IsIdentity = true,
            Properties = new Dictionary<int, PropertySturct>
                {
                    {
                        Real_Id,
                        new PropertySturct
                        {
                            Index        = Id,
                            Featrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                            Link         = "",
                            Name         = "Id",
                            Caption      = @"主键",
                            JsonName     = "Id",
                            ColumnName   = "id",
                            PropertyType = typeof(long),
                            CanNull      = false,
                            ValueType    = PropertyValueType.Value,
                            DbType       = 8,
                            CanImport    = false,
                            CanExport    = false,
                            Description  = @"主键"
                        }
                    },
                    {
                        Real_EventId,
                        new PropertySturct
                        {
                            Index        = EventId,
                            Featrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                            Link         = "",
                            Name         = "EventId",
                            Caption      = @"事件标识",
                            JsonName     = "EventId",
                            ColumnName   = "event_id",
                            PropertyType = typeof(long),
                            CanNull      = false,
                            ValueType    = PropertyValueType.Value,
                            DbType       = 8,
                            CanImport    = false,
                            CanExport    = false,
                            Description  = @"事件标识"
                        }
                    },
                    {
                        Real_Service,
                        new PropertySturct
                        {
                            Index        = Service,
                            Featrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                            Link         = "",
                            Name         = "Service",
                            Caption      = @"所属服务",
                            JsonName     = "Service",
                            ColumnName   = "service",
                            PropertyType = typeof(string),
                            CanNull      = false,
                            ValueType    = PropertyValueType.String,
                            DbType       = 15,
                            CanImport    = false,
                            CanExport    = false,
                            Description  = @"所属服务"
                        }
                    },
                    {
                        Real_IsLookUp,
                        new PropertySturct
                        {
                            Index        = IsLookUp,
                            Featrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                            Link         = "",
                            Name         = "IsLookUp",
                            Caption      = @"是否查阅服务",
                            JsonName     = "IsLookUp",
                            ColumnName   = "is_look_up",
                            PropertyType = typeof(bool),
                            CanNull      = false,
                            ValueType    = PropertyValueType.Value,
                            DbType       = 1,
                            CanImport    = false,
                            CanExport    = false,
                            Description  = @"如为查阅服务，则发送后不处理与等待结果"
                        }
                    },
                    {
                        Real_ApiName,
                        new PropertySturct
                        {
                            Index        = ApiName,
                            Featrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                            Link         = "",
                            Name         = "ApiName",
                            Caption      = @"接口名称",
                            JsonName     = "ApiName",
                            ColumnName   = "api_name",
                            PropertyType = typeof(string),
                            CanNull      = false,
                            ValueType    = PropertyValueType.String,
                            DbType       = 15,
                            CanImport    = false,
                            CanExport    = false,
                            Description  = @"接口名称"
                        }
                    },
                    {
                        Real_TargetDescription,
                        new PropertySturct
                        {
                            Index        = TargetDescription,
                            Featrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                            Link         = "",
                            Name         = "TargetDescription",
                            Caption      = @"目标说明",
                            JsonName     = "TargetDescription",
                            ColumnName   = "target_description",
                            PropertyType = typeof(string),
                            CanNull      = false,
                            ValueType    = PropertyValueType.String,
                            DbType       = 15,
                            CanImport    = false,
                            CanExport    = false,
                            Description  = @"目标说明"
                        }
                    },
                    {
                        Real_TargetName,
                        new PropertySturct
                        {
                            Index        = TargetName,
                            Featrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                            Link         = "",
                            Name         = "TargetName",
                            Caption      = @"目标名称",
                            JsonName     = "TargetName",
                            ColumnName   = "target_name",
                            PropertyType = typeof(string),
                            CanNull      = false,
                            ValueType    = PropertyValueType.String,
                            DbType       = 15,
                            CanImport    = false,
                            CanExport    = false,
                            Description  = @"*表示所有目标"
                        }
                    },
                    {
                        Real_TargetType,
                        new PropertySturct
                        {
                            Index        = TargetType,
                            Featrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                            Link         = "",
                            Name         = "TargetType",
                            Caption      = @"目标类型",
                            JsonName     = "TargetType",
                            ColumnName   = "target_type",
                            PropertyType = typeof(string),
                            CanNull      = false,
                            ValueType    = PropertyValueType.String,
                            DbType       = 15,
                            CanImport    = false,
                            CanExport    = false,
                            Description  = @"*表示所有类型"
                        }
                    },
                    {
                        Real_Memo,
                        new PropertySturct
                        {
                            Index        = Memo,
                            Featrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                            Link         = "",
                            Name         = "Memo",
                            Caption      = @"备注",
                            JsonName     = "Memo",
                            ColumnName   = "memo",
                            PropertyType = typeof(string),
                            CanNull      = false,
                            ValueType    = PropertyValueType.String,
                            DbType       = 752,
                            CanImport    = false,
                            CanExport    = false,
                            Description  = @"备注"
                        }
                    },
                    {
                        Real_IsFreeze,
                        new PropertySturct
                        {
                            Index        = IsFreeze,
                            Featrue      = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                            Link         = "DataIsFreeze",
                            Name         = "IsFreeze",
                            Caption      = @"冻结更新",
                            JsonName     = "isFreeze",
                            ColumnName   = "is_freeze",
                            PropertyType = typeof(bool),
                            CanNull      = false,
                            ValueType    = PropertyValueType.Value,
                            DbType       = 1,
                            CanImport    = false,
                            CanExport    = false,
                            Description  = @"无论在什么数据状态,一旦设置且保存后,数据将不再允许执行Update的操作,作为Update的统一开关.取消的方法是单独设置这个字段的值"
                        }
                    },
                    {
                        Real_DataState,
                        new PropertySturct
                        {
                            Index        = DataState,
                            Featrue      = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                            Link         = "DataState",
                            Name         = "DataState",
                            Caption      = @"数据状态",
                            JsonName     = "dataState",
                            ColumnName   = "data_state",
                            PropertyType = typeof(DataStateType),
                            CanNull      = false,
                            ValueType    = PropertyValueType.Value,
                            DbType       = 3,
                            CanImport    = false,
                            CanExport    = false,
                            Description  = @"数据状态"
                        }
                    },
                    {
                        Real_AddDate,
                        new PropertySturct
                        {
                            Index        = AddDate,
                            Featrue      = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                            Link         = "AddDate",
                            Name         = "AddDate",
                            Caption      = @"制作时间",
                            JsonName     = "addDate",
                            ColumnName   = "created_date",
                            PropertyType = typeof(DateTime),
                            CanNull      = false,
                            ValueType    = PropertyValueType.Value,
                            DbType       = 12,
                            CanImport    = false,
                            CanExport    = false,
                            Description  = @"制作时间"
                        }
                    },
                    {
                        Real_AuthorId,
                        new PropertySturct
                        {
                            Index        = AuthorId,
                            Featrue      = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                            Link         = "AddUserId",
                            Name         = "AuthorId",
                            Caption      = @"制作人标识",
                            JsonName     = "authorId",
                            ColumnName   = "created_user_id",
                            PropertyType = typeof(string),
                            CanNull      = false,
                            ValueType    = PropertyValueType.String,
                            DbType       = 15,
                            CanImport    = false,
                            CanExport    = false,
                            Description  = @"制作人标识"
                        }
                    },
                    {
                        Real_Author,
                        new PropertySturct
                        {
                            Index        = Author,
                            Featrue      = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                            Link         = "",
                            Name         = "Author",
                            Caption      = @"制作人",
                            JsonName     = "author",
                            ColumnName   = "created_user",
                            PropertyType = typeof(string),
                            CanNull      = false,
                            ValueType    = PropertyValueType.String,
                            DbType       = 15,
                            CanImport    = false,
                            CanExport    = false,
                            Description  = @"制作人"
                        }
                    },
                    {
                        Real_LastModifyDate,
                        new PropertySturct
                        {
                            Index        = LastModifyDate,
                            Featrue      = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                            Link         = "LastModifyDate",
                            Name         = "LastModifyDate",
                            Caption      = @"最后修改日期",
                            JsonName     = "lastModifyDate",
                            ColumnName   = "latest_updated_date",
                            PropertyType = typeof(DateTime),
                            CanNull      = false,
                            ValueType    = PropertyValueType.Value,
                            DbType       = 12,
                            CanImport    = false,
                            CanExport    = false,
                            Description  = @"最后修改日期"
                        }
                    },
                    {
                        Real_LastReviserId,
                        new PropertySturct
                        {
                            Index        = LastReviserId,
                            Featrue      = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                            Link         = "LastModifyUserId",
                            Name         = "LastReviserId",
                            Caption      = @"最后修改者标识",
                            JsonName     = "lastReviserId",
                            ColumnName   = "latest_updated_user_id",
                            PropertyType = typeof(string),
                            CanNull      = false,
                            ValueType    = PropertyValueType.String,
                            DbType       = 15,
                            CanImport    = false,
                            CanExport    = false,
                            Description  = @"最后修改者标识"
                        }
                    },
                    {
                        Real_LastReviser,
                        new PropertySturct
                        {
                            Index        = LastReviser,
                            Featrue      = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                            Link         = "",
                            Name         = "LastReviser",
                            Caption      = @"最后修改者",
                            JsonName     = "lastReviser",
                            ColumnName   = "latest_updated_user",
                            PropertyType = typeof(string),
                            CanNull      = false,
                            ValueType    = PropertyValueType.String,
                            DbType       = 15,
                            CanImport    = false,
                            CanExport    = false,
                            Description  = @"最后修改者"
                        }
                    }
                },
            ReadTableName = @"tb_event_subscribe",
            WriteTableName = @"tb_event_subscribe",
            FullLoadFields = @"
    `id` AS `Id`,
    `event_id` AS `EventId`,
    `service` AS `Service`,
    `is_look_up` AS `IsLookUp`,
    `api_name` AS `ApiName`,
    `target_description` AS `TargetDescription`,
    `target_name` AS `TargetName`,
    `target_type` AS `TargetType`,
    `memo` AS `Memo`,
    `is_freeze` AS `IsFreeze`,
    `data_state` AS `DataState`,
    `created_date` AS `AddDate`,
    `created_user_id` AS `AuthorId`,
    `created_user` AS `Author`,
    `latest_updated_date` AS `LastModifyDate`,
    `latest_updated_user_id` AS `LastReviserId`,
    `latest_updated_user` AS `LastReviser`",

            InsertSqlCode = $@"
INSERT INTO `tb_event_subscribe`
(
    `event_id`,
    `service`,
    `is_look_up`,
    `api_name`,
    `target_description`,
    `target_name`,
    `target_type`,
    `memo`,
    `is_freeze`,
    `data_state`,
    `created_date`,
    `created_user_id`,
    `created_user`,
    `latest_updated_date`,
    `latest_updated_user_id`,
    `latest_updated_user`
)
VALUES
(
    ?EventId,
    ?Service,
    ?IsLookUp,
    ?ApiName,
    ?TargetDescription,
    ?TargetName,
    ?TargetType,
    ?Memo,
    ?IsFreeze,
    ?DataState,
    ?AddDate,
    ?AuthorId,
    ?Author,
    ?LastModifyDate,
    ?LastReviserId,
    ?LastReviser
);
SELECT @@IDENTITY;",

            UpdateSqlCode = $@"
       `event_id` = ?EventId,
       `service` = ?Service,
       `is_look_up` = ?IsLookUp,
       `api_name` = ?ApiName,
       `target_description` = ?TargetDescription,
       `target_name` = ?TargetName,
       `target_type` = ?TargetType,
       `memo` = ?Memo"
        };
    }
}