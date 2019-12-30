using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agebull.Common.Configuration;
using Agebull.EntityModel.Redis;
using CSRedis;
using Newtonsoft.Json;
using Agebull.EntityModel.Common;

#pragma warning disable 693

namespace Agebull.Common.DataModel.Redis
{
    /// <summary>
    /// REDIS������
    /// </summary>
    public class CSRedisAsync : IDisposable, IRedisAsync
    {
        #region ����

        /// <summary>
        /// ���ر�
        /// </summary>
        bool IRedisAsync.NoClose => true;

        private static readonly bool IsFullConnectionStrings;

        /// <summary>
        /// ��̬����
        /// </summary>
        static CSRedisAsync()
        {
            connectionStrings = ConfigurationManager.ConnectionStrings["CSRedis"];
            IsFullConnectionStrings = !string.IsNullOrWhiteSpace(connectionStrings);
            if (IsFullConnectionStrings)
                return;
            connectionStrings = ConfigurationManager.ConnectionStrings["Redis"];
            var c = connectionStrings.Split(',', ':');
            Address = c[0];
            Port = c.Length > 1 ? int.Parse(c[1]) : 6379;
            //PoolSize = Convert.ToInt32(ConfigurationManager.AppSettings["RedisPoolSize"]);
            //if (PoolSize < 100)
            //    PoolSize = 100;
            if (c.Length > 2)
                PassWord = c[2];
        }

        /// <summary>
        /// ��ַ
        /// </summary>
        static readonly string connectionStrings;

        /// <summary>
        /// ��ַ
        /// </summary>
        static readonly string Address;

        /// <summary>
        /// ����
        /// </summary>
        static readonly string PassWord;

        /// <summary>
        /// �˿�
        /// </summary>
        public static readonly int Port;

        /*// <summary>
        /// ����������
        /// </summary>
        private static readonly int PoolSize;*/

        #endregion

        #region ����

        /// <summary>
        /// ��ǰ���ݿ�
        /// </summary>
        public long CurrentDb { get; }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="db"></param>
        public CSRedisAsync(long db = 0)
        {
            CurrentDb = db;
        }

        /// <summary>
        /// �ͻ�����
        /// </summary>
        private CSRedisClient _client;

        /// <summary>
        /// �õ�һ�����õ�Redis�ͻ���
        /// </summary>
        public CSRedisClient Client
        {
            get
            {
                if (_client != null)
                    return _client;
                return _client = CreateClient(CurrentDb);
            }
        }

        static string ConnectString(long db) =>
            IsFullConnectionStrings
                ? $"{connectionStrings},defaultDatabase={db}"
                : string.IsNullOrEmpty(PassWord)
                    ? $"{Address}:{Port},defaultDatabase={db},poolsize=50,ssl=false,writeBuffer=10240"
                    : $"{Address}:{Port},password={PassWord},defaultDatabase={db},poolsize=50,ssl=false,writeBuffer=10240";


        private static readonly ConcurrentDictionary<long, CSRedisClient> _clients =
            new ConcurrentDictionary<long, CSRedisClient>();

        CSRedisClient CreateClient(long db)
        {
            if (_clients.TryGetValue(db, out var client))
                return client;

            client = new CSRedisClient(ConnectString(db));
            _clients.TryAdd(db, client);
            return client;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            //foreach (var client in _clients.Values)
            //{
            //    client.Dispose();
            //}
        }

        #endregion

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
            return Client.SetAsync(key, value, (int) (last - DateTime.Now).TotalSeconds);
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
            return Client.SetAsync(key, value, (int) span.TotalSeconds);
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
            return Client.SetAsync(key, JsonConvert.SerializeObject(value), (int) span.TotalSeconds);
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
            return Client.SetAsync(key, JsonConvert.SerializeObject(value), (int) (last - DateTime.Now).TotalSeconds);
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
            return Client.SetAsync(key, JsonConvert.SerializeObject(value), (int) span.TotalSeconds);
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

        /// <summary>
        /// ԭʼ����,�ڲ�����ʱ��չ
        /// </summary>
        public T Original<T>() where T : class
        {
            return Client as T;
        }

        void IRedisAsync.ChangeDb(int db)
        {
            _client = CreateClient(db);
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