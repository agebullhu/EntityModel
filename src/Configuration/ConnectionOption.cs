using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Agebull.Common.Configuration
{
    /// <summary>
    /// 资源连接配置项目
    /// </summary>
    [DataContract, Serializable, JsonObject(MemberSerialization.OptOut)]
    public class ConnectionOption
    {
        /// <summary>
        /// 连接类型
        /// </summary>
        public enum ConnectionType
        {
            None,
            Redis,
            MsSql,
            MySql
        }

        /// <summary>
        /// 类型
        /// </summary>
        [JsonProperty("type")]
        public ConnectionType Type
        {
            get;
            set;
        }

        /// <summary>
        /// 基本连接字符串
        /// </summary>
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        [JsonProperty("dataBase")]
        public string DataBase { get; set; }
    }
}