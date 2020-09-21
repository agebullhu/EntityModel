// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.ComponentModel;
using System.Runtime.Serialization;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     编辑状态
    /// </summary>
    [DataContract, Serializable]
    public class IndexEditStatus
    {
        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="data"></param>
        internal IndexEditStatus(EditDataObject data)
        {
            Entity = data;
        }

        /// <summary>
        ///     对应的对象
        /// </summary>
        [IgnoreDataMember]
        public EditDataObject Entity { get; }

        #endregion

        #region 修改状态


        /// <summary>
        ///     修改的属性列表
        /// </summary>
        [DataMember] internal byte[] modifiedProperties;

        /// <summary>
        ///     修改的属性列表
        /// </summary>
        [ReadOnly(true), DisplayName("修改的属性列表"), Category("运行时")]
        public byte[] ModifiedProperties => modifiedProperties ??= new byte[Entity.__Struct.Count + 1];

        /// <summary>
        ///     是否修改
        /// </summary>
        internal bool FieldIsModified(int propertyIndex)
        {
            return modifiedProperties != null && propertyIndex < Entity.__Struct.Count && modifiedProperties[propertyIndex] == 1;
        }

        /// <summary>
        ///     设置为非改变
        /// </summary>
        internal void SetUnModify()
        {
            modifiedProperties = null;
        }

        /// <summary>
        ///     设置为改变
        /// </summary>
        internal void SetModified()
        {
            for (var index = 0; index < ModifiedProperties.Length - 1; index++)
            {
                ModifiedProperties[index] = 1;
            }
            ModifiedProperties[Entity.__Struct.Count] = (byte)Entity.__Struct.Count;
        }

        /// <summary>
        ///     设置为非改变
        /// </summary>
        /// <param name="propertyIndex"> 字段的名字 </param>
        /// <returns>总体是否修改</returns>
        internal bool SetUnModify(int propertyIndex)
        {
            if (modifiedProperties == null || propertyIndex >= Entity.__Struct.Count)
            {
                return false;
            }
            if (ModifiedProperties[propertyIndex] > 0)
            {
                ModifiedProperties[propertyIndex] = 0;
                ModifiedProperties[Entity.__Struct.Count] -= 1;
            }
            return ModifiedProperties[Entity.__Struct.Count] > 0;
        }

        /// <summary>
        ///     记录属性修改
        /// </summary>
        /// <param name="propertyIndex">属性</param>
        internal void RecordModified(int propertyIndex)
        {
            if (propertyIndex >= Entity.__Struct.Count || ModifiedProperties[propertyIndex] > 0)
                return;
            ModifiedProperties[propertyIndex] = 1;
            ModifiedProperties[Entity.__Struct.Count] += 1;
        }

        #endregion

        #region 复制支持

        /// <summary>
        ///     复制修改状态
        /// </summary>
        /// <param name="target">要复制的源</param>
        internal void CopyState(IndexEditStatus target)
        {
            CopyStateInner(target);
        }

        /// <summary>
        ///     复制修改状态
        /// </summary>
        /// <param name="target">要复制的源</param>
        private void CopyStateInner(IndexEditStatus target)
        {
            if (target.modifiedProperties == null)
            {
                modifiedProperties = null;
            }
            else
            {
                for (var index = 0; index < target.ModifiedProperties.Length; index++)
                {
                    modifiedProperties[index] = target.ModifiedProperties[index];
                }
            }
        }

        #endregion

    }
}