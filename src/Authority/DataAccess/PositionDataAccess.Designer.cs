/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/6/11 23:09:49*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Linq;
using System.Text;
using Gboxt.Common.DataModel;

using MySql.Data.MySqlClient;
using Gboxt.Common.DataModel.MySql;


namespace Agebull.SystemAuthority.Organizations.DataAccess
{
    /// <summary>
    /// 职位字典
    /// </summary>
    public partial class PositionDataAccess
    {
        #region 基本SQL语句

        /// <summary>
        /// 表的唯一标识
        /// </summary>
        public override int TableId
        {
            get { return Authorities.Table_Position; }
        }

        /// <summary>
        /// 读取表名
        /// </summary>
        protected sealed override string ReadTableName
        {
            get
            {
                return @"tb_sys_position";
            }
        }

        /// <summary>
        /// 写入表名
        /// </summary>
        protected sealed override string WriteTableName
        {
            get
            {
                return @"tb_sys_position";
            }
        }

        /// <summary>
        /// 主键
        /// </summary>
        protected sealed override string PrimaryKey
        {
            get
            {
                return @"Id";
            }
        }

        /// <summary>
        /// 全表读取的SQL语句
        /// </summary>
        protected sealed override string FullLoadFields
        {
            get
            {
                return @"
    `id` AS `Id`,
    `full_name` AS `FullName`,
    `short_name` AS `ShortName`,
    `position_level` AS `PositionLevel`,
    `memo` AS `Memo`,
    `data_state` AS `DataState`,
    `is_freeze` AS `IsFreeze`,
    `author_id` AS `AuthorID`,
    `add_date` AS `AddDate`,
    `last_reviser_id` AS `LastReviserID`,
    `last_modify_date` AS `LastModifyDate`,
    `audit_state` AS `AuditState`,
    `auditor_id` AS `AuditorId`,
    `audit_date` AS `AuditDate`";
            }
        }

        

        /// <summary>
        /// 插入的SQL语句
        /// </summary>
        protected sealed override string InsertSqlCode
        {
            get
            {
                return @"
INSERT INTO `tb_sys_position`
(
    `full_name`,
    `short_name`,
    `position_level`,
    `memo`,
    `data_state`,
    `is_freeze`,
    `author_id`,
    `add_date`,
    `last_reviser_id`,
    `last_modify_date`,
    `audit_state`,
    `auditor_id`,
    `audit_date`
)
VALUES
(
    ?FullName,
    ?ShortName,
    ?PositionLevel,
    ?Memo,
    ?DataState,
    ?IsFreeze,
    ?AuthorID,
    ?AddDate,
    ?LastReviserID,
    ?LastModifyDate,
    ?AuditState,
    ?AuditorId,
    ?AuditDate
);
SELECT @@IDENTITY;";
            }
        }

        /// <summary>
        /// 全部更新的SQL语句
        /// </summary>
        protected sealed override string UpdateSqlCode
        {
            get
            {
                return @"
UPDATE `tb_sys_position` SET
       `full_name` = ?FullName,
       `short_name` = ?ShortName,
       `position_level` = ?PositionLevel,
       `memo` = ?Memo,
       `data_state` = ?DataState,
       `is_freeze` = ?IsFreeze,
       `author_id` = ?AuthorID,
       `add_date` = ?AddDate,
       `last_reviser_id` = ?LastReviserID,
       `last_modify_date` = ?LastModifyDate,
       `audit_state` = ?AuditState,
       `auditor_id` = ?AuditorId,
       `audit_date` = ?AuditDate
 WHERE `id` = ?Id;";
            }
        }

        /// <summary>
        /// 取得仅更新的SQL语句
        /// </summary>
        internal string GetModifiedSqlCode(PositionData data)
        {
            if (data.__EntityStatusNull || !data.__EntityStatus.IsModified)
                return ";";
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE `tb_sys_position` SET");
            //全称
            if (data.__EntityStatus.ModifiedProperties[PositionData.Real_FullName] > 0)
                sql.AppendLine("       `full_name` = ?FullName");
            //简称
            if (data.__EntityStatus.ModifiedProperties[PositionData.Real_ShortName] > 0)
                sql.AppendLine("       `short_name` = ?ShortName");
            //级别
            if (data.__EntityStatus.ModifiedProperties[PositionData.Real_PositionLevel] > 0)
                sql.AppendLine("       `position_level` = ?PositionLevel");
            //备注
            if (data.__EntityStatus.ModifiedProperties[PositionData.Real_Memo] > 0)
                sql.AppendLine("       `memo` = ?Memo");
            //数据状态
            if (data.__EntityStatus.ModifiedProperties[PositionData.Real_DataState] > 0)
                sql.AppendLine("       `data_state` = ?DataState");
            //数据是否已冻结
            if (data.__EntityStatus.ModifiedProperties[PositionData.Real_IsFreeze] > 0)
                sql.AppendLine("       `is_freeze` = ?IsFreeze");
            //制作人
            if (data.__EntityStatus.ModifiedProperties[PositionData.Real_AuthorID] > 0)
                sql.AppendLine("       `author_id` = ?AuthorID");
            //制作时间
            if (data.__EntityStatus.ModifiedProperties[PositionData.Real_AddDate] > 0)
                sql.AppendLine("       `add_date` = ?AddDate");
            //最后修改者
            if (data.__EntityStatus.ModifiedProperties[PositionData.Real_LastReviserID] > 0)
                sql.AppendLine("       `last_reviser_id` = ?LastReviserID");
            //最后修改日期
            if (data.__EntityStatus.ModifiedProperties[PositionData.Real_LastModifyDate] > 0)
                sql.AppendLine("       `last_modify_date` = ?LastModifyDate");
            //审核状态
            if (data.__EntityStatus.ModifiedProperties[PositionData.Real_AuditState] > 0)
                sql.AppendLine("       `audit_state` = ?AuditState");
            //审核人
            if (data.__EntityStatus.ModifiedProperties[PositionData.Real_AuditorId] > 0)
                sql.AppendLine("       `auditor_id` = ?AuditorId");
            //审核时间
            if (data.__EntityStatus.ModifiedProperties[PositionData.Real_AuditDate] > 0)
                sql.AppendLine("       `audit_date` = ?AuditDate");
            sql.Append(" WHERE `id` = ?Id;");
            return sql.ToString();
        }

        #endregion

        #region 字段

        /// <summary>
        ///  所有字段
        /// </summary>
        static string[] _fields = new string[]{ "Id","FullName","ShortName","PositionLevel","Memo","DataState","IsFreeze","AuthorID","AddDate","LastReviserID","LastModifyDate","AuditState","AuditorId","AuditDate" };

        /// <summary>
        ///  所有字段
        /// </summary>
        public sealed override string[] Fields
        {
            get
            {
                return _fields;
            }
        }

        /// <summary>
        ///  字段字典
        /// </summary>
        public static Dictionary<string, string> fieldMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Id" , "id" },
            { "FullName" , "full_name" },
            { "full_name" , "full_name" },
            { "ShortName" , "short_name" },
            { "short_name" , "short_name" },
            { "PositionLevel" , "position_level" },
            { "position_level" , "position_level" },
            { "Memo" , "memo" },
            { "DataState" , "data_state" },
            { "data_state" , "data_state" },
            { "IsFreeze" , "is_freeze" },
            { "is_freeze" , "is_freeze" },
            { "AuthorID" , "author_id" },
            { "author_id" , "author_id" },
            { "AddDate" , "add_date" },
            { "add_date" , "add_date" },
            { "LastReviserID" , "last_reviser_id" },
            { "last_reviser_id" , "last_reviser_id" },
            { "LastModifyDate" , "last_modify_date" },
            { "last_modify_date" , "last_modify_date" },
            { "AuditState" , "audit_state" },
            { "audit_state" , "audit_state" },
            { "AuditorId" , "auditor_id" },
            { "auditor_id" , "auditor_id" },
            { "AuditDate" , "audit_date" },
            { "audit_date" , "audit_date" }
        };

        /// <summary>
        ///  字段字典
        /// </summary>
        public sealed override Dictionary<string, string> FieldMap
        {
            get { return fieldMap ; }
        }
        #endregion

        #region 方法实现


        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        protected sealed override void LoadEntity(MySqlDataReader reader,PositionData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._id = (int)reader.GetInt32(0);
                if (!reader.IsDBNull(1))
                    entity._fullname = reader.GetString(1).ToString();
                if (!reader.IsDBNull(2))
                    entity._shortname = reader.GetString(2).ToString();
                entity._positionlevel = (int)reader.GetInt32(3);
                if (!reader.IsDBNull(4))
                    entity._memo = reader.GetString(4).ToString();
                if (!reader.IsDBNull(5))
                    entity._datastate = (DataStateType)reader.GetInt32(5);
                entity._isfreeze = (bool)reader.GetBoolean(6);
                entity._authorid = (int)reader.GetInt32(7);
                if (!reader.IsDBNull(8))
                    try{entity._adddate = reader.GetMySqlDateTime(8).Value;}catch{}
                entity._lastreviserid = (int)reader.GetInt32(9);
                if (!reader.IsDBNull(10))
                    try{entity._lastmodifydate = reader.GetMySqlDateTime(10).Value;}catch{}
                if (!reader.IsDBNull(11))
                    entity._auditstate = (AuditStateType)reader.GetInt32(11);
                entity._auditorid = (int)reader.GetInt32(12);
                if (!reader.IsDBNull(13))
                    try{entity._auditdate = reader.GetMySqlDateTime(13).Value;}catch{}
            }
        }

        /// <summary>
        /// 得到字段的DbType类型
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>参数</returns>
        protected sealed override MySqlDbType GetDbType(string field)
        {
            switch (field)
            {
                case "Id":
                    return MySqlDbType.Int32;
                case "FullName":
                    return MySqlDbType.VarString;
                case "ShortName":
                    return MySqlDbType.VarString;
                case "PositionLevel":
                    return MySqlDbType.Int32;
                case "Memo":
                    return MySqlDbType.Text;
                case "DataState":
                    return MySqlDbType.Int32;
                case "IsFreeze":
                    return MySqlDbType.Byte;
                case "AuthorID":
                    return MySqlDbType.Int32;
                case "AddDate":
                    return MySqlDbType.DateTime;
                case "LastReviserID":
                    return MySqlDbType.Int32;
                case "LastModifyDate":
                    return MySqlDbType.DateTime;
                case "AuditState":
                    return MySqlDbType.Int32;
                case "AuditorId":
                    return MySqlDbType.Int32;
                case "AuditDate":
                    return MySqlDbType.DateTime;
            }
            return MySqlDbType.VarChar;
        }


        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        private void CreateFullSqlParameter(PositionData entity, MySqlCommand cmd)
        {
            //02:标识(Id)
            cmd.Parameters.Add(new MySqlParameter("Id",MySqlDbType.Int32){ Value = entity.Id});
            //04:全称(FullName)
            var isNull = string.IsNullOrWhiteSpace(entity.FullName);
            var parameter = new MySqlParameter("FullName",MySqlDbType.VarString , isNull ? 10 : (entity.FullName).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.FullName;
            cmd.Parameters.Add(parameter);
            //05:简称(ShortName)
            isNull = string.IsNullOrWhiteSpace(entity.ShortName);
            parameter = new MySqlParameter("ShortName",MySqlDbType.VarString , isNull ? 10 : (entity.ShortName).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.ShortName;
            cmd.Parameters.Add(parameter);
            //06:级别(PositionLevel)
            cmd.Parameters.Add(new MySqlParameter("PositionLevel",MySqlDbType.Int32){ Value = entity.PositionLevel});
            //07:备注(Memo)
            isNull = string.IsNullOrWhiteSpace(entity.Memo);
            parameter = new MySqlParameter("Memo",MySqlDbType.Text , isNull ? 10 : (entity.Memo).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Memo;
            cmd.Parameters.Add(parameter);
            //13:数据状态(DataState)
            cmd.Parameters.Add(new MySqlParameter("DataState",MySqlDbType.Int32){ Value = (int)entity.DataState});
            //15:数据是否已冻结(IsFreeze)
            cmd.Parameters.Add(new MySqlParameter("IsFreeze",MySqlDbType.Byte) { Value = entity.IsFreeze ? (byte)1 : (byte)0 });
            //17:制作人(AuthorID)
            cmd.Parameters.Add(new MySqlParameter("AuthorID",MySqlDbType.Int32){ Value = entity.AuthorID});
            //19:制作时间(AddDate)
            isNull = entity.AddDate.Year < 1900;
            parameter = new MySqlParameter("AddDate",MySqlDbType.DateTime);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.AddDate;
            cmd.Parameters.Add(parameter);
            //21:最后修改者(LastReviserID)
            cmd.Parameters.Add(new MySqlParameter("LastReviserID",MySqlDbType.Int32){ Value = entity.LastReviserID});
            //23:最后修改日期(LastModifyDate)
            isNull = entity.LastModifyDate.Year < 1900;
            parameter = new MySqlParameter("LastModifyDate",MySqlDbType.DateTime);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.LastModifyDate;
            cmd.Parameters.Add(parameter);
            //25:审核状态(AuditState)
            cmd.Parameters.Add(new MySqlParameter("AuditState",MySqlDbType.Int32){ Value = (int)entity.AuditState});
            //27:审核人(AuditorId)
            cmd.Parameters.Add(new MySqlParameter("AuditorId",MySqlDbType.Int32){ Value = entity.AuditorId});
            //29:审核时间(AuditDate)
            isNull = entity.AuditDate.Year < 1900;
            parameter = new MySqlParameter("AuditDate",MySqlDbType.DateTime);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.AuditDate;
            cmd.Parameters.Add(parameter);
        }


        /// <summary>
        /// 设置更新数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        protected sealed override void SetUpdateCommand(PositionData entity, MySqlCommand cmd)
        {
            cmd.CommandText = UpdateSqlCode;
            CreateFullSqlParameter(entity,cmd);
        }


        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        protected sealed override bool SetInsertCommand(PositionData entity, MySqlCommand cmd)
        {
            cmd.CommandText = InsertSqlCode;
            CreateFullSqlParameter(entity, cmd);
            return true;
        }


        /// <summary>
        /// 构造一个缺省可用的数据库对象
        /// </summary>
        /// <returns></returns>
        protected override MySqlDataBase CreateDefaultDataBase()
        {
            return Authorities.Default ?? new Authorities();
        }
        
        /// <summary>
        /// 生成数据库访问范围
        /// </summary>
        internal static MySqlDataTableScope<PositionData> CreateScope()
        {
            var db = Authorities.Default ?? new Authorities();
            return MySqlDataTableScope<PositionData>.CreateScope(db, db.Positions);
        }
        #endregion

    }

    partial class Authorities
    {


        /// <summary>
        /// 职位字典(tb_sys_position):职位字典
        /// </summary>
        public const int Table_Position = 0x50002;


        /// <summary>
        /// 职位字典的结构语句
        /// </summary>
        private TableSql _tb_sys_positionSql = new TableSql
        {
            TableName = "tb_sys_position",
            PimaryKey = "Id"
        };


        /// <summary>
        /// 职位字典数据访问对象
        /// </summary>
        private PositionDataAccess _positions;

        /// <summary>
        /// 职位字典数据访问对象
        /// </summary>
        public PositionDataAccess Positions
        {
            get
            {
                return this._positions ?? ( this._positions = new PositionDataAccess{ DataBase = this});
            }
        }
    }
}