namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 表示实体校验接口
    /// </summary>
    public interface IValidate
    {
        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        bool Validate(out ValidateResult result)
        {
            result = null;
            return true;
        }
    }
}