using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using System.Net.Http;

namespace MagicVilla_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string villaUrl;

        public VillaService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public async Task<T> CreateAsync<T>(VillaCreateDTO dto, string token)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = villaUrl + "/api/villaAPI",
                //Token = token
            });
        }
        public async Task<T> DeleteAsync<T>(int id, string token)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villaUrl + "/api/villaAPI/" + id,
                //Token = token
            });
        }

        public async Task<T> GetAllAsync<T>(string token)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                //Url = villaUrl + "/api/v1/villaAPI",
                Url = villaUrl + "/api/villaAPI",
                Token = token
            });
        }
        public async Task<T> GetAsync<T>(int id, string token)
        {
            var data = await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/villaAPI/" + id,
                Token = token
            });
            return data;
        }
        public async Task<T> UpdateAsync<T>(VillaUpdateDTO dto, string token)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = villaUrl + "/api/villaAPI/" + dto.Id,
                Token = token
            }) ;
        }
    }
}
