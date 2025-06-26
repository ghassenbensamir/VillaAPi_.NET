using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_VillaApi.Models.Security.User.DTO
{
    public class LoginResponseDto
    {
        public UserDTO User {get;set;}
        
        // public string Roles { get; set; }
        public string Token { get; set; }
    }
}