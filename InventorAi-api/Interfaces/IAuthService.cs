using InventorAi_api.Models;

namespace InventorAi_api.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(UserLoginRequest loginRequest);
        Task<ServiceResponse<AuthResponse>> RegisterAsync(UserRegistrationRequest registrationRequest);
        Task<AuthResponse> RefreshTokenAsync(string refreshtoken);
    }
}
