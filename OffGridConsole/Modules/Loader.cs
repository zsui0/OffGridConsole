using OffGridConsole.Interfaces;
using OffGridConsole.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json.Linq;

namespace OffGridConsole.Modules
{
    internal class Loader : ILoader
    {
        public Profiles loadConsumptionProfiles()
        {
            Profiles profiles = new Profiles();

            var json = new WebClient().DownloadString("http://193.6.19.58:2023/offgrid/input");

            JObject jObject = JObject.Parse(json);

            int length = jObject["profiles"].Count();
            
            for (int i = 0; i < length; i++)
            {
                JToken jProfile = jObject["profiles"][i];

                List<Consumption> consumptions = new List<Consumption>();
                int conLength = jProfile["consumptions"].Count();
                for (int j = 0; j < conLength; j++)
                {
                    consumptions.Add(new Consumption((int)jProfile["consumptions"][j]["hour"],(string)jProfile["consumptions"][j]["consumption"]));
                }
                Profile nPf = new Profile((string)jProfile["cId"],(string)jProfile["type"],(int)jProfile["priority"],consumptions);
                // adatok jól vannak elmentve
                profiles.AppendList(nPf);
            }

            return profiles;
        }
    }
}
