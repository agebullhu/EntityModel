﻿using System.Collections.Generic;
using System.Linq;
using Agebull.EntityModel.Common;

namespace Agebull.EntityModel.Redis
{
    /// <summary>
    /// 统一构建RedisKey的格式
    /// </summary>
    public static class DataKeyBuilder
    {
        /// <summary>
        /// 默认的数据键名称生成器
        /// </summary>
        /// <typeparam name="TData">数据键</typeparam>
        /// <param name="data">数据</param>
        /// <returns>数据键名</returns>
        public static string DataKey<TData>(TData data) where TData : class, IIdentityData
        {
            return $"data:{typeof(TData).Name.ToLower()}:{data.Id}";
        }

        /// <summary>
        /// 默认的数据键名称生成器
        /// </summary>
        /// <typeparam name="TData">数据键</typeparam>
        /// <param name="id">数据键</param>
        /// <returns>数据键名</returns>
        public static string DataKey<TData>(object id)
        {
            return $"data:{typeof(TData).Name.ToLower()}:{id}";
        }
        /// <summary>
        /// 默认的数据键名称生成器
        /// </summary>
        /// <param name="type">数据</param>
        /// <param name="id">数据键</param>
        /// <returns>数据键名</returns>
        public static string DataKey(string type, int id)
        {
            return $"data:{type.ToLower()}:{id}";
        }
        /// <summary>
        /// 连接为Key
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static string ToKey(IEnumerable<string> sp)
        {
            return sp.LinkToString(':');
        }

        /// <summary>
        /// 连接为Key
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sub"></param>
        /// <returns></returns>
        public static string ToKey(string type, string sub)
        {
            return $"{type}:{sub}";
        }

        /// <summary>
        /// 连接为Key
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sub"></param>
        /// <returns></returns>
        public static string ToKey(string type, params object[] sub)
        {
            return $"{type}:{sub.LinkToString(':')}";
        }

        /// <summary>
        /// 连接为Key
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sub"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static string ToKey(string type, string sub, string level)
        {
            return $"{type}:{sub}:{level}";
        }
        /// <summary>
        /// 连接为Key
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToEntityKey(string type)
        {
            return $"EOS:{type}";
        }

        /// <summary>
        /// 连接为Key
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string ToEntityKey(string type, object id)
        {
            return $"EOS:{type}:{id}";
        }

        /// <summary>
        /// 连接为Key
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToEntityIdsKey(string type)
        {
            return $"EOS:{type}:IdSort";
        }

        /// <summary>
        /// 连接为索引Key
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string ToEntityIndexKey(string entityName, string fieldName)
        {
            return $"EFI:{entityName}:{fieldName}";
        }


        /// <summary>
        /// 生成产品额度分配的键
        /// </summary>
        /// <param name="fid">产品ID</param>
        /// <param name="type">类型(1额度标识\2机构对照ID\3人员对照ID)</param>
        /// <param name="id">额度标识\机构对照ID\人员对照ID</param>
        /// <returns></returns>
        public static string ToQuotaPlanKey(this int fid, int type = 0, int id = 0)
        {
            return $"qp:{(fid == 0 ? "*" : fid.ToString())}:{(type == 0 ? "*" : type.ToString())}:{(id == 0 ? "*" : id.ToString())}";
        }


        /// <summary>
        /// 连接为Key
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static string ToKey(params object[] sp)
        {
            return sp.LinkToString(':');
        }
    }
}