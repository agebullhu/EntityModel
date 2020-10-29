/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/7 1:51:58*/
#region
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

using Agebull.EntityModel.Common;
using Agebull.EntityModel.Interfaces;
using Agebull.EntityModel.MySql;


#endregion
namespace Zeroteam.MessageMVC.EventBus.DataAccess
{
    /// <summary>
    /// 事件定义
    /// </summary>
    public sealed class EventDefaultEntityDataOperator : IDataOperator<EventDefaultEntity>, IEntityOperator<EventDefaultEntity>
    {
        #region 基本信息

        /// <summary>
        /// 驱动提供者信息
        /// </summary>
        public IDataAccessProvider<EventDefaultEntity> Provider { get; set; }

        /// <summary>
        /// 实体结构
        /// </summary>
        static EntityStruct Struct = new EntityStruct
        {
            IsIdentity = true,
            EntityName = EventBusDb.EventDefault_Struct_.EntityName,
            Caption = EventBusDb.EventDefault_Struct_.Caption,
            Description = EventBusDb.EventDefault_Struct_.Description,
            PrimaryProperty = EventBusDb.EventDefault_Struct_.PrimaryKey,
            ReadTableName = EventBusDb.EventDefault_Struct_.TableName,
            WriteTableName = EventBusDb.EventDefault_Struct_.TableName,
            InterfaceFeature = new HashSet<string> { nameof(GlobalDataInterfaces.IStateData), nameof(GlobalDataInterfaces.IHistoryData), nameof(GlobalDataInterfaces.IAuthorData) },
            Properties = new List<EntityProperty>
            {
                new EntityProperty(EventBusDb.EventDefault_Struct_.Id,0,"Id","tb_event_default","id",ReadWriteFeatrue.Read),
                new EntityProperty(GlobalDataInterfaces.IStateData.IsFreeze,1,"IsFreeze","tb_event_default","is_freeze",ReadWriteFeatrue.Read),
                new EntityProperty(GlobalDataInterfaces.IHistoryData.LastModifyDate,2,"LastModifyDate","tb_event_default","latest_updated_date",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert),
                new EntityProperty(GlobalDataInterfaces.IAuthorData.AuthorId,3,"AuthorId","tb_event_default","created_user_id",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert),
                new EntityProperty(EventBusDb.EventDefault_Struct_.EventName,4,"EventName","tb_event_default","event_name",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(GlobalDataInterfaces.IStateData.DataState,5,"DataState","tb_event_default","data_state",ReadWriteFeatrue.Read),
                new EntityProperty(GlobalDataInterfaces.IHistoryData.LastReviserId,6,"LastReviserId","tb_event_default","latest_updated_user_id",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert),
                new EntityProperty(GlobalDataInterfaces.IAuthorData.Author,7,"Author","tb_event_default","created_user",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert),
                new EntityProperty(EventBusDb.EventDefault_Struct_.EventCode,8,"EventCode","tb_event_default","event_code",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(GlobalDataInterfaces.IAuthorData.AddDate,9,"AddDate","tb_event_default","created_date",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert),
                new EntityProperty(EventBusDb.EventDefault_Struct_.Version,10,"Version","tb_event_default","version",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.Region,11,"Region","tb_event_default","region",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.EventType,12,"EventType","tb_event_default","event_type",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.ResultOption,13,"ResultOption","tb_event_default","result_option",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.SuccessOption,14,"SuccessOption","tb_event_default","success_option",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.App,15,"App","tb_event_default","app",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.Classify,16,"Classify","tb_event_default","classify",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.Tag,17,"Tag","tb_event_default","tag",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.Memo,18,"Memo","tb_event_default","memo",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.TargetType,19,"TargetType","tb_event_default","target_type",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.TargetName,20,"TargetName","tb_event_default","target_name",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventDefault_Struct_.TargetDescription,21,"TargetDescription","tb_event_default","target_description",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(GlobalDataInterfaces.IHistoryData.LastReviser,22,"LastReviser","tb_event_default","latest_updated_user",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert)
            }
        };

        /// <summary>
        /// 配置信息
        /// </summary>
        static DataTableOption TableOption = new DataTableOption
        {
            IsQuery = false,
            UpdateByMidified = true,
            SqlBuilder = new MySqlSqlBuilder<EventDefaultEntity>(),
            DataStruct = Struct,
            InjectionLevel = InjectionLevel.All,
            ReadTableName = FromSqlCode,
            WriteTableName = EventBusDb.EventDefault_Struct_.TableName,
            LoadFields = LoadFields,
            Having = Having,
            GroupFields = GroupFields,
            UpdateFields = UpdateFields,
            InsertSqlCode = InsertSqlCode,
        };

        static EventDefaultEntityDataOperator()
        {
            TableOption.Initiate();
        }

        /// <summary>
        /// 配置信息
        /// </summary>
        internal static DataAccessOption GetOption() => new DataAccessOption(TableOption);

        #endregion

        #region SQL

        /// <summary>
        /// 读取的字段
        /// </summary>
        public const string LoadFields = @"`tb_event_default`.`id` AS `id`
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
,`tb_event_default`.`is_freeze` AS `is_freeze`
,`tb_event_default`.`data_state` AS `data_state`
,`tb_event_default`.`latest_updated_date` AS `latest_updated_date`
,`tb_event_default`.`latest_updated_user_id` AS `latest_updated_user_id`
,`tb_event_default`.`latest_updated_user` AS `latest_updated_user`
,`tb_event_default`.`created_user_id` AS `created_user_id`
,`tb_event_default`.`created_user` AS `created_user`
,`tb_event_default`.`created_date` AS `created_date`
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
        public const string FromSqlCode = @"tb_event_default";

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
        public async Task LoadEntity(DbDataReader r, EventDefaultEntity entity)
        {
            var reader = r as MySqlDataReader;
            entity.Id = await reader.GetFieldValueAsync<long>(0);
            if (reader.IsDBNull(1))
                entity.EventName = null;
            else
                entity.EventName = await reader.GetFieldValueAsync<string>(1);
            if (reader.IsDBNull(2))
                entity.EventCode = null;
            else
                entity.EventCode = await reader.GetFieldValueAsync<string>(2);
            if (reader.IsDBNull(3))
                entity.Version = null;
            else
                entity.Version = await reader.GetFieldValueAsync<string>(3);
            entity.Region = (RegionType)(await reader.GetFieldValueAsync<int>(4));
            entity.EventType = (EventType)(await reader.GetFieldValueAsync<int>(5));
            entity.ResultOption = (ResultOptionType)(await reader.GetFieldValueAsync<int>(6));
            entity.SuccessOption = (SuccessOptionType)(await reader.GetFieldValueAsync<int>(7));
            if (reader.IsDBNull(8))
                entity.App = null;
            else
                entity.App = await reader.GetFieldValueAsync<string>(8);
            if (reader.IsDBNull(9))
                entity.Classify = null;
            else
                entity.Classify = await reader.GetFieldValueAsync<string>(9);
            if (reader.IsDBNull(10))
                entity.Tag = null;
            else
                entity.Tag = await reader.GetFieldValueAsync<string>(10);
            if (reader.IsDBNull(11))
                entity.Memo = null;
            else
                entity.Memo = await reader.GetFieldValueAsync<string>(11);
            if (reader.IsDBNull(12))
                entity.TargetType = null;
            else
                entity.TargetType = await reader.GetFieldValueAsync<string>(12);
            if (reader.IsDBNull(13))
                entity.TargetName = null;
            else
                entity.TargetName = await reader.GetFieldValueAsync<string>(13);
            if (reader.IsDBNull(14))
                entity.TargetDescription = null;
            else
                entity.TargetDescription = await reader.GetFieldValueAsync<string>(14);
            entity.IsFreeze = await reader.GetFieldValueAsync<bool>(15);
            entity.DataState = (DataStateType)(await reader.GetFieldValueAsync<int>(16));
            if (reader.IsDBNull(17))
                entity.LastModifyDate = default;
            else
                entity.LastModifyDate = await reader.GetFieldValueAsync<DateTime>(17);
            if (reader.IsDBNull(18))
                entity.LastReviserId = null;
            else
                entity.LastReviserId = await reader.GetFieldValueAsync<string>(18);
            if (reader.IsDBNull(19))
                entity.LastReviser = null;
            else
                entity.LastReviser = await reader.GetFieldValueAsync<string>(19);
            if (reader.IsDBNull(20))
                entity.AuthorId = null;
            else
                entity.AuthorId = await reader.GetFieldValueAsync<string>(20);
            if (reader.IsDBNull(21))
                entity.Author = null;
            else
                entity.Author = await reader.GetFieldValueAsync<string>(21);
            entity.AddDate = await reader.GetFieldValueAsync<DateTime>(22);
        }



        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        public void SetEntityParameter(EventDefaultEntity entity, MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter("Id", entity.Id));
            cmd.Parameters.Add(new MySqlParameter("IsFreeze", entity.IsFreeze ? (byte)1 : (byte)0));
            cmd.Parameters.Add(new MySqlParameter("LastModifyDate", entity.LastModifyDate));
            cmd.Parameters.Add(new MySqlParameter("AuthorId", entity.AuthorId));
            cmd.Parameters.Add(new MySqlParameter("EventName", entity.EventName));
            cmd.Parameters.Add(new MySqlParameter("DataState", (int)entity.DataState));
            cmd.Parameters.Add(new MySqlParameter("LastReviserId", entity.LastReviserId));
            cmd.Parameters.Add(new MySqlParameter("Author", entity.Author));
            cmd.Parameters.Add(new MySqlParameter("EventCode", entity.EventCode));
            cmd.Parameters.Add(new MySqlParameter("AddDate", entity.AddDate));
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
            cmd.Parameters.Add(new MySqlParameter("LastReviser", entity.LastReviser));
        }

        #endregion

        #region IEntityOperator


        /// <summary>
        ///     读取属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        object IEntityOperator<EventDefaultEntity>.GetValue(EventDefaultEntity entity, string property)
        {
            if (property == null) return null;
            return (property.Trim().ToLower()) switch
            {
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
        void IEntityOperator<EventDefaultEntity>.SetValue(EventDefaultEntity entity, string property, object value)
        {
            if (property == null)
                return;
            switch (property.Trim().ToLower())
            {
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