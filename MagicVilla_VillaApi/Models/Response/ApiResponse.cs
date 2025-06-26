using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MagicVilla_VillaApi.Models
{
    public class ApiResponse
    {

        public HttpStatusCode statusCode { get; set; }

        public bool IsSuccess { get; set; } = true;

        public List<string> ErrorMessages { get; set; }

        public object Result { get; set; }
        
         public ApiResponse()
        {
            ErrorMessages = new List<string>();
        }
    }
}