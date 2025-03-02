using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public VillaController(IVillaService villaService, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _villaService = villaService;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
        //[Authorize]
        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> list = new();
            var token = HttpContext.Session.GetString(SD.SessionToken);
            var response = await _villaService.GetAllAsync<APIResponse>(token);

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
        {
            if (ModelState.IsValid)
            {

                var response = await _villaService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                //if (response != null && response.IsSuccess)
                //{
                //    TempData["success"] = "Villa created successfully";
                //    return RedirectToAction(nameof(IndexVilla));
                //}

                if (response.StatusCode != System.Net.HttpStatusCode.BadRequest)
                {
                    TempData["success"] = "Villa created successfully";
                    return RedirectToAction(nameof(IndexVilla));
                }
                else
                {
                    TempData["success"] = $"Error encountered.{string.Join(",", response.ErrorMessages)}";
                    return View(model);
                }
            }
            TempData["error"] = "Error encountered. ";
            return View(model);
        }
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponse>(villaId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));

                return View(_mapper.Map<VillaUpdateDTO>(model));
            }
            return NotFound();
        }
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Villa updated successfully";
                var response = await _villaService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponse>(villaId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO model)
        {
            var response = await _villaService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa deleted successfully";
                return RedirectToAction(nameof(IndexVilla));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [HttpGet("Post-Data-Villa")]
        public async Task<IActionResult> PostDataVilla()
        {
            return View();
        }
        [HttpPost("Post-Data-Villa")]
        //[DisableFormValueModelBinding]
        [RequestSizeLimit(5L * 1024 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 5L * 1024 * 1024 * 1024)]
        public async Task<IActionResult> PostDataVilla2(VillaCreateDTO1 data)
        {
            try
            {
                var model = new VillaCreateDTO()
                {
                    Details = "1111",
                    ImageUrl = "31",
                    Sqft = 2,
                    Amenity = "23",
                    Name = "33333333333",
                    Occupancy = 533,
                    Rate = 333,
                };

                var response = await _villaService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));

                #region KO CẦN XEM Vì, File To và file có sẵn trong máy rồi PostVilla4
                //E:\API_127\API_127\MagicVilla_Web\wwwroot\200MB.pdf
                //string htmlFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "200MB.pdf");
                //var zzz = await ChunkFileAsync(htmlFilePath);
                #endregion

                #region PostVilla3 File Input là IFormFile (Quan trọng) Stream file then send to API
                object requestParams = null;
                requestParams = new { ImageUrl = "aaa" };
                using (MultipartFormDataContent content = new MultipartFormDataContent())
                {
                    if (requestParams != null)
                    {
                        foreach (var prop in requestParams.GetType().GetProperties())
                        {
                            content.Add(new StringContent(prop.GetValue(requestParams)?.ToString() ?? ""), prop.Name);
                        }
                    }

                    var streamContent = new StreamContent(data.files.OpenReadStream());
                    streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(data.files.ContentType);
                    streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "file",
                        FileName = data.files.FileName,
                    };
                    content.Add(streamContent);

                    var httpClient = new HttpClient();
                    var res = await httpClient.PostAsync($"https://localhost:5001/api/v1/VillaAPI/PostVilla3", content);
                    string responseData = await res.Content.ReadAsStringAsync();
                }
                #endregion

                #region work with IFormFile setting IFormFile trong program.cs là dc nhận hết to nhỏ (Ok) PostVilla5
                /*
                 * work with IFormFile nhưng chưa dùng dc stream
                 */
                //var target = new MemoryStream();
                //await data.files.CopyToAsync(target);
                //var binary = target.ToArray();
                //var responseUpload = await BasicUploadFile(binary, "5.zip", data.ImageUrl);
                #endregion

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa created successfully";
                    return RedirectToAction(nameof(IndexVilla));
                }
                TempData["error"] = "Error encountered.";
                return View(null);
            }
            catch (Exception ex)
            {
                return View(null);
            }
        }
        public async Task<int> BasicUploadFile(byte[] fileByte, string fileName, string ImageUrl)
        {
            try
            {
                var fileExtension = Path.GetExtension(fileName);
                string url = "https://localhost:5001/api/VillaAPI/PostVilla5";
                var requestParams = new { ImageUrl = ImageUrl };
                var result = await PostMediaAsync(url, fileByte, fileName,requestParams);
                return 200;
            }
            catch (Exception ex)
            {
                return 404;
            }

        }
        public async Task<string> PostMediaAsync(string url, byte[] fileData, string fileName, object requestParams = null)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient httpClient = new HttpClient(clientHandler);


                string contentType;
                new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);

                if (string.IsNullOrEmpty(contentType))
                {
                    contentType = "application/octet-stream";
                }

                using (MultipartFormDataContent content = new MultipartFormDataContent())
                {

                    if (requestParams != null)
                    {
                        foreach (var prop in requestParams.GetType().GetProperties())
                        {
                            content.Add(new StringContent(prop.GetValue(requestParams)?.ToString() ?? ""), prop.Name);
                        }
                    }

                    content.Add(new StreamContent(new MemoryStream(fileData))
                    {
                        Headers =
                        {
                            ContentLength = fileData.Length,
                            ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType)
                        }

                    }, "Files", fileName);

                    HttpResponseMessage response = await httpClient.PostAsync(url, content);

                    string responseData = await response.Content.ReadAsStringAsync();
                    return responseData;

                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        // Save File (file có sẵn trong máy rồi ) in to wwwroot then send to API => OK
        public async Task<bool> ChunkFileAsync(string htmlFilePath)
        {
            var client = new HttpClient();
            using (FileStream fs = new FileStream(htmlFilePath, FileMode.Open))
            {
                fs.Position = 0;
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);

                ByteArrayContent content = new ByteArrayContent(bytes);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                content.Headers.ContentLength = fs.Length;
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5001/api/VillaAPI/PostVilla4");
                request.Content = content;

                try
                {
                    HttpResponseMessage response = await client.SendAsync(request);
                    Console.WriteLine("Response status code: {0}", response.StatusCode);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: {0}", ex.Message);
                    return false;
                }
            }
        }
    }
}
