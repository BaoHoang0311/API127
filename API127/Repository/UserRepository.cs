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
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace API127.Repository
{
    public class ApiSettings
    {
        public string Secret { get; set; }
    }
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ApiSettings _appSettings;
        private string _secretkey;
        public UserRepository(ApplicationDbContext context,
            IConfiguration configuration, IOptions<ApiSettings> options, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _secretkey = configuration.GetValue<string>("ApiSettings:Secret");
            _appSettings = options.Value;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public bool ValidateCredentials(string username, string password)
        {
            return username.Equals("admin") && password.Equals("admin");
        }
        public bool IsUniqueUser(string username)
        {
            var isUserExists = _context.ApplicationUsers.Any(x => x.UserName.ToLower() == username.ToLower());
            return isUserExists;
        }

        public async Task<LoginResponseDTO1> Login(LoginRequestDTO loginRequestDTO)
        {

            try
            {
                var token1 = _secretkey;
                var token2 = _appSettings.Secret;

                // var user = await _context.LocalUsers.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower()
                //     && u.Password == loginRequestDTO.Password);

                var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

                bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

                if (user == null)
                {
                    return new LoginResponseDTO1()
                    {
                        Token = "",
                        User = null,
                        Success = false,
                        Message = "Wrong Password roi ku",
                    };
                }

                var roles = await _userManager.GetRolesAsync(user);

                #region CreateToken

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretkey);
                var tokenDes = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("ID",user.Id.ToString()),
                        new Claim("name",user.UserName.ToString()),
                        new Claim("hihi","Bao go hi hi"),
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(2),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                };
                foreach (var role in roles)
                {
                    tokenDes.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
                }
                var token = tokenHandler.CreateToken(tokenDes);
                #endregion
                
                var loginResponseDTO1 = new LoginResponseDTO1()
                {
                    Token = tokenHandler.WriteToken(token),
                    User = _mapper.Map<UserDTO>(user),
                    Role = roles.FirstOrDefault(),
                    Roles= roles.ToList(),
                    Success = true,
                };
                return loginResponseDTO1;
            }
            catch (Exception ex)
            {
                return new LoginResponseDTO1()
                {
                    Token = "",
                    User = null,
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        // public async Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO)
        // {
        //     var localUser = new LocalUser()
        //     {
        //         UserName = registerationRequestDTO.UserName,
        //         Password = registerationRequestDTO.Password,
        //         Name = registerationRequestDTO.Name,
        //         Role = registerationRequestDTO.Role,
        //     };

        //     _context.Add(localUser);
        //     await _context.SaveChangesAsync();
        //     localUser.Password = "";
        //     return localUser;
        // }

        public async Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            ApplicationUser user = new()
            {
                UserName = registerationRequestDTO.UserName,
                Email = registerationRequestDTO.UserName,
                NormalizedEmail = registerationRequestDTO.UserName.ToUpper(),
                Name = registerationRequestDTO.Name
            };

            try
            {
                var rs = await _userManager.CreateAsync(user, registerationRequestDTO.Password);
                if (rs.Succeeded)
                {
                    foreach (var role in registerationRequestDTO.Roles)
                    {
                        var isExist =  await _roleManager.RoleExistsAsync(role);
                        if (!isExist)
                        {
                            await _roleManager.CreateAsync(new IdentityRole(role));
                        }
                        await _userManager.AddToRoleAsync(user, role);
                    }
                }
                var userReturn = _context.ApplicationUsers.FirstOrDefault(x => x.UserName == registerationRequestDTO.UserName);
                var userDTO = _mapper.Map<UserDTO>(user);
                return userDTO;
            }
            catch (Exception ex)
            {
                return new UserDTO();
            }
        }
    }
}
