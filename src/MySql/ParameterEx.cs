/*****************************************************
(c)2016-2021 by ZeroTeam
作者: 胡天水
工程: Agebull.EntityModel.CoreAgebull.DataModel
建立: 忘了日期
修改: -
*****************************************************/

#region 引用

using MySqlConnector;
using System;

#endregion

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    /// 扩展
    /// </summary>
    public static class ParameterEx
    {
        /// <summary>
        /// 转为Mysql参数（空白会转为Null值）
        /// </summary>
        /// <param name="val"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static MySqlParameter ToDbParameter(this string val, string field) =>
            string.IsNullOrWhiteSpace(val)
            ? new MySqlParameter(field, DBNull.Value)
            : new MySqlParameter(field, val);

        /// <summary>
        /// 转为Mysql参数（DateTime.MinValue会转为Null值）
        /// </summary>
        /// <param name="val"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static MySqlParameter ToDbParameter(this DateTime val, string field) =>
            DateTime.MinValue == val
            ? new MySqlParameter(field, DBNull.Value)
            : new MySqlParameter(field, val);

        /// <summary>
        /// 转为Mysql参数
        /// </summary>
        /// <param name="val"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static MySqlParameter ToDbParameter(this bool val, string field) =>
            new MySqlParameter(field, val ? (byte)1 : (byte)0);

        /// <summary>
        /// 转为Mysql参数
        /// </summary>
        /// <param name="val"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static MySqlParameter ToDbParameter(this bool? val, string field) =>
            null == val
            ? new MySqlParameter(field, DBNull.Value)
            : new MySqlParameter(field, val.Value ? (byte)1 : (byte)0);

        /// <summary>
        /// 转为Mysql参数
        /// </summary>
        /// <param name="val"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static MySqlParameter ToDbParameter<T>(this Nullable<T> val, string field)
            where T : struct
            => !val.HasValue
            ? new MySqlParameter(field, DBNull.Value)
            : new MySqlParameter(field, (object)val.Value);


        /// <summary>
        /// 转为Mysql参数
        /// </summary>
        /// <param name="val"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static MySqlParameter ToDbParameter(this object val, string field) =>
            val == null
            ? new MySqlParameter(field, DBNull.Value)
            : new MySqlParameter(field, val);
    }
}