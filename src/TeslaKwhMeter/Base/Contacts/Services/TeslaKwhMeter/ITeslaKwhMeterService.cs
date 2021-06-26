using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeslaKwhMeter.Base.Models.Data;
using TeslaKwhMeter.Data;

namespace TeslaKwhMeter.Base.Contacts.Services.TeslaKwhMeter
{
    internal interface ITeslaKwhMeterService
    {
        Task<decimal> GetHuidigeKwhStandAsync(string baseUrl, string sensorName);

        Task<KwhStand> GetLaatsteKwhStandAsync(KwhStandContext kwhStandContext);

        Task RegistreerNieuweKwhStandAsync(KwhStandContext kwhStandContext, KwhStand kwhStand);

    }
}
