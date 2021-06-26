using Microsoft.EntityFrameworkCore;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TeslaKwhMeter.Base.Contacts.Services.TeslaKwhMeter;
using TeslaKwhMeter.Base.Models.Data;
using TeslaKwhMeter.Base.Models.Prometheus;
using TeslaKwhMeter.Data;

namespace TeslaKwhMeter.Services.TeslaKwhMeter
{
    public class TeslaKwhMeterService : ITeslaKwhMeterService
    {
        public async Task<decimal> GetHuidigeKwhStandAsync(string baseUrl, string sensorName)
        {
            var client = new RestClient(string.Format("{0}/api/v1/query?query=hassio_sensor_unit_kwh{{entity=%22{1}%22}}", baseUrl, sensorName));
            IRestResponse response = await client.ExecuteAsync(new RestRequest(Method.GET));
            var result = JsonSerializer.Deserialize<QueryResult>(response.Content);

            decimal? kwh = decimal.Parse(Convert.ToString(result.data.result.First().value[1]), CultureInfo.InvariantCulture);

            if (result == null || kwh == null)
                throw new NullReferenceException("Oops! Result was null :(");

            return (decimal)kwh;
        }

        public async Task<KwhStand> GetLaatsteKwhStandAsync(KwhStandContext kwhStandContext)
        {
            return await kwhStandContext.KwhStands.OrderByDescending(kwhStand => kwhStand.Datum).FirstOrDefaultAsync();
        }

        public async Task RegistreerNieuweKwhStandAsync(KwhStandContext kwhStandContext, KwhStand kwhStand)
        {
            await kwhStandContext.KwhStands.AddAsync(kwhStand);
            await kwhStandContext.SaveChangesAsync();
        }
    }
}
