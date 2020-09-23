#region

using Agebull.EntityModel.MySql;
using Agebull.Common.Ioc;
using Agebull.EntityModel.Events;
using Agebull.EntityModel.Common;
using Zeroteam.MessageMVC.EventBus.Zeroteam.MessageMVC.EventBus;
using System;

#endregion

namespace Zeroteam.MessageMVC.EventBus.DataAccess
{
    /// <summary>
    /// 事件订阅
    /// </summary>
    public sealed class EventSubscribeDataAccessOption : DataAccessOption<EventSubscribeData>, IDataOperator<EventSubscribeData>
    {
        public EventSubscribeDataAccessOption()
        {
            DataOperator = this;
            DataUpdateHandler = DependencyHelper.GetService<IDataUpdateHandler>();
            DataUpdateHandler?.InitType<EventSubscribeData>();
            CreateDataBase = () => DependencyHelper.GetService<EventBusDb>();
            SqlBuilder = new MySqlSqlBuilder<EventSubscribeData>
            {
                Option = this
            };
            DataSturct = EventSubscribeDataStruct.Struct;
            Init();
        }

        #region 方法实现


        /*// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        protected sealed override async Task ReadEntity(DbDataReader r, EventSubscribeData entity)
        {
            var  reader = r as MySqlDataReader;
            if (!reader.IsDBNull(0))
                entity._id = await reader.GetFieldValueAsync<long>(0);
            if (!reader.IsDBNull(1))
                entity._eventId = (long)reader.GetInt64(1);
            if (!reader.IsDBNull(2))
                entity._service = reader.GetString(2);
            if (!reader.IsDBNull(3))
                entity._isLookUp = reader.GetBoolean(3);
            if (!reader.IsDBNull(4))
                entity._apiName = reader.GetString(4);
            if (!reader.IsDBNull(5))
                entity._targetDescription = reader.GetString(5);
            if (!reader.IsDBNull(6))
                entity._targetName = reader.GetString(6);
            if (!reader.IsDBNull(7))
                entity._targetType = reader.GetString(7);
            if (!reader.IsDBNull(8))
                entity._memo = reader.GetString(8).ToString();
            if (!reader.IsDBNull(9))
                entity._isFreeze = reader.GetBoolean(9);
            if (!reader.IsDBNull(10))
                entity._dataState = (DataStateType)reader.GetInt32(10);
            if (!reader.IsDBNull(11))
                entity._addDate = reader.GetDateTime(11);
            if (!reader.IsDBNull(12))
                entity._authorId = reader.GetString(12);
            if (!reader.IsDBNull(13))
                entity._author = reader.GetString(13);
            if (!reader.IsDBNull(14))
                entity._lastModifyDate = reader.GetDateTime(14);
            if (!reader.IsDBNull(15))
                entity._lastReviserId = reader.GetString(15);
            if (!reader.IsDBNull(16))
                entity._lastReviser = reader.GetString(16);
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
                    return (int) MySqlDbType.Int64;
                case "event_id":
                case "EventId":
                    return (int) MySqlDbType.Int64;
                case "service":
                case "Service":
                    return (int) MySqlDbType.VarString;
                case "is_look_up":
                case "IsLookUp":
                    return (int) MySqlDbType.Byte;
                case "api_name":
                case "ApiName":
                    return (int) MySqlDbType.VarString;
                case "target_description":
                case "TargetDescription":
                    return (int) MySqlDbType.VarString;
                case "target_name":
                case "TargetName":
                    return (int) MySqlDbType.VarString;
                case "target_type":
                case "TargetType":
                    return (int) MySqlDbType.VarString;
                case "memo":
                case "Memo":
                    return (int) MySqlDbType.Text;
                case "is_freeze":
                case "IsFreeze":
                    return (int) MySqlDbType.Byte;
                case "data_state":
                case "DataState":
                    return (int) MySqlDbType.Int32;
                case "created_date":
                case "AddDate":
                    return (int) MySqlDbType.DateTime;
                case "created_user_id":
                case "AuthorId":
                    return (int) MySqlDbType.VarString;
                case "created_user":
                case "Author":
                    return (int) MySqlDbType.VarString;
                case "latest_updated_date":
                case "LastModifyDate":
                    return (int) MySqlDbType.DateTime;
                case "latest_updated_user_id":
                case "LastReviserId":
                    return (int) MySqlDbType.VarString;
                case "latest_updated_user":
                case "LastReviser":
                    return (int) MySqlDbType.VarString;
            }
            return (int) MySqlDbType.VarChar;
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
            var isNull = string.IsNullOrWhiteSpace(entity.Service);
            var parameter = new MySqlParameter("Service", MySqlDbType.VarString, isNull ? 10 : (entity.Service).Length);
            if (isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Service;
            cmd.Parameters.Add(parameter);
            //05:是否查阅服务(IsLookUp)
            cmd.Parameters.Add(new MySqlParameter("IsLookUp", MySqlDbType.Byte) { Value = entity.IsLookUp ? (byte)1 : (byte)0 });
            //06:接口名称(ApiName)
            isNull = string.IsNullOrWhiteSpace(entity.ApiName);
            parameter = new MySqlParameter("ApiName", MySqlDbType.VarString, isNull ? 10 : (entity.ApiName).Length);
            if (isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.ApiName;
            cmd.Parameters.Add(parameter);
            //07:目标说明(TargetDescription)
            isNull = string.IsNullOrWhiteSpace(entity.TargetDescription);
            parameter = new MySqlParameter("TargetDescription", MySqlDbType.VarString, isNull ? 10 : (entity.TargetDescription).Length);
            if (isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.TargetDescription;
            cmd.Parameters.Add(parameter);
            //08:目标名称(TargetName)
            isNull = string.IsNullOrWhiteSpace(entity.TargetName);
            parameter = new MySqlParameter("TargetName", MySqlDbType.VarString, isNull ? 10 : (entity.TargetName).Length);
            if (isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.TargetName;
            cmd.Parameters.Add(parameter);
            //09:目标类型(TargetType)
            isNull = string.IsNullOrWhiteSpace(entity.TargetType);
            parameter = new MySqlParameter("TargetType", MySqlDbType.VarString, isNull ? 10 : (entity.TargetType).Length);
            if (isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.TargetType;
            cmd.Parameters.Add(parameter);
            //10:备注(Memo)
            isNull = string.IsNullOrWhiteSpace(entity.Memo);
            parameter = new MySqlParameter("Memo", MySqlDbType.Text, isNull ? 10 : (entity.Memo).Length);
            if (isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Memo;
            cmd.Parameters.Add(parameter);
            //257:冻结更新(IsFreeze)
            cmd.Parameters.Add(new MySqlParameter("IsFreeze", MySqlDbType.Byte) { Value = entity.IsFreeze ? (byte)1 : (byte)0 });
            //258:数据状态(DataState)
            cmd.Parameters.Add(new MySqlParameter("DataState", MySqlDbType.Int32) { Value = (int)entity.DataState });
            //268:制作时间(AddDate)
            isNull = entity.AddDate.Year < 1900;
            parameter = new MySqlParameter("AddDate", MySqlDbType.DateTime);
            if (isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.AddDate;
            cmd.Parameters.Add(parameter);
            //269:制作人标识(AuthorId)
            isNull = string.IsNullOrWhiteSpace(entity.AuthorId);
            parameter = new MySqlParameter("AuthorId", MySqlDbType.VarString, isNull ? 10 : (entity.AuthorId).Length);
            if (isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.AuthorId;
            cmd.Parameters.Add(parameter);
            //270:制作人(Author)
            isNull = string.IsNullOrWhiteSpace(entity.Author);
            parameter = new MySqlParameter("Author", MySqlDbType.VarString, isNull ? 10 : (entity.Author).Length);
            if (isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Author;
            cmd.Parameters.Add(parameter);
            //271:最后修改日期(LastModifyDate)
            isNull = entity.LastModifyDate.Year < 1900;
            parameter = new MySqlParameter("LastModifyDate", MySqlDbType.DateTime);
            if (isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.LastModifyDate;
            cmd.Parameters.Add(parameter);
            //272:最后修改者标识(LastReviserId)
            isNull = string.IsNullOrWhiteSpace(entity.LastReviserId);
            parameter = new MySqlParameter("LastReviserId", MySqlDbType.VarString, isNull ? 10 : (entity.LastReviserId).Length);
            if (isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.LastReviserId;
            cmd.Parameters.Add(parameter);
            //273:最后修改者(LastReviser)
            isNull = string.IsNullOrWhiteSpace(entity.LastReviser);
            parameter = new MySqlParameter("LastReviser", MySqlDbType.VarString, isNull ? 10 : (entity.LastReviser).Length);
            if (isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.LastReviser;
            cmd.Parameters.Add(parameter);
        }
        */
        #endregion


        /// <summary>
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
    }

    /*
    partial class EventBusDb
    {


        /// <summary>
        /// 事件订阅的结构语句
        /// </summary>
        private TableSql _EventSubscribeSql = new TableSql
        {
            TableName = "tb_event_subscribe",
            PimaryKey = "Id"
        };


        /// <summary>
        /// 事件订阅数据访问对象
        /// </summary>
        private EventSubscribeDataAccess _eventSubscribes;

        /// <summary>
        /// 事件订阅数据访问对象
        /// </summary>
        public EventSubscribeDataAccess EventSubscribes
        {
            get
            {
                return this._eventSubscribes ?? ( this._eventSubscribes = new EventSubscribeDataAccess{ DataBase = this});
            }
        }


        /// <summary>
        /// 事件订阅(tb_event_subscribe):事件订阅
        /// </summary>
        public const int Table_EventSubscribe = 0x0;
    }*/
}