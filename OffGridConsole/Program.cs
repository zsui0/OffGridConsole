using OffGridConsole.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OffGridConsole.Modules;
using OffGridConsole.Objects;

namespace OffGridConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ILoader loader = new Loader();
            Profiles pf = new Profiles();
            pf = loader.loadConsumptionProfiles();
            pf.ReadData();
            Console.ReadKey();
        }
    }
}
