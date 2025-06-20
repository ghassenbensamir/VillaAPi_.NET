using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Logs;
using MagicVilla_VillaApi.Models;
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
        private readonly ApplicationDBContext _db;
        public IMapper _mapper;

        public VillaAPIController(ApplicationDBContext db, IMapper mapper )
        {
            this._mapper = mapper;
            this._db = db;
                    
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas(){
            // _logger.LogInformation("Get All Villas");
            IEnumerable<Villa> villas =await _db.Villas.ToListAsync();

            return Ok( _mapper.Map<VillaDTO>(villas));
        }


        [HttpGet("{id:int}", Name = "GetVilla")]
        // [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0)
            {
                // _logger.LogError("error getting villa with id= " + id);
                return BadRequest();
            }
            var villa = await _db.Villas.SingleOrDefaultAsync(u => u.Id == id);
            if (villa == null)
            {
                // _logger.LogWarning("Villa with id " + id + " not found");
                return NotFound();
            }

            return Ok(villa);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
        {
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest();
            // }


            //custom Validation
            if (await _db.Villas.FirstOrDefaultAsync(v => v.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                //custom Validation
                ModelState.AddModelError("UniqueVillaName", "villa already exists");
                return BadRequest(ModelState);
            }
            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }

           Villa model= _mapper.Map<Villa>(villaDTO);
            
            await _db.Villas.AddAsync(model);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);


        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa =await  _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
           _db.Villas.Remove(villa);
           await _db.SaveChangesAsync();
            return NoContent();
        }



        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO) {

            if (id == 0 || id!=villaDTO.Id)
            {
                return BadRequest();
            }
            // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            // if (villa == null)
            // {
            //     return NotFound();
            // }

            // // villa.Name = villaDTO.Name;
            // // villa.Occupancy = villaDTO.Occupancy;
            // // villa.Sqft = villaDTO.Sqft;
            Villa model = _mapper.Map<Villa>(villaDTO);
           
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            return NoContent();
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
            var villa =await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            VillaUpdateDTO model = _mapper.Map<VillaUpdateDTO>(patchDto);
            
            patchDto.ApplyTo(model, ModelState);
            Villa villa1=_mapper.Map<Villa>(model);
            
            _db.Villas.Update(villa1);
            await _db.SaveChangesAsync();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}