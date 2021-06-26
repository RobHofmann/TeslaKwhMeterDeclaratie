using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeslaKwhMeter.Base.Models.Configuration
{
    public class PrometheusConfiguration
    {
        public string BaseUrl { get; set; }
        public string SensorName { get; set; }
    }
}
