// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:YhxBank.FundsManagement
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Agebull.Common.Logging;
using Gboxt.Common.DataModel.MySql;
using MySql.Data.MySqlClient;

#endregion

namespace Gboxt.Common.DataModel.SqlServer
{
    /// <summary>
    ///     表示SQL SERVER数据库对象
    /// </summary>
    public class SqlServerDataBase : IDataBase
    {
        #region 事务

        //protected SqlServerDataBase()
        //{
        //    Trace.WriteLine(".ctor", "SqlServerDataBase");
        //}

        /// <summary>
        ///     事务
        /// </summary>
        public SqlTransaction Transaction { get; internal set; }

        #endregion

        #region 引用范围

        /// <summary>
        /// 生成数据库使用范围
        /// </summary>
        /// <returns></returns>
        IDisposable IDataBase.CreateDataBaseScope() => SqlServerDataBaseScope.CreateScope(this);

        /// <summary>
        /// 生成事务范围
        /// </summary>
        /// <returns></returns>
        ITransactionScope IDataBase.CreateTransactionScope() => TransactionScope.CreateScope(this);

        /// <summary>
        ///     引用数量
        /// </summary>
        protected internal int QuoteCount { get; set; }

        #endregion

        #region 线程实例

        /// <summary>
        ///     锁对象
        /// </summary>
        protected static readonly object LockData = new object();

        /// <summary>
        ///     缺省强类型数据库
        /// </summary>
        [ThreadStatic]
        private static SqlServerDataBase _default;

        /// <summary>
        ///     连接对象
        /// </summary>
        public static SqlServerDataBase DefaultDataBase
        {
            get
            {
                if (_default != null)
                {
                    return _default;
                }
                //lock (LockData)
                {
                    //Trace.WriteLine("CreateDefaultFunc", "SqlServerDataBase");
                    return _default = CreateDefaultFunc();
                }
            }
            set { _default = value; }
        }

        /// <summary>
        ///     生成缺省数据库访问对象的方法
        /// </summary>
        public static Func<SqlServerDataBase> CreateDefaultFunc { get; set; }

        #endregion

        #region 连接

        /// <summary>
        ///     连接字符串
        /// </summary>
        private string _connectionString;

        /// <summary>
        ///     连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return _connectionString ?? (_connectionString = LoadConnectionStringSetting());
            }
        }

        /// <summary>
        /// 读取连接字符串
        /// </summary>
        /// <returns></returns>
        protected virtual string LoadConnectionStringSetting()
        {
            return ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }

        /// <summary>
        ///     连接对象
        /// </summary>
        private SqlConnection _connection;

        /// <summary>
        ///     连接对象
        /// </summary>
        public SqlConnection Connection
        {
            get { return _connection; }
            internal set
            {
                _connection = value;
                //Trace.WriteLine(this.GetHashCode(), "Connection");
            }
        }
        /// <summary>
        ///     打开连接
        /// </summary>
        /// <returns>是否打开,是则为此时打开,否则为之前已打开</returns>
        public bool Open()
        {
            //if (_isClosed)
            //{
            //    //throw new Exception("已关闭的数据库对象不能再次使用");
            //}
            bool result = false;
            if (_connection == null)
            {
                result = true;
                _connection = new SqlConnection(ConnectionString);
                //Trace.WriteLine("Create Connection", "SqlServerDataBase");
            }
            else if (string.IsNullOrEmpty(_connection.ConnectionString))
            {
                result = true;
                //Trace.WriteLine("Set ConnectionString", "SqlServerDataBase");
                _connection.ConnectionString = ConnectionString;
            }
            if (_connection.State == ConnectionState.Open)
            {
                return result;
            }
            //Trace.WriteLine(_count++, "Open");
            //Trace.WriteLine("Opened Connection", "SqlServerDataBase");
            _connection.Open();
            return true;
        }

        /// <summary>
        ///     关闭连接
        /// </summary>
        public void Close()
        {
            if (_connection == null)
            {
                return;
            }
            try
            {
                lock (LockData)
                {
                    if (_connection.State == ConnectionState.Open)
                    {
                        //Trace.WriteLine("Close Connection", "SqlServerDataBase");
                        _connection.Close();
                    }
                    //Trace.WriteLine(_count--, "Close");
                    _connection = null;
                }
            }
            catch (Exception exception)
            {
                _connection?.Dispose();
                Trace.WriteLine("Close Error", "SqlServerDataBase");
                LogRecorder.Error(exception.ToString());
            }
        }
        /// <summary>
        ///     连接对象
        /// </summary>
        public SqlConnection GetCurrentConnection()
        {
            return _connection;
        }

        /// <summary>
        ///     连接对象
        /// </summary>
        public void ClearCurrentConnection()
        {
            _connection = null;
        }


        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            //Trace.WriteLine("Dispose", "SqlServerDataBase");
            Close();
            GC.ReRegisterForFinalize(this);
        }

        #endregion

        #region 数据库特殊操作

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>被影响的行数</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public int Execute(string sql)
        {
            //using (SqlServerDataBaseScope.CreateScope(this))
            {
                return ExecuteInner(sql);
            }
        }

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>被影响的行数</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public int Execute(string sql, params object[] args)
        {
            //using (SqlServerDataBaseScope.CreateScope(this))
            {
                return args.Length == 0
                    ? ExecuteInner(sql)
                    : ExecuteInner(sql, args.Select(p => p is SqlParameter ? (SqlParameter)p : new SqlParameter { Value = p }).ToArray());
            }
        }


        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>被影响的行数</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public int Execute(string sql, params SqlParameter[] args)
        {
            //using (SqlServerDataBaseScope.CreateScope(this))
            {
                return args.Length == 0
                    ? ExecuteInner(sql)
                    : ExecuteInner(sql, args);
            }
        }

        /// <summary>
        ///     执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public object ExecuteScalar(string sql, params object[] args)
        {
            //using (SqlServerDataBaseScope.CreateScope(this))
            {
                return args.Length == 0
                    ? ExecuteScalarInner(sql)
                    : ExecuteScalarInner(sql, args.Select(p => p is SqlParameter ? (SqlParameter)p : new SqlParameter { Value = p }).ToArray());
            }
        }

        /// <summary>
        ///     执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public object ExecuteScalar(string sql, params SqlParameter[] args)
        {
            //using (SqlServerDataBaseScope.CreateScope(this))
            {
                return args == null || args.Length == 0
                    ? ExecuteScalarInner(sql)
                    : ExecuteScalarInner(sql, args);
            }
        }

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public T ExecuteScalar<T>(string sql)
        {
            //using (SqlServerDataBaseScope.CreateScope(this))
            {
                return ExecuteScalarInner<T>(sql);
            }
        }

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public object ExecuteScalar(string sql)
        {
            //using (SqlServerDataBaseScope.CreateScope(this))
            {
                return ExecuteScalarInner(sql);
            }
        }

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public T ExecuteScalar<T>(string sql, params object[] args)
        {
            //using (SqlServerDataBaseScope.CreateScope(this))
            {
                return ExecuteScalarInner<T>(sql, args);
            }
        }


        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public T ExecuteScalar<T>(string sql, params SqlParameter[] args)
        {
            //using (SqlServerDataBaseScope.CreateScope(this))
            {
                return ExecuteScalarInner<T>(sql, args);
            }
        }


        /// <summary>
        ///     清除所有数据
        /// </summary>
        public void Clear(string table)
        {
            Execute(string.Format(@"TRUNCATE TABLE [{0}];", table));
        }

        /// <summary>
        ///     清除所有数据
        /// </summary>
        public void ClearAll()
        {
            var sql = new StringBuilder();
            foreach (var table in TableSql.Values)
            {
                Clear(table.TableName);
            }

            Execute(sql.ToString());
        }

        #endregion

        #region 内部方法

        /// <summary>
        ///     对连接执行 Transact-SQL 语句并返回受影响的行数。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>被影响的行数</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        protected int ExecuteInner(string sql, params SqlParameter[] args)
        {
            lock (this)
            {
                int result;
                using (var cmd = CreateCommand(sql, args))
                {
                    result = cmd.ExecuteNonQuery();
                }
                return result;
            }
        }

        /// <summary>
        ///     执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        protected object ExecuteScalarInner(string sql, params SqlParameter[] args)
        {
            lock (this)
            {
                object result;
                using (var cmd = CreateCommand(sql, args))
                {
                    result = cmd.ExecuteScalar();
                }
                return result == DBNull.Value ? null : result;
            }
        }

        /// <summary>
        ///     记录SQL日志
        /// </summary>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        public static void TraceSql(SqlCommand cmd)
        {
            if (!LogRecorder.LogDataSql)
                return;
            TraceSql(cmd.CommandText, cmd.Parameters.OfType<SqlParameter>());
        }

        /// <summary>
        ///     记录SQL日志
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        public static void TraceSql(string sql, IEnumerable<SqlParameter> args)
        {
            if (!LogRecorder.LogDataSql)
                return;
            if (string.IsNullOrWhiteSpace(sql))
                return;
            StringBuilder code = new StringBuilder();
            code.AppendLine($"/******************************{DateTime.Now}*********************************/");
            var sqlParameters = args as SqlParameter[] ?? args.ToArray();
            foreach (var par in sqlParameters.Where(p => p != null))
            {
                code.AppendLine($"declare @{par.ParameterName} {par.SqlDbType};");
            }
            foreach (var par in sqlParameters.Where(p => p != null))
            {
                code.AppendLine($"SET @{par.ParameterName} = '{par.Value}';");
            }
            code.AppendLine(sql);
            code.AppendLine("GO");
            LogRecorder.RecordDataLog(code.ToString());
        }

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        protected T ExecuteScalarInner<T>(string sql)
        {
            return (T)ExecuteScalarInner(sql);
        }

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        protected T ExecuteScalarInner<T>(string sql, params SqlParameter[] args)
        {
            return (T)ExecuteScalarInner(sql, args);
        }

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号(1-9999)的形式访问参数
        /// </remarks>
        protected T ExecuteScalarInner<T>(string sql, params object[] args)
        {
            var result = args.Length == 0
                ? ExecuteScalarInner(sql)
                : ExecuteScalarInner(sql, args.Select(p => p is SqlParameter ? (SqlParameter)p  : new SqlParameter { Value = p }).ToArray());
            return (T)result;
        }

        #endregion

        #region 生成命令对象

        /// <summary>
        ///     生成命令
        /// </summary>
        public SqlCommand CreateCommand(params SqlParameter[] args)
        {
            return CreateCommand(null, args);
        }

        //static int _count = 0;

        /// <summary>
        ///     生成命令
        /// </summary>
        public SqlCommand CreateCommand(string sql, IEnumerable<SqlParameter> args = null)
        {
            var cmd = Connection.CreateCommand();

            if (Transaction != null)
            {
                cmd.Transaction = Transaction;
            }
            if (sql != null)
            {
                cmd.CommandText = sql;
            }
            if (args != null)
            {
                var sqlParameters = args as SqlParameter[] ?? args.ToArray();
                if (sqlParameters.Any(p => p != null))
                {
                    cmd.Parameters.AddRange(
                        sqlParameters.Where(p => p != null)
                            .Select(
                                p =>
                                    new SqlParameter(p.ParameterName, p.SqlDbType, p.Size, p.Direction, p.Precision, p.Scale,
                                        p.SourceColumn, p.SourceVersion, p.SourceColumnNullMapping, p.Value,
                                        p.XmlSchemaCollectionDatabase, p.XmlSchemaCollectionOwningSchema,
                                        p.XmlSchemaCollectionName)).ToArray());
                }
            }
            TraceSql(cmd);
            return cmd;
        }

        #endregion

        #region 创建表的SQL字典

        /// <summary>
        ///     表的常用SQL
        /// </summary>
        protected Dictionary<string, TableSql> tableSql;

        /// <summary>
        ///     表的常用SQL
        /// </summary>
        /// <remarks>请设置为键大小写不敏感字典,因为Sql没有强制表名的大小写区别</remarks>
        public Dictionary<string, TableSql> TableSql
        {
            get { return tableSql ?? (tableSql = new Dictionary<string, TableSql>(StringComparer.OrdinalIgnoreCase)); }
        }

        #endregion

        #region 辅助

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


        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static SqlParameter CreateParameter(string parameterName, object value)
        {
            var s = value as string;
            if (s != null)
            {
                return CreateParameter(parameterName, s);
            }
            if (value == null)
            {
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

        #endregion

        #region 数据缓存

        /// <summary>
        ///     缓存数据
        /// </summary>
        private readonly Dictionary<int, Dictionary<long, EditDataObject>> _dataCache =
            new Dictionary<int, Dictionary<long, EditDataObject>>();

        /// <summary>
        ///     取缓存数据
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="table"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public TData GetData<TData>(int table, int id) where TData : EditDataObject
        {
            Dictionary<long, EditDataObject> tableDatas;
            if (!_dataCache.TryGetValue(table, out tableDatas))
            {
                return null;
            }
            EditDataObject data;
            if (!tableDatas.TryGetValue(id, out data))
            {
                return null;
            }
            return data as TData;
        }

        /// <summary>
        ///     如不存在于缓存中，则加入，返回自身，如存在，则返回缓存中的数据。
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="table"></param>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public TData TryAddToCache<TData>(int table, long id, TData data) where TData : EditDataObject
        {
            Dictionary<long, EditDataObject> tableDatas;
            if (!_dataCache.TryGetValue(table, out tableDatas))
            {
                _dataCache.Add(table, tableDatas = new Dictionary<long, EditDataObject>());
            }

            if (tableDatas.ContainsKey(id))
            {
                return tableDatas[id] as TData;
            }
            tableDatas.Add(id, data);
            return data;
        }

        #endregion

        #region 接口

        string IDataBase.ConnectionString => ConnectionString;


        int IDataBase.Execute(string sql, IEnumerable<DbParameter> args)
        {
            return Execute(sql, args.OfType<SqlParameter>());
        }

        int IDataBase.Execute(string sql, params DbParameter[] args)
        {
            return Execute(sql, args.OfType<SqlParameter>());
        }

        object IDataBase.ExecuteScalar(string sql, IEnumerable<DbParameter> args)
        {
            return ExecuteScalar(sql, args.OfType<SqlParameter>());
        }

        object IDataBase.ExecuteScalar(string sql, params DbParameter[] args)
        {
            return ExecuteScalar(sql, args.OfType<SqlParameter>().ToArray());
        }

        T IDataBase.ExecuteScalar<T>(string sql)
        {
            return ExecuteScalar<T>(sql);
        }

        object IDataBase.ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql);
        }

        T IDataBase.ExecuteScalar<T>(string sql, params DbParameter[] args)
        {
            return ExecuteScalar<T>(sql, args.OfType<SqlParameter>().ToArray());
        }

        DbCommand IDataBase.CreateCommand(params DbParameter[] args)
        {
            return CreateCommand(args.OfType<SqlParameter>().ToArray());
        }

        DbCommand IDataBase.CreateCommand(string sql, DbParameter arg)
        {
            return CreateCommand((SqlParameter)arg);
        }

        DbCommand IDataBase.CreateCommand(string sql, IEnumerable<DbParameter> args)
        {
            return CreateCommand(sql, args.OfType<SqlParameter>().ToArray());
        }



        #endregion
    }
}