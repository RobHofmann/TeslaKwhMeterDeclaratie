using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeslaKwhMeter.Base.Contacts.Helpers;

namespace TeslaKwhMeter.Utils.Helpers.Loggers
{
    internal class ConsoleLogger : ILogger
    {
        public void Log(string logEntry, params string[] prefixes)
        {
            Console.WriteLine($"[{DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss")}]{string.Join("", prefixes.Select(p => $"[{p}]"))} {logEntry}");
        }

        public void Error(string logEntry, params string[] prefixes)
        {
            Console.WriteLine($"[ERROR][{DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss")}]{string.Join("", prefixes.Select(p => $"[{p}]"))} {logEntry}");
        }

    }
}
