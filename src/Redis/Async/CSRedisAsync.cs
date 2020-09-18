using Agebull.EntityModel.Common;
using Agebull.EntityModel.Redis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable 693

namespace Agebull.Common.DataModel.Redis
{
    /// <summary>
    /// REDIS������
    /// </summary>
    [Obsolete]
    public class CSRedisAsync : CSRedisBase, IDisposable, IRedisAsync
    {
        #region �ı���д

        /// <summary>
        /// ɾ��KEY
        /// </summary>
        /// <param name="key"></param>
        public Task RemoveKey(string key)
        {
            return Client.DelAsync(key);
        }

        /// <summary>
        /// ���ҹ�ɾ��KEY
        /// </summary>
        /// <param name="pattern">����</param>
        public async Task<long> FindAndRemoveKey(string pattern)
        {
            long cnt = 0;
            long cursor = 0;
            do
            {
                var result = await Client.ScanAsync(cursor, pattern, 100);
                cursor = result.Cursor;
                cnt += await Client.DelAsync(result.Items);
            } while (cursor > 0);

            return cnt;
        }

        /// <summary>
        /// ȡ�ı�
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<string> Get(string key)
        {
            return Client.GetAsync(key);
        }


        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task Set(string key, string value)
        {
            return Client.SetAsync(key, value);
        }

        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public Task Set(string key, string value, DateTime last)
        {
            return Client.SetAsync(key, value, (int)(last - DateTime.Now).TotalSeconds);
        }

        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public Task Set(string key, string value, TimeSpan span)
        {
            return Client.SetAsync(key, value, (int)span.TotalSeconds);
        }

        #endregion

        #region ֵ��д

        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public Task SetTime(string key, DateTime last)
        {
            return Client.ExpireAsync(key, last - DateTime.Now);
        }

        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public Task SetTime(string key, TimeSpan span)
        {
            return Client.ExpireAsync(key, span);
        }

        /// <summary>
        /// ȡֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetValue<T>(string key)
            where T : struct
        {
            var str = await Client.GetAsync(key);
            return string.IsNullOrEmpty(str) ? default : JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task SetValue<T>(string key, T value)
            where T : struct
        {
            return Client.SetAsync(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public Task SetValue<T>(string key, T value, DateTime last)
            where T : struct
        {
            return SetValue(key, value, last - DateTime.Now);
        }

        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public Task SetValue<T>(string key, T value, TimeSpan span)
            where T : struct
        {
            return Client.SetAsync(key, JsonConvert.SerializeObject(value), (int)span.TotalSeconds);
        }

        #endregion

        #region �����д

        /// <summary>
        /// ���ҹ�ɾ��KEY
        /// </summary>
        /// <param name="pattern">����</param>
        public async Task<List<T>> GetAll<T>(string pattern)
            where T : class
        {
            var lists = new List<T>();
            long cursor = 0;
            do
            {
                var result = await Client.ScanAsync(cursor, pattern, 100);
                cursor = result.Cursor;
                foreach (var key in result.Items)
                {
                    lists.Add(await Get<T>(key));
                }
            } while (cursor > 0);

            return lists;
        }

        /// <summary>
        /// ȡֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <remarks>
        /// ��������л�ʧ�ܻ��׳��쳣
        /// </remarks>
        public async Task<T> Get<T>(string key)
            where T : class
        {
            var str = await Client.GetAsync(key);
            return string.IsNullOrEmpty(str) ? null : JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// ��ͼȡֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>��������л�ʧ��ʧ�ܻ�ûؿ�</returns>
        public async Task<T> TryGet<T>(string key)
            where T : class
        {
            var str = await Client.GetAsync(key);
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
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task Set<T>(string key, T value)
            where T : class
        {
            return Client.SetAsync(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public Task Set<T>(string key, T value, DateTime last)
            where T : class
        {
            return Client.SetAsync(key, JsonConvert.SerializeObject(value), (int)(last - DateTime.Now).TotalSeconds);
        }

        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public Task Set<T>(string key, T value, TimeSpan span)
            where T : class
        {
            return Client.SetAsync(key, JsonConvert.SerializeObject(value), (int)span.TotalSeconds);
        }

        #endregion

        #region ʵ�建��֧��

        /// <summary>
        /// ������
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="id">�������</param>
        /// <param name="def">Ĭ��ֵ</param>
        /// <returns></returns>
        public async Task<TData> GetEntity<TData>(long id, TData def = null) where TData : class, new()
        {
            return id == 0
                ? def ?? new TData()
                : (await Get<TData>(DataKeyBuilder.DataKey<TData>(id))) ?? def ?? new TData();
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="key">���ݼ�</param>
        /// <param name="def">Ĭ��ֵ</param>
        /// <returns></returns>
        public async Task<TData> GetEntity<TData>(string key, TData def = null) where TData : class, new()
        {
            return await Get<TData>(key) ?? def ?? new TData();
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data">�������</param>
        /// <returns></returns>
        public Task SetEntity<TData>(TData data) where TData : class, IIdentityData
        {
            return Set(DataKeyBuilder.DataKey(data), data);
        }

        /// <summary>
        /// ������Щ����
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="datas">����</param>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns></returns>
        public async Task CacheData<TData>(IEnumerable<TData> datas, Func<TData, string> keyFunc = null)
            where TData : class, IIdentityData, new()
        {
            if (keyFunc == null)
                keyFunc = DataKeyBuilder.DataKey;
            foreach (var data in datas)
            {
                await Set(keyFunc(data), data);
            }
        }

        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <returns>����</returns>
        public Task RemoveCache<TData>(long id)
        {
            return id > 0 ? Client.DelAsync(DataKeyBuilder.DataKey<TData>(id)) : Task.CompletedTask;
        }

        Task<bool> IRedisAsync.SetNx(string key, byte[] value)
        {
            return Client.SetNxAsync(key, value);
        }

        Task<long> IRedisAsync.Incr(string key)
        {
            return Client.IncrByAsync(key);
        }

        #endregion
    }
}