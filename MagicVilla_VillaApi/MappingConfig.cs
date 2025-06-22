using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.VillaNumber;
using MagicVilla_VillaApi.Models.VillaNumber.DTO;

namespace MagicVilla_VillaApi
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>().ReverseMap();
            CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
            CreateMap<Villa, VillaCreateDTO>().ReverseMap();


            CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberDto>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
            CreateMap<VillaNumberDto, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumberDto, VillaNumberUpdateDTO>().ReverseMap();

        }
    }
}