using InventorAi_api.Interfaces;
using InventorAi_api.Models;
using InventorAi_api.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InventorAi_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILicenseRepo _licenseRepo;

        public AuthController(IAuthService authService, ILicenseRepo licenseRepo)
        {
            _authService = authService;
            _licenseRepo = licenseRepo;
        
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegistrationRequest dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginRequest dto)
        {
            var user = await _authService.LoginAsync(dto);

            // Check license
            var license = await _licenseRepo.GetStoreLicenseAsync(user.StoreId);
            if (license == null || !license.IsActive || license.ExpiryDate < DateTime.UtcNow)
                return Unauthorized("License invalid or expired");

            return Ok(user);
        }
        [Authorize]  // Any logged-in user
        [HttpGet("Profile")]
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
