using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Agebull.EntityModel.Redis
{
    /// <summary>
    /// REDIS������
    /// </summary>
    [Obsolete]
    public class RedisProxy : RedisProxy<IRedis>
    {
        #region ����

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="db"></param>
        public RedisProxy(int db = 0) : base(db)
        {
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
            return Redis.Get(key);
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
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns></returns>
        public void CacheData<TData, TDataAccess>(Func<TData, string> keyFunc = null)
            where TDataAccess : IDataTable<TData>, new()
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
            where TDataAccess : IDataTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var key = DataKeyBuilder.DataKey<TData>(0);
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
            where TDataAccess : IDataTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var key = DataKeyBuilder.DataKey<TData>(0);
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
            where TDataAccess : IDataTable<TData>, new()
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
        public void ClearCache<TData>()
        {
            FindAndRemoveKey(DataKeyBuilder.DataKey<TData>("*"));
        }

        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <returns>����</returns>
        public void RefreshCache<TData, TDataAccess>(long id, Func<TData, bool> lambda = null)
            where TDataAccess : IDataTable<TData>, new()
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