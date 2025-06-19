using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_VillaApi.Models
{
    public class VillaDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public int Occupancy { get; set; }
        public int Sqft { get; set; }


    }
}