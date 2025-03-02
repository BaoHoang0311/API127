using API127.Data;
using API127.Logging;
using API127.Models;
using API127.Models.Dto;
using API127.Repository.IRepository;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
namespace API127.Controllers.v2
{
    [ApiVersion("2.0")]
    [ApiController]
    // [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _serilog;
        private ApplicationDbContext _context;
        private IVillaRepositoryV2 _villa;
        private readonly ILogging _logger;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public VillaAPIController(ILogging _logger, ApplicationDbContext context,
            IMapper mapper,
            IVillaRepositoryV2 villa,
            ILogger<VillaAPIController> serilog,
            IHttpContextAccessor httpContextAccessor)
        {
            this._logger = _logger;
            _context = context;
            _mapper = mapper;
            _villa = villa;
            _serilog = serilog;
            this._apiResponse = new();
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaCreateDTO>>> GetDemo()
        {
            _apiResponse.Result = new List<int> { 1, 2, 3, 4 };
            _apiResponse.StatusCode = HttpStatusCode.OK;
            Response.Headers.Add("X-Total-Count", "{555list.Count.ToString()");
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
        [HttpGet("GetDemo1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaCreateDTO>>> GetDemo1()
        {
            _apiResponse.Result = new List<int> { 5,5,5,5 };
            _apiResponse.StatusCode = HttpStatusCode.OK;
            Response.Headers.Add("X-Total-Count", "{555list.Count.ToString()");
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
        //[HttpGet("/abc/{id:int}")] // https://localhost:5001/abc/5
        //[HttpGet("abc/{id:int}")] // https://localhost:5001/api/VillaApi/abc/5
        [HttpGet("{id:int}")] // https://localhost:5001/api/VillaAPI/5
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaCreateDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<VillaCreateDTO>> GetVillaByID(int id)
        {
            try
            {

                if (id == 0) return BadRequest();
                //var zz = VillaStore.villalist.FirstOrDefault(x => x.ID == id);
                //var zz = await _context.Villas.FirstOrDefaultAsync(x => x.Id == id);
                var zz = await _villa.GetAsync(x => x.Id == id);
                if (zz == null)
                {
                    //_logger.Log(" zz null");
                    return NotFound();
                }
                _apiResponse.Result = zz;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                //_logger.Log("getting error");
                return BadRequest(new APIResponse() { ErrorMessages = new List<string>() { ex.Message }, IsSuccess = false, StatusCode = HttpStatusCode.BadRequest });
            }
        }
    }
}
