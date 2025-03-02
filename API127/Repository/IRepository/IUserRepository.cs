using API127.Models;
using API127.Models.Dto;

namespace API127.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser (string username);
        Task<LoginResponseDTO1> Login (LoginRequestDTO loginRequestDTO);
        // Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO);
        Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO);
        bool ValidateCredentials(string username, string password);
    }
}
