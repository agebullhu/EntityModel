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
    /// 角色权限
    /// </summary>
    public partial class RolePowerDataAccess
    {

        #region 基本SQL语句

        /// <summary>
        /// 表的唯一标识
        /// </summary>
        public override int TableId
        {
            get { return Authorities.Table_RolePower; }
        }

        /// <summary>
        /// 读取表名
        /// </summary>
        protected sealed override string ReadTableName
        {
            get
            {
                return @"tb_sys_role_power";
            }
        }

        /// <summary>
        /// 写入表名
        /// </summary>
        protected sealed override string WriteTableName
        {
            get
            {
                return @"tb_sys_role_power";
            }
        }

        /// <summary>
        /// 主键
        /// </summary>
        protected sealed override string PrimaryKey
        {
            get
            {
                return @"ID";
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
    `id` AS `ID`,
    `page_item_id` AS `PageItemId`,
    `role_id` AS `RoleId`,
    `power` AS `Power`,
    `data_scope` AS `DataScope`";
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
INSERT INTO `tb_sys_role_power`
(
    `page_item_id`,
    `role_id`,
    `power`,
    `data_scope`
)
VALUES
(
    ?PageItemId,
    ?RoleId,
    ?Power,
    ?DataScope
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
UPDATE `tb_sys_role_power` SET
       `page_item_id` = ?PageItemId,
       `role_id` = ?RoleId,
       `power` = ?Power,
       `data_scope` = ?DataScope
 WHERE `id` = ?ID;";
            }
        }

        /// <summary>
        /// 取得仅更新的SQL语句
        /// </summary>
        internal string GetModifiedSqlCode(RolePowerData data)
        {
            if (data.__EntityStatusNull || !data.__EntityStatus.IsModified)
                return ";";
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE `tb_sys_role_power` SET");
            //页面标识
            if (data.__EntityStatus.ModifiedProperties[RolePowerData.Real_PageItemId] > 0)
                sql.AppendLine("       `page_item_id` = ?PageItemId");
            //角色标识
            if (data.__EntityStatus.ModifiedProperties[RolePowerData.Real_RoleId] > 0)
                sql.AppendLine("       `role_id` = ?RoleId");
            //权限
            if (data.__EntityStatus.ModifiedProperties[RolePowerData.Real_Power] > 0)
                sql.AppendLine("       `power` = ?Power");
            //权限范围
            if (data.__EntityStatus.ModifiedProperties[RolePowerData.Real_DataScope] > 0)
                sql.AppendLine("       `data_scope` = ?DataScope");
            sql.Append(" WHERE `id` = ?ID;");
            return sql.ToString();
        }

        #endregion


        #region 字段

        /// <summary>
        ///  所有字段
        /// </summary>
        static string[] _fields = new string[]{ "ID","PageItemId","RoleId","Power","DataScope" };

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
            { "ID" , "id" },
            { "PageItemId" , "page_item_id" },
            { "page_item_id" , "page_item_id" },
            { "RoleId" , "role_id" },
            { "role_id" , "role_id" },
            { "Power" , "power" },
            { "DataScope" , "data_scope" },
            { "data_scope" , "data_scope" }
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
        protected sealed override void LoadEntity(MySqlDataReader reader,RolePowerData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._id = (int)reader.GetInt32(0);
                entity._pageitemid = (int)reader.GetInt32(1);
                entity._roleid = (int)reader.GetInt32(2);
                entity._power = (int)reader.GetInt32(3);
                if (!reader.IsDBNull(4))
                    entity._datascope = (PositionDataScopeType)reader.GetInt32(4);
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
                case "ID":
                    return MySqlDbType.Int32;
                case "PageItemId":
                    return MySqlDbType.Int32;
                case "RoleId":
                    return MySqlDbType.Int32;
                case "Power":
                    return MySqlDbType.Int32;
                case "DataScope":
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
        private void CreateFullSqlParameter(RolePowerData entity, MySqlCommand cmd)
        {
            //02:标识(ID)
            cmd.Parameters.Add(new MySqlParameter("ID",MySqlDbType.Int32){ Value = entity.ID});
            //04:页面标识(PageItemId)
            cmd.Parameters.Add(new MySqlParameter("PageItemId",MySqlDbType.Int32){ Value = entity.PageItemId});
            //05:角色标识(RoleId)
            cmd.Parameters.Add(new MySqlParameter("RoleId",MySqlDbType.Int32){ Value = entity.RoleId});
            //06:权限(Power)
            cmd.Parameters.Add(new MySqlParameter("Power",MySqlDbType.Int32){ Value = entity.Power});
            //07:权限范围(DataScope)
            cmd.Parameters.Add(new MySqlParameter("DataScope",MySqlDbType.Int32){ Value = (int)entity.DataScope});
        }


        /// <summary>
        /// 设置更新数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        protected sealed override void SetUpdateCommand(RolePowerData entity, MySqlCommand cmd)
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
        protected sealed override bool SetInsertCommand(RolePowerData entity, MySqlCommand cmd)
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
        internal static MySqlDataTableScope<RolePowerData> CreateScope()
        {
            var db = Authorities.Default ?? new Authorities();
            return MySqlDataTableScope<RolePowerData>.CreateScope(db, db.RolePowers);
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
    `id` AS `ID`,
    `page_item_id` AS `PageItemId`,
    `role_id` AS `RoleId`,
    `power` AS `Power`,
    `data_scope` AS `DataScope`";
            }
        }


        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public override void SimpleLoad(MySqlDataReader reader,RolePowerData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._id = (int)reader.GetInt32(0);
                entity._pageitemid = (int)reader.GetInt32(1);
                entity._roleid = (int)reader.GetInt32(2);
                entity._power = (int)reader.GetInt32(3);
                if (!reader.IsDBNull(4))
                    entity._datascope = (PositionDataScopeType)reader.GetInt32(4);
            }
        }
        #endregion

    }

    partial class Authorities
    {


        /// <summary>
        /// 角色权限(tb_sys_role_power):角色权限
        /// </summary>
        public const int Table_RolePower = 0x50007;


        /// <summary>
        /// 角色权限的结构语句
        /// </summary>
        private TableSql _tb_sys_role_powerSql = new TableSql
        {
            TableName = "tb_sys_role_power",
            PimaryKey = "ID"
        };


        /// <summary>
        /// 角色权限数据访问对象
        /// </summary>
        private RolePowerDataAccess _rolePowers;

        /// <summary>
        /// 角色权限数据访问对象
        /// </summary>
        public RolePowerDataAccess RolePowers
        {
            get
            {
                return this._rolePowers ?? ( this._rolePowers = new RolePowerDataAccess{ DataBase = this});
            }
        }
    }
}