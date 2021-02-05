﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Agebull.EntityModel.Vue
{
    /// <summary>
    /// 树节点
    /// </summary>
    public class TreeNode
    {
        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("label"), JsonPropertyName("label")]
        public string Label { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [JsonProperty("type"), JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// 扩展标签
        /// </summary>
        [JsonProperty("tag"), JsonPropertyName("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        [JsonProperty("ext"), JsonPropertyName("ext")]
        public string Ext { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [JsonProperty("icon"), JsonPropertyName("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [JsonProperty("desc"), JsonPropertyName("desc")]
        public string Description { get; set; }

        /// <summary>
        /// 是否有子级
        /// </summary>
        [JsonProperty("isLeaf"), JsonPropertyName("isLeaf")]
        public bool IsLeaf { get; set; }

        /// <summary>
        /// 子级
        /// </summary>
        [JsonProperty("children"), JsonPropertyName("children")]
        public List<TreeNode> Children { get; set; }

    }
}