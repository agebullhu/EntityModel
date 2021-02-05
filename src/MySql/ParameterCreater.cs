// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Common;
using MySqlConnector;
using System;
using System.Data;
using System.Data.Common;

#endregion

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    ///     Sql实体访问类
    /// </summary>
    public class ParameterCreater : IParameterCreater
    {
        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="csharpType">C#的类型</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public MySqlParameter CreateParameter(string csharpType, string parameterName, object value)
        {
            if (value is Enum)
            {
                return new MySqlParameter(parameterName, MySqlDbType.Int32)
                {
                    Value = Convert.ToInt32(value)
                };
            }
            if (value is bool b)
            {
                return new MySqlParameter(parameterName, MySqlDbType.Byte)
                {
                    Value = b ? (byte)1 : (byte)0
                };
            }
            return CreateParameter(parameterName, value, ToSqlDbType(csharpType));
        }

        DbParameter IParameterCreater.CreateParameter(string parameterName, object value)
        {
            return new MySqlParameter(parameterName, value);
        }

        DbParameter IParameterCreater.CreateParameter(string parameterName, string value)
        {
            return new MySqlParameter(parameterName, value);
        }

        DbParameter IParameterCreater.CreateParameter<T>(string parameterName, T value)
        {
            return new MySqlParameter(parameterName, value);
        }


        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="dbType">对应数据库的DbType，如MysqlDbType</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        DbParameter IParameterCreater.CreateParameter(string parameterName, object value, int dbType)
        {
            return new MySqlParameter(parameterName, (MySqlDbType)dbType)
            {
                Value = value
            };
            //return CreateParameter(parameterName, value, (MySqlDbType)dbType);
        }

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">类型</param>
        /// <returns>参数</returns>
        public MySqlParameter CreateParameter(string parameterName, object value, MySqlDbType type)
        {
            object val;
            switch (value)
            {
                case null:
                case DBNull _:
                    val = DBNull.Value;
                    break;
                case Enum _:
                    val = Convert.ToInt32(value);
                    break;
                case bool b:
                    val = b ? (byte)1 : (byte)0;
                    break;
                default:
                    val = value;
                    break;
                case string s:
                    return CreateParameter(parameterName, s);
            }

            return new MySqlParameter(parameterName, type)
            {
                Value = val
            };
        }

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数</param>
        /// <param name="dbType">对应数据库的DbType，如MysqlDbType</param>
        /// <returns>参数</returns>
        DbParameter IParameterCreater.CreateParameter(string parameterName, int dbType)
        {
            return new MySqlParameter(parameterName, (MySqlDbType)dbType);
        }


        DbParameter IParameterCreater.CreateParameter(string csharpType, string parameterName, object value)
        {
            return CreateParameter(csharpType, parameterName, value);
        }

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static MySqlParameter CreateParameter(string parameterName, string value)
        {
            MySqlDbType type = MySqlDbType.VarString;
            if (value == null)
            {
                return new MySqlParameter(parameterName, MySqlDbType.VarString, 10);
            }
            if (value.Length >= 4000)
            {
                type = MySqlDbType.Text;
            }
            else if (value.Length >= ushort.MaxValue)
            {
                type = MySqlDbType.LongText;
            }
            return new MySqlParameter(parameterName, type, value.Length)
            {
                Value = value
            };
        }

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static MySqlParameter CreateParameter(string parameterName, bool value)
        {
            return new MySqlParameter(parameterName, MySqlDbType.Byte)
            {
                Value = value ? (byte)1 : (byte)0
            };
        }

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static MySqlParameter CreateParameter<T>(string parameterName, T value)
            where T : struct
        {
            if (value is Enum)
            {
                return new MySqlParameter(parameterName, MySqlDbType.Int32)
                {
                    Value = Convert.ToInt32(value)
                };
            }
            return new MySqlParameter(parameterName, ToSqlDbType(typeof(T).Name))
            {
                Value = value
            };
        }


        /// <summary>
        ///     从C#的类型转为DBType
        /// </summary>
        /// <param name="csharpType"> </param>
        public static MySqlDbType ToSqlDbType(string csharpType)
        {
            switch (csharpType)
            {
                case "Boolean":
                case "bool":
                    return MySqlDbType.Bit;
                case "byte":
                case "Byte":
                case "sbyte":
                case "SByte":
                case "Char":
                case "char":
                    return MySqlDbType.Byte;
                case "short":
                case "Int16":
                case "ushort":
                case "UInt16":
                    return MySqlDbType.Int16;
                case "int":
                case "Int32":
                case "IntPtr":
                case "uint":
                case "UInt32":
                case "UIntPtr":
                    return MySqlDbType.Int32;
                case "long":
                case "Int64":
                case "ulong":
                case "UInt64":
                    return MySqlDbType.Int64;
                case "float":
                case "Float":
                    return MySqlDbType.Float;
                case "double":
                case "Double":
                    return MySqlDbType.Double;
                case "decimal":
                case "Decimal":
                    return MySqlDbType.Decimal;
                case "Guid":
                    return MySqlDbType.Guid;
                case "DateTime":
                    return MySqlDbType.DateTime;
                case "String":
                case "string":
                    return MySqlDbType.VarChar;
                case "Binary":
                case "byte[]":
                case "Byte[]":
                    return MySqlDbType.Binary;
                default:
                    return MySqlDbType.Binary;
            }
        }

        /// <summary>
        ///     从C#的类型转为DBType
        /// </summary>
        /// <param name="csharpType"> </param>
        public static DbType ToDbType(string csharpType)
        {
            switch (csharpType)
            {
                case "Boolean":
                case "bool":
                    return DbType.Boolean;
                case "byte":
                case "Byte":
                    return DbType.Byte;
                case "sbyte":
                case "SByte":
                    return DbType.SByte;
                case "short":
                case "Int16":
                    return DbType.Int16;
                case "ushort":
                case "UInt16":
                    return DbType.UInt16;
                case "int":
                case "Int32":
                case "IntPtr":
                    return DbType.Int32;
                case "uint":
                case "UInt32":
                case "UIntPtr":
                    return DbType.UInt32;
                case "long":
                case "Int64":
                    return DbType.Int64;
                case "ulong":
                case "UInt64":
                    return DbType.UInt64;
                case "float":
                case "Float":
                    return DbType.Single;
                case "double":
                case "Double":
                    return DbType.Double;
                case "decimal":
                case "Decimal":
                    return DbType.Decimal;
                case "Guid":
                    return DbType.Guid;
                case "DateTime":
                    return DbType.DateTime;
                case "Binary":
                case "byte[]":
                case "Byte[]":
                    return DbType.Binary;
                case "string":
                case "String":
                    return DbType.String;
                default:
                    return DbType.String;
            }
        }

    }
}