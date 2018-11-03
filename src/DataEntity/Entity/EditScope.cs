// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using Agebull.Common.Base;

#endregion

namespace Agebull.Common.DataModel
{
    /// <summary>
    ///     实体编辑范围,在范围中,不发出任何属性事件,结束后可选择是否重发事件(已取消功能)
    /// </summary>
    public sealed class EditScope : ScopeBase
    {
        /// <summary>
        ///     当前线程的当前范围
        /// </summary>
        [ThreadStatic]
        private static EditScope _currentScope;

        /*// <summary>
        ///     锁定完成后是否发出属性修改事件
        /// </summary>
        private readonly bool _endRaiseEvent;*/

        /// <summary>
        ///     在这之前是否也在进行初始化
        /// </summary>
        private readonly EditArrestMode _oldArrest;

        /// <summary>
        ///     上一个
        /// </summary>
        private readonly EditScope _preScope;

        /// <summary>
        ///     锁定的编辑对象
        /// </summary>
        private readonly IndexEditStatus _status;

        /// <summary>
        ///     构建编辑范围
        /// </summary>
        /// <param name="status">锁定的编辑对象</param>
        /// <param name="arrestMode">编辑阻止模式</param>
        /// <param name="endRaiseEvent">锁定完成后是否发出属性修改事件(已取消功能)</param>
        public EditScope(IndexEditStatus status, EditArrestMode arrestMode = EditArrestMode.All, bool endRaiseEvent = false)
        {
            _preScope = _currentScope;
            _currentScope = this;
            _status = status;
            _oldArrest = _status.Arrest;
            _status.Arrest = arrestMode;
            //this._endRaiseEvent = endRaiseEvent;
        }

        /// <summary>
        ///     当前线程的当前范围
        /// </summary>
        public static EditScope CurrentScope => _currentScope;

        /// <summary>
        ///     清理资源
        /// </summary>
        protected override void OnDispose()
        {
            _status.Arrest = _oldArrest;
            _currentScope = _preScope;
            //if (_endRaiseEvent)
            //{

            //}
        }
    }
}