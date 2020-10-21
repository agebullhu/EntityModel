/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/3 10:55:24*/
#region
using Agebull.EntityModel.Common;
using Agebull.EntityModel.MySql;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;


#endregion
namespace Zeroteam.MessageMVC.EventBus.DataAccess
{
    /// <summary>
    /// 事件定义
    /// </summary>
    public sealed class EventDefaultModelDataOperator : IDataOperator<EventDefaultModel>, IEntityOperator<EventDefaultModel>
    {
        #region 基本信息

        /// <summary>
        /// 驱动提供者信息
        /// </summary>
        public DataAccessProvider<EventDefaultModel> Provider { get; set; }

        static EntityStruct _struct;

        /// <summary>
        /// 实体结构
        /// </summary>
        public static EntityStruct Struct => _struct ??= new EntityStruct
        {
            IsIdentity = true,
            EntityName = EventBusDb.EventDefault_Struct_.EntityName,
            Caption = EventBusDb.EventDefault_Struct_.Caption,
            Description = EventBusDb.EventDefault_Struct_.Description,
            PrimaryProperty = EventBusDb.EventDefault_Struct_.PrimaryKey,
            ReadTableName = EventBusDb.EventDefault_Struct_.TableName,
            WriteTableName = EventBusDb.EventDefault_Struct_.TableName,
            InterfaceFeature = new[] { nameof(GlobalDataInterfaces.IStateData), nameof(GlobalDataInterfaces.IHistoryData), nameof(GlobalDataInterfaces.IAuthorData) },
            Properties = new List<EntityProperty>
            {
                new EntityProperty(EventBusDb.EventDefault_Struct_.Id,0,"Id","tb_event_default","id",ReadWriteFeatrue.Read),
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.Service,1,"Service","tb_event_subscribe","service",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.IsLookUp,2,"IsLookUp","tb_event_subscribe","is_look_up",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.ApiName,3,"ApiName","tb_event_subscribe","api_name",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.Memo,4,"EventSubscribeMemo","tb_event_subscribe","event_subscribe_memo",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.TargetName,5,"EventSubscribeTargetName","tb_event_subscribe","event_subscribe_target_name",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.TargetType,6,"EventSubscribeTargetType","tb_event_subscribe","event_subscribe_target_type",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.EventName,7,"EventName","tb_event_default","event_name",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.EventCode,8,"EventCode","tb_event_default","event_code",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.Version,9,"Version","tb_event_default","version",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.Region,10,"Region","tb_event_default","region",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.EventType,11,"EventType","tb_event_default","event_type",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.ResultOption,12,"ResultOption","tb_event_default","result_option",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.SuccessOption,13,"SuccessOption","tb_event_default","success_option",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.App,14,"App","tb_event_default","app",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.Classify,15,"Classify","tb_event_default","classify",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.Tag,16,"Tag","tb_event_default","tag",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.Memo,17,"Memo","tb_event_default","memo",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.TargetType,18,"TargetType","tb_event_default","target_type",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.TargetName,19,"TargetName","tb_event_default","target_name",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.TargetDescription,20,"TargetDescription","tb_event_default","target_description",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update)
            }
        };

        /// <summary>
        /// 配置信息
        /// </summary>
        internal static DataAccessOption Option = new DataAccessOption
        {
            NoInjection = false,
            IsQuery = false,
            UpdateByMidified = true,
            ReadTableName = FromSqlCode,
            WriteTableName = EventBusDb.EventDefault_Struct_.TableName,
            LoadFields = LoadFields,
            Having = Having,
            GroupFields = GroupFields,
            UpdateFields = UpdateFields,
            InsertSqlCode = InsertSqlCode,
            DataStruct = Struct
        };

        #endregion

        #region SQL

        /// <summary>
        /// 读取的字段
        /// </summary>
        public const string LoadFields = @"`tb_event_subscribe`.`service` AS `service`
,`tb_event_subscribe`.`is_look_up` AS `is_look_up`
,`tb_event_subscribe`.`api_name` AS `api_name`
,`tb_event_subscribe`.`memo` AS `event_subscribe_memo`
,`tb_event_subscribe`.`target_name` AS `event_subscribe_target_name`
,`tb_event_subscribe`.`target_type` AS `event_subscribe_target_type`
,`tb_event_default`.`id` AS `id`
,`tb_event_default`.`event_name` AS `event_name`
,`tb_event_default`.`event_code` AS `event_code`
,`tb_event_default`.`version` AS `version`
,`tb_event_default`.`region` AS `region`
,`tb_event_default`.`event_type` AS `event_type`
,`tb_event_default`.`result_option` AS `result_option`
,`tb_event_default`.`success_option` AS `success_option`
,`tb_event_default`.`app` AS `app`
,`tb_event_default`.`classify` AS `classify`
,`tb_event_default`.`tag` AS `tag`
,`tb_event_default`.`memo` AS `memo`
,`tb_event_default`.`target_type` AS `target_type`
,`tb_event_default`.`target_name` AS `target_name`
,`tb_event_default`.`target_description` AS `target_description`
";

        /// <summary>
        /// 汇总条件
        /// </summary>
        public const string Having = null;

        /// <summary>
        /// 分组字段
        /// </summary>
        public const string GroupFields = null;

        /// <summary>
        /// 读取的字段
        /// </summary>
        public const string FromSqlCode = @"tb_event_default
LEFT JOIN`tb_event_subscribe` ON `tb_event_default`.`id` = `tb_event_subscribe`.`event_id` ";

        /// <summary>
        /// 更新的字段
        /// </summary>
        public const string UpdateFields = @"
       `event_name` = ?EventName,
       `event_code` = ?EventCode,
       `version` = ?Version,
       `region` = ?Region,
       `event_type` = ?EventType,
       `result_option` = ?ResultOption,
       `success_option` = ?SuccessOption,
       `app` = ?App,
       `classify` = ?Classify,
       `tag` = ?Tag,
       `memo` = ?Memo,
       `target_type` = ?TargetType,
       `target_name` = ?TargetName,
       `target_description` = ?TargetDescription";

        /// <summary>
        /// 写入的Sql
        /// </summary>
        public const string InsertSqlCode = @"
INSERT INTO `tb_event_default`
(
    `event_name`,
    `event_code`,
    `version`,
    `region`,
    `event_type`,
    `result_option`,
    `success_option`,
    `app`,
    `classify`,
    `tag`,
    `memo`,
    `target_type`,
    `target_name`,
    `target_description`
)
VALUES
(
    ?EventName,
    ?EventCode,
    ?Version,
    ?Region,
    ?EventType,
    ?ResultOption,
    ?SuccessOption,
    ?App,
    ?Classify,
    ?Tag,
    ?Memo,
    ?TargetType,
    ?TargetName,
    ?TargetDescription
);";

        #endregion

        #region IDataOperator

        /// <summary>
        /// 得到字段的DbType类型
        /// </summary>
        /// <param name="property">字段名称</param>
        /// <returns>参数</returns>
        public int GetDbType(string property)
        {
            if (property == null)
                return (int)MySqlDbType.VarChar;
            switch (property)
            {
                case "service":
                    return (int)MySqlDbType.VarString;
                case "is_look_up":
                case "islookup":
                    return (int)MySqlDbType.Byte;
                case "api_name":
                case "apiname":
                    return (int)MySqlDbType.VarString;
                case "event_subscribe_memo":
                case "eventsubscribememo":
                    return (int)MySqlDbType.Text;
                case "event_subscribe_target_name":
                case "eventsubscribetargetname":
                    return (int)MySqlDbType.VarString;
                case "event_subscribe_target_type":
                case "eventsubscribetargettype":
                    return (int)MySqlDbType.VarString;
                case "id":
                    return (int)MySqlDbType.Int64;
                case "event_name":
                case "eventname":
                    return (int)MySqlDbType.VarString;
                case "event_code":
                case "eventcode":
                    return (int)MySqlDbType.VarString;
                case "version":
                    return (int)MySqlDbType.VarString;
                case "region":
                    return (int)MySqlDbType.Int32;
                case "event_type":
                case "eventtype":
                    return (int)MySqlDbType.Int32;
                case "result_option":
                case "resultoption":
                    return (int)MySqlDbType.Int32;
                case "success_option":
                case "successoption":
                    return (int)MySqlDbType.Int32;
                case "app":
                    return (int)MySqlDbType.VarString;
                case "classify":
                    return (int)MySqlDbType.VarString;
                case "tag":
                    return (int)MySqlDbType.VarString;
                case "memo":
                    return (int)MySqlDbType.Text;
                case "target_type":
                case "targettype":
                    return (int)MySqlDbType.VarString;
                case "target_name":
                case "targetname":
                    return (int)MySqlDbType.VarString;
                case "target_description":
                case "targetdescription":
                    return (int)MySqlDbType.Text;
                default:
                    return (int)MySqlDbType.VarChar;
            }
        }



        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="r">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public async Task LoadEntity(DbDataReader r, EventDefaultModel entity)
        {
            var reader = r as MySqlDataReader;
            if (reader.IsDBNull(0))
                entity.Service = null;
            else
                entity.Service = await reader.GetFieldValueAsync<string>(0);
            if (reader.IsDBNull(1))
                entity.IsLookUp = default;
            else
                entity.IsLookUp = await reader.GetFieldValueAsync<bool>(1);
            if (reader.IsDBNull(2))
                entity.ApiName = null;
            else
                entity.ApiName = await reader.GetFieldValueAsync<string>(2);
            if (reader.IsDBNull(3))
                entity.EventSubscribeMemo = null;
            else
                entity.EventSubscribeMemo = await reader.GetFieldValueAsync<string>(3);
            if (reader.IsDBNull(4))
                entity.EventSubscribeTargetName = null;
            else
                entity.EventSubscribeTargetName = await reader.GetFieldValueAsync<string>(4);
            if (reader.IsDBNull(5))
                entity.EventSubscribeTargetType = null;
            else
                entity.EventSubscribeTargetType = await reader.GetFieldValueAsync<string>(5);
            entity.Id = await reader.GetFieldValueAsync<long>(6);
            if (reader.IsDBNull(7))
                entity.EventName = null;
            else
                entity.EventName = await reader.GetFieldValueAsync<string>(7);
            if (reader.IsDBNull(8))
                entity.EventCode = null;
            else
                entity.EventCode = await reader.GetFieldValueAsync<string>(8);
            if (reader.IsDBNull(9))
                entity.Version = null;
            else
                entity.Version = await reader.GetFieldValueAsync<string>(9);
            entity.Region = (RegionType)(await reader.GetFieldValueAsync<int>(10));
            entity.EventType = (EventType)(await reader.GetFieldValueAsync<int>(11));
            entity.ResultOption = (ResultOptionType)(await reader.GetFieldValueAsync<int>(12));
            entity.SuccessOption = (SuccessOptionType)(await reader.GetFieldValueAsync<int>(13));
            if (reader.IsDBNull(14))
                entity.App = null;
            else
                entity.App = await reader.GetFieldValueAsync<string>(14);
            if (reader.IsDBNull(15))
                entity.Classify = null;
            else
                entity.Classify = await reader.GetFieldValueAsync<string>(15);
            if (reader.IsDBNull(16))
                entity.Tag = null;
            else
                entity.Tag = await reader.GetFieldValueAsync<string>(16);
            if (reader.IsDBNull(17))
                entity.Memo = null;
            else
                entity.Memo = await reader.GetFieldValueAsync<string>(17);
            if (reader.IsDBNull(18))
                entity.TargetType = null;
            else
                entity.TargetType = await reader.GetFieldValueAsync<string>(18);
            if (reader.IsDBNull(19))
                entity.TargetName = null;
            else
                entity.TargetName = await reader.GetFieldValueAsync<string>(19);
            if (reader.IsDBNull(20))
                entity.TargetDescription = null;
            else
                entity.TargetDescription = await reader.GetFieldValueAsync<string>(20);
        }

        /// <summary>
        ///     实体保存完成后期处理(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="operatorType">操作类型</param>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        public async Task AfterSave(EventDefaultModel entity, DataOperatorType operatorType)
        {
            var accessEventSubscribe = Provider.ServiceProvider.CreateDataAccess<EventSubscribeEntity>();
            {
                var ch = new EventSubscribeEntity
                {
                    Service = entity.Service,
                    IsLookUp = entity.IsLookUp,
                    ApiName = entity.ApiName,
                    Memo = entity.EventSubscribeMemo,
                    TargetName = entity.EventSubscribeTargetName,
                    TargetType = entity.EventSubscribeTargetType
                };
                ch.EventId = entity.Id;
                var (hase, id) = await accessEventSubscribe.LoadValueAsync(p => p.Id, p => p.EventId == entity.Id);
                ch.Id = id;
                if (hase)
                    await accessEventSubscribe.UpdateAsync(ch);
                else
                    await accessEventSubscribe.InsertAsync(ch);
            }
        }



        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        public void SetEntityParameter(EventDefaultModel entity, MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter("Service", entity.Service));
            cmd.Parameters.Add(new MySqlParameter("IsLookUp", entity.IsLookUp ? (byte)1 : (byte)0));
            cmd.Parameters.Add(new MySqlParameter("ApiName", entity.ApiName));
            cmd.Parameters.Add(new MySqlParameter("EventSubscribeMemo", entity.EventSubscribeMemo));
            cmd.Parameters.Add(new MySqlParameter("EventSubscribeTargetName", entity.EventSubscribeTargetName));
            cmd.Parameters.Add(new MySqlParameter("EventSubscribeTargetType", entity.EventSubscribeTargetType));
            cmd.Parameters.Add(new MySqlParameter("Id", entity.Id));
            cmd.Parameters.Add(new MySqlParameter("EventName", entity.EventName));
            cmd.Parameters.Add(new MySqlParameter("EventCode", entity.EventCode));
            cmd.Parameters.Add(new MySqlParameter("Version", entity.Version));
            cmd.Parameters.Add(new MySqlParameter("Region", (int)entity.Region));
            cmd.Parameters.Add(new MySqlParameter("EventType", (int)entity.EventType));
            cmd.Parameters.Add(new MySqlParameter("ResultOption", (int)entity.ResultOption));
            cmd.Parameters.Add(new MySqlParameter("SuccessOption", (int)entity.SuccessOption));
            cmd.Parameters.Add(new MySqlParameter("App", entity.App));
            cmd.Parameters.Add(new MySqlParameter("Classify", entity.Classify));
            cmd.Parameters.Add(new MySqlParameter("Tag", entity.Tag));
            cmd.Parameters.Add(new MySqlParameter("Memo", entity.Memo));
            cmd.Parameters.Add(new MySqlParameter("TargetType", entity.TargetType));
            cmd.Parameters.Add(new MySqlParameter("TargetName", entity.TargetName));
            cmd.Parameters.Add(new MySqlParameter("TargetDescription", entity.TargetDescription));
        }

        #endregion

        #region IEntityOperator


        /// <summary>
        ///     读取属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        object IEntityOperator<EventDefaultModel>.GetValue(EventDefaultModel entity, string property)
        {
            if (property == null) return null;
            return (property.Trim().ToLower()) switch
            {
                "service" => entity.Service,
                "islookup" => entity.IsLookUp,
                "apiname" => entity.ApiName,
                "eventsubscribememo" => entity.EventSubscribeMemo,
                "eventsubscribetargetname" => entity.EventSubscribeTargetName,
                "eventsubscribetargettype" => entity.EventSubscribeTargetType,
                "id" => entity.Id,
                "eventname" => entity.EventName,
                "eventcode" => entity.EventCode,
                "version" => entity.Version,
                "region" => entity.Region,
                "eventtype" => entity.EventType,
                "resultoption" => entity.ResultOption,
                "successoption" => entity.SuccessOption,
                "app" => entity.App,
                "classify" => entity.Classify,
                "tag" => entity.Tag,
                "memo" => entity.Memo,
                "targettype" => entity.TargetType,
                "targetname" => entity.TargetName,
                "targetdescription" => entity.TargetDescription,
                _ => null
            };
        }


        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        void IEntityOperator<EventDefaultModel>.SetValue(EventDefaultModel entity, string property, object value)
        {
            if (property == null)
                return;
            switch (property.Trim().ToLower())
            {
                case "service":
                    entity.Service = value == null ? null : value.ToString();
                    return;
                case "islookup":
                    if (value != null)
                    {
                        int vl;
                        if (int.TryParse(value.ToString(), out vl))
                        {
                            entity.IsLookUp = vl != 0;
                        }
                        else
                        {
                            entity.IsLookUp = Convert.ToBoolean(value);
                        }
                    }
                    return;
                case "apiname":
                    entity.ApiName = value == null ? null : value.ToString();
                    return;
                case "eventsubscribememo":
                    entity.EventSubscribeMemo = value == null ? null : value.ToString();
                    return;
                case "eventsubscribetargetname":
                    entity.EventSubscribeTargetName = value == null ? null : value.ToString();
                    return;
                case "eventsubscribetargettype":
                    entity.EventSubscribeTargetType = value == null ? null : value.ToString();
                    return;
                case "id":
                    entity.Id = (long)Convert.ToDecimal(value);
                    return;
                case "eventname":
                    entity.EventName = value == null ? null : value.ToString();
                    return;
                case "eventcode":
                    entity.EventCode = value == null ? null : value.ToString();
                    return;
                case "version":
                    entity.Version = value == null ? null : value.ToString();
                    return;
                case "region":
                    if (value != null)
                    {
                        if (value is int)
                        {
                            entity.Region = (RegionType)(int)value;
                        }
                        else if (value is RegionType)
                        {
                            entity.Region = (RegionType)value;
                        }
                        else
                        {
                            var str = value.ToString();
                            RegionType val;
                            if (RegionType.TryParse(str, out val))
                            {
                                entity.Region = val;
                            }
                            else
                            {
                                int vl;
                                if (int.TryParse(str, out vl))
                                {
                                    entity.Region = (RegionType)vl;
                                }
                            }
                        }
                    }
                    return;
                case "eventtype":
                    if (value != null)
                    {
                        if (value is int)
                        {
                            entity.EventType = (EventType)(int)value;
                        }
                        else if (value is EventType)
                        {
                            entity.EventType = (EventType)value;
                        }
                        else
                        {
                            var str = value.ToString();
                            EventType val;
                            if (EventType.TryParse(str, out val))
                            {
                                entity.EventType = val;
                            }
                            else
                            {
                                int vl;
                                if (int.TryParse(str, out vl))
                                {
                                    entity.EventType = (EventType)vl;
                                }
                            }
                        }
                    }
                    return;
                case "resultoption":
                    if (value != null)
                    {
                        if (value is int)
                        {
                            entity.ResultOption = (ResultOptionType)(int)value;
                        }
                        else if (value is ResultOptionType)
                        {
                            entity.ResultOption = (ResultOptionType)value;
                        }
                        else
                        {
                            var str = value.ToString();
                            ResultOptionType val;
                            if (ResultOptionType.TryParse(str, out val))
                            {
                                entity.ResultOption = val;
                            }
                            else
                            {
                                int vl;
                                if (int.TryParse(str, out vl))
                                {
                                    entity.ResultOption = (ResultOptionType)vl;
                                }
                            }
                        }
                    }
                    return;
                case "successoption":
                    if (value != null)
                    {
                        if (value is int)
                        {
                            entity.SuccessOption = (SuccessOptionType)(int)value;
                        }
                        else if (value is SuccessOptionType)
                        {
                            entity.SuccessOption = (SuccessOptionType)value;
                        }
                        else
                        {
                            var str = value.ToString();
                            SuccessOptionType val;
                            if (SuccessOptionType.TryParse(str, out val))
                            {
                                entity.SuccessOption = val;
                            }
                            else
                            {
                                int vl;
                                if (int.TryParse(str, out vl))
                                {
                                    entity.SuccessOption = (SuccessOptionType)vl;
                                }
                            }
                        }
                    }
                    return;
                case "app":
                    entity.App = value == null ? null : value.ToString();
                    return;
                case "classify":
                    entity.Classify = value == null ? null : value.ToString();
                    return;
                case "tag":
                    entity.Tag = value == null ? null : value.ToString();
                    return;
                case "memo":
                    entity.Memo = value == null ? null : value.ToString();
                    return;
                case "targettype":
                    entity.TargetType = value == null ? null : value.ToString();
                    return;
                case "targetname":
                    entity.TargetName = value == null ? null : value.ToString();
                    return;
                case "targetdescription":
                    entity.TargetDescription = value == null ? null : value.ToString();
                    return;
            }
        }

        #endregion
    }
}