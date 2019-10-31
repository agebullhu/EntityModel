using System.Text;
using Newtonsoft.Json;

namespace Agebull.MicroZero.ZeroApis
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
        /// <param name="message">���ص���Ϣ</param>
        /// <returns>�ɹ��򷵻���</returns>
        public virtual bool Validate(out string message)
        {
            var msg = new StringBuilder();
            var success = true;
            if (PageIndex < 0)
            {
                success = false;
                msg.Append("ҳ�ű�����ڻ����0");
            }

            if (PageSize <= 0 || PageSize > 100)
            {
                success = false;
                msg.Append("�����������0��С��100");
            }

            message = msg.ToString();
            return success;
        }
    }
}