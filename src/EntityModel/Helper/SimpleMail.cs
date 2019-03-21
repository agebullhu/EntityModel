using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using Agebull.Common.Configuration;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 发邮件
    /// </summary>
    public static class SimpleMail
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="reciver"></param>
        /// <param name="content"></param>
        public static void SendMail(string subject, string content, string reciver)
        {
            SendMail(subject, content, new List<string> { reciver });
        }

        /// <summary>
        /// 发送邮件
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
                From = new MailAddress(@from)//设置发件人,发件人需要与设置的邮件发送服务器的邮箱一致
            };

            foreach (var reciver in recivers)
                message.To.Add(reciver);//设置收件人

            message.Subject = subject; //设置邮件标题

            message.Body = content;//设置邮件内容

            SmtpClient client = new SmtpClient(smtp, port);//设置邮件发送服务器
            //设置发送人的邮箱账号和密码
            client.Credentials = new NetworkCredential(emailAcount, emailPassword);
            //启用ssl,也就是安全发送
            client.EnableSsl = true;
            //发送邮件
            client.Send(message);
        }
    }
}