using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicVilla_Utility;
using VillaApi_Web.Models;
using VillaApi_Web.Services.IServices;

namespace VillaApi_Web.Services.VillAServices
{
    public class VillaService : BaseService,IVillaService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string villaUrl;

        public VillaService(IHttpClientFactory httpClient, IConfiguration config) : base(httpClient)
        {
            _clientFactory = httpClient;
            this.villaUrl = config.GetValue<string>("ServicesUrl:Magic_VillaAPI");
        }

        public Task<T> CreateAsync<T>(VillaCreateDTO villa)
        {
            return SendAsync<T>(new ApiRequest
                {
                    ApiType = SD.ApiType.POST,
                    Url = villaUrl + "/api/VillaAPI",
                    Data = villa

                });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new ApiRequest
                {
                    ApiType = SD.ApiType.DELETE,
                    Url = villaUrl + "/api/VillaAPI/"+id,
                   

                });     
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new ApiRequest
                {
                    ApiType = SD.ApiType.GET,
                    Url = villaUrl + "/api/VillaAPI",
                   
                });     
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new ApiRequest
                {
                    ApiType = SD.ApiType.GET,
                    Url = villaUrl + "/api/VillaAPI/"+id,
                    

                });
        }

        public Task<T> UpdateAync<T>(VillaUpdateDTO villa)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.PUT,
                Url = villaUrl + "/api/VillaAPI/"+villa.Id,
                Data = villa

            });
        }
    }
}