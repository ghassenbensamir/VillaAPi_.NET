using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VillaApi_Web.Models;

namespace VillaApi_Web.Services.IServices
{
    public interface IVillaService
    {
        
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(VillaCreateDTO villa);
        
        Task<T> DeleteAsync<T>(int id);

        Task<T> UpdateAync<T>(VillaUpdateDTO villa);



    }
}