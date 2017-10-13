/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/6/27 17:50:40*/
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
    /// 区域,组织机构的分视图
    /// </summary>
    public partial class AreaDataAccess
    {

        #region 基本SQL语句

        /// <summary>
        /// 表的唯一标识
        /// </summary>
        public override int TableId
        {
            get { return Authorities.Table_Area; }
        }

        /// <summary>
        /// 读取表名
        /// </summary>
        protected sealed override string ReadTableName
        {
            get
            {
                return @"view_sys_area";
            }
        }

        /// <summary>
        /// 写入表名
        /// </summary>
        protected sealed override string WriteTableName
        {
            get
            {
                return @"view_sys_area";
            }
        }

        /// <summary>
        /// 主键
        /// </summary>
        protected sealed override string PrimaryKey
        {
            get
            {
                return @"AreaId";
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
    `id` AS `AreaId`,
    `type` AS `Type`,
    `code` AS `Code`,
    `full_name` AS `FullName`,
    `short_name` AS `ShortName`,
    `tree_name` AS `TreeName`,
    `org_level` AS `OrgLevel`,
    `parent_id` AS `ParentId`,
    `org_id` AS `OrgId`,
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
INSERT INTO `view_sys_area`
(
    `id`,
    `type`,
    `code`,
    `full_name`,
    `short_name`,
    `tree_name`,
    `org_level`,
    `parent_id`,
    `org_id`,
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
    ?AreaId,
    ?Type,
    ?Code,
    ?FullName,
    ?ShortName,
    ?TreeName,
    ?OrgLevel,
    ?ParentId,
    ?OrgId,
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
);";
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
UPDATE `view_sys_area` SET
       `id` = ?AreaId,
       `type` = ?Type,
       `code` = ?Code,
       `full_name` = ?FullName,
       `short_name` = ?ShortName,
       `tree_name` = ?TreeName,
       `org_level` = ?OrgLevel,
       `memo` = ?Memo,
       `data_state` = ?DataState,
       `is_freeze` = ?IsFreeze,
       `audit_state` = ?AuditState,
       `auditor_id` = ?AuditorId,
       `audit_date` = ?AuditDate
 WHERE `id` = ?AreaId;";
            }
        }

        /// <summary>
        /// 取得仅更新的SQL语句
        /// </summary>
        internal string GetModifiedSqlCode(AreaData data)
        {
            if (data.__EntityStatusNull || !data.__EntityStatus.IsModified)
                return ";";
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE `view_sys_area` SET");
            //区域标识
            if (data.__EntityStatus.ModifiedProperties[AreaData.Real_AreaId] > 0)
                sql.AppendLine("       `id` = ?AreaId");
            //机构类型
            if (data.__EntityStatus.ModifiedProperties[AreaData.Real_Type] > 0)
                sql.AppendLine("       `type` = ?Type");
            //编码
            if (data.__EntityStatus.ModifiedProperties[AreaData.Real_Code] > 0)
                sql.AppendLine("       `code` = ?Code");
            //全称
            if (data.__EntityStatus.ModifiedProperties[AreaData.Real_FullName] > 0)
                sql.AppendLine("       `full_name` = ?FullName");
            //简称
            if (data.__EntityStatus.ModifiedProperties[AreaData.Real_ShortName] > 0)
                sql.AppendLine("       `short_name` = ?ShortName");
            //树形名称
            if (data.__EntityStatus.ModifiedProperties[AreaData.Real_TreeName] > 0)
                sql.AppendLine("       `tree_name` = ?TreeName");
            //级别
            if (data.__EntityStatus.ModifiedProperties[AreaData.Real_OrgLevel] > 0)
                sql.AppendLine("       `org_level` = ?OrgLevel");
            //备注
            if (data.__EntityStatus.ModifiedProperties[AreaData.Real_Memo] > 0)
                sql.AppendLine("       `memo` = ?Memo");
            //数据状态
            if (data.__EntityStatus.ModifiedProperties[AreaData.Real_DataState] > 0)
                sql.AppendLine("       `data_state` = ?DataState");
            //数据是否已冻结
            if (data.__EntityStatus.ModifiedProperties[AreaData.Real_IsFreeze] > 0)
                sql.AppendLine("       `is_freeze` = ?IsFreeze");
            //制作人
            if (data.__EntityStatus.ModifiedProperties[AreaData.Real_AuthorID] > 0)
                sql.AppendLine("       `author_id` = ?AuthorID");
            //制作时间
            if (data.__EntityStatus.ModifiedProperties[AreaData.Real_AddDate] > 0)
                sql.AppendLine("       `add_date` = ?AddDate");
            //最后修改者
            if (data.__EntityStatus.ModifiedProperties[AreaData.Real_LastReviserID] > 0)
                sql.AppendLine("       `last_reviser_id` = ?LastReviserID");
            //最后修改日期
            if (data.__EntityStatus.ModifiedProperties[AreaData.Real_LastModifyDate] > 0)
                sql.AppendLine("       `last_modify_date` = ?LastModifyDate");
            //审核状态
            if (data.__EntityStatus.ModifiedProperties[AreaData.Real_AuditState] > 0)
                sql.AppendLine("       `audit_state` = ?AuditState");
            //审核人
            if (data.__EntityStatus.ModifiedProperties[AreaData.Real_AuditorId] > 0)
                sql.AppendLine("       `auditor_id` = ?AuditorId");
            //审核时间
            if (data.__EntityStatus.ModifiedProperties[AreaData.Real_AuditDate] > 0)
                sql.AppendLine("       `audit_date` = ?AuditDate");
            sql.Append(" WHERE `id` = ?AreaId;");
            return sql.ToString();
        }

        #endregion


        #region 字段

        /// <summary>
        ///  所有字段
        /// </summary>
        static string[] _fields = new string[]{ "AreaId","Type","Code","FullName","ShortName","TreeName","OrgLevel","ParentId","OrgId","Memo","DataState","IsFreeze","AuthorID","AddDate","LastReviserID","LastModifyDate","AuditState","AuditorId","AuditDate" };

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
            { "AreaId" , "id" },
            { "id" , "id" },
            { "Type" , "type" },
            { "Code" , "code" },
            { "FullName" , "full_name" },
            { "full_name" , "full_name" },
            { "ShortName" , "short_name" },
            { "short_name" , "short_name" },
            { "TreeName" , "tree_name" },
            { "tree_name" , "tree_name" },
            { "OrgLevel" , "org_level" },
            { "org_level" , "org_level" },
            { "ParentId" , "parent_id" },
            { "parent_id" , "parent_id" },
            { "OrgId" , "org_id" },
            { "org_id" , "org_id" },
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
        protected sealed override void LoadEntity(MySqlDataReader reader,AreaData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._areaid = (int)reader.GetInt32(0);
                if (!reader.IsDBNull(1))
                    entity._type = (OrganizationType)reader.GetInt32(1);
                if (!reader.IsDBNull(2))
                    entity._code = reader.GetString(2).ToString();
                if (!reader.IsDBNull(3))
                    entity._fullname = reader.GetString(3).ToString();
                if (!reader.IsDBNull(4))
                    entity._shortname = reader.GetString(4).ToString();
                if (!reader.IsDBNull(5))
                    entity._treename = reader.GetString(5).ToString();
                entity._orglevel = (int)reader.GetInt32(6);
                entity._parentid = (int)reader.GetInt32(7);
                entity._orgid = (int)reader.GetInt32(8);
                if (!reader.IsDBNull(9))
                    entity._memo = reader.GetString(9).ToString();
                if (!reader.IsDBNull(10))
                    entity._datastate = (DataStateType)reader.GetInt32(10);
                entity._isfreeze = (bool)reader.GetBoolean(11);
                entity._authorid = (int)reader.GetInt32(12);
                if (!reader.IsDBNull(13))
                    try{entity._adddate = reader.GetMySqlDateTime(13).Value;}catch{}
                entity._lastreviserid = (int)reader.GetInt32(14);
                if (!reader.IsDBNull(15))
                    try{entity._lastmodifydate = reader.GetMySqlDateTime(15).Value;}catch{}
                if (!reader.IsDBNull(16))
                    entity._auditstate = (AuditStateType)reader.GetInt32(16);
                entity._auditorid = (int)reader.GetInt32(17);
                if (!reader.IsDBNull(18))
                    try{entity._auditdate = reader.GetMySqlDateTime(18).Value;}catch{}
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
                case "AreaId":
                    return MySqlDbType.Int32;
                case "Type":
                    return MySqlDbType.Int32;
                case "Code":
                    return MySqlDbType.VarString;
                case "FullName":
                    return MySqlDbType.VarString;
                case "ShortName":
                    return MySqlDbType.VarString;
                case "TreeName":
                    return MySqlDbType.VarString;
                case "OrgLevel":
                    return MySqlDbType.Int32;
                case "ParentId":
                    return MySqlDbType.Int32;
                case "OrgId":
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
        private void CreateFullSqlParameter(AreaData entity, MySqlCommand cmd)
        {
            //02:区域标识(AreaId)
            cmd.Parameters.Add(new MySqlParameter("AreaId",MySqlDbType.Int32){ Value = entity.AreaId});
            //04:机构类型(Type)
            cmd.Parameters.Add(new MySqlParameter("Type",MySqlDbType.Int32){ Value = (int)entity.Type});
            //05:编码(Code)
            var isNull = string.IsNullOrWhiteSpace(entity.Code);
            var parameter = new MySqlParameter("Code",MySqlDbType.VarString , isNull ? 10 : (entity.Code).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Code;
            cmd.Parameters.Add(parameter);
            //06:全称(FullName)
            isNull = string.IsNullOrWhiteSpace(entity.FullName);
            parameter = new MySqlParameter("FullName",MySqlDbType.VarString , isNull ? 10 : (entity.FullName).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.FullName;
            cmd.Parameters.Add(parameter);
            //07:简称(ShortName)
            isNull = string.IsNullOrWhiteSpace(entity.ShortName);
            parameter = new MySqlParameter("ShortName",MySqlDbType.VarString , isNull ? 10 : (entity.ShortName).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.ShortName;
            cmd.Parameters.Add(parameter);
            //08:树形名称(TreeName)
            isNull = string.IsNullOrWhiteSpace(entity.TreeName);
            parameter = new MySqlParameter("TreeName",MySqlDbType.VarString , isNull ? 10 : (entity.TreeName).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.TreeName;
            cmd.Parameters.Add(parameter);
            //09:级别(OrgLevel)
            cmd.Parameters.Add(new MySqlParameter("OrgLevel",MySqlDbType.Int32){ Value = entity.OrgLevel});
            //10:上级标识(ParentId)
            cmd.Parameters.Add(new MySqlParameter("ParentId",MySqlDbType.Int32){ Value = entity.ParentId});
            //11:机构标识(OrgId)
            cmd.Parameters.Add(new MySqlParameter("OrgId",MySqlDbType.Int32){ Value = entity.OrgId});
            //12:备注(Memo)
            isNull = string.IsNullOrWhiteSpace(entity.Memo);
            parameter = new MySqlParameter("Memo",MySqlDbType.Text , isNull ? 10 : (entity.Memo).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Memo;
            cmd.Parameters.Add(parameter);
            //63:数据状态(DataState)
            cmd.Parameters.Add(new MySqlParameter("DataState",MySqlDbType.Int32){ Value = (int)entity.DataState});
            //64:数据是否已冻结(IsFreeze)
            cmd.Parameters.Add(new MySqlParameter("IsFreeze",MySqlDbType.Byte) { Value = entity.IsFreeze ? (byte)1 : (byte)0 });
            //66:制作人(AuthorID)
            cmd.Parameters.Add(new MySqlParameter("AuthorID",MySqlDbType.Int32){ Value = entity.AuthorID});
            //68:制作时间(AddDate)
            isNull = entity.AddDate.Year < 1900;
            parameter = new MySqlParameter("AddDate",MySqlDbType.DateTime);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.AddDate;
            cmd.Parameters.Add(parameter);
            //70:最后修改者(LastReviserID)
            cmd.Parameters.Add(new MySqlParameter("LastReviserID",MySqlDbType.Int32){ Value = entity.LastReviserID});
            //72:最后修改日期(LastModifyDate)
            isNull = entity.LastModifyDate.Year < 1900;
            parameter = new MySqlParameter("LastModifyDate",MySqlDbType.DateTime);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.LastModifyDate;
            cmd.Parameters.Add(parameter);
            //74:审核状态(AuditState)
            cmd.Parameters.Add(new MySqlParameter("AuditState",MySqlDbType.Int32){ Value = (int)entity.AuditState});
            //76:审核人(AuditorId)
            cmd.Parameters.Add(new MySqlParameter("AuditorId",MySqlDbType.Int32){ Value = entity.AuditorId});
            //78:审核时间(AuditDate)
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
        protected sealed override void SetUpdateCommand(AreaData entity, MySqlCommand cmd)
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
        protected sealed override bool SetInsertCommand(AreaData entity, MySqlCommand cmd)
        {
            cmd.CommandText = InsertSqlCode;
            CreateFullSqlParameter(entity, cmd);
            return false;
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
        internal static MySqlDataTableScope<AreaData> CreateScope()
        {
            var db = Authorities.Default ?? new Authorities();
            return MySqlDataTableScope<AreaData>.CreateScope(db, db.Areas);
        }
        #endregion

        #region 简单读取

        /// <summary>
        /// SQL语句
        /// </summary>
        public override string SimpleFields
        {
            get
            {
                return @"
    `id` AS `AreaId`,
    `type` AS `Type`,
    `code` AS `Code`,
    `full_name` AS `FullName`,
    `short_name` AS `ShortName`,
    `tree_name` AS `TreeName`,
    `org_level` AS `OrgLevel`,
    `parent_id` AS `ParentId`,
    `org_id` AS `OrgId`,
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
        /// 载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public override void SimpleLoad(MySqlDataReader reader,AreaData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._areaid = (int)reader.GetInt32(0);
                if (!reader.IsDBNull(1))
                    entity._type = (OrganizationType)reader.GetInt32(1);
                if (!reader.IsDBNull(2))
                    entity._code = reader.GetString(2).ToString();
                if (!reader.IsDBNull(3))
                    entity._fullname = reader.GetString(3).ToString();
                if (!reader.IsDBNull(4))
                    entity._shortname = reader.GetString(4).ToString();
                if (!reader.IsDBNull(5))
                    entity._treename = reader.GetString(5).ToString();
                entity._orglevel = (int)reader.GetInt32(6);
                entity._parentid = (int)reader.GetInt32(7);
                entity._orgid = (int)reader.GetInt32(8);
                if (!reader.IsDBNull(9))
                    entity._memo = reader.GetString(9).ToString();
                if (!reader.IsDBNull(10))
                    entity._datastate = (DataStateType)reader.GetInt32(10);
                entity._isfreeze = (bool)reader.GetBoolean(11);
                entity._authorid = (int)reader.GetInt32(12);
                if (!reader.IsDBNull(13))
                    try{entity._adddate = reader.GetMySqlDateTime(13).Value;}catch{}
                entity._lastreviserid = (int)reader.GetInt32(14);
                if (!reader.IsDBNull(15))
                    try{entity._lastmodifydate = reader.GetMySqlDateTime(15).Value;}catch{}
                if (!reader.IsDBNull(16))
                    entity._auditstate = (AuditStateType)reader.GetInt32(16);
                entity._auditorid = (int)reader.GetInt32(17);
                if (!reader.IsDBNull(18))
                    try{entity._auditdate = reader.GetMySqlDateTime(18).Value;}catch{}
            }
        }
        #endregion

    }

    partial class Authorities
    {


        /// <summary>
        /// 区域(view_sys_area):区域,组织机构的分视图
        /// </summary>
        public const int Table_Area = 0x5000a;


        /// <summary>
        /// 区域,组织机构的分视图的结构语句
        /// </summary>
        private TableSql _view_sys_areaSql = new TableSql
        {
            TableName = "view_sys_area",
            PimaryKey = "AreaId"
        };


        /// <summary>
        /// 区域,组织机构的分视图数据访问对象
        /// </summary>
        private AreaDataAccess _areas;

        /// <summary>
        /// 区域,组织机构的分视图数据访问对象
        /// </summary>
        public AreaDataAccess Areas
        {
            get
            {
                return this._areas ?? ( this._areas = new AreaDataAccess{ DataBase = this});
            }
        }
    }
}