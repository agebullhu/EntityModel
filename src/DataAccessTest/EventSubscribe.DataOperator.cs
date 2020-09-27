#region

using Agebull.EntityModel.Common;
using Agebull.EntityModel.Events;
using MySqlConnector;
using System;
using System.Data.Common;
using System.Threading.Tasks;

#endregion

namespace Zeroteam.MessageMVC.EventBus.DataAccess
{
    /// <summary>
    /// 事件订阅
    /// </summary>
    public sealed class EventSubscribeDataOperator : EventDataOperator<EventSubscribeData>,
        IDataOperator<EventSubscribeData>
    {
        #region 必须实现

        public object GetValue(EventSubscribeData entity, string field)
        {
            if (field == null) return null;
            return (field.Trim().ToLower()) switch
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
                "adddate" => entity.AddDate,
                "authorid" => entity.AuthorId,
                "author" => entity.Author,
                "lastmodifydate" => entity.LastModifyDate,
                "lastreviserid" => entity.LastReviserId,
                "lastreviser" => entity.LastReviser,
                _ => null,
            };
        }

        public void SetValue(EventSubscribeData entity, string field, object value)
        {
            if (field == null) return;
            switch (field.Trim().ToLower())
            {
                case "id":
                    entity.Id = (long)Convert.ToDecimal(value);
                    return;
                case "eventid":
                    entity.EventId = (long)Convert.ToDecimal(value);
                    return;
                case "service":
                    entity.Service = value?.ToString();
                    return;
                case "islookup":
                    if (value != null)
                    {
                        if (int.TryParse(value.ToString(), out int vl))
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
                    entity.ApiName = value?.ToString();
                    return;
                case "memo":
                    entity.Memo = value?.ToString();
                    return;
                case "targetname":
                    entity.TargetName = value?.ToString();
                    return;
                case "targettype":
                    entity.TargetType = value?.ToString();
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
                        else if (value is DataStateType type)
                        {
                            entity.DataState = type;
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
                case "adddate":
                    entity.AddDate = Convert.ToDateTime(value);
                    return;
                case "authorid":
                    entity.AuthorId = value?.ToString();
                    return;
                case "author":
                    entity.Author = value?.ToString();
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
            }
        }
        #endregion

        #region 优化实现

        /// <summary>
        /// 得到字段的DbType类型
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>参数</returns>
        public int GetDbType(string field)
        {
            switch (field)
            {
                case "id":
                case "Id":
                    return (int)MySqlDbType.Int64;
                case "event_id":
                case "EventId":
                    return (int)MySqlDbType.Int64;
                case "service":
                case "Service":
                    return (int)MySqlDbType.VarString;
                case "is_look_up":
                case "IsLookUp":
                    return (int)MySqlDbType.Byte;
                case "api_name":
                case "ApiName":
                    return (int)MySqlDbType.VarString;
                case "target_description":
                case "TargetDescription":
                    return (int)MySqlDbType.VarString;
                case "target_name":
                case "TargetName":
                    return (int)MySqlDbType.VarString;
                case "target_type":
                case "TargetType":
                    return (int)MySqlDbType.VarString;
                case "memo":
                case "Memo":
                    return (int)MySqlDbType.Text;
                case "is_freeze":
                case "IsFreeze":
                    return (int)MySqlDbType.Byte;
                case "data_state":
                case "DataState":
                    return (int)MySqlDbType.Int32;
                case "created_date":
                case "AddDate":
                    return (int)MySqlDbType.DateTime;
                case "created_user_id":
                case "AuthorId":
                    return (int)MySqlDbType.VarString;
                case "created_user":
                case "Author":
                    return (int)MySqlDbType.VarString;
                case "latest_updated_date":
                case "LastModifyDate":
                    return (int)MySqlDbType.DateTime;
                case "latest_updated_user_id":
                case "LastReviserId":
                    return (int)MySqlDbType.VarString;
                case "latest_updated_user":
                case "LastReviser":
                    return (int)MySqlDbType.VarString;
            }
            return (int)MySqlDbType.VarChar;
        }


        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public async Task LoadEntity(DbDataReader r, EventSubscribeData entity)
        {
            var reader = r as MySqlDataReader;
            entity.Id = await reader.GetFieldValueAsync<long>(0);
            if (!reader.IsDBNull(1))
                entity.EventId = await reader.GetFieldValueAsync<long>(1);
            if (!reader.IsDBNull(2))
                entity.Service = await reader.GetFieldValueAsync<string>(2);
            if (!reader.IsDBNull(3))
                entity.IsLookUp = await reader.GetFieldValueAsync<bool>(3);
            if (!reader.IsDBNull(4))
                entity.ApiName = await reader.GetFieldValueAsync<string>(4);
            if (!reader.IsDBNull(5))
                entity.TargetDescription = await reader.GetFieldValueAsync<string>(5);
            if (!reader.IsDBNull(6))
                entity.TargetName = await reader.GetFieldValueAsync<string>(6);
            if (!reader.IsDBNull(7))
                entity.TargetType = await reader.GetFieldValueAsync<string>(7);
            if (!reader.IsDBNull(8))
                entity.Memo = await reader.GetFieldValueAsync<string>(8);
            if (!reader.IsDBNull(9))
                entity.IsFreeze = await reader.GetFieldValueAsync<bool>(9);
            if (!reader.IsDBNull(10))
                entity.DataState = (DataStateType)await reader.GetFieldValueAsync<int>(10);
            if (!reader.IsDBNull(11))
                entity.AddDate = await reader.GetFieldValueAsync<DateTime>(11);
            if (!reader.IsDBNull(12))
                entity.AuthorId = await reader.GetFieldValueAsync<string>(12);
            if (!reader.IsDBNull(13))
                entity.Author = await reader.GetFieldValueAsync<string>(13);
            if (!reader.IsDBNull(14))
                entity.LastModifyDate = await reader.GetFieldValueAsync<DateTime>(14);
            if (!reader.IsDBNull(15))
                entity.LastReviserId = await reader.GetFieldValueAsync<string>(15);
            if (!reader.IsDBNull(16))
                entity.LastReviser = await reader.GetFieldValueAsync<string>(16);
        }

        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        public void SetEntityParameter(DbCommand cmd, EventSubscribeData entity)
        {
            //02:主键(Id)
            cmd.Parameters.Add(new MySqlParameter("Id", entity.Id));
            //03:事件标识(EventId)
            cmd.Parameters.Add(new MySqlParameter("EventId", entity.EventId));
            //04:所属服务(Service)
            cmd.Parameters.Add(new MySqlParameter("Service", entity.Service));
            //05:是否查阅服务(IsLookUp)
            cmd.Parameters.Add(new MySqlParameter("IsLookUp", entity.IsLookUp));
            //06:接口名称(ApiName)
            cmd.Parameters.Add(new MySqlParameter("ApiName", entity.ApiName));
            //07:目标说明(TargetDescription)
            cmd.Parameters.Add(new MySqlParameter("TargetDescription", entity.TargetDescription));
            //08:目标名称(TargetName)
            cmd.Parameters.Add(new MySqlParameter("TargetName", entity.TargetName));
            //09:目标类型(TargetType)
            cmd.Parameters.Add(new MySqlParameter("TargetType", entity.TargetType));
            //10:备注(Memo)
            cmd.Parameters.Add(new MySqlParameter("Memo", entity.Memo));
            //257:冻结更新(IsFreeze)
            cmd.Parameters.Add(new MySqlParameter("IsFreeze", entity.IsFreeze ? (byte)1 : (byte)0));
            //258:数据状态(DataState)
            cmd.Parameters.Add(new MySqlParameter("DataState", (int)entity.DataState));
            //268:制作时间(AddDate)
            if (entity.AddDate.Year < 1900)
                cmd.Parameters.Add(new MySqlParameter("AddDate", MySqlDbType.DateTime));
            else
                cmd.Parameters.Add(new MySqlParameter("AddDate", entity.AddDate));
            //269:制作人标识(AuthorId)
            cmd.Parameters.Add(new MySqlParameter("AuthorId", entity.AuthorId));
            //270:制作人(Author)
            cmd.Parameters.Add(new MySqlParameter("Author", entity.Author));
            //271:最后修改日期(LastModifyDate)
            if (entity.AddDate.Year < 1900)
                cmd.Parameters.Add(new MySqlParameter("LastModifyDate", MySqlDbType.DateTime));
            else
                cmd.Parameters.Add(new MySqlParameter("LastModifyDate", entity.LastModifyDate));
            //272:最后修改者标识(LastReviserId)
            cmd.Parameters.Add(new MySqlParameter("LastReviserId", entity.LastReviserId));
            //273:最后修改者(LastReviser)
            cmd.Parameters.Add(new MySqlParameter("LastReviser", entity.LastReviser));
        }

        #endregion

        #region Option

        /// <summary>
        /// 默认配置对象
        /// </summary>
        public static DataAccessOption<EventSubscribeData> DefaultOption => new DataAccessOption<EventSubscribeData>
        {
            NoInjection = true,
            CanRaiseEvent = false,
            UpdateByMidified = false,
            DataSturct = EventSubscribeDataStruct.Struct
        };

        /// <summary>
        /// 当前操作的配置对象
        /// </summary>
        public static DataAccessOption<EventSubscribeData> OperatorOption => new DataAccessOption<EventSubscribeData>
        {
            NoInjection = true,
            CanRaiseEvent = false,
            UpdateByMidified = false,
            DataSturct = EventSubscribeDataStruct.Struct,
            LoadFields = LoadFields,
            InsertSqlCode = InsertSqlCode,
            UpdateFields = UpdateFields,
            UpdateSqlCode = UpdateSqlCode
        };

        /// <summary>
        /// 读取的字段
        /// </summary>
        public const string LoadFields = @"
    `id`,
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
    `latest_updated_user`";

        /// <summary>
        /// 读取的SQL
        /// </summary>
        public const string LoadSqlCode = @"
SELECT 
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
    `latest_updated_user` AS `LastReviser`
FROM `tb_event_subscribe`";

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
    0,
    ?DataState,
    ?AddDate,
    ?AuthorId,
    ?Author,
    ?LastModifyDate,
    ?LastReviserId,
    ?LastReviser
);
SELECT @@IDENTITY;";

        /// <summary>
        /// 更新的字段
        /// </summary>
        public const string UpdateFields = @"
       `event_id` = ?EventId,
       `service` = ?Service,
       `is_look_up` = ?IsLookUp,
       `api_name` = ?ApiName,
       `target_description` = ?TargetDescription,
       `target_name` = ?TargetName,
       `target_type` = ?TargetType,
       `memo` = ?Memo";

        /// <summary>
        /// 更新的SQL
        /// </summary>
        public const string UpdateSqlCode = @"
UPDATE `tb_event_subscribe` set
       `event_id` = ?EventId,
       `service` = ?Service,
       `is_look_up` = ?IsLookUp,
       `api_name` = ?ApiName,
       `target_description` = ?TargetDescription,
       `target_name` = ?TargetName,
       `target_type` = ?TargetType,
       `memo` = ?Memo
WHERE `id` = ?Id";
        #endregion

    }
}