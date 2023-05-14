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
            Controller controller = new Controller();
            controller.Vezerlo();
            Console.ReadKey();
        }
    }
}
