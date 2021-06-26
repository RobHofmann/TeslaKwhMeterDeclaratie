using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeslaKwhMeter.Base.Contacts.Helpers
{
    public interface ILogger
    {
        void Log(string logEntry, params string[] prefixes);

        void Error(string logEntry, params string[] prefixes);
    }
}
