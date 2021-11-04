using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeslaKwhMeter.Base.Contacts.Helpers;

namespace TeslaKwhMeter.Services.Mailers
{
    internal class SendGridMailer : IMailer
    {
        public async Task SendMail(string smtpHost, int smtpPort, string smtpUserName, string smtpPassword, string to, string from, string subject, string body, bool isHtml)
        {
            var client = new SendGridClient(smtpPassword);
            var fromAddress = new EmailAddress(from, "Tesla kWh Meter");
            var toAddress = new EmailAddress(to);
            var msg = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, body, body);
            await client.SendEmailAsync(msg);
        }
    }
}
