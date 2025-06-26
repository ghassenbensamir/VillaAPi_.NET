using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagicVilla_Utility;
using Newtonsoft.Json;
using VillaApi_Web.Models;
using VillaApi_Web.Services.IServices;

namespace VillaApi_Web.Services
{
    public class BaseService : IBaseService
    {
        public ApiResponse ApiResponse { get ; set ; }
        public IHttpClientFactory httpClient;

        public BaseService(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
             this.ApiResponse = new();
        }


        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MagicApi");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                }
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.GET:
                        message.Method = HttpMethod.Get;
                        break;

                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;

                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;

                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;

                    default:
                        message.Method = HttpMethod.Get;
                        break;

                }

                HttpResponseMessage response = null;

                response = await client.SendAsync(message);

                var apiContent = await response.Content.ReadAsStringAsync();
                var APIresponse = JsonConvert.DeserializeObject<T>(apiContent);

                return APIresponse;
            }
            catch (Exception e)
            {
                var dto = new ApiResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIresponse = JsonConvert.DeserializeObject<T>(res);
                return APIresponse;
            }
        }
    }
}