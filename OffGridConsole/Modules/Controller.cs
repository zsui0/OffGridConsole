using OffGridConsole.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OffGridConsole.Objects;
using System.IO;

using System.Net;
using Newtonsoft.Json.Linq;

namespace OffGridConsole.Modules
{
    struct CleanData 
    {
        public string cId { get; set; }
        public string type { get; set; }
        public int priority { get; set; }
        public Consumption Consumption { get; set; }
    }

    internal class Controller
    {
        ILoader loader = new Loader();
        IDataCollector dataCollector = new DataCollector(); // Battery és Energy adatok készen
        IDriver driver = new Driver();

        /*
         2.1 1p
         1.8 2p
         0.5 3p
         1.2 3p
         */

        public void Vezerlo() {
            // készítsük elő a 12 órához tartozó adatokat
            List<CleanData> CleanData = CleanConsumptionByHour(12);
            double AvailableEnergy = dataCollector.getEnergyData(12).AvailableEnergy(); // 12 órakor az elérhető energia (battery + expected)
            if (MaxConsumption(CleanData) <= AvailableEnergy)
            {
                // üres parancsok kiküldése a IDrive-nak
                driver.SendCommand(dataCollector.getEnergyData(12).token,new Dictionary<string, string>(),"SUCCESS"); // dict üres
            }
            else if(FirstPriorityConsumption(CleanData) > AvailableEnergy)
            {
                /*
                  a kontroller egy szöveges fájlba napló bejegyzést készít, ahol
                    megadja, mikor történt az eset és mekkora energia igényt kellett lefedni az 1. prioritású
                    csoport tagjainak kikapcsolásával. Ebben az esetben is szükséges egy üres parancs
                    kiküldése, viszont már „Error” státusszal.
                 */
                FileStream stream = null;
                try
                {
                    stream = new FileStream("naplo.txt", FileMode.CreateNew);
                    // Create a StreamWriter from FileStream  
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.WriteLine("Leállás ideje: " + CleanData[0].Consumption.hour);
                        writer.WriteLine("1. prioritású csoport energia igénye: " + FirstPriorityConsumption(CleanData));
                        writer.WriteLine("A leállás idejében elérhető energia: " + AvailableEnergy);
                    }
                }
                finally
                {
                    if (stream != null) stream.Close();
                }
                driver.SendCommand(dataCollector.getEnergyData(12).token, new Dictionary<string, string>(), "ERROR"); // dict üres
            }
            else
            {
                //a megfelelő eszközök kikapcsolása, parancsok kiküldése a IDrive-nak

                driver.SendCommand(dataCollector.getEnergyData(12).token, CommandDictionary(CleanData, MaxConsumption(CleanData), AvailableEnergy), "SUCCESS");
            }
        }

        public List<CleanData> CleanConsumptionByHour(int hour)
        {
            List<CleanData> cleanData = new List<CleanData>();
            foreach (Profile pf in loader.loadConsumptionProfiles().ProfileList)
            {
                foreach (Consumption cp in pf.consumptions)
                {
                    if (cp.hour == hour)
                    {
                        CleanData cleanData2 = new CleanData();
                        cleanData2.cId = pf.cId;
                        cleanData2.type = pf.type;
                        cleanData2.priority = pf.priority;
                        cleanData2.Consumption = cp;
                        cleanData.Add(cleanData2);
                    }
                }
            }
            return cleanData;
        }
        public double MaxConsumption(List<CleanData> cd)
        {
            double maxConsumption = 0;
            foreach (CleanData item in cd)
            {
                maxConsumption += item.Consumption.ConvertConsumption();
            }
            return maxConsumption;
        }
        public double FirstPriorityConsumption(List<CleanData> cd)
        {
            double firstConsumption = 0;
            foreach (CleanData item in cd)
            {
                if (item.priority == 1) { 
                    firstConsumption += item.Consumption.ConvertConsumption();
                }
                
            }
            return firstConsumption;
        }
        public Dictionary<string, string> CommandDictionary(List<CleanData> cdl, double maxc, double ae) {
            Dictionary<string, string> dict = new Dictionary<string, string>();


            var json = new WebClient().DownloadString("http://193.6.19.58:2023/offgrid/typecommands");
            JObject jCommandTypes = JObject.Parse(json);
            //JToken jtCommandTypes = jCommandTypes.ToObject<JToken>();

            bool runnable = false;
            foreach (CleanData cd in cdl) {
                if (cd.priority == 3) {
                    if ((maxc - cd.Consumption.ConvertConsumption()) <= ae) {
                        dict.Add(cd.cId, ChangeToNumberCommand(jCommandTypes, cd.type));
                        runnable = true;
                    }
                }
                if (runnable) break;
            }
            return dict;
        }
        public string ChangeToNumberCommand(JObject commands, string type) {
            foreach (var c in commands) {
                if (c.Key == type) {
                    return Convert.ToString(c.Value);
                }
            }
            return "";
        }
    }
}
