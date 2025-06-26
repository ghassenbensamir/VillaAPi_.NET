using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using VillaApi_Web.Models;
using VillaApi_Web.Models.VillaNumber.DTO;

namespace VillaApi_Web
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
            CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();
            CreateMap<VillaNumberDto, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumberDto, VillaNumberUpdateDTO>().ReverseMap();

        }
    }
}