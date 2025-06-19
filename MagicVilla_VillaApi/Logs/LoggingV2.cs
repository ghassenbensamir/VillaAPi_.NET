using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_VillaApi.Logs
{
    public class LoggingV2 : ILogging
    {
        public void Log(string message, string type)
        {
            if (type == "error")
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Error- " + message);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(message);
                Console.BackgroundColor = ConsoleColor.Black;
            }

        }
    }
}