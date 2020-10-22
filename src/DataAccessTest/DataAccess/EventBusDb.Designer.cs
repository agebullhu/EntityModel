/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/3 10:20:54*/
#region
using System;
using System.Collections.Generic;
using System.Data;
using Agebull.Common;
using Agebull.Common.Configuration;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.MySql;
#endregion

namespace Zeroteam.MessageMVC.EventBus.DataAccess
{
    partial class EventBusDb
    {
        
        #region EventDefault(事件定义)

        /// <summary>
        /// 实体EventDefault的数据结构
        /// </summary>
        public static class EventDefault_Struct_
        {

            static EntityStruct _struct;

            /// <summary>
            /// 实体结构
            /// </summary>
            public static EntityStruct Struct => _struct ??= new EntityStruct
            {
                EntityName      = EntityName,
                Caption         = Caption,
                Description     = Description,
                PrimaryProperty      = PrimaryKey,
                IsIdentity      = true,
                ReadTableName   = TableName,
                WriteTableName  = "tb_event_default",
                InterfaceFeature= new HashSet<string> {nameof(GlobalDataInterfaces.IStateData),nameof(GlobalDataInterfaces.IHistoryData),nameof(GlobalDataInterfaces.IAuthorData)},
                Properties      = new List<EntityProperty>
                {
                    new EntityProperty(Id,0,"Id","tb_event_default","id",ReadWriteFeatrue.Read),
                    new EntityProperty(EventName,1,"EventName","tb_event_default","event_name",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(EventCode,2,"EventCode","tb_event_default","event_code",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(Version,3,"Version","tb_event_default","version",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(Region,4,"Region","tb_event_default","region",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(EventType,5,"EventType","tb_event_default","event_type",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(ResultOption,6,"ResultOption","tb_event_default","result_option",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(SuccessOption,7,"SuccessOption","tb_event_default","success_option",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(App,8,"App","tb_event_default","app",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(Classify,9,"Classify","tb_event_default","classify",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(Tag,10,"Tag","tb_event_default","tag",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(Memo,11,"Memo","tb_event_default","memo",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(TargetType,12,"TargetType","tb_event_default","target_type",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(TargetName,13,"TargetName","tb_event_default","target_name",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(TargetDescription,14,"TargetDescription","tb_event_default","target_description",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update)
                }
            };

            #region 常量

            /// <summary>
            /// 实体名称
            /// </summary>
            public const string Name = "EventDefault";

            /// <summary>
            /// 实体名称
            /// </summary>
            public const string EntityName = "EventDefault";

            /// <summary>
            /// 实体标题
            /// </summary>
            public const string Caption = "事件定义";

            /// <summary>
            /// 实体说明
            /// </summary>
            public const string Description = @"事件定义";

            /// <summary>
            /// 数据表名称
            /// </summary>
            public const string TableName = "tb_event_default";

            /// <summary>
            /// 实体说明
            /// </summary>
            public const string PrimaryKey = "Id";
            #endregion
            #region 字段


            /// <summary>
            /// 主键
            /// </summary>
            public static PropertyDefault Id = new PropertyDefault
            {
                Name           = "Id",
                ValueType      = PropertyValueType.Value,
                CanNull        = false,
                PropertyType   = typeof(long),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.Int64,
                FieldName      = "id",
                TableName      = "tb_event_default",
                DbReadWrite    = ReadWriteFeatrue.Read,
                JsonName       = "id",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventDefault",
                Caption        = @"主键",
                Description    = @"主键"
            };

            /// <summary>
            /// 事件名称
            /// </summary>
            public static PropertyDefault EventName = new PropertyDefault
            {
                Name           = "EventName",
                ValueType      = PropertyValueType.String,
                CanNull        = false,
                PropertyType   = typeof(string),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName      = "event_name",
                TableName      = "tb_event_default",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "eventName",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventDefault",
                Caption        = @"事件名称",
                Description    = @"事件名称"
            };

            /// <summary>
            /// 事件编码
            /// </summary>
            public static PropertyDefault EventCode = new PropertyDefault
            {
                Name           = "EventCode",
                ValueType      = PropertyValueType.String,
                CanNull        = false,
                PropertyType   = typeof(string),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName      = "event_code",
                TableName      = "tb_event_default",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "eventCode",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventDefault",
                Caption        = @"事件编码",
                Description    = @"事件编码"
            };

            /// <summary>
            /// 版本号
            /// </summary>
            public static PropertyDefault Version = new PropertyDefault
            {
                Name           = "Version",
                ValueType      = PropertyValueType.String,
                CanNull        = false,
                PropertyType   = typeof(string),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName      = "version",
                TableName      = "tb_event_default",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "version",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventDefault",
                Caption        = @"版本号",
                Description    = @"版本号"
            };

            /// <summary>
            /// 领域范围
            /// </summary>
            public static PropertyDefault Region = new PropertyDefault
            {
                Name           = "Region",
                ValueType      = PropertyValueType.NumberEnum,
                CanNull        = false,
                PropertyType   = typeof(RegionType),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.Int32,
                FieldName      = "region",
                TableName      = "tb_event_default",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "region",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventDefault",
                Caption        = @"领域范围",
                Description    = @"领域范围"
            };

            /// <summary>
            /// 事件类型
            /// </summary>
            public static PropertyDefault EventType = new PropertyDefault
            {
                Name           = "EventType",
                ValueType      = PropertyValueType.NumberEnum,
                CanNull        = false,
                PropertyType   = typeof(EventType),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.Int32,
                FieldName      = "event_type",
                TableName      = "tb_event_default",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "eventType",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventDefault",
                Caption        = @"事件类型",
                Description    = @"事件类型"
            };

            /// <summary>
            /// 处理结果
            /// </summary>
            public static PropertyDefault ResultOption = new PropertyDefault
            {
                Name           = "ResultOption",
                ValueType      = PropertyValueType.NumberEnum,
                CanNull        = false,
                PropertyType   = typeof(ResultOptionType),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.Int32,
                FieldName      = "result_option",
                TableName      = "tb_event_default",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "resultOption",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventDefault",
                Caption        = @"处理结果",
                Description    = @"处理结果"
            };

            /// <summary>
            /// 成功判断
            /// </summary>
            public static PropertyDefault SuccessOption = new PropertyDefault
            {
                Name           = "SuccessOption",
                ValueType      = PropertyValueType.NumberEnum,
                CanNull        = false,
                PropertyType   = typeof(SuccessOptionType),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.Int32,
                FieldName      = "success_option",
                TableName      = "tb_event_default",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "successOption",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventDefault",
                Caption        = @"成功判断",
                Description    = @"成功判断"
            };

            /// <summary>
            /// 所属应用
            /// </summary>
            public static PropertyDefault App = new PropertyDefault
            {
                Name           = "App",
                ValueType      = PropertyValueType.String,
                CanNull        = false,
                PropertyType   = typeof(string),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName      = "app",
                TableName      = "tb_event_default",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "app",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventDefault",
                Caption        = @"所属应用",
                Description    = @"所属应用"
            };

            /// <summary>
            /// 事件分类
            /// </summary>
            public static PropertyDefault Classify = new PropertyDefault
            {
                Name           = "Classify",
                ValueType      = PropertyValueType.String,
                CanNull        = false,
                PropertyType   = typeof(string),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName      = "classify",
                TableName      = "tb_event_default",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "classify",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventDefault",
                Caption        = @"事件分类",
                Description    = @"事件分类"
            };

            /// <summary>
            /// 事件标签
            /// </summary>
            public static PropertyDefault Tag = new PropertyDefault
            {
                Name           = "Tag",
                ValueType      = PropertyValueType.String,
                CanNull        = false,
                PropertyType   = typeof(string),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName      = "tag",
                TableName      = "tb_event_default",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "tag",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventDefault",
                Caption        = @"事件标签",
                Description    = @"事件标签"
            };

            /// <summary>
            /// 事件备注
            /// </summary>
            public static PropertyDefault Memo = new PropertyDefault
            {
                Name           = "Memo",
                ValueType      = PropertyValueType.String,
                CanNull        = false,
                PropertyType   = typeof(string),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.Text,
                FieldName      = "memo",
                TableName      = "tb_event_default",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "memo",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventDefault",
                Caption        = @"事件备注",
                Description    = @"事件备注"
            };

            /// <summary>
            /// 目标类型
            /// </summary>
            public static PropertyDefault TargetType = new PropertyDefault
            {
                Name           = "TargetType",
                ValueType      = PropertyValueType.String,
                CanNull        = false,
                PropertyType   = typeof(string),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName      = "target_type",
                TableName      = "tb_event_default",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "targetType",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventDefault",
                Caption        = @"目标类型",
                Description    = @"*表示所有类型"
            };

            /// <summary>
            /// 目标名称
            /// </summary>
            public static PropertyDefault TargetName = new PropertyDefault
            {
                Name           = "TargetName",
                ValueType      = PropertyValueType.String,
                CanNull        = false,
                PropertyType   = typeof(string),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName      = "target_name",
                TableName      = "tb_event_default",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "targetName",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventDefault",
                Caption        = @"目标名称",
                Description    = @"*表示所有目标"
            };

            /// <summary>
            /// 目标说明
            /// </summary>
            public static PropertyDefault TargetDescription = new PropertyDefault
            {
                Name           = "TargetDescription",
                ValueType      = PropertyValueType.String,
                CanNull        = false,
                PropertyType   = typeof(string),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.Text,
                FieldName      = "target_description",
                TableName      = "tb_event_default",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "targetDescription",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventDefault",
                Caption        = @"目标说明",
                Description    = @"目标说明"
            };
            #endregion

        }
        #endregion

        #region EventSubscribe(事件订阅)

        /// <summary>
        /// 实体EventSubscribe的数据结构
        /// </summary>
        public static class EventSubscribe_Struct_
        {

            static EntityStruct _struct;

            /// <summary>
            /// 实体结构
            /// </summary>
            public static EntityStruct Struct => _struct ??= new EntityStruct
            {
                EntityName      = EntityName,
                Caption         = Caption,
                Description     = Description,
                PrimaryProperty      = PrimaryKey,
                IsIdentity      = true,
                ReadTableName   = TableName,
                WriteTableName  = "tb_event_subscribe",
                InterfaceFeature= new HashSet<string> { nameof(GlobalDataInterfaces.IStateData),nameof(GlobalDataInterfaces.IHistoryData),nameof(GlobalDataInterfaces.IAuthorData)},
                Properties      = new List<EntityProperty>
                {
                    new EntityProperty(Id,0,"Id","tb_event_subscribe","id",ReadWriteFeatrue.Read),
                    new EntityProperty(EventId,1,"EventId","tb_event_subscribe","event_id",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(Service,2,"Service","tb_event_subscribe","service",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(IsLookUp,3,"IsLookUp","tb_event_subscribe","is_look_up",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(ApiName,4,"ApiName","tb_event_subscribe","api_name",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(Memo,5,"Memo","tb_event_subscribe","memo",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(TargetName,6,"TargetName","tb_event_subscribe","target_name",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(TargetType,7,"TargetType","tb_event_subscribe","target_type",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                    new EntityProperty(TargetDescription,8,"TargetDescription","tb_event_subscribe","target_description",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update)
                }
            };

            #region 常量

            /// <summary>
            /// 实体名称
            /// </summary>
            public const string Name = "EventSubscribe";

            /// <summary>
            /// 实体名称
            /// </summary>
            public const string EntityName = "EventSubscribe";

            /// <summary>
            /// 实体标题
            /// </summary>
            public const string Caption = "事件订阅";

            /// <summary>
            /// 实体说明
            /// </summary>
            public const string Description = @"事件订阅";

            /// <summary>
            /// 数据表名称
            /// </summary>
            public const string TableName = "tb_event_subscribe";

            /// <summary>
            /// 实体说明
            /// </summary>
            public const string PrimaryKey = "Id";
            #endregion
            #region 字段


            /// <summary>
            /// 主键
            /// </summary>
            public static PropertyDefault Id = new PropertyDefault
            {
                Name           = "Id",
                ValueType      = PropertyValueType.Value,
                CanNull        = false,
                PropertyType   = typeof(long),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.Int64,
                FieldName      = "id",
                TableName      = "tb_event_subscribe",
                DbReadWrite    = ReadWriteFeatrue.Read,
                JsonName       = "id",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventSubscribe",
                Caption        = @"主键",
                Description    = @"主键"
            };

            /// <summary>
            /// 事件标识
            /// </summary>
            public static PropertyDefault EventId = new PropertyDefault
            {
                Name           = "EventId",
                ValueType      = PropertyValueType.Value,
                CanNull        = false,
                PropertyType   = typeof(long),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.Int64,
                FieldName      = "event_id",
                TableName      = "tb_event_subscribe",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "eventId",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventSubscribe",
                Caption        = @"事件标识",
                Description    = @"事件标识"
            };

            /// <summary>
            /// 所属服务
            /// </summary>
            public static PropertyDefault Service = new PropertyDefault
            {
                Name           = "Service",
                ValueType      = PropertyValueType.String,
                CanNull        = false,
                PropertyType   = typeof(string),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName      = "service",
                TableName      = "tb_event_subscribe",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "service",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventSubscribe",
                Caption        = @"所属服务",
                Description    = @"所属服务"
            };

            /// <summary>
            /// 是否查阅服务
            /// </summary>
            public static PropertyDefault IsLookUp = new PropertyDefault
            {
                Name           = "IsLookUp",
                ValueType      = PropertyValueType.Value,
                CanNull        = false,
                PropertyType   = typeof(bool),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.Byte,
                FieldName      = "is_look_up",
                TableName      = "tb_event_subscribe",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "isLookUp",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventSubscribe",
                Caption        = @"是否查阅服务",
                Description    = @"如为查阅服务，则发送后不处理与等待结果"
            };

            /// <summary>
            /// 接口名称
            /// </summary>
            public static PropertyDefault ApiName = new PropertyDefault
            {
                Name           = "ApiName",
                ValueType      = PropertyValueType.String,
                CanNull        = false,
                PropertyType   = typeof(string),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName      = "api_name",
                TableName      = "tb_event_subscribe",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "apiName",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventSubscribe",
                Caption        = @"接口名称",
                Description    = @"接口名称"
            };

            /// <summary>
            /// 订阅备注
            /// </summary>
            public static PropertyDefault Memo = new PropertyDefault
            {
                Name           = "Memo",
                ValueType      = PropertyValueType.String,
                CanNull        = false,
                PropertyType   = typeof(string),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.Text,
                FieldName      = "memo",
                TableName      = "tb_event_subscribe",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "memo",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventSubscribe",
                Caption        = @"订阅备注",
                Description    = @"订阅备注"
            };

            /// <summary>
            /// 目标名称
            /// </summary>
            public static PropertyDefault TargetName = new PropertyDefault
            {
                Name           = "TargetName",
                ValueType      = PropertyValueType.String,
                CanNull        = false,
                PropertyType   = typeof(string),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName      = "target_name",
                TableName      = "tb_event_subscribe",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "targetName",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventSubscribe",
                Caption        = @"目标名称",
                Description    = @"*表示所有目标"
            };

            /// <summary>
            /// 目标类型
            /// </summary>
            public static PropertyDefault TargetType = new PropertyDefault
            {
                Name           = "TargetType",
                ValueType      = PropertyValueType.String,
                CanNull        = false,
                PropertyType   = typeof(string),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName      = "target_type",
                TableName      = "tb_event_subscribe",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "targetType",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventSubscribe",
                Caption        = @"目标类型",
                Description    = @"*表示所有类型"
            };

            /// <summary>
            /// 目标说明
            /// </summary>
            public static PropertyDefault TargetDescription = new PropertyDefault
            {
                Name           = "TargetDescription",
                ValueType      = PropertyValueType.String,
                CanNull        = false,
                PropertyType   = typeof(string),
                PropertyFeatrue= PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType         = (int)MySqlConnector.MySqlDbType.Text,
                FieldName      = "target_description",
                TableName      = "tb_event_subscribe",
                DbReadWrite    = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName       = "targetDescription",
                CanImport      = false,
                CanExport      = false,
                Entity         = "EventSubscribe",
                Caption        = @"目标说明",
                Description    = @"目标说明"
            };
            #endregion

        }
        #endregion

    }

    partial class DataAccessProviderHelper
    {
        
        static DataAccessOption GetOption<TEntity>()
        {
            return typeof(TEntity).Name switch
            {
                nameof(EventDefaultEntity) => EventDefaultEntityDataOperator.GetOption(),
                nameof(EventSubscribeEntity) => EventSubscribeEntityDataOperator.Option,
                nameof(EventDefaultModel) => EventDefaultModelDataOperator.Option,
                nameof(EventSubscribeModel) => EventSubscribeModelDataOperator.Option,
                _ => null,
            };
        }

        static object GetDataOperator<TEntity>()
            where TEntity : class, new()
        {
            return typeof(TEntity).Name switch
            {
                nameof(EventDefaultEntity) => new EventDefaultEntityDataOperator(),
                nameof(EventSubscribeEntity) => new EventSubscribeEntityDataOperator(),
                nameof(EventDefaultModel) => new EventDefaultModelDataOperator(),
                nameof(EventSubscribeModel) => new EventSubscribeModelDataOperator(),
                _ => null,
            };
        }

        static object GetEntityOperator<TEntity>()
            where TEntity : class, new()
        {
            return typeof(TEntity).Name switch
            {
                nameof(EventDefaultEntity) => new EventDefaultEntityDataOperator(),
                nameof(EventSubscribeEntity) => new EventSubscribeEntityDataOperator(),
                nameof(EventDefaultModel) => new EventDefaultModelDataOperator(),
                nameof(EventSubscribeModel) => new EventSubscribeModelDataOperator(),
                _ => null,
            };
        }
    }
}