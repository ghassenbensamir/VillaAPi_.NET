using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.Security.User;
using MagicVilla_VillaApi.Models.Security.User.DTO;
using MagicVilla_VillaApi.Repsitory.IRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MagicVilla_VillaApi.Repsitory.User
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDBContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private string secretKey;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ILogger<UserRepo> _logger;


        private readonly IMapper _mapper;

        public UserRepo(ApplicationDBContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager, ILogger<UserRepo> logger)
        {
            this._db = db;
            this._userManager = userManager;
            this.secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _mapper = mapper;
            this._roleManager = roleManager;
            _logger = logger;
        }

        public  async Task<bool> IsUserUnique(string username)
        {
            if ( await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == username)!=null)
                return false;
            return true;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == loginRequestDto.UserName);
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);


            if (user == null||isValid==false)
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    User = null
                };
            }

            var token = await GenerateToken(user);
           var userDto= _mapper.Map<UserDTO>(user);
            LoginResponseDto loginResponseDto = new()
            {
                Token = token,
                User = userDto,

            };

            return loginResponseDto;
            
        }

        private   async Task<string> GenerateToken(ApplicationUser user)
        {
            Console.WriteLine("=== GENERATING TOKEN ===");
            Console.WriteLine($"Secret Key: {secretKey}");
            Console.WriteLine($"User: {user.UserName}");
    
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var roles =  await _userManager.GetRolesAsync(user);
             Console.WriteLine($"Roles found: {string.Join(", ", roles)}");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name,user.UserName.ToString()),
                    new Claim(ClaimTypes.Role,roles.FirstOrDefault())
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
             
    Console.WriteLine($"Generated Token: {token}");
    Console.WriteLine("=== TOKEN GENERATION COMPLETE ===");
            return tokenHandler.WriteToken(token);
        }

        public async Task<UserDTO> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.UserName,
                Name = registrationRequestDto.Name,
                Email = registrationRequestDto.UserName,
                NormalizedEmail = registrationRequestDto.UserName.ToUpper()

            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                    await _userManager.AddToRoleAsync(user, "admin");
                    var userToReturn = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == registrationRequestDto.UserName);
                    return _mapper.Map<UserDTO>(user);
                }
                else
                {
                    _logger.LogError("error",result.Errors);
                }
                
            }
            catch (Exception e)
            {


            }

            return null;
        }
        
    }
}