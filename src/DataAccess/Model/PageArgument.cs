using System.Collections.Generic;
using System.Text;

namespace ZeroTeam.MessageMVC.ZeroApis
{
    /// <summary>
    ///     ��ҳ����Ĳ���
    /// </summary>
    public class PageArgument //: IApiArgument
    {
        /// <summary>
        ///     ҳ��
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        ///     ÿҳ����
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     ����
        /// </summary>
        public string Order { get; set; }


        /// <summary>
        ///     ����
        /// </summary>
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
        public int Page => PageIndex;

        /// <summary>
        ///     ��ǰҳ�ţ���1��ʼ��
        /// </summary>
        /// <example>1</example>
        public int PageIndex { get; set; }

        /// <summary>
        ///     һҳ����
        /// </summary>
        /// <example>16</example>
        public int PageSize { get; set; }

        /// <summary>
        ///     ��ҳ��
        /// </summary>
        /// <example>999</example>
        public long PageCount { get; set; }

        /// <summary>
        ///     ������
        /// </summary>
        /// <example>9999</example>
        public long RowCount { get; set; }

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