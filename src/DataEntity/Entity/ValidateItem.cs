using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Gboxt.Common.DataModel
{
    /// <summary>
    /// 校验节点
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ValidateResultDictionary
    {
        [JsonProperty("results", NullValueHandling = NullValueHandling.Ignore)]
        public List<ValidateResult> Result = new List<ValidateResult>();
        
        public void TryAdd(ValidateResult row)
        {
            Result.Add(row);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("检查结果:<br/>");
            foreach (var item in Result)
            {
                sb.Append($"{item.Id}:<br/>");
                sb.Append(item);
            }
            return sb.ToString();
        }
    }

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
        public int Id { get; set; }

        /// <summary>
        /// 类型: 0没错 1警告 2错误
        /// </summary>
        [JsonProperty("succeed", NullValueHandling = NullValueHandling.Ignore)]
        public bool succeed => Items.Count == 0 || Items.All(p => p.succeed);

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<ValidateItem> Items = new List<ValidateItem>();

        [JsonProperty("messages", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Messages = new List<string>();

        /// <summary>
        /// 到消息文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return succeed ? "" : Items.Select(p => $"[{p.Caption}]{p.Message }").LinkToString();
        }
        public void Add(string message)
        {
            Messages.Add(message);
        }
        public void AddNoEmpty(string caption, string field)
        {
            Items.Add(new ValidateItem
            {
                succeed = false,
                Name = field,
                Caption = caption,
                Message = "不能为空"
            }); 
        }

        public void Add(string caption, string field, string message)
        {
            Items.Add(new ValidateItem
            {
                succeed = false,
                Name = field,
                Caption = caption,
                Message = message
            });
        }

        public void AddWarning(string caption, string field, string message)
        {
            Items.Add(new ValidateItem
            {
                warning = true,
                Name = field,
                Caption = caption,
                Message = message
            });
        }
    }

    /// <summary>
    /// 校验节点
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ValidateItem
    {
        /// <summary>
        /// 正确
        /// </summary>
        [JsonProperty("succeed", NullValueHandling = NullValueHandling.Ignore)]
        public bool succeed { get; set; }

        /// <summary>
        /// 1警告
        /// </summary>
        [JsonProperty("warning", NullValueHandling = NullValueHandling.Ignore)]
        public bool warning { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        [JsonProperty("field", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// 字段标题目
        /// </summary>
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Caption { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}