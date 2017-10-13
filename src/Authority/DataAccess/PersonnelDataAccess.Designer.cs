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
    /// 职员
    /// </summary>
    public partial class PersonnelDataAccess
    {

        #region 基本SQL语句

        /// <summary>
        /// 表的唯一标识
        /// </summary>
        public override int TableId
        {
            get { return Authorities.Table_Personnel; }
        }

        /// <summary>
        /// 读取表名
        /// </summary>
        protected sealed override string ReadTableName
        {
            get
            {
                return @"view_sys_personnel";
            }
        }

        /// <summary>
        /// 写入表名
        /// </summary>
        protected sealed override string WriteTableName
        {
            get
            {
                return @"tb_sys_personnel";
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
    `six` AS `Six`,
    `birthday` AS `Birthday`,
    `tel` AS `Tel`,
    `mobile` AS `Mobile`,
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
    `role_id` AS `RoleId`,
    `role` AS `Role`,
    `user_id` AS `UserId`";
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
INSERT INTO `tb_sys_personnel`
(
    `full_name`,
    `six`,
    `birthday`,
    `tel`,
    `mobile`,
    `memo`,
    `data_state`,
    `is_freeze`,
    `author_id`,
    `add_date`,
    `last_reviser_id`,
    `last_modify_date`,
    `audit_state`,
    `auditor_id`,
    `audit_date`,
    `role_id`,
    `user_id`
)
VALUES
(
    ?FullName,
    ?Six,
    ?Birthday,
    ?Tel,
    ?Mobile,
    ?Memo,
    ?DataState,
    ?IsFreeze,
    ?AuthorID,
    ?AddDate,
    ?LastReviserID,
    ?LastModifyDate,
    ?AuditState,
    ?AuditorId,
    ?AuditDate,
    ?RoleId,
    ?UserId
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
UPDATE `tb_sys_personnel` SET
       `full_name` = ?FullName,
       `six` = ?Six,
       `birthday` = ?Birthday,
       `tel` = ?Tel,
       `mobile` = ?Mobile,
       `memo` = ?Memo,
       `data_state` = ?DataState,
       `is_freeze` = ?IsFreeze,
       `audit_state` = ?AuditState,
       `auditor_id` = ?AuditorId,
       `audit_date` = ?AuditDate,
       `role_id` = ?RoleId,
       `user_id` = ?UserId
 WHERE `id` = ?Id;";
            }
        }

        /// <summary>
        /// 取得仅更新的SQL语句
        /// </summary>
        internal string GetModifiedSqlCode(PersonnelData data)
        {
            if (data.__EntityStatusNull || !data.__EntityStatus.IsModified)
                return ";";
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE `tb_sys_personnel` SET");
            //姓名
            if (data.__EntityStatus.ModifiedProperties[PersonnelData.Real_FullName] > 0)
                sql.AppendLine("       `full_name` = ?FullName");
            //性别
            if (data.__EntityStatus.ModifiedProperties[PersonnelData.Real_Six] > 0)
                sql.AppendLine("       `six` = ?Six");
            //生日
            if (data.__EntityStatus.ModifiedProperties[PersonnelData.Real_Birthday] > 0)
                sql.AppendLine("       `birthday` = ?Birthday");
            //电话
            if (data.__EntityStatus.ModifiedProperties[PersonnelData.Real_Tel] > 0)
                sql.AppendLine("       `tel` = ?Tel");
            //手机
            if (data.__EntityStatus.ModifiedProperties[PersonnelData.Real_Mobile] > 0)
                sql.AppendLine("       `mobile` = ?Mobile");
            //备注
            if (data.__EntityStatus.ModifiedProperties[PersonnelData.Real_Memo] > 0)
                sql.AppendLine("       `memo` = ?Memo");
            //数据状态
            if (data.__EntityStatus.ModifiedProperties[PersonnelData.Real_DataState] > 0)
                sql.AppendLine("       `data_state` = ?DataState");
            //数据是否已冻结
            if (data.__EntityStatus.ModifiedProperties[PersonnelData.Real_IsFreeze] > 0)
                sql.AppendLine("       `is_freeze` = ?IsFreeze");
            //制作人
            if (data.__EntityStatus.ModifiedProperties[PersonnelData.Real_AuthorID] > 0)
                sql.AppendLine("       `author_id` = ?AuthorID");
            //制作时间
            if (data.__EntityStatus.ModifiedProperties[PersonnelData.Real_AddDate] > 0)
                sql.AppendLine("       `add_date` = ?AddDate");
            //最后修改者
            if (data.__EntityStatus.ModifiedProperties[PersonnelData.Real_LastReviserID] > 0)
                sql.AppendLine("       `last_reviser_id` = ?LastReviserID");
            //最后修改日期
            if (data.__EntityStatus.ModifiedProperties[PersonnelData.Real_LastModifyDate] > 0)
                sql.AppendLine("       `last_modify_date` = ?LastModifyDate");
            //审核状态
            if (data.__EntityStatus.ModifiedProperties[PersonnelData.Real_AuditState] > 0)
                sql.AppendLine("       `audit_state` = ?AuditState");
            //审核人
            if (data.__EntityStatus.ModifiedProperties[PersonnelData.Real_AuditorId] > 0)
                sql.AppendLine("       `auditor_id` = ?AuditorId");
            //审核时间
            if (data.__EntityStatus.ModifiedProperties[PersonnelData.Real_AuditDate] > 0)
                sql.AppendLine("       `audit_date` = ?AuditDate");
            //角色外键
            if (data.__EntityStatus.ModifiedProperties[PersonnelData.Real_RoleId] > 0)
                sql.AppendLine("       `role_id` = ?RoleId");
            //系统用户外键
            if (data.__EntityStatus.ModifiedProperties[PersonnelData.Real_UserId] > 0)
                sql.AppendLine("       `user_id` = ?UserId");
            sql.Append(" WHERE `id` = ?Id;");
            return sql.ToString();
        }

        #endregion


        #region 字段

        /// <summary>
        ///  所有字段
        /// </summary>
        static string[] _fields = new string[]{ "Id","FullName","Six","Birthday","Tel","Mobile","Memo","DataState","IsFreeze","AuthorID","AddDate","LastReviserID","LastModifyDate","AuditState","AuditorId","AuditDate","RoleId","Role","UserId" };

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
            { "Six" , "six" },
            { "Birthday" , "birthday" },
            { "Tel" , "tel" },
            { "Mobile" , "mobile" },
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
            { "RoleId" , "role_id" },
            { "role_id" , "role_id" },
            { "Role" , "role" },
            { "UserId" , "user_id" },
            { "user_id" , "user_id" }
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
        protected sealed override void LoadEntity(MySqlDataReader reader,PersonnelData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._id = (int)reader.GetInt32(0);
                if (!reader.IsDBNull(1))
                    entity._fullname = reader.GetString(1).ToString();
                entity._six = (bool)reader.GetBoolean(2);
                if (!reader.IsDBNull(3))
                    try{entity._birthday = reader.GetMySqlDateTime(3).Value;}catch{}
                if (!reader.IsDBNull(4))
                    entity._tel = reader.GetString(4).ToString();
                if (!reader.IsDBNull(5))
                    entity._mobile = reader.GetString(5).ToString();
                if (!reader.IsDBNull(6))
                    entity._memo = reader.GetString(6).ToString();
                if (!reader.IsDBNull(7))
                    entity._datastate = (DataStateType)reader.GetInt32(7);
                entity._isfreeze = (bool)reader.GetBoolean(8);
                entity._authorid = (int)reader.GetInt32(9);
                if (!reader.IsDBNull(10))
                    try{entity._adddate = reader.GetMySqlDateTime(10).Value;}catch{}
                entity._lastreviserid = (int)reader.GetInt32(11);
                if (!reader.IsDBNull(12))
                    try{entity._lastmodifydate = reader.GetMySqlDateTime(12).Value;}catch{}
                if (!reader.IsDBNull(13))
                    entity._auditstate = (AuditStateType)reader.GetInt32(13);
                entity._auditorid = (int)reader.GetInt32(14);
                if (!reader.IsDBNull(15))
                    try{entity._auditdate = reader.GetMySqlDateTime(15).Value;}catch{}
                entity._roleid = (int)reader.GetInt32(16);
                if (!reader.IsDBNull(17))
                    entity._role = reader.GetString(17).ToString();
                entity._userid = (int)reader.GetInt32(18);
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
                case "Six":
                    return MySqlDbType.Byte;
                case "Birthday":
                    return MySqlDbType.DateTime;
                case "Tel":
                    return MySqlDbType.VarString;
                case "Mobile":
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
                case "RoleId":
                    return MySqlDbType.Int32;
                case "Role":
                    return MySqlDbType.VarString;
                case "UserId":
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
        private void CreateFullSqlParameter(PersonnelData entity, MySqlCommand cmd)
        {
            //02:标识(Id)
            cmd.Parameters.Add(new MySqlParameter("Id",MySqlDbType.Int32){ Value = entity.Id});
            //04:姓名(FullName)
            var isNull = string.IsNullOrWhiteSpace(entity.FullName);
            var parameter = new MySqlParameter("FullName",MySqlDbType.VarString , isNull ? 10 : (entity.FullName).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.FullName;
            cmd.Parameters.Add(parameter);
            //05:性别(Six)
            cmd.Parameters.Add(new MySqlParameter("Six",MySqlDbType.Byte) { Value = entity.Six ? (byte)1 : (byte)0 });
            //06:生日(Birthday)
            isNull = entity.Birthday.Year < 1900;
            parameter = new MySqlParameter("Birthday",MySqlDbType.DateTime);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Birthday;
            cmd.Parameters.Add(parameter);
            //07:电话(Tel)
            isNull = string.IsNullOrWhiteSpace(entity.Tel);
            parameter = new MySqlParameter("Tel",MySqlDbType.VarString , isNull ? 10 : (entity.Tel).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Tel;
            cmd.Parameters.Add(parameter);
            //08:手机(Mobile)
            isNull = string.IsNullOrWhiteSpace(entity.Mobile);
            parameter = new MySqlParameter("Mobile",MySqlDbType.VarString , isNull ? 10 : (entity.Mobile).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Mobile;
            cmd.Parameters.Add(parameter);
            //09:备注(Memo)
            isNull = string.IsNullOrWhiteSpace(entity.Memo);
            parameter = new MySqlParameter("Memo",MySqlDbType.Text , isNull ? 10 : (entity.Memo).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Memo;
            cmd.Parameters.Add(parameter);
            //32:数据状态(DataState)
            cmd.Parameters.Add(new MySqlParameter("DataState",MySqlDbType.Int32){ Value = (int)entity.DataState});
            //33:数据是否已冻结(IsFreeze)
            cmd.Parameters.Add(new MySqlParameter("IsFreeze",MySqlDbType.Byte) { Value = entity.IsFreeze ? (byte)1 : (byte)0 });
            //35:制作人(AuthorID)
            cmd.Parameters.Add(new MySqlParameter("AuthorID",MySqlDbType.Int32){ Value = entity.AuthorID});
            //37:制作时间(AddDate)
            isNull = entity.AddDate.Year < 1900;
            parameter = new MySqlParameter("AddDate",MySqlDbType.DateTime);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.AddDate;
            cmd.Parameters.Add(parameter);
            //39:最后修改者(LastReviserID)
            cmd.Parameters.Add(new MySqlParameter("LastReviserID",MySqlDbType.Int32){ Value = entity.LastReviserID});
            //41:最后修改日期(LastModifyDate)
            isNull = entity.LastModifyDate.Year < 1900;
            parameter = new MySqlParameter("LastModifyDate",MySqlDbType.DateTime);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.LastModifyDate;
            cmd.Parameters.Add(parameter);
            //43:审核状态(AuditState)
            cmd.Parameters.Add(new MySqlParameter("AuditState",MySqlDbType.Int32){ Value = (int)entity.AuditState});
            //45:审核人(AuditorId)
            cmd.Parameters.Add(new MySqlParameter("AuditorId",MySqlDbType.Int32){ Value = entity.AuditorId});
            //47:审核时间(AuditDate)
            isNull = entity.AuditDate.Year < 1900;
            parameter = new MySqlParameter("AuditDate",MySqlDbType.DateTime);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.AuditDate;
            cmd.Parameters.Add(parameter);
            //48:角色外键(RoleId)
            cmd.Parameters.Add(new MySqlParameter("RoleId",MySqlDbType.Int32){ Value = entity.RoleId});
            //49:角色(Role)
            isNull = string.IsNullOrWhiteSpace(entity.Role);
            parameter = new MySqlParameter("Role",MySqlDbType.VarString , isNull ? 10 : (entity.Role).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Role;
            cmd.Parameters.Add(parameter);
            //51:系统用户外键(UserId)
            cmd.Parameters.Add(new MySqlParameter("UserId",MySqlDbType.Int32){ Value = entity.UserId});
        }


        /// <summary>
        /// 设置更新数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        protected sealed override void SetUpdateCommand(PersonnelData entity, MySqlCommand cmd)
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
        protected sealed override bool SetInsertCommand(PersonnelData entity, MySqlCommand cmd)
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
        internal static MySqlDataTableScope<PersonnelData> CreateScope()
        {
            var db = Authorities.Default ?? new Authorities();
            return MySqlDataTableScope<PersonnelData>.CreateScope(db, db.Personnels);
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
    `full_name` AS `FullName`,
    `six` AS `Six`,
    `birthday` AS `Birthday`,
    `tel` AS `Tel`,
    `mobile` AS `Mobile`,
    `memo` AS `Memo`,
    `data_state` AS `DataState`,
    `is_freeze` AS `IsFreeze`,
    `audit_state` AS `AuditState`,
    `role` AS `Role`,
    `user_id` AS `UserId`";
            }
        }


        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public override void SimpleLoad(MySqlDataReader reader,PersonnelData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._id = (int)reader.GetInt32(0);
                if (!reader.IsDBNull(1))
                    entity._fullname = reader.GetString(1).ToString();
                entity._six = (bool)reader.GetBoolean(2);
                if (!reader.IsDBNull(3))
                    try{entity._birthday = reader.GetMySqlDateTime(3).Value;}catch{}
                if (!reader.IsDBNull(4))
                    entity._tel = reader.GetString(4).ToString();
                if (!reader.IsDBNull(5))
                    entity._mobile = reader.GetString(5).ToString();
                if (!reader.IsDBNull(6))
                    entity._memo = reader.GetString(6).ToString();
                if (!reader.IsDBNull(7))
                    entity._datastate = (DataStateType)reader.GetInt32(7);
                entity._isfreeze = (bool)reader.GetBoolean(8);
                if (!reader.IsDBNull(9))
                    entity._auditstate = (AuditStateType)reader.GetInt32(9);
                if (!reader.IsDBNull(10))
                    entity._role = reader.GetString(10).ToString();
                entity._userid = (int)reader.GetInt32(11);
            }
        }
        #endregion

    }

    partial class Authorities
    {


        /// <summary>
        /// 职员(view_sys_personnel):职员
        /// </summary>
        public const int Table_Personnel = 0x50005;


        /// <summary>
        /// 职员的结构语句
        /// </summary>
        private TableSql _view_sys_personnelSql = new TableSql
        {
            TableName = "view_sys_personnel",
            PimaryKey = "Id"
        };


        /// <summary>
        /// 职员数据访问对象
        /// </summary>
        private PersonnelDataAccess _personnels;

        /// <summary>
        /// 职员数据访问对象
        /// </summary>
        public PersonnelDataAccess Personnels
        {
            get
            {
                return this._personnels ?? ( this._personnels = new PersonnelDataAccess{ DataBase = this});
            }
        }
    }
}