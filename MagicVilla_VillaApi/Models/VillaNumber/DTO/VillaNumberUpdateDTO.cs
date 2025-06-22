using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_VillaApi.Models.VillaNumber.DTO
{
    public class VillaNumberUpdateDTO
    {
        [Required]
        public int VillaNo { get; set; }

       [Required]
        public int VillaId { get; set; }
        public string SpecialDetails { get; set; }
    }
}