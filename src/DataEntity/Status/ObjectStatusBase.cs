// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Runtime.Serialization;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     对象状态基类
    /// </summary>
    [DataContract, Serializable]
    public class ObjectStatusBase
    {
        /// <summary>
        ///     对应的对象
        /// </summary>
        [IgnoreDataMember]
        public NotificationObject Object { get; private set; }

        /// <summary>
        ///     初始化
        /// </summary>
        internal void Initialize(NotificationObject obj)
        {
            Object = obj;
            InitializeInner();
        }

        /// <summary>
        ///     初始化的实现
        /// </summary>
        protected virtual void InitializeInner()
        {
        }
    }
}