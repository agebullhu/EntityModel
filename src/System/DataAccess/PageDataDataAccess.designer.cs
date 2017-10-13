
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using Gboxt.Common.DataModel;

using Gboxt.Common.DataModel.MySql;


namespace Gboxt.Common.SystemModel.DataAccess
{
    /// <summary>
    /// 用户的页面数据
    /// </summary>
    public partial class PageDataDataAccess
    {
        #region 基本SQL语句

        /// <summary>
        /// 表的唯一标识
        /// </summary>
        public override int TableId
        {
            get { return (int)SystemDb.EnumTables.ST_PageData; }
        }

        /// <summary>
        /// 读取表名
        /// </summary>
        protected sealed override string ReadTableName
        {
            get
            {
                return @"ST_PageData";
            }
        }

        /// <summary>
        /// 写入表名
        /// </summary>
        protected sealed override string WriteTableName
        {
            get
            {
                return @"ST_PageData";
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
    [ID] AS [ID],
    [UserId] AS [UserId],
    [PageId] AS [PageId],
    [PageData] AS [PageData]";
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
INSERT INTO [ST_PageData]
(
    [UserId],
    [PageId],
    [PageData]
)
VALUES
(
    @UserId,
    @PageId,
    @PageData
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
UPDATE [ST_PageData] SET
       [UserId] = @UserId,
       [PageId] = @PageId,
       [PageData] = @PageData
 WHERE [ID] = @ID;";
            }
        }

        /// <summary>
        /// 取得仅更新的SQL语句
        /// </summary>
        internal string GetModifiedSqlCode(PageDataData data)
        {
            if (data.__EntityStatusNull || !data.__EntityStatus.IsModified)
                return ";";
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE [ST_PageData] SET");
            //用户标识
            if (data.__EntityStatus.ModifiedProperties[PageDataData.Real_UserId] > 0)
                sql.AppendLine("       [UserId] = @UserId");
            //页面标识
            if (data.__EntityStatus.ModifiedProperties[PageDataData.Real_PageId] > 0)
                sql.AppendLine("       [PageId] = @PageId");
            //页面数据
            if (data.__EntityStatus.ModifiedProperties[PageDataData.Real_PageData] > 0)
                sql.AppendLine("       [PageData] = @PageData");
            sql.Append(" WHERE [ID] = @ID;");
            return sql.ToString();
        }

        #endregion

        #region 字段

        /// <summary>
        ///  所有字段
        /// </summary>
        static string[] _fields = new string[]{ "ID","UserId","PageId","PageData" };

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
            { "ID" , "ID" },
            { "UserId" , "UserId" },
            { "PageId" , "PageId" },
            { "PageData" , "PageData" }
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
        protected sealed override void LoadEntity(MySqlDataReader reader,PageDataData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._id = (int)reader.GetInt32(0);
                entity._userid = (int)reader.GetInt32(1);
                entity._pageid = (int)reader.GetInt32(2);
                if (!reader.IsDBNull(3))
                    entity._pagedata = reader.GetString(3);
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
                case "UserId":
                    return MySqlDbType.Int32;
                case "PageId":
                    return MySqlDbType.Int32;
                case "PageData":
                    return MySqlDbType.VarChar;
            }
            return MySqlDbType.VarChar;
        }


        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        private void CreateFullSqlParameter(PageDataData entity, MySqlCommand cmd)
        {
            //01:标识(ID)
            cmd.Parameters.Add(new MySqlParameter("ID",MySqlDbType.Int32){ Value = entity.ID});
            //02:用户标识(UserId)
            cmd.Parameters.Add(new MySqlParameter("UserId",MySqlDbType.Int32){ Value = entity.UserId});
            //03:页面标识(PageId)
            cmd.Parameters.Add(new MySqlParameter("PageId",MySqlDbType.Int32){ Value = entity.PageId});
            //04:页面数据(PageData)
            var isNull = string.IsNullOrWhiteSpace(entity.PageData);
            var parameter = new MySqlParameter("PageData",MySqlDbType.VarChar , isNull ? 10 : (entity.PageData).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.PageData;
            cmd.Parameters.Add(parameter);
        }


        /// <summary>
        /// 设置更新数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        protected sealed override void SetUpdateCommand(PageDataData entity, MySqlCommand cmd)
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
        protected sealed override bool SetInsertCommand(PageDataData entity, MySqlCommand cmd)
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
            return SystemDb.Default ?? new SystemDb();
        }
        
        /// <summary>
        /// 生成数据库访问范围
        /// </summary>
        internal static MySqlDataTableScope<PageDataData> CreateScope()
        {
            var db = SystemDb.Default ?? new SystemDb();
            return MySqlDataTableScope<PageDataData>.CreateScope(db, db.PageDatas);
        }
        #endregion

    }

    sealed partial class SystemDb
    {


        /// <summary>
        /// 用户的页面数据的结构语句
        /// </summary>
        private TableSql _ST_PageDataSql = new TableSql
        {
            TableName = "ST_PageData",
            PimaryKey = "ID"
        };


        /// <summary>
        /// 用户的页面数据数据访问对象
        /// </summary>
        private PageDataDataAccess _pageDatas;

        /// <summary>
        /// 用户的页面数据数据访问对象
        /// </summary>
        public PageDataDataAccess PageDatas
        {
            get
            {
                return this._pageDatas ?? ( this._pageDatas = new PageDataDataAccess{ DataBase = this});
            }
        }
    }
}
