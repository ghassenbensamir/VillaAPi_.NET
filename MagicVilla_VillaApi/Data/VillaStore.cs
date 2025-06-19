using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicVilla_VillaApi.Models;

namespace MagicVilla_VillaApi.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
        {
            new VillaDTO { Id = 1, Name = "Villa1", Occupancy = 4, Sqft = 11 },
            new VillaDTO { Id = 2, Name = "Villa2", Occupancy = 2, Sqft = 3 },
            new VillaDTO { Id = 3, Name = "Villa3", Occupancy = 5, Sqft = 8 },
            new VillaDTO { Id = 4, Name = "Villa4", Occupancy = 2, Sqft = 1 }
        };
    }
}