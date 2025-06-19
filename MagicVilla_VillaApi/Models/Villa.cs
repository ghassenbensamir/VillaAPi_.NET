using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_VillaApi.Models
{
    public class Villa
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Occupancy { get; set; }
        public int Sqft { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}