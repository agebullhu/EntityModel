using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Agebull.Common.Configuration
{
    /// <summary>
    /// 机器配置对象
    /// </summary>
    [DataContract, Serializable, JsonObject(MemberSerialization.OptOut)]
    public class Settings
    {
        /// <summary>
        /// 上级配置，全局配置为空
        /// </summary>
        public Settings Parent { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 连接对象
        /// </summary>
        [JsonProperty("connections")]
        public Dictionary<string, ConnectionOption> Connections { get; set; }


        /// <summary>
        /// 内部API连接地址
        /// </summary>
        [JsonProperty("internalApis")]
        public Dictionary<string, string> InternalApis { get; set; }

        /// <summary>
        /// 内部API连接地址
        /// </summary>
        [JsonProperty("appSettings")]
        public Dictionary<string, string> AppSettings { get; set; }


        /// <summary>
        /// 取数据库连接字符串
        /// </summary>
        /// <param name="key">名称</param>
        /// <param name="dbName"></param>
        /// <returns>API地址</returns>
        /// <exception cref="FormatException">找不到Api地址</exception>
        public string GetDbConnectionString(string key, string dbName)
        {
            if (Connections == null || !Connections.TryGetValue(key, out var value))
            {
                if (Parent != null)
                    return Parent.GetDbConnectionString(key, dbName);
                throw new ArgumentOutOfRangeException("key", $"数据库连接字符串（{key}）不存在");
            }
            if (value.ConnectionString == null)
            {
                if (Parent != null)
                    return Parent.GetDbConnectionString(key, value.DataBase ?? dbName);
                throw new ArgumentOutOfRangeException("key", $"数据库连接字符串（{key}）不正确");
            }
            return string.Format(value.ConnectionString, value.DataBase ?? dbName);
        }

        /// <summary>
        /// 取Redis连接字符串
        /// </summary>
        /// <param name="key">名称</param>
        /// <returns>API地址</returns>
        /// <exception cref="FormatException">找不到Api地址</exception>
        public string GetRedisConnectionString(string key)
        {
            if (Connections == null || !Connections.TryGetValue(key, out var value) || value.ConnectionString == null)
            {
                if (Parent != null)
                    return Parent.GetRedisConnectionString(key);
                throw new ArgumentOutOfRangeException("key", $"Redis连接字符串（{key}）");
            }
            return value.ConnectionString;
        }

        /// <summary>
        /// 取API地址值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>API地址</returns>
        /// <exception cref="FormatException">找不到Api地址</exception>
        public string GetApiHost(string name)
        {
            if (InternalApis == null || !InternalApis.TryGetValue(name, out var value))
            {
                if (Parent != null)
                    return Parent.GetApiHost(name);
                throw new ArgumentOutOfRangeException("name", $"找不到Api地址（{name}）");
            }
            return value;
        }

        /// <summary>
        /// 取文本值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="def">值不存在或为空的缺省值</param>
        /// <returns>文本值</returns>
        public string GetSettings(string name, string def = null)
        {
            if (AppSettings == null || !AppSettings.TryGetValue(name, out var value))
            {
                return Parent != null ? Parent.GetSettings(name, def) : def;
            }
            return value;
        }
        /// <summary>
        /// 取整数值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="def">值不存在或为空的缺省值</param>
        /// <returns>整数值</returns>
        /// <exception cref="FormatException">参数不能转换为整数</exception>
        public int GetInt(string name, int def = 0)
        {
            if (AppSettings == null || !AppSettings.TryGetValue(name, out var value))
                return Parent?.GetInt(name, def) ?? def;
            return int.Parse(value);
        }

        /// <summary>
        /// 取日期值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="def">值不存在或为空的缺省值</param>
        /// <returns>日期值</returns>
        /// <exception cref="FormatException">参数不能转换为日期</exception>
        public DateTime GetDateTime(string name, DateTime def)
        {
            if (AppSettings == null || !AppSettings.TryGetValue(name, out var value))
                return Parent?.GetDateTime(name, def) ?? def;
            return DateTime.Parse(value);
        }

        /// <summary>
        /// 取长整数值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="def">值不存在或为空的缺省值</param>
        /// <returns>长整数值</returns>
        /// <exception cref="FormatException">参数不能转换为长整数</exception>
        public long GetLong(string name, long def = 0)
        {
            if (AppSettings == null || !AppSettings.TryGetValue(name, out var value))
                return Parent?.GetLong(name, def) ?? def;
            return long.Parse(value);
        }

        /// <summary>
        /// 取布尔值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="def">值不存在或为空的缺省值</param>
        /// <returns>布尔值</returns>
        /// <exception cref="FormatException">参数不能转换为布尔</exception>
        public bool GetBool(string name, bool def = false)
        {
            if (AppSettings == null || !AppSettings.TryGetValue(name, out var value))
                return def;
            return bool.Parse(value);
        }

        /// <summary>
        /// 取小数值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="def">值不存在或为空的缺省值</param>
        /// <returns>小数值</returns>
        /// <exception cref="FormatException">参数不能转换为小数</exception>
        public Decimal GetDecimal(string name, Decimal def = 0M)
        {
            if (AppSettings == null || !AppSettings.TryGetValue(name, out var value))
                return Parent?.GetDecimal(name, def) ?? def;
            return Decimal.Parse(value);
        }

        /// <summary>
        /// 取单精小数值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="def">值不存在或为空的缺省值</param>
        /// <returns>单精小数值</returns>
        /// <exception cref="FormatException">参数不能转换为单精小数</exception>
        public Single GetSingle(string name, Single def = 0F)
        {
            if (AppSettings == null || !AppSettings.TryGetValue(name, out var value))
                return Parent?.GetSingle(name, def) ?? def;
            return Single.Parse(value);
        }

        /// <summary>
        /// 取双精小数值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="def">值不存在或为空的缺省值</param>
        /// <returns>双精小数值</returns>
        /// <exception cref="FormatException">参数不能转换为双精小数</exception>
        public Double GetDouble(string name, Double def = 0.0)
        {
            if (AppSettings == null || !AppSettings.TryGetValue(name, out var value))
                return Parent?.GetDouble(name, def) ?? def;
            return Double.Parse(value);
        }
    }
}