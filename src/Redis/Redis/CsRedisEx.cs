using Agebull.EntityModel.Common;
using Agebull.EntityModel.Redis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Agebull.Common.DataModel.Redis
{
    /// <summary>
    /// REDIS代理类
    /// </summary>
    public class CSRedisEx : CSRedisBase, IRedis
    {
        #region 文本读写

        /// <summary>
        /// 删除KEY
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(string key)
        {
            Client.Del(key);
        }

        /// <summary>
        /// 查找关删除KEY
        /// </summary>
        /// <param name="pattern">条件</param>
        public void FindAndRemoveKey(string pattern)
        {
            var keys = Client.Keys(pattern);
            Client.Del(keys);
        }

        /// <summary>
        /// 取文本
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return Client.Get(key);
        }


        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Set(string key, string value)
        {
            Client.Set(key, value);
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public void Set(string key, string value, DateTime last)
        {
            Client.Set(key, value, (int)(last - DateTime.Now).TotalSeconds);
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public void Set(string key, string value, TimeSpan span)
        {
            Client.Set(key, value, (int)span.TotalSeconds);
        }

        #endregion

        #region 值读写

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public void SetTime(string key, DateTime last)
        {
            Client.Expire(key, last - DateTime.Now);
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public void SetTime(string key, TimeSpan span)
        {
            Client.Expire(key, span);
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetValue<T>(string key)
            where T : struct
        {
            var str = Client.Get(key);
            return string.IsNullOrEmpty(str) ? default : JsonConvert.DeserializeObject<T>(str);
        }
        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetValue<T>(string key, T value)
            where T : struct
        {
            Client.Set(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public void SetValue<T>(string key, T value, DateTime last)
            where T : struct
        {
            SetValue(key, value, DateTime.Now - last);
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public void SetValue<T>(string key, T value, TimeSpan span)
            where T : struct
        {
            Client.Set(key, JsonConvert.SerializeObject(value), (int)span.TotalSeconds);
        }

        #endregion

        #region 对象读写


        /// <summary>
        /// 查找关删除KEY
        /// </summary>
        /// <param name="pattern">条件</param>
        public List<T> GetAll<T>(string pattern)
            where T : class
        {
            List<T> lists = new List<T>();
            long cursor = 0;
            do
            {
                var result = Client.Scan(cursor, pattern, 100);
                cursor = result.Cursor;
                foreach (var key in result.Items)
                {
                    lists.Add(Get<T>(key));
                }
            } while (cursor > 0);
            return lists;
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <remarks>
        /// 如果反序列化失败会抛出异常
        /// </remarks>
        public T Get<T>(string key)
            where T : class
        {
            var str = Client.Get(key);
            return string.IsNullOrEmpty(str) ? null : JsonConvert.DeserializeObject<T>(str);
        }
        /// <summary>
        /// 试图取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>如果反序列化失败失败会访回空</returns>
        public T TryGet<T>(string key)
            where T : class
        {
            var str = Client.Get(key);
            if (string.IsNullOrEmpty(str))
                return null;
            try
            {
                return JsonConvert.DeserializeObject<T>(str);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Set<T>(string key, T value)
            where T : class
        {
            Client.Set(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public void Set<T>(string key, T value, DateTime last)
            where T : class
        {
            Client.Set(key, JsonConvert.SerializeObject(value), (int)(last - DateTime.Now).TotalSeconds);
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public void Set<T>(string key, T value, TimeSpan span)
            where T : class
        {
            Client.Set(key, JsonConvert.SerializeObject(value), (int)span.TotalSeconds);
        }

        #endregion

        #region 实体缓存支持

        /// <summary>
        /// 读缓存
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="id">键的组合</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public TData GetEntity<TData>(long id, TData def = null) where TData : class, new()
        {
            return id == 0 ? def ?? new TData() : Get<TData>(DataKeyBuilder.DataKey<TData>(id)) ?? def ?? new TData();
        }

        /// <summary>
        /// 读缓存
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="key">数据键</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public TData GetEntity<TData>(string key, TData def = null) where TData : class, new()
        {
            return Get<TData>(key) ?? def ?? new TData();
        }

        /// <summary>
        /// 缓存数据
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data">键的组合</param>
        /// <returns></returns>
        public void SetEntity<TData>(TData data) where TData : class, IIdentityData
        {
            Set(DataKeyBuilder.DataKey(data), data);
        }

        /// <summary>
        /// 缓存这些对象
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="datas">数据</param>
        /// <param name="keyFunc">设置键的方法,可为空</param>
        /// <returns></returns>
        public void CacheData<TData>(IEnumerable<TData> datas, Func<TData, string> keyFunc = null) where TData : class, IIdentityData, new()
        {
            if (keyFunc == null)
                keyFunc = DataKeyBuilder.DataKey;
            foreach (var data in datas)
            {
                Set(keyFunc(data), data);
            }
        }

        /// <summary>
        ///     缓存这些对象
        /// </summary>
        /// <returns>数据</returns>
        public void RemoveCache<TData>(long id)
        {
            if (id > 0)
                Client.Del(DataKeyBuilder.DataKey<TData>(id));
        }


        bool IRedis.SetNx(string key, byte[] value)
        {
            return Client.SetNx(key, value);
        }

        long IRedis.Incr(string key)
        {
            return Client.IncrBy(key);
        }

        long IRedis.Decr(string key)
        {
            throw new NotSupportedException("未实现");
        }

        #endregion

    }
}