using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Logs;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Repsitory.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaApi.Controllers
{
    [Route("api/VillaAPI")]
    // [Route("api/[controller]")]
    [ApiController]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetVillas() {
            // _logger.LogInformation("Get All Villas");
            try
            {
                IEnumerable<Villa> villas = await _db.GetAll();
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
        // [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                    // _logger.LogWarning("Villa with id " + id + " not found");
                    return NotFound();
                }
                _apiResponse.Result = villa;
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
                    return BadRequest(_apiResponse);
                }

                Villa villa = _mapper.Map<Villa>(villaDTO);

                await _db.Create(villa);
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villa = await _db.Get(v => v.Id == id);
                if (villa == null)
                {
                    _apiResponse.statusCode = HttpStatusCode.NotFound;
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO) {
            try
            {
                if (id == 0 || id != villaDTO.Id)
                {
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
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDto){
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            var villa =await _db.Get(v => v.Id == id,false);
            if (villa == null)
            {
                return NotFound();
            }
            VillaUpdateDTO model = _mapper.Map<VillaUpdateDTO>(patchDto);
            
            patchDto.ApplyTo(model, ModelState);
            Villa villa1=_mapper.Map<Villa>(model);
            
            await _db.Update(villa1);
            await _db.Save();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}