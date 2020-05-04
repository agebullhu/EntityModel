using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agebull.EntityModel.Redis
{
    /// <summary>
    /// Redis操作接口
    /// </summary>
    public interface IRedisAsync : IRedisBase
    {
        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        Task SetTime(string key, DateTime last);

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        Task SetTime(string key, TimeSpan span);

        /// <summary>
        /// 删除KEY
        /// </summary>
        /// <param name="key"></param>
        Task RemoveKey(string key);

        /// <summary>
        /// 查找关删除KEY
        /// </summary>
        /// <param name="pattern">条件</param>
        Task<long> FindAndRemoveKey(string pattern);

        /// <summary>
        /// 查找关删除KEY
        /// </summary>
        /// <param name="pattern">条件</param>
        Task<List<T>> GetAll<T>(string pattern) where T : class;

        /// <summary>
        /// 取文本
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> Get(string key);

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task Set(string key, string value);

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        Task Set(string key, string value, DateTime last);

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        Task Set(string key, string value, TimeSpan span);

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetValue<T>(string key)
            where T : struct;

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetValue<T>(string key, T value)
            where T : struct;

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        Task SetValue<T>(string key, T value, DateTime last)
            where T : struct;

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        Task SetValue<T>(string key, T value, TimeSpan span)
            where T : struct;

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <remarks>
        /// 如果反序列化失败会抛出异常
        /// </remarks>
        Task<T> Get<T>(string key)
            where T : class;

        /// <summary>
        /// 试图取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>如果反序列化失败失败会访回空</returns>
        Task<T> TryGet<T>(string key)
            where T : class;

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task Set<T>(string key, T value)
            where T : class;

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        Task Set<T>(string key, T value, DateTime last)
            where T : class;

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        Task Set<T>(string key, T value, TimeSpan span)
            where T : class;

        /// <summary>
        /// 读缓存
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="id">键的组合</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        Task<TData> GetEntity<TData>(long id, TData def = null) where TData : class, new();

        /// <summary>
        /// 读缓存
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="key">数据键</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        Task<TData> GetEntity<TData>(string key, TData def = null) where TData : class, new();

        /// <summary>
        /// 缓存数据
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data">键的组合</param>
        /// <returns></returns>
        Task SetEntity<TData>(TData data) where TData : class, IIdentityData;

        /// <summary>
        /// 缓存这些对象
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="datas">数据</param>
        /// <param name="keyFunc">设置键的方法,可为空</param>
        /// <returns></returns>
        Task CacheData<TData>(IEnumerable<TData> datas, Func<TData, string> keyFunc = null) where TData : class, IIdentityData, new();

        /// <summary>
        ///     缓存这些对象
        /// </summary>
        /// <returns>数据</returns>
        Task RemoveCache<TData>(long id);

        /// <summary>
        /// 如果值不存在，则写入，否则失败
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> SetNx(string key, byte[] value = null);

        /// <summary>
        /// 取得一个自增值
        /// </summary>
        /// <param name="key"></param>
        /// <returns>0表示失败，其它数字成功</returns>
        Task<long> Incr(string key);
    }
}