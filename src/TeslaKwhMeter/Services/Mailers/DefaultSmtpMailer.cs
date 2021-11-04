using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeslaKwhMeter.Base.Contacts.Helpers;

namespace TeslaKwhMeter.Services.Mailers
{
    internal class DefaultSmtpMailer : IMailer
    {
        public async Task SendMail(string smtpHost, int smtpPort, string smtpUserName, string smtpPassword, string to, string from, string subject, string body, bool isHtml)
        {
            await Task.Run(() => SendMailSync(smtpHost, smtpPort, smtpUserName, smtpPassword, to, from, subject, body, isHtml));
        }

        public void SendMailSync(string smtpHost, int smtpPort, string smtpUserName, string smtpPassword, string to, string from, string subject, string body, bool isHtml)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            if (isHtml)
                email.Body = new TextPart(TextFormat.Html) { Text = body };
            else
                email.Body = new TextPart(TextFormat.Plain) { Text = body };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(smtpHost, smtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(smtpUserName, smtpPassword);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
