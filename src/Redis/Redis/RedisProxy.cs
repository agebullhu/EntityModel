using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Agebull.Common.Ioc;
using Agebull.Common.DataModel;
using Agebull.Common.DataModel.MySql;

namespace Agebull.Common.DataModel.Redis
{
    /// <summary>
    /// REDIS������
    /// </summary>
    public class RedisProxy : IDisposable
    {
        #region ����

        /// <summary>
        /// ϵͳ����
        /// </summary>
        public const int DbSystem = 15;

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
        /// ʹ���ĸ����ݿ�
        /// </summary>
        private int _db;

        /// <summary>
        /// ��ǰ���ݿ�
        /// </summary>
        public long CurrentDb => Redis.CurrentDb;

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="db"></param>
        public RedisProxy(int db = 0)
        {
            _db = db;
            ChangeDb();
        }

        /*// <summary>
        /// ʹ���е�
        /// </summary>
        private static readonly Dictionary<int, List<IRedis>> Used = new Dictionary<int, List<IRedis>>();
        /// <summary>
        /// ���е�
        /// </summary>
        private static readonly Dictionary<int, List<IRedis>> Idle = new Dictionary<int, List<IRedis>>();*/
        /// <summary>
        /// �ͻ�����
        /// </summary>
        internal IRedis _redis;
        /// <summary>
        /// �õ�һ�����õ�Redis�ͻ���
        /// </summary>
        public IRedis Redis
        {
            get
            {
                if (_redis != null)
                    return _redis;
                return _redis = CreateClient();
            }
        }
        
        /// <summary>
        /// ����
        /// </summary>
        internal IRedis ChangeDb(int db)
        {
            _db = db;
            return ChangeDb();
        }

        /// <summary>
        /// ����
        /// </summary>
        private IRedis ChangeDb()
        {
            if (_redis == null)
            {
                var old = _redis;
                _redis = CreateClient();
                return old;
            }
            if (_db == _redis.CurrentDb)
            {
                return _redis;
            }
            _redis.ChangeDb(_db);
            return _redis;
        }

        /// <summary>
        /// ����
        /// </summary>
        internal void ResetClient(IRedis client)
        {
            _redis = client;
        }

        private IRedis CreateClient()
        {
            _redis= IocHelper.Create<IRedis>();
            if (_redis == null)
                throw new Exception("δ��ȷע��Redis����");
            ChangeDb();
            return _redis;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (_redis == null || _redis.NoClose )
                return;
            _redis.Dispose();
            _redis = null;
        }


        #endregion

        #region �ı���д

        /// <summary>
        /// ɾ��KEY
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(string key)
        {
            Redis.RemoveKey(key);
        }

        /// <summary>
        /// ���ҹ�ɾ��KEY
        /// </summary>
        /// <param name="pattern">����</param>
        public void FindAndRemoveKey(string pattern)
        {
            Redis.FindAndRemoveKey(pattern);
        }

        /// <summary>
        /// ȡ�ı�
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return Redis.Get<string>(key);
        }


        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Set(string key, string value)
        {
            Redis.Set(key, value);
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
            Redis.Set(key, value, last);
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
            Redis.Set(key, value, span);
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
            return Redis.GetValue<T>(key);
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
            Redis.SetValue(key, value);
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
            Redis.SetValue(key, value, last);
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
            Redis.SetValue(key, value, span);
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
            return Redis.GetAll<T>(pattern);
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
            return Redis.Get<T>(key);
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
            return Redis.TryGet<T>(key);
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
            Redis.Set(key, value);
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
            Redis.Set(key, value, last);
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
            Redis.Set(key, value, span);
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
           return Redis.GetEntity(id, def);
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
            Redis.SetEntity(data);
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
            Redis.CacheData(datas, keyFunc);
        }

        /// <summary>
        /// ������Щ����
        /// </summary>
        /// <typeparam name="TDataAccess"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TDatabase"></typeparam>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns></returns>
        public void CacheData<TData, TDataAccess, TDatabase>(Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData, TDatabase>, new()
            where TData : EditDataObject, IIdentityData, new()
            where TDatabase : MySqlDataBase
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
        /// <typeparam name="TDatabase"></typeparam>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns></returns>
        public void TryCacheData<TData, TDataAccess, TDatabase>(Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData, TDatabase>, new()
            where TData : EditDataObject, IIdentityData, new()
            where TDatabase : MySqlDataBase
        {
            var key = DataKeyBuilder.DataKey<TData>(0);
            var date = GetValue<DateTime>(key);
            if (date == DateTime.MinValue)
                CacheData<TData, TDataAccess, TDatabase>(keyFunc);
        }

        /// <summary>
        ///     ���û�л������Щ����,�ͻ�����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns>����</returns>
        public void TryCacheData<TData, TDataAccess, TDatabase>(Expression<Func<TData, bool>> lambda, Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData, TDatabase>, new()
            where TData : EditDataObject, IIdentityData, new()
            where TDatabase : MySqlDataBase
        {
            var key = DataKeyBuilder.DataKey<TData>(0);
            var date = GetValue<DateTime>(key);
            if (date == DateTime.MinValue)
                CacheData<TData, TDataAccess, TDatabase>(lambda, keyFunc);
        }

        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns>����</returns>
        public void CacheData<TData, TDataAccess, TDatabase>(Expression<Func<TData, bool>> lambda, Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData, TDatabase>, new()
            where TData : EditDataObject, IIdentityData, new()
            where TDatabase : MySqlDataBase
        {
            var access = new TDataAccess();
            var datas = access.All(lambda);
            CacheData(datas, keyFunc);
        }

        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <returns>����</returns>
        public void ClearCache<TData>()
        {
            FindAndRemoveKey(DataKeyBuilder.DataKey<TData>("*"));
        }

        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <returns>����</returns>
        public void RefreshCache<TData, TDataAccess>(long id, Func<TData, bool> lambda = null)
            where TDataAccess : MySqlTable<TData, MySqlDataBase>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            if (id <= 0)
                return;
            var access = new TDataAccess();
            var data = access.LoadByPrimaryKey(id);
            if (data != null && (lambda == null || lambda(data)))
                Set(DataKeyBuilder.DataKey(data), data);
            else
                Redis.RemoveKey(DataKeyBuilder.DataKey<TData>(id));
        }

        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <returns>����</returns>
        public void RemoveCache<TData>(long id)
        {
            Redis.RemoveCache<TData>(id);
        }

        #endregion

    }
}