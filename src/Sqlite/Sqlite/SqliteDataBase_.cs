using Agebull.EntityModel.Common;
using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Data.Common;

namespace Agebull.EntityModel.Sqlite
{
    /// <summary>
    ///     表示Sqlite数据库对象
    /// </summary>
    public class SqliteDataBase_ : SimpleConfig, IParameterCreater
    {
        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="csharpType">C#的类型</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static SqliteParameter CreateParameter(string csharpType, string parameterName, object value)
        {
            if (value is Enum)
            {
                return new SqliteParameter(parameterName, SqliteType.Integer)
                {
                    Value = Convert.ToInt32(value)
                };
            }
            if (value is bool b)
            {
                return new SqliteParameter(parameterName, SqliteType.Integer)
                {
                    Value = b ? 1 : 0
                };
            }
            return CreateParameter(parameterName, value, ToSqlDbType(csharpType));
        }

        DbParameter IParameterCreater.CreateParameter(string parameterName, object value)
        {
            return CreateParameter(parameterName, value);
        }

        DbParameter IParameterCreater.CreateParameter(string parameterName, string value)
        {
            return CreateParameter(parameterName, value);
        }

        DbParameter IParameterCreater.CreateParameter<T>(string parameterName, T value)
        {
            return CreateParameter(parameterName, value);
        }


        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">类型</param>
        /// <returns>参数</returns>
        public static SqliteParameter CreateParameter(string parameterName, object value, SqliteType type)
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
                    val = b ? 1 : 0;
                    break;
                default:
                    val = value;
                    break;
                case string s:
                    return CreateParameter(parameterName, s);
            }

            return new SqliteParameter(parameterName, type)
            {
                Value = val
            };
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
        public static SqliteParameter CreateParameter(string parameterName, object value)
        {
            return CreateParameter(parameterName, value, ToSqlDbType(value?.GetType().Name));
        }

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static SqliteParameter CreateParameter(string parameterName, string value)
        {
            return new SqliteParameter(parameterName, SqliteType.Text, value.Length)
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
        public static SqliteParameter CreateParameter(string parameterName, bool value)
        {
            return new SqliteParameter(parameterName, SqliteType.Integer)
            {
                Value = value ? 1 : 0
            };
        }

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static SqliteParameter CreateParameter<T>(string parameterName, T value)
            where T : struct
        {
            if (value is Enum)
            {
                return new SqliteParameter(parameterName, SqliteType.Integer)
                {
                    Value = Convert.ToInt32(value)
                };
            }
            return new SqliteParameter(parameterName, ToSqlDbType(typeof(T).Name))
            {
                Value = value
            };
        }


        /// <summary>
        ///     从C#的类型转为DBType
        /// </summary>
        /// <param name="csharpType"> </param>
        public static SqliteType ToSqlDbType(string csharpType)
        {
            switch (csharpType)
            {
                case "Boolean":
                case "bool":
                case "byte":
                case "Byte":
                case "sbyte":
                case "SByte":
                case "Char":
                case "char":
                case "short":
                case "Int16":
                case "ushort":
                case "UInt16":
                case "int":
                case "Int32":
                case "IntPtr":
                case "uint":
                case "UInt32":
                case "UIntPtr":
                case "long":
                case "Int64":
                case "ulong":
                case "UInt64":
                    return SqliteType.Integer;
                case "float":
                case "Float":
                case "double":
                case "Double":
                case "decimal":
                case "Decimal":
                    return SqliteType.Real;
                case "Binary":
                case "byte[]":
                case "Byte[]":
                    return SqliteType.Blob;
                default:
                //case "Guid":
                //case "DateTime":
                //case "String":
                //case "string":
                    return SqliteType.Text;
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