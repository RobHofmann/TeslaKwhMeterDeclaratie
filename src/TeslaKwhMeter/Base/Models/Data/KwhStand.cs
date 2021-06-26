using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeslaKwhMeter.Base.Models.Data
{
    public class KwhStand
    {
        public long Id { get; set; }
        public DateTime Datum { get; set; }
        public decimal StandInKwh { get; set; }
    }
}
