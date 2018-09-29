// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-24
// // *****************************************************/

#region ����

using System;
using System.Collections.Generic;
using Agebull.Common.AppManage;
using Agebull.Common.DataModel.Redis;
using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Agebull.Common.Redis;
using Agebull.Common.WebApi.Auth;
using Gboxt.Common.DataModel.MySql;
using Newtonsoft.Json;

#endregion

namespace Agebull.Common.WebApi
{
    /// <summary>
    ///     Ϊҵ���������Ķ���
    /// </summary>
    public class BusinessContext : ApiContext
    {

        #region �̵߳���
        
        /// <summary>
        ///     ȡ�û������̵߳������󣬵�ǰ���󲻴���ʱ�����Զ�����һ��
        /// </summary>
        public static BusinessContext Context => Current as BusinessContext;

        /// <summary>
        /// ���浱ǰ������
        /// </summary>
        public void Cache()
        {
            LogRecorder.MonitorTrace(JsonConvert.SerializeObject(this));
            using (RedisProxy proxy = new RedisProxy())
            {
                proxy.Set(GetCacheKey(RequestInfo.RequestId), this);
            }
        }

        /// <summary>
        /// �õ�����ļ�
        /// </summary>
        public static string GetCacheKey(Guid requestId)
        {
            return RedisKeyBuilder.ToSystemKey("api", "ctx", requestId.ToString().ToUpper());
        }

        /// <summary>
        /// �õ�����ļ�
        /// </summary>
        public static string GetCacheKey(string requestId)
        {
            return RedisKeyBuilder.ToSystemKey("api", "ctx", requestId.Trim('$').ToUpper());
        }
        #endregion

        #region ����������

        /// <summary>
        ///     ����
        /// </summary>
        public BusinessContext()
        {
            LogRecorder.MonitorTrace("BusinessContext.ctor");
        }

        /// <summary>
        ///     ����
        /// </summary>
        ~BusinessContext()
        {
            Dispose();
        }

        /// <inheritdoc />
        protected override void OnDispose()
        {
            GC.ReRegisterForFinalize(this);
            TransactionScope.EndAll();
            LogRecorder.MonitorTrace("BusinessContext.DoDispose");
        }

        #endregion


        #region Ȩ�޶���

        /// <summary>
        ///     ��ǰҳ��ڵ�����
        /// </summary>
        public IPageItem PageItem { get; set; }

        /// <summary>
        ///     Ȩ��У�����
        /// </summary>
        private IPowerChecker _powerChecker;


        /// <summary>
        ///     Ȩ��У�����
        /// </summary>
        public IPowerChecker PowerChecker => _powerChecker ?? (_powerChecker = IocHelper.Create<IPowerChecker>());

        /// <summary>
        ///     �û��Ľ�ɫȨ��
        /// </summary>
        private List<IRolePower> _powers;

        /// <summary>
        ///     �û��Ľ�ɫȨ��
        /// </summary>
        public List<IRolePower> Powers => _powers ?? (_powers = PowerChecker.LoadUserPowers(Customer));

        /// <summary>
        /// ��ǰҳ��Ȩ������
        /// </summary>
        public IRolePower CurrentPagePower
        {
            get;
            set;
        }

        /// <summary>
        /// �ڵ�ǰҳ�����Ƿ����ִ�в���
        /// </summary>
        /// <param name="action">����</param>
        /// <returns></returns>
        public bool CanDoCurrentPageAction(string action)
        {
            return PowerChecker == null || PowerChecker.CanDoAction(Customer, PageItem, action);
        }

        #endregion
    }
}