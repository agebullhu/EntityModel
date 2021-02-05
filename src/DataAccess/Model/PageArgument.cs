using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace ZeroTeam.MessageMVC.ZeroApis
{
    /// <summary>
    ///     ��ҳ����Ĳ���
    /// </summary>
    [JsonObject]
    public class PageArgument //: IApiArgument
    {

        /// <summary>
        ///     ҳ��
        /// </summary>
        [JsonProperty("_page_")]
        public int Page { get; set; }

        /// <summary>
        ///     ÿҳ����
        /// </summary>
        [JsonProperty("_size_")]
        public int PageSize { get; set; }

        /// <summary>
        ///     ����
        /// </summary>
        [JsonProperty("_sort_")]
        public string Order { get; set; }

        /// <summary>
        ///     ����
        /// </summary>
        [JsonProperty("_order_")]
        public bool Desc { get; set; }


        /// <summary>
        ///     ����У��
        /// </summary>
        /// <param name="message">���״̬</param>
        /// <returns>�ɹ��򷵻���</returns>
        public bool Validate(out string message)
        {
            var msg = new StringBuilder();
            var success = true;
            if (Page < 0)
            {
                success = false;
                msg.Append("ҳ�ű�����ڻ����0");
            }

            if (PageSize <= 0 || PageSize > 100)
            {
                success = false;
                msg.Append("�����������0��С��100");
            }
            message = success ? null : msg.ToString();
            return success;
        }
    }

    /// <summary>
    ///     API���ط�ҳ��Ϣ
    /// </summary>
    public class ApiPage
    {
        /// <summary>
        ///     ��ǰҳ�ţ���1��ʼ��
        /// </summary>
        /// <example>1</example>
        public int Page { get; set; }

        /// <summary>
        ///     һҳ����
        /// </summary>
        /// <example>16</example>
        public int PageSize { get; set; }

        /// <summary>
        ///     ��ҳ��
        /// </summary>
        /// <example>999</example>
        public int PageCount { get; set; }

        /// <summary>
        ///     ������
        /// </summary>
        /// <example>9999</example>
        public int Total { get; set; }

    }

    /// <summary>
    ///     API���طֲ�ҳ����
    /// </summary>
    public class ApiPageData<TEntity> : ApiPage
    {
        /// <summary>
        ///     ����ֵ
        /// </summary>
        public List<TEntity> Rows { get; set; }
    }
}