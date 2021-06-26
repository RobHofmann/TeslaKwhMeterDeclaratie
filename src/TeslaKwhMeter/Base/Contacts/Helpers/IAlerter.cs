using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeslaKwhMeter.Base.Contacts.Helpers
{
    public enum AlertPriority
    {
        High,
        Low
    }

    public interface IAlerter
    {
        void Alert(string bodyMessage, AlertPriority priority);
    }
}
