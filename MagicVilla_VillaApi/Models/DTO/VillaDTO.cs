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
        public string Details { get; set; }

        [Required]
        public double Rate { get; set; }

        public string ImageUrl { get; set; }
        public string Amenity { get; set; }

        public int Occupancy { get; set; }
        public int Sqft { get; set; }


        public static  Villa VillaMapper(VillaDTO villaDTO)
        {
            return new Villa
            {
                Id=villaDTO.Id,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Rate = villaDTO.Rate,
                ImageUrl = villaDTO.ImageUrl,
                Amenity = villaDTO.Amenity,
                Occupancy = villaDTO.Occupancy,
                Sqft = villaDTO.Sqft
            };
        }

    }
}