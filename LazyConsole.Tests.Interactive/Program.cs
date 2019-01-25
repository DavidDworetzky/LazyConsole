using LazyConsole.Tests.Interactive.Consoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyConsole.Tests.Interactive
{
    class Program
    {
        static void Main(string[] args)
        {
            LazyConsole.StartConsole(typeof(AsyncConsole));
        }
    }
}