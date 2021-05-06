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
    ///     表示属性类型
    /// </summary>
    public enum PropertyValueType
    {
        /// <summary>
        ///     值类型
        /// </summary>
        Value,

        /// <summary>
        ///     数字枚举
        /// </summary>
        NumberEnum,

        /// <summary>
        ///     文本
        /// </summary>
        String,

        /// <summary>
        ///     文本枚举
        /// </summary>
        StringEnum,

        /// <summary>
        ///     可为空的值对象
        /// </summary>
        Nullable,

        /// <summary>
        ///     对象
        /// </summary>
        Object
    }
}