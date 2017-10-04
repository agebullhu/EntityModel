// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     对象生态状态
    /// </summary>
    public enum EntitySubsist
    {
        /// <summary>
        ///     未知,只读对象被识别为存在
        /// </summary>
        None,

        /// <summary>
        ///     新增未保存
        /// </summary>
        Adding,

        /// <summary>
        ///     新增已保存,相当于Exist,但可用于处理新增保存的后期事件
        /// </summary>
        Added,

        /// <summary>
        ///     影子(即其它对象的复制品且未修改,可不保存而重用复制器信息)
        /// </summary>
        Shadow,

        /// <summary>
        ///     将要删除
        /// </summary>
        Deleting,

        /// <summary>
        ///     已经删除
        /// </summary>
        Deleted,

        /// <summary>
        ///     将要修改
        /// </summary>
        Modify,

        /// <summary>
        ///     已经修改
        /// </summary>
        Modified,

        /// <summary>
        ///     已存在
        /// </summary>
        Exist = None
    }

    /// <summary>
    ///     数据操作状态
    /// </summary>
    public enum DataOperatorType
    {
        /// <summary>
        ///     未知
        /// </summary>
        None,

        /// <summary>
        ///     新增
        /// </summary>
        Insert,

        /// <summary>
        ///     更新
        /// </summary>
        Update,
        
        /// <summary>
        ///     删除
        /// </summary>
        Delete
    }
}