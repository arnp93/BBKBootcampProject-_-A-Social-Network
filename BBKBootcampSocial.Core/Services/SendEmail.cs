using System.Net.Mail;
using BBKBootcampSocial.Core.IServices;

namespace BBKBootcampSocial.Core.Services
{
    public class SendEmail : IMailSender
    {
        public void Send(string to, string subject, string body)
        {
            var defaultEmail = "arashcodenp@gmail.com";

            var mail = new MailMessage();

            var SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(defaultEmail, "BBK Bootcamp Social Network");

            mail.To.Add(to);

            mail.Subject = subject;

            mail.Body = body;

            mail.IsBodyHtml = true;

            SmtpServer.UseDefaultCredentials = true;

            // System.Net.Mail.Attachment attachment;
            // attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            // mail.Attachments.Add(attachment);

            SmtpServer.Port = 587;

            SmtpServer.Credentials = new System.Net.NetworkCredential(defaultEmail, "137213721372");

            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }
    }
}

