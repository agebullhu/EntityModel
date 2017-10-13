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
    /// 行级权限关联
    /// </summary>
    internal partial class SubjectionDataAccess
    {

        #region 基本SQL语句

        /// <summary>
        /// 表的唯一标识
        /// </summary>
        public override int TableId
        {
            get { return Authorities.Table_Subjection; }
        }

        /// <summary>
        /// 读取表名
        /// </summary>
        protected sealed override string ReadTableName
        {
            get
            {
                return @"tb_sys_subjection";
            }
        }

        /// <summary>
        /// 写入表名
        /// </summary>
        protected sealed override string WriteTableName
        {
            get
            {
                return @"tb_sys_subjection";
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
    `master_id` AS `MasterId`,
    `slave_id` AS `SlaveId`";
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
INSERT INTO `tb_sys_subjection`
(
    `master_id`,
    `slave_id`
)
VALUES
(
    ?MasterId,
    ?SlaveId
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
UPDATE `tb_sys_subjection` SET
       `master_id` = ?MasterId,
       `slave_id` = ?SlaveId
 WHERE `id` = ?Id;";
            }
        }

        /// <summary>
        /// 取得仅更新的SQL语句
        /// </summary>
        internal string GetModifiedSqlCode(SubjectionData data)
        {
            if (data.__EntityStatusNull || !data.__EntityStatus.IsModified)
                return ";";
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE `tb_sys_subjection` SET");
            //主键
            if (data.__EntityStatus.ModifiedProperties[SubjectionData.Real_MasterId] > 0)
                sql.AppendLine("       `master_id` = ?MasterId");
            //关联
            if (data.__EntityStatus.ModifiedProperties[SubjectionData.Real_SlaveId] > 0)
                sql.AppendLine("       `slave_id` = ?SlaveId");
            sql.Append(" WHERE `id` = ?Id;");
            return sql.ToString();
        }

        #endregion


        #region 字段

        /// <summary>
        ///  所有字段
        /// </summary>
        static string[] _fields = new string[]{ "Id","MasterId","SlaveId" };

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
            { "MasterId" , "master_id" },
            { "master_id" , "master_id" },
            { "SlaveId" , "slave_id" },
            { "slave_id" , "slave_id" }
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
        protected sealed override void LoadEntity(MySqlDataReader reader,SubjectionData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._id = (int)reader.GetInt32(0);
                entity._masterid = (int)reader.GetInt32(1);
                entity._slaveid = (int)reader.GetInt32(2);
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
                case "MasterId":
                    return MySqlDbType.Int32;
                case "SlaveId":
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
        private void CreateFullSqlParameter(SubjectionData entity, MySqlCommand cmd)
        {
            //02:标识(Id)
            cmd.Parameters.Add(new MySqlParameter("Id",MySqlDbType.Int32){ Value = entity.Id});
            //05:主键(MasterId)
            cmd.Parameters.Add(new MySqlParameter("MasterId",MySqlDbType.Int32){ Value = entity.MasterId});
            //06:关联(SlaveId)
            cmd.Parameters.Add(new MySqlParameter("SlaveId",MySqlDbType.Int32){ Value = entity.SlaveId});
        }


        /// <summary>
        /// 设置更新数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        protected sealed override void SetUpdateCommand(SubjectionData entity, MySqlCommand cmd)
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
        protected sealed override bool SetInsertCommand(SubjectionData entity, MySqlCommand cmd)
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
        internal static MySqlDataTableScope<SubjectionData> CreateScope()
        {
            var db = Authorities.Default ?? new Authorities();
            return MySqlDataTableScope<SubjectionData>.CreateScope(db, db.Subjections);
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
    `master_id` AS `MasterId`,
    `slave_id` AS `SlaveId`";
            }
        }


        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public override void SimpleLoad(MySqlDataReader reader,SubjectionData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._id = (int)reader.GetInt32(0);
                entity._masterid = (int)reader.GetInt32(1);
                entity._slaveid = (int)reader.GetInt32(2);
            }
        }
        #endregion

    }

    partial class Authorities
    {


        /// <summary>
        /// 行级权限关联(tb_sys_subjection):行级权限关联
        /// </summary>
        public const int Table_Subjection = 0x50009;


        /// <summary>
        /// 行级权限关联的结构语句
        /// </summary>
        private TableSql _tb_sys_subjectionSql = new TableSql
        {
            TableName = "tb_sys_subjection",
            PimaryKey = "Id"
        };


        /// <summary>
        /// 行级权限关联数据访问对象
        /// </summary>
        private SubjectionDataAccess _subjections;

        /// <summary>
        /// 行级权限关联数据访问对象
        /// </summary>
        internal SubjectionDataAccess Subjections
        {
            get
            {
                return this._subjections ?? ( this._subjections = new SubjectionDataAccess{ DataBase = this});
            }
        }
    }
}