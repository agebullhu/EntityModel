namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 校验节点
    /// </summary>
    public static class EntityValueChecker
    {
        /// <summary>
        /// 对比文本是否相同
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsModify(this string txt,string val)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                return !string.IsNullOrWhiteSpace(val);
            }
            return string.IsNullOrWhiteSpace(val) || string.Equals(txt, val.Trim());
        }
    }
}