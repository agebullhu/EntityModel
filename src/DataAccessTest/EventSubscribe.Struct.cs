#region
using System;
using System.Collections.Generic;
using Agebull.EntityModel.Common;


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
        /// 数据表名称
        /// </summary>
        public const string TableName = @"tb_event_subscribe";
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
        public static readonly EntitySturct Struct = new EntitySturct
        {
            EntityName = EntityName,
            Caption = EntityCaption,
            Description = EntityDescription,
            PrimaryKey = EntityPrimaryKey,
            EntityType = EntityIdentity,
            IsIdentity = true,
            ReadTableName = TableName,
            WriteTableName = TableName,
            Properties = new List<EntitiyProperty>
            {
                new EntitiyProperty
                {
                    Index        = Id,
                    PropertyIndex= Real_Id,
                    PropertyFeatrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                    DbReadWrite  = ReadWriteFeatrue.Read,
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
                },
                new EntitiyProperty
                {
                    Index        = EventId,
                    PropertyIndex= Real_EventId,
                    PropertyFeatrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                    DbReadWrite  = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
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
                },
                new EntitiyProperty
                {
                    Index        = Service,
                    PropertyIndex= Real_Service,
                    PropertyFeatrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                    DbReadWrite  = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
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
                },
                new EntitiyProperty
                {
                    Index        = IsLookUp,
                    PropertyIndex= Real_IsLookUp,
                    PropertyFeatrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                    DbReadWrite  = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
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
                },
                new EntitiyProperty
                {
                    Index        = ApiName,
                    PropertyIndex= Real_ApiName,
                    PropertyFeatrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                    DbReadWrite  = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
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
                },
                new EntitiyProperty
                {
                    Index        = TargetDescription,
                    PropertyIndex= Real_TargetDescription,
                    PropertyFeatrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                    DbReadWrite  = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
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
                },
                new EntitiyProperty
                {
                    Index        = TargetName,
                    PropertyIndex= Real_TargetName,
                    PropertyFeatrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                    DbReadWrite  = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
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
                },
                new EntitiyProperty
                {
                    Index        = TargetType,
                    PropertyIndex= Real_TargetType,
                    PropertyFeatrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                    DbReadWrite  = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
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
                },
                new EntitiyProperty
                {
                    Index        = Memo,
                    PropertyIndex= Real_Memo,
                    PropertyFeatrue      = PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                    DbReadWrite  = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
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
                },
                new EntitiyProperty
                {
                    Index        = IsFreeze,
                    PropertyIndex= Real_IsFreeze,
                    PropertyFeatrue      = PropertyFeatrue.DbCloumn,
                    DbReadWrite  = ReadWriteFeatrue.None,
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
                },
                new EntitiyProperty
                {
                    Index        = DataState,
                    PropertyIndex= Real_DataState,
                    PropertyFeatrue      = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                    DbReadWrite  = ReadWriteFeatrue.Read,
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
                },
                new EntitiyProperty
                {
                    Index        = AddDate,
                    PropertyIndex= Real_AddDate,
                    PropertyFeatrue      = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                    DbReadWrite  = ReadWriteFeatrue.Read| ReadWriteFeatrue.Insert,
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
                },
                new EntitiyProperty
                {
                    Index        = AuthorId,
                    PropertyIndex= Real_AuthorId,
                    PropertyFeatrue      = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                    DbReadWrite  = ReadWriteFeatrue.Read| ReadWriteFeatrue.Insert,
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
                },
                new EntitiyProperty
                {
                    Index        = Author,
                    PropertyIndex= Real_Author,
                    PropertyFeatrue      = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                    DbReadWrite  = ReadWriteFeatrue.Read,
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
                },
                new EntitiyProperty
                {
                    Index        = LastModifyDate,
                    PropertyIndex= Real_LastModifyDate,
                    PropertyFeatrue      = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                    DbReadWrite  = ReadWriteFeatrue.Read,
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
                },
                new EntitiyProperty
                {
                    Index        = LastReviserId,
                    PropertyIndex= Real_LastReviserId,
                    PropertyFeatrue      = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                    DbReadWrite  = ReadWriteFeatrue.Read,
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
                },
                new EntitiyProperty
                {
                    Index        = LastReviser,
                    PropertyIndex= Real_LastReviser,
                    PropertyFeatrue      = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.DbCloumn,
                    DbReadWrite  = ReadWriteFeatrue.Read,
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
        };
    }
}