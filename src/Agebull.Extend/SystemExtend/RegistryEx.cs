#if !NETCOREAPP
using Microsoft.Win32;

namespace Agebull.Common.Base
{
    /// <summary>
    ///   注册表操作的辅助类
    /// </summary>
    public static class RegistryEx
    {
        /// <summary>
        ///   得到子值
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="sub"> </param>
        /// <returns> </returns>
        public static RegistryKey SubKey(this RegistryKey key, string sub)
        {
            return RegistryHelper.SubKey(key, sub);
        }

        /// <summary>
        ///   得到文本值
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="name"> </param>
        /// <param name="def"> </param>
        /// <returns> </returns>
        public static string GetStringValue(this RegistryKey key, string name, string def)
        {
            return RegistryHelper.GetStringValue(key, name, def);
        }

        /// <summary>
        ///   配置文本值
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="name"> </param>
        /// <param name="val"> </param>
        public static void SetStringValue(this RegistryKey key, string name, string val)
        {
            RegistryHelper.SetStringValue(key, name, val);
        }

        /// <summary>
        ///   得到文本值
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="name"> </param>
        /// <param name="def"> </param>
        /// <returns> </returns>
        public static bool GetBoolValue(this RegistryKey key, string name, bool def)
        {
            return RegistryHelper.GetBoolValue(key, name, def);
        }

        /// <summary>
        ///   配置文本值
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="name"> </param>
        /// <param name="val"> </param>
        public static void SetBoolValue(this RegistryKey key, string name, bool val)
        {
            RegistryHelper.SetBoolValue(key, name, val);
        }
    }
}
#endif