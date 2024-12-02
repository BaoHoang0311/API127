using API127.Models;
using API127.Models.Dto;

namespace API127.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser (string username);
        Task<LoginResponseDTO> Login (LoginRequestDTO loginRequestDTO);
        Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO);
        bool ValidateCredentials(string username, string password);
    }
}
