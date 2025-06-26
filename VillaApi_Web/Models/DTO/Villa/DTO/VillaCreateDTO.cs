using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VillaApi_Web.Models
{
    public class VillaCreateDTO
    {
      
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public string Details { get; set; }

        [Required]
        public double Rate { get; set; }

        public string ImageUrl { get; set; }
        public string Amenity { get; set; }

        public int Occupancy { get; set; }
        public int Sqft { get; set; }


        
        }

    }
