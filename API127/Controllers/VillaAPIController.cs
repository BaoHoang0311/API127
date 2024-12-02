using API127.Data;
using API127.Logging;
using API127.Models;
using API127.Models.Dto;
using API127.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.IO;
namespace API127.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _serilog;
        private ApplicationDbContext _context;
        //private IVillaRepository _villa;
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
        [HttpGet(Name = "abc")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaCreateDTO>>> GetVillas()
        {
            try
            {
                var aaa = User.Identity.Name;
                var id = _httpContextAccessor.HttpContext?.User.FindFirstValue("ID");
                var loginType = _httpContextAccessor.HttpContext?.User.FindFirstValue("LoginType");
                _serilog.LogInformation("Getting all villa hjaha");
                _logger.Log("Getting all villa hjaha _logger", "");
                //return Ok(VillaStore.villalist);
                var villa = await _villa.GetAllAsync();
                var villaDTO = _mapper.Map<List<VillaDTO>>(villa);
                _apiResponse.Result = villaDTO;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess =true;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                //logger.LogError("getting error");
                //_logger.Log("getting error", "");
                _apiResponse.Result = "";
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.Message };
                return BadRequest(_apiResponse);
            }
        }

        //[HttpGet("/abc/{id:int}")] // https://localhost:5001/abc/5
        //[HttpGet("abc/{id:int}")] // https://localhost:5001/api/VillaApi/abc/5
        [HttpGet("{id:int}")] // https://localhost:5001/api/VillaAPI/5
        [Authorize(Roles ="1")]
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

        // https://localhost:44300/api/VillaAPI/getbyname/abc {}bat buoc
        //[HttpGet("getbyname/{name}")]
        [HttpGet("getbyname/name")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaCreateDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<VillaCreateDTO>> GetVillaByName(string? Name)
        {
            try
            {
                string? a ;
                string b;
                var zz = await _context.Villas.FirstOrDefaultAsync(x => x.Name == Name);
                if (zz == null) return NotFound(new { message = "khong tim ra cai name do", error = "Not found"});
                return Ok(zz);
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse() { ErrorMessages = new List<string>() { ex.Message }, IsSuccess = false, StatusCode = HttpStatusCode.BadRequest });
            }
        }

        // https://localhost:44300/api/VillaAPI
        [HttpPost(Name = "abcde")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaCreateDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaCreateDTO>> PostVilla([FromBody] VillaCreateDTO villaDTO)
        {
            try
            {
                // [ApiController] khỏi cần modelstate 
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}

                var chkExist = await _villa.GetAsync(x => x.Name.ToLower() == villaDTO.Name);
                //if (chkExist != null)
                //{
                //    ModelState.AddModelError("", "Villa already Exiist !");
                //    return BadRequest(ModelState);
                //}

                if (villaDTO == null) return BadRequest();

                //var villa =  new Villa()
                //{
                //    Name =  villaDTO.Name,
                //    Details = villaDTO.Details,
                //};

                var villa = _mapper.Map<Villa>(villaDTO);
                villa.UpdatedDate = DateTime.Now;
                await _villa.CreateAsync(villa);
                // header: location có cái link~
                //return CreatedAtRoute("abcde", new { id = villa.Id }, villa);
                return Ok(new APIResponse() { ErrorMessages = new List<string>() { "" }, IsSuccess =true ,Result = new { villa.Id }, StatusCode = HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse() { ErrorMessages = new List<string>() { ex.Message }, IsSuccess = false, StatusCode = HttpStatusCode.BadRequest });
            }
        }

        // https://localhost:44300/api/VillaAPI/PostVilla2 
        [HttpPost("PostVilla2")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaCreateDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaCreateDTO> PostVilla2([FromBody] VillaCreateDTO villaDTO)
        {
            try
            {

                return Ok(new { message =  "okkkkk" , thongtin = "may cut" });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        ///     Work with File to IFormFile MVC -> API nhận stream file
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("PostVilla3")]
        [RequestSizeLimit(5L * 1024 * 1024 * 1024)]
        [MultipartFormData]
        public async Task<ActionResult<VillaCreateDTO>> PostVilla3([FromForm] VillaCreateAPIDTO2 data)
        {
            try
            {

                #region File to trực tiếp vào API , đã làm bên upload FileSLN.sln
                var fileUploadSummary = await _villa.UploadFileAsync(HttpContext.Request.Body, Request.ContentType);
                #endregion

                return Ok(new { message = "okkkkk", thongtin = "may cut" });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        /// <summary>
        ///     Chunk File to nhưng file đã nằm trong máy rồi (MVC->API)
        /// </summary>
        /// <returns></returns>
        [HttpPost("PostVilla4")]
        public async Task<ActionResult<VillaCreateDTO>> PostVilla4()
        {
            try
            {
                var zzz = HttpContext.Request.Body;

                #region Chunkfile file to nhung file nằm trong máy rồi
                byte[] bytes;

                // To get bytes, use this code (await is required, you missed it)
                // But I recommend working with Stream in order not to load the server's RAM.
                using (var memoryStream = new MemoryStream())
                {
                    await Request.BodyReader.CopyToAsync(memoryStream).ConfigureAwait(false);
                    bytes = memoryStream.ToArray();
                }
                string path = Path.Combine(Directory.GetCurrentDirectory(), "File");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Post File xong Download vào folder File
                var folderFile = Path.Combine(Directory.GetCurrentDirectory(), "Files", "200MB.pdf");
                // Ensure the folder exists
                //Write the bytes to the file asynchronously
                await System.IO.File.WriteAllBytesAsync(folderFile, bytes);
                #endregion

                return Ok(new { message = "okkkkk", thongtin = "may cut" });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        ///     IformFIle chưa dùng dc stream, Work with File nhỏ IFormFile (MVC->API)
        /// </summary>
        /// <returns></returns>
        [HttpPost("PostVilla5")]
        public async Task<ActionResult<VillaCreateDTO>> PostVilla5([FromForm] VillaCreateAPIDTO1 data)
        {
            try
            {
                #region Work with File nhỏ IFormFile
                var filePath = $"E:\\API_127\\API_127\\API127\\Files\\{data.ImageUrl}.pdf";
                using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await data.Files.CopyToAsync(fileStream);
                }
                #endregion

                return Ok(new { message = "okkkkk", thongtin = "may cut" });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}", Name = "Hard Delete Villa")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                //var zz = _context.Villas.FirstOrDefault(x => x.Id == id);
                var data = await _villa.GetAsync(u => u.Id == id);
                await _villa.RemoveAsync(data);
                _apiResponse.Result = "";
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);
            }
            catch(Exception ex)
            {
                //_logger.Log("getting error");
                return BadRequest(new APIResponse() { ErrorMessages = new List<string>() { ex.Message }, IsSuccess = false, StatusCode = HttpStatusCode.BadRequest });
            }
        }

        [HttpPut("{id}", Name = "UpdatePutData")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status101SwitchingProtocols)]
        public async Task<IActionResult> UpdatePutData(int id, [FromBody] VillaUpdateDTO villaDTO)
        {
            // nếu string? Hello_Hi  thì không cần đủ properties of object {
            //            "sqft": 5,
            //             "occupancy": 5
            //}

            // nếu string Hello_Hi  thì phải đủ các properties of object
            try
            {
                if (id != villaDTO.Id)
                {
                    return NotFound("????");
                }

                var villa = _context.Villas.AsNoTracking().FirstOrDefault(x => x.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }

                Villa model = _mapper.Map<Villa>(villaDTO);
                model.CreatedDate = villa.CreatedDate;
                await _villa.UpdateAsync(model);
                _apiResponse.Result = "";
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);

            }
            catch(Exception ex)
            {
                _apiResponse.Result = "";
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = true;
                _apiResponse.ErrorMessages = new List<string>() {ex.InnerException.ToString() };
                return BadRequest(_apiResponse);
            }
        }

        [HttpPatch("{id}", Name = "UpdatePartialData")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status101SwitchingProtocols)]
        public async Task<IActionResult> UpdatePartialData(int id, [FromBody] VillaUpdateDTO villaDTO)
        {
            try
            {
                if (id != villaDTO.Id)
                {
                    return NotFound("????");
                }

                var villa = _context.Villas.AsNoTracking().FirstOrDefault(x => x.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }

                Villa model = _mapper.Map<Villa>(villaDTO);
                model.CreatedDate = villa.CreatedDate;
                await _villa.UpdateAsync(model);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }
    }

    internal class ChunkMetaData
    {
        public string UploadUid { get; set; }
        public string FileName { get; set; }
        public int ChunkIndex { get; set; }
        public int TotalChunks { get; set; }
        public long TotalFileSize { get; set; }
        public string ContentType { get; set; }
    }
}
