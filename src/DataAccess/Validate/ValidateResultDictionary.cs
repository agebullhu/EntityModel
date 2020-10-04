using System.Collections.Generic;
using System.Text;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 校验节点
    /// </summary>
    public class ValidateResultDictionary
    {
        /// <summary>
        /// 校验结果
        /// </summary>
        public List<ValidateResult> Result = new List<ValidateResult>();

        /// <summary>
        /// 尝试加入
        /// </summary>
        /// <param name="row"></param>
        public void TryAdd(ValidateResult row)
        {
            Result.Add(row);
        }
        /// <summary>
        /// 文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("检查结果:");
            foreach (var item in Result)
            {
                sb.AppendLine($"{item.Id}:");
                sb.Append(item);
            }
            return sb.ToString();
        }
    }
}