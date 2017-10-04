// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Configuration;
using System.Runtime.Serialization;
using Newtonsoft.Json;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     实体基类
    /// </summary>
    [DataContract, Serializable]
    public abstract class DataObjectBase : NotificationObject, IDataObject
    {
        #region 实体操作支持

        /// <summary>
        ///     复制值
        /// </summary>
        /// <param name="source">复制的源字段</param>
        public void CopyValue(IDataObject source)
        {
            var entity = source as DataObjectBase;
            if (entity != null)
            {
                CopyValueInner(entity);
            }
        }

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public void SetValue(string property, object value)
        {
            SetValueInner(property, value);
        }

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="property"></param>
        public object GetValue(string property)
        {
            return GetValueInner(property);
        }

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="property"></param>
        public TValue GetValue<TValue>(string property)
        {
            return GetValueInner<TValue>(property);
        }

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public void SetValue(int property, object value)
        {
            SetValueInner(property, value);
        }

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="property"></param>
        public object GetValue(int property)
        {
            return GetValueInner(property);
        }

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="property"></param>
        public TValue GetValue<TValue>(int property)
        {
            return GetValueInner<TValue>(property);
        }

        #endregion

        #region 内部实现

        /// <summary>
        ///     复制值
        /// </summary>
        /// <param name="source">复制的源字段</param>
        protected abstract void CopyValueInner(DataObjectBase source);

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        protected abstract void SetValueInner(string property, object value);

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="property"></param>
        protected abstract object GetValueInner(string property);

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        protected abstract void SetValueInner(int property, object value);

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="property"></param>
        protected abstract object GetValueInner(int property);

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="property"></param>
        protected virtual TValue GetValueInner<TValue>(string property)
        {
            return (TValue) GetValue(property);
        }

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="property"></param>
        protected virtual TValue GetValueInner<TValue>(int property)
        {
            return (TValue) GetValue(property);
        }

        #endregion

        #region 数据版本管理

        private static byte _version;

        /// <summary>
        ///     实体格式版本号
        /// </summary>
        public static byte EntityVersion
        {
            get
            {
                if (_version > 0)
                {
                    return _version;
                }
                var ev = ConfigurationManager.AppSettings["EntityVersion"];
                return _version = string.IsNullOrWhiteSpace(ev) ? (byte) 1 : byte.Parse(ev);
            }
        }

#if WEB

        /// <summary>
        ///     用于页面的选择属性
        /// </summary>
        [JsonProperty("IsSelected")]
        public bool __IsSelected { get; set; }
#endif

        #endregion
    }
}