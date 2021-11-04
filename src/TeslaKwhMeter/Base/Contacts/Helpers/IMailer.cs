using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeslaKwhMeter.Base.Contacts.Helpers
{
    public interface IMailer
    {
        Task SendMail(string smtpHost, int smtpPort, string smtpUserName, string smtpPassword, string to, string from, string subject, string body, bool isHtml);
    }
}
