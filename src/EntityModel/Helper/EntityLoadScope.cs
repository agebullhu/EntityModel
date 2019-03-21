// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.Common.Base;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     实体载入范围,在范围中,不发出任何属性事件不记录任何修改状态
    /// </summary>
    public sealed class EntityLoadScope : ScopeBase
    {
        /// <summary>
        ///     在这之前是否也在进行初始化
        /// </summary>
        private readonly EditDataObject _entity;

        /// <summary>
        ///     实体之前的只读状态
        /// </summary>
        private readonly bool _readOnly;

        /// <summary>
        ///     构建编辑范围
        /// </summary>
        /// <param name="entity">锁定的编辑对象</param>
        public EntityLoadScope(EditDataObject entity)
        {
            _entity = entity;
            _readOnly = entity.__IsReadOnly;
            entity.__IsReadOnly = true;
        }

        /// <summary>
        ///     清理资源
        /// </summary>
        protected override void OnDispose()
        {
            _entity.__IsReadOnly = _readOnly;
        }
    }
}