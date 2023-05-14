using OffGridConsole.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffGridConsole.Interfaces
{
    interface IDataCollector
    {
        EnergyData getEnergyData(int hour);
    }
}
