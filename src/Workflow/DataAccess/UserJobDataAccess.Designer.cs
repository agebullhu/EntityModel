/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/10/7 11:33:12*/
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


namespace Gboxt.Common.Workflow.DataAccess
{
    /// <summary>
    /// 用户工作列表
    /// </summary>
    public partial class UserJobDataAccess
    {

        #region 基本SQL语句

        /// <summary>
        /// 表的唯一标识
        /// </summary>
        public override int TableId
        {
            get { return WorkflowDataBase.Table_UserJob; }
        }

        /// <summary>
        /// 读取表名
        /// </summary>
        protected sealed override string ReadTableName
        {
            get
            {
                return @"tb_wf_user_job";
            }
        }

        /// <summary>
        /// 写入表名
        /// </summary>
        protected sealed override string WriteTableName
        {
            get
            {
                return @"tb_wf_user_job";
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
    `_user_work_id` AS `Id`,
    `_title` AS `Title`,
    `_date` AS `Date`,
    `_message` AS `Message`,
    `_job_type` AS `JobType`,
    `_job_status` AS `JobStatus`,
    `_command_type` AS `CommandType`,
    `_link_id` AS `LinkId`,
    `_entity_type` AS `EntityType`,
    `_to_user_id` AS `ToUserId`,
    `_to_user_name` AS `ToUserName`,
    `_from_user_id` AS `FromUserId`,
    `_from_user_name` AS `FromUserName`,
    `_argument` AS `Argument`,
    `_data_state` AS `DataState`,
    `_is_freeze` AS `IsFreeze`,
    `_department_id` AS `DepartmentId`";
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
INSERT INTO `tb_wf_user_job`
(
    `_title`,
    `_date`,
    `_message`,
    `_job_type`,
    `_job_status`,
    `_command_type`,
    `_link_id`,
    `_entity_type`,
    `_to_user_id`,
    `_to_user_name`,
    `_from_user_id`,
    `_from_user_name`,
    `_argument`,
    `_data_state`,
    `_is_freeze`,
    `_department_id`
)
VALUES
(
    ?Title,
    ?Date,
    ?Message,
    ?JobType,
    ?JobStatus,
    ?CommandType,
    ?LinkId,
    ?EntityType,
    ?ToUserId,
    ?ToUserName,
    ?FromUserId,
    ?FromUserName,
    ?Argument,
    ?DataState,
    ?IsFreeze,
    ?DepartmentId
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
UPDATE `tb_wf_user_job` SET
       `_title` = ?Title,
       `_date` = ?Date,
       `_message` = ?Message,
       `_job_status` = ?JobStatus,
       `_to_user_id` = ?ToUserId,
       `_to_user_name` = ?ToUserName,
       `_from_user_id` = ?FromUserId,
       `_from_user_name` = ?FromUserName,
       `_argument` = ?Argument,
       `_data_state` = ?DataState,
       `_is_freeze` = ?IsFreeze,
       `_department_id` = ?DepartmentId
 WHERE `_user_work_id` = ?Id;";
            }
        }

        /// <summary>
        /// 取得仅更新的SQL语句
        /// </summary>
        internal string GetModifiedSqlCode(UserJobData data)
        {
            if (data.__EntityStatusNull || !data.__EntityStatus.IsModified)
                return ";";
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE `tb_wf_user_job` SET");
            //标题
            if (data.__EntityStatus.ModifiedProperties[UserJobData.Real_Title] > 0)
                sql.AppendLine("       `_title` = ?Title");
            //发生日期
            if (data.__EntityStatus.ModifiedProperties[UserJobData.Real_Date] > 0)
                sql.AppendLine("       `_date` = ?Date");
            //工作消息
            if (data.__EntityStatus.ModifiedProperties[UserJobData.Real_Message] > 0)
                sql.AppendLine("       `_message` = ?Message");
            //工作状态
            if (data.__EntityStatus.ModifiedProperties[UserJobData.Real_JobStatus] > 0)
                sql.AppendLine("       `_job_status` = ?JobStatus");
            //目标用户标识
            if (data.__EntityStatus.ModifiedProperties[UserJobData.Real_ToUserId] > 0)
                sql.AppendLine("       `_to_user_id` = ?ToUserId");
            //目标用户名字
            if (data.__EntityStatus.ModifiedProperties[UserJobData.Real_ToUserName] > 0)
                sql.AppendLine("       `_to_user_name` = ?ToUserName");
            //来源用户标识
            if (data.__EntityStatus.ModifiedProperties[UserJobData.Real_FromUserId] > 0)
                sql.AppendLine("       `_from_user_id` = ?FromUserId");
            //来源用户名字
            if (data.__EntityStatus.ModifiedProperties[UserJobData.Real_FromUserName] > 0)
                sql.AppendLine("       `_from_user_name` = ?FromUserName");
            //其它参数
            if (data.__EntityStatus.ModifiedProperties[UserJobData.Real_Argument] > 0)
                sql.AppendLine("       `_argument` = ?Argument");
            //数据状态
            if (data.__EntityStatus.ModifiedProperties[UserJobData.Real_DataState] > 0)
                sql.AppendLine("       `_data_state` = ?DataState");
            //数据是否已冻结
            if (data.__EntityStatus.ModifiedProperties[UserJobData.Real_IsFreeze] > 0)
                sql.AppendLine("       `_is_freeze` = ?IsFreeze");
            //接收者部门
            if (data.__EntityStatus.ModifiedProperties[UserJobData.Real_DepartmentId] > 0)
                sql.AppendLine("       `_department_id` = ?DepartmentId");
            sql.Append(" WHERE `_user_work_id` = ?Id;");
            return sql.ToString();
        }

        #endregion


        #region 字段

        /// <summary>
        ///  所有字段
        /// </summary>
        static string[] _fields = new string[]{ "Id","Title","Date","Message","JobType","JobStatus","CommandType","LinkId","EntityType","ToUserId","ToUserName","FromUserId","FromUserName","Argument","DataState","IsFreeze","DepartmentId" };

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
            { "Id" , "_user_work_id" },
            { "_user_work_id" , "_user_work_id" },
            { "Title" , "_title" },
            { "_title" , "_title" },
            { "Date" , "_date" },
            { "_date" , "_date" },
            { "Message" , "_message" },
            { "_message" , "_message" },
            { "JobType" , "_job_type" },
            { "_job_type" , "_job_type" },
            { "JobStatus" , "_job_status" },
            { "_job_status" , "_job_status" },
            { "CommandType" , "_command_type" },
            { "_command_type" , "_command_type" },
            { "LinkId" , "_link_id" },
            { "_link_id" , "_link_id" },
            { "DataId" , "_link_id" },
            { "EntityType" , "_entity_type" },
            { "_entity_type" , "_entity_type" },
            { "ToUserId" , "_to_user_id" },
            { "_to_user_id" , "_to_user_id" },
            { "ToUserName" , "_to_user_name" },
            { "_to_user_name" , "_to_user_name" },
            { "FromUserId" , "_from_user_id" },
            { "_from_user_id" , "_from_user_id" },
            { "FromUserName" , "_from_user_name" },
            { "_from_user_name" , "_from_user_name" },
            { "Argument" , "_argument" },
            { "_argument" , "_argument" },
            { "DataState" , "_data_state" },
            { "_data_state" , "_data_state" },
            { "IsFreeze" , "_is_freeze" },
            { "_is_freeze" , "_is_freeze" },
            { "DepartmentId" , "_department_id" },
            { "_department_id" , "_department_id" }
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
        protected sealed override void LoadEntity(MySqlDataReader reader,UserJobData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._id = (int)reader.GetInt32(0);
                if (!reader.IsDBNull(1))
                    entity._title = reader.GetString(1).ToString();
                if (!reader.IsDBNull(2))
                    try{entity._date = reader.GetMySqlDateTime(2).Value;}catch{}
                if (!reader.IsDBNull(3))
                    entity._message = reader.GetString(3).ToString();
                if (!reader.IsDBNull(4))
                    entity._jobtype = (UserJobType)reader.GetInt32(4);
                if (!reader.IsDBNull(5))
                    entity._jobstatus = (JobStatusType)reader.GetInt32(5);
                if (!reader.IsDBNull(6))
                    entity._commandtype = (JobCommandType)reader.GetInt32(6);
                entity._linkid = (int)reader.GetInt32(7);
                entity._entitytype = (int)reader.GetInt32(8);
                entity._touserid = (int)reader.GetInt32(9);
                if (!reader.IsDBNull(10))
                    entity._tousername = reader.GetString(10).ToString();
                entity._fromuserid = (int)reader.GetInt32(11);
                if (!reader.IsDBNull(12))
                    entity._fromusername = reader.GetString(12).ToString();
                if (!reader.IsDBNull(13))
                    entity._argument = /*(LONGTEXT)*/reader.GetValue(13).ToString();
                if (!reader.IsDBNull(14))
                    entity._datastate = (DataStateType)reader.GetInt32(14);
                if (!reader.IsDBNull(15))
                    entity._isfreeze = (bool)reader.GetBoolean(15);
                entity._departmentid = (int)reader.GetInt32(16);
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
                case "Title":
                    return MySqlDbType.VarString;
                case "Date":
                    return MySqlDbType.DateTime;
                case "Message":
                    return MySqlDbType.VarString;
                case "JobType":
                    return MySqlDbType.Int32;
                case "JobStatus":
                    return MySqlDbType.Int32;
                case "CommandType":
                    return MySqlDbType.Int32;
                case "LinkId":
                    return MySqlDbType.Int32;
                case "EntityType":
                    return MySqlDbType.Int32;
                case "ToUserId":
                    return MySqlDbType.Int32;
                case "ToUserName":
                    return MySqlDbType.VarString;
                case "FromUserId":
                    return MySqlDbType.Int32;
                case "FromUserName":
                    return MySqlDbType.VarString;
                case "Argument":
                    return MySqlDbType.LongText;
                case "DataState":
                    return MySqlDbType.Int32;
                case "IsFreeze":
                    return MySqlDbType.Byte;
                case "DepartmentId":
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
        private void CreateFullSqlParameter(UserJobData entity, MySqlCommand cmd)
        {
            //02:标识(Id)
            cmd.Parameters.Add(new MySqlParameter("Id",MySqlDbType.Int32){ Value = entity.Id});
            //04:标题(Title)
            var isNull = string.IsNullOrWhiteSpace(entity.Title);
            var parameter = new MySqlParameter("Title",MySqlDbType.VarString , isNull ? 10 : (entity.Title).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Title;
            cmd.Parameters.Add(parameter);
            //05:发生日期(Date)
            isNull = entity.Date.Year < 1900;
            parameter = new MySqlParameter("Date",MySqlDbType.DateTime);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Date;
            cmd.Parameters.Add(parameter);
            //06:工作消息(Message)
            isNull = string.IsNullOrWhiteSpace(entity.Message);
            parameter = new MySqlParameter("Message",MySqlDbType.VarString , isNull ? 10 : (entity.Message).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Message;
            cmd.Parameters.Add(parameter);
            //07:任务分类(JobType)
            cmd.Parameters.Add(new MySqlParameter("JobType",MySqlDbType.Int32){ Value = (int)entity.JobType});
            //08:工作状态(JobStatus)
            cmd.Parameters.Add(new MySqlParameter("JobStatus",MySqlDbType.Int32){ Value = (int)entity.JobStatus});
            //09:命令类型(CommandType)
            cmd.Parameters.Add(new MySqlParameter("CommandType",MySqlDbType.Int32){ Value = (int)entity.CommandType});
            //10:关联标识(LinkId)
            cmd.Parameters.Add(new MySqlParameter("LinkId",MySqlDbType.Int32){ Value = entity.LinkId});
            //11:连接类型(EntityType)
            cmd.Parameters.Add(new MySqlParameter("EntityType",MySqlDbType.Int32){ Value = entity.EntityType});
            //12:目标用户标识(ToUserId)
            cmd.Parameters.Add(new MySqlParameter("ToUserId",MySqlDbType.Int32){ Value = entity.ToUserId});
            //13:目标用户名字(ToUserName)
            isNull = string.IsNullOrWhiteSpace(entity.ToUserName);
            parameter = new MySqlParameter("ToUserName",MySqlDbType.VarString , isNull ? 10 : (entity.ToUserName).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.ToUserName;
            cmd.Parameters.Add(parameter);
            //14:来源用户标识(FromUserId)
            cmd.Parameters.Add(new MySqlParameter("FromUserId",MySqlDbType.Int32){ Value = entity.FromUserId});
            //15:来源用户名字(FromUserName)
            isNull = string.IsNullOrWhiteSpace(entity.FromUserName);
            parameter = new MySqlParameter("FromUserName",MySqlDbType.VarString , isNull ? 10 : (entity.FromUserName).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.FromUserName;
            cmd.Parameters.Add(parameter);
            //16:其它参数(Argument)
            isNull = string.IsNullOrWhiteSpace(entity.Argument);
            parameter = new MySqlParameter("Argument",MySqlDbType.LongText , isNull ? 10 : (entity.Argument).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Argument;
            cmd.Parameters.Add(parameter);
            //17:数据状态(DataState)
            cmd.Parameters.Add(new MySqlParameter("DataState",MySqlDbType.Int32){ Value = (int)entity.DataState});
            //18:数据是否已冻结(IsFreeze)
            cmd.Parameters.Add(new MySqlParameter("IsFreeze",MySqlDbType.Byte) { Value = entity.IsFreeze ? (byte)1 : (byte)0 });
            //19:接收者部门(DepartmentId)
            cmd.Parameters.Add(new MySqlParameter("DepartmentId",MySqlDbType.Int32){ Value = entity.DepartmentId});
        }


        /// <summary>
        /// 设置更新数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        protected sealed override void SetUpdateCommand(UserJobData entity, MySqlCommand cmd)
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
        protected sealed override bool SetInsertCommand(UserJobData entity, MySqlCommand cmd)
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
            return WorkflowDataBase.Default ?? new WorkflowDataBase();
        }
        
        /// <summary>
        /// 生成数据库访问范围
        /// </summary>
        internal static MySqlDataTableScope<UserJobData> CreateScope()
        {
            var db = WorkflowDataBase.Default ?? new WorkflowDataBase();
            return MySqlDataTableScope<UserJobData>.CreateScope(db, db.UserJobs);
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
    `_user_work_id` AS `Id`,
    `_title` AS `Title`,
    `_date` AS `Date`,
    `_message` AS `Message`,
    `_job_type` AS `JobType`,
    `_job_status` AS `JobStatus`,
    `_command_type` AS `CommandType`,
    `_to_user_name` AS `ToUserName`,
    `_from_user_name` AS `FromUserName`,
    `_data_state` AS `DataState`,
    `_is_freeze` AS `IsFreeze`";
            }
        }


        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public override void SimpleLoad(MySqlDataReader reader,UserJobData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._id = (int)reader.GetInt32(0);
                if (!reader.IsDBNull(1))
                    entity._title = reader.GetString(1).ToString();
                if (!reader.IsDBNull(2))
                    try{entity._date = reader.GetMySqlDateTime(2).Value;}catch{}
                if (!reader.IsDBNull(3))
                    entity._message = reader.GetString(3).ToString();
                if (!reader.IsDBNull(4))
                    entity._jobtype = (UserJobType)reader.GetInt32(4);
                if (!reader.IsDBNull(5))
                    entity._jobstatus = (JobStatusType)reader.GetInt32(5);
                if (!reader.IsDBNull(6))
                    entity._commandtype = (JobCommandType)reader.GetInt32(6);
                if (!reader.IsDBNull(7))
                    entity._tousername = reader.GetString(7).ToString();
                if (!reader.IsDBNull(8))
                    entity._fromusername = reader.GetString(8).ToString();
                if (!reader.IsDBNull(9))
                    entity._datastate = (DataStateType)reader.GetInt32(9);
                if (!reader.IsDBNull(10))
                    entity._isfreeze = (bool)reader.GetBoolean(10);
            }
        }
        #endregion

    }

    partial class WorkflowDataBase
    {


        /// <summary>
        /// 用户工作列表(tb_wf_user_job):用户工作列表
        /// </summary>
        public const int Table_UserJob = 0x70001;


        /// <summary>
        /// 用户工作列表的结构语句
        /// </summary>
        private TableSql _tb_wf_user_jobSql = new TableSql
        {
            TableName = "tb_wf_user_job",
            PimaryKey = "Id"
        };


        /// <summary>
        /// 用户工作列表数据访问对象
        /// </summary>
        private UserJobDataAccess _userJobs;

        /// <summary>
        /// 用户工作列表数据访问对象
        /// </summary>
        public UserJobDataAccess UserJobs
        {
            get
            {
                return this._userJobs ?? ( this._userJobs = new UserJobDataAccess{ DataBase = this});
            }
        }
    }
}