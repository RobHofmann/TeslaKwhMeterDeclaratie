using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeslaKwhMeter.Base.Contacts.Helpers;
using TeslaKwhMeter.Base.Contacts.Services.TeslaKwhMeter;
using TeslaKwhMeter.Base.Models.Configuration;
using TeslaKwhMeter.Data;

namespace TeslaKwhMeter
{
    internal class Runner
    {
        // Helpers
        private readonly ILogger _logger;
        private readonly IAlerter _alerter;
        private readonly IMailer _mailer;

        // Services
        private readonly ITeslaKwhMeterService _teslaKwhMeterService;
        
        // Configurations
        private readonly AlertingConfiguration _alertingConfiguration;
        private readonly PrometheusConfiguration _prometheusConfiguration;
        private readonly SmtpConfiguration _smtpConfiguration;
        private readonly EnergieConfiguration _energieConfiguration;

        // Data
        private readonly KwhStandContext _kwhStandContext;


        public Runner(ILogger logger, IAlerter alerter, IMailer mailer, AlertingConfiguration alertingConfiguration,
            ITeslaKwhMeterService teslaKwhMeterService, KwhStandContext kwhStandContext,
            PrometheusConfiguration prometheusConfiguration, SmtpConfiguration smtpConfiguration,
            EnergieConfiguration energieConfiguration)
        {
            _logger = logger;
            _alerter = alerter;
            _mailer = mailer;
            _alertingConfiguration = alertingConfiguration;
            _teslaKwhMeterService = teslaKwhMeterService;
            _kwhStandContext = kwhStandContext;
            _prometheusConfiguration = prometheusConfiguration;
            _smtpConfiguration = smtpConfiguration;
            _energieConfiguration = energieConfiguration;
        }

        public async Task RunAsync()
        {
            try
            {
                _logger.Log("Hello World! Welcome to the Tesla kWh Collector");
                _logger.Log("COLLECT ALL THE THINGS!!!!");
                _logger.Log($"Connecting to SQL Host: {_kwhStandContext.Database.GetDbConnection().DataSource}");
                _logger.Log($"Connecting to SQL Database: {_kwhStandContext.Database.GetDbConnection().Database}");

                DateTime currentDate = DateTime.Now;

                // Ophalen laatste kWh stand uit de database
                var laatsteKwhStand = await _teslaKwhMeterService.GetLaatsteKwhStandAsync(_kwhStandContext);
                _logger.Log($"Laatste Kwh Stand op {laatsteKwhStand.Datum.ToString("dd-MM-yyyy")}: {laatsteKwhStand.StandInKwh}");

                // Huidige (nieuwe) meterstand ophalen uit prometheus
                var huidigeKwhStand = await _teslaKwhMeterService.GetHuidigeKwhStandAsync(_prometheusConfiguration.BaseUrl, _prometheusConfiguration.SensorName);
                _logger.Log($"New Kwh Stand op {currentDate.ToString("dd-MM-yyyy")}: {huidigeKwhStand}");

                var verschilInKwh = huidigeKwhStand - laatsteKwhStand.StandInKwh;
                var teDeclarerenBedrag = Decimal.Round(verschilInKwh * _energieConfiguration.KwHPrijsInEuros, 2);

                // Email met te declareren informatie.
                var mailBody = $"Periode {laatsteKwhStand.Datum.ToString("dd-MM-yyyy")} tot {currentDate.ToString("dd-MM-yyyy")}. Beginstand: {laatsteKwhStand.StandInKwh} kWh. Eindstand: {huidigeKwhStand} kWh. Verschil: {verschilInKwh} kWh. Te Declareren: €{teDeclarerenBedrag}.";
                _logger.Log($"Mail versturen: {mailBody}");
                _mailer.SendMail(_smtpConfiguration.Host, _smtpConfiguration.Port, _smtpConfiguration.Username, _smtpConfiguration.Password
                    , _smtpConfiguration.To, _smtpConfiguration.From, "Nieuwe Tesla Energie Declaratie", mailBody, true);
                _logger.Log($"Mail verstuurd!");

                // Registreren nieuwe kWh stand in de database
                _logger.Log($"Nieuwe kwhStand in de database registreren...");
                await _teslaKwhMeterService.RegistreerNieuweKwhStandAsync(_kwhStandContext, new Base.Models.Data.KwhStand() { Datum = currentDate, StandInKwh = huidigeKwhStand });
                _logger.Log($"Nieuwe kwhStand in de database geregistreerd!");

                _logger.Log("Done!");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                _alerter.Alert(ex.ToString(), AlertPriority.Low);
                throw;
            }
        }
    }
}
