using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OffGridConsole.Objects;

namespace OffGridConsole.Interfaces
{
    interface ILoader
    {
        Profiles loadConsumptionProfiles();
    }
}
