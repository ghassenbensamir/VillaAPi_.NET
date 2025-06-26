using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VillaApi_Web.Models;

namespace VillaApi_Web.Services.IServices
{
    public interface IBaseService
    {
        ApiResponse ApiResponse { get; set; }

        Task<T> SendAsync<T>(ApiRequest apiRequest);
        
    }
}