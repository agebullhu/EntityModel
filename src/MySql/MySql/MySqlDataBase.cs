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
using System.Linq;
using System.Text;
using Agebull.Common.Logging;
using System.Data.Common;
using System.Threading.Tasks;
using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;
using Agebull.Common.Base;

#endregion

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    ///     表示MySql数据库对象
    /// </summary>
    public abstract partial class MySqlDataBase : MySqlDataBase_, IDataBase
    {
        #region 构造

        /// <summary>
        /// 构造
        /// </summary>
        protected MySqlDataBase()
        {
            MySqlConnectionsManager.InternalInitialize();
            DependencyScope.DisposeFunc.Add(Dispose);
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => DataBaseType.MySql;


        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DataBaseName { get; private set; }

        #endregion

        #region 事务

        /// <summary>
        ///     事务对象
        /// </summary>
        public MySqlTransaction Transaction { get; internal set; }

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
            Open();
            Transaction = _connection.BeginTransaction();

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
        /// 是否锁定连接对象(更新插入删除发生后自动启用)
        /// </summary>
        public bool IsLockConnection { get; set; }

        internal MySqlConnection _connection;

        /// <summary>
        ///     打开连接
        /// </summary>
        /// <returns>是否打开,是则为此时打开,否则为之前已打开</returns>
        public bool Open()
        {
            if (_connection != null/* && _connection.State == ConnectionState.Open*/)
                return false;
            _connection = MySqlConnectionsManager.InitConnection(ConnectionStringName);
            IsLockConnection = true;
            return true;
        }
        #endregion

        #region 析构
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

            if (_connection != null)
            {
                _connection = null;
                IsLockConnection = false;
                if (Transaction != null)
                {
                    Transaction.Rollback();
                    Transaction = null;
                }
                MySqlConnectionsManager.Close(_connection, ConnectionStringName);
            }
        }

        /// <summary>
        /// 析构
        /// </summary>
        ~MySqlDataBase()
        {
            Dispose();
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        protected virtual void DoDispose()
        {
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
            return ExecuteInner(sql);
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
        public int Execute(string sql, IEnumerable<DbParameter> args)
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
        public int Execute(string sql, params DbParameter[] args)
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
        public object ExecuteScalar(string sql, IEnumerable<DbParameter> args)
        {
            return ExecuteScalarInner(sql, args.ToArray());
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
            return args == null || args.Length == 0
                   ? ExecuteScalarInner(sql)
                   : ExecuteScalarInner(sql, args);
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
            return ExecuteScalarInner<T>(sql);
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
            return ExecuteScalarInner(sql);
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
            return ExecuteScalarInner<T>(sql, args);
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
                sql.AppendLine($@"TRUNCATE TABLE `{table}`;");
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
            using var scope = new ConnectionScope(this);
            using var cmd = CreateCommand(scope, sql, args);
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
            using var scope = new ConnectionScope(this);
            object result;
            using (var cmd = CreateCommand(scope, sql, args))
            {
                result = cmd.ExecuteScalar();
            }
            return result == DBNull.Value ? null : result;
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
            var result = args.Length == 0
                ? ExecuteScalarInner(sql)
                : ExecuteScalarInner(sql, args);
            return (T)result;
        }

        #endregion

        #region 生成命令对象

        /// <summary>
        ///     生成命令
        /// </summary>
        public MySqlCommand CreateCommand(ConnectionScope scope, params DbParameter[] args)
        {
            return CreateCommand(scope, null, args);
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        public MySqlCommand CreateCommand(ConnectionScope scope, string sql, DbParameter arg)
        {
            return CreateCommand(scope, sql, new[] { arg });
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        public MySqlCommand CreateCommand(ConnectionScope scope)
        {
            var cmd = scope.Connection.CreateCommand();

            if (scope.Transaction != null)
            {
                cmd.Transaction = scope.Transaction;
            }
            return cmd;
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        public MySqlCommand CreateCommand(ConnectionScope scope, string sql, IEnumerable<DbParameter> args = null)
        {
            var cmd = scope.Connection.CreateCommand();

            if (scope.Transaction != null)
            {
                cmd.Transaction = scope.Transaction;
            }
            if (sql != null)
            {
                cmd.CommandText = sql;
            }
            if (args != null)
            {
                var parameters = args.OfType<MySqlParameter>().ToArray();
                if (parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
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
        public Dictionary<string, TableSql> TableSql => _tableSql ??= new Dictionary<string, TableSql>(StringComparer.OrdinalIgnoreCase);

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

        #region SQL日志

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
            TraceSql(cmd.CommandText, cmd.Parameters.Cast<MySqlParameter>());
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
            foreach (var par in parameters)
            {
                code.AppendLine($"declare ?{par.ParameterName} {par.MySqlDbType};");
            }
            foreach (var par in parameters)
            {
                if (par.Value == null || par.IsNullable)
                    code.AppendLine($"SET ?{par.ParameterName} = NULL;");
                else
                    code.AppendLine($"SET ?{par.ParameterName} = '{par.Value}';");
            }
            code.AppendLine(sql);
            LogRecorder.RecordDataLog(code.ToString());
        }


        #endregion
    }

}