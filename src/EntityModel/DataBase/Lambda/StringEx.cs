// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:
// // 建立:2016-06-16
// // 修改:2016-06-16
// // *****************************************************/

#region 引用


#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     用于SQL解析
    /// </summary>
    public static class StringEx
    {
        /// <summary>
        /// 用于SQL中的后部Like,如[field] like 'abc%',以使用索引
        /// </summary>
        /// <param name="str"></param>
        /// <param name="des"></param>
        /// <returns></returns>
        public static bool LeftLike(this string str, string des)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(des) || str.Length <= des.Length)
                return false;
            return str.Length == des.Length
                ? string.Equals(str, des, System.StringComparison.OrdinalIgnoreCase)
                : string.Equals(str.Substring(0, des.Length), des, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}