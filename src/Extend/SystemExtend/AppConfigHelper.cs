// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:13
#if !SILVERLIGHT

#region

using System;
using System.Configuration;

#endregion

namespace Agebull.Common.Base
{
    /// <summary>
    ///   配置文件的帮助类
    /// </summary>
    public static class AppConfigHelper
    {
        /// <summary>
        ///   得到AppSetting中的文本值
        /// </summary>
        /// <param name="key"> 键 </param>
        /// <param name="def"> 缺省值（不存在或不合理时使用） </param>
        /// <returns> 文本值 </returns>
        public static string GetAppSettingString(string key, string def = null)
        {
            string value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(value)
                           ? def
                           : value;
        }

        /// <summary>
        ///   得到AppSetting中的长整数值
        /// </summary>
        /// <param name="key"> 键 </param>
        /// <param name="def"> 缺省值（不存在或不合理时使用） </param>
        /// <returns> 长整数值 </returns>
        public static int GetAppSettingInt(string key, int def)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                return def;
            }
            int.TryParse(value, out def);
            return def;
        }

        /// <summary>
        ///   得到AppSetting中的长整数值
        /// </summary>
        /// <param name="key"> 键 </param>
        /// <param name="def"> 缺省值（不存在或不合理时使用） </param>
        /// <returns> 长整数值 </returns>
        public static long GetAppSettingLong(string key, long def)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                return def;
            }
            long.TryParse(value, out def);
            return def;
        }

        /// <summary>
        ///   得到AppSetting中的双精数值
        /// </summary>
        /// <param name="key"> 键 </param>
        /// <param name="def"> 缺省值（不存在或不合理时使用） </param>
        /// <returns> 双精数值 </returns>
        public static double GetAppSettingDouble(string key, double def = 0.0)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                return def;
            }
            double.TryParse(value, out def);
            return def;
        }

        /// <summary>
        ///   配置一个配置内容
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="key"> </param>
        /// <param name="value"> </param>
        public static void SetValue<T>(string key, T value) where T : struct
        {
            if (key == null)
            {
                return;
            }
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection appSettings = config.AppSettings;
            if (appSettings == null)
            {
                return;
            }
            appSettings.Settings.Remove(key);
            appSettings.Settings.Add(key, value.ToString());
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");

        }

        /// <summary>
        ///   获取连接串的节点信息
        /// </summary>
        /// <param name="key"> </param>
        /// <returns> </returns>
        public static string GetConnectionString(string key)
        {
            ConnectionStringSettings css = ConfigurationManager.ConnectionStrings[key];
            if (css == null || string.IsNullOrWhiteSpace(css.ConnectionString))
            {
                throw new Exception(string.Format("Web.Config文件中的ConnectionStrings节不存在{0}节点！", key));
            }
            return css.ConnectionString;
        }

        /// <summary>
        ///   设置连接串的节点信息
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="providerName">数据提供程序</param>
        /// <param name="connectionString">连接字符</param>
        public static void SetConnectionString(string name, string providerName, string connectionString)
        {
            if (name == null)
            {
                return;
            }
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStrings = config.ConnectionStrings;
            if (connectionStrings == null)
            {
                return;
            }
            connectionStrings.ConnectionStrings.Remove(name);
            connectionStrings.ConnectionStrings.Add(new ConnectionStringSettings
            {
                Name = name,
                ProviderName = providerName,
                ConnectionString = connectionString
            });
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}

#endif
