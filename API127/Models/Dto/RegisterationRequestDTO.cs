namespace API127.Models.Dto
{
    public class RegisterationRequestDTO
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }= new List<string>();
        public string? Role { get; set; }
    }
}
