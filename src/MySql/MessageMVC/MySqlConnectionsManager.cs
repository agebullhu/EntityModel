using Agebull.Common.Configuration;
using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Agebull.EntityModel.Common;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC;

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    /// 连接管理
    /// </summary>
    internal class MySqlConnectionsManager : IFlowMiddleware, IHealthCheck
    {
        #region IFlowMiddleware
        static ILogger logger;
        string IZeroDependency.Name => nameof(MySqlConnectionsManager);

        int IZeroMiddleware.Level => 0xFFFF;

        /// <summary>
        /// 是否在控制中
        /// </summary>
        static bool IsManagement;

        /// <summary>
        ///     初始化
        /// </summary>
        Task ILifeFlow.Destory()
        {
            foreach (var group in Connections)
            {
                foreach (var conn in group.Value.ActiveConnections.Values)
                    CloseConnection(conn.Connection, group.Key);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        ///     初始化
        /// </summary>
        Task ILifeFlow.Initialize()
        {

            IsManagement = true;
            InternalInitialize();
            ConfigurationHelper.RegistOnChange("ConnectionStrings", CheckOption, false);
            return Task.CompletedTask;
        }

        /// <summary>
        ///     配置检查(关闭已过时的配置连接)
        /// </summary>
        void CheckOption()
        {
            foreach (var group in Connections.ToArray())
            {
                var str = ConfigurationHelper.GetConnectionString(group.Key);
                if (str == group.Value.ConnectionString)
                    continue;
                Connections.TryRemove(group.Key, out var info);
                info ??= group.Value;
                foreach (var conn in info.ActiveConnections.Values.ToArray())
                {
                    Close(conn.Connection, group.Key);
                }
            }
        }

        /// <summary>
        ///     内部初始化
        /// </summary>
        internal static void InternalInitialize()
        {
            logger ??= DependencyHelper.LoggerFactory.CreateLogger<MySqlConnectionsManager>();
            logger.LogInformation("[MySqlConnectionsManager.InternalInitialize]");
        }
        #endregion

        #region 连接对象

        /// <summary>
        ///     连接对象
        /// </summary>
        internal static readonly ConcurrentDictionary<string, DataBaseInfo> Connections = new ConcurrentDictionary<string, DataBaseInfo>();

        /// <summary>
        /// 取得一个空闲连接对象
        /// </summary>
        /// <returns></returns>
        internal static MySqlConnection GetConnection(string name)
        {
            if (!IsManagement)
                return InitConnection(name);
            return GetManagement(name);
        }

        /// <summary>
        /// 关闭连接对象
        /// </summary>
        /// <returns></returns>
        internal static void Close(MySqlConnection connection, string name)
        {
            if (!IsManagement)
                CloseConnection(connection, name);
            else
                AddFree(connection, name);
        }
        #endregion


        #region 连接池

        /// <summary>
        ///     空闲连接对象
        /// </summary>
        internal static readonly ConcurrentDictionary<string, ConcurrentQueue<MySqlConnection>> FreeConnections = new ConcurrentDictionary<string, ConcurrentQueue<MySqlConnection>>();

        /// <summary>
        ///     使用中连接对象
        /// </summary>
        internal static readonly ConcurrentDictionary<string, ConcurrentQueue<MySqlConnection>> UsingConnections = new ConcurrentDictionary<string, ConcurrentQueue<MySqlConnection>>();

        /// <summary>
        /// 从连接池中获取
        /// </summary>
        /// <param name="name"></param>
        static MySqlConnection GetManagement(string name)
        {
            if (!FreeConnections.TryGetValue(name, out var useQueue))
            {
                FreeConnections.TryAdd(name, new ConcurrentQueue<MySqlConnection>());
                FreeConnections.TryGetValue(name, out useQueue);
            }
            while (useQueue.TryDequeue(out var con))
            {
                if (con.State != ConnectionState.Open)
                {
                    Close(con, name);
                    continue;
                }
                AddUsing(con, name);
                return con;
            }
            return InitConnection(name);
        }
        /// <summary>
        /// 加入使用池中
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="name"></param>
        static void AddUsing(MySqlConnection connection, string name)
        {
            if (!IsManagement)
                return;
            if (!UsingConnections.TryGetValue(name, out var queue))
            {
                UsingConnections.TryAdd(name, new ConcurrentQueue<MySqlConnection>());
                UsingConnections.TryGetValue(name, out queue);
            }
            queue.Enqueue(connection);
        }
        /// <summary>
        /// 加入空闲池中
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="name"></param>
        static void AddFree(MySqlConnection connection, string name)
        {
            if (connection == null)
                return;
            if (!IsManagement || connection.State != ConnectionState.Open)
            {
                Close(connection, name);
                return;
            }
            if (!FreeConnections.TryGetValue(name, out var queue))
            {
                FreeConnections.TryAdd(name, new ConcurrentQueue<MySqlConnection>());
                FreeConnections.TryGetValue(name, out queue);
            }
            if (queue.Count > 100 && queue.TryDequeue(out var old))
            {
                Close(old, name);
            }
            queue.Enqueue(connection);
        }
        #endregion

        #region 开关

        /// <summary>
        /// 初始化连接对象
        /// </summary>
        /// <returns></returns>
        internal static MySqlConnection InitConnection(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new EntityModelDbException("连接字符串的配置名称不能为空");
            }
            var constr = ConfigurationHelper.GetConnectionString(name, null);

            if (string.IsNullOrEmpty(constr))
            {
                throw new EntityModelDbException($"无法找到配置名称为{name}的连接字符串");
            }
            try
            {
                //var b = new MySqlConnectionStringBuilder(connectionString);
                var connection = new MySqlConnection(constr);
                //int id = connection.ConnectionString.GetHashCode();
                //DependencyScope.DisposeFunc.Add(() => CloseByFree(connection, name));
                if (!Connections.TryGetValue(name, out var dataBaseInfo))
                {
                    Connections.TryAdd(name, new DataBaseInfo
                    {
                        ConnectionString = constr
                    });
                    Connections.TryGetValue(name, out dataBaseInfo);
                }
                dataBaseInfo.ActiveConnections.TryAdd(connection.GetHashCode(), new ConnectionInfo
                {
                    DateTime = DateTime.Now,
                    Connection = connection
                });
                AddUsing(connection, name);
                connection.Open();
                logger.Debug("打开连接数：{0}", dataBaseInfo.ActiveConnections.Count);
                return connection;
            }
            catch (Exception exception)
            {
                logger.Exception(exception, nameof(InitConnection));
            }
            return null;
        }

        /// <summary>
        /// 关闭连接对象
        /// </summary>
        /// <returns></returns>
        internal static void CloseConnection(MySqlConnection connection, string name)
        {
            if (connection == null)
            {
                return;
            }
            if (Connections.TryGetValue(name, out var list))
                list.ActiveConnections.TryRemove(connection.GetHashCode(), out _);

            if (connection.State == ConnectionState.Open)
            {
                try
                {
                    connection.Close();
                }
                catch (Exception exception)
                {
                    logger.Exception(exception, nameof(Close));
                }
            }

            try
            {
                connection.Dispose();
            }
            catch (Exception exception)
            {
                logger.Exception(exception, nameof(Close));
            }
            logger.Debug("未关闭总数：{0}", list.ActiveConnections.Count);
        }

        #endregion

        #region IHealthCheck

        async Task<HealthInfo> IHealthCheck.Check()
        {
            var info = new HealthInfo
            {
                ItemName = nameof(MySqlConnectionsManager),
                Items = new List<HealthItem>()
            };

            var values = ConfigurationHelper.Get<Dictionary<string, string>>("ConnectionStrings");
            foreach (var kv in values)
            {
                info.Items.Add(await ConnectionTest(kv.Key, kv.Value));
            }
            return info;
        }

        private static async Task<HealthItem> ConnectionTest(string name, string constr)
        {
            HealthItem item = new HealthItem
            {
                ItemName = name
            };
            var connection = new MySqlConnection(constr);
            DateTime start = DateTime.Now;
            try
            {
                await connection.OpenAsync();
                await connection.CloseAsync();

                item.Value = (DateTime.Now - start).TotalMilliseconds;
                if (item.Value < 10)
                    item.Level = 5;
                else if (item.Value < 100)
                    item.Level = 4;
                else if (item.Value < 500)
                    item.Level = 3;
                else if (item.Value < 3000)
                    item.Level = 2;
                else item.Level = 1;
            }
            catch (Exception ex)
            {
                item.Level = -1;
                item.Details = ex.Message;
            }
            finally
            {
                connection.Dispose();
            }
            return item;
        }

        #endregion
    }
}