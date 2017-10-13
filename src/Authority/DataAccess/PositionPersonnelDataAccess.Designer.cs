/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/9/16 22:24:35*/
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
    /// 人员职位设置
    /// </summary>
    public partial class PositionPersonnelDataAccess
    {

        #region 基本SQL语句

        /// <summary>
        /// 表的唯一标识
        /// </summary>
        public override int TableId
        {
            get { return Authorities.Table_PositionPersonnel; }
        }

        /// <summary>
        /// 读取表名
        /// </summary>
        protected sealed override string ReadTableName
        {
            get
            {
                return @"view_sys_position_personnel";
            }
        }

        /// <summary>
        /// 写入表名
        /// </summary>
        protected sealed override string WriteTableName
        {
            get
            {
                return @"tb_sys_position_personnel";
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
    `personnel_id` AS `PersonnelId`,
    `personnel` AS `Personnel`,
    `appellation` AS `Appellation`,
    `organize_position_id` AS `OrganizePositionId`,
    `position` AS `Position`,
    `role_id` AS `RoleId`,
    `role` AS `Role`,
    `six` AS `Six`,
    `birthday` AS `Birthday`,
    `tel` AS `Tel`,
    `mobile` AS `Mobile`,
    `organization_id` AS `OrganizationId`,
    `organization` AS `Organization`,
    `department_id` AS `DepartmentId`,
    `department` AS `Department`,
    `memo` AS `Memo`,
    `data_state` AS `DataState`,
    `is_freeze` AS `IsFreeze`,
    `author_id` AS `AuthorID`,
    `add_date` AS `AddDate`,
    `last_reviser_id` AS `LastReviserID`,
    `last_modify_date` AS `LastModifyDate`,
    `audit_state` AS `AuditState`,
    `auditor_id` AS `AuditorId`,
    `audit_date` AS `AuditDate`,
    `user_id` AS `UserId`,
    `org_level` AS `OrgLevel`";
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
INSERT INTO `tb_sys_position_personnel`
(
    `personnel_id`,
    `appellation`,
    `organize_position_id`,
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
    ?PersonnelId,
    ?Appellation,
    ?OrganizePositionId,
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
UPDATE `tb_sys_position_personnel` SET
       `personnel_id` = ?PersonnelId,
       `appellation` = ?Appellation,
       `organize_position_id` = ?OrganizePositionId,
       `memo` = ?Memo,
       `data_state` = ?DataState,
       `is_freeze` = ?IsFreeze,
       `audit_state` = ?AuditState,
       `auditor_id` = ?AuditorId,
       `audit_date` = ?AuditDate
 WHERE `id` = ?Id;";
            }
        }

        /// <summary>
        /// 取得仅更新的SQL语句
        /// </summary>
        internal string GetModifiedSqlCode(PositionPersonnelData data)
        {
            if (data.__EntityStatusNull || !data.__EntityStatus.IsModified)
                return ";";
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE `tb_sys_position_personnel` SET");
            //员工标识
            if (data.__EntityStatus.ModifiedProperties[PositionPersonnelData.Real_PersonnelId] > 0)
                sql.AppendLine("       `personnel_id` = ?PersonnelId");
            //称谓
            if (data.__EntityStatus.ModifiedProperties[PositionPersonnelData.Real_Appellation] > 0)
                sql.AppendLine("       `appellation` = ?Appellation");
            //职位标识
            if (data.__EntityStatus.ModifiedProperties[PositionPersonnelData.Real_OrganizePositionId] > 0)
                sql.AppendLine("       `organize_position_id` = ?OrganizePositionId");
            //备注
            if (data.__EntityStatus.ModifiedProperties[PositionPersonnelData.Real_Memo] > 0)
                sql.AppendLine("       `memo` = ?Memo");
            //数据状态
            if (data.__EntityStatus.ModifiedProperties[PositionPersonnelData.Real_DataState] > 0)
                sql.AppendLine("       `data_state` = ?DataState");
            //数据是否已冻结
            if (data.__EntityStatus.ModifiedProperties[PositionPersonnelData.Real_IsFreeze] > 0)
                sql.AppendLine("       `is_freeze` = ?IsFreeze");
            //制作人
            if (data.__EntityStatus.ModifiedProperties[PositionPersonnelData.Real_AuthorID] > 0)
                sql.AppendLine("       `author_id` = ?AuthorID");
            //制作时间
            if (data.__EntityStatus.ModifiedProperties[PositionPersonnelData.Real_AddDate] > 0)
                sql.AppendLine("       `add_date` = ?AddDate");
            //最后修改者
            if (data.__EntityStatus.ModifiedProperties[PositionPersonnelData.Real_LastReviserID] > 0)
                sql.AppendLine("       `last_reviser_id` = ?LastReviserID");
            //最后修改日期
            if (data.__EntityStatus.ModifiedProperties[PositionPersonnelData.Real_LastModifyDate] > 0)
                sql.AppendLine("       `last_modify_date` = ?LastModifyDate");
            //审核状态
            if (data.__EntityStatus.ModifiedProperties[PositionPersonnelData.Real_AuditState] > 0)
                sql.AppendLine("       `audit_state` = ?AuditState");
            //审核人
            if (data.__EntityStatus.ModifiedProperties[PositionPersonnelData.Real_AuditorId] > 0)
                sql.AppendLine("       `auditor_id` = ?AuditorId");
            //审核时间
            if (data.__EntityStatus.ModifiedProperties[PositionPersonnelData.Real_AuditDate] > 0)
                sql.AppendLine("       `audit_date` = ?AuditDate");
            sql.Append(" WHERE `id` = ?Id;");
            return sql.ToString();
        }

        #endregion


        #region 字段

        /// <summary>
        ///  所有字段
        /// </summary>
        static string[] _fields = new string[]{ "Id","PersonnelId","Personnel","Appellation","OrganizePositionId","Position","RoleId","Role","Six","Birthday","Tel","Mobile","OrganizationId","Organization","DepartmentId","Department","Memo","DataState","IsFreeze","AuthorID","AddDate","LastReviserID","LastModifyDate","AuditState","AuditorId","AuditDate","master_id","UserId","OrgLevel" };

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
            { "PersonnelId" , "personnel_id" },
            { "personnel_id" , "personnel_id" },
            { "Personnel" , "personnel" },
            { "Appellation" , "appellation" },
            { "OrganizePositionId" , "organize_position_id" },
            { "organize_position_id" , "organize_position_id" },
            { "Position" , "position" },
            { "RoleId" , "role_id" },
            { "role_id" , "role_id" },
            { "Role" , "role" },
            { "Six" , "six" },
            { "Birthday" , "birthday" },
            { "Tel" , "tel" },
            { "Mobile" , "mobile" },
            { "OrganizationId" , "organization_id" },
            { "organization_id" , "organization_id" },
            { "Organization" , "organization" },
            { "DepartmentId" , "department_id" },
            { "department_id" , "department_id" },
            { "Department" , "department" },
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
            { "audit_date" , "audit_date" },
            { "master_id" , "master_id" },
            { "UserId" , "user_id" },
            { "user_id" , "user_id" },
            { "OrgLevel" , "org_level" },
            { "org_level" , "org_level" }
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
        protected sealed override void LoadEntity(MySqlDataReader reader,PositionPersonnelData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._id = (int)reader.GetInt32(0);
                entity._personnelid = (int)reader.GetInt32(1);
                if (!reader.IsDBNull(2))
                    entity._personnel = reader.GetString(2).ToString();
                if (!reader.IsDBNull(3))
                    entity._appellation = reader.GetString(3).ToString();
                entity._organizepositionid = (int)reader.GetInt32(4);
                if (!reader.IsDBNull(5))
                    entity._position = reader.GetString(5).ToString();
                if (!reader.IsDBNull(6))
                    entity._roleid = (int)reader.GetInt32(6);
                if (!reader.IsDBNull(7))
                    entity._role = reader.GetString(7).ToString();
                if (!reader.IsDBNull(8))
                    entity._six = (bool)reader.GetBoolean(8);
                if (!reader.IsDBNull(9))
                    try{entity._birthday = reader.GetMySqlDateTime(9).Value;}catch{}
                if (!reader.IsDBNull(10))
                    entity._tel = reader.GetString(10).ToString();
                if (!reader.IsDBNull(11))
                    entity._mobile = reader.GetString(11).ToString();
                if (!reader.IsDBNull(12))
                    entity._organizationid = (int)reader.GetInt32(12);
                if (!reader.IsDBNull(13))
                    entity._organization = reader.GetString(13).ToString();
                if (!reader.IsDBNull(14))
                    entity._departmentid = (int)reader.GetInt32(14);
                if (!reader.IsDBNull(15))
                    entity._department = reader.GetString(15).ToString();
                if (!reader.IsDBNull(16))
                    entity._memo = reader.GetString(16).ToString();
                if (!reader.IsDBNull(17))
                    entity._datastate = (DataStateType)reader.GetInt32(17);
                if (!reader.IsDBNull(18))
                    entity._isfreeze = (bool)reader.GetBoolean(18);
                if (!reader.IsDBNull(19))
                    entity._authorid = (int)reader.GetInt32(19);
                if (!reader.IsDBNull(20))
                    try{entity._adddate = reader.GetMySqlDateTime(20).Value;}catch{}
                if (!reader.IsDBNull(21))
                    entity._lastreviserid = (int)reader.GetInt32(21);
                if (!reader.IsDBNull(22))
                    try{entity._lastmodifydate = reader.GetMySqlDateTime(22).Value;}catch{}
                if (!reader.IsDBNull(23))
                    entity._auditstate = (AuditStateType)reader.GetInt32(23);
                if (!reader.IsDBNull(24))
                    entity._auditorid = (int)reader.GetInt32(24);
                if (!reader.IsDBNull(25))
                    try{entity._auditdate = reader.GetMySqlDateTime(25).Value;}catch{}
                if (!reader.IsDBNull(26))
                    entity._userid = (int)reader.GetInt32(26);
                if (!reader.IsDBNull(27))
                    entity._orglevel = (int)reader.GetInt32(27);
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
                case "PersonnelId":
                    return MySqlDbType.Int32;
                case "Personnel":
                    return MySqlDbType.VarString;
                case "Appellation":
                    return MySqlDbType.VarString;
                case "OrganizePositionId":
                    return MySqlDbType.Int32;
                case "Position":
                    return MySqlDbType.VarString;
                case "RoleId":
                    return MySqlDbType.Int32;
                case "Role":
                    return MySqlDbType.VarString;
                case "Six":
                    return MySqlDbType.Byte;
                case "Birthday":
                    return MySqlDbType.DateTime;
                case "Tel":
                    return MySqlDbType.VarString;
                case "Mobile":
                    return MySqlDbType.VarString;
                case "OrganizationId":
                    return MySqlDbType.Int32;
                case "Organization":
                    return MySqlDbType.VarString;
                case "DepartmentId":
                    return MySqlDbType.Int32;
                case "Department":
                    return MySqlDbType.VarString;
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
                case "UserId":
                    return MySqlDbType.Int32;
                case "OrgLevel":
                    return MySqlDbType.Int32;
            }
            return MySqlDbType.VarChar;
        }


        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        private void CreateFullSqlParameter(PositionPersonnelData entity, MySqlCommand cmd)
        {
            //02:标识(Id)
            cmd.Parameters.Add(new MySqlParameter("Id",MySqlDbType.Int32){ Value = entity.Id});
            //03:员工标识(PersonnelId)
            cmd.Parameters.Add(new MySqlParameter("PersonnelId",MySqlDbType.Int32){ Value = entity.PersonnelId});
            //04:职员(Personnel)
            var isNull = string.IsNullOrWhiteSpace(entity.Personnel);
            var parameter = new MySqlParameter("Personnel",MySqlDbType.VarString , isNull ? 10 : (entity.Personnel).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Personnel;
            cmd.Parameters.Add(parameter);
            //05:称谓(Appellation)
            isNull = string.IsNullOrWhiteSpace(entity.Appellation);
            parameter = new MySqlParameter("Appellation",MySqlDbType.VarString , isNull ? 10 : (entity.Appellation).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Appellation;
            cmd.Parameters.Add(parameter);
            //06:职位标识(OrganizePositionId)
            cmd.Parameters.Add(new MySqlParameter("OrganizePositionId",MySqlDbType.Int32){ Value = entity.OrganizePositionId});
            //07:职位(Position)
            isNull = string.IsNullOrWhiteSpace(entity.Position);
            parameter = new MySqlParameter("Position",MySqlDbType.VarString , isNull ? 10 : (entity.Position).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Position;
            cmd.Parameters.Add(parameter);
            //08:角色标识(RoleId)
            cmd.Parameters.Add(new MySqlParameter("RoleId",MySqlDbType.Int32){ Value = entity.RoleId});
            //09:角色(Role)
            isNull = string.IsNullOrWhiteSpace(entity.Role);
            parameter = new MySqlParameter("Role",MySqlDbType.VarString , isNull ? 10 : (entity.Role).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Role;
            cmd.Parameters.Add(parameter);
            //10:性别(Six)
            cmd.Parameters.Add(new MySqlParameter("Six",MySqlDbType.Byte) { Value = entity.Six ? (byte)1 : (byte)0 });
            //11:生日(Birthday)
            isNull = entity.Birthday.Year < 1900;
            parameter = new MySqlParameter("Birthday",MySqlDbType.DateTime);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Birthday;
            cmd.Parameters.Add(parameter);
            //12:电话(Tel)
            isNull = string.IsNullOrWhiteSpace(entity.Tel);
            parameter = new MySqlParameter("Tel",MySqlDbType.VarString , isNull ? 10 : (entity.Tel).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Tel;
            cmd.Parameters.Add(parameter);
            //13:手机(Mobile)
            isNull = string.IsNullOrWhiteSpace(entity.Mobile);
            parameter = new MySqlParameter("Mobile",MySqlDbType.VarString , isNull ? 10 : (entity.Mobile).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Mobile;
            cmd.Parameters.Add(parameter);
            //14:机构标识(OrganizationId)
            cmd.Parameters.Add(new MySqlParameter("OrganizationId",MySqlDbType.Int32){ Value = entity.OrganizationId});
            //15:所在机构(Organization)
            isNull = string.IsNullOrWhiteSpace(entity.Organization);
            parameter = new MySqlParameter("Organization",MySqlDbType.VarString , isNull ? 10 : (entity.Organization).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Organization;
            cmd.Parameters.Add(parameter);
            //16:部门外键(DepartmentId)
            cmd.Parameters.Add(new MySqlParameter("DepartmentId",MySqlDbType.Int32){ Value = entity.DepartmentId});
            //17:部门(Department)
            isNull = string.IsNullOrWhiteSpace(entity.Department);
            parameter = new MySqlParameter("Department",MySqlDbType.VarString , isNull ? 10 : (entity.Department).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Department;
            cmd.Parameters.Add(parameter);
            //18:备注(Memo)
            isNull = string.IsNullOrWhiteSpace(entity.Memo);
            parameter = new MySqlParameter("Memo",MySqlDbType.Text , isNull ? 10 : (entity.Memo).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Memo;
            cmd.Parameters.Add(parameter);
            //19:数据状态(DataState)
            cmd.Parameters.Add(new MySqlParameter("DataState",MySqlDbType.Int32){ Value = (int)entity.DataState});
            //20:数据是否已冻结(IsFreeze)
            cmd.Parameters.Add(new MySqlParameter("IsFreeze",MySqlDbType.Byte) { Value = entity.IsFreeze ? (byte)1 : (byte)0 });
            //21:制作人(AuthorID)
            cmd.Parameters.Add(new MySqlParameter("AuthorID",MySqlDbType.Int32){ Value = entity.AuthorID});
            //22:制作时间(AddDate)
            isNull = entity.AddDate.Year < 1900;
            parameter = new MySqlParameter("AddDate",MySqlDbType.DateTime);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.AddDate;
            cmd.Parameters.Add(parameter);
            //23:最后修改者(LastReviserID)
            cmd.Parameters.Add(new MySqlParameter("LastReviserID",MySqlDbType.Int32){ Value = entity.LastReviserID});
            //24:最后修改日期(LastModifyDate)
            isNull = entity.LastModifyDate.Year < 1900;
            parameter = new MySqlParameter("LastModifyDate",MySqlDbType.DateTime);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.LastModifyDate;
            cmd.Parameters.Add(parameter);
            //25:审核状态(AuditState)
            cmd.Parameters.Add(new MySqlParameter("AuditState",MySqlDbType.Int32){ Value = (int)entity.AuditState});
            //26:审核人(AuditorId)
            cmd.Parameters.Add(new MySqlParameter("AuditorId",MySqlDbType.Int32){ Value = entity.AuditorId});
            //27:审核时间(AuditDate)
            isNull = entity.AuditDate.Year < 1900;
            parameter = new MySqlParameter("AuditDate",MySqlDbType.DateTime);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.AuditDate;
            cmd.Parameters.Add(parameter);
            //29:系统用户外键(UserId)
            cmd.Parameters.Add(new MySqlParameter("UserId",MySqlDbType.Int32){ Value = entity.UserId});
            //30:级别(OrgLevel)
            cmd.Parameters.Add(new MySqlParameter("OrgLevel",MySqlDbType.Int32){ Value = entity.OrgLevel});
        }


        /// <summary>
        /// 设置更新数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        protected sealed override void SetUpdateCommand(PositionPersonnelData entity, MySqlCommand cmd)
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
        protected sealed override bool SetInsertCommand(PositionPersonnelData entity, MySqlCommand cmd)
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
        internal static MySqlDataTableScope<PositionPersonnelData> CreateScope()
        {
            var db = Authorities.Default ?? new Authorities();
            return MySqlDataTableScope<PositionPersonnelData>.CreateScope(db, db.PositionPersonnels);
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
    `id` AS `Id`,
    `personnel` AS `Personnel`,
    `appellation` AS `Appellation`,
    `position` AS `Position`,
    `role` AS `Role`,
    `six` AS `Six`,
    `birthday` AS `Birthday`,
    `tel` AS `Tel`,
    `mobile` AS `Mobile`,
    `organization` AS `Organization`,
    `department` AS `Department`,
    `memo` AS `Memo`,
    `data_state` AS `DataState`,
    `is_freeze` AS `IsFreeze`,
    `audit_state` AS `AuditState`";
            }
        }


        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public override void SimpleLoad(MySqlDataReader reader,PositionPersonnelData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._id = (int)reader.GetInt32(0);
                if (!reader.IsDBNull(1))
                    entity._personnel = reader.GetString(1).ToString();
                if (!reader.IsDBNull(2))
                    entity._appellation = reader.GetString(2).ToString();
                if (!reader.IsDBNull(3))
                    entity._position = reader.GetString(3).ToString();
                if (!reader.IsDBNull(4))
                    entity._role = reader.GetString(4).ToString();
                if (!reader.IsDBNull(5))
                    entity._six = (bool)reader.GetBoolean(5);
                if (!reader.IsDBNull(6))
                    try{entity._birthday = reader.GetMySqlDateTime(6).Value;}catch{}
                if (!reader.IsDBNull(7))
                    entity._tel = reader.GetString(7).ToString();
                if (!reader.IsDBNull(8))
                    entity._mobile = reader.GetString(8).ToString();
                if (!reader.IsDBNull(9))
                    entity._organization = reader.GetString(9).ToString();
                if (!reader.IsDBNull(10))
                    entity._department = reader.GetString(10).ToString();
                if (!reader.IsDBNull(11))
                    entity._memo = reader.GetString(11).ToString();
                if (!reader.IsDBNull(12))
                    entity._datastate = (DataStateType)reader.GetInt32(12);
                if (!reader.IsDBNull(13))
                    entity._isfreeze = (bool)reader.GetBoolean(13);
                if (!reader.IsDBNull(14))
                    entity._auditstate = (AuditStateType)reader.GetInt32(14);
            }
        }
        #endregion

    }

    partial class Authorities
    {


        /// <summary>
        /// 人员职位设置(view_sys_position_personnel):人员职位设置
        /// </summary>
        public const int Table_PositionPersonnel = 0x50006;


        /// <summary>
        /// 人员职位设置的结构语句
        /// </summary>
        private TableSql _view_sys_position_personnelSql = new TableSql
        {
            TableName = "view_sys_position_personnel",
            PimaryKey = "Id"
        };


        /// <summary>
        /// 人员职位设置数据访问对象
        /// </summary>
        private PositionPersonnelDataAccess _positionPersonnels;

        /// <summary>
        /// 人员职位设置数据访问对象
        /// </summary>
        public PositionPersonnelDataAccess PositionPersonnels
        {
            get
            {
                return this._positionPersonnels ?? ( this._positionPersonnels = new PositionPersonnelDataAccess{ DataBase = this});
            }
        }
    }
}