using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace ZeroTeam.MessageMVC.ZeroApis
{
    /// <summary>
    ///     ��ҳ����Ĳ���
    /// </summary>
    public class PageArgument : IApiArgument
    {
        /// <summary>
        ///     ҳ��
        /// </summary>
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int PageIndex { get; set; }

        /// <summary>
        ///     ÿҳ����
        /// </summary>
        [JsonProperty("pageSize", NullValueHandling = NullValueHandling.Ignore)]
        public int PageSize { get; set; }

        /// <summary>
        ///     ÿҳ����
        /// </summary>
        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        public string Order { get; set; }


        /// <summary>
        ///     ����
        /// </summary>
        [JsonProperty("desc", NullValueHandling = NullValueHandling.Ignore)]
        public bool Desc { get; set; }


        /// <summary>
        ///     ����У��
        /// </summary>
        /// <param name="status">���״̬</param>
        /// <returns>�ɹ��򷵻���</returns>
        public virtual bool Validate(out IOperatorStatus status)
        {
            status = ApiResultHelper.Succees(); 
            var msg = new StringBuilder();
            status.Success = true;
            if (PageIndex < 0)
            {
                status.Success = false;
                msg.Append("ҳ�ű�����ڻ����0");
            }

            if (PageSize <= 0 || PageSize > 100)
            {
                status.Success = false;
                msg.Append("�����������0��С��100");
            }
            if (status.Success)
            {
                return true;
            }
            status.Message= msg.ToString();
            status.Code = OperatorStatusCode.ArgumentError;
            return status.Success;
        }
    }

    /// <summary>
    ///     API���ط�ҳ��Ϣ
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApiPage
    {
        /// <summary>
        ///     ��ǰҳ�ţ���1��ʼ��
        /// </summary>
        /// <example>1</example>
        public int Page => PageIndex;

        /// <summary>
        ///     ��ǰҳ�ţ���1��ʼ��
        /// </summary>
        /// <example>1</example>
        [JsonProperty("page")]
        public int PageIndex { get; set; }

        /// <summary>
        ///     һҳ����
        /// </summary>
        /// <example>16</example>
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        ///     ��ҳ��
        /// </summary>
        /// <example>999</example>
        [JsonProperty("pageCount")]
        public int PageCount { get; set; }

        /// <summary>
        ///     ������
        /// </summary>
        /// <example>9999</example>
        [JsonProperty("rowCount")]
        public int RowCount { get; set; }

        /// <summary>
        ///     ������
        /// </summary>
        /// <example>9999</example>
        [JsonProperty("total", Required = Required.Always)]
        public int Total => RowCount;
    }

    /// <summary>
    ///     API���طֲ�ҳ����
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApiPageData<TEntity> : ApiPage
    {
        /// <summary>
        ///     ����ֵ
        /// </summary>
        [JsonProperty("rows")]
        public List<TEntity> Rows { get; set; }
    }
}