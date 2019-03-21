// /*****************************************************
// (c)2008-2013 Copy right www.Agebull.com
// 作者:bull2
// 工程:AgebullProjectDesigner-Agebull.EntityModel.Common
// 建立:2014-08-21
// 修改:2014-08-25
// *****************************************************/

#region 引用

using System.Runtime.Serialization;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     名称内容对象
    /// </summary>
    /// <typeparam name="TName"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [DataContract(Name = "NameValue1", Namespace = "http://www.gboxt.com/2011")]
    public class NameValue<TName, TValue>
    {
        /// <summary>
        ///     名称
        /// </summary>
        [DataMember(Name = "name")]
        public TName name;

        /// <summary>
        ///     值
        /// </summary>
        [DataMember(Name = "value")]
        public TValue value;
    }

    /// <summary>
    ///     名称内容对象
    /// </summary>
    [DataContract(Name = "NameValue2", Namespace = "http://www.gboxt.com/2011")]
    public class NameValue<TValue> : NameValue<string, TValue>
    {
    }

    /// <summary>
    ///     名称内容对象
    /// </summary>
    [DataContract(Name = "NameValue", Namespace = "http://www.gboxt.com/2011")]
    public class NameValue : NameValue<string, string>
    {
        /// <summary>
        ///     构造
        /// </summary>
        public NameValue()
        {
        }

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="n"></param>
        /// <param name="v"></param>
        public NameValue(string n, object v)
        {
            name = n;
            value = v == null ? null : v.ToString();
        }
    }
    /// <summary>
    ///     名称内容对象
    /// </summary>
    [DataContract(Name = "NameValue2", Namespace = "http://www.gboxt.com/2011")]
    public class NameValue2 : NameValue<string, object>
    {
        /// <summary>
        ///     构造
        /// </summary>
        public NameValue2()
        {
        }

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="n"></param>
        /// <param name="v"></param>
        public NameValue2(string n, object v)
        {
            name = n;
            value = v;
        }
    }
}
