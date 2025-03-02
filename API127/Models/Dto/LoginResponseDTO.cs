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

    public class LoginResponseDTO1
    {
        public UserDTO User { get; set; }
        public string Role {get;set;}
        public List<string> Roles {get;set;} = new List<string>();
        public string Token { get; set; }
        public bool Success { get;set;}
        public string? Message { get;set;}
    }

    public class UserDTO
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
    }
}
