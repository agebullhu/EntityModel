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
        internal static MySqlConnectionsManager Instance { get; set; }

        /// <summary>
        ///     初始化
        /// </summary>
        Task ILifeFlow.Initialize()
        {
            InternalInitialize();
            ConfigurationHelper.RegistOnChange("ConnectionStrings", CheckOption, false);
            return Task.CompletedTask;
        }

        /// <summary>
        ///     配置检查(关闭已过时的配置连接)
        /// </summary>
        void CheckOption()
        {
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
        /// 取得一个空闲连接对象
        /// </summary>
        /// <returns></returns>
        internal static MySqlConnection GetConnection(string name)
        {
            return InitConnection(name);
        }

        /// <summary>
        /// 关闭连接对象
        /// </summary>
        /// <returns></returns>
        internal static void Close(MySqlConnection connection, string name)
        {
            CloseConnection(connection, name);
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
                var connection = new MySqlConnection(constr);
                connection.Open();
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