using System;
using System.Collections.Generic;
using Agebull.Common.Configuration;
using System.Threading;
using CSRedis;
using Gboxt.Common.DataModel;
using Newtonsoft.Json;

namespace Agebull.Common.DataModel.Redis
{
    /// <summary>
    /// REDIS������
    /// </summary>
    public class CSRedisEx : IDisposable, IRedis
    {
        #region ����

        /// <summary>
        /// ���ر�
        /// </summary>
        bool IRedis.NoClose => false;

        /// <summary>
        /// ��̬����
        /// </summary>
        static CSRedisEx()
        {
            var c = ConfigurationManager.ConnectionStrings["Redis"].Split(',', ':');
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

        private static ConnectionPool _connectionPool;
        private RedisConnection2 _connection;
        /// <summary>
        /// ������
        /// </summary>
        private static readonly object LockObj = new object();

        /// <summary>
        /// ʹ���ĸ����ݿ�
        /// </summary>
        private long _db;

        /// <summary>
        /// ��ǰ���ݿ�
        /// </summary>
        public long CurrentDb => _db;

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="db"></param>
        public CSRedisEx(long db = 0)
        {
            _db = db;
        }

        /// <summary>
        /// ʹ���е�
        /// </summary>
        private readonly Dictionary<long, RedisConnection2> Used = new Dictionary<long, RedisConnection2>();

        /// <summary>
        /// �ͻ�����
        /// </summary>
        internal RedisClient _client;
        /// <summary>
        /// �õ�һ�����õ�Redis�ͻ���
        /// </summary>
        public RedisClient Client
        {
            get
            {
                if (_client != null)
                    return _client;
                return _client = CreateClient(_db);
            }
        }

        private string ConnectString(long db) =>
            string.IsNullOrEmpty(PassWord)
                ? $"{Address}:{Port},defaultDatabase={db},poolsize=50,ssl=false,writeBuffer=10240"
                : $"{Address}:{Port},password={PassWord},defaultDatabase={db},poolsize=50,ssl=false,writeBuffer=10240";

        private RedisClient CreateClient(long db)
        {
            Monitor.Enter(LockObj);
            try
            {
                _db = db;
                if (Used.TryGetValue(db, out _connection))
                    return _connection.Client;
                _connectionPool = new ConnectionPool
                {
                    ConnectionString = ConnectString(db)
                };
                _connection = _connectionPool.GetConnection();
                Used.Add(db, _connection);
                return _connection.Client;
            }
            finally
            {
                Monitor.Exit(LockObj);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Monitor.Enter(LockObj);
            try
            {
                foreach (var con in Used.Values)
                    con.Dispose();
                Used.Clear();
            }
            finally
            {
                Monitor.Exit(LockObj);
            }
        }


        #endregion

        #region �ı���д

        /// <summary>
        /// ɾ��KEY
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(string key)
        {
            Client.Del(key);
        }

        /// <summary>
        /// ���ҹ�ɾ��KEY
        /// </summary>
        /// <param name="pattern">����</param>
        public void FindAndRemoveKey(string pattern)
        {
            var keys = Client.Keys(pattern);
            Client.Del(keys);
        }

        /// <summary>
        /// ȡ�ı�
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return Client.Get(key);
        }


        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Set(string key, string value)
        {
            Client.Set(key, value);
        }

        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public void Set(string key, string value, DateTime last)
        {
            Set(key, value, DateTime.Now - last);
        }

        /// <summary>
        /// дֵ
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

        #region ֵ��д

        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public void SetTime(string key, DateTime last)
        {
            Client.Expire(key, DateTime.Now - last);
        }

        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public void SetTime(string key, TimeSpan span)
        {
            Client.Expire(key, span);
        }
        /// <summary>
        /// ȡֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetValue<T>(string key)
            where T : struct
        {
            var str = Client.Get(key);
            return string.IsNullOrEmpty(str) ? default(T) : JsonConvert.DeserializeObject<T>(str);
        }
        /// <summary>
        /// дֵ
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
        /// дֵ
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
        /// дֵ
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

        #region �����д


        /// <summary>
        /// ���ҹ�ɾ��KEY
        /// </summary>
        /// <param name="pattern">����</param>
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
        /// ȡֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <remarks>
        /// ��������л�ʧ�ܻ��׳��쳣
        /// </remarks>
        public T Get<T>(string key)
            where T : class
        {
            var str = Client.Get(key);
            if (string.IsNullOrEmpty(str))
                return default(T);
            return JsonConvert.DeserializeObject<T>(str);
        }
        /// <summary>
        /// ��ͼȡֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>��������л�ʧ��ʧ�ܻ�ûؿ�</returns>
        public T TryGet<T>(string key)
            where T : class
        {
            var str = Client.Get(key);
            if (string.IsNullOrEmpty(str))
                return default(T);
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
        public void Set<T>(string key, T value)
            where T : class
        {
            Client.Set(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public void Set<T>(string key, T value, DateTime last)
            where T : class
        {
            Client.Set(key, JsonConvert.SerializeObject(value),DateTime.Now- last);
        }

        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public void Set<T>(string key, T value, TimeSpan span)
            where T : class
        {
            Client.Set(key, JsonConvert.SerializeObject(value), span);
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
        public TData GetEntity<TData>(long id, TData def = null) where TData : class, new()
        {
            return id == 0 ? def ?? new TData() : Get<TData>(DataKeyBuilder.DataKey<TData>(id)) ?? def ?? new TData();
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="key">���ݼ�</param>
        /// <param name="def">Ĭ��ֵ</param>
        /// <returns></returns>
        public TData GetEntity<TData>(string key, TData def = null) where TData : class, new()
        {
            return Get<TData>(key) ?? def ?? new TData();
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data">�������</param>
        /// <returns></returns>
        public void SetEntity<TData>(TData data) where TData : class, IIdentityData
        {
            Set(DataKeyBuilder.DataKey(data), data);
        }

        /// <summary>
        /// ������Щ����
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="datas">����</param>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns></returns>
        public void CacheData<TData>(IEnumerable<TData> datas, Func<TData, string> keyFunc = null) where TData : class, IIdentityData, new()
        {
            if (keyFunc == null)
                keyFunc = DataKeyBuilder.DataKey;
            var key = keyFunc(new TData());
            Client.Set(key, DateTime.Now);
            foreach (var data in datas)
            {
                Set(keyFunc(data), data);
            }
        }

        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <returns>����</returns>
        public void RemoveCache<TData>(long id)
        {
            if (id > 0)
                Client.Del(DataKeyBuilder.DataKey<TData>(id));
        }

        /// <summary>
        /// ԭʼ����,�ڲ�����ʱ��չ
        /// </summary>
        public T Original<T>() where T : class
        {
            return Client as T;
        }

        void IRedis.ChangeDb(int db)
        {
            _client = CreateClient(db);
        }

        bool IRedis.SetNx(string key, byte[] value)
        {
            return Client.SetNx(key, value);
        }

        long IRedis.Incr(string key)
        {
            return Client.Incr(key);
        }

        long IRedis.Decr(string key)
        {
            return Client.Decr(key);
        }

        #endregion

    }
}