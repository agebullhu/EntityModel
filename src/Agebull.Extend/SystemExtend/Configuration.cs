// 所在工程：GBoxtCommonService
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:13
namespace Agebull.Common.Settings
{
    /// <summary>
    ///   自定的配置文件
    /// </summary>
    public class AppliationConfiguration
    {
        private static readonly ConfigurationSettings _Settings = new ConfigurationSettings() ;

        /// <summary>
        ///   配置集合
        /// </summary>
        public static ConfigurationSettings Settings
        {
            get
            {
                return _Settings ;
            }
        }
    }
}
