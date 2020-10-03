/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/3 11:04:54*/
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
    /// 事件订阅
    /// </summary>
    public sealed class EventSubscribeModelDataOperator : IDataOperator<EventSubscribeModel> , IEntityOperator<EventSubscribeModel>
    {
        #region 基本信息

        /// <summary>
        /// 驱动提供者信息
        /// </summary>
        public DataAccessProvider<EventSubscribeModel> Provider { get; set; }

        static EntityStruct _struct;

        /// <summary>
        /// 实体结构
        /// </summary>
        public static EntityStruct Struct => _struct ??= new EntityStruct
        {
            IsIdentity       = false,
            EntityName       = EventBusDb.EventSubscribe_Struct_.EntityName,
            Caption          = EventBusDb.EventSubscribe_Struct_.Caption,
            Description      = EventBusDb.EventSubscribe_Struct_.Description,
            PrimaryKey       = EventBusDb.EventSubscribe_Struct_.PrimaryKey,
            ReadTableName    = EventBusDb.EventSubscribe_Struct_.TableName,
            WriteTableName   = EventBusDb.EventSubscribe_Struct_.TableName,
            InterfaceFeature = new[] {nameof(GlobalDataInterfaces.IStateData),nameof(GlobalDataInterfaces.IHistoryData),nameof(GlobalDataInterfaces.IAuthorData)},
            Properties       = new List<EntityProperty>
            {
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.Service,0,"Service","tb_event_subscribe","service",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.EventId,1,"EventId","tb_event_subscribe","event_id",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update),
                new EntityProperty(EventBusDb.EventSubscribe_Struct_.IsLookUp,2,"IsLookUp","tb_event_subscribe","is_look_up",ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert | ReadWriteFeatrue.Update)
            }
        };

        /// <summary>
        /// 配置信息
        /// </summary>
        internal static DataAccessOption Option = new DataAccessOption
        {
            NoInjection      = false,
            IsQuery          = true,
            UpdateByMidified = false,
            ReadTableName    = FromSqlCode,
            WriteTableName   = EventBusDb.EventSubscribe_Struct_.TableName,
            LoadFields       = LoadFields,
            Having           = Having,
            GroupFields      = GroupFields,
            UpdateFields     = UpdateFields,
            InsertSqlCode    = InsertSqlCode,
            DataStruct       = Struct
        };

        #endregion

        #region SQL

        /// <summary>
        /// 读取的字段
        /// </summary>
        public const string LoadFields = @"Count(`tb_event_subscribe`.`service`) AS `service`
,`tb_event_subscribe`.`event_id` AS `event_id`
,`tb_event_subscribe`.`is_look_up` AS `is_look_up`
";

        /// <summary>
        /// 汇总条件
        /// </summary>
        public const string Having = null;

        /// <summary>
        /// 分组字段
        /// </summary>
        public const string GroupFields = "\nGROUP BY `tb_event_subscribe`.`event_id`,`tb_event_subscribe`.`is_look_up`";

        /// <summary>
        /// 读取的字段
        /// </summary>
        public const string FromSqlCode = @"tb_event_subscribe";

        /// <summary>
        /// 更新的字段
        /// </summary>
        public const string UpdateFields = @"";

        /// <summary>
        /// 写入的Sql
        /// </summary>
        public const string InsertSqlCode = @"";

        #endregion

        #region IDataOperator

        /// <summary>
        /// 得到字段的DbType类型
        /// </summary>
        /// <param name="property">字段名称</param>
        /// <returns>参数</returns>
        public int GetDbType(string property)
        {
            if(property == null) 
               return (int)MySqlDbType.VarChar;
            switch (property)
            {
                case "service":
                    return (int)MySqlDbType.Int64;
                case "event_id":
                case "eventid":
                    return (int)MySqlDbType.Int64;
                case "is_look_up":
                case "islookup":
                    return (int)MySqlDbType.Byte;
                default:
                    return (int)MySqlDbType.VarChar;
            }
        }



        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="r">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public async Task LoadEntity(DbDataReader r,EventSubscribeModel entity)
        {
            var reader = r as MySqlDataReader;
            if (reader.IsDBNull(0))
                entity.Service = default;
            else
                entity.Service = await reader.GetFieldValueAsync<long>(0);
            if (reader.IsDBNull(1))
                entity.EventId = default;
            else
                entity.EventId = await reader.GetFieldValueAsync<long>(1);
            if (reader.IsDBNull(2))
                entity.IsLookUp = default;
            else
                entity.IsLookUp = await reader.GetFieldValueAsync<bool>(2);
        }



        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        public void SetEntityParameter(EventSubscribeModel entity, MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter("Service", entity.Service));
            cmd.Parameters.Add(new MySqlParameter("EventId", entity.EventId));
            cmd.Parameters.Add(new MySqlParameter("IsLookUp", entity.IsLookUp ? (byte)1 : (byte)0));
        }

        #endregion

        #region IEntityOperator


        /// <summary>
        ///     读取属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        object IEntityOperator<EventSubscribeModel>.GetValue(EventSubscribeModel entity, string property)
        {
            if (property == null) return null;
            return (property.Trim().ToLower()) switch
            {
                "service" => entity.Service,
                "eventid" => entity.EventId,
                "islookup" => entity.IsLookUp,
                _ => null
            };
        }
    

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        void IEntityOperator<EventSubscribeModel>.SetValue(EventSubscribeModel entity, string property, object value)
        {
            if(property == null)
                return;
            switch(property.Trim().ToLower())
            {
            case "service":
                entity.Service = (long)Convert.ToDecimal(value);
                return;
            case "eventid":
                entity.EventId = (long)Convert.ToDecimal(value);
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
            }
        }

        #endregion
    }
}