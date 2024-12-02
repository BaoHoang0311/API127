using API127.Data;
using API127.Logging;
using API127.Models;
using API127.Repository.IRepository;
using AutoMapper;
using API127.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Azure;

namespace API127.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _serilog;
        private readonly ILogging _logger;
        private readonly IMapper _mapper;
        private readonly APIResponse _response;
        private readonly IUserRepository _userRepo;
        public UserAPIController(ILogging _logger,
                ApplicationDbContext context,
                IMapper mapper,
                IVillaRepositoryV2 villa,
                ILogger<VillaAPIController> serilog,
                IUserRepository userService)
        {
            this._logger = _logger;
            _mapper = mapper;
            _serilog = serilog;
            this._response = new();
            _userRepo = userService;
        }
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> LogIn(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var res  = await _userRepo.Login(loginRequestDTO);
                if(!res.Success)
                {
                    _response.Result = res;
                    _response.StatusCode = HttpStatusCode.Unauthorized;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>(){ res.Message };
                    return Unauthorized (_response);
                }


                _serilog.LogInformation("Getting all villa hjaha");
                _logger.Log("Getting all villa hjaha", "");
                _response.Result = res;
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.Log("getting error", "");
                _response.Result = "";
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
                return BadRequest(_response);
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO model)
        {
            bool isExist = _userRepo.IsUniqueUser(model.UserName);
            if (isExist)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add( "Username already exists" );
                return BadRequest(_response);
            }

            var user = await _userRepo.Register(model);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error while registering");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
    }
}
