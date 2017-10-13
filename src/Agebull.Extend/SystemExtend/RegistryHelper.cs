// 所在工程：GBoxtCommonService
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:13
#if !SILVERLIGHT

#region

using System;
using System.Security.AccessControl;


using Microsoft.Win32;

#endregion

namespace Agebull.Common.Base
{
    /// <summary>
    ///   注册表操作的辅助类
    /// </summary>
    public static class RegistryHelper
    {
        /// <summary>
        ///   得到子值
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="sub"> </param>
        /// <returns> </returns>
        public static RegistryKey SubKey(RegistryKey key, string sub)
        {
            return key.OpenSubKey(sub, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl) ?? key.CreateSubKey(sub);
        }

        /// <summary>
        ///   得到文本值
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="def"> </param>
        /// <returns> </returns>
        public static string GetErpStringValue(string name, string def)
        {
            return GetStringValue(GetQFErpRootKey(), name, def);
        }

        /// <summary>
        ///   得到文本值
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="def"> </param>
        /// <returns> </returns>
        public static bool GetErpBoolValue(string name, bool def = true)
        {
            return GetBoolValue(GetQFErpRootKey(), name, def);
        }

        /// <summary>
        ///   得到文本值
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static void SetErpBoolValue(string name, bool value)
        {
            SetBoolValue(GetQFErpRootKey(), name, value);
        }

        /// <summary>
        ///   得到文本值
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static void SetErpStringValue(string name, string value)
        {
            SetStringValue(GetQFErpRootKey(), name, value);
        }

        /// <summary>
        ///   得到清风ERP在注册表信息的根
        /// </summary>
        /// <returns> </returns>
        public static RegistryKey GetQFErpRootKey()
        {
            using (var root = RegistryKey.OpenBaseKey(RegistryHive.CurrentConfig, RegistryView.Default))
            {
                var r = root.OpenSubKey("上海秦奋网络科技有限公司") ?? root.CreateSubKey("上海秦奋网络科技有限公司");
                if (r != null)
                {
                    return r.OpenSubKey("清风企业智能管理系统") ?? root.CreateSubKey("清风企业智能管理系统");
                }
            }
            return null;
        }

        /// <summary>
        ///   得到文本值
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="name"> </param>
        /// <param name="def"> </param>
        /// <returns> </returns>
        public static string GetStringValue(RegistryKey key, string name, string def)
        {
            object re = key.GetValue(name);
            if (re == null)
            {
                key.SetValue(name, def);
                key.Flush();
                return def;
            }
            return re.ToString();
        }

        /// <summary>
        ///   配置文本值
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="name"> </param>
        /// <param name="val"> </param>
        public static void SetStringValue(RegistryKey key, string name, string val)
        {
            key.SetValue(name, val);
            key.Flush();
        }

        /// <summary>
        ///   得到文本值
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="name"> </param>
        /// <param name="def"> </param>
        /// <returns> </returns>
        public static bool GetBoolValue(RegistryKey key, string name, bool def)
        {
            object re = key.GetValue(name);
            if (re == null)
            {
                key.SetValue(name, def);
                key.Flush();
                return def;
            }
            return Convert.ToBoolean(re);
        }

        /// <summary>
        ///   配置文本值
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="name"> </param>
        /// <param name="val"> </param>
        public static void SetBoolValue(RegistryKey key, string name, bool val)
        {
            key.SetValue(name, val);
            key.Flush();
        }
    }
}

#endif
