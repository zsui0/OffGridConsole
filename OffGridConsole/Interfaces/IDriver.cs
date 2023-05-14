using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffGridConsole.Interfaces
{
    interface IDriver
    {
        int SendCommand(string token,Dictionary<string, string> consumers,string status);
    }
}
