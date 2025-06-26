using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VillaApi_Web.Models;
using VillaApi_Web.Services.IServices;

namespace VillaApi_Web.Controllers
{
    [Route("Villa")]
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }

        [HttpGet("IndexVilla")]
        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> villas = new();
            var response = await _villaService.GetAllAsync<ApiResponse>();
            if (response != null && response.IsSuccess == true)
            {
                villas = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(villas);
        }

        [HttpGet("CreateVilla")]
        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }

        [HttpPost("CreateVilla")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO villa)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.CreateAsync<ApiResponse>(villa);
                if (response != null && response.IsSuccess == true)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }

            return View(villa);
        }
        
        [HttpPost("UpdateVilla")]
        public async Task<IActionResult> UpdateVilla(int id)
        {
            var response = await _villaService.GetAsync<ApiResponse>(id);
            if (response != null && response.IsSuccess == true)
            {
                    return RedirectToAction(nameof(IndexVilla));
            }
            return View();
        }

    }
}