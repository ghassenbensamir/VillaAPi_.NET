using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicVilla_VillaApi.Models.VillaNumber;

namespace MagicVilla_VillaApi.Repsitory.IRepository
{
    public interface IVillaNumberRepo:IRepository<VillaNumber>
    {
        Task<VillaNumber> Update(VillaNumber villaNumber);
    }
}