using System;
using System.Runtime.Serialization;
using Agebull.EntityModel.Common;
using Agebull.MicroZero.ZeroApis;
using Newtonsoft.Json;

namespace Agebull.MicroZero.ApiDocuments
{
    /// <summary>
    ///     Api方法的信息
    /// </summary>
    [DataContract]
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiActionInfo : ApiDocument
    {
        /// <summary>
        ///     无参方法
        /// </summary>
        public Func<IApiResult> Action;

        /// <summary>
        ///     有参方法
        /// </summary>
        public Func<IApiArgument, IApiResult> ArgumentAction;

        /// <summary>
        ///     参数类型
        /// </summary>
        public Type ArgumenType;

        /// <summary>
        ///     所在控制器类型
        /// </summary>
        public string Controller;

        /// <summary>
        ///     是否有调用参数
        /// </summary>
        public bool HaseArgument;
    }
}