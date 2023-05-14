using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffGridConsole.Objects
{
    internal class EnergyData
    {
        public double expected { get; set; }
        public double battery { get; set; }
        public string token { get; set; }

        public EnergyData(double expected, double battery, string token)
        {
            this.expected = expected;
            this.battery = battery;
            this.token = token;
        }

        public double AvailableEnergy() {
            return expected + battery;
        }
    }
}
