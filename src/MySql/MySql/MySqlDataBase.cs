// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Agebull.Common.Logging;
using System.Data.Common;
using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;

#endregion

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    ///     表示SQL SERVER数据库对象
    /// </summary>
    public abstract class MySqlDataBase : SimpleConfig, IDataBase
    {
        #region 事务

        //protected MySqlDataBase()
        //{
        //    Trace.WriteLine(".ctor", "MySqlDataBase");
        //}

        /// <summary>
        ///     事务
        /// </summary>
        public MySqlTransaction Transaction { get; internal set; }

        #endregion

        #region 引用范围

        /// <summary>
        /// 生成数据库使用范围
        /// </summary>
        /// <returns></returns>
        IDisposable IDataBase.CreateDataBaseScope() => MySqlDataBaseScope.CreateScope(this);


        /// <summary>
        /// 生成事务范围
        /// </summary>
        /// <returns></returns>
        public ITransactionScope CreateTransactionScope() => TransactionScope.CreateScope(this);

        /// <summary>
        ///     引用数量
        /// </summary>
        protected internal int QuoteCount { get; set; }

        #endregion

        #region 线程实例

        /// <summary>
        /// 构造
        /// </summary>
        protected MySqlDataBase()
        {
            if (_default == null)
                _default = this;
        }

        /// <summary>
        ///     锁对象
        /// </summary>
        protected static readonly object LockData = new object();

        /// <summary>
        ///     缺省强类型数据库
        /// </summary>
        [ThreadStatic]
        private static MySqlDataBase _default;

        /// <summary>
        ///     连接对象
        /// </summary>
        public static MySqlDataBase DataBase
        {
            get => _default ?? (_default = IocHelper.Create<MySqlDataBase>());
            set => _default = value;
        }

        #endregion

        #region 数据库连接对象

        private MySqlConnection _connection;
        /// <summary>
        ///     连接对象
        /// </summary>
        public MySqlConnection Connection => _connection ?? (_connection = InitConnection());


        /// <summary>
        ///     连接对象
        /// </summary>
        public static readonly List<MySqlConnection> Connections = new List<MySqlConnection>();

        /// <summary>
        ///     连接对象
        /// </summary>
        public MySqlConnection GetCurrentConnection()
        {
            return Connection;
        }

        /// <summary>
        /// 初始化连接对象
        /// </summary>
        /// <returns></returns>
        private MySqlConnection InitConnection()
        {
            lock (LockData)
            {
                var connection = new MySqlConnection(ConnectionString);
                Connections.Add(connection);
                //Trace.WriteLine(_count++, "Open");
                //Trace.WriteLine("Opened _connection", "MySqlDataBase");
                connection.Open();
                return connection;
            }
        }
        
        #endregion

        #region 连接

        /// <summary>
        ///     连接字符串
        ///     Database=test;Data Source=localhost;User Id=root;Password=123456;pooling=false;CharSet=utf8;port=3306
        /// </summary>
        private string _connectionString;

        /// <summary>
        ///     连接字符串
        /// </summary>
        public string ConnectionString => _connectionString ?? (_connectionString = LoadConnectionStringSetting());

        /// <summary>
        /// 读取连接字符串
        /// </summary>
        /// <returns></returns>
        protected abstract string LoadConnectionStringSetting();
        
        /// <summary>
        ///     打开连接
        /// </summary>
        /// <returns>是否打开,是则为此时打开,否则为之前已打开</returns>
        public bool Open()
        {
            lock (LockData)
            {
                //if (_isClosed)
                //{
                //    //throw new Exception("已关闭的数据库对象不能再次使用");
                //}
                bool result = false;
                if (_connection == null)
                {
                    result = true;
                    _connection = InitConnection();
                    return true;
                    //Trace.WriteLine("Create _connection", "MySqlDataBase");
                }
                if (string.IsNullOrEmpty(_connection.ConnectionString))
                {
                    result = true;
                    //Trace.WriteLine("Set ConnectionString", "MySqlDataBase");
                    _connection.ConnectionString = ConnectionString;
                }
                if (_connection.State == ConnectionState.Open)
                {
                    return result;
                }
                //Trace.WriteLine(_count++, "Open");
                //Trace.WriteLine("Opened _connection", "MySqlDataBase");
                _connection.Open();
            }
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
            lock (LockData)
            {
                try
                {
                    if (_connection.State == ConnectionState.Open)
                    {
                        _connection.Close();
                        //Trace.WriteLine("Close Connection", "MySqlDataBase");
                    }
                    LogRecorder.MonitorTrace($"未关闭总数{Connections.Count}");
                    _connection.Dispose();

                }
                catch (Exception exception)
                {
                    _connection?.Dispose();
                    Debug.WriteLine("Close Error", "MySqlDataBase");
                    LogRecorder.Exception(exception);
                }
                finally
                {
                    if (_default == this)
                        _default = null;
                    Connections.Remove(_connection);
                    _connection = null;
                }
            }
        }


        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        protected virtual void DoDispose()
        {
        }


        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            DoDispose();
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
            //using (MySqlDataBaseScope.CreateScope(this))
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
        public int Execute(string sql, IEnumerable<MySqlParameter> args)
        {
            return ExecuteInner(sql, args.ToArray());
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
        public int Execute(string sql, params MySqlParameter[] args)
        {
            return args.Length == 0
                ? ExecuteInner(sql)
                : ExecuteInner(sql, args);
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
        public object ExecuteScalar(string sql, IEnumerable<MySqlParameter> args)
        {
            //using (MySqlDataBaseScope.CreateScope(this))
            {
                return ExecuteScalarInner(sql, args.ToArray());
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
        public object ExecuteScalar(string sql, params MySqlParameter[] args)
        {
            //using (MySqlDataBaseScope.CreateScope(this))
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
            //using (MySqlDataBaseScope.CreateScope(this))
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
            //using (MySqlDataBaseScope.CreateScope(this))
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
        public T ExecuteScalar<T>(string sql, params MySqlParameter[] args)
        {
            //using (MySqlDataBaseScope.CreateScope(this))
            {
                return ExecuteScalarInner<T>(sql, args);
            }
        }


        /// <summary>
        ///     清除所有数据
        /// </summary>
        public void Clear(string table)
        {
            Execute($@"TRUNCATE TABLE `{table}`;");
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
        protected int ExecuteInner(string sql, params MySqlParameter[] args)
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
        protected object ExecuteScalarInner(string sql, params MySqlParameter[] args)
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
        protected T ExecuteScalarInner<T>(string sql, params MySqlParameter[] args)
        {
            var result = args.Length == 0
                ? ExecuteScalarInner(sql)
                : ExecuteScalarInner(sql, args);
            return (T)result;
        }
        /// <summary>
        ///     记录SQL日志
        /// </summary>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        public static void TraceSql(MySqlCommand cmd)
        {
            if (!LogRecorder.LogDataSql)
                return;
            TraceSql(cmd.CommandText, cmd.Parameters.OfType<MySqlParameter>());
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
        public static void TraceSql(string sql, IEnumerable<MySqlParameter> args)
        {
            if (!LogRecorder.LogDataSql)
                return;
            StringBuilder code = new StringBuilder();
            code.AppendLine("/***************************************************************/");
            var parameters = args as MySqlParameter[] ?? args.ToArray();
            foreach (var par in parameters.Where(p => p != null))
            {
                code.AppendLine($"declare ?{par.ParameterName} {par.MySqlDbType};");
            }
            foreach (var par in parameters.Where(p => p != null))
            {
                code.AppendLine($"SET ?{par.ParameterName} = '{par.Value}';");
            }
            code.AppendLine(sql);
            LogRecorder.RecordDataLog(code.ToString());
        }


        #endregion

        #region 生成命令对象

        /// <summary>
        ///     生成命令
        /// </summary>
        public MySqlCommand CreateCommand(params MySqlParameter[] args)
        {
            return CreateCommand(null, args);
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        public MySqlCommand CreateCommand(string sql, MySqlParameter arg)
        {
            return CreateCommand(sql, new[] { arg });
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        public MySqlCommand CreateCommand(string sql, IEnumerable<MySqlParameter> args = null)
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
                var parameters = args as MySqlParameter[] ?? args.ToArray();
                if (parameters.Any(p => p != null))
                {
                    cmd.Parameters.AddRange(
                        parameters.Where(p => p != null)
                            .Select(
                                p =>
                                    new MySqlParameter(p.ParameterName, p.MySqlDbType, p.Size, p.Direction, p.IsNullable, p.Precision, p.Scale,
                                        p.SourceColumn, p.SourceVersion, p.Value)).ToArray());
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
        protected Dictionary<string, TableSql> _tableSql;

        /// <summary>
        ///     表的常用SQL
        /// </summary>
        /// <remarks>请设置为键大小写不敏感字典,因为Sql没有强制表名的大小写区别</remarks>
        public Dictionary<string, TableSql> TableSql => _tableSql ?? (_tableSql = new Dictionary<string, TableSql>(StringComparer.OrdinalIgnoreCase));

        #endregion

        #region 生成Sql参数

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="csharpType">C#的类型</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static MySqlParameter CreateParameter(string csharpType, string parameterName, object value)
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


        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">类型</param>
        /// <returns>参数</returns>
        public static MySqlParameter CreateParameter(string parameterName, object value, MySqlDbType type)
        {
            switch (value)
            {
                case null:
                    return new MySqlParameter(parameterName, MySqlDbType.VarChar)
                    {
                        Value = DBNull.Value
                    };
                case string s:
                    return CreateParameter(parameterName, s);
                case Enum _:
                    return new MySqlParameter(parameterName, MySqlDbType.Int32)
                    {
                        Value = Convert.ToInt32(value)
                    };
                case bool _:
                    return new MySqlParameter(parameterName, MySqlDbType.Byte)
                    {
                        Value = (bool)value ? (byte)1 : (byte)0
                    };
            }

            return new MySqlParameter(parameterName, type)
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
        public static MySqlParameter CreateParameter(string parameterName, object value)
        {
            return CreateParameter(parameterName, value, ToSqlDbType(value?.GetType().Name));
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

        #endregion

        #region 数据缓存

        /*// <summary>
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
        }*/


        #endregion


        #region 接口

        string IDataBase.ConnectionString => ConnectionString;


        int IDataBase.Execute(string sql, IEnumerable<DbParameter> args)
        {
            return Execute(sql, args.OfType<MySqlParameter>());
        }

        int IDataBase.Execute(string sql, params DbParameter[] args)
        {
            return Execute(sql, args.OfType<MySqlParameter>());
        }

        object IDataBase.ExecuteScalar(string sql, IEnumerable<DbParameter> args)
        {
            return ExecuteScalar(sql, args.OfType<MySqlParameter>());
        }

        object IDataBase.ExecuteScalar(string sql, params DbParameter[] args)
        {
            return ExecuteScalar(sql, args.OfType<MySqlParameter>().ToArray());
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
            return ExecuteScalar<T>(sql, args.OfType<MySqlParameter>().ToArray());
        }

        DbCommand IDataBase.CreateCommand(params DbParameter[] args)
        {
            return CreateCommand(args.OfType<MySqlParameter>().ToArray());
        }

        DbCommand IDataBase.CreateCommand(string sql, DbParameter arg)
        {
            return CreateCommand((MySqlParameter)arg);
        }

        DbCommand IDataBase.CreateCommand(string sql, IEnumerable<DbParameter> args)
        {
            return CreateCommand(sql, args.OfType<MySqlParameter>().ToArray());
        }



        #endregion
    }

}