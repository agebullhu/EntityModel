/*****************************************************
(c)2016-2021 by ZeroTeam
作者: 胡天水
工程: Agebull.EntityModel.CoreAgebull.DataModel
建立:2016-06-07
修改: -
*****************************************************/

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     表明是一个数据对象
    /// </summary>
    public interface IDataObject
    {
        /// <summary>
        ///     复制值
        /// </summary>
        /// <param name="source">复制的源字段</param>
        void CopyValue(IDataObject source);

        /// <summary>
        ///     得到字段的值
        /// </summary>
        /// <param name="field"> 字段的名字 </param>
        /// <returns> 字段的值 </returns>
        object GetValue(string field);

        /// <summary>
        ///     配置字段的值
        /// </summary>
        /// <param name="field"> 字段的名字 </param>
        /// <param name="value"> 字段的值 </param>
        void SetValue(string field, object value);
    }
}