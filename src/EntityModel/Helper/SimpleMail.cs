using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using Agebull.Common.Configuration;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// ���ʼ�
    /// </summary>
    public static class SimpleMail
    {
        /// <summary>
        /// �����ʼ�
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="reciver"></param>
        /// <param name="content"></param>
        public static void SendMail(string subject, string content, string reciver)
        {
            SendMail(subject, content, new List<string> { reciver });
        }

        /// <summary>
        /// �����ʼ�
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="recivers"></param>
        /// <param name="content"></param>
        public static void SendMail(string subject, string content, List<string> recivers)
        {
            var sec = ConfigurationManager.Get("Email");
            var emailAcount = sec["EmailAcount"];
            var emailPassword = sec["EmailPassword"];
            var from = sec["FromEmail"];
            var smtp = sec["EmailSmtpAddress"];
            int port = sec.GetInt("EmailSmtpPort", 25);
            MailMessage message = new MailMessage
            {
                From = new MailAddress(@from)//���÷�����,��������Ҫ�����õ��ʼ����ͷ�����������һ��
            };

            foreach (var reciver in recivers)
                message.To.Add(reciver);//�����ռ���

            message.Subject = subject; //�����ʼ�����

            message.Body = content;//�����ʼ�����

            SmtpClient client = new SmtpClient(smtp, port);//�����ʼ����ͷ�����
            //���÷����˵������˺ź�����
            client.Credentials = new NetworkCredential(emailAcount, emailPassword);
            //����ssl,Ҳ���ǰ�ȫ����
            client.EnableSsl = true;
            //�����ʼ�
            client.Send(message);
        }
    }
}