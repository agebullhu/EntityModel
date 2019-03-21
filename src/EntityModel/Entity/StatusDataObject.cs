// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     ʵ��״̬����
    /// </summary>
    [DataContract, Serializable]
    public abstract class StatusDataObject<TStatus> : DataObjectBase
        where TStatus : ObjectStatusBase, new()
    {
        /// <summary>
        ///     ״̬����
        /// </summary>
        [IgnoreDataMember, JsonIgnore]
        protected TStatus __status;

        /// <summary>
        ///     �Ƿ�ֻ��,�����ڷ���ǰ__EntityStatusǰ��ֹ����__EntityStatus�����ڴ�
        /// </summary>
        [IgnoreDataMember, JsonIgnore]
        private bool __isReadOnly;

        /// <summary>
        ///     �Ƿ�ֻ��,�����ڷ���ǰ__EntityStatusǰ��ֹ����__EntityStatus�����ڴ�
        /// </summary>
        [Browsable(false), IgnoreDataMember, JsonIgnore]
        public bool __IsReadOnly
        {
            get => __isReadOnly;
            set => __isReadOnly = value;
        }

        /// <summary>
        ///     ״̬����Ϊ��
        /// </summary>
        [Browsable(false), IgnoreDataMember, JsonIgnore]
        public bool __EntityStatusNull => __status == null;

        /// <summary>
        ///     ״̬����
        /// </summary>
        [Browsable(false), IgnoreDataMember, JsonIgnore]
        public TStatus __EntityStatus => __status ?? (/*__isReadOnly ? null : */__status = CreateStatus());

        /// <summary>
        ///     ����״̬����
        /// </summary>
        protected virtual TStatus CreateStatus()
        {
            var status = new TStatus();
            status.Initialize(this);
            return status;
        }
    }

    /// <summary>
    ///     ʵ��״̬����
    /// </summary>
    [DataContract, Serializable]
    public abstract class StatusDataObject : StatusDataObject<ObjectStatusBase>
    {
    }
}