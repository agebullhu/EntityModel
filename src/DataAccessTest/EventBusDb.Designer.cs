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
                EntityName = EntityName,
                Caption = Caption,
                Description = Description,
                PrimaryKey = PrimaryKey,
                IsIdentity = true,
                ReadTableName = TableName,
                WriteTableName = TableName,
                Properties = new List<EntityProperty>
                {
                    new EntityProperty(Id,1),
                    new EntityProperty(EventName,2),
                    new EntityProperty(EventCode,3),
                    new EntityProperty(Version,4),
                    new EntityProperty(Region,5),
                    new EntityProperty(EventType,6),
                    new EntityProperty(ResultOption,7),
                    new EntityProperty(SuccessOption,8),
                    new EntityProperty(App,9),
                    new EntityProperty(Classify,10),
                    new EntityProperty(Tag,11),
                    new EntityProperty(Memo,12),
                    new EntityProperty(TargetType,13),
                    new EntityProperty(TargetName,14),
                    new EntityProperty(TargetDescription,15)
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


            /// <summary>
            /// 主键
            /// </summary>
            public static PropertyDefault Id = new PropertyDefault
            {
                Name = "Id",
                ValueType = PropertyValueType.Value,
                CanNull = false,
                PropertyType = typeof(long),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.Int64,
                FieldName = "id",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "Id",
                CanImport = false,
                CanExport = false,
                Entity = "EventDefault",
                Caption = @"主键",
                Description = @"主键"
            };

            /// <summary>
            /// 事件名称
            /// </summary>
            public static PropertyDefault EventName = new PropertyDefault
            {
                Name = "EventName",
                ValueType = PropertyValueType.String,
                CanNull = false,
                PropertyType = typeof(string),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName = "event_name",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "EventName",
                CanImport = false,
                CanExport = false,
                Entity = "EventDefault",
                Caption = @"事件名称",
                Description = @"事件名称"
            };

            /// <summary>
            /// 事件编码
            /// </summary>
            public static PropertyDefault EventCode = new PropertyDefault
            {
                Name = "EventCode",
                ValueType = PropertyValueType.String,
                CanNull = false,
                PropertyType = typeof(string),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName = "event_code",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "EventCode",
                CanImport = false,
                CanExport = false,
                Entity = "EventDefault",
                Caption = @"事件编码",
                Description = @"事件编码"
            };

            /// <summary>
            /// 版本号
            /// </summary>
            public static PropertyDefault Version = new PropertyDefault
            {
                Name = "Version",
                ValueType = PropertyValueType.String,
                CanNull = false,
                PropertyType = typeof(string),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName = "version",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "Version",
                CanImport = false,
                CanExport = false,
                Entity = "EventDefault",
                Caption = @"版本号",
                Description = @"版本号"
            };

            /// <summary>
            /// 领域范围
            /// </summary>
            public static PropertyDefault Region = new PropertyDefault
            {
                Name = "Region",
                ValueType = PropertyValueType.NumberEnum,
                CanNull = false,
                PropertyType = typeof(RegionType),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.Int32,
                FieldName = "region",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "Region",
                CanImport = false,
                CanExport = false,
                Entity = "EventDefault",
                Caption = @"领域范围",
                Description = @"领域范围"
            };

            /// <summary>
            /// 事件类型
            /// </summary>
            public static PropertyDefault EventType = new PropertyDefault
            {
                Name = "EventType",
                ValueType = PropertyValueType.NumberEnum,
                CanNull = false,
                PropertyType = typeof(EventType),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.Int32,
                FieldName = "event_type",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "EventType",
                CanImport = false,
                CanExport = false,
                Entity = "EventDefault",
                Caption = @"事件类型",
                Description = @"事件类型"
            };

            /// <summary>
            /// 处理结果
            /// </summary>
            public static PropertyDefault ResultOption = new PropertyDefault
            {
                Name = "ResultOption",
                ValueType = PropertyValueType.NumberEnum,
                CanNull = false,
                PropertyType = typeof(ResultOptionType),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.Int32,
                FieldName = "result_option",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "ResultOption",
                CanImport = false,
                CanExport = false,
                Entity = "EventDefault",
                Caption = @"处理结果",
                Description = @"处理结果"
            };

            /// <summary>
            /// 成功判断
            /// </summary>
            public static PropertyDefault SuccessOption = new PropertyDefault
            {
                Name = "SuccessOption",
                ValueType = PropertyValueType.NumberEnum,
                CanNull = false,
                PropertyType = typeof(SuccessOptionType),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.Int64,
                FieldName = "success_option",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "SuccessOption",
                CanImport = false,
                CanExport = false,
                Entity = "EventDefault",
                Caption = @"成功判断",
                Description = @"成功判断"
            };

            /// <summary>
            /// 所属应用
            /// </summary>
            public static PropertyDefault App = new PropertyDefault
            {
                Name = "App",
                ValueType = PropertyValueType.String,
                CanNull = false,
                PropertyType = typeof(string),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName = "app",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "App",
                CanImport = false,
                CanExport = false,
                Entity = "EventDefault",
                Caption = @"所属应用",
                Description = @"所属应用"
            };

            /// <summary>
            /// 事件分类
            /// </summary>
            public static PropertyDefault Classify = new PropertyDefault
            {
                Name = "Classify",
                ValueType = PropertyValueType.String,
                CanNull = false,
                PropertyType = typeof(string),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName = "classify",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "Classify",
                CanImport = false,
                CanExport = false,
                Entity = "EventDefault",
                Caption = @"事件分类",
                Description = @"事件分类"
            };

            /// <summary>
            /// 事件标签
            /// </summary>
            public static PropertyDefault Tag = new PropertyDefault
            {
                Name = "Tag",
                ValueType = PropertyValueType.String,
                CanNull = false,
                PropertyType = typeof(string),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName = "tag",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "Tag",
                CanImport = false,
                CanExport = false,
                Entity = "EventDefault",
                Caption = @"事件标签",
                Description = @"事件标签"
            };

            /// <summary>
            /// 事件备注
            /// </summary>
            public static PropertyDefault Memo = new PropertyDefault
            {
                Name = "Memo",
                ValueType = PropertyValueType.String,
                CanNull = false,
                PropertyType = typeof(string),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.Text,
                FieldName = "memo",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "Memo",
                CanImport = false,
                CanExport = false,
                Entity = "EventDefault",
                Caption = @"事件备注",
                Description = @"事件备注"
            };

            /// <summary>
            /// 目标类型
            /// </summary>
            public static PropertyDefault TargetType = new PropertyDefault
            {
                Name = "TargetType",
                ValueType = PropertyValueType.String,
                CanNull = false,
                PropertyType = typeof(string),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName = "target_type",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "TargetType",
                CanImport = false,
                CanExport = false,
                Entity = "EventDefault",
                Caption = @"目标类型",
                Description = @"*表示所有类型"
            };

            /// <summary>
            /// 目标名称
            /// </summary>
            public static PropertyDefault TargetName = new PropertyDefault
            {
                Name = "TargetName",
                ValueType = PropertyValueType.String,
                CanNull = false,
                PropertyType = typeof(string),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName = "target_name",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "TargetName",
                CanImport = false,
                CanExport = false,
                Entity = "EventDefault",
                Caption = @"目标名称",
                Description = @"*表示所有目标"
            };

            /// <summary>
            /// 目标说明
            /// </summary>
            public static PropertyDefault TargetDescription = new PropertyDefault
            {
                Name = "TargetDescription",
                ValueType = PropertyValueType.String,
                CanNull = false,
                PropertyType = typeof(string),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.Text,
                FieldName = "target_description",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "TargetDescription",
                CanImport = false,
                CanExport = false,
                Entity = "EventDefault",
                Caption = @"目标说明",
                Description = @"目标说明"
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
                EntityName = EntityName,
                Caption = Caption,
                Description = Description,
                PrimaryKey = PrimaryKey,
                IsIdentity = true,
                ReadTableName = TableName,
                WriteTableName = TableName,
                Properties = new List<EntityProperty>
                {
                    new EntityProperty(Id,1),
                    new EntityProperty(EventId,2),
                    new EntityProperty(Service,3),
                    new EntityProperty(IsLookUp,4),
                    new EntityProperty(ApiName,5),
                    new EntityProperty(Memo,6),
                    new EntityProperty(TargetName,7),
                    new EntityProperty(TargetType,8),
                    new EntityProperty(TargetDescription,9)
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


            /// <summary>
            /// 主键
            /// </summary>
            public static PropertyDefault Id = new PropertyDefault
            {
                Name = "Id",
                ValueType = PropertyValueType.Value,
                CanNull = false,
                PropertyType = typeof(long),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.Int64,
                FieldName = "id",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "id",
                CanImport = false,
                CanExport = false,
                Entity = "EventSubscribe",
                Caption = @"主键",
                Description = @"主键"
            };

            /// <summary>
            /// 事件标识
            /// </summary>
            public static PropertyDefault EventId = new PropertyDefault
            {
                Name = "EventId",
                ValueType = PropertyValueType.Value,
                CanNull = false,
                PropertyType = typeof(long),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.Int64,
                FieldName = "event_id",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "eventId",
                CanImport = false,
                CanExport = false,
                Entity = "EventSubscribe",
                Caption = @"事件标识",
                Description = @"事件标识"
            };

            /// <summary>
            /// 所属服务
            /// </summary>
            public static PropertyDefault Service = new PropertyDefault
            {
                Name = "Service",
                ValueType = PropertyValueType.String,
                CanNull = false,
                PropertyType = typeof(string),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName = "service",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "service",
                CanImport = false,
                CanExport = false,
                Entity = "EventSubscribe",
                Caption = @"所属服务",
                Description = @"所属服务"
            };

            /// <summary>
            /// 是否查阅服务
            /// </summary>
            public static PropertyDefault IsLookUp = new PropertyDefault
            {
                Name = "IsLookUp",
                ValueType = PropertyValueType.Value,
                CanNull = false,
                PropertyType = typeof(bool),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.Byte,
                FieldName = "is_look_up",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "isLookUp",
                CanImport = false,
                CanExport = false,
                Entity = "EventSubscribe",
                Caption = @"是否查阅服务",
                Description = @"如为查阅服务，则发送后不处理与等待结果"
            };

            /// <summary>
            /// 接口名称
            /// </summary>
            public static PropertyDefault ApiName = new PropertyDefault
            {
                Name = "ApiName",
                ValueType = PropertyValueType.String,
                CanNull = false,
                PropertyType = typeof(string),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName = "api_name",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "apiName",
                CanImport = false,
                CanExport = false,
                Entity = "EventSubscribe",
                Caption = @"接口名称",
                Description = @"接口名称"
            };

            /// <summary>
            /// 订阅备注
            /// </summary>
            public static PropertyDefault Memo = new PropertyDefault
            {
                Name = "Memo",
                ValueType = PropertyValueType.String,
                CanNull = false,
                PropertyType = typeof(string),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.Text,
                FieldName = "memo",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "memo",
                CanImport = false,
                CanExport = false,
                Entity = "EventSubscribe",
                Caption = @"订阅备注",
                Description = @"订阅备注"
            };

            /// <summary>
            /// 目标名称
            /// </summary>
            public static PropertyDefault TargetName = new PropertyDefault
            {
                Name = "TargetName",
                ValueType = PropertyValueType.String,
                CanNull = false,
                PropertyType = typeof(string),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName = "target_name",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "targetName",
                CanImport = false,
                CanExport = false,
                Entity = "EventSubscribe",
                Caption = @"目标名称",
                Description = @"*表示所有目标"
            };

            /// <summary>
            /// 目标类型
            /// </summary>
            public static PropertyDefault TargetType = new PropertyDefault
            {
                Name = "TargetType",
                ValueType = PropertyValueType.String,
                CanNull = false,
                PropertyType = typeof(string),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.VarString,
                FieldName = "target_type",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "targetType",
                CanImport = false,
                CanExport = false,
                Entity = "EventSubscribe",
                Caption = @"目标类型",
                Description = @"*表示所有类型"
            };

            /// <summary>
            /// 目标说明
            /// </summary>
            public static PropertyDefault TargetDescription = new PropertyDefault
            {
                Name = "TargetDescription",
                ValueType = PropertyValueType.String,
                CanNull = false,
                PropertyType = typeof(string),
                PropertyFeatrue = PropertyFeatrue.Property | PropertyFeatrue.Field | PropertyFeatrue.Property,
                DbType = (int)MySqlConnector.MySqlDbType.Text,
                FieldName = "target_description",
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update,
                JsonName = "targetDescription",
                CanImport = false,
                CanExport = false,
                Entity = "EventSubscribe",
                Caption = @"目标说明",
                Description = @"目标说明"
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
                nameof(EventSubscribeData) => EventSubscribeDataOperator.Option,
                _ => null,
            };
        }

        static object GetDataOperator<TEntity>()
            where TEntity : class, new()
        {
            return typeof(TEntity).Name switch
            {
                nameof(EventSubscribeData) => new EventSubscribeDataOperator(),
                _ => null,
            };
        }

        static object GetEntityOperator<TEntity>()
            where TEntity : class, new()
        {
            return typeof(TEntity).Name switch
            {
                nameof(EventSubscribeData) => new EventSubscribeDataOperator(),
                _ => null,
            };
        }
    }
}