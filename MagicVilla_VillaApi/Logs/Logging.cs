using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_VillaApi.Logs
{
    public class Logging : ILogging
    {
        public void Log(string message, string type)
        {
            if (type == "error")
            {
                Console.WriteLine("Error -" + message);
            }

            else
            {
                Console.WriteLine(message);
            }
        }
    }
}