/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/2 2:12:46*/
#region using
using System.Runtime.Serialization;

#endregion

namespace Zeroteam.MessageMVC.EventBus
{
    /// <summary>
    /// 事件订阅
    /// </summary>
    [DataContract]
    sealed partial class EventSubscribeEntity
    {

        /// <summary>
        /// 初始化
        /// </summary>
        partial void Initialize()
        {
            /*
                        _isfreeze = true;
                        _datastate = 0;
                        _authorid = 0;*/
        }

    }
}