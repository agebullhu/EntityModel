using Agebull.Common.Configuration;
using Agebull.EntityModel.Common;
using System.Collections.Generic;
using ZeroTeam.MessageMVC;

namespace Agebull.EntityModel.DataEvents
{
    /// <summary>
    /// 数据事件配置
    /// </summary>
    public class DataEventOption : IZeroOption
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// 使用SQL注入
        /// </summary>
        public bool Injection { get; set; }

        /// <summary>
        /// 使用数据事件
        /// </summary>
        public bool Event { get; set; }

        /// <summary>
        /// 服务类型
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// 排除注入数据接口类型
        /// </summary>
        public HashSet<string> ExcludeInterfaces { get; set; }

        /// <summary>
        /// 排除的事件
        /// </summary>
        public HashSet<DataOperatorType> ExcludeEvents { get; set; }

        #region IZeroOption


        /// <summary>
        /// 单一实例
        /// </summary>
        public static DataEventOption Instance = new DataEventOption();

        const string sectionName = "DataEvent";

        const string optionName = "数据事件配置";

        const string supperUrl = "https://";

        /// <summary>
        /// 支持地址
        /// </summary>
        string IZeroOption.SupperUrl => supperUrl;

        /// <summary>
        /// 配置名称
        /// </summary>
        string IZeroOption.OptionName => optionName;


        /// <summary>
        /// 节点名称
        /// </summary>
        string IZeroOption.SectionName => sectionName;

        /// <summary>
        /// 是否动态配置
        /// </summary>
        bool IZeroOption.IsDynamic => false;

        void IZeroOption.Load(bool first)
        {
            var opt = ConfigurationHelper.Get<DataEventOption>(sectionName);
            if (opt == null)
            {
                Injection = true;
                return;
            }
            Service = opt.Service;
            Receiver = opt.Receiver;
            Event = opt.Event;
            Injection = opt.Injection;
            ExcludeEvents = opt.ExcludeEvents ?? new HashSet<DataOperatorType>();
            ExcludeInterfaces = opt.ExcludeInterfaces ?? new HashSet<string>();
        }
        #endregion
    }
}