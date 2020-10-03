using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 校验节点
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ValidateResult
    {
        /// <summary>
        /// 主键
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// 是否正确
        /// </summary>
        [JsonProperty("succeed", NullValueHandling = NullValueHandling.Ignore)]
        public bool Succeed => Items.Count == 0 || Items.All(p => p.Succeed || p.Warning);

        /// <summary>
        /// 节点
        /// </summary>
        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<ValidateItem> Items = new List<ValidateItem>();
        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty("messages", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Messages = new List<string>();

        /// <summary>
        /// 到消息文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Succeed ? "" : Items.Select(p => $"{p.Caption}:{p.Message}").LinkToString("\r\n");
        }

        /// <summary>
        /// 到消息文本
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return Succeed ? "{}" : Items.Select(p => $"\"{p.Name}\":\"{p.Message.Replace('\"', '\'') }\"").LinkToString("{", ",", "}");
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