using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Agebull.Common.DataModel
{
    /// <summary>
    ///     配置基础
    /// </summary>
    [DataContract, JsonObject(MemberSerialization.OptIn)]
    public class SimpleConfig : NotificationObject, IConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember, JsonProperty("name",NullValueHandling = NullValueHandling.Ignore)]
        protected string _name;

        /// <summary>
        ///     名称
        /// </summary>
        [IgnoreDataMember,JsonIgnore, Category("*设计"), DisplayName("名称")]
        public string Name
        {
            get => _name;
            set
            {
                var now = !string.IsNullOrWhiteSpace(value) ? value.Trim() : null;
                if (_name == now)
                {
                    return;
                }
                _name = now;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        ///     标题
        /// </summary>
        [DataMember, JsonProperty("caption", NullValueHandling = NullValueHandling.Ignore)]
        protected string _caption;

        /// <summary>
        ///     标题
        /// </summary>
        [IgnoreDataMember,JsonIgnore, Category("*设计"), DisplayName("标题")]
        public string Caption
        {
            get => _caption ?? _name;
            set
            {
                if (_caption == value)
                {
                    return;
                }
                _caption = !string.IsNullOrWhiteSpace(value) ? value.Trim() : null;
                RaisePropertyChanged(nameof(Caption));
            }
        }

        /// <summary>
        ///     说明
        /// </summary>
        [DataMember, JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        protected string _description;

        /// <summary>
        ///     说明
        /// </summary>
        [IgnoreDataMember,JsonIgnore, Category("*设计"), DisplayName("说明")]
        public string Description
        {
            get => _description;
            set
            {
                if (_description == value)
                {
                    return;
                }
                _description = !string.IsNullOrWhiteSpace(value) ? value.Trim() : null; ;
                RaisePropertyChanged(nameof(Description));
            }
        }

    }
}