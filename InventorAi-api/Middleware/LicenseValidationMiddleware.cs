using InventorAi_api.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InventorAi_api.Middleware
{
    public class LicenseValidationMiddleware
    {
        private readonly RequestDelegate _next;
        public LicenseValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, ILicenseService licenseService)
        {
            // Skip if request is NOT protected (like login/register)
            if (context.User.Identity?.IsAuthenticated == true)
            {
                foreach (var claim in context.User.Claims)
                {
                    Console.WriteLine($"{claim.Type} = {claim.Value}");
                }
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    var license = await licenseService.GetUserLicenseAsync(userId);

                    if (license == null || !license.IsActive || license.ExpiryDate < DateTime.UtcNow)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("License invalid or expired.");
                        return; // Stop pipeline here
                    }
                }
            }

            // Continue to next middleware/controller
            await _next(context);
        }
    }
}
