using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicVilla_VillaApi.Models.Security.User;
using MagicVilla_VillaApi.Models.Security.User.DTO;

namespace MagicVilla_VillaApi.Repsitory.IRepository
{
    public interface IUserRepo
    {
        Task<bool> IsUserUnique(string username);

        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

        Task<UserDTO> Register(RegistrationRequestDto registrationRequestDto);
    }
}