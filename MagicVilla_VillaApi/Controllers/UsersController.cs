using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.Security.User;
using MagicVilla_VillaApi.Models.Security.User.DTO;
using MagicVilla_VillaApi.Repsitory.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaApi.Controllers
{
    [ApiController]

    [Route("api/v{version:apiVersion}/UsersAuth")]
    
    [ApiVersion("1.0")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _db;
        protected ApiResponse _apiResponse;

        public UsersController(IUserRepo db)
        {
            this._db = db;
            _apiResponse = new();

        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            LoginResponseDto loginResponseDto = await _db.Login(loginRequestDto);

            if (string.IsNullOrEmpty(loginResponseDto.Token) || loginResponseDto.User == null)
            {
                _apiResponse.statusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("username or password incorrect");
                return BadRequest(_apiResponse);
            }
            else
            {
                _apiResponse.statusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = loginResponseDto;
                return Ok(_apiResponse);
            }
        }
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            if (await _db.IsUserUnique(registrationRequestDto.UserName) == false)
            {
                _apiResponse.statusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("username already exist");
                return BadRequest(_apiResponse);

            }

            var user = await _db.Register(registrationRequestDto);
            if (user == null)
            {
                _apiResponse.statusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("Error while registring");
                return BadRequest(_apiResponse);
            }

            _apiResponse.statusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;

            return Ok(_apiResponse);

        }
    }
}