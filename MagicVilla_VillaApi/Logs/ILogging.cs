using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_VillaApi.Logs
{
    public interface ILogging
    {
        public void Log(string message, string type);
    }
}