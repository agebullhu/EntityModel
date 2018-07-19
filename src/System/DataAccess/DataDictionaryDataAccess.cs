// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-12
// // *****************************************************/

#region 引用

using System;
using MySql.Data.MySqlClient;
using Gboxt.Common.DataModel.MySql;

#endregion

namespace Gboxt.Common.SystemModel.DataAccess
{
    /// <summary>
    ///     数据字典
    /// </summary>
    sealed partial class DataDictionaryDataAccess : MySqlTable<DataDictionaryData, SystemDb>
    {
        /// <summary>
        /// 保存使用的SQL语句
        /// </summary>
        private const string SaveSql = @"
IF EXISTS(SELECT * FROM [ST_Dictionary] WHERE [Name] = @Name)
BEGIN
    UPDATE [ST_Dictionary] SET 
                   [Value] = @Value
    WHERE [Name] = @Name
END
ELSE
BEGIN
    INSERT INTO [ST_Dictionary]
          ([Name],   [Value],  [State])
    VALUES(@Name ,   @Value ,  0);
END";
        /// <summary>
        /// 查询用的SQL语句
        /// </summary>
        private const string FindSql = @"
SELECT TOP 1 [Value]
  FROM [ST_Dictionary]
 WHERE [Name] = @Name;";

        /// <summary>
        ///     写值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Save(string name, string value)
        {
            var pars = new[]
            {
                new MySqlParameter("@Name", MySqlDbType.VarChar, 500),
                new MySqlParameter("@Value", MySqlDbType.VarChar, -1)
            };

            pars[0].Value = name;
            pars[1].Value = value;

            DataBase.Execute(SaveSql, pars);
        }

        /// <summary>
        ///     取值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Get(string name)
        {
            var pars = new[]
            {
                new MySqlParameter("@Name", MySqlDbType.VarChar, 500)
            };
            pars[0].Value = name;
            var value = DataBase.ExecuteScalar(FindSql, pars);
            return value == null || value == DBNull.Value ? null : value.ToString();
        }
    }
}