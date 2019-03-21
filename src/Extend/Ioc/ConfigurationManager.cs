using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Agebull.Common.Configuration
{
    /// <summary>
    ///   配置文件的帮助类
    /// </summary>
    public class ConfigurationManager
    {
        #region 实例

        /// <summary>
        /// 配置对象
        /// </summary>
        public IConfigurationSection Configuration { get; private set; }

        /// <summary>
        /// 显示式设置配置对象(依赖)
        /// </summary>
        public ConfigurationManager Child(string section)
        {

            return new ConfigurationManager
            {
                Configuration = Configuration.GetSection(section)
            };
        }

        /// <summary>
        /// 转为强类型配置
        /// </summary>
        public TConfig ToConfig<TConfig>(string section)
        {
            return Configuration.Get<TConfig>();
        }

        /// <summary>
        /// 显示式设置配置对象(依赖)
        /// </summary>
        public TConfiguration Child<TConfiguration>(string section)
        {
            var sec = Configuration.GetSection(section);

            return sec.Exists() ? sec.Get<TConfiguration>() : default(TConfiguration);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsEmpty => Configuration == null || !Configuration.Exists();

        /// <summary>
        /// 取配置
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>配置内容</returns>
        public string this[string key]
        {
            get => key == null ? null : Configuration[key];
            set
            {
                if (key != null) Configuration[key] = value;
            }
        }

        /// <summary>
        ///   得到文本值
        /// </summary>
        /// <param name="key"> 键 </param>
        /// <param name="def"> 缺省值（不存在会回写） </param>
        /// <returns> 文本值 </returns>
        public string GetStr(string key, string def)
        {
            if (key == null)
            {
                return def;
            }
            var value = this[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                return this[key] = def;
            }
            return value;
        }

        /// <summary>
        ///   得到长整数值
        /// </summary>
        /// <param name="key"> 键 </param>
        /// <param name="def"> 缺省值（不存在或不合理时使用） </param>
        /// <returns> 长整数值 </returns>
        public int GetInt(string key, int def)
        {
            if (key == null)
            {
                return def;
            }
            var value = this[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                return def;
            }
            return !int.TryParse(value, out var vl) ? def : vl;
        }

        /// <summary>
        ///   得到长整数值
        /// </summary>
        /// <param name="key"> 键 </param>
        /// <param name="def"> 缺省值（不存在或不合理时使用） </param>
        /// <returns> 长整数值 </returns>
        public long GetLong(string key, long def)
        {
            if (key == null)
            {
                return def;
            }
            var value = this[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                return def;
            }
            return !long.TryParse(value, out var vl) ? def : vl;
        }

        /// <summary>
        ///   得到双精数值
        /// </summary>
        /// <param name="key"> 键 </param>
        /// <param name="def"> 缺省值（不存在或不合理时使用） </param>
        /// <returns> 双精数值 </returns>
        public bool GetBool(string key, bool def = false)
        {
            if (key == null)
            {
                return def;
            }

            var value = this[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                return def;
            }
            return !bool.TryParse(value, out var vl) ? def : vl;
        }

        /// <summary>
        ///   得到双精数值
        /// </summary>
        /// <param name="key"> 键 </param>
        /// <param name="def"> 缺省值（不存在或不合理时使用） </param>
        /// <returns> 双精数值 </returns>
        public double GetDouble(string key, double def = 0.0)
        {
            if (key == null)
            {
                return def;
            }

            var value = this[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                return def;
            }
            return !double.TryParse(value, out var vl) ? def : vl;
        }

        /// <summary>
        ///   得到双精数值
        /// </summary>
        /// <param name="key"> 键 </param>
        /// <param name="def"> 缺省值（不存在或不合理时使用） </param>
        /// <returns> 双精数值 </returns>
        public decimal GetDecimal(string key, decimal def = 0M)
        {
            if (key == null)
            {
                return def;
            }

            var value = this[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                return def;
            }
            return !decimal.TryParse(value, out var vl) ? def : vl;
        }

        /// <summary>
        ///   得到双精数值
        /// </summary>
        /// <param name="key"> 键 </param>
        /// <returns> 双精数值 </returns>
        public DateTime? GetDateTime(string key)
        {
            if (key == null)
            {
                return null;
            }

            var value = this[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (DateTime.TryParse(value, out var vl)) return vl;
            return null;
        }

        /// <summary>
        ///   得到双精数值
        /// </summary>
        /// <param name="key"> 键 </param>
        /// <returns> 双精数值 </returns>
        public Guid? GetGuid(string key)
        {
            if (key == null)
            {
                return null;
            }

            var value = this[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (Guid.TryParse(value, out var vl)) return vl;
            return null;
        }

        /// <summary>
        ///   配置一个配置内容
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="key">名称</param>
        /// <param name="value"> </param>
        public void SetValue<T>(string key, T value) where T : struct
        {
            if (key == null)
            {
                return;
            }

            this[key] = value.ToString();
        }
        #endregion

        #region 预定义对象

        #region Root

        private static IConfigurationRoot _root;
        /// <summary>
        /// 全局配置
        /// </summary>
        public static IConfiguration Root => _root ?? (_root = Builder.Build());
        /// <summary>
        /// 基本目录
        /// </summary>
        public static string BasePath { get; set; }

        #endregion

        #region AppSetting

        /// <summary>
        /// 显示式设置配置对象(依赖)
        /// </summary>
        static ConfigurationManager _appSettings;

        /// <summary>
        /// 显示式设置配置对象(依赖)
        /// </summary>
        public static ConfigurationManager AppSettings => _appSettings ?? (_appSettings = new ConfigurationManager
        {
            Configuration = Root.GetSection("AppSettings")
        });

        #endregion

        #region ConnectionString

        /// <summary>
        /// 显示式设置配置对象(依赖)
        /// </summary>
        static ConfigurationManager _connectionStrings;

        /// <summary>
        /// 显示式设置配置对象(依赖)
        /// </summary>
        public static ConfigurationManager ConnectionStrings => _connectionStrings ?? (_connectionStrings = new ConfigurationManager
        {
            Configuration = Root.GetSection("ConnectionStrings")
        });


        /// <summary>
        /// 显示式设置配置对象(依赖)
        /// </summary>
        public static ConfigurationManager Get(string section)
        {
            return new ConfigurationManager
            {
                Configuration = Root.GetSection(section)
            };
        }


        /// <summary>
        /// 强类型取根节点
        /// </summary>
        public static TConfig Get<TConfig>(string section)
        {
            return Root.GetSection(section).Get<TConfig>();
        }
        #endregion


        #endregion

        #region 配置文件组合


        private static ConfigurationBuilder _builder;
        /// <summary>
        /// 全局配置
        /// </summary>
        public static ConfigurationBuilder Builder
        {
            get
            {
                if (_builder != null)
                    return _builder;
                _builder = new ConfigurationBuilder();
                _builder.SetBasePath(BasePath ?? Directory.GetCurrentDirectory());
                var file = Path.Combine(Environment.CurrentDirectory, "appsettings.json");
                if (File.Exists(file))
                    _builder.AddJsonFile("appsettings.json");
                else
                {
                    file = Path.Combine(Environment.CurrentDirectory, "appSettings.json");
                    if (File.Exists(file))
                        _builder.AddJsonFile("appSettings.json");
                    else
                    {
                        file = Path.Combine(Environment.CurrentDirectory, "AppSettings.json");
                        if (File.Exists(file))
                            _builder.AddJsonFile("AppSettings.json");
                        else
                        {
                            file = Path.Combine(Environment.CurrentDirectory, "Appsettings.json");
                            if (File.Exists(file))
                                _builder.AddJsonFile("Appsettings.json");
                        }
                    }
                }
                _root = null;
                _appSettings = null;
                _connectionStrings = null;
                return _builder;
            }
            set => _builder = value;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private static void Flush()
        {
            _root = null;
            _appSettings = null;
            _connectionStrings = null;
        }

        /// <summary>
        /// 显示式设置配置对象(依赖)
        /// </summary>
        /// <param name="configuration"></param>
        public static void SetConfiguration(IConfiguration configuration)
        {
            Builder.AddConfiguration(configuration);
            Flush();
        }


        /// <summary>
        /// 载入配置文件
        /// </summary>
        public static void Load(string jsonFile)
        {
            Builder.AddJsonFile(jsonFile);
            Flush();
        }

        #endregion

        #region 内容获取


        /// <summary>
        ///   得到文本值
        /// </summary>
        /// <param name="key"> 键 </param>
        /// <param name="def"> 缺省值（不存在或不合理时使用） </param>
        /// <returns> 文本值 </returns>
        public static string GetAppSetting(string key, string def = null)
        {
            return AppSettings[key] ?? def;
        }

        /// <summary>
        ///   得到长整数值
        /// </summary>
        /// <param name="key"> 键 </param>
        /// <param name="def"> 缺省值（不存在或不合理时使用） </param>
        /// <returns> 长整数值 </returns>
        public static int GetAppSettingInt(string key, int def)
        {
            return AppSettings.GetInt(key, def);
        }

        /// <summary>
        ///   得到长整数值
        /// </summary>
        /// <param name="key"> 键 </param>
        /// <param name="def"> 缺省值（不存在或不合理时使用） </param>
        /// <returns> 长整数值 </returns>
        public static long GetAppSettingLong(string key, long def)
        {
            return AppSettings.GetLong(key, def);
        }

        /// <summary>
        ///   得到双精数值
        /// </summary>
        /// <param name="key"> 键 </param>
        /// <param name="def"> 缺省值（不存在或不合理时使用） </param>
        /// <returns> 双精数值 </returns>
        public static double GetAppSettingDouble(string key, double def = 0.0)
        {
            return AppSettings.GetDouble(key, def);
        }

        /// <summary>
        ///   得到双精数值
        /// </summary>
        /// <param name="key"> 键 </param>
        /// <param name="def"> 缺省值（不存在或不合理时使用） </param>
        /// <returns> 双精数值 </returns>
        public static decimal GetAppSettingDecimal(string key, decimal def = 0M)
        {
            return AppSettings.GetDecimal(key, def);
        }

        /// <summary>
        ///   配置一个配置内容
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="key">名称</param>
        /// <param name="value"> </param>
        public static void SetAppSetting<T>(string key, T value) where T : struct
        {
            if (key == null)
            {
                return;
            }

            AppSettings[key] = value.ToString();
        }

        /// <summary>
        ///   获取连接串的节点信息
        /// </summary>
        /// <param name="key">名称</param>
        /// <param name="def">找不到的缺省值</param>
        /// <returns> </returns>
        public static string GetConnectionString(string key, string def = "")
        {
            return key == null ? def : ConnectionStrings[key];
        }

        /// <summary>
        ///   设置连接串的节点信息
        /// </summary>
        /// <param name="key">名称</param>
        /// <param name="connectionString">连接字符</param>
        public static void SetConnectionString(string key, string connectionString)
        {
            if (key == null)
            {
                return;
            }
            ConnectionStrings[key] = connectionString;
        }

        #endregion
    }
}