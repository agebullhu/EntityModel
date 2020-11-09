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
    /// 事件定义
    /// </summary>
    public sealed class EventDefaultEntityDataOperator : IDataOperator<EventDefaultEntity>, IEntityOperator<EventDefaultEntity>
    {
        #region 基本信息

        /// <summary>
        /// 单例
        /// </summary>
        public static EventDefaultEntityDataOperator Instance = new EventDefaultEntityDataOperator();

        /// <summary>
        /// 驱动提供者信息
        /// </summary>
        public IDataAccessProvider<EventDefaultEntity> Provider { get; set; }

        /// <summary>
        /// 实体结构
        /// </summary>
        readonly static EntityStruct Struct = new EntityStruct
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
                new EntityProperty(EventBusDb.EventDefault_Struct_.Id,0,"Id","tb_event_default_test","id",ReadWriteFeatrue.Read),
                new EntityProperty(EventBusDb.EventDefault_Struct_.EventName,4,"EventName","tb_event_default_test","event_name",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
            }
        };

        /// <summary>
        /// 配置信息
        /// </summary>
        readonly static DataTableOption TableOption;

        static EventDefaultEntityDataOperator()
        {
            TableOption = new DataTableOption
            {
                IsQuery = false,
                SqlBuilder = new MySqlSqlBuilder<EventDefaultEntity>(),
                UpdateByMidified = false,
                DataStruct = Struct,
                InjectionLevel = InjectionLevel.All,
                ReadTableName = FromSqlCode,
                WriteTableName = EventBusDb.EventDefault_Struct_.TableName,
                LoadFields = LoadFields,
                Having = Having,
                GroupFields = GroupFields,
                UpdateFields = UpdateFields,
                InsertSqlCode = InsertSqlCode
            };
            TableOption.Initiate();
        }


        /// <summary>
        /// 配置信息
        /// </summary>
        public static DataAccessOption GetOption() => new DataAccessOption(TableOption);

        #endregion

        #region SQL

        /// <summary>
        /// 读取的字段
        /// </summary>
        public const string LoadFields = @"`tb_event_default_test`.`id` AS `id`
,`tb_event_default_test`.`event_name` AS `event_name`";

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
        public const string FromSqlCode = @"tb_event_default_test";

        /// <summary>
        /// 更新的字段
        /// </summary>
        public const string UpdateFields = @"
       `event_name` = ?EventName";

        /// <summary>
        /// 写入的Sql
        /// </summary>
        public const string InsertSqlCode = @"
INSERT INTO `tb_event_default_test`
(
    `event_name`
)
VALUES
(
    ?EventName
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
            cmd.Parameters.Add(new MySqlParameter("EventName", entity.EventName));
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
                    entity.Id = (long)value;
                    return;
                case "eventname":
                    entity.EventName = (string)value;
                    return;
                case "eventcode":
                    entity.EventCode = value?.ToString();
                    return;
                case "version":
                    entity.Version = value?.ToString();
                    return;
                case "region":
                    if (value != null)
                    {
                        if (value is int @int)
                        {
                            entity.Region = (RegionType)@int;
                        }
                        else if (value is RegionType type)
                        {
                            entity.Region = type;
                        }
                        else
                        {
                            var str = value.ToString();
                            if (Enum.TryParse(str, out RegionType val))
                            {
                                entity.Region = val;
                            }
                            else
                            {
                                if (int.TryParse(str, out int vl))
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
                        if (value is int @int)
                        {
                            entity.EventType = (EventType)@int;
                        }
                        else if (value is EventType type)
                        {
                            entity.EventType = type;
                        }
                        else
                        {
                            var str = value.ToString();
                            if (Enum.TryParse(str, out EventType val))
                            {
                                entity.EventType = val;
                            }
                            else
                            {
                                if (int.TryParse(str, out int vl))
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
                        if (value is int @int)
                        {
                            entity.ResultOption = (ResultOptionType)@int;
                        }
                        else if (value is ResultOptionType type)
                        {
                            entity.ResultOption = type;
                        }
                        else
                        {
                            var str = value.ToString();
                            if (Enum.TryParse(str, out ResultOptionType val))
                            {
                                entity.ResultOption = val;
                            }
                            else
                            {
                                if (int.TryParse(str, out int vl))
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
                        if (value is int @int)
                        {
                            entity.SuccessOption = (SuccessOptionType)@int;
                        }
                        else if (value is SuccessOptionType type)
                        {
                            entity.SuccessOption = type;
                        }
                        else
                        {
                            var str = value.ToString();
                            if (Enum.TryParse(str, out SuccessOptionType val))
                            {
                                entity.SuccessOption = val;
                            }
                            else
                            {
                                if (int.TryParse(str, out int vl))
                                {
                                    entity.SuccessOption = (SuccessOptionType)vl;
                                }
                            }
                        }
                    }
                    return;
                case "app":
                    entity.App = value?.ToString();
                    return;
                case "classify":
                    entity.Classify = value?.ToString();
                    return;
                case "tag":
                    entity.Tag = value?.ToString();
                    return;
                case "memo":
                    entity.Memo = value?.ToString();
                    return;
                case "targettype":
                    entity.TargetType = value?.ToString();
                    return;
                case "targetname":
                    entity.TargetName = value?.ToString();
                    return;
                case "targetdescription":
                    entity.TargetDescription = value?.ToString();
                    return;
                case "isfreeze":
                    if (value != null)
                    {
                        if (int.TryParse(value.ToString(), out int vl))
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
                        if (value is int @int)
                        {
                            entity.DataState = (DataStateType)@int;
                        }
                        else if (value is DataStateType)
                        {
                            entity.DataState = (DataStateType)value;
                        }
                        else
                        {
                            var str = value.ToString();
                            if (Enum.TryParse(str, out DataStateType val))
                            {
                                entity.DataState = val;
                            }
                            else
                            {
                                if (int.TryParse(str, out int vl))
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
                    entity.LastReviserId = value?.ToString();
                    return;
                case "lastreviser":
                    entity.LastReviser = value?.ToString();
                    return;
                case "authorid":
                    entity.AuthorId = value?.ToString();
                    return;
                case "author":
                    entity.Author = value?.ToString();
                    return;
                case "adddate":
                    entity.AddDate = Convert.ToDateTime(value);
                    return;
            }
        }

        #endregion
    }
}