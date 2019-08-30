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
using System.Data.Common;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     表示数据库对象
    /// </summary>
    public interface IDataBase : IConfig, IDisposable
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        DataBaseType DataBaseType
        {
            get;
        }

        #region 事务及连接范围

        /// <summary>
        /// 生成数据库使用范围
        /// </summary>
        /// <returns></returns>
        IDisposable CreateDataBaseScope();

        /// <summary>
        ///     事务对象
        /// </summary>
        DbTransaction Transaction { get; }
        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        bool BeginTransaction();

        /// <summary>
        /// 生成事务范围
        /// </summary>
        /// <returns></returns>
        ITransactionScope CreateTransactionScope();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();

        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();

        #endregion

        #region 连接
        /// <summary>
        ///     引用计数
        /// </summary>
        int QuoteCount { get; set; }

        /// <summary>
        ///     连接字符串
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        ///     打开连接
        /// </summary>
        /// <returns>是否打开,是则为此时打开,否则为之前已打开</returns>
        bool Open();

        /// <summary>
        ///     关闭连接
        /// </summary>
        void Close();
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
        int Execute(string sql);

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>被影响的行数</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        int Execute(string sql, IEnumerable<DbParameter> args);


        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>被影响的行数</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        int Execute(string sql, params DbParameter[] args);

        /// <summary>
        ///     执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        object ExecuteScalar(string sql, IEnumerable<DbParameter> args);

        /// <summary>
        ///     执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        object ExecuteScalar(string sql, params DbParameter[] args);

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        T ExecuteScalar<T>(string sql);

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        object ExecuteScalar(string sql);


        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        T ExecuteScalar<T>(string sql, params DbParameter[] args);

        #endregion

        #region 生成命令对象

        /// <summary>
        ///     生成命令
        /// </summary>
        DbCommand CreateCommand(params DbParameter[] args);

        /// <summary>
        ///     生成命令
        /// </summary>
        DbCommand CreateCommand(string sql, DbParameter arg);

        /// <summary>
        ///     生成命令
        /// </summary>
        DbCommand CreateCommand(string sql, IEnumerable<DbParameter> args = null);

        #endregion

    }
    /// <summary>
    /// 参数生成器
    /// </summary>
    public interface IParameterCreater
    {

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="csharpType">C#的类型</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        DbParameter CreateParameter(string csharpType, string parameterName, object value);


        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        DbParameter CreateParameter(string parameterName, object value);

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        DbParameter CreateParameter(string parameterName, string value);

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        DbParameter CreateParameter<T>(string parameterName, T value)
            where T : struct;
    }
}