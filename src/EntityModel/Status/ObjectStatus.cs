// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     ����״̬����
    /// </summary>
    [DataContract, Serializable]
    public class ObjectStatus
    {
        /// <summary>
        ///     �Ƿ�ֻ��,�����ڷ���ǰ__EntityStatusǰ��ֹ����__EntityStatus�����ڴ�
        /// </summary>
        [Browsable(false), IgnoreDataMember, JsonIgnore]
        public bool IsReadOnly { get; set; }

        /// <summary>
        ///     ״̬����Ϊ��
        /// </summary>
        [Browsable(false), IgnoreDataMember, JsonIgnore]
        public bool EntityStatusNull => Status == null;

        /// <summary>
        ///     ״̬����
        /// </summary>
        [Browsable(false), IgnoreDataMember, JsonIgnore]
        internal EditDataObject Entity { get; set; }

        /// <summary>
        ///     ״̬����
        /// </summary>
        internal IndexEditStatus _status;

        /// <summary>
        ///     ״̬����
        /// </summary>
        [Browsable(false), IgnoreDataMember, JsonIgnore]
        public IndexEditStatus Status => IsReadOnly ? null : (_status ??= new IndexEditStatus(Entity));


        /// <summary>
        ///     ����
        /// </summary>
        public void Reset(sbyte type = Exist)
        {
            _status = null;
            IsReadOnly = false;
            _state = type;
        }

        /// <summary>
        ///     �����޸�
        /// </summary>
        public void AcceptChanged()
        {
            if (IsReadOnly)
                return;
            _state = !IsDelete ? Exist : Deleted;
            Status.SetUnModify();
            Entity.AcceptChangedInner();
            Entity.OnStatusChanged(NotificationStatusType.Refresh);
        }

        /// <summary>
        ///     �����޸�
        /// </summary>
        public void RejectChanged()
        {
            if (IsReadOnly)
                return;
            IsModified = false;
            Entity.RejectChangedInner();
            Entity.OnStatusChanged(NotificationStatusType.Refresh);
        }

        /// <summary>
        ///     �Ƿ��޸�
        /// </summary>
        public bool FieldIsModified(int propertyIndex)
        {
            return !IsReadOnly && _status != null && _status.FieldIsModified(propertyIndex);
        }

        /// <summary>
        ///     ����Ϊ�Ǹı�
        /// </summary>
        /// <param name="propertyIndex"> �ֶε����� </param>
        public void SetUnModify(int propertyIndex)
        {
            if (IsReadOnly)
                return;
            SetModified(Status.SetUnModify(propertyIndex));
        }

        /// <summary>
        ///     ����Ϊ�ı�
        /// </summary>
        /// <param name="property"> �ֶε����� </param>
        public void SetModify(string property)
        {
            if (IsReadOnly)
                return;
            Status.RecordModified(propertyIndex);
            SetModified(true);
        }


        #region ������̬

        private const sbyte Added = 0x1;

        private const sbyte Exist = 0x2;

        private const sbyte Deleted = 0x4;

        private const sbyte Shadow = 0x8;

        private const sbyte Modify = 0x10;

        private const sbyte User = 0x20;


        private sbyte _state;

        /*// <summary>
        ///     ������̬״̬
        /// </summary>
        [ReadOnly(true), DisplayName("������̬״̬"), Category("����ʱ")]
        public EntitySubsist Subsist
        {
            get
            {
                switch (_state & 0xF)
                {
                    case Added:
                        return EntitySubsist.Added;
                    case Added | Modify:
                        return EntitySubsist.Adding;
                    case Deleted:
                        return EntitySubsist.Deleted;
                    case Deleted | Modify:
                        return EntitySubsist.Deleting;
                    case Exist:
                        return EntitySubsist.Exist;
                    case Exist | Modify:
                        return EntitySubsist.Modified;
                    default:
                        return EntitySubsist.None;
                }
            }
        }*/

        /// <summary>
        ///     �Ƿ����޸�
        /// </summary>
        [ReadOnly(true), DisplayName("�Ƿ��޸�"), Category("����ʱ")]
        public bool IsModified
        {
            get => (_state & Modify) == Modify;
            set
            {
                if (value)
                {
                    _state |= Modify;
                }
                else
                {
                    _state &= ~Modify;
                }

                if (IsReadOnly)
                {
                    if ((_state & Modify) == Modify)
                    {
                        Status.SetModified();
                    }
                    else
                    {
                        Status.SetUnModify();
                    }
                }
                Entity.OnStatusChanged(NotificationStatusType.Modified);
            }
        }
        void SetModified(bool modify)
        {
            if (modify)
            {
                _state |= Modify;
            }
            else
            {
                _state &= ~Modify;
            }
            Entity.OnStatusChanged(NotificationStatusType.Modified);
        }
        /// <summary>
        ///     �Ƿ��Ѵ���
        /// </summary>
        [ReadOnly(true), DisplayName("�Ƿ��Ѵ���"), Category("����ʱ")]
        public bool IsExist
        {
            get => (_state & Exist) == Exist;
            set
            {
                //Trace.WriteLine(string.Format("{1} => IsNew:{0}", value, Entity.GetValue(Entity.__Struct.PrimaryKey)), Entity.GetType().Name);
                if (value)
                {
                    _state |= Exist;
                }
                else
                {
                    _state &= ~Exist;
                }
            }
        }

        /// <summary>
        ///     �Ƿ������ӵ�
        /// </summary>
        [ReadOnly(true), DisplayName("�Ƿ��޸�"), Category("����ʱ")]
        public bool IsNew
        {
            get => (_state & Added) == Added;
            set
            {
                //Trace.WriteLine(string.Format("{1} => IsNew:{0}", value, Entity.GetValue(Entity.__Struct.PrimaryKey)), Entity.GetType().Name);
                if (value)
                {
                    _state |= Added;
                }
                else
                {
                    _state &= ~Added;
                }
                Entity.OnAdd(value);
                Entity.OnStatusChanged(NotificationStatusType.Added);
            }
        }

        /// <summary>
        ///     �Ƿ���ɾ��
        /// </summary>
        /// <returns></returns>
        [ReadOnly(true), DisplayName("�Ƿ���ɾ��"), Category("����ʱ")]
        public bool IsDelete
        {
            get => (_state & Deleted) == Deleted;
            set
            {
                //Trace.WriteLine(string.Format("{1} => IsDelete:{0}", value, Entity.GetValue(Entity.__Struct.PrimaryKey)), Entity.GetType().Name);
                if (value)
                {
                    _state |= Deleted;
                }
                else
                {
                    _state &= ~Deleted;
                }
                Entity.OnDelete(value);
                Entity.OnStatusChanged(NotificationStatusType.Deleted);
            }
        }

        /// <summary>
        ///     �Ƿ����Ա༭�ͻ���
        /// </summary>
        public bool IsFromClient
        {
            get => (_state & User) == User;
            set
            {
                if (value)
                {
                    _state |= User;
                }
                else
                {
                    _state &= ~User;
                }
            }
        }

        /// <summary>
        ///     �Ƿ�Ӱ�Ӹ���
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///     Ӱ�Ӹ�����Ϊ��ʡ���ڶ����õ�,����������
        ///     1 ��һ����������(������ͬ)�Ķ����ƶ���
        ///     2 ���п��ܱ��޸�
        ///     3 �������޸�ʱ,ʹ�����޸ĵ��¿����������洢
        ///     4 ����δ�޸�ʱ,ʹ��Դ����������Ҳ����д洢
        ///     ����˵��
        ///     1 ������Ϊ��ʱ,����һ���������������(�������������)
        ///     2 ������Ϊ��ʱ,������ͨ����������(��ֻ���������)
        ///     3 ��������Ϊ���,������ǽ�����
        ///     4 ������Ҫ�������ٲ���Ҫ�ı���,����������������(OnAdd)
        /// </remarks>
        [ReadOnly(true), DisplayName("�Ƿ���ɾ��"), Category("����ʱ")]
        public bool IsShadow
        {
            get => (_state & Shadow) == Shadow;
            set
            {
                if (value)
                {
                    _state |= Shadow;
                }
                else
                {
                    _state &= ~Shadow;
                }
            }
        }

        #endregion
    }
}