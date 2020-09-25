#region

using Agebull.EntityModel.MySql;
using Agebull.Common.Ioc;
using Agebull.EntityModel.Events;
using Agebull.EntityModel.Common;
using Zeroteam.MessageMVC.EventBus.Zeroteam.MessageMVC.EventBus;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

#endregion

namespace Zeroteam.MessageMVC.EventBus.DataAccess
{
    /// <summary>
    /// 事件订阅
    /// </summary>
    public sealed class EventSubscribeDataAccessOption :
        DataAccessOption<EventSubscribeData, MySqlSqlBuilder<EventSubscribeData>, EventBusDb>,
        IDataOperator<EventSubscribeData>
    {
        #region 方法实现

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initiate()
        {
            NoInjection = true;
            CanRaiseEvent = false;
            DataOperator = this;
            UpdateByMidified = false;
            SqlBuilder = new MySqlSqlBuilder<EventSubscribeData>
            {
                Option = this
            };
            DataSturct = EventSubscribeDataStruct.Struct;

            //LoadFields = loadFields;
            //InsertSqlCode = insertSqlCode;
            //UpdateFields = updateFields;


            base.Initiate();

            UpdateSqlCode = $@"
update {WriteTableName} set
{UpdateFields}
where {FieldMap[PrimaryKey]} = ?{PrimaryKey}";
        }

        /*// <summary>
        ///     得到可正确更新的条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public override void CheckUpdateContition(ref string condition)
        {
            condition = string.IsNullOrWhiteSpace(condition)
                ? "`is_freeze` = 0"
               : $"({condition}) AND `is_freeze` = 0";
        }

        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public sealed override async Task LoadEntity(DbDataReader r, EventSubscribeData entity)
        {
            var reader = r as MySqlDataReader;
            //if (!reader.IsDBNull(0))
                entity.Id = await reader.GetFieldValueAsync<long>(0);
            //if (!reader.IsDBNull(1))
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
                entity.AddDate =await reader.GetFieldValueAsync<DateTime>(11);
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
        /// 得到字段的DbType类型
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>参数</returns>
        public sealed override int GetDbType(string field)
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
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        public override void SetEntityParameter(DbCommand cmd, EventSubscribeData entity)
        {
            //02:主键(Id)
            cmd.Parameters.Add(new MySqlParameter("Id", MySqlDbType.Int64) { Value = entity.Id });
            //03:事件标识(EventId)
            cmd.Parameters.Add(new MySqlParameter("EventId", MySqlDbType.Int64) { Value = entity.EventId });
            //04:所属服务(Service)
            //var //isNull = string.IsNullOrWhiteSpace(entity.Service);
            var parameter = new MySqlParameter("Service", MySqlDbType.VarString, isNull ? 10 : (entity.Service).Length);
            ////if (isNull)
            //    //parameter.Value = DBNull.Value;
            ////else
                parameter.Value = entity.Service;
            cmd.Parameters.Add(parameter);
            //05:是否查阅服务(IsLookUp)
            cmd.Parameters.Add(new MySqlParameter("IsLookUp", MySqlDbType.Byte) { Value = entity.IsLookUp ? (byte)1 : (byte)0 });
            //06:接口名称(ApiName)
            //isNull = string.IsNullOrWhiteSpace(entity.ApiName);
            parameter = new MySqlParameter("ApiName", MySqlDbType.VarString, isNull ? 10 : (entity.ApiName).Length);
            //if (isNull)
                //parameter.Value = DBNull.Value;
            //else
                parameter.Value = entity.ApiName;
            cmd.Parameters.Add(parameter);
            //07:目标说明(TargetDescription)
            //isNull = string.IsNullOrWhiteSpace(entity.TargetDescription);
            parameter = new MySqlParameter("TargetDescription", MySqlDbType.VarString, isNull ? 10 : (entity.TargetDescription).Length);
            //if (isNull)
                //parameter.Value = DBNull.Value;
            //else
                parameter.Value = entity.TargetDescription;
            cmd.Parameters.Add(parameter);
            //08:目标名称(TargetName)
            //isNull = string.IsNullOrWhiteSpace(entity.TargetName);
            parameter = new MySqlParameter("TargetName", MySqlDbType.VarString, isNull ? 10 : (entity.TargetName).Length);
            //if (isNull)
                //parameter.Value = DBNull.Value;
            //else
                parameter.Value = entity.TargetName;
            cmd.Parameters.Add(parameter);
            //09:目标类型(TargetType)
            //isNull = string.IsNullOrWhiteSpace(entity.TargetType);
            parameter = new MySqlParameter("TargetType", MySqlDbType.VarString, isNull ? 10 : (entity.TargetType).Length);
            //if (isNull)
                //parameter.Value = DBNull.Value;
            //else
                parameter.Value = entity.TargetType;
            cmd.Parameters.Add(parameter);
            //10:备注(Memo)
            //isNull = string.IsNullOrWhiteSpace(entity.Memo);
            parameter = new MySqlParameter("Memo", MySqlDbType.Text, isNull ? 10 : (entity.Memo).Length);
            //if (isNull)
                //parameter.Value = DBNull.Value;
            //else
                parameter.Value = entity.Memo;
            cmd.Parameters.Add(parameter);
            //257:冻结更新(IsFreeze)
            cmd.Parameters.Add(new MySqlParameter("IsFreeze", MySqlDbType.Byte) { Value = entity.IsFreeze ? (byte)1 : (byte)0 });
            //258:数据状态(DataState)
            cmd.Parameters.Add(new MySqlParameter("DataState", MySqlDbType.Int32) { Value = (int)entity.DataState });
            //268:制作时间(AddDate)
            //isNull = entity.AddDate.Year < 1900;
            parameter = new MySqlParameter("AddDate", MySqlDbType.DateTime);
            //if (isNull)
                //parameter.Value = DBNull.Value;
            //else
                parameter.Value = entity.AddDate;
            cmd.Parameters.Add(parameter);
            //269:制作人标识(AuthorId)
            //isNull = string.IsNullOrWhiteSpace(entity.AuthorId);
            parameter = new MySqlParameter("AuthorId", MySqlDbType.VarString, isNull ? 10 : (entity.AuthorId).Length);
            //if (isNull)
                //parameter.Value = DBNull.Value;
            //else
                parameter.Value = entity.AuthorId;
            cmd.Parameters.Add(parameter);
            //270:制作人(Author)
            //isNull = string.IsNullOrWhiteSpace(entity.Author);
            parameter = new MySqlParameter("Author", MySqlDbType.VarString, isNull ? 10 : (entity.Author).Length);
            //if (isNull)
                //parameter.Value = DBNull.Value;
            //else
                parameter.Value = entity.Author;
            cmd.Parameters.Add(parameter);
            //271:最后修改日期(LastModifyDate)
            //isNull = entity.LastModifyDate.Year < 1900;
            parameter = new MySqlParameter("LastModifyDate", MySqlDbType.DateTime);
            //if (isNull)
                //parameter.Value = DBNull.Value;
            //else
                parameter.Value = entity.LastModifyDate;
            cmd.Parameters.Add(parameter);
            //272:最后修改者标识(LastReviserId)
            //isNull = string.IsNullOrWhiteSpace(entity.LastReviserId);
            parameter = new MySqlParameter("LastReviserId", MySqlDbType.VarString, isNull ? 10 : (entity.LastReviserId).Length);
            //if (isNull)
                //parameter.Value = DBNull.Value;
            //else
                parameter.Value = entity.LastReviserId;
            cmd.Parameters.Add(parameter);
            //273:最后修改者(LastReviser)
            //isNull = string.IsNullOrWhiteSpace(entity.LastReviser);
            parameter = new MySqlParameter("LastReviser", MySqlDbType.VarString, isNull ? 10 : (entity.LastReviser).Length);
            //if (isNull)
                //parameter.Value = DBNull.Value;
            //else
                parameter.Value = entity.LastReviser;
            cmd.Parameters.Add(parameter);
        }
        */
        #endregion

        #region SQL
        public const string loadFields = @"
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
    `latest_updated_user` AS `LastReviser`";

        const string insertSqlCode = @"
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


        public const string updateFields = @"
       `event_id` = ?EventId,
       `service` = ?Service,
       `is_look_up` = ?IsLookUp,
       `api_name` = ?ApiName,
       `target_description` = ?TargetDescription,
       `target_name` = ?TargetName,
       `target_type` = ?TargetType,
       `memo` = ?Memo";
        #endregion

        #region IDataOperator

        object IDataOperator<EventSubscribeData>.GetValue(EventSubscribeData entity, string field)
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

        void IDataOperator<EventSubscribeData>.SetValue(EventSubscribeData entity, string field, object value)
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
    }
}