using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;
using TeslaKwhMeter.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeslaKwhMeter.Base.Contacts.Helpers;
using TeslaKwhMeter.Utils.Helpers.Loggers;
using TeslaKwhMeter.Base.Contacts.Services.TeslaKwhMeter;
using TeslaKwhMeter.Services.TeslaKwhMeter;
using TeslaKwhMeter.Base.Models.Configuration;
using Microsoft.Extensions.Options;
using TeslaKwhMeter.Utils.Helpers.Alerters;
using Microsoft.EntityFrameworkCore;
using TeslaKwhMeter.Services.Mailers;

namespace TeslaKwhMeter
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<ILogger, ConsoleLogger>();
            services.AddSingleton<ITeslaKwhMeterService, TeslaKwhMeterService>();
            services.AddSingleton<IMailer, SendGridMailer>();
            services.AddSingleton<Runner>();
            services.AddSingleton<IConfiguration>(configuration);

            services.AddDbContext<KwhStandContext>(options => options.UseSqlServer(configuration.GetConnectionString("KwhStandContextConnectionString")));

            #region Configuration registering
            services.AddSingleton<AlertingConfiguration>();
            services.Configure<AlertingConfiguration>(configuration.GetSection("Alerting"));
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AlertingConfiguration>>().Value);

            services.AddSingleton<PrometheusConfiguration>();
            services.Configure<PrometheusConfiguration>(configuration.GetSection("Prometheus"));
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<PrometheusConfiguration>>().Value);

            services.AddSingleton<SmtpConfiguration>();
            services.Configure<SmtpConfiguration>(configuration.GetSection("Smtp"));
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<SmtpConfiguration>>().Value);

            services.AddSingleton<EnergieConfiguration>();
            services.Configure<EnergieConfiguration>(configuration.GetSection("Energie"));
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<EnergieConfiguration>>().Value);
            #endregion Configuration registering

            var serviceProvider = services.BuildServiceProvider();

            var alertingConfiguration = serviceProvider.GetService<AlertingConfiguration>();
            switch (alertingConfiguration.Type)
            {
                default:
                case "OpsGenie":
                    services.AddSingleton<IAlerter, OpsGenieAlerter>();
                    break;
            }

            serviceProvider = services.BuildServiceProvider();
            var runner = serviceProvider.GetService<Runner>();
            await runner.RunAsync();
        }
    }
}
