using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Agebull.EntityModel.Redis
{
    /// <summary>
    /// REDIS������
    /// </summary>
    [Obsolete]
    public class RedisProxyAsync : RedisProxy<IRedisAsync>
    {
        #region ����

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="db"></param>
        public RedisProxyAsync(int db = 0) : base(db)
        {
        }
        #endregion

        #region �ı���д

        /// <summary>
        /// ɾ��KEY
        /// </summary>
        /// <param name="key"></param>
        public Task RemoveKey(string key)
        {
            return Redis.RemoveKey(key);
        }

        /// <summary>
        /// ���ҹ�ɾ��KEY
        /// </summary>
        /// <param name="pattern">����</param>
        public Task FindAndRemoveKey(string pattern)
        {
            return Redis.FindAndRemoveKey(pattern);
        }

        /// <summary>
        /// ȡ�ı�
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<string> Get(string key)
        {
            return Redis.Get(key);
        }


        /// <summary>
        /// дֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task Set(string key, string value)
        {
            return Redis.Set(key, value);
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
            return Redis.Set(key, value, last);
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
            return Redis.Set(key, value, span);
        }

        #endregion

        #region ֵ��д

        /// <summary>
        /// ȡֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<T> GetValue<T>(string key)
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
        public Task SetValue<T>(string key, T value)
            where T : struct
        {
            return Redis.SetValue(key, value);
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
            return Redis.SetValue(key, value, last);
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
            return Redis.SetValue(key, value, span);
        }

        #endregion

        #region �����д


        /// <summary>
        /// ���ҹ�ɾ��KEY
        /// </summary>
        /// <param name="pattern">����</param>
        public Task<List<T>> GetAll<T>(string pattern)
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
        public Task<T> Get<T>(string key)
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
        public Task<T> TryGet<T>(string key)
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
        public Task Set<T>(string key, T value)
            where T : class
        {
            return Redis.Set(key, value);
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
            return Redis.Set(key, value, last);
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
            return Redis.Set(key, value, span);
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
        public Task<TData> GetEntity<TData>(long id, TData def = null) where TData : class, new()
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
        public Task<TData> GetEntity<TData>(string key, TData def = null) where TData : class, new()
        {
            return Redis.GetEntity(key, def);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data">�������</param>
        /// <returns></returns>
        public Task SetEntity<TData>(TData data) where TData : class, IIdentityData
        {
            return Redis.SetEntity(data);
        }

        /// <summary>
        /// ������Щ����
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="datas">����</param>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns></returns>
        public Task CacheData<TData>(IEnumerable<TData> datas, Func<TData, string> keyFunc = null) where TData : class, IIdentityData, new()
        {
            return Redis.CacheData(datas, keyFunc);
        }

        /// <summary>
        /// ������Щ����
        /// </summary>
        /// <typeparam name="TDataAccess"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns></returns>
        public Task CacheData<TData, TDataAccess>(Func<TData, string> keyFunc = null)
            where TDataAccess : IDataTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var access = new TDataAccess();
            var datas = access.All();
            return CacheData(datas, keyFunc);
        }

        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns>����</returns>
        public async Task CacheData<TData, TDataAccess>(Expression<Func<TData, bool>> lambda, Func<TData, string> keyFunc = null)
            where TDataAccess : IDataTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var access = new TDataAccess();
            var datas = await access.AllAsync();
            await CacheData(datas, keyFunc);
        }

        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <returns>����</returns>
        public Task ClearCache<TData>()
        {
            return FindAndRemoveKey(DataKeyBuilder.DataKey<TData>("*"));
        }

        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <returns>����</returns>
        public async Task RefreshCache<TData, TDataAccess>(long id, Func<TData, bool> lambda = null)
            where TDataAccess : IDataTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            if (id <= 0)
                return;
            var access = new TDataAccess();
            var data = await access.LoadByPrimaryKeyAsync(id);
            if (data != null && (lambda == null || lambda(data)))
                await Set(DataKeyBuilder.DataKey(data), data);
            else
                await Redis.RemoveKey(DataKeyBuilder.DataKey<TData>(id));
        }

        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <returns>����</returns>
        public Task RemoveCache<TData>(long id)
        {
            return Redis.RemoveCache<TData>(id);
        }

        #endregion

    }
}