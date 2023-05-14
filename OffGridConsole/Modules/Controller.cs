using OffGridConsole.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OffGridConsole.Objects;

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
                driver.SendCommand(dataCollector.getEnergyData(12).token, new Dictionary<string, string>(), "ERROR"); // dict üres
            }
            else
            {
                //a megfelelő eszközök kikapcsolása, parancsok kiküldése a IDrive-nak

                driver.SendCommand(dataCollector.getEnergyData(12).token, new Dictionary<string, string>(), "SUCCESS");
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
        public Dictionary<string, string> CommandDictionary(List<CleanData> cd) {
            return new Dictionary<string, string>();
        }
    }
}
