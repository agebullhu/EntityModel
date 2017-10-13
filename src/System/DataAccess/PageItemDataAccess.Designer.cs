
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
    /// 页面节点
    /// </summary>
    public partial class PageItemDataAccess
    {
        #region 基本SQL语句

        /// <summary>
        /// 表的唯一标识
        /// </summary>
        public override int TableId
        {
            get { return (int)SystemDb.EnumTables.PageItem; }
        }

        /// <summary>
        /// 读取表名
        /// </summary>
        protected sealed override string ReadTableName
        {
            get
            {
                return @"tb_sys_page_item";
            }
        }

        /// <summary>
        /// 写入表名
        /// </summary>
        protected sealed override string WriteTableName
        {
            get
            {
                return @"tb_sys_page_item";
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
    `ID` AS `ID`,
    `ItemType` AS `ItemType`,
    `Name` AS `Name`,
    `Caption` AS `Caption`,
    `Icon` AS `Icon`,
    `Json` AS `Json`,
    `Url` AS `Url`,
    `Memo` AS `Memo`,
    `ParentId` AS `ParentId`,
    `ExtendValue` AS `ExtendValue`,
    `Index` AS `Index`";
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
INSERT INTO `tb_sys_page_item`
(
    `ItemType`,
    `Name`,
    `Caption`,
    `Icon`,
    `Json`,
    `Url`,
    `Memo`,
    `ParentId`,
    `ExtendValue`,
    `Index`
)
VALUES
(
    ?ItemType,
    ?Name,
    ?Caption,
    ?Icon,
    ?Json,
    ?Url,
    ?Memo,
    ?ParentId,
    ?ExtendValue,
    ?Index
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
UPDATE `tb_sys_page_item` SET
       `ItemType` = ?ItemType,
       `Name` = ?Name,
       `Caption` = ?Caption,
       `Icon` = ?Icon,
       `Json` = ?Json,
       `Url` = ?Url,
       `Memo` = ?Memo,
       `ParentId` = ?ParentId,
       `ExtendValue` = ?ExtendValue,
       `Index` = ?Index
 WHERE `ID` = ?ID;";
            }
        }

        /// <summary>
        /// 取得仅更新的SQL语句
        /// </summary>
        internal string GetModifiedSqlCode(PageItemData data)
        {
            if (data.__EntityStatusNull || !data.__EntityStatus.IsModified)
                return ";";
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE [tb_sys_page_item] SET");
            //节点类型
            if (data.__EntityStatus.ModifiedProperties[PageItemData.Real_ItemType] > 0)
                sql.AppendLine("       [ItemType] = @ItemType");
            //名称
            if (data.__EntityStatus.ModifiedProperties[PageItemData.Real_Name] > 0)
                sql.AppendLine("       [Name] = @Name");
            //标题
            if (data.__EntityStatus.ModifiedProperties[PageItemData.Real_Caption] > 0)
                sql.AppendLine("       [Caption] = @Caption");
            //图标
            if (data.__EntityStatus.ModifiedProperties[PageItemData.Real_Icon] > 0)
                sql.AppendLine("       [Icon] = @Icon");
            //Json内容
            if (data.__EntityStatus.ModifiedProperties[PageItemData.Real_Json] > 0)
                sql.AppendLine("       [Json] = @Json");
            //页面连接
            if (data.__EntityStatus.ModifiedProperties[PageItemData.Real_Url] > 0)
                sql.AppendLine("       [Url] = @Url");
            //备注
            if (data.__EntityStatus.ModifiedProperties[PageItemData.Real_Memo] > 0)
                sql.AppendLine("       [Memo] = @Memo");
            //上级节点
            if (data.__EntityStatus.ModifiedProperties[PageItemData.Real_ParentId] > 0)
                sql.AppendLine("       [ParentId] = @ParentId");
            //扩展内容
            if (data.__EntityStatus.ModifiedProperties[PageItemData.Real_ExtendValue] > 0)
                sql.AppendLine("       [ExtendValue] = @ExtendValue");
            //扩展内容
            if (data.__EntityStatus.ModifiedProperties[PageItemData.Real_ExtendValue] > 0)
                sql.AppendLine("       [Index] = @Index");
            sql.Append(" WHERE [ID] = @ID;");
            return sql.ToString();
        }

        #endregion

        #region 字段

        /// <summary>
        ///  所有字段
        /// </summary>
        static string[] _fields = new string[]{ "ID", "Index", "ItemType","Name","Caption","Icon","Json","Url","Memo","ParentId","ExtendValue" };

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
            { "Index" , "Index" },
            { "ItemType" , "ItemType" },
            { "Name" , "Name" },
            { "Caption" , "Caption" },
            { "Icon" , "Icon" },
            { "Json" , "Json" },
            { "Url" , "Url" },
            { "Memo" , "Memo" },
            { "ParentId" , "ParentId" },
            { "ExtendValue" , "ExtendValue" }
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
        protected sealed override void LoadEntity(MySqlDataReader reader,PageItemData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._id = (int)reader.GetInt32(0);
                entity._itemtype = (PageItemType)reader.GetInt32(1);
                if (!reader.IsDBNull(2))
                    entity._name = reader.GetString(2);
                if (!reader.IsDBNull(3))
                    entity._caption = reader.GetString(3);
                if (!reader.IsDBNull(4))
                    entity._icon = reader.GetString(4);
                if (!reader.IsDBNull(5))
                    entity._json = reader.GetString(5);
                if (!reader.IsDBNull(6))
                    entity._url = reader.GetString(6);
                if (!reader.IsDBNull(7))
                    entity._memo = reader.GetString(7);
                entity._parentid = (int)reader.GetInt32(8);
                if (!reader.IsDBNull(9))
                    entity._extendvalue = reader.GetString(9);
                if (!reader.IsDBNull(10))
                    entity._index = reader.GetInt32(10);
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
                case "Index":
                    return MySqlDbType.Int32;
                case "ItemType":
                    return MySqlDbType.Int32;
                case "Name":
                    return MySqlDbType.VarChar;
                case "Caption":
                    return MySqlDbType.VarChar;
                case "Icon":
                    return MySqlDbType.VarChar;
                case "Json":
                    return MySqlDbType.VarChar;
                case "Url":
                    return MySqlDbType.VarChar;
                case "Memo":
                    return MySqlDbType.VarChar;
                case "ParentId":
                    return MySqlDbType.Int32;
                case "ExtendValue":
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
        private void CreateFullSqlParameter(PageItemData entity, MySqlCommand cmd)
        {
            //01:标识(ID)
            cmd.Parameters.Add(new MySqlParameter("ID",MySqlDbType.Int32){ Value = entity.ID});
            //02:节点类型(ItemType)
            cmd.Parameters.Add(new MySqlParameter("ItemType",MySqlDbType.Int32){ Value = entity.ItemType});
            //03:名称(Name)
            var isNull = string.IsNullOrWhiteSpace(entity.Name);
            var parameter = new MySqlParameter("Name",MySqlDbType.VarChar , isNull ? 10 : (entity.Name).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Name;
            cmd.Parameters.Add(parameter);
            //04:标题(Caption)
            isNull = string.IsNullOrWhiteSpace(entity.Caption);
            parameter = new MySqlParameter("Caption",MySqlDbType.VarChar , isNull ? 10 : (entity.Caption).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Caption;
            cmd.Parameters.Add(parameter);
            //05:图标(Icon)
            isNull = string.IsNullOrWhiteSpace(entity.Icon);
            parameter = new MySqlParameter("Icon",MySqlDbType.VarChar , isNull ? 10 : (entity.Icon).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Icon;
            cmd.Parameters.Add(parameter);
            //06:Json内容(Json)
            isNull = string.IsNullOrWhiteSpace(entity.Json);
            parameter = new MySqlParameter("Json",MySqlDbType.VarChar , isNull ? 10 : (entity.Json).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Json;
            cmd.Parameters.Add(parameter);
            //07:扩展内容(ExtendValue)
            isNull = string.IsNullOrWhiteSpace(entity.ExtendValue);
            parameter = new MySqlParameter("ExtendValue",MySqlDbType.VarChar , isNull ? 10 : (entity.ExtendValue).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.ExtendValue;
            cmd.Parameters.Add(parameter);
            //08:页面连接(Url)
            isNull = string.IsNullOrWhiteSpace(entity.Url);
            parameter = new MySqlParameter("Url",MySqlDbType.VarChar , isNull ? 10 : (entity.Url).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Url;
            cmd.Parameters.Add(parameter);
            //08:备注(Memo)
            isNull = string.IsNullOrWhiteSpace(entity.Memo);
            parameter = new MySqlParameter("Memo",MySqlDbType.VarChar , isNull ? 10 : (entity.Memo).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Memo;
            cmd.Parameters.Add(parameter);
            //09:上级节点(ParentId)
            cmd.Parameters.Add(new MySqlParameter("ParentId", MySqlDbType.Int32) { Value = entity.ParentId });
            //09:上级节点(ParentId)
            cmd.Parameters.Add(new MySqlParameter("Index", MySqlDbType.Int32) { Value = entity.Index });
        }


        /// <summary>
        /// 设置更新数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        protected sealed override void SetUpdateCommand(PageItemData entity, MySqlCommand cmd)
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
        protected sealed override bool SetInsertCommand(PageItemData entity, MySqlCommand cmd)
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
        internal static MySqlDataTableScope<PageItemData> CreateScope()
        {
            var db = SystemDb.Default ?? new SystemDb();
            return MySqlDataTableScope<PageItemData>.CreateScope(db, db.PageItems);
        }
        #endregion

    }

    sealed partial class SystemDb
    {


        /// <summary>
        /// 页面节点的结构语句
        /// </summary>
        private TableSql _st_pageitemSql = new TableSql
        {
            TableName = "tb_sys_page_item",
            PimaryKey = "ID"
        };


        /// <summary>
        /// 页面节点数据访问对象
        /// </summary>
        private PageItemDataAccess _pageItems;

        /// <summary>
        /// 页面节点数据访问对象
        /// </summary>
        public PageItemDataAccess PageItems
        {
            get
            {
                return this._pageItems ?? ( this._pageItems = new PageItemDataAccess{ DataBase = this});
            }
        }
    }
}
