using API127.Data;
using API127.Logging;
using API127.Models;
using API127.Models.Dto;
using API127.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API127.Models;

namespace API127.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        private ApplicationDbContext _context;
        private IVillaNumberRepository _villanumber;
        private readonly ILogging logger;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;
        public VillaNumberAPIController(ApplicationDbContext context,
            IVillaNumberRepository villanumber,
             ILogging _logger,
             IMapper mapper)
        {
            _context = context;
            logger = _logger;
            _apiResponse =  new APIResponse();
            _villanumber = villanumber;
            _mapper = mapper;
        }

        [HttpGet(Name = "abcD")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaNumberDTO>>> GetVillasNumber()
        {
            try
            {
                var villanumber = await _villanumber.GetAllAsync(includeProperties: "Villa");
                var villaNumberDTO = _mapper.Map<List<VillaNumberDTO>>(villanumber);
                _apiResponse.Result = villaNumberDTO;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                logger.Log("getting error", "");
                _apiResponse.Result = "";
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.Message };
                return BadRequest(_apiResponse);
            }
        }
        [HttpGet("{id}",Name = "GetVillasNumberByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaNumberDTO>>> GetVillasNumberByID(int id)
        {
            try
            {
                var villanumber = await _villanumber.GetAsync(x=>x.VillaNo == id,includeProperties: "Villa");
                var villaNumberDTO = _mapper.Map<VillaNumberDTO>(villanumber);
                _apiResponse.Result = villaNumberDTO;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                logger.Log("getting error", "");
                _apiResponse.Result = "";
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.Message };
                return BadRequest(_apiResponse);
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> CreateVillasNumber(VillaNumberCreateDTO model)
        {
            try
            {
                var villaNumber = _mapper.Map<VillaNumber>(model);
                await _villanumber.CreateAsync(villaNumber);

                _apiResponse.Result = villaNumber;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                logger.Log("getting error", "");
                _apiResponse.Result = "";
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.Message};
                return BadRequest(_apiResponse);
            }
        }
        //[Authorize(Roles = "admin")]
        [HttpPut("{id:int}", Name = "UpdateVillaNumber_HIHI")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.VillaNo)
                {
                    return BadRequest();
                }
                if (await _villanumber.GetAsync(u => u.VillaID == id) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa ID is Invalid!");
                    return BadRequest(ModelState);
                }
                VillaNumber model = _mapper.Map<VillaNumber>(updateDTO);

                await _villanumber.UpdateAsync(model);

                _apiResponse.Result = model;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                logger.Log("getting error", "");
                _apiResponse.Result = "";
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.Message };
                return BadRequest(_apiResponse);
            }
        }
        [HttpDelete("{id:int}", Name = "DeleteVillaNumber_HIHI")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                if (await _villanumber.GetAsync(u => u.VillaID == id) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa ID is Invalid!");
                    return BadRequest(ModelState);
                }
                VillaNumber model = await _villanumber.GetAsync(u => u.VillaID == id);

                await _villanumber.RemoveAsync(model);

                _apiResponse.Result = model;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                logger.Log("getting error", "");
                _apiResponse.Result = "";
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.Message };
                return BadRequest(_apiResponse);
            }
        }

    }
}
