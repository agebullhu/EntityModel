using Agebull.EntityModel.Common;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Agebull.EntityModel.SqlServer
{
    /// <summary>
    ///     表示SQL SERVER数据库对象
    /// </summary>
    public class SqlServerDataBase_ : SimpleConfig, IParameterCreater
    {
        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">类型</param>
        /// <returns>参数</returns>
        public static SqlParameter CreateParameter(string parameterName, object value, SqlDbType type)
        {
            switch (value)
            {
                case string s:
                    return CreateParameter(parameterName, s);
                case Enum _:
                    return new SqlParameter(parameterName, SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(value)
                    };
                case bool _:
                    return new SqlParameter(parameterName, SqlDbType.Bit)
                    {
                        Value = (bool)value ? (byte)1 : (byte)0
                    };
                case null:
                    return new SqlParameter(parameterName, type)
                    {
                        Value = DBNull.Value
                    };
                default:
                    return new SqlParameter(parameterName, type)
                    {
                        Value = value
                    };
            }

        }

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="csharpType">C#的类型</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static SqlParameter CreateParameter(string csharpType, string parameterName, object value)
        {
            return new SqlParameter(parameterName, ToSqlDbType(csharpType))
            {
                Value = value
            };
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
        public static SqlParameter CreateParameter(string parameterName, object value)
        {
            switch (value)
            {
                case string s:
                    return CreateParameter(parameterName, s);
                case null:
                    return new SqlParameter(parameterName, SqlDbType.NVarChar)
                    {
                        Value = DBNull.Value
                    };
            }

            return new SqlParameter(parameterName, ToSqlDbType(value.GetType().Name))
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
        public static SqlParameter CreateParameter(string parameterName, string value)
        {
            return new SqlParameter(parameterName, SqlDbType.NVarChar, string.IsNullOrWhiteSpace(value) ? 10 : value.Length)
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
        public static SqlParameter CreateParameter<T>(string parameterName, T value)
        {
            return new SqlParameter(parameterName, ToSqlDbType(typeof(T).Name))
            {
                Value = value
            };
        }


        /// <summary>
        ///     从C#的类型转为DBType
        /// </summary>
        /// <param name="csharpType"> </param>
        public static SqlDbType ToSqlDbType(string csharpType)
        {
            switch (csharpType)
            {
                case "Boolean":
                case "bool":
                    return SqlDbType.Bit;
                case "byte":
                case "Byte":
                case "sbyte":
                case "SByte":
                    return SqlDbType.TinyInt;
                case "Char":
                case "char":
                    return SqlDbType.NChar;
                case "short":
                case "Int16":
                case "ushort":
                case "UInt16":
                    return SqlDbType.SmallInt;
                case "int":
                case "Int32":
                case "IntPtr":
                case "uint":
                case "UInt32":
                case "UIntPtr":
                    return SqlDbType.Int;
                case "long":
                case "Int64":
                case "ulong":
                case "UInt64":
                    return SqlDbType.BigInt;
                case "float":
                case "Float":
                    return SqlDbType.Float;
                case "double":
                case "Double":
                    return SqlDbType.Real;
                case "decimal":
                case "Decimal":
                    return SqlDbType.Decimal;
                case "Guid":
                    return SqlDbType.UniqueIdentifier;
                case "DateTime":
                    return SqlDbType.DateTime;
                case "String":
                case "string":
                    return SqlDbType.NVarChar;
                case "Binary":
                case "byte[]":
                case "Byte[]":
                    return SqlDbType.Binary;
                default:
                    return SqlDbType.Binary;
            }
        }

        /// <summary>
        ///     从C#的类型转为DbType
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