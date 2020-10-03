using System.ComponentModel;
using System.Runtime;

namespace HY.GameApi.Model
{
    /// <summary>
    ///     System.ComponentModel.INotifyPropertyChanging.PropertyChanged �¼��ṩ���ݡ�
    /// </summary>
    public sealed class PropertyChangingEventArgsEx : PropertyChangingEventArgs
    {
        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="propertyIndex">��������</param>
        /// <param name="oldValue">��ֵ</param>
        /// <param name="newValue">��ֵ</param>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public PropertyChangingEventArgsEx(string propertyIndex, object oldValue, object newValue)
            : base(propertyIndex)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        /// <summary>
        ///     ԭʼ������
        /// </summary>
        public object OldValue
        {
            get;
            private set;
        }

        /// <summary>
        ///     ���ĵ�����
        /// </summary>
        public object NewValue
        {
            get;
            set;
        }

        /// <summary>
        ///     ��ֵ�Ƿ��ڴ���������޸�
        /// </summary>
        public bool ValueIsChaged
        {
            get;
            set;
        }

        /// <summary>
        ///     �޸ı��뱻��ֹ
        /// </summary>
        public bool Arrest
        {
            get;
            set;
        }
    }
}