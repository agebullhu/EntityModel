using Agebull.Common.Configuration;
using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;

namespace Agebull.EntityModel.Redis
{
    /// <summary>
    /// REDIS������
    /// </summary>
    [Obsolete]
    public class RedisProxy<TRedis> : IDisposable
        where TRedis : IRedisBase
    {
        #region ����֧��
        /// <summary>
        /// ����֧��
        /// </summary>
        public static bool IsTest { get; set; }

        #endregion

        #region ����

        /// <summary>
        /// ����ʵ��
        /// </summary>
        private readonly static List<TRedis> Instances = new List<TRedis>();

        /// <summary>
        /// ����
        /// </summary>
        public static RedisOption Option { get; set; }

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        static RedisProxy()
        {
            ConfigurationManager.RegistOnChange("Redis", OnOptionChange, true);
        }

        /// <summary>
        /// ���ñ��
        /// </summary>
        private static void OnOptionChange()
        {
            Option = ConfigurationManager.Get<RedisOption>("Redis") ?? new RedisOption();
            foreach (var client in Instances)
                client.OnOptionChange(Option);
        }

        /// <summary>
        /// ���ر�
        /// </summary>
        public bool NoClose => true;

        /*// <summary>
        /// ����������
        /// </summary>
        private static readonly int PoolSize;*/

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
        protected RedisProxy(int db)
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
        internal TRedis _redis;
        /// <summary>
        /// �õ�һ�����õ�Redis�ͻ���
        /// </summary>
        public TRedis Redis
        {
            get
            {
                if (_redis != null)
                    return _redis;
                _redis = CreateClient();
                ChangeDb();
                return _redis;
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        internal TRedis ChangeDb(int db)
        {
            _db = db;
            return ChangeDb();
        }

        /// <summary>
        /// ����
        /// </summary>
        private TRedis ChangeDb()
        {
            if (_redis == null)
            {
                var old = _redis;
                _redis = CreateClient();
                _redis.ChangeDb(_db);
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
        internal void ResetClient(TRedis client)
        {
            _redis = client;
        }

        private TRedis CreateClient()
        {
            var redis = DependencyHelper.Create<TRedis>();
            redis.Option = Option;
            if (redis == null)
                throw new DependencyException(typeof(TRedis), "δ��ȷע��Redis����");
            Instances.Add(redis);
            return redis;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (_redis == null || _redis.NoClose)
                return;
            Instances.Remove(_redis);
            _redis.Dispose();
            _redis = default;
        }


        #endregion

    }
}