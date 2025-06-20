using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MagicVilla_VillaApi.Models;
using Microsoft.EntityFrameworkCore.Update;

namespace MagicVilla_VillaApi.Repsitory.IRepository
{
    public interface IVillaRepo:IRepository<Villa>
    {

        Task<Villa> Update(Villa villa);
        
    }
}