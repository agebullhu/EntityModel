// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.Runtime.Serialization;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     ����״̬����
    /// </summary>
    [DataContract, Serializable]
    public class ObjectStatusBase
    {
        /// <summary>
        ///     ��Ӧ�Ķ���
        /// </summary>
        [IgnoreDataMember]
        public NotificationObject Object { get; private set; }

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        internal void Initialize(NotificationObject obj)
        {
            Object = obj;
            InitializeInner();
        }

        /// <summary>
        ///     ��ʼ����ʵ��
        /// </summary>
        protected virtual void InitializeInner()
        {
        }
    }
}