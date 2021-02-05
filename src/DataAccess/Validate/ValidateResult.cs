using System.Collections.Generic;
using System.Linq;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 校验节点
    /// </summary>
    public class ValidateResult
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 是否正确
        /// </summary>
        public bool Succeed => Items.Count == 0 || Items.All(p => p.Succeed || p.Warning);

        /// <summary>
        /// 节点
        /// </summary>
        public List<ValidateItem> Items = new List<ValidateItem>();
        /// <summary>
        /// 消息
        /// </summary>
        public List<string> Messages = new List<string>();

        /// <summary>
        /// 到消息文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Succeed ? "" : string.Join("\r\n", Items.Select(p => $"{p.Caption}:{p.Message}"));
        }

        /// <summary>
        /// 到消息文本
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return Succeed
                ? "{}"
                : $"{{{string.Join(",", Items.Select(p => $"\"{p.Name}\":\"{p.Message.Replace('\"', '\'') }\""))}}}";
        }

        /// <summary>
        /// 加入消息
        /// </summary>
        /// <param name="message"></param>
        public void Add(string message)
        {
            Messages.Add(message);
        }
        /// <summary>
        /// 加入校验不能为空的消息
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="field"></param>
        public void AddNoEmpty(string caption, string field)
        {
            Items.Add(new ValidateItem
            {
                Succeed = false,
                Name = field,
                Caption = caption,
                Message = "不能为空"
            });
        }
        /// <summary>
        /// 加入消息
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="field"></param>
        /// <param name="message"></param>
        public void Add(string caption, string field, string message)
        {
            Items.Add(new ValidateItem
            {
                Succeed = false,
                Name = field,
                Caption = caption,
                Message = message
            });
        }
        /// <summary>
        /// 加入警告
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="field"></param>
        /// <param name="message"></param>
        public void AddWarning(string caption, string field, string message)
        {
            Items.Add(new ValidateItem
            {
                Warning = true,
                Name = field,
                Caption = caption,
                Message = message
            });
        }
    }
}