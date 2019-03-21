using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;
using Gboxt.Common.DataModel;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Agebull.Common.DataModel.Redis
{
    /// <summary>
    /// REDIS������
    /// </summary>
    public class RedisProxy : IDisposable
    {
        #region ����

        /// <summary>
        /// ��̬����
        /// </summary>
        static RedisProxy()
        {
            var c = ConfigurationManager.AppSettings["RedisConnectionString"].Split(',', ':');
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

        /// <summary>
        /// ϵͳ����
        /// </summary>
        public const int DbSystem = 15;

        /// <summary>
        /// ����������
        /// </summary>
        public const int DbContext = 11;

        /// <summary>
        /// WEB�˵Ļ���
        /// </summary>
        public const int DbWebCache = 14;

        /// <summary>
        /// Ȩ������
        /// </summary>
        public const int DbAuthority = 13;

        /// <summary>
        /// WEB�˵Ļ���
        /// </summary>
        public const int DbComboCache = 12;


        #endregion

        #region ����֧��
        /// <summary>
        /// ����֧��
        /// </summary>
        public static bool IsTest { get; set; }

        #endregion

        #region ����


        /// <summary>
        /// ������
        /// </summary>
        private static readonly object LockObj = new object();
        /// <summary>
        /// ʹ���ĸ����ݿ�
        /// </summary>
        private readonly int _db;

        /// <summary>
        /// ��ǰ���ݿ�
        /// </summary>
        public long CurrentDb => _db;

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="db"></param>
        public RedisProxy(int db = 0)
        {
            _db = db;
        }

        /*// <summary>
        /// ʹ���е�
        /// </summary>
        private static readonly Dictionary<int, List<RedisClient>> Used = new Dictionary<int, List<RedisClient>>();
        /// <summary>
        /// ���е�
        /// </summary>
        private static readonly Dictionary<int, List<RedisClient>> Idle = new Dictionary<int, List<RedisClient>>();*/
        private ConnectionMultiplexer connect;
        /// <summary>
        /// �ͻ�����
        /// </summary>
        internal IDatabase _client;
        /// <summary>
        /// �õ�һ�����õ�Redis�ͻ���
        /// </summary>
        public IDatabase Client
        {
            get
            {
                if (_client != null)
                    return _client;
                return _client = CreateClient(_db);
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        internal IDatabase ChangeDb(int db)
        {
            return CreateClient(db);
        }
        private IDatabase CreateClient(int db)
        {
            Monitor.Enter(LockObj);
            try
            {
                if (connect == null)
                {
                    connect = ConnectionMultiplexer.Connect($"{Address}:{Port}");
                }
                if (_client != null && _db == db)
                    return _client;
                return connect.GetDatabase(db);
            }
            finally
            {
                Monitor.Exit(LockObj);
            }
        }


        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            if (connect == null)
                return;
            Monitor.Enter(LockObj);
            try
            {
                connect.Close();
                connect.Dispose();
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
            Client.KeyDelete(key);
        }


        /// <summary>
        /// ȡ�ı�
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return Client.StringGet(key);
        }


        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Set(string key, string value)
        {
            Client.StringSet(key, value);
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
            Client.StringSet(key, value, last - DateTime.Now);
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
            Client.StringSet(key, value, span);
        }

        #endregion

        #region ֵ��д

        /// <summary>
        /// ȡֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetValue<T>(string key)
            where T : struct
        {
            string vl = Client.StringGet(key);
            return string.IsNullOrWhiteSpace(vl) ? default(T) : JsonConvert.DeserializeObject<T>(vl);
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
            string json = JsonConvert.SerializeObject(value);
            Client.StringSet(key, json);
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
            string json = JsonConvert.SerializeObject(value);
            Client.StringSet(key, json, last - DateTime.Now);
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
            string json = JsonConvert.SerializeObject(value);
            Client.StringSet(key, json, span);
        }

        #endregion

        #region �����д

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
            var bytes = Client.StringGet(key);
            if (bytes.IsNullOrEmpty)
                return default(T);
            return JsonConvert.DeserializeObject<T>(bytes);
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
            var bytes = Client.StringGet(key);
            if (bytes.IsNullOrEmpty)
                return default(T);
            try
            {
                return JsonConvert.DeserializeObject<T>(bytes);
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
            if (IsTest)
            {
                Trace.WriteLine(JsonConvert.SerializeObject(value, Formatting.Indented), key);
            }
            Client.StringSet(key, JsonConvert.SerializeObject(value));
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
            if (IsTest)
            {
                Trace.WriteLine(JsonConvert.SerializeObject(value, Formatting.Indented), key);
            }
            Client.StringSet(key, JsonConvert.SerializeObject(value), last - DateTime.Now);
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
            if (IsTest)
            {
                Trace.WriteLine(JsonConvert.SerializeObject(value, Formatting.Indented), key);
            }
            Client.StringSet(key, JsonConvert.SerializeObject(value), span);
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
        public TData GetEntity<TData>(int id, TData def = null) where TData : class, new()
        {
            return id == 0 ? def ?? new TData() : Get<TData>(DataKey<TData>(id)) ?? def ?? new TData();
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
            Set(DataKey(data), data);
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
                keyFunc = DataKey;
            var key = keyFunc(new TData());
            SetValue(key, DateTime.Now);
            foreach (var data in datas)
            {
                Set(keyFunc(data), data);
            }
        }
        /*
        /// <summary>
        /// ������Щ����
        /// </summary>
        /// <typeparam name="TDataAccess"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns></returns>
        public void CacheData<TData, TDataAccess>(Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var access = new TDataAccess();
            var datas = access.All();
            CacheData(datas, keyFunc);
        }
        /// <summary>
        /// ������Щ����
        /// </summary>
        /// <typeparam name="TDataAccess"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns></returns>
        public void TryCacheData<TData, TDataAccess>(Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var key = DataKey<TData>(0);
            var date = GetValue<DateTime>(key);
            if (date == DateTime.MinValue)
                CacheData<TData, TDataAccess>(keyFunc);
        }

        /// <summary>
        ///     ���û�л������Щ����,�ͻ�����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns>����</returns>
        public void TryCacheData<TData, TDataAccess>(Expression<Func<TData, bool>> lambda, Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var key = DataKey<TData>(0);
            var date = GetValue<DateTime>(key);
            if (date == DateTime.MinValue)
                CacheData<TData, TDataAccess>(lambda, keyFunc);
        }

        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns>����</returns>
        public void CacheData<TData, TDataAccess>(Expression<Func<TData, bool>> lambda, Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var access = new TDataAccess();
            var datas = access.All(lambda);
            CacheData(datas, keyFunc);
        }
        
        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <returns>����</returns>
        public void RefreshCache<TData, TDataAccess>(int id, Func<TData, bool> lambda = null)
            where TDataAccess : MySqlTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            if (id <= 0)
                return;
            var access = new TDataAccess();
            var data = access.LoadByPrimaryKey(id);
            if (data != null && (lambda == null || lambda(data)))
                Set(DataKey(data), data);
            else
                Client.KeyDelete(DataKey<TData>(id));
        }
        
             */

        /// <summary>
        /// Ĭ�ϵ����ݼ�����������
        /// </summary>
        /// <typeparam name="TData">���ݼ�</typeparam>
        /// <param name="data">����</param>
        /// <returns>���ݼ���</returns>
        public static string DataKey<TData>(TData data) where TData : class, IIdentityData
        {
            return $"data:{typeof(TData).Name.ToLower()}:{data.Id}";
        }

        /// <summary>
        /// Ĭ�ϵ����ݼ�����������
        /// </summary>
        /// <typeparam name="TData">���ݼ�</typeparam>
        /// <param name="id">���ݼ�</param>
        /// <returns>���ݼ���</returns>
        public static string DataKey<TData>(object id)
        {
            return $"data:{typeof(TData).Name.ToLower()}:{id}";
        }
        /// <summary>
        /// Ĭ�ϵ����ݼ�����������
        /// </summary>
        /// <param name="type">����</param>
        /// <param name="id">���ݼ�</param>
        /// <returns>���ݼ���</returns>
        public static string DataKey(string type, int id)
        {
            return $"data:{type.ToLower()}:{id}";
        }

        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <returns>����</returns>
        public void RemoveCache<TData>(int id)
        {
            if (id > 0)
                Client.KeyDelete(DataKey<TData>(id));
        }

        #endregion
        

        #region ������չ
        /// <summary>
        /// ���ֵ�����ڣ���д�룬����ʧ��
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetNx(string key, byte[] value = null)
        {
            var re = Client.Execute("SetNx", key, value ?? new byte[0]);
            return !re.IsNull && (int)re == 1;
        }

        /// <summary>
        /// ȡ��һ������ֵ
        /// </summary>
        /// <param name="key"></param>
        /// <returns>0��ʾʧ�ܣ��������ֳɹ�</returns>
        public long Incr(string key)
        {
            var re = Client.Execute("Incr", key);
            return re.IsNull ? 0 : (long)re;
        }

        /// <summary>
        /// ȡ��һ���Լ�ֵ
        /// </summary>
        /// <param name="key"></param>
        /// <returns>0��ʾʧ�ܣ��������ֳɹ�</returns>
        public long Decr(string key)
        {
            var re = Client.Execute("Decr", key);
            return re.IsNull ? 0 : (long)re;
        }

        #endregion
    }
}