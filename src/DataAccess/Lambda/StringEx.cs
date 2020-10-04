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
    public static class Ex
    {
        /// <summary>
        /// 用于SQL中的后部Like,如[field] like 'abc%',以使用索引
        /// </summary>
        /// <param name="str"></param>
        /// <param name="des"></param>
        /// <returns></returns>
        public static bool Like(this string str, string des)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(des) || str.Length <= des.Length)
                return false;
            return str.Contains(str);
        }

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

        /// <summary>
        /// 用于SQL中的文本字段比较
        /// </summary>
        /// <param name="str"></param>
        /// <param name="des"></param>
        /// <returns></returns>
        public static bool FieldEquals<T>(this string field, T value) => true;

        /// <summary>
        /// 用于SQL中的文本字段比较
        /// </summary>
        /// <param name="str"></param>
        /// <param name="des"></param>
        /// <returns></returns>
        public static bool Expression<T>(this string field, string expression, T value) => true;

        /// <summary>
        /// 用于SQL中的文本字段比较
        /// </summary>
        /// <param name="str"></param>
        /// <param name="des"></param>
        /// <returns></returns>
        public static bool Condition(this string condition) => true;

        /// <summary>
        /// 用于SQL中的文本字段比较
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool IsNull(this string field) => true;

        /// <summary>
        /// 用于SQL中的文本字段比较
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool In<T>(this T field, params T[] values) => true;

        /// <summary>
        /// 用于SQL中的文本字段比较
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool IsNotNull(this string field) => true;
    }
}