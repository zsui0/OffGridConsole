using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffGridConsole.Objects
{
    internal class Consumption
    {
        public int hour { get; set; }
        public string consumption { get; set; }

        public Consumption(int hour, string consumption)
        {
            this.hour = hour;
            this.consumption = consumption;
        }

        public void Read()
        {
            Console.WriteLine("\n"+hour+" : "+consumption);
        }
    }
}
