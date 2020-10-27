/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/7 1:51:58*/
#region
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

using Agebull.EntityModel.Common;
using Agebull.EntityModel.MySql;


#endregion
namespace Zeroteam.MessageMVC.EventBus.DataAccess
{
    /// <summary>
    /// 事件订阅
    /// </summary>
    public sealed class EventSubscribeEntityDataOperator : IDataOperator<EventSubscribeEntity>, IEntityOperator<EventSubscribeEntity>
    {
        #region 基本信息

        /// <summary>
        /// 驱动提供者信息
        /// </summary>
        public DataAccessProvider<EventSubscribeEntity> Provider { get; set; }

        static EntityStruct _struct;

        /// <summary>
        /// 实体结构
        /// </summary>
        public static EntityStruct Struct => _struct ??= new EntityStruct
        {
            IsIdentity = true,
            EntityName = EventBusDb.EventSubscribe_Struct_.EntityName,
            Caption = EventBusDb.EventSubscribe_Struct_.Caption,
            Description = EventBusDb.EventSubscribe_Struct_.Description,
            PrimaryProperty = EventBusDb.EventSubscribe_Struct_.PrimaryKey,
            ReadTableName = EventBusDb.EventSubscribe_Struct_.TableName,
            WriteTableName = EventBusDb.EventSubscribe_Struct_.TableName,
            InterfaceFeature = new HashSet<string> { nameof(GlobalDataInterfaces.IStateData), nameof(GlobalDataInterfaces.IHistoryData), nameof(GlobalDataInterfaces.IAuthorData) },
            Properties = new List<EntityProperty>
            {
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.Id,0,"Id","tb_event_subscribe","id",ReadWriteFeatrue.Read),
                new EntityProperty(GlobalDataInterfaces.IStateData.IsFreeze,1,"IsFreeze","tb_event_subscribe","is_freeze",ReadWriteFeatrue.Read),
                new EntityProperty(GlobalDataInterfaces.IHistoryData.LastModifyDate,2,"LastModifyDate","tb_event_subscribe","latest_updated_date",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert),
                new EntityProperty(GlobalDataInterfaces.IAuthorData.AuthorId,3,"AuthorId","tb_event_subscribe","created_user_id",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert),
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.EventId,4,"EventId","tb_event_subscribe","event_id",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(GlobalDataInterfaces.IStateData.DataState,5,"DataState","tb_event_subscribe","data_state",ReadWriteFeatrue.Read),
                new EntityProperty(GlobalDataInterfaces.IHistoryData.LastReviserId,6,"LastReviserId","tb_event_subscribe","latest_updated_user_id",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert),
                new EntityProperty(GlobalDataInterfaces.IAuthorData.Author,7,"Author","tb_event_subscribe","created_user",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert),
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.Service,8,"Service","tb_event_subscribe","service",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(GlobalDataInterfaces.IAuthorData.AddDate,9,"AddDate","tb_event_subscribe","created_date",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert),
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.IsLookUp,10,"IsLookUp","tb_event_subscribe","is_look_up",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.ApiName,11,"ApiName","tb_event_subscribe","api_name",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.Memo,12,"Memo","tb_event_subscribe","memo",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.TargetName,13,"TargetName","tb_event_subscribe","target_name",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.TargetType,14,"TargetType","tb_event_subscribe","target_type",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.TargetDescription,15,"TargetDescription","tb_event_subscribe","target_description",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(GlobalDataInterfaces.IHistoryData.LastReviser,16,"LastReviser","tb_event_subscribe","latest_updated_user",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert)
            }
        };

        /// <summary>
        /// 配置信息
        /// </summary>
        internal static DataAccessOption Option = new DataAccessOption
        {
            IsQuery = false,
            UpdateByMidified = false,
            DataStruct = Struct,
            BaseOption = new DynamicOption
            {
                InjectionLevel = InjectionLevel.All,
                ReadTableName = FromSqlCode,
                WriteTableName = EventBusDb.EventSubscribe_Struct_.TableName,
                LoadFields = LoadFields,
                Having = Having,
                GroupFields = GroupFields,
                UpdateFields = UpdateFields,
                InsertSqlCode = InsertSqlCode,
            }
        };

        #endregion

        #region SQL

        /// <summary>
        /// 读取的字段
        /// </summary>
        public const string LoadFields = @"`tb_event_subscribe`.`id` AS `id`
,`tb_event_subscribe`.`event_id` AS `event_id`
,`tb_event_subscribe`.`service` AS `service`
,`tb_event_subscribe`.`is_look_up` AS `is_look_up`
,`tb_event_subscribe`.`api_name` AS `api_name`
,`tb_event_subscribe`.`memo` AS `memo`
,`tb_event_subscribe`.`target_name` AS `target_name`
,`tb_event_subscribe`.`target_type` AS `target_type`
,`tb_event_subscribe`.`target_description` AS `target_description`
,`tb_event_subscribe`.`is_freeze` AS `is_freeze`
,`tb_event_subscribe`.`data_state` AS `data_state`
,`tb_event_subscribe`.`latest_updated_date` AS `latest_updated_date`
,`tb_event_subscribe`.`latest_updated_user_id` AS `latest_updated_user_id`
,`tb_event_subscribe`.`latest_updated_user` AS `latest_updated_user`
,`tb_event_subscribe`.`created_user_id` AS `created_user_id`
,`tb_event_subscribe`.`created_user` AS `created_user`
,`tb_event_subscribe`.`created_date` AS `created_date`
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
        public const string FromSqlCode = @"tb_event_subscribe";

        /// <summary>
        /// 更新的字段
        /// </summary>
        public const string UpdateFields = @"
       `event_id` = ?EventId,
       `service` = ?Service,
       `is_look_up` = ?IsLookUp,
       `api_name` = ?ApiName,
       `memo` = ?Memo,
       `target_name` = ?TargetName,
       `target_type` = ?TargetType,
       `target_description` = ?TargetDescription";

        /// <summary>
        /// 写入的Sql
        /// </summary>
        public const string InsertSqlCode = @"
INSERT INTO `tb_event_subscribe`
(
    `event_id`,
    `service`,
    `is_look_up`,
    `api_name`,
    `memo`,
    `target_name`,
    `target_type`,
    `target_description`,
    `latest_updated_date`,
    `latest_updated_user_id`,
    `latest_updated_user`,
    `created_user_id`,
    `created_user`,
    `created_date`
)
VALUES
(
    ?EventId,
    ?Service,
    ?IsLookUp,
    ?ApiName,
    ?Memo,
    ?TargetName,
    ?TargetType,
    ?TargetDescription,
    ?LastModifyDate,
    ?LastReviserId,
    ?LastReviser,
    ?AuthorId,
    ?Author,
    ?AddDate
);
SELECT @@IDENTITY;";

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
                case "id":
                    return (int)MySqlDbType.Int64;
                case "event_id":
                case "eventid":
                    return (int)MySqlDbType.Int64;
                case "service":
                    return (int)MySqlDbType.VarString;
                case "is_look_up":
                case "islookup":
                    return (int)MySqlDbType.Byte;
                case "api_name":
                case "apiname":
                    return (int)MySqlDbType.VarString;
                case "memo":
                    return (int)MySqlDbType.Text;
                case "target_name":
                case "targetname":
                    return (int)MySqlDbType.VarString;
                case "target_type":
                case "targettype":
                    return (int)MySqlDbType.VarString;
                case "target_description":
                case "targetdescription":
                    return (int)MySqlDbType.Text;
                case "is_freeze":
                case "isfreeze":
                    return (int)MySqlDbType.Byte;
                case "data_state":
                case "datastate":
                    return (int)MySqlDbType.Int32;
                case "latest_updated_date":
                case "lastmodifydate":
                    return (int)MySqlDbType.DateTime;
                case "latest_updated_user_id":
                case "lastreviserid":
                    return (int)MySqlDbType.VarString;
                case "latest_updated_user":
                case "lastreviser":
                    return (int)MySqlDbType.VarString;
                case "created_user_id":
                case "authorid":
                    return (int)MySqlDbType.VarString;
                case "created_user":
                case "author":
                    return (int)MySqlDbType.VarString;
                case "created_date":
                case "adddate":
                    return (int)MySqlDbType.DateTime;
                default:
                    return (int)MySqlDbType.VarChar;
            }
        }



        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="r">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public async Task LoadEntity(DbDataReader r, EventSubscribeEntity entity)
        {
            var reader = r as MySqlDataReader;
            entity.Id = await reader.GetFieldValueAsync<long>(0);
            if (reader.IsDBNull(1))
                entity.EventId = default;
            else
                entity.EventId = await reader.GetFieldValueAsync<long>(1);
            if (reader.IsDBNull(2))
                entity.Service = null;
            else
                entity.Service = await reader.GetFieldValueAsync<string>(2);
            if (reader.IsDBNull(3))
                entity.IsLookUp = default;
            else
                entity.IsLookUp = await reader.GetFieldValueAsync<bool>(3);
            if (reader.IsDBNull(4))
                entity.ApiName = null;
            else
                entity.ApiName = await reader.GetFieldValueAsync<string>(4);
            if (reader.IsDBNull(5))
                entity.Memo = null;
            else
                entity.Memo = await reader.GetFieldValueAsync<string>(5);
            if (reader.IsDBNull(6))
                entity.TargetName = null;
            else
                entity.TargetName = await reader.GetFieldValueAsync<string>(6);
            if (reader.IsDBNull(7))
                entity.TargetType = null;
            else
                entity.TargetType = await reader.GetFieldValueAsync<string>(7);
            if (reader.IsDBNull(8))
                entity.TargetDescription = null;
            else
                entity.TargetDescription = await reader.GetFieldValueAsync<string>(8);
            entity.IsFreeze = await reader.GetFieldValueAsync<bool>(9);
            entity.DataState = (DataStateType)(await reader.GetFieldValueAsync<int>(10));
            if (reader.IsDBNull(11))
                entity.LastModifyDate = default;
            else
                entity.LastModifyDate = await reader.GetFieldValueAsync<DateTime>(11);
            if (reader.IsDBNull(12))
                entity.LastReviserId = null;
            else
                entity.LastReviserId = await reader.GetFieldValueAsync<string>(12);
            if (reader.IsDBNull(13))
                entity.LastReviser = null;
            else
                entity.LastReviser = await reader.GetFieldValueAsync<string>(13);
            if (reader.IsDBNull(14))
                entity.AuthorId = null;
            else
                entity.AuthorId = await reader.GetFieldValueAsync<string>(14);
            if (reader.IsDBNull(15))
                entity.Author = null;
            else
                entity.Author = await reader.GetFieldValueAsync<string>(15);
            entity.AddDate = await reader.GetFieldValueAsync<DateTime>(16);
        }



        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        public void SetEntityParameter(EventSubscribeEntity entity, MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter("Id", entity.Id));
            cmd.Parameters.Add(new MySqlParameter("IsFreeze", entity.IsFreeze ? (byte)1 : (byte)0));
            cmd.Parameters.Add(new MySqlParameter("LastModifyDate", entity.LastModifyDate));
            cmd.Parameters.Add(new MySqlParameter("AuthorId", entity.AuthorId));
            cmd.Parameters.Add(new MySqlParameter("EventId", entity.EventId));
            cmd.Parameters.Add(new MySqlParameter("DataState", (int)entity.DataState));
            cmd.Parameters.Add(new MySqlParameter("LastReviserId", entity.LastReviserId));
            cmd.Parameters.Add(new MySqlParameter("Author", entity.Author));
            cmd.Parameters.Add(new MySqlParameter("Service", entity.Service));
            cmd.Parameters.Add(new MySqlParameter("AddDate", entity.AddDate));
            cmd.Parameters.Add(new MySqlParameter("IsLookUp", entity.IsLookUp ? (byte)1 : (byte)0));
            cmd.Parameters.Add(new MySqlParameter("ApiName", entity.ApiName));
            cmd.Parameters.Add(new MySqlParameter("Memo", entity.Memo));
            cmd.Parameters.Add(new MySqlParameter("TargetName", entity.TargetName));
            cmd.Parameters.Add(new MySqlParameter("TargetType", entity.TargetType));
            cmd.Parameters.Add(new MySqlParameter("TargetDescription", entity.TargetDescription));
            cmd.Parameters.Add(new MySqlParameter("LastReviser", entity.LastReviser));
        }

        #endregion

        #region IEntityOperator


        /// <summary>
        ///     读取属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        object IEntityOperator<EventSubscribeEntity>.GetValue(EventSubscribeEntity entity, string property)
        {
            if (property == null) return null;
            return (property.Trim().ToLower()) switch
            {
                "id" => entity.Id,
                "eventid" => entity.EventId,
                "service" => entity.Service,
                "islookup" => entity.IsLookUp,
                "apiname" => entity.ApiName,
                "memo" => entity.Memo,
                "targetname" => entity.TargetName,
                "targettype" => entity.TargetType,
                "targetdescription" => entity.TargetDescription,
                "isfreeze" => entity.IsFreeze,
                "datastate" => entity.DataState,
                "lastmodifydate" => entity.LastModifyDate,
                "lastreviserid" => entity.LastReviserId,
                "lastreviser" => entity.LastReviser,
                "authorid" => entity.AuthorId,
                "author" => entity.Author,
                "adddate" => entity.AddDate,
                _ => null
            };
        }


        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        void IEntityOperator<EventSubscribeEntity>.SetValue(EventSubscribeEntity entity, string property, object value)
        {
            if (property == null)
                return;
            switch (property.Trim().ToLower())
            {
                case "id":
                    entity.Id = (long)Convert.ToDecimal(value);
                    return;
                case "eventid":
                    entity.EventId = (long)Convert.ToDecimal(value);
                    return;
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
                case "memo":
                    entity.Memo = value == null ? null : value.ToString();
                    return;
                case "targetname":
                    entity.TargetName = value == null ? null : value.ToString();
                    return;
                case "targettype":
                    entity.TargetType = value == null ? null : value.ToString();
                    return;
                case "targetdescription":
                    entity.TargetDescription = value == null ? null : value.ToString();
                    return;
                case "isfreeze":
                    if (value != null)
                    {
                        int vl;
                        if (int.TryParse(value.ToString(), out vl))
                        {
                            entity.IsFreeze = vl != 0;
                        }
                        else
                        {
                            entity.IsFreeze = Convert.ToBoolean(value);
                        }
                    }
                    return;
                case "datastate":
                    if (value != null)
                    {
                        if (value is int)
                        {
                            entity.DataState = (DataStateType)(int)value;
                        }
                        else if (value is DataStateType)
                        {
                            entity.DataState = (DataStateType)value;
                        }
                        else
                        {
                            var str = value.ToString();
                            DataStateType val;
                            if (DataStateType.TryParse(str, out val))
                            {
                                entity.DataState = val;
                            }
                            else
                            {
                                int vl;
                                if (int.TryParse(str, out vl))
                                {
                                    entity.DataState = (DataStateType)vl;
                                }
                            }
                        }
                    }
                    return;
                case "lastmodifydate":
                    entity.LastModifyDate = Convert.ToDateTime(value);
                    return;
                case "lastreviserid":
                    entity.LastReviserId = value == null ? null : value.ToString();
                    return;
                case "lastreviser":
                    entity.LastReviser = value == null ? null : value.ToString();
                    return;
                case "authorid":
                    entity.AuthorId = value == null ? null : value.ToString();
                    return;
                case "author":
                    entity.Author = value == null ? null : value.ToString();
                    return;
                case "adddate":
                    entity.AddDate = Convert.ToDateTime(value);
                    return;
            }
        }

        #endregion
    }
}