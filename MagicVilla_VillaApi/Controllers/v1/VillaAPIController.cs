using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Logs;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Repsitory.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/VillaAPI")]
    [ApiVersion("1.0")]

    // [Route("api/[controller]")]

    public class VillaAPIController : ControllerBase
    {
        // private readonly ILogger<VillaAPIController> _logger;
        private readonly IVillaRepo _db;
        public IMapper _mapper;

        protected ApiResponse _apiResponse;
        public VillaAPIController(IVillaRepo db, IMapper mapper)
        {
            this._mapper = mapper;
            this._db = db;
            this._apiResponse = new();

        }


        [HttpGet]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ResponseCache(Duration = 30)]
        public async Task<ActionResult<ApiResponse>> GetVillas([FromQuery(Name = "filterOccupancy")] int? Occupancy,
        [FromQuery(Name = "filterName")] string? Name, int pageSize = 3, int pageNumber = 1
        )
        {
            // _logger.LogInformation("Get All Villas");
            try
            {
                IEnumerable<Villa> villas;
                if (Occupancy > 0)
                {
                    villas = await _db.GetAll(u => u.Occupancy == Occupancy, pageSize: pageSize, pageNumber: pageNumber);
                }
                else
                {
                    villas = await _db.GetAll(pageSize: pageSize, pageNumber: pageNumber);
                }
                if (!string.IsNullOrEmpty(Name))
                {
                    villas = villas.Where(v => v.Name.ToLower().Contains(Name));
                }
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _apiResponse.Result = _mapper.Map<IEnumerable<VillaDTO>>(villas);
                _apiResponse.statusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception e)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { e.ToString() };
            }
            return _apiResponse;
        }



        [HttpGet("{id:int}", Name = "GetVilla")]
        [Authorize(Roles = "admin")]
        // [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    // _logger.LogError("error getting villa with id= " + id);
                    return BadRequest();
                }
                var villa = await _db.Get(u => u.Id == id);

                if (villa == null)
                {
                    _apiResponse.statusCode = HttpStatusCode.NotFound;
                    _apiResponse.IsSuccess = false;
                    // _logger.LogWarning("Villa with id " + id + " not found");
                    return NotFound();
                }
                VillaDTO villaDTO = _mapper.Map<VillaDTO>(villa);
                _apiResponse.Result = villaDTO;
                _apiResponse.statusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception e)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { e.ToString() };
            }
            return _apiResponse;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
        {

            try
            {
                if (await _db.Get(v => v.Name.ToLower() == villaDTO.Name.ToLower()) != null)
                {
                    //custom Validation
                    ModelState.AddModelError("UniqueVillaName", "villa already exists");
                    return BadRequest(ModelState);
                }
                if (villaDTO == null)
                {
                    _apiResponse.statusCode = HttpStatusCode.BadRequest;
                    _apiResponse.IsSuccess = false;
                    return BadRequest(_apiResponse);
                }

                Villa villa = _mapper.Map<Villa>(villaDTO);

                await _db.Create(villa);
                VillaCreateDTO villaCreateDTO = _mapper.Map<VillaCreateDTO>(villa);
                _apiResponse.Result = villa;
                _apiResponse.statusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _apiResponse);
            }
            catch (Exception e)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { e.ToString() };
            }
            return _apiResponse;


        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
          [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiResponse.statusCode = HttpStatusCode.BadRequest;
                    _apiResponse.IsSuccess = false;
                    return BadRequest(_apiResponse);
                }
                var villa = await _db.Get(v => v.Id == id);
                if (villa == null)
                {
                    _apiResponse.statusCode = HttpStatusCode.NotFound;
                    _apiResponse.IsSuccess = false;
                    return NotFound(_apiResponse);
                }
                await _db.Remove(villa);
                _apiResponse.statusCode = HttpStatusCode.NoContent;
                _apiResponse.IsSuccess = true;

                return Ok(_apiResponse);
            }
            catch (Exception e)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { e.ToString() };
            }
            return _apiResponse;
        }



        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO)
        {
            try
            {
                if (id == 0 || id != villaDTO.Id)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.statusCode = HttpStatusCode.NoContent;
                    return BadRequest(_apiResponse);
                }
                Villa villa = _mapper.Map<Villa>(villaDTO);

                await _db.Update(villa);
                _apiResponse.statusCode = HttpStatusCode.NoContent;
                _apiResponse.IsSuccess = true;

                return Ok(_apiResponse);
            }
            catch (Exception e)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { e.ToString() };
            }
            return _apiResponse;
        }

        [HttpPatch()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            var villa = await _db.Get(v => v.Id == id, false);
            if (villa == null)
            {
                return NotFound();
            }
            VillaUpdateDTO model = _mapper.Map<VillaUpdateDTO>(patchDto);

            patchDto.ApplyTo(model, ModelState);
            Villa villa1 = _mapper.Map<Villa>(model);

            await _db.Update(villa1);
            await _db.Save();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return NoContent();
        }

[HttpGet("debug-auth")]
public IActionResult DebugAuth()
{
    var user = HttpContext.User;
    var claims = user.Claims.Select(c => new { Type = c.Type, Value = c.Value }).ToList();
    
    var roleClaims = user.Claims.Where(c => 
        c.Type == ClaimTypes.Role || 
        c.Type == "role" || 
        c.Type == "roles" ||
        c.Type.Contains("role")).ToList();
    
    return Ok(new 
    { 
        IsAuthenticated = user.Identity.IsAuthenticated,
        UserName = user.Identity.Name,
        ClaimTypesRole = ClaimTypes.Role,
        AllClaims = claims,
        RoleClaims = roleClaims.Select(c => new { c.Type, c.Value }),
        IsInAdminRole = user.IsInRole("admin"),
        IsInAdminRoleExplicit = user.HasClaim(ClaimTypes.Role, "admin"),
        IsInAdminRoleCustom = user.HasClaim("role", "admin")
    });
}

    }
}