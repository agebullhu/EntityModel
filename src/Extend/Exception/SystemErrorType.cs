// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12
namespace Agebull.Common
{
    /// <summary>
    ///   系统错误类型
    /// </summary>
    public enum SystemErrorType
    {
        /// <summary>
        ///   操作系统错误
        /// </summary>
        OSError ,

        /// <summary>
        ///   数据库错误
        /// </summary>
        DataBaseError ,

        /// <summary>
        ///   内存错误
        /// </summary>
        MemoryError ,

        /// <summary>
        ///   磁盘错误
        /// </summary>
        DiskError ,

        /// <summary>
        ///   网络错误
        /// </summary>
        NetError ,

        /// <summary>
        ///   未知错误
        /// </summary>
        UnknowError
    }
}
