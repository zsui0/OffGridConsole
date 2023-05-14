using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffGridConsole.Objects
{
    internal class Profile
    {
        public string cId { get; set; }
        public string type { get; set; }
        public int priority { get; set; }
        public List<Consumption> consumptions { get; set; }

        public Profile(string cId, string type, int priority, List<Consumption> consumptions)
        {
            this.cId = cId;
            this.type = type;
            this.priority = priority;
            this.consumptions = consumptions;
        }

        public void ReadProfileData()
        {
            Console.WriteLine(" "+cId + " " + type + " " + priority + " ");
        }

        public void ReadConsumptions()
        {
            foreach(Consumption cp in consumptions)
            {
                cp.Read();
            }
        }
    }
}
