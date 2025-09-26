using InventorAi_api.Interfaces;
using InventorAi_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InventorAi_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILicenseService _licenseService;

        public AuthController(IAuthService authService, ILicenseService licenseService)
        {
            _authService = authService;
            _licenseService = licenseService;
        
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationRequest dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest dto)
        {
            var user = await _authService.LoginAsync(dto);

            // Check license
            var license = await _licenseService.GetUserLicenseAsync(user.UserId);
            if (license == null || !license.IsActive || license.ExpiryDate < DateTime.UtcNow)
                return Unauthorized("License invalid or expired");

            return Ok(user);
        }
        [Authorize]  // Any logged-in user
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            return Ok(new { Username = name });
        }

        [Authorize(Roles = "Admin,Cashier")]  // Only Admin
        [HttpGet("admin-data")]
        public IActionResult AdminData()
        {
            return Ok("Secret admin info");
        }
    }
}
