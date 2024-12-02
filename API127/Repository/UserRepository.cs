using API127.Data;
using API127.Models;
using API127.Repository.IRepository;
using API127.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace API127.Repository
{
    public class ApiSettings
    {
        public string Secret { get; set; }
    }
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ApiSettings _appSettings;
        private string _secretkey;
        public UserRepository(ApplicationDbContext context,
            IConfiguration configuration, IOptions<ApiSettings> options)
        {
            _context = context;
            _secretkey = configuration.GetValue<string>("ApiSettings:Secret");
            _appSettings = options.Value;
        }
        public bool ValidateCredentials(string username, string password)
        {
            return username.Equals("admin") && password.Equals("admin");
        }
        public bool IsUniqueUser(string username)
        {
            var isUserExists = _context.LocalUsers.Any(x => x.UserName == username);
            return isUserExists;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {

            try
            {
                var token1 = _secretkey;
                var token2 = _appSettings.Secret;

                var user = await _context.LocalUsers.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower()
                    && u.Password == loginRequestDTO.Password);

                if (user == null)
                {
                    return new LoginResponseDTO()
                    {
                        Token = "",
                        User = null,
                        Success = false,
                        Message = "Wrong Password roi ku",
                    };
                }


                #region CreateToken

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretkey);

                var tokenDes = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim("ID",user.UserId.ToString()),
                    new Claim("name",user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("hihi","Bao go hi hi"),
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(2),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                };
                var token = tokenHandler.CreateToken(tokenDes);

                #endregion
                LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
                {
                    Token = tokenHandler.WriteToken(token),
                    User = user,
                    Success = true,
                };
                return loginResponseDTO;
            }
            catch (Exception ex)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null,
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            var localUser = new LocalUser()
            {
                UserName = registerationRequestDTO.UserName,
                Password = registerationRequestDTO.Password,
                Name = registerationRequestDTO.Name,
                Role = registerationRequestDTO.Role,
            };

            _context.Add(localUser);
            await _context.SaveChangesAsync();
            localUser.Password = "";
            return localUser;
        }
    }
}
