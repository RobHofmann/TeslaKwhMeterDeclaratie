# TeslaKwhMeterDeclaratie
Tool die maandelijks een mail stuurt met de te declareren informatie voor B.V. verrekening t.b.v. elektrisch thuisladen.

De tool leest een sensor uit Prometheus (informatie wordt gescraped uit HomeAssistant) en zet hiervoor een declaratie klaar. Deze declaratie wordt geregistreerd in de database en wordt via de mail verstuurd voor een eenvoudige declaratie via je bank.

# Environment variables needed
| Variable name | Example value | Description |
| ------------- | ------------- | ------------- |
| CRON_EXPRESSION | `0 0 1 * *` | The CRON expression in which frequency to run the script. |
| ConnectionStrings__KwhStandContextConnectionString | `Server=[SERVERADDRESS];Database=[DATABASENAME];User Id=[SQLUSERANME];Password=[SQLPASSWORD];MultipleActiveResultSets=true` | Connectionstring van je MSSQL Database. |
| Prometheus__BaseUrl | `https://mijnprometheusinstance.local` | De BaseURL van je Prometheus instantie |
| Prometheus__SensorName | `sensor.total_tesla_kwh` | Sensorname zoals hij in Promteheus geregistreerd staat. Deze zal worden uitgelezen en gebruikt als energiemeter in kWh. |
| Smtp__Host | `smtp.gmail.com` | SMTP Hostname |
| Smtp__Port | `597` | SMTP Port |
| Smtp__Username | `mijnemail@provider.nl` | SMTP Username |
| Smtp__Password | `mijnsmtppassword` | SMTP Password or SendGrid API Key |
| Smtp__To | `ontvager@provider.nl` | SMTP To Adres |
| Smtp__From | `mijnemail@provider.nl` | SMTP From Adres |
| Energie__KwHPrijsInEuros | `0.22` | De energieprijs in euros |

# How to use
1. Create a MSSQL Database where persistent storage will be.
2. Make sure you filled all the above environment variables.
3. Run :).

## Example
```
docker run --name=tesladeclaratiecollector -e CRON_EXPRESSION='0 0 1 * *' -e ConnectionStrings__KwhStandContextConnectionString="Server=10.0.0.5;Database=TeslaDb;User Id=MyUser;Password=MyPassword;MultipleActiveResultSets=true" -e Prometheus__BaseUrl="https://https://mijnprometheusinstance.local" -e Prometheus__SensorName="sensor.total_tesla_kwh" -e Smtp__Host="smtp.gmail.com" -e Smtp__Port="587" -e Smtp__Username="mijnemail@provider.nl" -e Smtp__Password="mijnsmtppassword" -e Smtp__To="ontvager@provider.nl" -e Smtp__From="mijnemail@provider.nl" -e Energie__KwHPrijsInEuros="0.22" -d robhofmann/tesladeclaratiecollector
```
