using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.VillaNumber;
using MagicVilla_VillaApi.Models.VillaNumber.DTO;
using MagicVilla_VillaApi.Repsitory.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MagicVilla_VillaApi.Controllers
{
    [Route("[controller]")]
    public class VillaNumberAPIController : Controller
    {
        public IVillaNumberRepo _db { get; }

        public IVillaRepo _villaDb;
        public IMapper _mapper { get; set; }

        protected ApiResponse _response;
        public VillaNumberAPIController(IVillaNumberRepo db, IMapper _mapper, IVillaRepo villaRepo)
        {
            this._mapper = _mapper;
            this._db = db;
            this._response = new();
            this._villaDb = villaRepo;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetAll()
        {

            try
            {
                IEnumerable<VillaNumber> villasNo = await _db.GetAll();
                _response.Result = _mapper.Map<IEnumerable<VillaNumberDto>>(villasNo);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpGet("{villaNo:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetVillaNumber(int villaNo)
        {
            try
            {
                if (villaNo == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                VillaNumber villaNumber = await _db.Get(u => u.VillaNo == villaNo);

                if (villaNumber == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }
                VillaNumberDto villaNumberDto = _mapper.Map<VillaNumberDto>(villaNumber);
                _response.Result = villaNumberDto;
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpPost(Name ="CreateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaNoDTO)
        {
            try
            {
                if (await _db.Get(v => v.VillaNo == villaNoDTO.VillaNo) != null)
                    {
                        //custom Validation
                        ModelState.AddModelError("UniqueVillaName", "villa already exists");
                        return BadRequest(ModelState);
                    }
                if (await _villaDb.Get(v => v.Id == villaNoDTO.VillaId) == null)
                    {
                        //custom Validation
                        ModelState.AddModelError("VillaIdNotValid", "the villa id is not valid");
                        return BadRequest(ModelState);
                    }
                if (villaNoDTO == null)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                VillaNumber villaNo = _mapper.Map<VillaNumber>(villaNoDTO);

                await _db.Create(villaNo);
                VillaNumberDto dto = _mapper.Map<VillaNumberDto>(villaNo);
                _response.Result = dto;
                _response.statusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = dto.VillaNo }, _response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return _response;


        }
        
        [HttpDelete("{villaNo:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> DeleteVillaNumber(int villaNo)
        {
            try
            {
                if (villaNo == 0)
                {
                    return BadRequest();
                }
                var villaNumber = await _db.Get(v => v.VillaNo == villaNo);
                if (villaNumber == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _db.Remove(villaNumber);
                _response.statusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);
            }
             catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return _response; 
        }

        [HttpPut("{villaNo:int}",Name ="UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> UpdateVillaNumber(int villaNo, [FromBody] VillaNumberUpdateDTO villaNoUpdateDTO) {
            try
            {
                if (villaNo == 0 || villaNo != villaNoUpdateDTO.VillaNo)
                {
                    _response.statusCode = HttpStatusCode.NoContent;
                    return BadRequest(_response);
                }
                if (await _villaDb.Get(v => v.Id == villaNoUpdateDTO.VillaId) == null)
                    {
                        //custom Validation
                        ModelState.AddModelError("VillaIdNotValid", "the villa id is not valid");
                        return BadRequest(ModelState);
                    }
                VillaNumber villaNumber = _mapper.Map<VillaNumber>(villaNoUpdateDTO);

                await _db.Update(villaNumber);
                _response.statusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);
            }
             catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return _response; 
        }
        
    }
    }
