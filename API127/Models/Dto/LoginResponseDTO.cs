using API127.Models;

namespace API127.Models.Dto
{
    public class LoginResponseDTO
    {
        public LocalUser User { get; set; }
        public string Token { get; set; }
        public bool Success { get;set;}
        public string? Message { get;set;}
    }
}
