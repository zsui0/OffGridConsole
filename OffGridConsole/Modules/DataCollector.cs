using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json.Linq;

using OffGridConsole.Interfaces;
using OffGridConsole.Objects;

namespace OffGridConsole.Modules
{
    internal class DataCollector : IDataCollector
    {
        public EnergyData getEnergyData(int hour)
        {
            var WeatherService = new WebClient().DownloadString("http://193.6.19.58:2023/offgrid/weather/12");
            var BatteryService = new WebClient().DownloadString("http://193.6.19.58:2023/offgrid/batteries/12");

            JObject jWeather = JObject.Parse(WeatherService);
            JObject jBattery = JObject.Parse(BatteryService);

            JToken jtWeather = jWeather;
            //JToken jtBattery = jBattery["batteries"];

            double expectedEnergy = CalculateExpected(jtWeather);
            double storedEnergy = 0D;

            int batteriesLength = jBattery["batteries"].Count();


            for (int i = 0; i < batteriesLength; i++)
            {
                JToken jtBattery = jBattery["batteries"][i];
                storedEnergy += (double)jtBattery["kWh"] * ((int)jtBattery["level"] / 100);
            }


            return new EnergyData(expectedEnergy, storedEnergy, (string)jBattery["token"]);
        }

        public double CalculateExpected(JToken jw)
        {
            double expectedEnergy = (24 * 0.156 * 0.62) * (double)jw["irradiation"];
            return expectedEnergy; 
        }
    }
}
