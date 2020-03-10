using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Agebull.Common.Configuration
{
    /// <summary>
    ///   �����ļ��İ�����
    /// </summary>
    public class ConfigurationManager
    {
        #region ʵ��

        /// <summary>
        /// ���ö���
        /// </summary>
        public IConfigurationSection Configuration { get; private set; }

        /// <summary>
        /// ��ʾʽ�������ö���(����)
        /// </summary>
        public ConfigurationManager Child(string section)
        {
            return new ConfigurationManager
            {
                Configuration = Configuration.GetSection(section)
            };
        }

        /// <summary>
        /// תΪǿ��������
        /// </summary>
        public TConfig ToConfig<TConfig>()
        {
            return Configuration.Get<TConfig>();
        }

        /// <summary>
        /// ��ʾʽ�������ö���(����)
        /// </summary>
        public TConfiguration Child<TConfiguration>(string section)
        {
            var sec = Configuration.GetSection(section);

            return sec.Exists() ? sec.Get<TConfiguration>() : default;
        }

        /// <summary>
        /// �Ƿ�Ϊ��
        /// </summary>
        public bool IsEmpty => Configuration == null || !Configuration.Exists();

        /// <summary>
        /// ȡ����
        /// </summary>
        /// <param name="key">��</param>
        /// <returns>��������</returns>
        public string this[string key]
        {
            get => key == null ? null : Configuration[key];
            set
            {
                if (key != null) Configuration[key] = value;
            }
        }

        /// <summary>
        ///   �õ��ı�ֵ
        /// </summary>
        /// <param name="key"> �� </param>
        /// <param name="def"> ȱʡֵ�������ڻ��д�� </param>
        /// <returns> �ı�ֵ </returns>
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
        ///   �õ�������ֵ
        /// </summary>
        /// <param name="key"> �� </param>
        /// <param name="def"> ȱʡֵ�������ڻ򲻺���ʱʹ�ã� </param>
        /// <returns> ������ֵ </returns>
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
        ///   �õ�������ֵ
        /// </summary>
        /// <param name="key"> �� </param>
        /// <param name="def"> ȱʡֵ�������ڻ򲻺���ʱʹ�ã� </param>
        /// <returns> ������ֵ </returns>
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
        ///   �õ�˫����ֵ
        /// </summary>
        /// <param name="key"> �� </param>
        /// <param name="def"> ȱʡֵ�������ڻ򲻺���ʱʹ�ã� </param>
        /// <returns> ˫����ֵ </returns>
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
        ///   �õ�˫����ֵ
        /// </summary>
        /// <param name="key"> �� </param>
        /// <param name="def"> ȱʡֵ�������ڻ򲻺���ʱʹ�ã� </param>
        /// <returns> ˫����ֵ </returns>
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
        ///   �õ�˫����ֵ
        /// </summary>
        /// <param name="key"> �� </param>
        /// <param name="def"> ȱʡֵ�������ڻ򲻺���ʱʹ�ã� </param>
        /// <returns> ˫����ֵ </returns>
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
        ///   �õ�˫����ֵ
        /// </summary>
        /// <param name="key"> �� </param>
        /// <returns> ˫����ֵ </returns>
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
        ///   �õ�˫����ֵ
        /// </summary>
        /// <param name="key"> �� </param>
        /// <returns> ˫����ֵ </returns>
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
        ///   ����һ����������
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="key">����</param>
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

        #region Ԥ�������

        #region Root

        private static IConfigurationRoot _root;
        /// <summary>
        /// ȫ������
        /// </summary>
        public static IConfiguration Root => _root ?? (_root = Builder.Build());

        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        public static string BasePath { get; set; }

        #endregion

        #region AppSetting

        /// <summary>
        /// ��ʾʽ�������ö���(����)
        /// </summary>
        static ConfigurationManager _appSettings;

        /// <summary>
        /// ��ʾʽ�������ö���(����)
        /// </summary>
        public static ConfigurationManager AppSettings => _appSettings ?? (_appSettings = new ConfigurationManager
        {
            Configuration = Root.GetSection("AppSettings")
        });

        #endregion

        #region ConnectionString

        /// <summary>
        /// ��ʾʽ�������ö���(����)
        /// </summary>
        static ConfigurationManager _connectionStrings;

        /// <summary>
        /// ��ʾʽ�������ö���(����)
        /// </summary>
        public static ConfigurationManager ConnectionStrings => _connectionStrings ?? (_connectionStrings = new ConfigurationManager
        {
            Configuration = Root.GetSection("ConnectionStrings")
        });


        /// <summary>
        /// ��ʾʽ�������ö���(����)
        /// </summary>
        public static ConfigurationManager Get(string section)
        {
            return new ConfigurationManager
            {
                Configuration = Root.GetSection(section)
            };
        }


        /// <summary>
        /// ǿ����ȡ���ڵ�
        /// </summary>
        public static TConfig Get<TConfig>(string section)
        {
            return Root.GetSection(section).Get<TConfig>();
        }
        #endregion


        #endregion

        #region �����ļ����


        private static ConfigurationBuilder _builder;
        /// <summary>
        /// ȫ������
        /// </summary>
        public static ConfigurationBuilder Builder
        {
            get
            {
                if (_builder != null)
                    return _builder;
                _builder = new ConfigurationBuilder();
                _builder.SetBasePath(BasePath ?? Directory.GetCurrentDirectory());
                UpdateAppsettings();
                return _builder;
            }
            set => _builder = value;
        }

        /// <summary>
        /// ˢ��AppSettings�ļ�,ʹ֮�������ȼ����
        /// </summary>
        public static void UpdateAppsettings()
        {
            var files =new string[] { "appsettings.json", "appSettings.json", "AppSettings.json", "Appsettings.json" };
            foreach (var fileName in files)
            {
                var file = Path.Combine(Environment.CurrentDirectory, fileName);
                if (File.Exists(file))
                    _builder.AddJsonFile(file, true, true);
            }
            Flush();
        }

        /// <summary>
        /// ˢ��
        /// </summary>
        private static void Flush()
        {
            _root = null;
            _appSettings = null;
            _connectionStrings = null;
        }

        /// <summary>
        /// ��ʾʽ�������ö���(����)
        /// </summary>
        /// <param name="configuration"></param>
        public static void SetConfiguration(IConfiguration configuration)
        {
            Builder.AddConfiguration(configuration);
            Flush();
        }


        /// <summary>
        /// ���������ļ�
        /// </summary>
        public static void Load(string jsonFile, bool isNewtonsoft = false)
        {
            if (isNewtonsoft)
                Builder.AddNewtonsoftJsonFile(jsonFile, true, true);
            else
                Builder.AddJsonFile(jsonFile, true, true);
            Flush();
        }

        /// <summary>
        /// ע����´�����
        /// </summary>
        /// <param name="action">���´�����</param>
        /// <param name="runNow">�Ƿ�����ִ��һ��</param>
        public static void RegistOnChange(Action action, bool runNow = true)
        {
            ChangeToken.OnChange(() => Root.GetReloadToken(), action);
            if (runNow)
                action();
        }

        #endregion

        #region ���ݻ�ȡ


        /// <summary>
        ///   �õ��ı�ֵ
        /// </summary>
        /// <param name="key"> �� </param>
        /// <param name="def"> ȱʡֵ�������ڻ򲻺���ʱʹ�ã� </param>
        /// <returns> �ı�ֵ </returns>
        public static string GetAppSetting(string key, string def = null)
        {
            return AppSettings[key] ?? def;
        }

        /// <summary>
        ///   �õ�������ֵ
        /// </summary>
        /// <param name="key"> �� </param>
        /// <param name="def"> ȱʡֵ�������ڻ򲻺���ʱʹ�ã� </param>
        /// <returns> ������ֵ </returns>
        public static int GetAppSettingInt(string key, int def)
        {
            return AppSettings.GetInt(key, def);
        }

        /// <summary>
        ///   �õ�������ֵ
        /// </summary>
        /// <param name="key"> �� </param>
        /// <param name="def"> ȱʡֵ�������ڻ򲻺���ʱʹ�ã� </param>
        /// <returns> ������ֵ </returns>
        public static long GetAppSettingLong(string key, long def)
        {
            return AppSettings.GetLong(key, def);
        }

        /// <summary>
        ///   �õ�˫����ֵ
        /// </summary>
        /// <param name="key"> �� </param>
        /// <param name="def"> ȱʡֵ�������ڻ򲻺���ʱʹ�ã� </param>
        /// <returns> ˫����ֵ </returns>
        public static double GetAppSettingDouble(string key, double def = 0.0)
        {
            return AppSettings.GetDouble(key, def);
        }

        /// <summary>
        ///   �õ�˫����ֵ
        /// </summary>
        /// <param name="key"> �� </param>
        /// <param name="def"> ȱʡֵ�������ڻ򲻺���ʱʹ�ã� </param>
        /// <returns> ˫����ֵ </returns>
        public static decimal GetAppSettingDecimal(string key, decimal def = 0M)
        {
            return AppSettings.GetDecimal(key, def);
        }

        /// <summary>
        ///   ����һ����������
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="key">����</param>
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
        ///   ��ȡ���Ӵ��Ľڵ���Ϣ
        /// </summary>
        /// <param name="key">����</param>
        /// <param name="def">�Ҳ�����ȱʡֵ</param>
        /// <returns> </returns>
        public static string GetConnectionString(string key, string def = "")
        {
            return key == null ? def : ConnectionStrings[key];
        }

        /// <summary>
        ///   �������Ӵ��Ľڵ���Ϣ
        /// </summary>
        /// <param name="key">����</param>
        /// <param name="connectionString">�����ַ�</param>
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