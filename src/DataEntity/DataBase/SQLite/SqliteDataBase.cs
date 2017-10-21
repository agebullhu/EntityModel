using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Agebull.Common.Logging;
using Gboxt.Common.DataModel;

namespace Gboxt.Common.SimpleDataAccess.SQLite
{
    public class SqliteDataBase
    {
        protected static readonly object LockData = new object();
        /// <summary>
        /// 缺省强类型数据库
        /// </summary>
        [ThreadStatic]
        private static SqliteDataBase _default;
        /// <summary>
        /// 连接对象
        /// </summary>
        public static SqliteDataBase DefaultDataBase
        {
            get
            {
                if (_default != null)
                {
                    return _default;
                }
                //lock (LockData)
                {
                    return _default = CreateDefaultFunc();
                }
            }
        }

        /// <summary>
        /// 生成缺省数据库访问对象的方法
        /// </summary>
        public static Func<SqliteDataBase> CreateDefaultFunc
        {
            get;
            set;
        }


        private SQLiteConnection _connection;
        /// <summary>
        /// 连接对象
        /// </summary>
        public SQLiteConnection Connection
        {
            get { return this._connection ?? (this._connection = new SQLiteConnection(this.ConnectionString)); }
            internal set { this._connection = value; }
        }

        /// <summary>
        /// 事务
        /// </summary>
        public SQLiteTransaction Transaction
        {
            get;
            internal set;
        }
        /// <summary>
        /// 打开数据库,如果数据库不存在,创建它
        /// </summary>
        /// <param name="file">数据库文件名</param>
        /// <returns>是否成功</returns>
        public static SqliteDataBase OpenDataBase(string file)
        {
            try
            {
                if (!File.Exists(file))
                    SQLiteConnection.CreateFile(file);
                var builder = new SQLiteConnectionStringBuilder { DataSource = file };
                return new SqliteDataBase
                {
                    ConnectionString = builder.ConnectionString
                };
            }
            catch (Exception ex)
            {
                LogRecorder.Exception(ex, "OpenDataBase");
                return null;
            }
        }
        #region 连接字符串
        /// <summary>
        /// 数据库路径
        /// </summary>
        public string FileName
        {
            get { return this._fileName; }
            set
            {
                if (this._fileName == value)
                    return;
                this._fileName = value;
                var builder = new SQLiteConnectionStringBuilder { DataSource = FileName };
                ConnectionString = builder.ConnectionString;
            }
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; private set; }

        #endregion
        #region 引用范围
        /// <summary>
        /// 引用数量
        /// </summary>
        internal protected int QuoteCount { get; set; }
        #endregion

        #region 数据库特殊操作

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>被影响的行数</returns>
        /// <remarks> 
        /// 注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public int Execute(string sql)
        {
            using (SqliteDataBaseScope.CreateScope(this))
            {
                return this.ExecuteInner(sql);
            }
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>被影响的行数</returns>
        /// <remarks> 
        /// 注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public int Execute(string sql, params object[] args)
        {
            using (SqliteDataBaseScope.CreateScope(this))
            {
                return args.Length == 0
                    ? this.ExecuteInner(sql)
                    : this.ExecuteInner(sql, args.Select(p => p is SQLiteParameter ? p as SQLiteParameter : new SQLiteParameter { Value = p }).ToArray());
            }
        }


        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>被影响的行数</returns>
        /// <remarks> 
        /// 注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public int Execute(string sql, params SQLiteParameter[] args)
        {

            using (SqliteDataBaseScope.CreateScope(this))
            {
                return args.Length == 0
                    ? this.ExecuteInner(sql)
                    : this.ExecuteInner(sql, args);
            }
        }
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks> 
        /// 注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public object ExecuteScalar(string sql, params object[] args)
        {
            using (SqliteDataBaseScope.CreateScope(this))
            {
                return args.Length == 0
                    ? this.ExecuteScalarInner(sql)
                    : this.ExecuteScalarInner(sql, args.Select(p => p is SQLiteParameter ? p as SQLiteParameter : new SQLiteParameter { Value = p }).ToArray());
            }
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks> 
        /// 注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public object ExecuteScalar(string sql, params SQLiteParameter[] args)
        {
            using (SqliteDataBaseScope.CreateScope(this))
            {
                return args.Length == 0
                    ? this.ExecuteScalarInner(sql)
                    : this.ExecuteScalarInner(sql, args);
            }
        }
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks> 
        /// 注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public T ExecuteScalar<T>(string sql)
        {
            using (SqliteDataBaseScope.CreateScope(this))
            {
                return ExecuteScalarInner<T>(sql);
            }
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks> 
        /// 注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public T ExecuteScalar<T>(string sql, params object[] args)
        {
            using (SqliteDataBaseScope.CreateScope(this))
            {
                return ExecuteScalarInner<T>(sql, args);
            }
        }
        /// <summary>
        /// 设置数据库只读,以提高性能
        /// </summary>
        public void SetReadOnly(bool readOnly)
        {
            Execute(readOnly ? "PRAGMA synchronous = OFF;" : "PRAGMA synchronous = NORMAL;");
        }
        /// <summary>
        /// 设置数据库只读,以提高性能
        /// </summary>
        public void Attach(string file)
        {
            Execute("ATTACH '" + file + "';");
        }

        /// <summary>
        /// 打开连接
        /// </summary>
        /// <returns>是否成功</returns>
        public void Open()
        {
            if (Connection == null || Connection.State != ConnectionState.Open)
            {
                Trace.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Trace.WriteLine("Open Database");
                Connection = new SQLiteConnection(this.ConnectionString);
                Connection.Open();
                QuoteCount = short.MaxValue;
            }
            QuoteCount += 1;
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            if (Connection == null)
                return;
            if (Connection.State != ConnectionState.Open)
                Connection.Close();
            Connection = null;

            Trace.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Trace.WriteLine("Close Database");
        }
        /// <summary>
        /// 检查数据库,如果数据库不存在,创建它
        /// </summary>
        /// <returns>是否成功</returns>
        public void CheckDataBase()
        {
            lock (LockData)
            {
                if (!File.Exists(FileName))
                {
                    SQLiteConnection.CreateFile(FileName);
                }
                foreach (var table in this.TableSql.Keys)
                {
                    this.CheckTable(table);
                }
                var builder = new SQLiteConnectionStringBuilder { DataSource = FileName };
                ConnectionString = builder.ConnectionString;
            }
        }
        /// <summary>
        /// 如果表不存在,就创建它
        /// </summary>
        /// <param name="table">表名</param>
        /// <remarks>TableSql中必须存在对应的表的创建SQL,否则会因为在字典中找不到对象或执行SQL为空而发生异常</remarks>
        public void CheckTable(string table)
        {
            using (SqliteDataBaseScope.CreateScope(this))
            {
                if (IsExistTable(table))
                    return;
                CreateTableInner(table);
            }
        }


        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="table">表名</param>
        /// <returns>True表示表已存在</returns>
        public bool IsExist(string table)
        {
            using (SqliteDataBaseScope.CreateScope(this))
            {
                return IsExistTable(table);
            }
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear(string table)
        {
            Execute(string.Format(@"DELETE FROM [{0}];
UPDATE sqlite_sequence SET seq = 0 WHERE name = '{0}';", table));
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void ClearAll()
        {
            StringBuilder sql = new StringBuilder();
            foreach (var table in this.TableSql.Values)
            {
                sql.AppendFormat(@"DELETE FROM [{0}];
UPDATE sqlite_sequence SET seq = 0 WHERE name = '{0}';", table.TableName);
            }

            Execute(sql.ToString());
        }
        #endregion
        #region 内部方法

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>被影响的行数</returns>
        /// <remarks> 
        /// 注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        protected int ExecuteInner(string sql, params SQLiteParameter[] args)
        {
           // LogRecorder.RecordDataLog(sql);
            var cmd = Connection.CreateCommand();
            if (args != null && args.Length > 0)
            {
                cmd.Parameters.AddRange(args);
            }
            cmd.CommandText = sql;
            lock (this)
            {
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks> 
        /// 注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        protected object ExecuteScalarInner(string sql, params SQLiteParameter[] args)
        {
           // LogRecorder.RecordDataLog(sql);
            var cmd = Connection.CreateCommand();
            if (args != null && args.Length > 0)
            {
                cmd.Parameters.AddRange(args);
            }
            cmd.CommandText = sql;
            lock (this)
            {
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks> 
        /// 注意,如果有参数时,都是匿名参数,请使用?序号(1-9999)的形式访问参数
        /// </remarks>
        protected T ExecuteScalarInner<T>(string sql, params object[] args)
        {
           // LogRecorder.RecordDataLog(sql);
            object result = args.Length == 0
                ? this.ExecuteScalarInner(sql)
                : this.ExecuteScalarInner(sql, args.Select(p => p is SQLiteParameter ? p as SQLiteParameter : new SQLiteParameter { Value = p }).ToArray());
            return (T)result;
        }

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="table">表名</param>
        /// <returns>True表示表已存在</returns>
        public bool IsExistTable(string table)
        {
            return ExecuteScalar<long>(@"SELECT COUNT([name]) FROM [sqlite_master] WHERE [type]='table' AND [name] = ?", table) == 1;
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="table">表名</param>
        protected void CreateTableInner(string table)
        {
            Execute(TableSql[table].CreateSql);
        }
        #endregion
        #region 创建表的SQL字典

        protected Dictionary<string, TableSql> tableSql;
        private string _fileName;
        /// <summary>
        /// 表的常用SQL
        /// </summary>
        /// <remarks>请设置为键大小写不敏感字典,因为SQLite没有强制表名的大小写区别</remarks>
        public Dictionary<string, TableSql> TableSql
        {
            get { return this.tableSql ?? (this.tableSql = new Dictionary<string, TableSql>(StringComparer.OrdinalIgnoreCase)); }
        }
        #endregion
        #region 辅助



        /// <summary>
        /// 生成SQLite参数
        /// </summary>
        /// <param name="csharpType">C#的类型</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static SQLiteParameter CreateParameter(string csharpType, string parameterName, object value)
        {
            return new SQLiteParameter(parameterName, ToDbType(csharpType))
            {
                Value = value
            };

        }


        /// <summary>
        /// 生成SQLite参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static SQLiteParameter CreateParameter(string parameterName, object value)
        {
            return new SQLiteParameter(parameterName, ToDbType(value.GetType().Name))
            {
                Value = value
            };

        }

        /// <summary>
        /// 生成SQLite参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static SQLiteParameter CreateParameter<T>(string parameterName, T value)
        {
            return new SQLiteParameter(parameterName, ToDbType(typeof(T).Name))
            {
                Value = value
            };
        }


        /// <summary>
        ///   从C#的类型转为DBType
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
                //case "Guid":
                //    return DbType.Guid;
                //case "DateTime":
                //    return DbType.DateTime2;
                case "int":
                case "Int32":
                case "IntPtr":
                    return DbType.Int32;
                case "uint":
                case "UInt32":
                case "UIntPtr":
                    return DbType.Int32;
                case "Binary":
                case "byte[]":
                case "Byte[]":
                    return DbType.Binary;
                default:
                    return DbType.String;
            }
        }
        #endregion
    }
}