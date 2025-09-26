using InventorAi_api.Data;
using InventorAi_api.EntityModels;
using InventorAi_api.Helpers;
using InventorAi_api.Interfaces;
using InventorAi_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InventorAi_api.Services
{
    public class AuthService : IAuthService
    {
        private readonly InventorAiContext _context;
        private readonly IConfiguration _config;
        public AuthService(InventorAiContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<AuthResponse> LoginAsync(UserLoginRequest loginRequest)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.License)
                .FirstOrDefaultAsync(u => u.Username == loginRequest.Username);
            if (user == null || PasswordHasher.VerifyPasswordHash(loginRequest.Password ?? "", user.PasswordHash, user.PasswordSalt))
                throw new Exception("Invalid username or password");
            if (!user.IsActive) throw new Exception("User is inactive");
            if (user.License == null || !user.License.IsActive || user.License.ExpiryDate < DateTime.UtcNow)
                throw new Exception("License expired or incative");

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
                LicenseKey = user.License.LicenseKey,
                Role = user.Role?.RoleName,
                Token = jwttoken,
                UserId= user.UserId,
            };
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshtoken)
        {
            var tokenEntry = await _context.RefreshTokens
                .Include(x => x.User).
                ThenInclude(x => x.Role).
                Include(x => x.User.License)
                .FirstOrDefaultAsync(rt => rt.Token == refreshtoken && !rt.IsRevoked);
            if (tokenEntry == null || tokenEntry.ExpiryDate < DateTime.UtcNow)
                throw new Exception("Invalid or expired token");
            tokenEntry.IsRevoked = true;

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
                LicenseKey = user.License.LicenseKey
            };

        }

        public async Task<AuthResponse> RegisterAsync(UserRegistrationRequest registrationRequest)
        {
            if (await _context.Users.AnyAsync(u => u.Username == registrationRequest.Username))
                throw new Exception("user already exists");
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
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var license = new License
            {
                ExpiryDate = DateTime.UtcNow.AddDays(30),
                LicenseKey = Guid.NewGuid().ToString(),
                IsActive = true,
                UserId = user.UserId
            };
            _context.Licenses.Add(license);
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

            return new AuthResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(double.Parse(_config["JwtSettings:ExpiresInMinutes"])),
                Role = (await _context.Roles.FindAsync(user.RoleId))?.RoleName,
                LicenseKey = license.LicenseKey
            };
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
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
