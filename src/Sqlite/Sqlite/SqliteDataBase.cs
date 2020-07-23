﻿#region 引用

using Agebull.Common.Configuration;
using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Text;

#endregion

namespace Agebull.EntityModel.Sqlite
{
    /// <summary>
    ///     表示Sqlite数据库对象
    /// </summary>
    public partial class SqliteDataBase : SqliteDataBase_, IDataBase
    {
        #region 事务

        //protected SqliteDataBase()
        //{
        //    Trace.WriteLine(".ctor", "SqliteDataBase");
        //}

        /// <summary>
        /// 是否锁定连接对象(更新插入删除发生后自动启用)
        /// </summary>
        public bool IsLockConnection { get; set; }

        /// <summary>
        ///     事务
        /// </summary>
        public SqliteTransaction Transaction { get; internal set; }

        /// <inheritdoc />
        /// <summary>
        ///     事务对象
        /// </summary>
        DbTransaction IDataBase.Transaction => Transaction;


        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        public bool BeginTransaction()
        {
            if (Transaction != null)
                return false;
            Transaction = Connection.BeginTransaction();
            return true;
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        void IDataBase.Rollback()
        {
            Transaction?.Rollback();
            Transaction?.Dispose();
            Transaction = null;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        void IDataBase.Commit()
        {
            Transaction?.Commit();
            Transaction?.Dispose();
            Transaction = null;
        }
        #endregion

        #region 连接

        /// <summary>
        /// 连接字符串配置节点名称,用于取出
        /// </summary>
        public string ConnectionStringName { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => DataBaseType.Sqlite;

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DataBaseName { get; private set; }

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
                if (_connectionString != null)
                {
                    return _connectionString;
                }

                var str = LoadConnectionStringSetting();
                var b = new SqliteConnectionStringBuilder(str);
                //if (b.ConnectionTimeout <= 0 || b.ConnectionTimeout > 10)
                //    b.ConnectionTimeout = 10;

                //if (b.DefaultCommandTimeout <= 0 || b.DefaultCommandTimeout > 10)
                //    b.DefaultCommandTimeout = 10;

                DataBaseName = b.DataSource;
                return _connectionString = b.ConnectionString;
            }
        }

        /// <summary>
        /// 读取连接字符串
        /// </summary>
        /// <returns></returns>
        protected virtual string LoadConnectionStringSetting()
        {
            return ConfigurationHelper.ConnectionStrings[ConnectionStringName ?? "Sqlite"];
        }

        /// <summary>
        ///     连接对象
        /// </summary>
        private SqliteConnection _connection;

        /// <summary>
        ///     连接对象
        /// </summary>
        public SqliteConnection Connection => _connection ??= InitConnection();


        /// <summary>
        ///     连接对象
        /// </summary>
        public static readonly List<SqliteConnection> Connections = new List<SqliteConnection>();

        /// <summary>
        /// 初始化连接对象
        /// </summary>
        /// <returns></returns>
        private SqliteConnection InitConnection()
        {
            var connection = new SqliteConnection(ConnectionString);
            DependencyScope.DisposeFunc.Add(() => Close(connection));
            int cnt;
            lock (Connections)
            {
                Connections.Add(connection);
                cnt = Connections.Count;
            }
            DependencyScope.Logger.Debug("打开连接数：{0}", cnt);
            //Trace.WriteLine(_count++, "Open");
            //Trace.WriteLine("Opened _connection", "SqliteDataBase");
            connection.Open();
            return connection;
        }
        /// <summary>
        ///     打开连接
        /// </summary>
        /// <returns>是否打开,是则为此时打开,否则为之前已打开</returns>
        public bool Open()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                return false;
            }
            if (_connection == null)
            {
                _connection = InitConnection();
                return true;
                //Trace.WriteLine("Create _connection", "SqliteDataBase");
            }
            if (string.IsNullOrEmpty(_connection.ConnectionString))
            {
                //Trace.WriteLine("Set ConnectionString", "SqliteDataBase");
                _connection.ConnectionString = ConnectionString;
            }
            //Trace.WriteLine(_count++, "Open");
            //Trace.WriteLine("Opened _connection", "SqliteDataBase");
            _connection.Open();
            return true;
        }

        /// <summary>
        ///     关闭连接
        /// </summary>
        public void Close()
        {
            Close(_connection);
            _connection = null;
        }


        /// <summary>
        ///     关闭连接
        /// </summary>
        private void Close(SqliteConnection connection)
        {
            //int cnt;
            lock (Connections)
            {
                if (!Connections.Remove(connection))
                    return;
                //cnt = Connections.Count;
            }
            if (connection == null)
            {
                return;
            }
            if (connection.State == ConnectionState.Open)
            {
                try
                {
                    connection.Close();
                }
                catch (Exception exception)
                {
                    DependencyScope.Logger.Exception(exception);
                }
            }

            try
            {
                connection.Dispose();
            }
            catch (Exception exception)
            {
                DependencyScope.Logger.Exception(exception);
            }
        }
        /// <summary>
        ///     连接对象
        /// </summary>
        public SqliteConnection GetCurrentConnection()
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
        protected virtual void DoDispose()
        {
        }

        private bool _isDisposed;

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;
            DoDispose();
            Close();
            GC.ReRegisterForFinalize(this);
        }

        /// <summary>
        /// 析构
        /// </summary>
        ~SqliteDataBase()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;
            DoDispose();
            Close();
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
            //using (DataBaseScope.CreateScope(this))
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
            //using (DataBaseScope.CreateScope(this))
            {
                return args.Length == 0
                    ? ExecuteInner(sql)
                    : ExecuteInner(sql, args.Select(p => p is DbParameter parameter ? parameter : new SqliteParameter { Value = p }).ToArray());
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
        public int Execute(string sql, params DbParameter[] args)
        {
            //using (DataBaseScope.CreateScope(this))
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
            //using (DataBaseScope.CreateScope(this))
            {
                return args.Length == 0
                    ? ExecuteScalarInner(sql)
                    : ExecuteScalarInner(sql, args.Select(p => p is DbParameter parameter ? parameter : new SqliteParameter { Value = p }).ToArray());
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
        public object ExecuteScalar(string sql, params DbParameter[] args)
        {
            //using (DataBaseScope.CreateScope(this))
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
            //using (DataBaseScope.CreateScope(this))
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
            //using (DataBaseScope.CreateScope(this))
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
            //using (DataBaseScope.CreateScope(this))
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
        public T ExecuteScalar<T>(string sql, params DbParameter[] args)
        {
            //using (DataBaseScope.CreateScope(this))
            {
                return ExecuteScalarInner<T>(sql, args);
            }
        }


        /// <summary>
        ///     清除所有数据
        /// </summary>
        public void Clear(string table)
        {
            Execute($@"TRUNCATE TABLE [{table}];");
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
        protected int ExecuteInner(string sql, params DbParameter[] args)
        {
            using var cmd = CreateCommand(sql, args);
            return cmd.ExecuteNonQuery();
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
        protected object ExecuteScalarInner(string sql, params DbParameter[] args)
        {
            object result;
            using (var cmd = CreateCommand(sql, args))
            {
                result = cmd.ExecuteScalar();
            }
            return result == DBNull.Value ? null : result;
        }

        /// <summary>
        ///     记录SQL日志
        /// </summary>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        public static void TraceSql(SqliteCommand cmd)
        {
            TraceSql(cmd.CommandText, cmd.Parameters.OfType<DbParameter>());
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
        public static void TraceSql(string sql, IEnumerable<DbParameter> args)
        {
            if (!LoggerExtend.LogDataSql || string.IsNullOrWhiteSpace(sql))
                return;
            StringBuilder code = new StringBuilder();
            code.AppendLine($"/******************************{DateTime.Now}*********************************/");
            var parameters = args as SqliteParameter[] ?? args.Cast<SqliteParameter>().ToArray();
            foreach (var par in parameters)
            {
                code.AppendLine($"declare @{par.ParameterName} {par.DbType};");
            }
            foreach (var par in parameters.Where(p => p.Value != null && !p.IsNullable))
            {
                code.AppendLine($"SET @{par.ParameterName} = '{par.Value}';");
            }
            foreach (var par in parameters.Where(p => p.Value == null || p.IsNullable))
            {
                code.AppendLine($"SET @{par.ParameterName} = NULL;");
            }
            code.AppendLine(sql);
            DependencyScope.Logger.RecordDataLog(code.ToString());
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
        protected T ExecuteScalarInner<T>(string sql, params DbParameter[] args)
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
                : ExecuteScalarInner(sql, args.Select(p => p is DbParameter parameter ? parameter : new SqliteParameter { Value = p }).ToArray());
            return (T)result;
        }

        #endregion

        #region 生成命令对象

        /// <summary>
        ///     生成命令
        /// </summary>
        public SqliteCommand CreateCommand(params DbParameter[] args)
        {
            return CreateCommand(null, args);
        }

        //static int _count = 0;

        /// <summary>
        ///     生成命令
        /// </summary>
        public SqliteCommand CreateCommand(string sql, IEnumerable<DbParameter> args = null)
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
                var sqlParameters = args as SqliteParameter[] ?? args.Cast<SqliteParameter>().ToArray();
                if (sqlParameters.Any(p => p != null))
                {
                    foreach (SqliteParameter parameter in sqlParameters)
                    {
                        cmd.Parameters.Add(new SqliteParameter(parameter.ParameterName, parameter.SqliteType, parameter.Size, parameter.SourceColumn)
                        {
                            Value = parameter.Value
                        });
                    }
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
        public Dictionary<string, TableSql> TableSql => tableSql ??= new Dictionary<string, TableSql>(StringComparer.OrdinalIgnoreCase);

        #endregion

        #region 接口

        int IDataBase.Execute(string sql, IEnumerable<DbParameter> args)
        {
            return Execute(sql, args);
        }

        int IDataBase.Execute(string sql, params DbParameter[] args)
        {
            return Execute(sql, args);
        }

        object IDataBase.ExecuteScalar(string sql, IEnumerable<DbParameter> args)
        {
            return ExecuteScalar(sql, args);
        }

        object IDataBase.ExecuteScalar(string sql, params DbParameter[] args)
        {
            return ExecuteScalar(sql, args.ToArray());
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
            return ExecuteScalar<T>(sql, args.ToArray());
        }

        #endregion
    }
}