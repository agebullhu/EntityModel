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
    /// 系统用户
    /// </summary>
    public partial class UserDataAccess
    {

        #region 基本SQL语句

        /// <summary>
        /// 表的唯一标识
        /// </summary>
        public override int TableId
        {
            get { return Authorities.Table_User; }
        }

        /// <summary>
        /// 读取表名
        /// </summary>
        protected sealed override string ReadTableName
        {
            get
            {
                return @"view_sys_user";
            }
        }

        /// <summary>
        /// 写入表名
        /// </summary>
        protected sealed override string WriteTableName
        {
            get
            {
                return @"tb_sys_user";
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
    `real_name` AS `RealName`,
    `user_name` AS `UserName`,
    `pass_word` AS `PassWord`,
    `role_id` AS `RoleId`,
    `Role` AS `Role`,
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
INSERT INTO `tb_sys_user`
(
    `real_name`,
    `user_name`,
    `pass_word`,
    `role_id`,
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
    ?RealName,
    ?UserName,
    ?PassWord,
    ?RoleId,
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
UPDATE `tb_sys_user` SET
       `real_name` = ?RealName,
       `pass_word` = ?PassWord,
       `role_id` = ?RoleId,
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
        internal string GetModifiedSqlCode(UserData data)
        {
            if (data.__EntityStatusNull || !data.__EntityStatus.IsModified)
                return ";";
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE `tb_sys_user` SET");
            //姓名
            if (data.__EntityStatus.ModifiedProperties[UserData.Real_RealName] > 0)
                sql.AppendLine("       `real_name` = ?RealName");
            //密码
            if (data.__EntityStatus.ModifiedProperties[UserData.Real_PassWord] > 0)
                sql.AppendLine("       `pass_word` = ?PassWord");
            //标识
            if (data.__EntityStatus.ModifiedProperties[UserData.Real_RoleId] > 0)
                sql.AppendLine("       `role_id` = ?RoleId");
            //备注
            if (data.__EntityStatus.ModifiedProperties[UserData.Real_Memo] > 0)
                sql.AppendLine("       `memo` = ?Memo");
            //数据状态
            if (data.__EntityStatus.ModifiedProperties[UserData.Real_DataState] > 0)
                sql.AppendLine("       `data_state` = ?DataState");
            //数据是否已冻结
            if (data.__EntityStatus.ModifiedProperties[UserData.Real_IsFreeze] > 0)
                sql.AppendLine("       `is_freeze` = ?IsFreeze");
            //制作人
            if (data.__EntityStatus.ModifiedProperties[UserData.Real_AuthorID] > 0)
                sql.AppendLine("       `author_id` = ?AuthorID");
            //制作时间
            if (data.__EntityStatus.ModifiedProperties[UserData.Real_AddDate] > 0)
                sql.AppendLine("       `add_date` = ?AddDate");
            //最后修改者
            if (data.__EntityStatus.ModifiedProperties[UserData.Real_LastReviserID] > 0)
                sql.AppendLine("       `last_reviser_id` = ?LastReviserID");
            //最后修改日期
            if (data.__EntityStatus.ModifiedProperties[UserData.Real_LastModifyDate] > 0)
                sql.AppendLine("       `last_modify_date` = ?LastModifyDate");
            //审核状态
            if (data.__EntityStatus.ModifiedProperties[UserData.Real_AuditState] > 0)
                sql.AppendLine("       `audit_state` = ?AuditState");
            //审核人
            if (data.__EntityStatus.ModifiedProperties[UserData.Real_AuditorId] > 0)
                sql.AppendLine("       `auditor_id` = ?AuditorId");
            //审核时间
            if (data.__EntityStatus.ModifiedProperties[UserData.Real_AuditDate] > 0)
                sql.AppendLine("       `audit_date` = ?AuditDate");
            sql.Append(" WHERE `id` = ?Id;");
            return sql.ToString();
        }

        #endregion


        #region 字段

        /// <summary>
        ///  所有字段
        /// </summary>
        static string[] _fields = new string[]{ "Id","RealName","UserName","PassWord","RoleId","Role","Memo","DataState","IsFreeze","AuthorID","AddDate","LastReviserID","LastModifyDate","AuditState","AuditorId","AuditDate" };

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
            { "RealName" , "real_name" },
            { "real_name" , "real_name" },
            { "UserName" , "user_name" },
            { "user_name" , "user_name" },
            { "PassWord" , "pass_word" },
            { "pass_word" , "pass_word" },
            { "RoleId" , "role_id" },
            { "role_id" , "role_id" },
            { "Role" , "Role" },
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
        protected sealed override void LoadEntity(MySqlDataReader reader,UserData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._id = (int)reader.GetInt32(0);
                if (!reader.IsDBNull(1))
                    entity._realname = reader.GetString(1).ToString();
                if (!reader.IsDBNull(2))
                    entity._username = reader.GetString(2).ToString();
                if (!reader.IsDBNull(3))
                    entity._password = reader.GetString(3).ToString();
                entity._roleid = (int)reader.GetInt32(4);
                if (!reader.IsDBNull(5))
                    entity._role = reader.GetString(5).ToString();
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
                case "RealName":
                    return MySqlDbType.VarString;
                case "UserName":
                    return MySqlDbType.VarString;
                case "PassWord":
                    return MySqlDbType.Text;
                case "RoleId":
                    return MySqlDbType.Int32;
                case "Role":
                    return MySqlDbType.VarString;
                case "Memo":
                    return MySqlDbType.VarString;
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
        private void CreateFullSqlParameter(UserData entity, MySqlCommand cmd)
        {
            //02:标识(Id)
            cmd.Parameters.Add(new MySqlParameter("Id",MySqlDbType.Int32){ Value = entity.Id});
            //04:姓名(RealName)
            var isNull = string.IsNullOrWhiteSpace(entity.RealName);
            var parameter = new MySqlParameter("RealName",MySqlDbType.VarString , isNull ? 10 : (entity.RealName).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.RealName;
            cmd.Parameters.Add(parameter);
            //05:用户名(UserName)
            isNull = string.IsNullOrWhiteSpace(entity.UserName);
            parameter = new MySqlParameter("UserName",MySqlDbType.VarString , isNull ? 10 : (entity.UserName).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.UserName;
            cmd.Parameters.Add(parameter);
            //06:密码(PassWord)
            isNull = string.IsNullOrWhiteSpace(entity.PassWord);
            parameter = new MySqlParameter("PassWord",MySqlDbType.Text , isNull ? 10 : (entity.PassWord).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.PassWord;
            cmd.Parameters.Add(parameter);
            //07:标识(RoleId)
            cmd.Parameters.Add(new MySqlParameter("RoleId",MySqlDbType.Int32){ Value = entity.RoleId});
            //08:角色(Role)
            isNull = string.IsNullOrWhiteSpace(entity.Role);
            parameter = new MySqlParameter("Role",MySqlDbType.VarString , isNull ? 10 : (entity.Role).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Role;
            cmd.Parameters.Add(parameter);
            //09:备注(Memo)
            isNull = string.IsNullOrWhiteSpace(entity.Memo);
            parameter = new MySqlParameter("Memo",MySqlDbType.VarString , isNull ? 10 : (entity.Memo).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Memo;
            cmd.Parameters.Add(parameter);
            //29:数据状态(DataState)
            cmd.Parameters.Add(new MySqlParameter("DataState",MySqlDbType.Int32){ Value = (int)entity.DataState});
            //31:数据是否已冻结(IsFreeze)
            cmd.Parameters.Add(new MySqlParameter("IsFreeze",MySqlDbType.Byte) { Value = entity.IsFreeze ? (byte)1 : (byte)0 });
            //33:制作人(AuthorID)
            cmd.Parameters.Add(new MySqlParameter("AuthorID",MySqlDbType.Int32){ Value = entity.AuthorID});
            //35:制作时间(AddDate)
            isNull = entity.AddDate.Year < 1900;
            parameter = new MySqlParameter("AddDate",MySqlDbType.DateTime);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.AddDate;
            cmd.Parameters.Add(parameter);
            //37:最后修改者(LastReviserID)
            cmd.Parameters.Add(new MySqlParameter("LastReviserID",MySqlDbType.Int32){ Value = entity.LastReviserID});
            //39:最后修改日期(LastModifyDate)
            isNull = entity.LastModifyDate.Year < 1900;
            parameter = new MySqlParameter("LastModifyDate",MySqlDbType.DateTime);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.LastModifyDate;
            cmd.Parameters.Add(parameter);
            //41:审核状态(AuditState)
            cmd.Parameters.Add(new MySqlParameter("AuditState",MySqlDbType.Int32){ Value = (int)entity.AuditState});
            //43:审核人(AuditorId)
            cmd.Parameters.Add(new MySqlParameter("AuditorId",MySqlDbType.Int32){ Value = entity.AuditorId});
            //45:审核时间(AuditDate)
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
        protected sealed override void SetUpdateCommand(UserData entity, MySqlCommand cmd)
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
        protected sealed override bool SetInsertCommand(UserData entity, MySqlCommand cmd)
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
        internal static MySqlDataTableScope<UserData> CreateScope()
        {
            var db = Authorities.Default ?? new Authorities();
            return MySqlDataTableScope<UserData>.CreateScope(db, db.Users);
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
    `real_name` AS `RealName`,
    `user_name` AS `UserName`,
    `pass_word` AS `PassWord`,
    `Role` AS `Role`,
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
        public override void SimpleLoad(MySqlDataReader reader,UserData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._id = (int)reader.GetInt32(0);
                if (!reader.IsDBNull(1))
                    entity._realname = reader.GetString(1).ToString();
                if (!reader.IsDBNull(2))
                    entity._username = reader.GetString(2).ToString();
                if (!reader.IsDBNull(3))
                    entity._password = reader.GetString(3).ToString();
                if (!reader.IsDBNull(4))
                    entity._role = reader.GetString(4).ToString();
                if (!reader.IsDBNull(5))
                    entity._memo = reader.GetString(5).ToString();
                if (!reader.IsDBNull(6))
                    entity._datastate = (DataStateType)reader.GetInt32(6);
                entity._isfreeze = (bool)reader.GetBoolean(7);
                entity._authorid = (int)reader.GetInt32(8);
                if (!reader.IsDBNull(9))
                    try{entity._adddate = reader.GetMySqlDateTime(9).Value;}catch{}
                entity._lastreviserid = (int)reader.GetInt32(10);
                if (!reader.IsDBNull(11))
                    try{entity._lastmodifydate = reader.GetMySqlDateTime(11).Value;}catch{}
                if (!reader.IsDBNull(12))
                    entity._auditstate = (AuditStateType)reader.GetInt32(12);
                entity._auditorid = (int)reader.GetInt32(13);
                if (!reader.IsDBNull(14))
                    try{entity._auditdate = reader.GetMySqlDateTime(14).Value;}catch{}
            }
        }
        #endregion

    }

    partial class Authorities
    {


        /// <summary>
        /// 系统用户(view_sys_user):系统用户
        /// </summary>
        public const int Table_User = 0x50008;


        /// <summary>
        /// 系统用户的结构语句
        /// </summary>
        private TableSql _view_sys_userSql = new TableSql
        {
            TableName = "view_sys_user",
            PimaryKey = "Id"
        };


        /// <summary>
        /// 系统用户数据访问对象
        /// </summary>
        private UserDataAccess _users;

        /// <summary>
        /// 系统用户数据访问对象
        /// </summary>
        public UserDataAccess Users
        {
            get
            {
                return this._users ?? ( this._users = new UserDataAccess{ DataBase = this});
            }
        }
    }
}