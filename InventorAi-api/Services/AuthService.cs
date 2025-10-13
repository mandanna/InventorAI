using AutoMapper;
using InventorAi_api.Data;
using InventorAi_api.EntityModels;
using InventorAi_api.Helpers;
using InventorAi_api.Interfaces;
using InventorAi_api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using InventorAi_api;
using static InventorAi_api.Models.ApiResponse;

namespace InventorAi_api.Services
{
    public class AuthService : IAuthService
    {
        private readonly InventorAiContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public AuthService(InventorAiContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
        }
        public async Task<AuthResponse> LoginAsync(UserLoginRequest loginRequest)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Store)
                .ThenInclude(s => s.Licenses)
                .FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

            var activeLicense = user.Store?.Licenses.FirstOrDefault(l => l.IsActive);

            if (user == null || !PasswordHasher.VerifyPasswordHash(loginRequest.Password ?? "", user.PasswordHash, user.PasswordSalt))
                throw new Exception("Invalid username or password");
            if (!user.IsActive) throw new Exception("User is inactive");
            if (activeLicense == null || activeLicense.ExpiryDate < DateTime.UtcNow)
                throw new Exception("License expired or inactive");


            var jwttoken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();
            _context.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.UserId,
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(int.Parse(_config["JwtSettings:RefreshTokenExpiryDays"])),
                CreatedDate = DateTime.UtcNow,
                IsRevoked = false,
            });
            await _context.SaveChangesAsync();
            return new AuthResponse
            {
                ExpiresAt = DateTime.UtcNow.AddMinutes(double.Parse(_config["JwtSettings:ExpiresInMinutes"])),
                RefreshToken = refreshToken,
                LicenseKey = activeLicense.LicenseKey,
                Role = user.Role?.RoleName,
                Token = jwttoken,
                UserId = user.UserId,
                StoreId=user.StoreId??0
            };
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshtoken)
        {
            var tokenEntry = await _context.RefreshTokens
                .Include(x => x.User)
                .ThenInclude(x => x.Role)
                .Include(x => x.User)
                .ThenInclude(x => x.Store)
                .ThenInclude(x => x.Licenses)
                .FirstOrDefaultAsync(rt => rt.Token == refreshtoken && !rt.IsRevoked);
            if (tokenEntry == null || tokenEntry.ExpiryDate < DateTime.UtcNow)
                throw new Exception("Invalid or expired token");
            tokenEntry.IsRevoked = true;

            var activeLicense = tokenEntry.User.Store?.Licenses.FirstOrDefault(l => l.IsActive);

            //issue new token
            var user = tokenEntry.User;
            var token = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            _context.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.UserId,
                ExpiryDate = DateTime.UtcNow.AddMinutes(double.Parse(_config["JwtSettings:ExpiresInMinutes"])),
                IsRevoked = false,
                Token = newRefreshToken,
                CreatedDate = DateTime.UtcNow,
            });

            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                Token = token,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(double.Parse(_config["JwtSettings:ExpiresInMinutes"])),
                Role = user.Role.RoleName,
                LicenseKey = activeLicense.LicenseKey
            };

        }

        public async Task<ServiceResponse<AuthResponse>> RegisterAsync(UserRegistrationRequest registrationRequest)
        {
            var response = new ServiceResponse<AuthResponse>();

            try
            {
                if (await _context.Users.AnyAsync(u => u.Email == registrationRequest.Email && u.StoreId == registrationRequest.StoreId))
                {

                    response.Success = false;
                    response.Error = new ApiError
                    {
                        Code = "USER_ALREADY_EXISTS",
                        Message = "A user with this email already exists.",
                        Field = "email"
                    };
                    return response;
                    
                }
                PasswordHasher.CreatePasswordHash(registrationRequest.Password, out var hash, out var salt);
                var user = new User
                {
                    PasswordSalt = salt,
                    PasswordHash = hash,
                    Email = registrationRequest.Email,
                    Phone = registrationRequest.Phonenumber,
                    Username = registrationRequest.Username,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    RoleId = registrationRequest.RoleId,
                    StoreId = registrationRequest.StoreId,
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var store = await _context.Stores.Include(s => s.Licenses).FirstOrDefaultAsync(s => s.StoreId == registrationRequest.StoreId);
                var license = store.Licenses.FirstOrDefault(i => i.IsActive);

                if (license == null)
                {
                    // Create license for the store if it doesn't exist
                    license = new License
                    {
                        ExpiryDate = DateTime.UtcNow.AddDays(30),
                        LicenseKey = Guid.NewGuid().ToString(),
                        IsActive = true,
                        StoreId = user?.StoreId ?? 0
                    };
                    _context.Licenses.Add(license);
                    await _context.SaveChangesAsync();
                }
                await _context.SaveChangesAsync();
                var token = GenerateJwtToken(user);
                var refreshToken = GenerateRefreshToken();

                _context.RefreshTokens.Add(new RefreshToken
                {
                    UserId = user.UserId,
                    Token = refreshToken,
                    ExpiryDate = DateTime.UtcNow.AddDays(int.Parse(_config["JwtSettings:RefreshTokenExpiryDays"])),
                    IsRevoked = false,
                    CreatedDate = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();


                response.Success = true;
                response.Data = new AuthResponse
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(double.Parse(_config["JwtSettings:ExpiresInMinutes"])),
                    Role = (await _context.Roles.FindAsync(user.RoleId))?.RoleName,
                    LicenseKey = license.LicenseKey
                };
                return response;
            }
            catch (Exception ex)
            {

                response.Success = false;
                response.Error = new ApiError
                {
                    Code = "SERVER_ERROR",
                    Message = ex.Message
                };
                return response;
            }
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
             new Claim("StoreId", user.StoreId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, (user.Role != null ? user.Role.RoleName : "User"))
        };

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["JwtSettings:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateRefreshToken()
        {
            var randomnumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomnumber);
            return Convert.ToBase64String(randomnumber);
        }
    }
}
