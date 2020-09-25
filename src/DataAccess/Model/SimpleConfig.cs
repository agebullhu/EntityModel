namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     配置基础
    /// </summary>
    public class SimpleConfig : IConfig//NotificationObject, 
    {
        /// <summary>
        /// 名称
        /// </summary>
        protected string _name;

        /// <summary>
        ///     名称
        /// </summary>
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
                //OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        ///     标题
        /// </summary>
        protected string _caption;

        /// <summary>
        ///     标题
        /// </summary>
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
                //OnPropertyChanged(nameof(Caption));
            }
        }

        /// <summary>
        ///     说明
        /// </summary>
        protected string _description;

        /// <summary>
        ///     说明
        /// </summary>
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
                //OnPropertyChanged(nameof(Description));
            }
        }
    }
}