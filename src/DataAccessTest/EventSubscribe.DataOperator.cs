#region
using System;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

using Agebull.EntityModel.Common;
using Agebull.EntityModel.Interfaces;


#endregion
namespace Zeroteam.MessageMVC.EventBus.DataAccess
{
    /// <summary>
    /// 事件订阅
    /// </summary>
    public sealed class EventSubscribeDataOperator : IDataOperator<EventSubscribeData>, IEntityOperator<EventSubscribeData>
    {
        /// <summary>
        /// 驱动提供者信息
        /// </summary>
        public DataAccessProvider<EventSubscribeData> Provider { get; set; }

        /// <summary>
        /// 配置信息
        /// </summary>
        internal static DataAccessOption Option => new DataAccessOption
        {
            LoadFields = LoadFields,
            UpdateFields = UpdateFields,
            InsertSqlCode = InsertSqlCode,
            NoInjection = true,
            UpdateByMidified = false,
            DataSturct = EventBusDb.EventSubscribe_Struct_.Struct
        };

        #region 基本SQL语句

        /// <summary>
        /// 读取的字段
        /// </summary>
        public const string LoadFields = @"
    `id`,
    `event_id`,
    `service`,
    `is_look_up`,
    `api_name`,
    `memo`,
    `target_name`,
    `target_type`,
    `target_description`,
    `is_freeze`,
    `data_state`,
    `created_date`,
    `created_user_id`,
    `created_user`,
    `latest_updated_date`,
    `latest_updated_user_id`,
    `latest_updated_user`";

        /// <summary>
        /// 更新的字段
        /// </summary>
        public static string UpdateFields = $@"
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
        public static string InsertSqlCode => $@"
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
    ?Memo,
    ?TargetName,
    ?TargetType,
    ?TargetDescription,
    ?IsFreeze,
    ?DataState,
    ?AddDate,
    ?AuthorId,
    ?Author,
    ?LastModifyDate,
    ?LastReviserId,
    ?LastReviser
);";

        #endregion

        #region 操作代码

        /// <summary>
        /// 得到字段的DbType类型
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>参数</returns>
        public int GetDbType(string field)
        {
            if (field == null)
                return (int)MySqlDbType.VarChar;
            switch (field)
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
                case "created_date":
                case "adddate":
                    return (int)MySqlDbType.DateTime;
                case "created_user_id":
                case "authorid":
                    return (int)MySqlDbType.VarString;
                case "created_user":
                case "author":
                    return (int)MySqlDbType.VarString;
                case "latest_updated_date":
                case "lastmodifydate":
                    return (int)MySqlDbType.DateTime;
                case "latest_updated_user_id":
                case "lastreviserid":
                    return (int)MySqlDbType.VarString;
                case "latest_updated_user":
                case "lastreviser":
                    return (int)MySqlDbType.VarString;
                default:
                    return (int)MySqlDbType.VarChar;
            }
        }



        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="r">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public async Task LoadEntity(DbDataReader r, EventSubscribeData entity)
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
            entity.AddDate = await reader.GetFieldValueAsync<DateTime>(11);
            if (reader.IsDBNull(12))
                entity.AuthorId = null;
            else
                entity.AuthorId = await reader.GetFieldValueAsync<string>(12);
            if (reader.IsDBNull(13))
                entity.Author = null;
            else
                entity.Author = await reader.GetFieldValueAsync<string>(13);
            if (reader.IsDBNull(14))
                entity.LastModifyDate = default;
            else
                entity.LastModifyDate = await reader.GetFieldValueAsync<DateTime>(14);
            if (reader.IsDBNull(15))
                entity.LastReviserId = null;
            else
                entity.LastReviserId = await reader.GetFieldValueAsync<string>(15);
            if (reader.IsDBNull(16))
                entity.LastReviser = null;
            else
                entity.LastReviser = await reader.GetFieldValueAsync<string>(16);
        }



        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        public void SetEntityParameter(EventSubscribeData entity, MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter("Id", entity.Id));
            cmd.Parameters.Add(new MySqlParameter("EventId", entity.EventId));
            cmd.Parameters.Add(new MySqlParameter("Service", entity.Service));
            cmd.Parameters.Add(new MySqlParameter("IsLookUp", entity.IsLookUp ? (byte)1 : (byte)0));
            cmd.Parameters.Add(new MySqlParameter("ApiName", entity.ApiName));
            cmd.Parameters.Add(new MySqlParameter("Memo", entity.Memo));
            cmd.Parameters.Add(new MySqlParameter("TargetName", entity.TargetName));
            cmd.Parameters.Add(new MySqlParameter("TargetType", entity.TargetType));
            cmd.Parameters.Add(new MySqlParameter("TargetDescription", entity.TargetDescription));
            cmd.Parameters.Add(new MySqlParameter("IsFreeze", entity.IsFreeze ? (byte)1 : (byte)0));
            cmd.Parameters.Add(new MySqlParameter("DataState", (int)entity.DataState));
            cmd.Parameters.Add(new MySqlParameter("AddDate", entity.AddDate));
            cmd.Parameters.Add(new MySqlParameter("AuthorId", entity.AuthorId));
            cmd.Parameters.Add(new MySqlParameter("Author", entity.Author));
            cmd.Parameters.Add(new MySqlParameter("LastModifyDate", entity.LastModifyDate));
            cmd.Parameters.Add(new MySqlParameter("LastReviserId", entity.LastReviserId));
            cmd.Parameters.Add(new MySqlParameter("LastReviser", entity.LastReviser));
        }



        /// <summary>
        ///     读取属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        object IEntityOperator<EventSubscribeData>.GetValue(EventSubscribeData entity, string property)
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
                _ => null
            };
        }


        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        void IEntityOperator<EventSubscribeData>.SetValue(EventSubscribeData entity, string property, object value)
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
            }
        }

        #endregion
    }
}