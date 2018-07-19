// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     实体状态对象
    /// </summary>
    [DataContract, Serializable]
    public abstract class StatusDataObject<TStatus> : DataObjectBase
        where TStatus : ObjectStatusBase, new()
    {
        /// <summary>
        ///     状态对象
        /// </summary>
        [IgnoreDataMember, JsonIgnore]
        protected TStatus __status;

        /// <summary>
        ///     是否只读,用于在访问前__EntityStatus前防止构造__EntityStatus消耗内存
        /// </summary>
        [IgnoreDataMember, JsonIgnore]
        private bool __isReadOnly;

        /// <summary>
        ///     是否只读,用于在访问前__EntityStatus前防止构造__EntityStatus消耗内存
        /// </summary>
        [Browsable(false), IgnoreDataMember, JsonIgnore]
        public bool __IsReadOnly
        {
            get => __isReadOnly;
            set => __isReadOnly = value;
        }

        /// <summary>
        ///     状态对象为空
        /// </summary>
        [Browsable(false), IgnoreDataMember, JsonIgnore]
        public bool __EntityStatusNull => __status == null;

        /// <summary>
        ///     状态对象
        /// </summary>
        [Browsable(false), IgnoreDataMember, JsonIgnore]
        public TStatus __EntityStatus => __status ?? (__status = CreateStatus());

        /// <summary>
        ///     构建状态对象
        /// </summary>
        protected virtual TStatus CreateStatus()
        {
            var status = new TStatus();
            status.Initialize(this);
            return status;
        }
    }

    /// <summary>
    ///     实体状态对象
    /// </summary>
    [DataContract, Serializable]
    public abstract class StatusDataObject : StatusDataObject<ObjectStatusBase>
    {
    }
}