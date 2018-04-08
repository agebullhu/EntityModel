using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Agebull.Common.Configuration
{
    /// <summary>
    /// 读取配置文件
    /// </summary>
    public class AppConfigurtaionServices
    {
        private static IOptions<MachineConfig> _globalConfig;
        private static IConfigurationRoot _globalRoot;
        private static IOptions<LacalConfig> _localConfig;
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            var localBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"local.json", optional: true, reloadOnChange: true)
                .Build();

            var lc = new ServiceCollection().AddOptions()
                .Configure<LacalConfig>(localBuilder)
                .BuildServiceProvider();
            _localConfig = lc.GetService<IOptions<LacalConfig>>();

            _globalRoot = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .AddJsonFile(_localConfig.Value.GlobalFile, optional: true, reloadOnChange: true)
                .Build();

            var sp = new ServiceCollection().AddOptions()
                .Configure<MachineConfig>(_globalRoot)
                .BuildServiceProvider();

            _globalConfig = sp.GetService<IOptions<MachineConfig>>();
        }

        /// <summary>
        /// 配置对象
        /// </summary>
        public static IOptions<MachineConfig> Configuration
        {
            get
            {
                if (_globalConfig == null)
                    Initialize();
                return _globalConfig;
            }
        }

        /// <summary>
        /// 配置对象
        /// </summary>
        public static IOptions<T> GetConfiguration<T>(string path) where T : class, new()
        {
            if (_globalConfig == null)
                Initialize();
            var sp = new ServiceCollection().AddOptions()
                .Configure<T>(_globalRoot.GetSection(path))
                .BuildServiceProvider();

            return sp.GetService<IOptions<T>>();
        }
    }
}