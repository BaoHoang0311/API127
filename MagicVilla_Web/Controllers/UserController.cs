using API127.Models;
using API127.Models.Dto;
using API127.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly APIResponse _response;
        public UserController(IUserRepository userRepository)
        {
            _userRepo = userRepository;
            this._response = new();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO obj)
        {
            var loginResponse = await _userRepo.Login(obj);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO obj)
        {
            bool ifUserUnique = _userRepo.IsUniqueUser(obj.UserName);
            if (ifUserUnique)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
            }
            var loginResponse = await _userRepo.Register(obj);
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }
    }
}
