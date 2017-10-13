
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
    /// 数据字典
    /// </summary>
    public partial class DataDictionaryDataAccess
    {
        #region 基本SQL语句

        /// <summary>
        /// 表的唯一标识
        /// </summary>
        public override int TableId
        {
            get { return (int)SystemDb.EnumTables.ST_Dictionary; }
        }

        /// <summary>
        /// 读取表名
        /// </summary>
        protected sealed override string ReadTableName
        {
            get
            {
                return @"ST_Dictionary";
            }
        }

        /// <summary>
        /// 写入表名
        /// </summary>
        protected sealed override string WriteTableName
        {
            get
            {
                return @"ST_Dictionary";
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
    [Id] AS [Id],
    [Name] AS [Name],
    [State] AS [State],
    [Feature] AS [Feature],
    [Memo] AS [Memo],
    [Value] AS [Value]";
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
INSERT INTO [ST_Dictionary]
(
    [Name],
    [State],
    [Feature],
    [Memo],
    [Value]
)
VALUES
(
    @Name,
    @State,
    @Feature,
    @Memo,
    @Value
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
UPDATE [ST_Dictionary] SET
       [Name] = @Name,
       [State] = @State,
       [Feature] = @Feature,
       [Memo] = @Memo,
       [Value] = @Value
 WHERE [Id] = @Id;";
            }
        }

        /// <summary>
        /// 取得仅更新的SQL语句
        /// </summary>
        internal string GetModifiedSqlCode(DataDictionaryData data)
        {
            if (data.__EntityStatusNull || !data.__EntityStatus.IsModified)
                return ";";
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE [ST_Dictionary] SET");
            //名称
            if (data.__EntityStatus.ModifiedProperties[DataDictionaryData.Real_Name] > 0)
                sql.AppendLine("       [Name] = @Name");
            //状态
            if (data.__EntityStatus.ModifiedProperties[DataDictionaryData.Real_State] > 0)
                sql.AppendLine("       [State] = @State");
            //特性
            if (data.__EntityStatus.ModifiedProperties[DataDictionaryData.Real_Feature] > 0)
                sql.AppendLine("       [Feature] = @Feature");
            //备注
            if (data.__EntityStatus.ModifiedProperties[DataDictionaryData.Real_Memo] > 0)
                sql.AppendLine("       [Memo] = @Memo");
            //值
            if (data.__EntityStatus.ModifiedProperties[DataDictionaryData.Real_Value] > 0)
                sql.AppendLine("       [Value] = @Value");
            sql.Append(" WHERE [Id] = @Id;");
            return sql.ToString();
        }

        #endregion

        #region 字段

        /// <summary>
        ///  所有字段
        /// </summary>
        static string[] _fields = new string[]{ "Id","Name","State","Feature","Memo","Value" };

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
            { "Id" , "Id" },
            { "Name" , "Name" },
            { "State" , "State" },
            { "Feature" , "Feature" },
            { "Memo" , "Memo" },
            { "Value" , "Value" }
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
        protected sealed override void LoadEntity(MySqlDataReader reader,DataDictionaryData entity)
        {
            using (new EditScope(entity.__EntityStatus, EditArrestMode.All, false))
            {
                entity._id = (int)reader.GetInt32(0);
                if (!reader.IsDBNull(1))
                    entity._name = reader.GetString(1);
                entity._state = (long)reader.GetInt64(2);
                if (!reader.IsDBNull(3))
                    entity._feature = reader.GetString(3);
                if (!reader.IsDBNull(4))
                    entity._memo = reader.GetString(4);
                if (!reader.IsDBNull(5))
                    entity._value = reader.GetString(5);
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
                case "Name":
                    return MySqlDbType.VarChar;
                case "State":
                    return MySqlDbType.Int64;
                case "Feature":
                    return MySqlDbType.VarChar;
                case "Memo":
                    return MySqlDbType.VarChar;
                case "Value":
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
        private void CreateFullSqlParameter(DataDictionaryData entity, MySqlCommand cmd)
        {
            //01:标识(Id)
            cmd.Parameters.Add(new MySqlParameter("Id",MySqlDbType.Int32){ Value = entity.Id});
            //02:名称(Name)
            var isNull = string.IsNullOrWhiteSpace(entity.Name);
            var parameter = new MySqlParameter("Name",MySqlDbType.VarChar , isNull ? 10 : (entity.Name).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Name;
            cmd.Parameters.Add(parameter);
            //03:状态(State)
            cmd.Parameters.Add(new MySqlParameter("State",MySqlDbType.Int64){ Value = entity.State});
            //04:特性(Feature)
            isNull = string.IsNullOrWhiteSpace(entity.Feature);
            parameter = new MySqlParameter("Feature",MySqlDbType.VarChar , isNull ? 10 : (entity.Feature).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Feature;
            cmd.Parameters.Add(parameter);
            //05:备注(Memo)
            isNull = string.IsNullOrWhiteSpace(entity.Memo);
            parameter = new MySqlParameter("Memo",MySqlDbType.VarChar , isNull ? 10 : (entity.Memo).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Memo;
            cmd.Parameters.Add(parameter);
            //06:值(Value)
            isNull = string.IsNullOrWhiteSpace(entity.Value);
            parameter = new MySqlParameter("Value",MySqlDbType.VarChar , isNull ? 10 : (entity.Value).Length);
            if(isNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = entity.Value;
            cmd.Parameters.Add(parameter);
        }


        /// <summary>
        /// 设置更新数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        protected sealed override void SetUpdateCommand(DataDictionaryData entity, MySqlCommand cmd)
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
        protected sealed override bool SetInsertCommand(DataDictionaryData entity, MySqlCommand cmd)
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
        internal static MySqlDataTableScope<DataDictionaryData> CreateScope()
        {
            var db = SystemDb.Default ?? new SystemDb();
            return MySqlDataTableScope<DataDictionaryData>.CreateScope(db, db.DataDictionaries);
        }
        #endregion

    }

    sealed partial class SystemDb
    {


        /// <summary>
        /// 数据字典的结构语句
        /// </summary>
        private TableSql _ST_DictionarySql = new TableSql
        {
            TableName = "ST_Dictionary",
            PimaryKey = "Id"
        };


        /// <summary>
        /// 数据字典数据访问对象
        /// </summary>
        private DataDictionaryDataAccess _dataDictionaries;

        /// <summary>
        /// 数据字典数据访问对象
        /// </summary>
        public DataDictionaryDataAccess DataDictionaries
        {
            get
            {
                return this._dataDictionaries ?? ( this._dataDictionaries = new DataDictionaryDataAccess{ DataBase = this});
            }
        }
    }
}
