using System.Collections.Generic;
using System.Diagnostics;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     编辑状态
    /// </summary>
    public class EntityEditStatus
    {
        #region 修改状态

        private HashSet<string> modifiedProperties;

        /// <summary>
        ///     修改的属性列表
        /// </summary>
        public HashSet<string> ModifiedProperties
        {
            get => modifiedProperties ??= new HashSet<string>();
            set => modifiedProperties = value;
        }

        /// <summary>
        ///     修改的属性列表
        /// </summary>
        public bool IsSetFullModify
        {
            get;
            set;
        }

        /// <summary>
        ///     来自客户端
        /// </summary>
        public bool IsFromClient
        {
            get;
            set;
        }

        /// <summary>
        ///     是否存在
        /// </summary>
        public bool IsExist
        {
            get;
            set;
        }

        /// <summary>
        ///     是否修改
        /// </summary>
        public bool IsModified
        {
            get => IsSetFullModify || modifiedProperties != null && modifiedProperties.Count > 0;
            set
            {
                IsSetFullModify = true;
                modifiedProperties = null;
            }
        }

        /// <summary>
        ///     是否修改
        /// </summary>
        public bool IsChanged(string property)
        {
            return IsSetFullModify || modifiedProperties != null && modifiedProperties.Contains(property);
        }

        /// <summary>
        ///     设置为非改变
        /// </summary>
        /// <param name="property"> 字段的名字 </param>
        /// <returns>总体是否修改</returns>
        public void SetModified(string property)
        {
            if (!IsSetFullModify)
                ModifiedProperties.Add(property);
        }

        /// <summary>
        ///     设置为非改变
        /// </summary>
        public void SetUnModify()
        {
            IsSetFullModify = false;
            modifiedProperties = null;
        }

        /// <summary>
        ///     设置为非改变
        /// </summary>
        /// <param name="property"> 字段的名字 </param>
        /// <returns>总体是否修改</returns>
        public void SetUnModify(string property)
        {
            if (!IsSetFullModify && modifiedProperties != null)
                modifiedProperties.Remove(property);
        }

        #endregion

        #region 复制支持

        /// <summary>
        ///     复制修改状态
        /// </summary>
        /// <param name="target">要复制的源</param>
        public void CopyFrom(EntityEditStatus target)
        {
            IsSetFullModify = target.IsSetFullModify;
            if (target.modifiedProperties == null)
            {
                modifiedProperties = null;
            }
            else
            {
                modifiedProperties = new HashSet<string>();
                foreach (var name in target.modifiedProperties)
                    modifiedProperties.Add(name);
            }
        }

        #endregion

    }
}