using System;
using System.Collections.Generic;
using Agebull.Common.DataModel;

namespace Agebull.Common.DataModel.Redis
{
    /// <summary>
    /// Redis操作接口
    /// </summary>
    public interface IRedis
    {
        /// <summary>
        /// 当前数据库
        /// </summary>
        long CurrentDb { get; }

        /// <summary>
        /// 不关闭
        /// </summary>
        bool NoClose { get; }

        /// <summary>
        /// 原始对象,在不够用时扩展
        /// </summary>
        T Original<T>() where T : class;

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        void SetTime(string key, DateTime last);

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        void SetTime(string key, TimeSpan span);

        /// <summary>
        /// 更改Db
        /// </summary>
        void ChangeDb(int db);

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        void Dispose();

        /// <summary>
        /// 删除KEY
        /// </summary>
        /// <param name="key"></param>
        void RemoveKey(string key);

        /// <summary>
        /// 查找关删除KEY
        /// </summary>
        /// <param name="pattern">条件</param>
        void FindAndRemoveKey(string pattern);

        /// <summary>
        /// 查找关删除KEY
        /// </summary>
        /// <param name="pattern">条件</param>
        List<T> GetAll<T>(string pattern) where T : class;

        /// <summary>
        /// 取文本
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key);

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void Set(string key, string value);

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        void Set(string key, string value, DateTime last);

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        void Set(string key, string value, TimeSpan span);

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetValue<T>(string key)
            where T : struct;

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetValue<T>(string key, T value)
            where T : struct;

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        void SetValue<T>(string key, T value, DateTime last)
            where T : struct;

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        void SetValue<T>(string key, T value, TimeSpan span)
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
        T Get<T>(string key)
            where T : class;

        /// <summary>
        /// 试图取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>如果反序列化失败失败会访回空</returns>
        T TryGet<T>(string key)
            where T : class;

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void Set<T>(string key, T value)
            where T : class;

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        void Set<T>(string key, T value, DateTime last)
            where T : class;

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        void Set<T>(string key, T value, TimeSpan span)
            where T : class;

        /// <summary>
        /// 读缓存
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="id">键的组合</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        TData GetEntity<TData>(long id, TData def = null) where TData : class, new();

        /// <summary>
        /// 读缓存
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="key">数据键</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        TData GetEntity<TData>(string key, TData def = null) where TData : class, new();

        /// <summary>
        /// 缓存数据
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data">键的组合</param>
        /// <returns></returns>
        void SetEntity<TData>(TData data) where TData : class, IIdentityData;

        /// <summary>
        /// 缓存这些对象
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="datas">数据</param>
        /// <param name="keyFunc">设置键的方法,可为空</param>
        /// <returns></returns>
        void CacheData<TData>(IEnumerable<TData> datas, Func<TData, string> keyFunc = null) where TData : class, IIdentityData, new();

        /// <summary>
        ///     缓存这些对象
        /// </summary>
        /// <returns>数据</returns>
        void RemoveCache<TData>(long id);

        /// <summary>
        /// 如果值不存在，则写入，否则失败
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool SetNx(string key, byte[] value = null);

        /// <summary>
        /// 取得一个自增值
        /// </summary>
        /// <param name="key"></param>
        /// <returns>0表示失败，其它数字成功</returns>
        long Incr(string key);

        /// <summary>
        /// 取得一个自减值
        /// </summary>
        /// <param name="key"></param>
        /// <returns>0表示失败，其它数字成功</returns>
        long Decr(string key);
    }
}