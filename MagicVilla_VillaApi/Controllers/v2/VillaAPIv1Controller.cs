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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaApi.Controllers.v2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/VillaAPI")]
    [ApiVersion("2.0")]
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
        [MapToApiVersion("2.0")]
        public List<string> Get()
        {
            return
            [
                "v1","LoggingV2","v3"
            ];
        }

    }
}