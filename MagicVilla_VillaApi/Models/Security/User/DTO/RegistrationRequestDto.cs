using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_VillaApi.Models.Security.User.DTO
{
    public class RegistrationRequestDto
    {

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
        
    }
}